using AutoMapper;
using ExternalServices.CareCloudREST.Models.CortexModels.Enum;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using POS_display.Display.Views;
using POS_display.Models.CRM;
using POS_display.Models.Loyalty;
using POS_display.Models.NBO;
using POS_display.Models.SalesOrder;
using POS_display.Presenters;
using POS_display.Presenters.Display;
using POS_display.Repository.Discount;
using POS_display.Repository.HomeMode;
using POS_display.Repository.Loyalty;
using POS_display.Repository.NBO;
using POS_display.Repository.PersonalPharmacist;
using POS_display.Repository.Pos;
using POS_display.Repository.Price;
using POS_display.Repository.Recipe;
using POS_display.Repository.SalesOrder;
using POS_display.Utils.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI.WebControls;
using System.Windows.Forms;
using Tamroutilities.Client;
using TamroUtilities.MinIO.Models;
using static POS_display.Enumerator;
using Task = System.Threading.Tasks.Task;

namespace POS_display.Display.Presenters
{
    public class Display1Presenter : BasePresenter, IDisplay1Presenter
    {
        private IDisplay1View _view;
        private Items.posh PoshItem = new Items.posh();
        private readonly RecipeRepository _recipeRepository;
        private readonly SalesOrderRepository _salesOrderRepository;
        private readonly LoyaltyRepository _loyaltyRepository;
        private readonly PriceRepository _priceRepository;
        private readonly PersonalPharmacistRepository _personalPharmacistRepository;
        private readonly NBORepository _nboRepository;
        private readonly PosRepository _posRepository;
        private readonly DiscountRepository _discountRepository;
        private readonly HomeModeRepository _homeModeRepository;
        private readonly object _lock = new object();
        private const string _intercationType = "viewed";
        private const string _itemIDType = "RetailNo";
        private const string _loyaltyTypeDiscount = "D";
        private const string _loyaltyTypeAccruals = "A";
        private const int _doctorCardLength = 37;
        private System.Windows.Forms.Timer _checkRecipeTimer;
        private Enumerator.CRMDataLoadState _crmLoadState;
        private readonly ITamroClient _tamroClient;
        private readonly IMapper _mapper;

        public Display1Presenter(IDisplay1View view, ITamroClient tamroClient, IMapper mapper)
        {
            _view = view;
            InitializeEvents();
            _tamroClient = tamroClient ?? throw new ArgumentNullException();
            _mapper = mapper ?? throw new ArgumentNullException();
            _recipeRepository = new RecipeRepository();
            _salesOrderRepository = new SalesOrderRepository();
            _loyaltyRepository = new LoyaltyRepository();
            _priceRepository = new PriceRepository();
            _personalPharmacistRepository = new PersonalPharmacistRepository();
            _nboRepository = new NBORepository();
            _posRepository = new PosRepository();
            _discountRepository = new DiscountRepository();
            _homeModeRepository = new HomeModeRepository();

            _checkRecipeTimer = new System.Windows.Forms.Timer();
            int checkRecipeTimerDelay = Convert.ToInt32(Session.getParam("CHECKRECIPE", "TIMERDELAY")) * 1000;
            if (checkRecipeTimerDelay != 0)
            {
                _checkRecipeTimer.Interval = checkRecipeTimerDelay;
                _checkRecipeTimer.Tick += CheckRecipeTimer_Tick;
                _checkRecipeTimer.Start();
            }
            _crmLoadState = Enumerator.CRMDataLoadState.None;
        }

        public bool IsDoctorCard(string cardNo) 
        {
            return 
                cardNo.IndexOf("=") > 0 && 
                cardNo.Length == _doctorCardLength &&
                cardNo.StartsWith("9");
        }

        public async Task RefreshPoshAsync()
        {
            _view.currentPosdRow = null;
            Items.posh posh_old = PoshItem;
            var flags = CollectFlags();
            PoshItem = new Items.posh();
            DataTable data = await DB.POS.GetPosh();
            if (data.Rows.Count > 0)
            {
                PoshItem.Id = data.Rows[0]["id"].ToDecimal();
                PoshItem.CheckNo = data.Rows[0]["checkno"].ToString();
                PoshItem.DiscountSum = data.Rows[0]["DiscountSum"].ToDecimal();
            }
            else
                PoshItem.Id = await DB.POS.CreatePosh();

            if (PoshItem.Id <= 0)
                throw new Exception("Negautas arba nesukurtas kvitas! Patikrinkite praeito kvito numerį \nKasa - Operacijos - Funkcijos - Pakeisti blogą kvitą į sekantį");
            Items.Loyalty.loyaltyh loyalty_item = (await DB.Loyalty.GetLoyaltyh<Items.Loyalty.loyaltyh>(PoshItem.Id))
                .DefaultIfEmpty(new Items.Loyalty.loyaltyh()
                {
                    card_type = posh_old == PoshItem ? posh_old.LoyaltyCardType : "NOLC",
                    card_no = "",
                    manual_vouchers = "",
                    accrue_points = 0
                }).First();

            PoshItem.CRMItem = new CRMData
            {
                ManualVouchers = new List<ManualVoucher>()
            };

            if (loyalty_item.manual_vouchers != "")
            {
                string[] vouchers_array = loyalty_item.manual_vouchers.Split(';');
                for (int i = 0; i < vouchers_array.Length; i++)
                    PoshItem.CRMItem.ManualVouchers.Add(new ManualVoucher() { Code = vouchers_array[i] });             
            }

            if (loyalty_item.card_type == "BENU")
                await Task.Run(async () => PoshItem.CRMItem.Account = await Session.CRMRestUtils.CollectClientData(loyalty_item.card_no));
            else
                PoshItem.CRMItem.Account = new CRMClientData() { CardNumber = loyalty_item.card_no };

            PoshItem.LoyaltyCardType = loyalty_item.card_type;
            PoshItem.CRMItem.AccruePoints = (int)loyalty_item.accrue_points;

            await DB.POS.update_posh_date(PoshItem.Id);
            await RefreshPosdAsync();
            PoshItem.RimiDiscAmount = await GetRimiDiscountAmount();
            _view.PoshItem = PoshItem;//update view
            await _view.BindGrid();
            if (Session.Params.FirstOrDefault(x => x.system == "FMD" && x.par == "ENABLED")?.value == "1")
            {
                foreach (var pd in PoshItem?.PosdItems ?? new List<Items.posd>())
                {
                    Program.Display1.ExecuteAsyncAction(async () =>
                    {
                        var models = (await _posRepository.GetFMDItem(pd.hid.ToLong(), pd.id.ToLong()));
                        if (models.Count <= 0)
                            return;
                        models.ForEach(e => e.posDetail = pd);
                        pd.fmd_model.AddRange(models);
                        var vm = new wpf.ViewModel.FMDposd()
                        {
                            CurrentPosdRow = pd
                        };
                        foreach (var m in pd.fmd_model.Where(w => w.type == "verify" && string.IsNullOrWhiteSpace(w.Response.operationCode)))
                        {
                            var response_model = await vm.VerifySinglePack(m);
                            m.Response = response_model.Response;
                        }
                        Program.Display1.BeginInvoke(new Action(() =>
                        {
                            _view.BindGrid();
                        }));
                    });
                }
            }
            AppendFlags(flags);

            Task.Run(LoadRecommendations).GetAwaiter();
        }

        private void AppendFlags(Dictionary<decimal, Enumerator.ProductFlag> flags) 
        {
            if (PoshItem.PosdItems == null) return;
            foreach (var posDetail in PoshItem.PosdItems) 
            {
                if (flags.ContainsKey(posDetail.id))
                    posDetail.Flags = flags[posDetail.id];
            }
        }

        private Dictionary<decimal, Enumerator.ProductFlag> CollectFlags()
        {
            Dictionary<decimal, Enumerator.ProductFlag> flags = new Dictionary<decimal, Enumerator.ProductFlag>();
            if (PoshItem.PosdItems == null)
                return flags;
            foreach (var posDetail in PoshItem.PosdItems)
            {
                if (!flags.ContainsKey(posDetail.id))
                    flags[posDetail.id] = posDetail.Flags;
            }
            return flags;
        }

        private async Task<decimal> GetRimiDiscountAmount() 
        {
            if (Session.getParam("RIMIDISCAMOUNT", "FIXED") == "1")
            {
                return await _posRepository.CalculateRimiDiscount(PoshItem.Id);
            }
            else
            {
                return await DB.POS.CalcRimiDisc(PoshItem.Id);
            }
        }

        private async Task HandleLoyalty()
        {
            if (Session.CRM)
            {
                await _loyaltyRepository.DeleteLoyaltyDetailsByPosHeaderIdAndTypes(
                    PoshItem.Id, 
                    new List<string> 
                    {
                        _loyaltyTypeDiscount,
                        _loyaltyTypeAccruals
                    });

                PoshItem.PosdItems = await _posRepository.GetPosDetails(PoshItem.Id);

                // Delete non manual discounts
                //
                foreach (var posDetail in PoshItem.PosdItems.Where(e => e.loyalty_type != "P"))
                {
                    await _discountRepository.CreateDiscount(
                        posDetail.hid,
                        posDetail.id,
                        (decimal)DiscountType.Cumulative,
                        (decimal)SumType.Perecent,
                        0,
                        string.Empty);
                }

                PoshItem.PosdItems = await _posRepository.GetPosDetails(PoshItem.Id);

                if (PoshItem.PosdItems.Any(w => w.Type == "POSD")) //dont send empty bill to CRM
                {
                    PoshItem.CRMItem.AcceptedPaymentResponse = await Session.CRMRestUtils.AcceptPayment(PoshItem);
                    List<CRMDiscountItem> discountItems = new List<CRMDiscountItem>();

                    // Discounts
                    //
                    if (PoshItem.CRMItem.AcceptedPaymentResponse.Data.RecommendedDiscounts != null)
                    {
                        foreach (var discountItem in PoshItem.CRMItem.AcceptedPaymentResponse.Data.RecommendedDiscounts)
                        {
                            if (discountItem.DiscountValue.ToDecimal() == 0) continue;
                            var posDetailId = Session.CRMRestUtils.GetId(discountItem.BillItemId);
                            await _loyaltyRepository.CreateOrUpdateLoyaltyDetail(PoshItem.Id,
                                posDetailId.ToDecimal(),
                                _loyaltyTypeDiscount,
                                SumType.Perecent,
                                discountItem.DiscountPercent.ToDecimal(),
                                discountItem.DiscountCode);
                            discountItems.Add(_mapper.Map<CRMDiscountItem>(discountItem));
                        }
                    }

                    // Vouchers
                    //
                    var codeLenghtRestrict = Session.getParam("VOUCHERCODE", "LENGTHRESTRICT") == "1";
                    if (PoshItem.CRMItem.AcceptedPaymentResponse.Data.Vouchers != null)
                    {
                        foreach (var paymentVoucher in PoshItem.CRMItem.AcceptedPaymentResponse.Data.Vouchers)
                        {
                            if (paymentVoucher.DiscountValue.ToDecimal() == 0) continue;
                            var voucherCode = codeLenghtRestrict ? (paymentVoucher.Code.Length > 20 ? paymentVoucher.Code.Substring(0, 20) : paymentVoucher.Code) : paymentVoucher.Code;
                            var posDetailId = Session.CRMRestUtils.GetId(paymentVoucher.BillItemId);
                            await _loyaltyRepository.CreateOrUpdateLoyaltyDetail(PoshItem.Id,
                                posDetailId.ToDecimal(),
                                _loyaltyTypeDiscount,
                                SumType.Perecent,
                                paymentVoucher.DiscountPercent.ToDecimal(),
                                voucherCode);
                            discountItems.Add(_mapper.Map<CRMDiscountItem>(paymentVoucher));
                        }
                    }

                    // Accrual Points
                    //
                    if (PoshItem.CRMItem.AcceptedPaymentResponse.Data.CreditPointsOfThisTransaction > 0)
                    {
                        PoshItem.PosdItems = await _posRepository.GetPosDetails(PoshItem.Id);
                        var totalSum = PoshItem.PosdItems.Where(AcceptableCreditPoints).Sum(pd => pd.sum);
                        var totalAccruals = Math.Round((decimal)PoshItem.CRMItem.AcceptedPaymentResponse.Data.CreditPointsOfThisTransaction, 4, MidpointRounding.AwayFromZero);
                        var countAccruals = PoshItem.PosdItems.Count(AcceptableCreditPoints);
                        var restAccruals = totalAccruals;
                        var i = 0;

                        foreach (var posDetailItem in PoshItem.PosdItems.Where(AcceptableCreditPoints))
                        {
                            i++;
                            var lineAccruals = Math.Round(totalAccruals * posDetailItem.sum / totalSum, 2, MidpointRounding.AwayFromZero);
                            await _loyaltyRepository.CreateOrUpdateLoyaltyDetail(posDetailItem.hid,
                                posDetailItem.id,
                                _loyaltyTypeAccruals,
                                SumType.Value,
                                i < countAccruals ? lineAccruals : restAccruals,
                                "accruals");
                            restAccruals -= lineAccruals;
                        }
                    }
     
                    var groupedDiscounts = discountItems
                         .GroupBy(d => d.BillItemId)
                         .Select(group =>
                         {
                             return new
                             {
                                 BillItemId = group.Key,
                                 FinalDiscountValue = group.Sum(e => e.DiscountValue),
                                 Code = group.First().DiscountCode
                             };
                         });

                    // Apply discounts
                    //
                    foreach (var discount in groupedDiscounts)
                    {
                        var posDetail = PoshItem.PosdItems.FirstOrDefault(e => e.id == Session.CRMRestUtils.GetId(discount.BillItemId).ToDecimal());
                        if (posDetail == null) continue;
                        await _discountRepository.CreateDiscount(
                            posDetail.hid,
                            posDetail.id,
                            (decimal)DiscountType.Cumulative,
                            (decimal)SumType.Perecent,
                            GetDiscountPercentValue(posDetail.sum, discount.FinalDiscountValue),
                            discount.Code);
                    }
                }
            }
        }

        private decimal GetDiscountPercentValue(decimal sum, decimal discountValue)
        {
            if (sum <= 0)            
                return 0;            

            return Math.Round((discountValue / sum) * 100, 2);
        }

        private async Task RefreshPosdAsync()
        {
            await HandleLoyalty();
            PoshItem.PosdItems = await _posRepository.GetPosDetails(PoshItem.Id);

            if (PoshItem.InsuranceItem == null)
            {
                var dtInsurance = await DB.cheque.GetInsuranceData(PoshItem.Id);
                if (dtInsurance.Rows.Count > 0)
                {
                    var chequeFrom = dtInsurance.Rows[0]["cheque_from"].ToString();
                    var cardNo = dtInsurance.Rows[0]["card_no"].ToString();
                    PoshItem.InsuranceItem = new Items.Insurance(chequeFrom, cardNo)
                    {
                        CardSessionId = dtInsurance.Rows[0]["info"].ToString(),
                        ConfirmedTransaction = dtInsurance.AsEnumerable().Any(a => a.Field<int>("status").Equals(10))
                    };
                    if (!PoshItem.InsuranceItem.ConfirmedTransaction)
                        PoshItem.InsuranceItem = await PoshItem.InsuranceItem.Utils.CalcInsuranceCompensation(PoshItem);
                    PoshItem.PosdItems = await _posRepository.GetPosDetails(PoshItem.Id);
                }
            }
        }

        public async Task CancelPoshAsync()
        {
            using (var paymentView = new PaymentView(_view.PoshItem))
            {
                await paymentView.VoidPayment();                
                await DB.POS.AsyncCancelPosh(_view.PoshItem.Id);
                foreach (var pd in PoshItem.PosdItems)
                    await DeletePosdAsync(pd, true);

                await CancelPosh3rdParties();
                await CancelMultipleDispense();
            }
        }

        public async Task CancelMultipleDispense() 
        {
            await new PosRepository().UpdatePosMemo(Session.Devices.debtorid, POSMemoParamter.CreateMultipeDispense, string.Empty);
            Session.GruopDispenseRequests.Clear();
        }

        public async Task<decimal> DeletePosdAsync(Items.posd posdItem, bool isCancelPosh = false)
        {
            if (isCancelPosh == false && PoshItem?.InsuranceItem?.ConfirmedTransaction == true && posdItem.status_insurance == 10)
                throw new Exception("Neįmanoma panaikinti eilutės, nes draudimo kompensacija jau yra patvirtinta.\nJei norite panaikinti kvitą, spauskite mygtuką Trinti kvitą.");
            #region CancelPosd3rdParties
            var status_reason = "POS: Panaikintas kvitas";
            if (posdItem.erecipe_no != 0 && posdItem.erecipe_active == 1)
            {
                decimal composition_id = await DB.eRecipe.GetErecipeAsync(posdItem.id);
                if (composition_id <= 0)
                    throw new Exception("Nepavyko nuskaityti elektroninio recepto duomenų!");
                if (!await Session.eRecipeUtils.CancelRecipeDispense(composition_id.ToString(), status_reason, null))
                    throw new Exception("Nepavyko ištrinti elektroninio recepto!");
                if (!await DB.eRecipe.MarkErecipeAsync(posdItem.id, posdItem.erecipe_no, posdItem.recipeid, status_reason))
                    throw new Exception("Nepavyko pažymėti kaip neaktyvaus elektroninio recepto duomenų bazėje!");
                await Session.eRecipeUtils.RetryLockRecipe(posdItem.erecipe_no.ToString(), false, posdItem.eRecipeDispenseBySubstancesGroupId);

                Session.GruopDispenseRequests.RemoveAll(e => e.eRecipeId == composition_id);
                await new PosRepository().UpdatePosMemo(Session.Devices.debtorid, POSMemoParamter.CreateMultipeDispense, JsonConvert.SerializeObject(Session.GruopDispenseRequests));
            }
            if (posdItem.recipeid != 0 && posdItem.tlk_status == 1)
            {
                var kvapRecipeId = await DB.recipe.asyncGetKVAPrecipe(posdItem.recipeid);
                await DB.recipe.UpdateTLKStatus(posdItem.recipeid, 0);
                var deleteRecipe = await Session.KVAP.DeleteRecipe(kvapRecipeId.ToString());

                if (deleteRecipe.Id > 0)//jei gaunam ištrinto rec id is tlk
                {
                    if (!await DB.recipe.asyncDeleteKVAPrecipe(deleteRecipe.Id))
                        throw new Exception("Nepavyko pakeisti recepto statuso duomenų bazėje!");
                }
                else
                {
                    string error = "";
                    foreach (var el in deleteRecipe.RecipeErrors)
                    {
                        error += el.Name + "\n\n";
                        error += el.Notes + "\n";
                        error += "-----------------------------------------------------------------------------------------\n";
                    }
                    throw new Exception("Nepavyko ištrinti recepto iš TLK!\n" + error);
                }
            }

            if (posdItem.NotCompensatedRecipeId > 0) 
            {
                await _recipeRepository.DeleteNotCompensatedRecipe(posdItem.hid, posdItem.id);
            }

            if (Session.ExtendedPracticePractitioner != null)
            {
                if (posdItem.VaccineDispensationCompositionId != 0)
                {
                    await Session.eRecipeUtils.CancelVaccinationDispensation<dynamic>(
                           posdItem.VaccineDispensationCompositionId.ToString(),
                           status_reason);
                }

                if (posdItem.VaccinePrescriptionCompositionId != 0)
                {
                    await Session.eRecipeUtils.CancelVaccinationPrescription<dynamic>(
                           posdItem.VaccinePrescriptionCompositionId.ToString(),
                           status_reason);
                }
            }
           
            var FMDitems = PoshItem.fmd_models.Where(w => w.posDetail?.id == posdItem.id);
            foreach (var f in FMDitems.Where(w => w.type == "supply" && w.deleted == false && w.Response?.Success == true))
            {
                var vm = new wpf.ViewModel.FMDposd()
                {
                    CurrentPosdRow = posdItem
                };
                await vm.ChangeStateSinglePackAsync(f, FMD.Model.State.Active);
            }

            await _homeModeRepository.DeleteHomeDeliveryDetail(PoshItem.Id, posdItem.id);

            #endregion
            decimal deleted = -1;
            if (posdItem.Type == "POSD")
                deleted = await DB.POS.AsyncDeletePosd(PoshItem.Id, posdItem.id);
            else if (posdItem.Type == "ADVANCEPAYMENT")
                deleted = await _posRepository.DeleteAdvancePayment(posdItem.id);
            if (deleted <= 0)
                throw new Exception("Šio įrašo trinti negalima!");
            if (PoshItem.PosdItems.Count == 1 && isCancelPosh == false)
                await CancelPosh3rdParties();

            return deleted;
        }

        public async Task<decimal> GetQtyInRemotePharamcy(decimal productID)
        {
            var result = await Session.RemotePharmacyGateway.PostCallApi<dynamic>(new
            {
                method = "getqty",
                productid = productID.ToString(CultureInfo.InvariantCulture)
            }.ToJsonString());
            return result != null ? result.quantity : 0;
        }

        public async Task<decimal> GetQty(long productID)
        {
            return await _priceRepository.GetProductQty(productID) ?? 0;
        }

        public async Task LoadNBORecommendationIDs()
        {
            Session.NBOSession.RecommendationsID = await _nboRepository.LoadNBORecommendationsByPosHeaderID(PoshItem.Id.ToLong());
        }

        public async Task LoadPersonalPharmacist()
        {
            Session.PersonalPharmacistData = await _personalPharmacistRepository.GetPersonalPharmacistData(PoshItem.Id.ToLong());
            _view.PersonalPharmacistButton.ForeColor = Session.PersonalPharmacistData != null
                ? System.Drawing.Color.Green
                : System.Drawing.Color.Black;
        }

        public async Task LoadRecommendations()
        {
            var recommendations = new List<NBORecommendation>();
            if (_crmLoadState != Enumerator.CRMDataLoadState.Success || !Session.NBOSession.NeedRefresh())
                return;
            bool isLoyaltyCard = !string.IsNullOrEmpty(PoshItem?.CRMItem?.Account?.CardNumber);
            try
            {
                _view.LoaderControl.IsLoading = true;
                if (PoshItem?.PosdItems?.Count == 0 && !isLoyaltyCard) return;
                var posDetailItem = PoshItem?.PosdItems?.FirstOrDefault();
                var basketItems = PoshItem?.PosdItems
                    .ConvertAll(e => e.productid.ToString(CultureInfo.InvariantCulture));

                var content = new Dictionary<string, string>()
                {
                    { "scenario",  Session.Develop ? Session.getParam("NBO_TEST", "SCENARIO") : Session.getParam("NBO", "SCENARIO") },
                    { "customerId", Session.NBOSession.CustomerId },
                    { "itemId", posDetailItem != null ? posDetailItem.productid.ToString(CultureInfo.InvariantCulture) : string.Empty},
                    { "explain", false.ToString() },
                    { "count", Session.Develop ? Session.getParam("NBO_TEST", "COUNT") : Session.getParam("NBO", "COUNT")},
                    { "basket", string.Join(",",basketItems) },
                    { "locationCode", Session.SystemData.kas_client_id.ToString() },
                    { "sessionId", Session.NBOSession.SessionId },
                    { "method", isLoyaltyCard ? "personal" : "bought_together" }
                };

                var encodedUrl = $"recommendations?{string.Join("&", content.Select(kvp => $"{HttpUtility.UrlEncode(kvp.Key)}={HttpUtility.UrlEncode(kvp.Value)}"))}";
                var result = await Session.NBOUtils.GetAsync<NBORecommendationResponse>(encodedUrl);

                if (result != null)
                {
                    recommendations = result.Recommendations;
                    await CollectRecommendationsAdditionalData(recommendations);
                    recommendations = recommendations.Where(e => FilterByStock(e) && 
                                                                 FilterByPictureExistence(e)).Take(3).ToList();
                    lock (_lock)
                    {
                        if (posDetailItem != null)
                        {
                            if (!Session.NBOSession.RecommendationsID.ContainsKey(posDetailItem.id.ToLong()))
                                Session.NBOSession.RecommendationsID.Add(posDetailItem.id.ToLong(), result.Id);
                            else
                                Session.NBOSession.RecommendationsID[posDetailItem.id.ToLong()] = result.Id;
                        }
                    }

                    if (posDetailItem != null)
                    {
                        await _nboRepository.DeleteNBORecommendationByPosDetailID(posDetailItem.id.ToLong());
                        await _nboRepository.InsertNBORecommendation(PoshItem.Id.ToLong(),
                                                                 posDetailItem.id.ToLong(),
                                                                 result.Id);
                    }

                    await PerformViewedRecommendationsInteraction(recommendations);
                }
            }
            catch (Exception ex)
            {
                Serilogger.GetLogger().Error(ex, ex.Message);
            }
            finally
            {
                if (Session.NBOSession.NeedRefresh())
                {
                    DetermineRecommendationHeaders(recommendations);
                    _view
                        .RecomendationsGridView
                        .Invoke((Action)(() => _view
                            .RecomendationsGridView
                            .DataSource = recommendations));

                    if (Session.getParam("NBO", "LOADPICTURES") == "1")
                        Program.Display2.LoadRecommendations(recommendations);
                    Session.NBOSession.Refresh();
                }
                _view.LoaderControl.IsLoading = false;
            }
        }

        public async Task LoadCRMData()
        {
            if (Session.getParam("CRM", "LOADDATA") == "0")
            {
                SetCRMDataLoadStatus(Enumerator.CRMDataLoadState.None);
                return;
            }

            SetCRMDataLoadStatus(Enumerator.CRMDataLoadState.Loading);

            try
            {
                await LoadCRMCampaigns();
                SetCRMDataLoadStatus(Enumerator.CRMDataLoadState.Success);
            }
            catch (Exception ex) 
            {
                Serilogger.GetLogger().Error(ex, ex.Message);
                SetCRMDataLoadStatus(Enumerator.CRMDataLoadState.Error);
            }
        }

        public async Task CancelSalesOrder(Items.posh poshItem)
        {
            var data = await Session.RemotePharmacyGateway.PostCallApi<List<SalesOrderClientRef>>(new
            {
                method = "getclientrefbyposhid",
                poshid = poshItem.Id.ToString(CultureInfo.InvariantCulture)
            }.ToJsonString());

            foreach (var socr in poshItem.PosdItems.Select(posdItem =>
                data?.FirstOrDefault(e => e.PoshID == posdItem.hid)))
            {
                if (socr == null) continue;
                await _salesOrderRepository.DeleteTransferData(socr.ExternalHID);
                await Session.RemotePharmacyGateway.PostCallApi<List<SalesOrderClientRef>>(new
                {
                    method = "deletedocbyposdid",
                    posdid = socr.PosdID.ToString(CultureInfo.InvariantCulture)
                }.ToJsonString());
            }
        }

        public async Task LoadUserPrioritesRatio()
        {
            bool isEnabledUserPrioritesRatio = Convert.ToInt32(Session.getParam("PRIORITIES_RATIO", "ENABLED")) == 1;
            _view.UserPrioritiesRatio.Visible = isEnabledUserPrioritesRatio;
            if (isEnabledUserPrioritesRatio)
            {
                DateTime now = DateTime.Now;
                var firstDayOfMonth = new DateTime(now.Year, now.Month, 1);
                var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);

                decimal ratio = await _posRepository.GetUserPrioritiesRationInPeriod(Session.User.id.ToLong(), firstDayOfMonth, lastDayOfMonth);
                _view.UserPrioritiesRatio.Text = $"Jūsų šio mėnesio prioritetinių prekių pardavimo santykis: {Math.Round(ratio, 2)} %";
            }
        }

        public async Task LoadMedictionActiveSubstances()
        {
            Session.ActiveSubstances = await _tamroClient.GetAsync<List<string>>(Session.TransactionV1GetMedicationProductsActiveSubstance);
        }

        public async Task LoadMedictionNames()
        {
            Session.MedicationNames = await _tamroClient.GetAsync<List<string>>(Session.TransactionV1GetMedicationProductsMedicationName);
        }

        public async Task LoadWoltProductIds()
        {
            Session.WoltProducts = await _tamroClient.GetAsync<List<long>>(string.Format(Session.ItemV1GetSalesChannelsItemBindings,"LT","Wolt"));
        }

        public async Task SetHomeMode(bool apply) 
        {
            try
            {
                Session.HomeMode = apply;
                await _posRepository.UpdatePosMemo(
                    Session.Devices.debtorid,
                    POSMemoParamter.HomeMode,
                    Session.HomeMode ? "1" : "0");

                ((Form)_view).SuspendLayout();
                if (Session.HomeMode)
                {
                    ((Form)_view).BackColor = System.Drawing.Color.LightGreen;
                    ((Form)_view).Text = "Kasos pardavimo operacija v" + Session.AssemblyVersion + " ***** PREKĖS BUS PRISTATOMOS KLIENTUI Į NAMUS !!! *****";
                    _view.HomeModeButton.Enabled = false;
                    _view.WoltModeButton.Enabled = false;
                }
                else
                {
                    ((Form)_view).BackColor = System.Drawing.SystemColors.Control;
                    ((Form)_view).Text = "Kasos pardavimo operacija v" + Session.AssemblyVersion;
                    _view.HomeModeButton.Enabled = true;
                    _view.WoltModeButton.Enabled = true;
                }
            }
            finally
            {
                ((Form)_view).ResumeLayout(false);
                ((Form)_view).PerformLayout();
            }
        }

        public async Task SetWoltMode(bool apply)
        {
            try
            {
                var WoltCrmEnabled = Session.getParam("WOLT", "CRM") == "1";
                if (Session.HomeMode)
                    return;

                Session.WoltMode = apply;
                await _posRepository.UpdatePosMemo(
                    Session.Devices.debtorid,
                    POSMemoParamter.Wolt,
                    Session.WoltMode ? "1" : "0");

                ((Form)_view).SuspendLayout();
                if (Session.WoltMode)
                {
                    ((Form)_view).BackColor = System.Drawing.Color.LightBlue;
                    ((Form)_view).Text = "Kasos pardavimo operacija v" + Session.AssemblyVersion + " ***** PREKĖS PARDUODAMOS PER WOLT!!! *****";
                    _view.WoltModeButton.Enabled = false;
                    _view.HomeModeButton.Enabled = false;
                    if (!WoltCrmEnabled)
                        Session.CRM = false;
                }
                else
                {
                    ((Form)_view).BackColor = System.Drawing.SystemColors.Control;
                    ((Form)_view).Text = "Kasos pardavimo operacija v" + Session.AssemblyVersion;
                    _view.WoltModeButton.Enabled = true;
                    _view.HomeModeButton.Enabled = true;
                    if (!WoltCrmEnabled)
                        Session.CRM = true;
                }
            }
            finally
            {
                ((Form)_view).ResumeLayout(false);
                ((Form)_view).PerformLayout();
            }
        }

        public async Task CancelHomeDeliveryorder(Items.posh poshItem)
        {
            await _homeModeRepository.DeleteHomeDeliveryOrder(poshItem.Id);
        }

        #region Private methods

        private async Task LoadCRMCampaigns()
        {
            int maxAmountPerRequest = 1000;
            DateTime today = DateTime.Now;
            GetCampaignItemsRequest request = new GetCampaignItemsRequest
            {
                Company = Session.getParam("CRM", "COMPANYCODE"),
                DateFrom = new DateTime(today.Year, today.Month, today.Day, 0, 0, 0),
                DateTo = new DateTime(today.Year, today.Month, today.Day, 23, 59, 59),
                ClientType = new List<string>()
                    {
                        Enumerator.CRMCustomerType.nonLC.ToString(),
                        Enumerator.CRMCustomerType.LC.ToString(),
                        Enumerator.CRMCustomerType.RIMI.ToString()
                    },
                SystemId = Session.SystemData.kas_client_id.ToString(),
                Count = maxAmountPerRequest,
                Offset = 0
            };

            lock (_lock)
            {
                Session.CRMCampaginsCache.Clear();
            }

            var currentPage = 0;
            var totalPages = 1;
            while (currentPage < totalPages)
            {
                var campaignItem = await GetCampaignItemsList(request);
                foreach (var crmCampaignItem in campaignItem?.CampaignItemsList)
                {
                    lock (_lock)
                    {
                        if (!Session.CRMCampaginsCache.ContainsKey(crmCampaignItem.LocalItemCode.ToLong()))
                        {
                            Session.CRMCampaginsCache.Add(crmCampaignItem.LocalItemCode.ToLong(),
                                new List<CampaignItemsList> { crmCampaignItem });
                        }
                        else
                        {
                            Session.CRMCampaginsCache[crmCampaignItem.LocalItemCode.ToLong()].Add(crmCampaignItem);
                        }
                    }
                }

                var total = campaignItem?.TotalItems ?? 0;
                totalPages = Math.Ceiling((decimal)total / (decimal)maxAmountPerRequest).ToInt();
                currentPage++;
                request.Offset = currentPage * maxAmountPerRequest;
            }
        }

        private async Task<CampaignItem> GetCampaignItemsList(GetCampaignItemsRequest request)
        {
            var jsonRequest = JObject.Parse(JsonConvert.SerializeObject(request));
            return await Session.TamroGateway.PostAsync<CampaignItem>("api/v2/crm/getcampaignitemslist", jsonRequest);
        }

        private void SetCRMDataLoadStatus(Enumerator.CRMDataLoadState state)
        {
            string crmLabelStatusText = "CRM Duomenys:";
            _crmLoadState = state;
            switch (state)
            {
                case Enumerator.CRMDataLoadState.None:
                    {
                        _view.CRMDataLoadStatus
                            .Invoke((MethodInvoker)(() => _view.CRMDataLoadStatus.Visible = false));
                        _view.CRMDataReload
                            .Invoke((MethodInvoker)(() => _view.CRMDataReload.Visible = false));
                        break;
                    }
                case Enumerator.CRMDataLoadState.Loading:
                    {
                        _view.CRMDataLoadStatus
                            .Invoke((MethodInvoker)(() => _view.CRMDataLoadStatus.Text = $"{crmLabelStatusText} Kraunasi..."));
                        _view.CRMDataLoadStatus
                            .Invoke((MethodInvoker)(() => _view.CRMDataLoadStatus.ForeColor = System.Drawing.Color.Black));
                        _view.CRMDataReload
                            .Invoke((MethodInvoker)(() => _view.CRMDataReload.Enabled = false));
                        break;
                    }
                case Enumerator.CRMDataLoadState.Success:
                    {
                        _view.CRMDataLoadStatus
                            .Invoke((MethodInvoker)(() => _view.CRMDataLoadStatus.Text = $"{crmLabelStatusText} Sėkmingai užkrauti"));
                        _view.CRMDataLoadStatus
                            .Invoke((MethodInvoker)(() => _view.CRMDataLoadStatus.ForeColor = System.Drawing.Color.Green));
                        _view.CRMDataReload
                            .Invoke((MethodInvoker)(() => _view.CRMDataReload.Enabled = true));
                        break;
                    }
                case Enumerator.CRMDataLoadState.Error:
                    {
                        _view.CRMDataLoadStatus
                            .Invoke((MethodInvoker)(() => _view.CRMDataLoadStatus.Text = $"{crmLabelStatusText} Nepavyko užkrauti"));
                        _view.CRMDataLoadStatus
                            .Invoke((MethodInvoker)(() => _view.CRMDataLoadStatus.ForeColor = System.Drawing.Color.Red));
                        _view.CRMDataReload
                            .Invoke((MethodInvoker)(() => _view.CRMDataReload.Enabled = true));
                        break;
                    }
            }
        }

        private async void CheckRecipeTimer_Tick(object sender, EventArgs e)
        {
            if (await _recipeRepository.HasUnsignedUkraineRefugeeRecipes(Session.User.id.ToLong()))
            {
                helpers.alert(Enumerator.alert.error, "Yra nepasirašytų receptų, išduotų Ukrainos piliečiams.\nPRIVALOTE PASIRAŠYTI!");
                Serilogger.GetLogger().Information($"User ID: {Session.User.id.ToLong()} has unsigned ukrainian reffuge recipes");
            }
        }

        private bool FilterByStock(NBORecommendation recommendation)
        {
            return Session.getParam("NBO", "FILTERBYSTOCK") == "0" ? true : !recommendation.OutOfStock;
        }

        private bool FilterByPictureExistence(NBORecommendation recommendation)
        {
            return Session.getParam("NBO", "LOADPICTURES") == "0" ? true : recommendation.HasPicture;
        }

        private async Task PerformViewedRecommendationsInteraction(List<NBORecommendation> recommendations) 
        {
            if (recommendations == null) return;
            var content = new
            {
                interactionTypeList = new List<string>() { _intercationType },
                itemIdList = recommendations.Select(e => e.RetailNo),
                itemIdTypeList = new List<string>() { _itemIDType },
                recommendationIdList = Session.NBOSession.RecommendationsID.Values,
                sessionIdList = new List<string>() { Session.NBOSession.SessionId },
                locationCodeList = new List<string>() { Session.SystemData.kas_client_id.ToString() }
            };
            Serilogger.GetLogger().Information($"[PerformViewedRecommendationsInteraction] Pharmacy ID: {Session.SystemData.kas_client_id}; Content: {content?.ToJsonString()}");
            await Session.NBOUtils.PostAsync<dynamic>("recommendations/interactions", content);
        }

        private async Task CancelPosh3rdParties()
        {
            Session.FeedbackTerminal.ExecuteAction<Models.FeedbackTerminal.BaseRequest>(Models.FeedbackTerminal.RequestName.CustomerCancel);
            if (PoshItem.InsuranceItem != null && PoshItem.InsuranceItem.CardSessionId != "")
            {
                if (PoshItem.InsuranceItem.ConfirmedTransaction)
                    await _view.PoshItem.InsuranceItem.Utils.VoidInsuranceCompensation(_view.PoshItem);
                else
                    await PoshItem.InsuranceItem.Utils.CancelInsuranceCompensation(PoshItem);
            }
            await InvokePersonalPharmacist();
            await CancelNBORecommendations();
        }

        private async Task InvokePersonalPharmacist()
        {
            await _personalPharmacistRepository.DeletePersonalPharmacistByPoshID(PoshItem.Id.ToLong());
            Session.PersonalPharmacistData = null;
            _view.PersonalPharmacistButton.ForeColor = System.Drawing.Color.Black;
        }

        private async Task CancelNBORecommendations()
        {
            Session.NBOSession.ResetSession();
            await _nboRepository.DeleteNBORecommendationByPosHeaderID(PoshItem.Id.ToLong());
        }

        private async Task CollectRecommendationsAdditionalData(List<NBORecommendation> recommendations)
        {
            if (recommendations == null) return;
            foreach (var rec in recommendations)
            {
                try
                {
                    var productId = rec.RetailNo.ToLong();
                    rec.Price = Math.Round(await _priceRepository.GetSalesPrice(productId), 2);
                    var qty = await _priceRepository.GetProductQty(productId);
                    rec.OutOfStock = qty == null || qty == 0;
                    rec.ProductGr4 = await PosRepository.GetProductGr4(productId);
                    rec.Picture = Session.getParam("NBO", "LOADPICTURES") == "1" ? await Session.MinioStorage.Get(new BaseFileRequest
                    {
                        Path = $"{productId}_LT.png"
                    }) : null;
                    CalculateRecommendationItemDiscount(rec);
                }
                catch (Exception ex) 
                {
                    Serilogger.GetLogger().Error(ex, ex.Message);
                }
            }
        }

        private void DetermineRecommendationHeaders(List<NBORecommendation> recommendations)
        {
            if (recommendations == null || recommendations.Count == 0) return;

            recommendations[0].HeaderText = recommendations[0]
                .ProductGr4?
                .IndexOf("otc", StringComparison.InvariantCultureIgnoreCase) >= 0 ? "AKTUALU" : "KITI KARTU PIRKO";

            if (recommendations.Count > 1)
            {
                recommendations[1].HeaderText = recommendations[1]
                    .ProductGr4?
                    .IndexOf("otc", StringComparison.InvariantCultureIgnoreCase) >= 0 ? "AKTUALU" : "KITI KARTU PIRKO";
            }
        }

        private void CalculateRecommendationItemDiscount(NBORecommendation recommendation)
        {
            if (Session.CRMCampaginsCache.Count == 0)
                return;

            if (!Session.CRMCampaginsCache.ContainsKey(recommendation.RetailNo.ToLong()))
                return;

            if (recommendation.ProductGr4?.IndexOf("otc", StringComparison.InvariantCultureIgnoreCase) >= 0)
                return;

            Enumerator.CRMCustomerType customerType = GetActualCustomerDiscountType();
            List<CampaignItemsList> campaignItems = Session.CRMCampaginsCache[recommendation.RetailNo.ToLong()];

            CampaignItemsList crmCampaignItem = campaignItems.FirstOrDefault(e => e.ClientType == customerType);
            if (crmCampaignItem == null)
                return;

            Enum.TryParse(crmCampaignItem?.ValueTypeId, out ValueTypeId valueTypeId);
            switch (valueTypeId)
            {
                case ValueTypeId.PercentageDiscount:
                    {
                        recommendation.OldPrice = recommendation.Price;
                        recommendation.Discount = crmCampaignItem.Value;
                        recommendation.Price = Math.Round(recommendation.Price - ((recommendation.Price * crmCampaignItem.Value) / 100), 2);
                        break;
                    }
                case ValueTypeId.FinalPrice:
                    {
                        recommendation.OldPrice = recommendation.Price;
                        recommendation.Discount = Math.Round(100 - ((crmCampaignItem.Value * 100) / recommendation.Price), 0);
                        recommendation.Price = crmCampaignItem.Value;
                        break;
                    }
            }
        }

        private Enumerator.CRMCustomerType GetActualCustomerDiscountType()
        {
            switch (_view.PoshItem?.LoyaltyCardType ?? string.Empty) 
            {
                case "BENU":
                    return Enumerator.CRMCustomerType.LC;
                case "RIMI":
                    return Enumerator.CRMCustomerType.RIMI;
                default:
                    return Enumerator.CRMCustomerType.nonLC;
            }
        }

        #endregion

        #region Events
        void InitializeEvents()
        {
            _view.btnPosdFMD_Event += new EventHandler(btnPosdFMD_Event);
            _view.btnPosdFMDdlg_Event += new EventHandler(btnPosdFMDdlg_Event);
            _view.CurrentCellChanged_Event += new EventHandler(CurrentCellChanged_Event);
        }

        void btnPosdFMD_Event(object sender, EventArgs e)
        {
            if (Session.Params.FirstOrDefault(x => x.system == "FMD" && x.par == "ENABLED")?.value != "1")
                return;
            var vm = new wpf.ViewModel.FMDposd()
            {
                CurrentPosdRow = _view.currentPosdRow
            };
            var wpf = new wpf.View.FMDposd()
            {
                DataContext = vm
            };
            using (var dlg = new POS_display.Popups.wpf_dlg(wpf, "FMD eilutės langas"))
            {
                dlg.Location = helpers.middleScreen(sender as System.Windows.Forms.Form, dlg);
                dlg.ShowDialog();
            }
            _view.currentPosdRow = vm.CurrentPosdRow;
            _view.BindGrid();
        }

        void btnPosdFMDdlg_Event(object sender, EventArgs e)
        {
            if (Session.Params.FirstOrDefault(x => x.system == "FMD" && x.par == "ENABLED")?.value != "1")
                return;
            var vm = new wpf.ViewModel.FMDdlg();
            var wpf = new wpf.View.FMDdlg()
            {
                DataContext = vm
            };
            using (var dlg = new POS_display.Popups.wpf_dlg(wpf, "FMD statuso keitimo langas"))
            {
                dlg.Location = helpers.middleScreen(sender as System.Windows.Forms.Form, dlg);
                dlg.ShowDialog();
            }
        }

        void CurrentCellChanged_Event(object sender, EventArgs e)
        {
            System.Windows.Forms.DataGridView dgv = sender as System.Windows.Forms.DataGridView;
            if (dgv?.CurrentRow != null)
            {
                var current_id = dgv.CurrentRow?.Cells["id"]?.Value?.ToDecimal() ?? -1;
                _view.currentPosdRow = PoshItem?.PosdItems?.FirstOrDefault(f => f.id == current_id) ?? new Items.posd();
            }
        }
        #endregion

        #region Predicates
        private readonly Func<Items.posd, bool> AcceptableCreditPoints = pd =>
        {
            bool discountLoyaltyCondition = Session.getParam("DISCOUNTLOYALTY", "INVOLVE") == "1";
            if (discountLoyaltyCondition) 
            {
                return pd.recipeid == 0 && !new[] { "rx", "g", "vac" }.Any(prefix => pd.gr4.StartsWith(prefix, StringComparison.InvariantCultureIgnoreCase));

            }
            return pd.recipeid == 0 && pd.discount_sum == 0 &&
            !new[] { "rx", "g", "vac" }.Any(prefix => pd.gr4.StartsWith(prefix, StringComparison.InvariantCultureIgnoreCase));
        };
        #endregion
    }

}
