using CKas.Contracts.PresentCards;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using POS_display.Exceptions;
using POS_display.Models;
using POS_display.Models.DeliveryService;
using POS_display.Models.DeliveryService.Shipment;
using POS_display.Models.E1Gateway.Order;
using POS_display.Models.Pos;
using POS_display.Models.PresentCard;
using POS_display.Repository.HomeMode;
using POS_display.Repository.NBO;
using POS_display.Repository.Partners;
using POS_display.Repository.Payment;
using POS_display.Repository.PersonalPharmacist;
using POS_display.Repository.Pos;
using POS_display.Utils;
using POS_display.Utils.Logging;
using POS_display.wpf.Model;
using POS_display.wpf.ViewModel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using System.Windows.Forms;
using Tamroutilities.Client;
using TamroUtilities.HL7.Models;
using static POS_display.Enumerator;

namespace POS_display.Presenters
{
    public class PaymentPresenter
    {
        private const string intercationType = "bought";
        private const string itemIDType = "RetailNo";
        private Views.Ipayment _view;
        private Models.payment _model;
        private readonly PosRepository _posRepository;
        private readonly PersonalPharmacistRepository _personalPharmacistRepository;
        private readonly PaymentRepository _paymentRepository;
        private readonly NBORepository _nboRepository;
        private readonly HomeModeRepository _homeModeRepository;
        private readonly PartnerRepository _partnerRepository;
        private readonly ITamroClient _tamroClient;
        private const string FiscalBankCardValue = "C";
        private const string FiscalCredit1Value = "I";
        private const string FiscalCredit3Value = "K";
        private const decimal TamroSupplierId = 10000000281;
        private const string E1OrderClientGUID = "5EBDEBAF-24BC-4319-B8E0-C006FC6F1A66";
        private const long WoltPaymentMethodId = 10000121170;
        public const decimal GiftCardPaymentId = 4098;
        public const decimal SavasChequePaymentId = 6191;
        public const decimal GoodPresentPaymentId = 6452;
        public const decimal RoundingItemId = 10000121707;
        public readonly Guid _instanceGuid = Guid.Empty;
        public bool _completed = false;

        public PaymentPresenter(Views.Ipayment view, ITamroClient tamroClient)
        {
            _view = view ?? throw new ArgumentNullException();
            InitializeEvents();
            _paymentRepository = new PaymentRepository();
            _posRepository = new PosRepository();
            _personalPharmacistRepository = new PersonalPharmacistRepository();
            _nboRepository = new NBORepository();
            _homeModeRepository = new HomeModeRepository();
            _partnerRepository = new PartnerRepository();
            _tamroClient = tamroClient ?? throw new ArgumentNullException();
            _instanceGuid = Guid.NewGuid();
        }

        public async Task RefreshGrid()
        {
            await VoidPayment();
            var lst = await DB.POS.getPayment<Models.payment>();
            lst.Where(l => l.fiscal == "I").ToList().ForEach(l => l.Buyer = l.id.ToString());
            var bankCard = lst.FirstOrDefault(w => w.id == 63833);
            bankCard.enabled = Session.Devices.cash_only == 0;
            var loyalty = lst.FirstOrDefault(f => f.id == 10000109134);
            var loyaltyAmount = _view.PoshItem?.CRMItem?.AcceptedPaymentResponse?.Data.TotalCreditPoints - _view.PoshItem?.CRMItem?.AcceptedPaymentResponse?.Data.CreditPointsOfThisTransaction ?? 0;
            loyalty.name = loyalty.name.Replace("{1}", Math.Round(loyaltyAmount, 2).ToString());
            loyalty.enabled = !string.IsNullOrWhiteSpace(_view.PoshItem?.CRMItem?.Account?.CardNumber);
            loyalty.code = _view.PoshItem?.CRMItem?.Account?.CardNumber;
            var insurance = lst.FirstOrDefault(f => f.id == 10000109105);
            insurance.amount = Math.Abs(_view.PoshItem?.ChequeInsuranceSum ?? 0);
            insurance.enabled = insurance.amount > 0;
            var moq = lst.FirstOrDefault(f => f.id == 10000112385);
            moq.enabled = (moq.enabled && !string.IsNullOrWhiteSpace(Session.Devices.header4));
            if (_view.PoshItem.PosdItems.Any(a => a.Type == "ADVANCEPAYMENT"))
            {
                foreach (var p in lst.Where(w => w.id != 63833 && w.id != 0))
                {
                    p.enabled = false;
                }
            }

            if (Session.WoltMode)
            {
                foreach (var paymentMethod in lst)
                {
                    if (paymentMethod.id != WoltPaymentMethodId)
                        paymentMethod.enabled = false;
                }

                _view.CardButton.Enabled = false;
                _view.CashButton.Enabled = false;
            }
            else 
            {
                foreach (var paymentMethod in lst)
                {
                    if (paymentMethod.id == WoltPaymentMethodId)
                        paymentMethod.enabled = false;
                }
            }

            _view.payment_list = new List<Models.payment>(lst.Where(w => w.enabled == true).OrderBy(o => o.rank).ToList());
        }

        public async Task Pay()
        {
            try
            {
                if (_completed) return;
                if (Session.PaymentInProgress) return;

                Session.PaymentInProgress = true;
                await VoidPayment();
                await Validate();
                await PayCrm();
                await PayInsurance();
                await PayPresentCard();
                await PerformFMD();
                await PayFiscal();
                await PayHomeMode();
                await FixCheckNumber();
                await ShowFeedback();
                await PayDB();
                await PerformPersonalPharmacist();
                await PerformCompletion();
            }
            catch (NullReferenceException ex)
            {
                Serilogger.GetLogger().Error($"[Pay null]: Stack trace: {ex.StackTrace}");
                await VoidPayment();
                throw;
            }
            catch (Exception ex)
            {
                await VoidPayment();
                throw ex;
            }
            finally 
            {
                Session.PaymentInProgress = false;
            }
        }

        public async Task VoidPayment()
        {
            var posPayments = await _posRepository.GetPosPayment(_view.PoshItem.Id);
            foreach (var posPayment in posPayments)
            {
                if (posPayment.PaymentType == PosPaymentType.BENUM)
                    await VoidBenuM();

                if (posPayment.PaymentType == PosPaymentType.CRM)
                    await VoidCrm();

                if (posPayment.PaymentType == PosPaymentType.PRESENTCARD)
                    await VoidPresentCard(posPayment.Code);

                if (posPayment.PaymentType == PosPaymentType.ADVANCEPAYMENT)
                    await VoidAdvancePayment(_view.PoshItem?.PosdItems?.FirstOrDefault(e => e.barcodename == posPayment.Code)?.PresentCardId ?? 0m);

                await _posRepository.VoidPosPayment(posPayment);
            }
        }

        private async Task PayDB()
        {
            if (_view.PoshItem.PosdItems.Any(a => a.Type == "ADVANCEPAYMENT"))
                await PayAdvancePayment();
            else
                await PayKAS();
        }

        private async Task FixCheckNumber()
        {

            if (_view.lastCheckNo == null)
                _view.lastCheckNo = "5555555";
            if (Session.Devices.fiscal == 0)
                _view.lastCheckNo = "6666666";
            await Task.FromResult(0);
        }

        private async Task ShowFeedback()
        {
            var requestModel = new Models.FeedbackTerminal.FeedbackStartRequest
            {
                GuestNumber = _view?.PoshItem?.CRMItem?.Account?.CardNumber ?? "",
                ReceiptId = _view.PoshItem.CheckNo
            };
            Session.FeedbackTerminal.ExecuteAction(Models.FeedbackTerminal.RequestName.FeedbackStart, requestModel);
            await Task.FromResult(0);
        }

        private async Task Validate()
        {
            //apsauga nuo nepilnai supildytu ereceptu
            if (_view.PoshItem.CountStrikedOut > 0)
                throw new Exception("Yra neteisingai užpildytų eilučių kvite(perbrauktos eilutės)!\nŠias eilutes reikėtų panaikinti ir mėginti pildyti iš naujo. ");
            // apsauga nuo per dideles arba nulines sumos
            if (_view.RestSum > 499.99M || _view.DebtorSum < 0)
                throw new Exception("Neteisingai įvesta mokama suma!!!");

            if (!_view.payment_list.Where(pl => pl.fiscal_rank >= 0).Any(pl => pl.amount > 0) && _view.PaySum > 0) 
            {
                Serilogger.GetLogger().Information($"Apmokėjimo sumos negali būti nulines, patikrinkite! KAS ID: {Session.SystemData.kas_client_id}, Posh id: {_view.PoshItem.Id}");
                throw new Exception("Apmokėjimo sumos negali būti nulines, patikrinkite!");
            }

            if (_view.PoshItem.PosdItems.Any(e => (
                e.note2?.IndexOf("narc", StringComparison.InvariantCultureIgnoreCase) >= 0 ||
                e.note2?.IndexOf("psyc", StringComparison.InvariantCultureIgnoreCase) >= 0) && 
                !HasRecipe(e)) &&
                Session.getParam("SELLNARCOTIC", "ALLOW") == "0")
            {
                throw new PrescriptionProductInBasketException("Parduodate narkotinę arba psichotropinę prekę be recepto!");

            }

            if (_view.PoshItem.PosdItems.Any(e => e.gr4.StartsWith("rx", StringComparison.InvariantCultureIgnoreCase) && !HasRecipe(e)) && 
                Session.getParam("SELLNARCOTIC", "ALLOW") == "0")
            {
                if (!helpers.alert(Enumerator.alert.warning, "Krepšelyje yra receptinis vaistas kuris bandomas parduoti be recepto!\n" +
                    "Ar tikrai norite tęsti pardavimą?", true))
                {
                    throw new PrescriptionProductInBasketException("Ištrinkite receptinę prekę, kuri yra be recepto ir tęskite pardavimą");
                }
                else
                    Serilogger.GetLogger().Information($"Pardavimas be recepto!. Prekės: {_view.PoshItem.BuildLogs()}");

            }

            if (_view.PoshItem.PosdItems.Any(e => (e.fmd_required && !e.IsValidToSellFMD))
                && Session.getParam("FMD", "ALLOWSELLINVALID") == "0") 
            { 
                throw new Exception("Krepšelyje yra prekių neatitinkančių FMD reikalavimus!");
            }

            await Task.FromResult(0);
        }

        private async Task PayKAS()
        {
            await DB.Pay.update_posh(_view.lastCheckNo, _view.PoshItem.Id);
            await DB.Pay.delete_posd(_view.PoshItem.Id);
            if (_view.lastCheckNo != "" && _view.lastCheckNo != "0")
                await DB.Pay.update_recipe_checkno(_view.lastCheckNo, _view.PoshItem.Id);
            foreach (var payment in _view.payment_list.Where(pl => pl.id == 63833 && pl.amount > 0))// banko kortele
                await DB.POS.create_posd_service(_view.PoshItem.Id, payment.id.ToString(), 1, payment.amount);
            foreach (var payment in _view.payment_list.Where(pl => pl.fiscal == "I" && pl.amount > 0))//cekiai
            {
                decimal cheque_sum = payment.amount;
                if (cheque_sum > _view.PaySum)
                    cheque_sum = _view.PaySum;
                await DB.POS.create_posd_service(_view.PoshItem.Id, "10000100596", 1, cheque_sum);
                await DB.Pay.insert_cheque_presents(_view.PoshItem.Id, payment.Buyer, payment.amount, cheque_sum, payment.code);
                await CreatePosPayment(_view.PoshItem.Id, PosPaymentType.KAS_CHEQUE, null, "10000100596", cheque_sum);
            }
            if (_view.PoshItem.InsuranceItem != null && _view.PoshItem.InsuranceItem.CardSessionId != "")//draudimai
            {
                var insuranceSum = _view.PoshItem.PosdItems.Where(pd => pd.apply_insurance == 1).Sum(pd => pd.cheque_sum_insurance);
                await DB.POS.create_posd_service(_view.PoshItem.Id, "10000109105", 1, -1 * insuranceSum);
                await CreatePosPayment(_view.PoshItem.Id, PosPaymentType.KAS_INSURANCE, null, "10000109105", insuranceSum);
            }
            foreach (var payment in _view.payment_list.Where(pl => pl.id == WoltPaymentMethodId && pl.amount > 0)) // WOLT
            {
                await DB.POS.create_posd_service(_view.PoshItem.Id, payment.id.ToString(), 1, payment.amount);
                await CreatePosPayment(_view.PoshItem.Id, PosPaymentType.WOLT, null, payment.id.ToString(), payment.amount);
            }
            foreach (var payment in _view.payment_list.Where(pl => pl.id == 10000112385 && pl.amount > 0))// MoQ
                await DB.POS.create_posd_service(_view.PoshItem.Id, payment.id.ToString(), 1, payment.amount);
            decimal cheque_discount = _view.payment_list.Where(pl => pl.id == 10000109134).Sum(pl => pl.amount);
            if (cheque_discount > 0)//Lojalumo taškai
            {
                decimal totalsum = _view.PoshItem.PosdItems
                    .Where(pd => (pd.recipeid == 0 || (pd.recipeid != 0 && pd.compensationsum == 0)) && !pd.is_deposit)
                    .Sum(pd => pd.sum);

                decimal sum0 = _view.PoshItem.PosdItems
                    .Where(pd => pd.vatsize == 0 && (pd.recipeid == 0 || (pd.recipeid != 0 && pd.compensationsum == 0)) && !pd.is_deposit)
                    .Sum(pd => pd.sum);
                decimal points0 = Math.Round(cheque_discount * sum0 / totalsum, 2, MidpointRounding.AwayFromZero);

                decimal sum5 = _view.PoshItem.PosdItems
                    .Where(pd => pd.vatsize == 5 && (pd.recipeid == 0 || (pd.recipeid != 0 && pd.compensationsum == 0)))
                    .Sum(pd => pd.sum);
                decimal points5 = Math.Round(cheque_discount * sum5 / totalsum, 2, MidpointRounding.AwayFromZero);

                decimal sum21 = _view.PoshItem.PosdItems
                    .Where(pd => pd.vatsize == 21 && (pd.recipeid == 0 || (pd.recipeid != 0 && pd.compensationsum == 0)))
                    .Sum(pd => pd.sum);
                decimal points21 = Math.Round(cheque_discount * sum21 / totalsum, 2, MidpointRounding.AwayFromZero);

                if (points0 > 0)
                    await DB.POS.create_posd_service(_view.PoshItem.Id, "10000109134", 3, -1 * points0);
                if (points5 > 0)
                    await DB.POS.create_posd_service(_view.PoshItem.Id, "10000109134", 4, -1 * points5);
                if (points21 > 0)
                    await DB.POS.create_posd_service(_view.PoshItem.Id, "10000109134", 5, -1 * points21);
                await CreatePosPayment(_view.PoshItem.Id, PosPaymentType.KAS_LOYALTYPOINTS, null, "10000109134", cheque_discount);
            }

            if (_view.IsRoundingEnabled && _view.CashRoundingCalculation != null && _view.CashRoundingCalculation.RoundingValue != 0)
            {
                await DB.POS.create_posd_service(_view.PoshItem.Id, RoundingItemId.ToString(), 2, _view.CashRoundingCalculation.RoundingValue);
            }
        }

        private async Task PayAdvancePayment()
        {
            var creditId = 63833;
            var cashId = 0;
            var paySum = _view.PaySum;
            var creditAmount = _view.payment_list.Where(pl => pl.id == creditId && pl.amount > 0).Sum(s => s.amount);// banko kortele
            var cashAmount = _view.payment_list.Where(pl => pl.id == cashId && pl.amount > 0).Sum(s => s.amount);
            var paymentType = creditAmount > 0 ? "EK" : "EG";//e-kortele e-grynais
            var paymentAmount = creditAmount > 0 ? creditAmount : paySum;
            await _paymentRepository.UpdatePoshAdvancePayment(_view.PoshItem.Id, paymentType, paymentAmount);
            foreach (var posDetailItem in _view.PoshItem.PosdItems.Where(e => e.Type == "ADVANCEPAYMENT" && e.PresentCardId != 0)) 
            {
                try
                {
                    await _paymentRepository.UpdateAdvancePayment(posDetailItem.id, creditAmount > 0 ? creditId : cashId, posDetailItem.sum);

                    var content = new
                    {
                        issuedByPharmacyId = Session.SystemData.kas_client_id.ToInt(),
                        scalaId = creditAmount > 0 ? "A2" : "A1",
                        nr = _view.lastCheckNo.ToInt()
                    };

                    await _tamroClient.PostAsync<PresentCardIssuerViewModel>
                        (string.Format(Session.CKasV1PostPresentCardIssuer, posDetailItem.PresentCardId), content);

                    await CreatePosPayment(
                        _view.PoshItem.Id,
                        PosPaymentType.ADVANCEPAYMENT,
                        creditAmount > 0 ? creditId : cashId,
                        posDetailItem.barcodename,
                        posDetailItem.sum);
                }
                catch (Exception ex) 
                {
                    throw new PresentCardException("Apmokėjimas už dovanų kuponą nepavyko!");
                }
            }
        }

        private async Task PerformFMD()
        {
            var overallStopwatch = new Stopwatch();
            overallStopwatch.Start();

            if (Session.Params.FirstOrDefault(x => x.system == "FMD" && x.par == "ENABLED")?.value == "1"
                    && _view.PoshItem.fmd_models.Count > 0)
            {
                List<Task> tasks = new List<Task>();
                foreach (var posDetail in _view.PoshItem.PosdItems)
                {
                    if (posDetail.Flags.HasFlag(Enumerator.ProductFlag.FmdException))
                        continue;

                    foreach (var fmdModel in posDetail.fmd_model)
                    {
                        tasks.Add(Task.Run(async () =>
                        {
                            var taskStopwatch = new Stopwatch();
                            taskStopwatch.Start();

                            await CreatePosPayment(_view.PoshItem.Id, PosPaymentType.FMD, null, fmdModel.posDetail?.id.ToString(), 0);
                            Serilogger.GetLogger().Information($"[PerformFMD] KAS ID: {Session.SystemData.kas_client_id}" +
                                $" POSD.ID: {posDetail.id} CreatePosPayment took {taskStopwatch.ElapsedMilliseconds} ms");

                            taskStopwatch.Restart();
                            bool hasSupplied = await _posRepository.CheckIfSuppliedFMDItemExist(posDetail.productid.ToLong(),
                                                                                                fmdModel.productCode,
                                                                                                fmdModel.batchId,
                                                                                                fmdModel.serialNumber);
                            Serilogger.GetLogger().Information($"[PerformFMD] KAS ID: {Session.SystemData.kas_client_id}" +
                                $" POSD.ID: {posDetail.id} CheckIfSuppliedFMDItemExist took {taskStopwatch.ElapsedMilliseconds} ms");

                            if (!hasSupplied)
                            {
                                taskStopwatch.Restart();
                                var vm = new FMDposd()
                                {
                                    CurrentPosdRow = posDetail
                                };

                                var response_model = await vm.ChangeStateSinglePackAsync(fmdModel, FMD.Model.State.Supplied);
                                fmdModel.Response = response_model.Response;
                                Serilogger.GetLogger().Information($"[PerformFMD] KAS ID: {Session.SystemData.kas_client_id}" +
                                    $" POSD.ID: {posDetail.id} ChangeStateSinglePackAsync took {taskStopwatch.ElapsedMilliseconds} ms");
                            }
                        }));
                    }
                }
                await Task.WhenAll(tasks);
                Serilogger.GetLogger().Information($"[PerformFMD] KAS ID: {Session.SystemData.kas_client_id}" +
                    $" All tasks completed in {overallStopwatch.ElapsedMilliseconds} ms");

                var alertedFMDModel = _view.PoshItem.PosdItems.SelectMany(e => e.fmd_model).FirstOrDefault(e => !string.IsNullOrWhiteSpace(e?.Response?.alertId));
                if (alertedFMDModel != null)
                {
                    var alertStopwatch = new Stopwatch();
                    alertStopwatch.Start();

                    PerformFMDReport(alertedFMDModel);
                    Serilogger.GetLogger().Information($"[PerformFMD] {Session.SystemData.kas_client_id}" +
                        $" PerformFMDReport took {alertStopwatch.ElapsedMilliseconds} ms");

                    throw new FMDRequirementsException("Krepšelyje yra prekių neatitinkančių FMD reikalavimus!");
                }
            }
            overallStopwatch.Stop();
            Serilogger.GetLogger().Information($"[PerformFMD] {Session.SystemData.kas_client_id}" +
                $" method completed in {overallStopwatch.ElapsedMilliseconds} ms");
        }

        private async Task PayPresentCard()
        {
            foreach (var pl in _view.payment_list.Where(pl => pl.id == 4098 && pl.amount > 0))
            {
                try
                {
                    var content = new
                    {
                        soldByPharmacyId = Session.SystemData.kas_client_id.ToInt(),
                        amount = _view.PoshItem.PosdItems.Sum(e => e.sum)
                    };

                    foreach (var externalId in pl.ExternalIds)
                    {
                        await _tamroClient.PostAsync<PresentCardSellerViewModel>
                            (string.Format(Session.CKasV1PostPresentCardSeller, externalId), content);
                    }
                }
                catch (Exception ex) 
                {
                    throw new PresentCardException("Nepavyko aptarnauti su dovanų kuponu");
                }
  
                await CreatePosPayment(_view.PoshItem.Id, PosPaymentType.PRESENTCARD, pl.id, pl.code, _view.PaySum);
            }
        }

        private async Task VoidPresentCard(string presentCardNumbers)
        {
            try
            {
                var codes = presentCardNumbers.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                var presentCards = await _tamroClient.GetAsync<List<PresentCardViewModel>>
                    (string.Format(Session.CKasV1GetPresentCard, helpers.BuildQueryString("CardNumbers=", codes.ToList())));

                if (presentCards?.Count > 0)
                {
                    var content = new
                    {
                        soldByPharmacyId = 0
                    };

                    foreach (var presentCard in presentCards)
                    {
                        await _tamroClient.DeleteAsync<PresentCardSellerViewModel>
                            (string.Format(Session.CKasV1DeletePresentCardSeller, presentCard.Id), content);
                    }
                }
            }
            catch (Exception ex) 
            {
                Serilogger.GetLogger().Error(ex, ex.Message);
            }
        }

        private async Task VoidAdvancePayment(decimal presentCardId)
        {
            try
            {
                await _tamroClient.DeleteAsync<PresentCardSellerViewModel>
                        (string.Format(Session.CKasV1DeletePresentCardIssuer, presentCardId), new { issueByPharmacyId = 0});

            }
            catch (Exception ex)
            {
                Serilogger.GetLogger().Error(ex, ex.Message);
            }
        }

        private async Task PayInsurance()
        {
            if (_view.PoshItem?.InsuranceItem != null
                    && !_view.PoshItem.InsuranceItem.ConfirmedTransaction
                    && _view.PoshItem.InsuranceItem.CardSessionId != ""
                    && Math.Abs(_view.PoshItem.ChequeInsuranceSum) > 0)
            {
                var cardSessionId = _view.PoshItem.InsuranceItem.CardSessionId;
                _view.PoshItem.InsuranceItem.CardSessionId = await _view.PoshItem.InsuranceItem.Utils.ConfirmInsuranceCompensation(_view.PoshItem);
                _view.PoshItem.InsuranceItem.ConfirmedTransaction = !string.IsNullOrEmpty(_view.PoshItem.InsuranceItem.CardSessionId);
                if (!_view.PoshItem.InsuranceItem.ConfirmedTransaction)
                {
                    _view.PoshItem.InsuranceItem.CardSessionId = cardSessionId;
                    throw new Exception("Aptarnavimas su draudimo kompanija nepavyko!");
                }
                await CreatePosPayment(_view.PoshItem.Id, PosPaymentType.INSURANCE, null, _view.PoshItem.InsuranceItem.CardSessionId, _view.PoshItem.ChequeInsuranceSum);
            }
        }

        private async Task PayCrm()
        {
            if (Session.CRM && _view.PoshItem.CRMItem?.AcceptedPaymentResponse != null)
            {
                var success = await Session.CRMRestUtils.SendPurchase(_view.PoshItem, false);
                if (success)
                {
                    await DB.Loyalty.changeLoyaltyhStatus(_view.PoshItem.Id, 1, 1);
                    await CreateVaccinationReminders();
                }
                else
                {
                    await DB.Loyalty.changeLoyaltyhStatus(_view.PoshItem.Id, 0, 0);
                    throw new Exception("Nepavyksta patvirtinti tranzakcijos lojalumo sistemoje. Bandykite dar kartą!");
                }
                await CreatePosPayment(_view.PoshItem.Id, PosPaymentType.CRM, null, _view.PoshItem.CRMItem.Account.CardNumber, _view.PoshItem.TotalSum);
                await CreatePosPayment(_view.PoshItem.Id, PosPaymentType.CRM_USEDLOYALTYPOINTS, null, _view.PoshItem.CRMItem.Account.CardNumber, _view.PoshItem.CRMItem.AcceptedPaymentResponse.Data.CreditPoints.ToDecimal());
            }
        }

        private async Task CreateVaccinationReminders() 
        {
            if (string.IsNullOrEmpty(_view.PoshItem.CRMItem?.Account?.CardNumber))
                return;

            try
            {
                foreach (var posDetail in _view.PoshItem.PosdItems)
                {
                    if (posDetail.VaccinatedPatientId == 0)
                        continue;

                    var endTime = DateTime.Now;
                    var startTime = endTime.AddYears(-3);
                    var patientImmunizations = Session.eRecipeUtils.GetImmunization<ImmunizationListDto>(
                        posDetail.VaccinatedPatientId.ToString(),
                        Session.PractitionerItem.PractitionerId,
                        startTime.ToString("yyyy-MM-dd"),
                        endTime.AddDays(1).ToString("yyyy-MM-dd"));

                    string evenTypeId = Session.CRMRestUtils.GetEventTypeByName("Vaccination");

                    decimal npkaId7 = await Session.SamasUtils.GetNpakid7ByProductId(posDetail.productid);
                    if (npkaId7 == 0m) continue;

                    ImmunizationDto immunization = patientImmunizations.ImmunizationList
                        .Where(e => e.VaccineNPAKID7 == npkaId7.ToString())
                        .OrderByDescending(e => e.DoseNumber)
                        .FirstOrDefault();

                    if (immunization == null)
                        continue;

                    var vaccineDays = Session
                        .VaccineRemindersConfig
                        .FirstOrDefault(e => e.ProductId == posDetail.productid.ToLong() &&
                                        e.NumberOfVaccine == immunization.DoseNumber);

                    if (vaccineDays == null)
                        continue;

                    DateTime currentDate = DateTime.Now;
                    List<Cortex.Client.Model.PropertyRecord> propertyRecords = new List<Cortex.Client.Model.PropertyRecord>();

                    Cortex.Client.Model.PropertyRecord propertyRecord = new Cortex.Client.Model.PropertyRecord("p1_purchase_date");
                    propertyRecord.PropertyValue = currentDate.ToString("yyyy-MM-dd");
                    propertyRecords.Add(propertyRecord);

                    propertyRecord = new Cortex.Client.Model.PropertyRecord("p1_item_name");
                    propertyRecord.PropertyValue = immunization.VaccineName;
                    propertyRecords.Add(propertyRecord);

                    propertyRecord = new Cortex.Client.Model.PropertyRecord("p1_due_date");
                    propertyRecord.PropertyValue = currentDate.AddDays(vaccineDays.Days).ToString("yyyy-MM-dd");
                    propertyRecords.Add(propertyRecord);

                    var vaccineNoProperty = Session.CRMRestUtils.GetEventPropertyById("p1_number_of_vaccine");
                    var vaccineNoPropertyItem = vaccineNoProperty?.Items?.FirstOrDefault(val => val.Name.Contains(immunization.DoseNumber.ToString()));
                    if (vaccineNoPropertyItem == null)
                        continue;

                    propertyRecord = new Cortex.Client.Model.PropertyRecord(vaccineNoProperty.PropertyId);
                    propertyRecord.PropertyValue = new { vaccineNoPropertyItem };
                    propertyRecords.Add(propertyRecord);

                    if (!string.IsNullOrEmpty(evenTypeId) && !string.IsNullOrEmpty(_view.PoshItem.CRMItem?.Account?.CardNumber))
                    {
                        if (await Session.CRMRestUtils.CreateEvent(evenTypeId, _view.PoshItem.CRMItem?.Account?.ID, posDetail.id.ToString(), propertyRecords))
                        {
                            Serilogger.GetLogger().Information($"[CreateVaccinationReminders] Vaccination event has been created. " +
                                $"CardNumber {_view.PoshItem.CRMItem?.Account?.CardNumber}. " +
                                $"Pos Details: {_view.PoshItem.BuildLogs()}. " +
                                $"Event properties: {propertyRecords.ToJsonString()}");
                        }
                    }
                }
            }
            catch (Exception ex) 
            {
                Serilogger.GetLogger().Error(ex, ex.Message);
            }
        }

        private async Task VoidCrm()
        {
            try
            {
                if (Session.CRM && _view.PoshItem.CRMItem?.AcceptedPaymentResponse != null)
                {
                    if (await Session.CRMRestUtils.SendPurchase(_view.PoshItem, true))
                        await DB.Loyalty.changeLoyaltyhStatus(_view.PoshItem.Id, 0, 0);
                    var maxPoints = _view.payment_list?.Where(p => p.id == 10000109134).Sum(p => p.amount) ?? 0;
                    _view.PoshItem.CRMItem.AcceptedPaymentResponse = await Session.CRMRestUtils.AcceptPayment(_view.PoshItem, (float)maxPoints);
                }
            }
            catch (Exception ex) 
            {
                Serilogger.GetLogger().Error(ex, ex.Message);
            }
        }

        private async Task PayBenuM()
        {
            foreach (var payment in _view.payment_list.Where(pl => pl.id == 1111 && pl.amount > 0).ToList())
            {
                await Task.Factory.StartNew(() =>
                {
                    var transaction = new SR_Privat.Transaction();
                    SR_Privat.Items privat_items = new SR_Privat.Items
                    {
                        It = new List<SR_Privat.Item>()
                    };
                    privat_items.It = (from el in _view.PoshItem.PosdItems.ToList()
                                       select new SR_Privat.Item
                                       {
                                           PosdId = el.id,
                                           ProductId = el.productid,
                                           Sum = (double)el.sum,
                                           Qty = (double)el.qty
                                       }).ToList();
                    using (var scPrivat = new SR_Privat.PrivatSoapClient())
                        transaction = scPrivat.CreateTransaction(
                            payment.code,
                            DateTime.Now,
                            -1 * (double)payment.amount,
                            Session.SystemData.kas_client_id,
                            2,
                            1,
                            (double)payment.amount - (double)_view.PaySum,
                            _view.PoshItem.Id,
                            privat_items,
                            !Session.Develop);

                    if (transaction.Err != "" && transaction.Id == 0)
                    {
                        //if (privat_trans.ErrCode == 2)//nepakankamas likutis
                        //edtChSum.Text = privat_trans.Acc.CurrentBalance.ToString(CultureInfo.CurrentCulture);
                        throw new Exception("Nepavyksta įvykdyti transakcijos:\n" + transaction.Err);
                    }
                    payment.Buyer = transaction.Acc.AccountType.ToString();
                });
                await CreatePosPayment(_view.PoshItem.Id, PosPaymentType.BENUM, payment.id, payment.code, payment.amount);
            }
        }

        private async Task CreatePosPayment(decimal poshId, PosPaymentType paymentType, decimal? paymentId, string code, decimal amount)
        {
            var posPayment = new PosPayment
            {
                Hid = poshId,
                PaymentType = paymentType,
                PaymentId = paymentId,
                Code = code,
                Amount = amount
            };
            await _posRepository.CreatePosPayment(posPayment);
        }

        private async Task VoidBenuM()
        {
            try
            {
                if (_view.payment_list.Where(pl => pl.id == 1111 && pl.amount > 0).Any())
                {
                    using (var scPrivat = new SR_Privat.PrivatSoapClient())
                    {
                        await Task.Factory.StartNew(() =>
                        {
                            scPrivat.VoidTransaction((int)Session.SystemData.kas_client_id, _view.PoshItem.Id, !Session.Develop);
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Serilogger.GetLogger().Error(ex, ex.Message);
            }
        }

        private async Task PayFiscal()
        {
            if (Session.Devices.fiscal != 1)
                return;

            bool isAdvancePayment = _view.PoshItem.PosdItems.Any(item => item.Type == "ADVANCEPAYMENT");
            bool ispresentCardAdvancePayment = _view.PoshItem.PosdItems.Any(item => item.Type == "ADVANCEPAYMENT" && item.PresentCardId != 0);
            //mokejimas su fiskalu
            #region sell only recipes when cash
            if (_view.PoshItem.CompensatedSum > 0 && _view.payment_list.FirstOrDefault(f => f.fiscal == "P").amount > 0)//jei yra kompensuojamu vaistu ir atsiskaitymui naudojami gryni pinigai
            {
                await PayFiscalCompensatedAmount();
            }
            #endregion
            #region sell everything else
            if ((_view.PoshItem.TotalSum > 0 || _view.PoshItem.ChequeSum > 0 || _view.PoshItem.ChequeInsuranceSum > 0) && !isAdvancePayment)
            {
                await Session.FP550.FiscalSaleOpen();
                await Session.FP550.FiscalSaleText("Jus aptarnavo " + Session.User.DisplayName);
                await Session.FP550.FiscalSaleText("------------------------------------------");

                //kitos prekes
                foreach (Items.posd it in _view.PoshItem.PosdItems.Where(pd => pd.recipeid == 0 && !pd.is_deposit).OrderBy(pd => pd.id))
                {
                    int tax = (int)it.ecrtax;
                    if (it.is_deposit)
                        tax = 3;
                    var name2 = $"{it.price} EUR X {it.qty} vnt.";
                    await Session.FP550.FiscalSaleItem(it.barcodename, name2, tax, it.price, it.qty, -1 * it.discount, ',');
                    if (-1 * it.cheque_sum_insurance > 0)
                        await Session.FP550.FiscalSaleText("Draud. komp. " + -1 * it.cheque_sum_insurance + " EUR");
                    if (-1 * it.cheque_sum_insurance > 0 && _view.PoshItem.InsuranceItem.Company == "CMP" && it.compensation_type != "")
                    {
                        await Session.FP550.FiscalSaleText("Kompensuota iš:");
                        await Session.FP550.FiscalSaleText(it.compensation_type);
                    }
                }

                // prekės su receptu, be kompencacijos
                foreach (Items.posd it in _view.PoshItem.PosdItems.Where(pd => pd.recipeid != 0 && pd.compensationsum == 0).OrderBy(pd => pd.id))
                {
                    int tax = (int)it.ecrtax;
                    var name2 = $"Priemoka {it.paysum} EUR";
                    await Session.FP550.FiscalSaleItem(it.barcodename, name2, tax, it.paysum, 1, -it.discount, ',');
                }

                decimal cheque_discount = _view.payment_list.Where(pl => pl.id == 10000109134).Sum(pl => pl.amount);
                if (cheque_discount > 0)
                {
                    await Session.FP550.FiscalSaleText("------------------------------------------");
                    foreach (var payment in _view.payment_list.Where(pl => pl.id == 10000109134))//NUOLAIDA
                    {
                        await Session.FP550.FiscalDiscount(payment.amount);
                    }
                    await Session.FP550.FiscalSaleText(" ");
                }

                // tik priemokos
                foreach (Items.posd it in _view.PoshItem.PosdItems.Where(pd => pd.recipeid != 0 && pd.compensationsum > 0).OrderBy(pd => pd.id))
                {
                    int tax = (int)it.ecrtax;
                    if (it.sum != 0 || it.discount != 0 || it.price != 0)
                    {    
                        var name2 = $"Priemoka {it.paysum} EUR";
                        await Session.FP550.FiscalSaleItem(it.barcodename, name2, tax, it.paysum, 1, -it.discount, ',');
                        if (-1 * it.cheque_sum_insurance > 0)
                            await Session.FP550.FiscalSaleText("Draud. komp. " + -1 * it.cheque_sum_insurance + " EUR");
                        if (-1 * it.cheque_sum_insurance > 0 && _view.PoshItem.InsuranceItem.Company == "CMP" && it.compensation_type != "")
                        {
                            await Session.FP550.FiscalSaleText("Kompensuota iš:");
                            await Session.FP550.FiscalSaleText(it.compensation_type);
                        }
                    }
                }

                //depozitas
                foreach (Items.posd it in _view.PoshItem.PosdItems.Where(pd => pd.is_deposit).OrderBy(pd => pd.id))
                {
                    int tax = Session.getParam("EKA", "NEW") == "1" ? 13 : 3;
                    var name2 = $"{it.price} EUR X {it.qty} vnt.";
                    await Session.FP550.FiscalSaleItem(it.barcodename, name2, tax, it.price, it.qty, -1 * it.discount, ',');
                }

                if (_view.PoshItem.ChequeSum != 0 || _view.PoshItem.ChequeInsuranceSum != 0 || _view.PoshItem.DiscountSum != 0)
                {
                    await Session.FP550.FiscalSaleText("------------------------------------------");
                    if (_view.PoshItem.DiscountSum != 0)
                        await Session.FP550.FiscalSaleText("NUOLAIDOS: " + _view.PoshItem.DiscountSum.ToString());
                    if (_view.PoshItem.ChequeInsuranceSum != 0)
                        await Session.FP550.FiscalSaleText("DRAUDIMAS APMOKA: " + Math.Round(_view.PoshItem.ChequeInsuranceSum * (-1), 2).ToString());
                    if (_view.PoshItem.ChequeSum != 0)
                        await Session.FP550.FiscalSaleText("GSK NUOLAIDA: " + Math.Round(_view.PoshItem.ChequeSum * (-1), 2).ToString());
                    /*if (payment_type == 0)
                        await Session.FP550.FiscalSaleText("M.GRYNAIS: " + edtPaySum.Text);
                    if (payment_type == 1)
                        await Session.FP550.FiscalSaleText("M.KORTELE: " + edtPaySum.Text);*/
                }

                string comment1 = "";
                decimal sum2pay = _view.PoshItem.TotalSum;
                if (_view.PoshItem.DiscountSum != 0)
                    comment1 = "IŠ VISO NUOLAIDŲ " + (_view.PoshItem.DiscountSum + cheque_discount).ToString().Replace('.', ',') + " EUR";
                string comment2 = "IŠ VISO SUMOKĖTA " + (sum2pay - cheque_discount).ToString().Replace('.', ',') + " EUR";

                if (!_view.payment_list.Where(pl => pl.fiscal_rank >= 0).Any(pl => pl.amount > 0))
                {
                    try
                    {
                        Serilogger.GetLogger().Information($"[PayFiscal] Apmokėjimo sumos nulines! " +
                            $"Posh ID: {_view.PoshItem?.Id}; " +
                            $"Check No: {_view.PoshItem?.CheckNo}; " +
                            $"KAS Client ID: {Session.SystemData.kas_client_id}; " +
                            $"POS Version: {Session.AssemblyVersion}; " +
                            $"Pay Sum: {_view.PaySum}; " +
                            $"payment_list: {_view.payment_list.ToJsonString()};" +
                            $"PaymentViewGrid: {helpers.DataGridViewToJson(_view.PaymentViewGrid)};");
                    }
                    catch (Exception ex)
                    {
                    }
                }

                foreach (var payment in _view.payment_list.Where(pl => pl.fiscal_rank >= 0).OrderBy(pl => pl.fiscal_rank))
                {
                    decimal payment_amount = payment.amount;
                    if (payment.amount > 0)
                    {
                        if (payment.fiscal == FiscalCredit1Value)//cekiai
                        {
                            if (payment_amount > sum2pay)
                                payment_amount = sum2pay;
                        }
                        if (payment.fiscal == FiscalBankCardValue)//banko kortele
                        {
                            if (payment.amount > sum2pay)
                            {
                                throw new Exception("Neteisingos atsiskaitymo sumos. Kvito vertė " + sum2pay + ", mokama suma banko kortele " + payment.amount + ".\nSkambinkite į Service Desk!");
                            }
                        }
                        if (payment.id == 10000112385)
                        {
                            var requestModel = new Models.FeedbackTerminal.MoQRequest
                            {
                                Code = Session.Devices.header4
                            };
                            Session.FeedbackTerminal.ExecuteAction(Models.FeedbackTerminal.RequestName.MoQ, requestModel);
                        }
                        await Session.FP550.FiscalSalePay(comment1, comment2, GetFiscalRank(payment.fiscal), payment_amount);
                        sum2pay -= payment_amount;
                        comment1 = "";
                        comment2 = "";
                    }
                }//foreach payment

                if (_view.PoshItem?.CRMItem != null &&
                    !string.IsNullOrEmpty(_view.PoshItem?.CRMItem?.Account.CardNumber) &&
                    _view.PoshItem.LoyaltyCardType != "RIMI")
                {
                    await Session.FP550.FiscalSaleText("BENU lojalumo kortelė");
                    string txt = _view.PoshItem?.CRMItem?.Account?.CardNumber;
                    await Session.FP550.FiscalSaleText(txt);
                    txt = _view.PoshItem.CRMItem.AcceptedPaymentResponse.Data.CreditPointsOfThisTransaction.ToString().Replace('.', ',');
                    if (_view.PoshItem.CRMItem.AccruePoints == 5)//donations
                    {
                        await Session.FP550.FiscalSaleText("AČIŪ už Jūsų gerumą");
                        await Session.FP550.FiscalSaleText("bei paaukotą " + txt + " EUR sumą!");
                        txt = (_view.PoshItem.CRMItem.AcceptedPaymentResponse.Data.TotalCreditPoints - _view.PoshItem.CRMItem.AcceptedPaymentResponse.Data.CreditPointsOfThisTransaction).ToString().Replace('.', ',');
                    }
                    else
                    {
                        await Session.FP550.FiscalSaleText("Taškai už kvitą " + txt);
                        txt = _view.PoshItem.CRMItem.AcceptedPaymentResponse.Data.TotalCreditPoints.ToString().Replace('.', ',');
                    }
                    await Session.FP550.FiscalSaleText("Sukaupti taškai " + txt);
                    if (_view.PoshItem.CRMItem.AcceptedPaymentResponse.Data.CreditPoints > 0)
                    {
                        txt = _view.PoshItem.CRMItem.AcceptedPaymentResponse.Data.CreditPoints.ToString().Replace('.', ',');
                        await Session.FP550.FiscalSaleText("Panaudoti taškai " + txt);
                    }
                }

                if (_view.PoshItem.UsedCardNo > 0 && _view.PoshItem.LoyaltyCardType == "RIMI")
                {
                    await Session.FP550.FiscalSaleText("Mano RIMI kortelė");
                    string txt = "**** **** **** " + _view.PoshItem.UsedCardNo.ToString().Substring(Math.Max(0, _view.PoshItem.UsedCardNo.ToString().Length - 4));
                    await Session.FP550.FiscalSaleText(txt);
                    await Session.FP550.FiscalSaleText("Uždirbti Mano RIMI");
                    txt = "pinigai " + _view.PoshItem.RimiDiscAmount.ToString().Replace('.', ',');
                    await Session.FP550.FiscalSaleText(txt);
                }

                var benum = _view.payment_list.Where(pl => pl.id == 1111).SingleOrDefault();
                if (benum != null && benum.amount > 0)//BENUM
                {
                    SR_Privat.Account acc;
                    using (var scPrivat = new SR_Privat.PrivatSoapClient())
                        acc = scPrivat.GetAccount(benum.code, Session.SystemData.kas_client_id, DateTime.Now, !Session.Develop);
                    if (acc.AccountName == "MALTA")
                    {
                        await Session.FP550.FiscalSaleText("Pasaulio Tautų Teisuolio kortelė");
                        string txt = "***** **** " + benum.code.Substring(9, 4);
                        await Session.FP550.FiscalSaleText(txt);
                        await Session.FP550.FiscalSaleText("Likutis: " + acc.CurrentBalance.ToString(System.Globalization.CultureInfo.CurrentCulture));
                    }
                    if (acc.AccountName == "VICI")
                    {
                        await Session.FP550.FiscalSaleText("BENU partnerio kortelė");
                        await Session.FP550.FiscalSaleText("Amber Food");//todo
                        await Session.FP550.FiscalSaleText(benum.code);
                    }
                }

                if (_view.PoshItem.InsuranceItem != null && _view.PoshItem.InsuranceItem.CardSessionId != "")
                {
                    if (_view.PoshItem.InsuranceItem.Company != "CMP")
                    {
                        await Session.FP550.FiscalSaleText(" ");
                        await Session.FP550.FiscalSaleText(_view.PoshItem.InsuranceItem.CompanyString);
                        await Session.FP550.FiscalSaleText(_view.PoshItem.InsuranceItem.Utils.getCardNo(_view.PoshItem.InsuranceItem.CardNoLong));
                        List<Items.ComboBox<decimal>> cardLimits = _view.PoshItem.InsuranceItem.Utils.GetCardBalance(_view.PoshItem.InsuranceItem);
                        foreach (var cardLimit in cardLimits)
                        {
                            await Session.FP550.FiscalSaleText(cardLimit.DisplayMember);
                            await Session.FP550.FiscalSaleText("Likutis: " + cardLimit.ValueMember);
                        }
                    }
                    if (_view.PoshItem.InsuranceItem.Company != "ERG" && _view.PoshItem.InsuranceItem.Company != "GJN")
                        await Session.FP550.FiscalSaleText("Autorizacijos nr. " + _view.PoshItem.InsuranceItem.CardSessionId);
                }

                if (Session.Devices.realtime == 5 || Session.Devices.realtime == 7)
                {
                    await Session.FP550.FiscalSaleText(" ");
                    await Session.FP550.FiscalSaleText("Sveikatos Jums!");
                    await Session.FP550.FiscalSaleText("Mums svarbi Jūsų nuomonė!");
                    await Session.FP550.FiscalSaleText("www.benu.lt/atsiliepimai");
                }

                // kvito pabaiga
                _view.lastCheckNo = await Session.FP550.GetNextCheckNo();
                await Session.FP550.FiscalSaleClose();
            }
            #endregion
            #region sell only recipes when without cash
            if (_view.PoshItem.CompensatedSum > 0 && _view.payment_list.FirstOrDefault(f => f.fiscal == "P").amount <= 0)//jei yra kompensuojamu vaistu ir atsiskaitymui nenaudojami gryni pinigai
            {
                await PayFiscalCompensatedAmount();
            }
            #endregion
            #region receptu spausdinimias
            foreach (Items.posd it in _view.PoshItem.PosdItems.Where(pd => pd.recipeid != 0 || pd.erecipe_no != 0).OrderBy(pd => pd.id))
            {
                List<string> eRecipeCheque = new List<string>();
                //Recipe cheque
                if (it.recipeid != 0)//kompensuojamas
                {
                    if (Session.Devices.realtime == 8)
                    {
                        if (it.erecipe_no == 0)
                            await PrintRecipeCopy(it, it.barcodename);
                        await PrintRecipeCopy(it, it.barcodename);
                    }
                    else if (Session.Devices.realtime == 3 || Session.Devices.realtime == 4)
                    {
                        await PrintRecipeCopy(it, it.barcodename);
                        await PrintRecipeCopy(it, it.barcodename);
                    }
                    else if (Session.Devices.realtime == 5)
                    {
                        await PrintRecipeCopy(it, it.barcodename);
                    }
                }
                //nekompensuojamas eRecipe
                else
                {
                    eRecipeCheque.Add("------------------------------------------");
                    eRecipeCheque.Add(it.barcodename);
                    eRecipeCheque.Add("IŠDUOT. KIEKIS: " + Math.Round(it.qty, 3).ToString().Replace('.', ','));
                    eRecipeCheque.Add("KAINA: " + it.sum.ToString().Replace('.', ','));
                    //todo pakanka iki datos reik
                    if (it.till_date.ToString().Length > 10)
                        eRecipeCheque.Add("Pakanka iki: " + it.till_date.ToString().Substring(0, 10));
                    else
                        eRecipeCheque.Add("Pakanka iki: " + it.till_date.ToString());
                }
                if (eRecipeCheque.Count > 0)
                    await PrintERecipeCopy(eRecipeCheque);
            }
            
            // nekompensuojamas popierinis receptas
            foreach (Items.posd it in _view.PoshItem.PosdItems.Where(pd => pd.NotCompensatedRecipeId != 0))
            {
                await Session.FP550.OpenNonFiscal();
                await Session.FP550.PrintNonFiscal(it.barcodename);
                if (it.NotCompensatedTillDate.HasValue)
                    await Session.FP550.PrintNonFiscal("PAKANKA IKI: " + it.NotCompensatedTillDate.Value.ToString("yyyy.MM.dd"));
                await Session.FP550.PrintNonFiscal("IŠDUOT. KIEKIS: " + it.qty.ToString());
                await Session.FP550.PrintNonFiscal("KAINA: " + it.sum.ToString());
                await Session.FP550.PrintNonFiscal("Vaistai išduoti: ");
                await Session.FP550.PrintNonFiscal("    " + Session.User.postname);
                await Session.FP550.PrintNonFiscal("    " + Session.User.DisplayName);
                await Session.FP550.CloseNonFiscal();
            }
            #endregion            
            if (_view.PoshItem.InsuranceItem != null)
                await PrintInsuranceCheque();
            #region Advance payment
            if (ispresentCardAdvancePayment)
            {
                _view.lastCheckNo = await Session.FP550.GetNextCheckNo();
                var cashPayment = _view.payment_list.Where(list => list.id == 0).FirstOrDefault();
                var cardPayment = _view.payment_list.Where(list => list.id == 63833).FirstOrDefault();
                string type = cardPayment.amount > 0 ? "C" : "A";

                string line = "DK: ";
                string codes = string.Join(",", _view.PoshItem.PosdItems.ConvertAll(e=>e.barcodename));
                line += codes;
                if (line.Length > 64)
                    line = line.Substring(0, 61) + "...";

                await Session.FP550.FiscalAdvancePay(line, type, _view.PaySum);
            }
            else if (isAdvancePayment)
            {
                _view.lastCheckNo = await Session.FP550.GetNextCheckNo();
                var cashPayment = _view.payment_list.Where(list => list.id == 0).FirstOrDefault();
                var cardPayment = _view.payment_list.Where(list => list.id == 63833).FirstOrDefault();

                if (cardPayment.amount > 0)
                    await Session.FP550.FiscalAdvancePay(_view.PoshItem.PosdItems.FirstOrDefault().barcodename, "C", _view.PaySum);

                if (cashPayment.amount > 0)
                    await Session.FP550.FiscalAdvancePay(_view.PoshItem.PosdItems.FirstOrDefault().barcodename, "A", _view.PaySum - cardPayment.amount);
            }
            #endregion

            if (_view.PoshItem.CompensatedSum == 0)
                await PrintDiscountCheque();
            foreach (var payment in _view.payment_list.Where(pl => pl.amount > 0))
            {
                await CreatePosPayment(_view.PoshItem.Id, PosPaymentType.FISCAL, payment.id, payment.code, payment.amount);
            }
        }

        private async Task PayHomeMode() 
        {
            if (Session.HomeMode) 
            {
                try
                {
                    var orderHeaderId = await _homeModeRepository.CreateOrder();
                    var clientOrderSeqNo = await _homeModeRepository.GetClientOrderSequenceNo();
                    var clientOrderNo = $"HD{Session.SystemData.PharmacyNo}{DateTime.Now:yyyyMMdd}{clientOrderSeqNo}";

                    var orderLines = new List<E1OrderLineCreateViewModel>();

                    var shipmentLines = new List<ShipmentLineWriteViewModel>();

                    Serilogger.GetLogger().Information($"[PayHomeMode] Order Id: {orderHeaderId}");

                    await _homeModeRepository.SetOrderName(orderHeaderId, clientOrderNo);

                    var homeDeliveryOrderDetails = await _homeModeRepository.GetHomeDeliveryDetailSummarized(_view.PoshItem.Id);

                    foreach (var homeDeliveryDetail in homeDeliveryOrderDetails)
                    {
                        var barcodeName = _view.PoshItem?.PosdItems?.FirstOrDefault(e => e.productid == homeDeliveryDetail.ProductId)?.barcodename ?? string.Empty;
                        decimal? tamroItemId = await _homeModeRepository.GetTamroItemIdByProductId(homeDeliveryDetail.ProductId);

                        await _homeModeRepository.CreateOrderLine(orderHeaderId,
                            homeDeliveryDetail.ProductId,
                            homeDeliveryDetail.HomeQty,
                            tamroItemId.Value.ToString(),
                            TamroSupplierId);

                        orderLines.Add(new E1OrderLineCreateViewModel()
                        {
                            ItemCode = tamroItemId.Value.ToString(),
                            Quantity = homeDeliveryDetail.HomeQty.ToInt()
                        });

                        shipmentLines.Add(new ShipmentLineWriteViewModel()
                        {
                            ItemId = homeDeliveryDetail.ProductId.ToString(),
                            ItemName = barcodeName,
                            Quantity = homeDeliveryDetail.HomeQty.ToInt(),
                            RowVer = DateTime.Now
                        });
                    }

                    await _homeModeRepository.FormOrder(orderHeaderId);

                    var e1OrderCreateViewModel = new E1OrderCreateViewModel
                    {
                        Country = "LT",
                        ClientId = new Guid(E1OrderClientGUID),
                        AddressNumber = Session.SystemData.ProdisCustNo.ToInt(),
                        ClientOrderNo = clientOrderNo,
                        RouteNumber = RouteNumberViewModel.EshopRoute,
                        Lines = orderLines
                    };

                    Serilogger.GetLogger().Information($"[PayHomeMode] Order No: {clientOrderNo} request to E1 {e1OrderCreateViewModel.ToJsonString()}");

                    var orderResponse = await _tamroClient.PostAsync<E1OrderViewModel>(Session.E1GatewayV1PostOrders,
                            JObject.Parse(JsonConvert.SerializeObject(e1OrderCreateViewModel)));

                    Serilogger.GetLogger().Information($"[PayHomeMode] Order No: {clientOrderNo} response: {orderResponse.ToJsonString()}");

                    var partnerId = await _homeModeRepository.GetPartnerIdByPosHeaderId(_view.PoshItem.Id);
                    var partner = await _partnerRepository.GetPartnerById(partnerId);

                    var shipmentWriteViewModel = new ShipmentWriteViewModel
                    {
                        CountryId = 1,
                        Provider = ProviderViewModel.DPD,
                        ClientOrderNo = clientOrderNo,
                        BoxCount = 1,
                        Receiver  = new Models.DeliveryService.Receiver.ReceiverViewModel
                        {
                            Name = partner.Name.Length >= 40 ? partner.Name.Substring(0, 40) : partner.Name,
                            Phone = partner.Phone,
                            Email = partner.Email,
                            Address = partner.Address,
                            ZipCode = helpers.ParseNumberFromString(partner.PostIndex),
                            Country = "LT",
                            City = partner.City
                        },
                        DestinationType = DestinationTypeViewModel.Home,
                        Comment = partner.Descrip,
                        ShipmentLines = shipmentLines
                    };

                    Serilogger.GetLogger().Information($"[PayHomeMode] Order No: {clientOrderNo} request to delivery service {shipmentWriteViewModel.ToJsonString()}");

                    var shipmentResponse = await Session.BalticPosGateway.PutAsync<ShipmentViewModel>(Session.DeliveryV1PutShipments,
                            JObject.Parse(JsonConvert.SerializeObject(shipmentWriteViewModel)));

                    Serilogger.GetLogger().Information($"[PayHomeMode] Order No: {clientOrderNo} delivery service response: {shipmentResponse.ToJsonString()}");

                    await _homeModeRepository.SetClientOrderNoToHomeDeliveryOrder(_view.PoshItem.Id, shipmentResponse?.ClientOrderNo ?? string.Empty);
                    await _homeModeRepository.SetOrderIdToHomeDeliveryOrder(_view.PoshItem.Id, orderHeaderId);
                    await _homeModeRepository.CommitOrder(orderHeaderId, TamroSupplierId);
                    await PrintHomeDeliveryReceipt(shipmentResponse?.ClientOrderNo ?? string.Empty, _view.PoshItem.PosdItems);
                    Serilogger.GetLogger().Information($"[PayHomeMode] Order No: {clientOrderNo}; Order ID: {orderHeaderId} has been successfully sent E1");
                }
                catch (Exception ex) 
                {
                    Serilogger.GetLogger().Error(ex, $"[PayHomeMode] error: {ex.Message}");
                }
            }
        }

        private async Task PayFiscalCompensatedAmount()
        {
            await Session.FP550.FiscalSaleOpen();
            await Session.FP550.FiscalSaleText("Jus aptarnavo " + Session.User.DisplayName);
            await Session.FP550.FiscalSaleText("------------------------------------------");
            foreach (Items.posd it in _view.PoshItem.PosdItems.Where(pd => pd.recipeid != 0).OrderBy(pd => pd.id))
            {
                int tax = (int)it.ecrtax;
                if (it.compensationsum != 0)
                {
                    var name2 = $"Kompensuota {it.compensationsum} EUR";
                    await Session.FP550.FiscalSaleItem(it.barcodename, name2, tax, it.compensationsum, 1, 0, ';');
                }
            }
            if (_view.PoshItem.CompensatedSum != 0)
            {
                await Session.FP550.FiscalSalePay("KOMPENSUOTA " + _view.PoshItem.CompensatedSum.ToString().Replace('.', ',') + " EUR", "", "N", _view.PoshItem.CompensatedSum);
                await CreatePosPayment(_view.PoshItem.Id, PosPaymentType.TLK_COMPENSATION, null, null, _view.PoshItem.CompensatedSum);
            }
            // kvito pabaiga
            _view.lastCheckNo = await Session.FP550.GetNextCheckNo();
            await Session.FP550.FiscalSaleClose();
        }

        private async Task PrintHomeDeliveryReceipt(string clientOrderNo, List<Items.posd> posDetails) 
        {
            if (string.IsNullOrWhiteSpace(clientOrderNo))
                return;

            await Session.FP550.OpenNonFiscal();
            await Session.FP550.PrintNonFiscal($"Užsakymo numeris - {clientOrderNo}");
            foreach (Items.posd posDetail in posDetails.Where(pd => pd.recipeid != 0 || pd.erecipe_no != 0).OrderBy(pd => pd.id))
            {
                await Session.FP550.PrintNonFiscal($"Recepto išdavimo numeris - {posDetail.recipeno}");
            }
            await Session.FP550.CloseNonFiscal();
        }

        private async Task PrintRecipeCopy(Items.posd it, string item)
        {
            decimal mk = Math.Round(it.salesprice * it.rqty, 2);
            decimal bk = Math.Round(it.basicprice * it.rqty, 2);
            decimal rp = Math.Round(it.retail_price * it.rqty, 2);
            decimal nuol = rp - mk;
            string proc = "";
            if (it.paysum + nuol > 0)
                proc = Math.Round(nuol / (it.paysum + nuol) * 100).ToString();
            int proc_len = proc.ToString().Length;
            if (proc_len == 1)
                proc.Insert(0, "  ");
            if (proc_len == 2)
                proc.Insert(0, " ");

            await Session.FP550.OpenNonFiscal();
            await Session.FP550.PrintNonFiscal("------------------------------------------");
            await Session.FP550.PrintNonFiscal("Eil. Nr. : " + it.row_no);
            await Session.FP550.PrintNonFiscal("Recepto Nr. : " + it.recipeno);
            string given_qty = it.gqty.ToString();
            if (it.gqty - (int)it.gqty == 0)
                given_qty = given_qty.Substring(0, given_qty.LastIndexOf(','));
            await Session.FP550.PrintNonFiscal("10. " + item + " IŠDUOT. KIEKIS: " + Math.Round(it.rqty, 3).ToString().Replace('.', ',') + " (" + given_qty + ")");
            if (it.till_date.ToString().Length > 10)
                await Session.FP550.PrintNonFiscal("11. Pakanka iki    : " + it.till_date.ToString().Substring(0, 10));
            else
                await Session.FP550.PrintNonFiscal("11. Pakanka iki    : " + it.till_date.ToString());
            await Session.FP550.PrintNonFiscal("12. Mažmeninė kaina: " + helpers.doFormat(mk.ToString().Replace('.', ',')) + " EUR");
            var maxPrePayment = Math.Round(it.paysum + nuol, 2);
            if (maxPrePayment > 0)
                await Session.FP550.PrintNonFiscal("Maksimali priemoka : " + helpers.doFormat(maxPrePayment.ToString().Replace('.', ',')) + " EUR");
            if (nuol > 0)
                await Session.FP550.PrintNonFiscal("BENU nuolaida      : " + helpers.doFormat(nuol.ToString().Replace('.', ',')) + " EUR");
            await Session.FP550.PrintNonFiscal("13. Mokėti         : " + helpers.doFormat(it.paysum.ToString().Replace('.', ',')) + " EUR");
            if (it.cheque_sum != 0)
            {
                await Session.FP550.PrintNonFiscal("GSK kompensacija   : " + helpers.doFormat(((-1) * it.cheque_sum).ToString().Replace('.', ',')) + " EUR");
                await Session.FP550.PrintNonFiscal("Kliento mokama suma: " + helpers.doFormat((it.paysum.ToDecimal() - (-1) * it.cheque_sum).ToString().Replace('.', ',')) + " EUR");
            }
            await Session.FP550.PrintNonFiscal("14. Kompensuota    : " + helpers.doFormat(it.compensationsum.ToString().Replace('.', ',')) + " EUR (" + it.comppercent.ToString() + "%)");
            if (it.salesdate.ToString().Length > 10)
                await Session.FP550.PrintNonFiscal("15. Išdavimo data  : " + it.salesdate.ToString().Substring(0, 10));
            else
                await Session.FP550.PrintNonFiscal("15. Išdavimo data  : " + it.salesdate.ToString());
            await Session.FP550.PrintNonFiscal("16. Vaistai išduoti: ");
            await Session.FP550.PrintNonFiscal("    " + Session.User.DisplayName);
            await Session.FP550.PrintNonFiscal("------------------------------------------");
            await Session.FP550.CloseNonFiscal();
        }

        private async Task PrintERecipeCopy(List<string> Lines)
        {
            await Session.FP550.OpenNonFiscal();
            foreach (string line in Lines)
            {
                await Session.FP550.PrintNonFiscal(line);
            }
            await Session.FP550.PrintNonFiscal("------------------------------------------");
            await Session.FP550.CloseNonFiscal();
        }

        private async Task PrintInsuranceCheque()
        {
            await Session.FP550.OpenNonFiscal();
            await Session.FP550.PrintNonFiscal("Jus aptarnavo " + Session.User.DisplayName);
            await Session.FP550.PrintNonFiscal("------------------------------------------");
            await Session.FP550.PrintNonFiscal(_view.PoshItem.InsuranceItem.CompanyString);
            //await Session.FP550.PrintNonFiscal("***********" + _view.PoshItem.InsuranceItem.card_no.Substring(15));
            await Session.FP550.PrintNonFiscal(_view.PoshItem.InsuranceItem.Utils.getCardNo(_view.PoshItem.InsuranceItem.CardNoLong));
            await Session.FP550.PrintNonFiscal(" ");
            foreach (Items.posd pd in _view.PoshItem.PosdItems.Where(od => od.cheque_sum_insurance < 0).OrderBy(pd => pd.id))
            {
                await Session.FP550.PrintNonFiscal(pd.barcodename);
                if (pd.erecipe_no != 0)
                    await Session.FP550.PrintNonFiscal("Pagal elektroninį receptą");
                await Session.FP550.PrintNonFiscal("Komp. suma " + -1 * pd.cheque_sum_insurance + " EUR");
                if (_view.PoshItem.InsuranceItem.Company == "CMP" && pd.compensation_type != "")
                {
                    await Session.FP550.PrintNonFiscal("Kompensuota iš:");
                    await Session.FP550.PrintNonFiscal(pd.compensation_type);
                }
            }
            await Session.FP550.PrintNonFiscal("------------------------------------------");
            await Session.FP550.PrintNonFiscal("Draudimas kompensuoja " + -1 * _view.PoshItem.ChequeInsuranceSum + " EUR");
            await Session.FP550.PrintNonFiscal(" ");
            await Session.FP550.PrintNonFiscal("Patvirtinu, kad pirmiau pateikta informacija teisinga ir nurodytas prekes gavau");
            await Session.FP550.PrintNonFiscal(" ");
            await Session.FP550.PrintNonFiscal("__________________________________________");
            await Session.FP550.PrintNonFiscal(".             (parašas)");
            if (_view.PoshItem.InsuranceItem.Company != "ERG" && _view.PoshItem.InsuranceItem.Company != "GJN")
                await Session.FP550.PrintNonFiscal("Autorizacijos nr. " + _view.PoshItem.InsuranceItem.CardSessionId);
            await Session.FP550.PrintNonFiscal("------------------------------------------");
            await Session.FP550.CloseNonFiscal();
        }

        private async Task PrintDiscountCheque()
        {
            if (Session.Devices.realtime == 6)
                await Session.FP550.CutLine();

            if (Session.PromChequeLines != null && Session.PromChequeLines.Count > 0)
            {
                foreach (var promotionCheque in Session.PromChequeLines.GroupBy(p => p.HeaderId).
                    Select(group => new PromotionCheque
                    {
                        HeaderId = group.First().HeaderId,
                        Line = group.First().Line,
                        LineNo = group.First().LineNo,
                        Style = group.First().Style,
                        NoLC = group.First().NoLC,
                        Rimi = group.First().Rimi,
                        Benu = group.First().Benu,
                        Cost = group.First().Cost
                    }))
                {
                    if (!promotionCheque.IsFitByPOSHeader(_view.PoshItem))
                        continue;

                    await Session.FP550.OpenNonFiscal();
                    foreach (var lines in Session.PromChequeLines.Where(p => p.HeaderId == promotionCheque.HeaderId).OrderBy(p => p.LineNo))
                    {
                        if (lines.Style >= 0) 
                        {
                            await Session.FP550.PrintNonFiscal(FormatText(lines.Line, lines.Style));
                        }                    
                        else//print BC
                            await Session.FP550.PrintBarcode(lines.Line, -1 * lines.Style);
                    }
                    await Session.FP550.CloseNonFiscal();
                }
            }
            if (Session.Devices.realtime == 6)
                await Session.FP550.CutLine();
        }

        private string FormatText(string txt, decimal style)
        {
            int max_length = Session.getParam("NONFISCALTEXT", "LENGHT").ToInt();
            if (style == 2)
                max_length = max_length / 2 + 1;
            int spaces = ((max_length - txt.Length) / 2) - 1;
            string line_to_print = "";
            if (style > 0)
                line_to_print += "\t\t" + style;
            line_to_print += "\t";//center text
            for (int i = 1; i < spaces; i++)
                line_to_print += " ";
            if (txt.Length > max_length)
                line_to_print += txt.Substring(0, max_length);
            else
                line_to_print += txt;

            return line_to_print;
        }

        void InitializeEvents()
        {
            _view.Amount_KeyUp_Event += new EventHandler<KeyEventArgs>(Amount_KeyUp_Event);
        }

        void Amount_KeyUp_Event(object sender, KeyEventArgs e)
        {
            _view.DebtorSum = _view.payment_list.Sum(pl => pl.amount);
        }

        public async Task CheckChequePaper()
        {
            if (Session.Devices.fiscal != 0)
            {
                string dateTime = await Session.FP550.CheckDateTime(); // Need to call any method to trigger update of device current statuses
                if (!string.IsNullOrEmpty(dateTime))
                {
                    var status = await Session.FP550.CheckStatus((int)Status.CHEQUE_PAPER_IS_ENDING);
                    _view.Info = status.Equals("1") ? "Kasos kvito popierius baiginėjasi!" : string.Empty;
                }
                else
                    _view.Info = string.Empty;
            }
        }

        public async Task PerformPersonalPharmacist() 
        {
            bool disabled = true;
            if (disabled) return;
            if (_view.PoshItem.PosdItems == null || _view.PoshItem.PosdItems.Count == 0) return;
            string eventTypeId = Session.CRMRestUtils.GetEventTypeByName("PersonalPharmacist");
            if (string.IsNullOrEmpty(_view.PoshItem.CRMItem?.Account?.ID) || string.IsNullOrEmpty(eventTypeId)) return;
            foreach (var posdItem in _view.PoshItem.PosdItems.Where(e => e.recipeid != 0 || e.recipeno > 0)) 
            {
                List<Cortex.Client.Model.PropertyRecord> propertyRecords = new List<Cortex.Client.Model.PropertyRecord>();

                Cortex.Client.Model.PropertyRecord propertyRecord = new Cortex.Client.Model.PropertyRecord("p1_billid");
                propertyRecord.PropertyValue = Session.SystemData.kas_client_id + _view.PoshItem.Id.ToString() + "_" + (await DB.Loyalty.getCounter(_view.PoshItem.Id));
                propertyRecords.Add(propertyRecord);

                propertyRecord = new Cortex.Client.Model.PropertyRecord("p1_pharmacyname");
                propertyRecord.PropertyValue = Session.SystemData.name;
                propertyRecords.Add(propertyRecord);

                propertyRecord = new Cortex.Client.Model.PropertyRecord("p1_productname");
                propertyRecord.PropertyValue = posdItem.barcodename;
                propertyRecords.Add(propertyRecord);

                propertyRecord = new Cortex.Client.Model.PropertyRecord("p1_quantity");
                propertyRecord.PropertyValue = posdItem.qty;
                propertyRecords.Add(propertyRecord);

                propertyRecord = new Cortex.Client.Model.PropertyRecord("p1_doctorid");
                propertyRecord.PropertyValue = Session.PractitionerItem.StampCode;
                propertyRecords.Add(propertyRecord);

                propertyRecord = new Cortex.Client.Model.PropertyRecord("p1_doctorname");
                propertyRecord.PropertyValue = Session.PractitionerItem.GivenName.FirstOrDefault() ?? string.Empty;
                propertyRecords.Add(propertyRecord);

                propertyRecord = new Cortex.Client.Model.PropertyRecord("p1_doctorsurename");
                propertyRecord.PropertyValue = Session.PractitionerItem.FamilyName.FirstOrDefault() ?? string.Empty;
                propertyRecords.Add(propertyRecord);

                propertyRecord = new Cortex.Client.Model.PropertyRecord("p1_hospital");
                propertyRecord.PropertyValue = Session.OrganizationItem.Name;
                propertyRecords.Add(propertyRecord);

                propertyRecord = new Cortex.Client.Model.PropertyRecord("p1_duedate");
                propertyRecord.PropertyValue = posdItem.till_date.ToString("yyyy-MM-dd");
                propertyRecords.Add(propertyRecord);

                propertyRecord = new Cortex.Client.Model.PropertyRecord("p1_product_price");
                propertyRecord.PropertyValue = posdItem.sum.ToString();
                propertyRecords.Add(propertyRecord);

                propertyRecord = new Cortex.Client.Model.PropertyRecord("p1_product_id");
                propertyRecord.PropertyValue = posdItem.productid.ToString();
                propertyRecords.Add(propertyRecord);

                propertyRecord = new Cortex.Client.Model.PropertyRecord("total_price");
                propertyRecord.PropertyValue = posdItem.pricediscounted.ToString();
                propertyRecords.Add(propertyRecord);

                await Session.CRMRestUtils.CreateEvent(eventTypeId, _view.PoshItem.CRMItem?.Account?.ID, posdItem.id.ToString(), propertyRecords);
            }
        }

        /// <summary>
        /// In this method do everything what you need to do after successfully payment
        /// </summary>
        public async Task PerformCompletion()
        {
            _completed = true;
            var content = new
            {
                costList = _view.PoshItem.PosdItems.Select(e => e.sum.ToString()),
                documentNumberList = new List<string>() { $"{Session.SystemData.kas_client_id}{_view.PoshItem.Id}" },
                grossPriceList = _view.PoshItem.PosdItems.Select(e => e.price.ToString()),
                interactionTypeList = new List<string>() { intercationType },
                isCampaignItem = _view.PoshItem.PosdItems.Select(e => string.IsNullOrEmpty(e.info)),
                itemIdList = _view.PoshItem.PosdItems.Select(e => e.productid.ToString()),
                itemIdTypeList = new List<string>() { itemIDType },
                lineNumberList = _view.PoshItem.PosdItems.Select(e => e.id.ToString()),
                netPriceList = _view.PoshItem.PosdItems.Select(e => Math.Round(e.price - (e.price * e.vatsize / 100),2, MidpointRounding.AwayFromZero)),
                quantityList = _view.PoshItem.PosdItems.Select(e => e.qty.ToString()),
                recommendationIdList = Session.NBOSession.RecommendationsID.Values,
                sessionIdList = new List<string>() { Session.NBOSession.SessionId },
                locationCodeList = new List<string>() { Session.SystemData.kas_client_id.ToString() }
            };

            Task.Run(() => Task.FromResult(Session.NBOUtils.PostAsync<dynamic>("recommendations/interactions", content))).GetAwaiter();

            Session.NBOSession.ResetSession();
            await _nboRepository.DeleteNBORecommendationByPosHeaderID(_view.PoshItem.Id.ToLong());
            await Program.Display1.TurnOffHomeMode();
            await Program.Display1.TurnOffWoltMode();
            await new PosRepository().UpdatePosMemo(Session.Devices.debtorid, POSMemoParamter.CreateMultipeDispense, string.Empty);
            Session.GruopDispenseRequests.Clear();
        }

        public async Task<PresentCardValidationData> ValidatePresentCard(string cardNumber)
        {
            var presentCardValidationData = new PresentCardValidationData()
            {
                PresentCardId = 0m,
                ValidationMessage = string.Empty,
                Amount = 0m
            };

            var presentCards = await _tamroClient.GetAsync<List<PresentCardViewModel>>
                (string.Format(Session.CKasV1GetPresentCard, helpers.BuildQueryString("CardNumbers=", new List<string> { cardNumber })));
            if (presentCards is null || presentCards.Count == 0)
            {
                presentCardValidationData.ValidationMessage = $"Dovanų kupono nr: {cardNumber} nėra!";
                return presentCardValidationData;
            }

            var presentCard = presentCards.First();

            if (presentCard.Status == PresentCardStatus.New)
            {
                presentCardValidationData.ValidationMessage = $"Dovanų kortelė nr: {cardNumber} yra neišduota ir negali būti būti parduota";
                return presentCardValidationData;
            }

            if (presentCard.Status == PresentCardStatus.Sold)
            {
                presentCardValidationData.ValidationMessage = $"Dovanų kuponas nr: {cardNumber} jau aptarnautas!";
                return presentCardValidationData;
            }

            presentCardValidationData.PresentCardId = presentCard.Id;
            presentCardValidationData.Amount = presentCard.Amount;
            return presentCardValidationData;
        }

        public async Task RecalculateLeftPayValues(bool isRoundingEnabled) 
        {
            var receiptSum = _view.PoshItem.TotalSum;
            var cashSum = _view.payment_list.Sum(pl => pl.id == 0 ? pl.amount : 0);
            var creditSum = _view.payment_list.Sum(pl => pl.id != 0 ? pl.amount : 0);

            if (isRoundingEnabled && !Session.WoltMode)
            {
                _view.CashRoundingCalculation = await Session.FP550.CashPaymentRoundingSimulation(receiptSum, cashSum, creditSum, false);
                if (_view.CashRoundingCalculation.ResponseStatus == Enumerator.PosResponseStatus.OK)
                {
                    var cashPayment = _view.payment_list.FirstOrDefault(pl => pl.id == 0);
                    if (cashPayment != null)
                    {
                        cashPayment.LeftPay = _view.CashRoundingCalculation.CashAmountToFinish;
                    }
                    _view.payment_list.Where(pl => pl.id != 0).ToList().ForEach(pl => pl.LeftPay = _view.CashRoundingCalculation.CreditAmountToFinish);
                }
                else
                {
                    _view.payment_list.ForEach(pl => pl.LeftPay = 0.0m);
                }

                Program.Display2.UpdateData(_view.PoshItem, true, _view.CashRoundingCalculation.RoundingValue);
                _view.CashButton.Enabled = cashSum == 0;
            }
        }

        private void PerformFMDReport(fmd fmdModel)
        {
            var wpf = new wpf.View.FMDAlertReport();
            wpf.DataContext = new FMDAlertReportViewModel(wpf, fmdModel);

            using (Popups.wpf_dlg pfForm = new Popups.wpf_dlg(wpf, "FMD Pranešimas"))
            {
                pfForm.HideControlBox();
                pfForm.SetNotResizeable();
                pfForm.Location = helpers.middleScreen(Program.Display1, pfForm);
                pfForm.Activate();
                pfForm.BringToFront();
                pfForm.ShowDialog();
            }
        }

        private string GetFiscalRank(string ficalRank) 
        {
            return ficalRank == FiscalCredit1Value && Session.getParam("EKA", "NEW") == "1" ?
                FiscalCredit3Value : ficalRank;
        }

        private bool HasRecipe(Items.posd posDetail)
        {
            /*
              recipeid   - paper type prescription
              erecipe_no - e-recipe type
            */

            return posDetail.recipeid != 0 || 
                   posDetail.erecipe_no != 0 ||
                  (posDetail.Saveable && posDetail.NotCompensatedRecipeId != 0);
        }

        public bool IsGoodPresentPayment() 
        {
            return _view.payment_list.Sum(pl => pl.id == GoodPresentPaymentId ? pl.amount : 0) > 0;
        }
    }
}