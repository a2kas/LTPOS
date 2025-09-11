using AutoMapper;
using Cortex.Client.Api;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using POS_display.Display.Presenters;
using POS_display.Display.Views;
using POS_display.Helpers;
using POS_display.Models;
using POS_display.Models.Loyalty;
using POS_display.Models.NBO;
using POS_display.Properties;
using POS_display.Utils;
using POS_display.Utils.Logging;
using POS_display.Views;
using POS_display.Views.AdvancePayment;
using POS_display.Views.CRM;
using POS_display.Views.HomeDelivery;
using POS_display.Views.NarcoticAlert;
using POS_display.Views.PersonalPharmacist;
using POS_display.Views.SalesOrder;
using POS_display.Views.Vouchers;
using POS_display.wpf.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Deployment.Application;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Tamroutilities.Client;
using TransactionService.Models.TransModel;

namespace POS_display
{
    public partial class Display1View : FormBase, IDisplay1View, IBarcodeView
    {
        #region Variables
        public event EventHandler btnPosdFMD_Event;
        public event EventHandler btnPosdFMDdlg_Event;
        public event EventHandler CurrentCellChanged_Event;
        private readonly Display1Presenter _display1Presenter;

        static int currentPageIndex = 0;
        static int lastPageIndex = 0;
        public string form_action = "";
        private decimal sysTimerCounter = 0;

        private bool _IsBusy;
        private decimal WoltPackingFeeId = 10000121328;
        private string WoltLoyaltyCardNumber;
        #endregion
        public Display1View()
        {
            InitializeComponent();
            _display1Presenter = new Display1Presenter(
                this,
                Program.ServiceProvider.GetRequiredService<ITamroClient>(),
                Program.ServiceProvider.GetRequiredService<IMapper>());
            WoltLoyaltyCardNumber = Session.getParam("WOLT", "LOYALTYCARD");
        }

        #region Methods
        public async Task TurnOffHomeMode()
        {
            await ExecuteWithWaitAsync(async () => await _display1Presenter.SetHomeMode(false), false);
        }

        public async Task TurnOffWoltMode()
        {
            await ExecuteWithWaitAsync(async () => await _display1Presenter.SetWoltMode(false), false);
        }

        public async Task RefreshPosh()
        {
             await ExecuteWithWaitAsync(async () =>
            {
                currentPageIndex = 1;
                await _display1Presenter.RefreshPoshAsync();

                form_action = "";
                edtBarcode.Focus();
                edtBarcode.SelectAll();
            }, false);
        }
        private async void ShowInfo2()
        {
            lblInfo2.Text = "";
            LblMarketingConsent.Text = "";
            PoshItem.UsedCardNo = (await DB.POS.GetRimiCardNo(PoshItem.Id)).ToDecimal();
            if (PoshItem.UsedCardNo > 0 || PoshItem.LoyaltyCardType == "RIMI")
                lblInfo2.Text = "RIMI Kortelė";

            decimal employee = await DB.POS.GetEmployeePos(PoshItem.Id);
            if (employee > 0)
            {
                lblInfo2.Text = "Pardavimas darbuotojui";
                btnDiscount.Enabled = false;
                btnCheque.Enabled = false;
                btnPrep.Enabled = false;
            }

            decimal doctor = await DB.POS.GetDoctorPos(PoshItem.Id);
            if (doctor > 0)
                lblInfo2.Text = "BENU gydytojo kortelė";

            if (PoshItem.LoyaltyCardType == "BENU")
            {
                if (PoshItem.CRMItem.Pensioner == true)
                    lblInfo2.Text = "BENU senjoro kortelė";
                else
                    lblInfo2.Text = "BENU lojalumo kortelė";

                LblMarketingConsent.Font = new Font("Microsoft Sans Serif", 10, FontStyle.Bold);
                if (PoshItem.CRMItem.Account.AgreementMarketingCommunication == 1)
                {
                    LblMarketingConsent.Text = "Gautas sutikimas komunikacijai";
                    LblMarketingConsent.ForeColor = Color.FromArgb(0, 38, 100);
                }
                else
                {
                    LblMarketingConsent.Text = "Negautas sutikimas komunikacijai";
                    LblMarketingConsent.ForeColor = Color.Red;
                }

                if (PoshItem.CRMItem.Account.PersonalPharmacistClient)
                {
                    LblMarketingConsent.Text = "Asmeninio vaistininko projekto dalyvis";
                    LblMarketingConsent.ForeColor = Color.Black;
                }
            }

            if (PoshItem.LoyaltyCardType == "BENU0S")
                lblInfo2.Text = "BENU vaistinės kortelė";

            if (PoshItem.InsuranceItem?.Company == "SAM" && PoshItem.InsuranceItem?.CardNoLong != string.Empty)
                lblInfo2.Text = "Ukrainos karo pabėgelis";
        }
        private void ShowDisplay2()
        {
            if (Display2Visible)
            {
                Program.Display2.Show();
                Program.Display2.PoshItem = PoshItem;
            }
            else            
                Program.Display2.Hide();            
        }
        public async Task SubmitBarcode(Barcode barcodeModel)
        {
            if (barcodeModel.BarcodeStr.Equals(""))
                return;
            IsBusy = true;
            BarcodeString = barcodeModel.BarcodeStr;
            #region CRM
            if (Session.CRM)
            {
                if (barcodeModel.BarcodeStr.StartsWith("BENU01") || barcodeModel.BarcodeStr.StartsWith("BENUS") || barcodeModel.BarcodeStr.StartsWith("BENU02"))//benu
                {
                    var clientData = await Session.CRMRestUtils.CollectClientData(barcodeModel.BarcodeStr);
                    if (clientData.IsCardActive && PoshItem?.CRMItem != null)
                    {
                        PoshItem.CRMItem.Account = clientData;
                        if (PoshItem?.CRMItem?.Account != null && PoshItem?.CRMItem?.Account?.CardNumber != "")
                        {
                            await DB.Loyalty.createLoyaltyh(PoshItem.Id, "BENU", barcodeModel.BarcodeStr, 1, 0, 1);
                            DisplayFeedbackInfo();
                        }
                        barcodeModel.PosdId = 6;
                        var content = new
                        {
                            customerIDList = new List<string>() { Session.NBOSession.CustomerId, PoshItem?.CRMItem?.Account?.CardNumber },
                            customerIdTypeList = new List<string>() { Session.NBOSession.CustomerIdType, "CardNo" }
                        };
                        Task.Run(async () => await Session
                            .NBOUtils
                            .PatchAsync<Response<NBORecommendationResponse>>("recommendations/interactions", content))
                            .GetAwaiter();

                        Session.NBOSession.AssignSessionValues(PoshItem?.CRMItem?.Account?.CardNumber);
                    }
                    else
                    {
                        helpers.alert(Enumerator.alert.error, "BENU kortelė yra blokuota arba negaliojanti ir negali būti pritaikyta!");
                        return;
                    }
                }
                else if (barcodeModel.BarcodeStr.Contains("9440385200"))//rimi
                {
                    string rimi_card = barcodeModel.BarcodeStr;
                    if (barcodeModel.BarcodeStr.IndexOf('=') > 0)
                        rimi_card = barcodeModel.BarcodeStr.Substring(0, barcodeModel.BarcodeStr.IndexOf('='));
                    if (rimi_card.Length == 19 && rimi_card.All(char.IsDigit))
                    {
                        await DB.Loyalty.createLoyaltyh(PoshItem.Id, "RIMI", rimi_card, 1, 0, 0);
                        barcodeModel.PosdId = await DB.POS.CreatePosdPos(PoshItem.Id, barcodeModel.BarcodeStr, 0, 0);
                    }
                    else
                    {
                        Serilogger.GetLogger().Information($"Failed to read RIMI card. Card No: {rimi_card}");
                        helpers.alert(Enumerator.alert.error, "Nepavyko nuskaityti RIMI kortelės, pabandykite per naują");
                        return;
                    }
                }
                else if (barcodeModel.BarcodeStr.StartsWith("BENU0S"))//vaistines
                {
                    await DB.Loyalty.createLoyaltyh(PoshItem.Id, "BENU0S", barcodeModel.BarcodeStr, 1, 0, 0);
                    barcodeModel.PosdId = 6;
                }
                else if (barcodeModel.BarcodeStr.StartsWith("BENU0H"))//hemofilija
                {
                    await DB.Loyalty.createLoyaltyh(PoshItem.Id, "BENU", barcodeModel.BarcodeStr, 1, 0, 0);
                    barcodeModel.PosdId = 6;
                }
                else if (barcodeModel.BarcodeStr.StartsWith("BENU0IP"))//integrali pagalba
                {
                    await DB.Loyalty.createLoyaltyh(PoshItem.Id, "BENU", barcodeModel.BarcodeStr, 1, 0, 0);
                    barcodeModel.PosdId = 6;
                }
                else if (barcodeModel.BarcodeStr.StartsWith("BENUM"))//mokejimo
                {
                    await DB.Loyalty.createLoyaltyh(PoshItem.Id, "BENU", barcodeModel.BarcodeStr, 1, 0, 0);
                    barcodeModel.PosdId = 6;
                }
                else if (_display1Presenter.IsDoctorCard(barcodeModel.BarcodeStr)) 
                {
                    var doctorCardExpire = Session.getParam("DOCTORCARD", "EXPIRE");
                    if (DateTime.TryParse(doctorCardExpire, out var date) && date <= DateTime.Now) 
                    {
                        helpers.alert(Enumerator.alert.warning, "Gydytojo kortelė nebegalioja. Išduokite naują kortelę.");
                        return;
                    }
                }
                else//NON LC
                {
                    if (PoshItem.CRMItem.Account.CardNumber == "")
                        await DB.Loyalty.createLoyaltyh(PoshItem.Id, "NOLC", "", 1, 0, 0);
                }

                if (barcodeModel.BarcodeStr.StartsWith("PTV"))
                {
                    if (PoshItem?.PosdItems == null || PoshItem.PosdItems.Count == 0) 
                    {
                        IsBusy = false;
                        helpers.alert(Enumerator.alert.error, "Kuponas negali būti taikomas, nes nėra jokių prekių!");
                        return;
                    }
                    var recommendedBestRewards = await Session.CRMRestUtils.RecommendedBestRewards(PoshItem, "C");
                    var voucherBC = recommendedBestRewards?.Data?.RecommendedBestRewards?.Where(vl => vl.Code == barcodeModel.BarcodeStr);
                    if (voucherBC != null && voucherBC.Any())
                    {
                        PoshItem.ManualVouchers.Add(new ManualVoucher()
                        {
                            Selected = 1,
                            Code = voucherBC.First().Code,
                            MaxCount = voucherBC.First().MaxCount,
                            Name = voucherBC.First().Name,
                            Qty = voucherBC.First().MaxCount,
                            RewardPriority = (int)voucherBC.First().RewardPriority
                        });

                        string vouchers_array = string.Empty;
                        foreach (var el in PoshItem.ManualVouchers.Where(vl => vl.Selected == 1))
                        {
                            for (int i = 0; i < el.Qty; i++)
                            {
                                vouchers_array += el.Code + ';';
                            }
                        }
                        await DB.Loyalty.setManualVouchers(PoshItem.Id, vouchers_array.TrimEnd(';'));
                        await RefreshPosh();
                    }
                    else
                    {
                        IsBusy = false;
                        helpers.alert(Enumerator.alert.error, "Neteisingas akcijos kodas.");
                    }
                    return;
                }
            }
            #endregion
            if (PoshItem?.PosdItems?.Count == 0 && string.IsNullOrWhiteSpace(PoshItem?.CRMItem?.Account?.CardNumber))
                DisplayFeedbackInfo();
            var isEshopCode = barcodeModel.BarcodeStr.StartsWith("B1P");
            if (Session.Develop && !isEshopCode)
                isEshopCode = barcodeModel.BarcodeStr.StartsWith("T1P");

            if (isEshopCode)
            {
                if (PoshItem.PosdItems.Count > 0)
                {
                    helpers.alert(Enumerator.alert.error, "Atliekant užsakymo mokėjimą, krepšelyje negali būti kitų prekių.");
                    return;
                }
                var result = await Session.EShopGateway.GetAsync<Response<BecomeTransactionAmount>>($"/api/v1/transaction/becometransaction/getbecometransactionamountforpaymentbyclientorderno?ClientOrderNo={BarcodeString}&StoreId={Session.SystemData.kas_client_id}");
                if (result == null)
                {
                    helpers.alert(Enumerator.alert.info, "Nepavyko gauti informacijos apie uzsakyma");
                    return;
                }
                if (result?.baseResponse != null && result.baseResponse.IsSuccess)
                {
                    await ExecuteWithWaitAsync(async () =>
                    {
                        barcodeModel.PosdId = await DB.POS.CreateAdvancePayment(PoshItem.Id, "ESHOP", barcodeModel.BarcodeStr, result.result.LeftAmount.ToDecimal());
                        if (barcodeModel.PosdId > 0)
                            this.DialogResult = DialogResult.OK;
                        else
                        {
                            helpers.alert(Enumerator.alert.error, "Klaida atliekant avansinį mokėjimą");
                            return;
                        }
                    }, false);
                }
                else
                {
                    switch (result?.baseResponse?.Information)
                    {
                        case "Order does not have minimum required status":
                            helpers.alert(Enumerator.alert.info, "Užsakymas dar nepriėjo \"Supakuotas\" statuso");
                            break;
                        case "Order already paid for":
                            helpers.alert(Enumerator.alert.info, "Užsakymas jau apmokėtas");
                            break;
                        case "Order not found":
                            helpers.alert(Enumerator.alert.info, "Užsakymas nerastas");
                            break;
                        case "Order has different packing pharmacy":
                            helpers.alert(Enumerator.alert.info, "Užsakymas registruotas kitai vaistinei");
                            break;
                        case null:
                            break;
                        default:
                            helpers.alert(Enumerator.alert.info, result?.baseResponse?.Information);
                            break;
                    }
                    return;
                }
            }

            if ((barcodeModel?.PosdId ?? 0) == 0)
            {
                Presenters.BarcodePresenter BC_presenter = new Presenters.BarcodePresenter(this, barcodeModel);
                await BC_presenter.GetDataFromBarcode();
                await BC_presenter.ScanBarcode();
            }

            if ((barcodeModel?.HasPriceChange ?? false) && Session.getParam("PRIC","NOTIFYCHANGES") == "1")
                helpers.alert(Enumerator.alert.warning, "Keitėsi prekės pirkimo kaina! \n" +
                    "Sena kaina pirktam likučiui automatiškai apskaičiuota nauja pardavimo kaina.");

            if (barcodeModel.IsSalesOrderProduct &&
                helpers.alert(Enumerator.alert.confirm, "Ar norite prekę ar jos dalis parduoti į namus?", true))
            {
                var productQty = await _display1Presenter.GetQty(barcodeModel.ProductId.ToLong());
                string inputQty = "";
                if (DialogResult.OK == helpers.ShowInputDialogBox(ref inputQty, $"Kokį kiekį norite parduoti į namus? Likutis vaistinėje: {productQty}", "Prekės kiekis"))
                {
                    if (int.TryParse(inputQty, out int qty))
                    {
                        if (qty >= 1)
                        {
                            var remotePharmacyQty = await _display1Presenter.GetQtyInRemotePharamcy(barcodeModel.ProductId);
                            if (qty <= remotePharmacyQty)
                            {
                                using (var salesOrderView = new SalesOrderView(barcodeModel, PoshItem))
                                {
                                    salesOrderView.QtyToTransfer = qty;
                                    if (salesOrderView.ShowDialog() == DialogResult.OK)
                                    {
                                        helpers.alert(Enumerator.alert.info,
                                            "Prėkes perkėlimas į vaistinę sėkmingai atliktas");
                                        if (barcodeModel.PosdId.ToString().Length != Session.IDLength)
                                        {
                                            barcodeModel.PosdId = await DB.POS.CreatePosdPos(PoshItem.Id, barcodeModel.BarcodeStr, barcodeModel.Mode, 0);
                                            await salesOrderView.UpdateCurrentSalesOrderPosDetail(barcodeModel.PosdId.ToLong());
                                        }
                                    }
                                }
                            }
                            else
                                helpers.alert(Enumerator.alert.warning,
                                    "Prekė negali būti parduota į namus!\n" +
                                    "Nepakankamas likutis nutolusioje vaistinėje atlikti pardavimo užsakymą!\n" +
                                    "Bus atliekamas įprastas prekės pardavimas");
                        }
                    } 
                    else                    
                        helpers.alert(Enumerator.alert.warning,"Įvestas klaidingas prekės kiekis!\n" +
                            "Kiekis privalo būti sveikasis skaičius ");
                    
                }
            }

            //todo
            var error = Session.POSErrors.Where(e => e.system == "create_posd_pos" && e.code == barcodeModel.PosdId.ToString());
            if (error.Count() > 0)
                helpers.alert(error.First());
            else
                Session.NBOSession.InitRefresh();

            switch (barcodeModel.PosdId.ToString())
            {
                case "30":
                    if (PoshItem.LoyaltyCardType == "")
                    {
                        PoshItem.LoyaltyCardType = "BENU";
                        ShowInfo2();
                    }
                    edtBarcode.Text = "";
                    break;
                case "32":
                    ShowInfo2();
                    edtBarcode.Text = "";
                    break;
                default:
                    edtBarcode.Text = "";
                    break;
            }
            if (barcodeModel.PosdId != 0 && barcodeModel.PosdId != 2)
            {
                form_action = "edtBarcode";
                if (barcodeModel.PosdId == 20)
                    form_action = "sut_drb";
                if (barcodeModel.PosdId == 21)
                    form_action = "pan_drb";
            }

            await RefreshPosh();
            edtBarcode.SelectAll();

            var lastItem = PoshItem.PosdItems?.FirstOrDefault();
            if (lastItem != null
                && !string.IsNullOrEmpty(lastItem.note2)
                && barcodeModel.PosdId.ToString().Length == Session.IDLength)
            {
                Enumerator.DrugType drugType;
                if (Enum.TryParse(lastItem.note2, out drugType))
                {
                    if (drugType == Enumerator.DrugType.NARC || drugType == Enumerator.DrugType.PSYC)
                    {
                        using (var narcoticAlertView = new NarcoticAlertView(drugType, lastItem.atc))
                        {
                            narcoticAlertView.ShowDialog();
                        }
                    }
                }
            }
            if ((lastItem?.Saveable ?? false) && barcodeModel.EnteredByMainFieldInDisplay1 
                && barcodeModel.PosdId.ToString().Length == Session.IDLength) 
            {
                btnPrescriptionCheck.PerformClick();
            }
        }
        private string GetDeleteConfirmationMessage(string question)
        {
            var confirmMessage = question;
            if (currentPosdRow?.erecipe_no != 0 && currentPosdRow?.erecipe_active == 1)
                confirmMessage += "\nKartu bus panaikintas elektroninis receptas;";
            if (currentPosdRow?.recipeid != 0 && currentPosdRow?.tlk_status == 1)
                confirmMessage += "\nKartu bus atšauktas receptas iš TLK;";
            return confirmMessage;
        }
        public async Task BindGrid()
        {
            if (PoshItem?.PosdItems?.Count<Items.posd>() > 0)
            {
                int startIndex = (currentPageIndex - 1) * Settings.Default.grid_page_size;
                int endIndex = (currentPageIndex - 1) * Settings.Default.grid_page_size + Settings.Default.grid_page_size;
                if (endIndex > PoshItem.PosdItems.Count<Items.posd>())
                    endIndex = PoshItem.PosdItems.Count<Items.posd>();
                gvPosd.DataSource = (from el in PoshItem.PosdItems.Skip(startIndex).Take(endIndex)
                                     select el).ToList();
                lastPageIndex = (int)Math.Ceiling(Convert.ToDecimal(PoshItem.PosdItems.Count<Items.posd>()) / Settings.Default.grid_page_size);
                lblRecordsStatus.Text = currentPageIndex + " / " + lastPageIndex;
                SumGrid();
            }
            else
            {
                gvPosd.DataSource = new List<Items.posd>();
                lblRecordsStatus.Text = currentPageIndex + " / " + currentPageIndex;
                currentPageIndex = 0;
                lblInfo2.Text = "";
                LblMarketingConsent.Text = "";
            }
            Program.Display2.PoshItem = PoshItem;
            ShowInfo2();
            EnableButtons();
            EnableNavigation();
            GridView_order();
        }
        private void GridView_order()
        {
            string[] array = new string[16]
            {
                "btnDeleteLine",
                "fmd_link",
                "apply_insurance",
                "have_recipe",
                "symbol",
				"Saveable",
                //"fmd_is_valid_for_sale",
                "vatsize",
                //"basic_price",
                "barcode",
                "barcodename_info",
                "qty",
                "price",
                "discount",
                "pricediscounted",
                "sum",
                "cheque_sum",
                "cheque_sum_insurance"
            };
            //show columns
            for (int i = 0; i < array.Count(); i++)
                gvPosd.Columns[array[i]].DisplayIndex = i;

            //hide columns
            foreach (DataGridViewColumn dc in gvPosd.Columns)
            {
                if (!array.Contains(dc.DataPropertyName))
                    dc.Visible = false;
                if (dc.DataPropertyName == "fmd_link" && Session.Params.FirstOrDefault(x => x.system == "FMD" && x.par == "ENABLED")?.value != "1")
                    dc.Visible = false;
            }
            if (PoshItem.InsuranceItem != null)
            {
                gvPosd.Columns["apply_insurance"].Visible = true;
                gvPosd.Columns["have_recipe"].Visible = true;
                gvPosd.Columns["cheque_sum_insurance"].Visible = true;
            }
            else
            {
                gvPosd.Columns["apply_insurance"].Visible = false;
                gvPosd.Columns["have_recipe"].Visible = false;
                gvPosd.Columns["cheque_sum_insurance"].Visible = false;
            }
        }
        public void SumGrid()
        {
            decimal TotalSum = 0;
            decimal ChequeSum = 0;
            decimal InsuranceSum = 0;
            decimal RecipeId = 0;
            foreach (DataGridViewRow dr in gvPosd.Rows)
            {
                var posd_item = PoshItem.PosdItems.FirstOrDefault(f => f.id == dr.Cells["id"].Value.ToDecimal());
                TotalSum += Convert.ToDecimal(posd_item.sum);
                ChequeSum += Convert.ToDecimal(posd_item.cheque_sum);
                InsuranceSum += Convert.ToDecimal(posd_item.cheque_sum_insurance);
                RecipeId = Convert.ToDecimal(posd_item.recipeid);
                if (RecipeId > 0 || posd_item.erecipe_no > 0 || posd_item.NotCompensatedRecipeId > 0)
                {
                    dr.DefaultCellStyle.ForeColor = Color.LimeGreen;
                    dr.DefaultCellStyle.SelectionForeColor = Color.Lime;
                    if (posd_item.erecipe_no > 0 && posd_item.erecipe_active != 1 && string.IsNullOrEmpty(posd_item.eRecipeDispenseBySubstancesGroupId))
                        dr.DefaultCellStyle.Font = new Font(this.Font, FontStyle.Strikeout);
                }
                if (posd_item.fmd_required && !posd_item.IsValidToSellFMD)
                {
                    dr.Cells["fmd_link"].Style.BackColor = Color.Red;
                    dr.Cells["fmd_link"].Style.SelectionBackColor = Color.Red;
                }
            }
            tbCheckValue.Text = (PoshItem.TotalSum + PoshItem.ChequeSum + PoshItem.ChequeInsuranceSum + PoshItem.CompensatedSum).ToString();
            tbTotalSum.Text = TotalSum.ToString();
            RestSum = (-1 * (PoshItem.TotalSum + PoshItem.ChequeSum + PoshItem.ChequeInsuranceSum)).ToString();
            tbChequeSum.Text = ChequeSum.ToString();
            tbInsuranceSum.Text = InsuranceSum.ToString();
        }
        private void EnableButtons()
        {
            bool is_fiscal = Session.Devices.fiscal != 0;
            bool isAdvancePayment = PoshItem?.PosdItems?.Any(a => a.Type == "ADVANCEPAYMENT") ?? false;

            if (PoshItem.PosdItems != null && currentPosdRow != null && PoshItem.PosdItems.Count<Items.posd>() > 0 && !isAdvancePayment)
            {
                bool is_depozit = currentPosdRow.is_deposit;
                bool is_erecipe = currentPosdRow.erecipe_no != 0;
                btnPrescriptionCheck.Enabled = !is_depozit;
                btnRecipe.Enabled = (is_erecipe || is_depozit) ? false : true;
                btnDiscount.Enabled = (is_erecipe && currentPosdRow.compensationsum != 0) || is_depozit ? false : true;
                btnPrice.Enabled = (is_erecipe || is_depozit) ? false : true;
                btnCheque.Enabled = !is_depozit;
                btnPrep.Enabled = !is_depozit;
                btnCancel.Enabled = true;
                btnEcrRep.Enabled = false;
                btnResetPos.Enabled = is_fiscal;
                btnX.Enabled = is_fiscal;
                btnCard.Enabled = is_fiscal;
                btnInsurance.Enabled = PoshItem.InsuranceItem == null ? true : false;
                btnLabel.Enabled = true;
                btnVouchers.Enabled = true;
                //if (Session.Params.FirstOrDefault(x => x.system == "FMD" && x.par == "ENABLED")?.value == "1")//TM: nestabdom pardavimo, nepriklausomai nuo statuso keitimo
                //    btnPayment.Enabled = !PoshItem.fmd_models.Any(a => (a.state != null && a.state_enum != FMD.Model.State.Active) || !string.IsNullOrWhiteSpace(a.alertId));
            }
            else
            {
                btnRecipe.Enabled = false;
                btnPrescriptionCheck.Enabled = false;
                btnDiscount.Enabled = false;
                btnCheque.Enabled = false;
                btnPrep.Enabled = false;
                btnPrice.Enabled = false;
                //btnCancel.Enabled = false;
                btnResetPos.Enabled = is_fiscal;
                btnEcrRep.Enabled = true;//is_fiscal;
                btnCard.Enabled = is_fiscal;
                btnX.Enabled = is_fiscal;
                btnInsurance.Enabled = false;
                btnLabel.Enabled = false;
                btnVouchers.Enabled = false;
                lblInfo.Text = "";
                lblProductName.Text = "";
                lblInitName.Text = "";
                tbDiscPercent.Text = "";
                tbVatSize.Text = "";
                tbQty.Text = "";
                tbPrice.Text = "";
                tbSum.Text = "";
                tbRecipeNo.Text = "";
                tbCompCode.Text = "";
                tbCompPercent.Text = "";
                tbRecTotalSum.Text = "";
                tbRecCompSum.Text = "";
                tbPaySum.Text = "";
                tbEndSum.Text = "";
                tbTotalSum.Text = "";
                tbChequeSum.Text = "";
                tbInsuranceSum.Text = "";
                tbCheckValue.Text = "";
                tbOnHandQty.Text = "";
            }
            edtBarcode.Enabled = !isAdvancePayment;
            btnSelBarcode.Enabled = !isAdvancePayment;
            btnService.Enabled = !isAdvancePayment;
            btnAdvancePayment.Enabled = PoshItem?.PosdItems?.Count(val => val.Type != "ADVANCEPAYMENT") == 0;
            btnHomeMode.Enabled = !Session.HomeMode && !Session.WoltMode && (PoshItem?.PosdItems?.Where(e => e.barcodeid != 0).ToList().Count ?? 0) == 0;
            btnWoltMode.Enabled = !Session.WoltMode && !Session.HomeMode && (PoshItem?.PosdItems?.Where(e => e.barcodeid != 0).ToList().Count ?? 0) == 0;
            btnERecipe.Enabled = false;
            if (!isAdvancePayment && !string.IsNullOrWhiteSpace(Session.PractitionerItem?.PractitionerId) && !String.IsNullOrWhiteSpace(Session.OrganizationItem?.OrganizationId))
                btnERecipe.Enabled = true;
            btnDonation.Enabled = PoshItem.LoyaltyCardType.StartsWith("BENU");
        }
        private void EnableNavigation()
        {
            if (currentPageIndex == (int)Math.Ceiling(Convert.ToDecimal(PoshItem?.PosdItems?.Count<Items.posd>()) / Settings.Default.grid_page_size))
            {
                btnNext.Enabled = false;
                btnLast.Enabled = false;
            }
            else
            {
                btnNext.Enabled = true;
                btnLast.Enabled = true;
            }

            if (currentPageIndex <= 1)
            {
                btnPrevious.Enabled = false;
                btnFirst.Enabled = false;
            }
            else
            {
                btnPrevious.Enabled = true;
                btnFirst.Enabled = true;
            }
        }
 
        public async Task AddInsuranceCard(string insurance_code, string card_no)
        {
            await ExecuteWithWaitAsync(async () =>
            {
                PoshItem.InsuranceItem = new Items.Insurance(insurance_code, card_no);
                PoshItem.InsuranceItem.CardSessionId = PoshItem.InsuranceItem.Utils.AuthoriseInsuranceCard(PoshItem.InsuranceItem);
                if (PoshItem.InsuranceItem != null)
                {
                    foreach (var el in PoshItem.PosdItems)
                    {
                        if (el.status_insurance == 0)
                            await DB.cheque.CreateChequeTrans(el.id, 0, PoshItem.InsuranceItem.Company, PoshItem.InsuranceItem.CardSessionId, PoshItem.InsuranceItem.CardNoLong, null, 11, "");
                        else
                            await DB.cheque.UpdateChequeTrans(el.id, 0, PoshItem.InsuranceItem.Company, PoshItem.InsuranceItem.CardSessionId, PoshItem.InsuranceItem.CardNoLong, el.status_insurance, "");
                    }
                }
            }, false);
        }
        private void DisplayFeedbackInfo()
        {
            if (!string.IsNullOrWhiteSpace(PoshItem?.CRMItem?.Account?.CardNumber))
            {
                var requestModel = new Models.FeedbackTerminal.CustomerAccountRequest
                {
                    CardNumber = PoshItem.CRMItem.Account.CardNumber
                };
                Session.FeedbackTerminal.ExecuteAction(Models.FeedbackTerminal.RequestName.CustomerAccount, requestModel);
            }
            else
                Session.FeedbackTerminal.ExecuteAction<Models.FeedbackTerminal.BaseRequest>(Models.FeedbackTerminal.RequestName.CustomerWelcome);
        }

        private string SolveOnHandQuantity(Items.posd currentPosdRow)
        {
            if (currentPosdRow.OnHandQty <= 0m)
                return "0";

            decimal barcodeRatio = currentPosdRow.BarcodeRatio >= 0 ? currentPosdRow.BarcodeRatio : 1;
            return Math.Round(currentPosdRow.OnHandQty / barcodeRatio, 2).ToString("0.##");
        }
        #endregion

        #region Properties
        public int display2_timer
        {
            get
            {
                int time = 0;
                string txt = btnDrugPrice.Text;
                if (txt.IndexOf('(') > 0)
                {
                    txt = txt.Substring(txt.IndexOf('(') + 1);
                    txt = txt.Substring(0, txt.IndexOf(')'));
                    int.TryParse(txt, out time);
                }
                return time;
            }
            set
            {
                if (value <= 0)
                {
                    btnDrugPrice.Text = "&Vaistų kaina";
                    btnDrugPrice.Enabled = true;
                    lblDisplay2.Text = "";
                }
                else
                {
                    btnDrugPrice.Text = "&Vaistų kaina(" + value + ")";
                    btnDrugPrice.Enabled = false;
                    lblDisplay2.Text = "Blokuojamas receptinių vaistų pardavimas " + value;
                }
            }
        }

        public string RestSum
        {
            get
            {
                return tbRestSum.Text;
            }
            set
            {
                tbRestSum.Text = value;
            }
        }

        public string BarcodeString
        {
            get
            {
                return edtBarcode.Text;
            }

            set
            {
                edtBarcode.Text = value;
            }
        }

        private Items.posh _PoshItem = new Items.posh();
        public Items.posh PoshItem
        {
            get
            {
                return _PoshItem;
            }

            set
            {
                _PoshItem = value;
            }
        }

        private Items.posd _currentPosdRow;
        public Items.posd currentPosdRow
        {
            get
            {
                return _currentPosdRow;
            }
            set
            {
                _currentPosdRow = value;
                if (_currentPosdRow == null)
                    return;
                lblInfo.Text = _currentPosdRow.info;
                lblProductName.Text = _currentPosdRow.barcodename;
                lblInitName.Text = _currentPosdRow.barcodename2;
                tbDiscPercent.Text = _currentPosdRow.discount.ToString();
                tbVatSize.Text = _currentPosdRow.vatsize.ToString();
                tbQty.Text = _currentPosdRow.qty.ToString();
                tbPrice.Text = _currentPosdRow.price.ToString();
                tbSum.Text = _currentPosdRow.sum.ToString();
                tbRecipeNo.Text = _currentPosdRow.recipeno.ToString();
                tbCompCode.Text = _currentPosdRow.compcode;
                tbCompPercent.Text = _currentPosdRow.comppercent.ToString();
                tbRecTotalSum.Text = _currentPosdRow.totalsum.ToString();
                tbRecCompSum.Text = _currentPosdRow.compensationsum.ToString();
                tbPaySum.Text = _currentPosdRow.paysum.ToString();
                tbEndSum.Text = _currentPosdRow.endsum;
                tbOnHandQty.Text = SolveOnHandQuantity(_currentPosdRow);
                if (_currentPosdRow.erecipe_no != 0)
                    tbRecipeNo.Text = _currentPosdRow.erecipe_no.ToString();
                EnableButtons();
            }
        }

        public bool Display2Visible
        {
            get
            {
                return chb2display.Checked;
            }

            set
            {
                chb2display.Checked = value;
            }
        }
        internal override bool IsBusy
        {
            get
            {
                return _IsBusy;
            }
            set
            {
                _IsBusy = value;
                if (_IsBusy == true)
                {
                    this.UseWaitCursor = true;
                    this.Cursor = Cursors.WaitCursor;
                    this.gvPosd.Cursor = Cursors.WaitCursor;
                    gvPosd.Enabled = false;
                }
                else
                {
                    this.UseWaitCursor = false;
                    this.Cursor = Cursors.Default;
                    this.gvPosd.Cursor = Cursors.Hand;
                    gvPosd.Enabled = true;
                }
            }
        }

        public DataGridView RecomendationsGridView
        {
            get => gvRecommandations;
        }

        public LoaderUserControl LoaderControl
        {
            get => loaderUserControl;
        }

        public Button PersonalPharmacistButton
        {
            get => btnPersonalPharmacist;
        }

        public Label CRMDataLoadStatus
        {            
            get => lblCRMDataLoadStatus;
        }

        public Button CRMDataReload
        {
            get => btnCRMDataReload;
        }

        public ToolStripStatusLabel UserPrioritiesRatio 
        {
            get => tsslUserPrioritiesRatio;
        }

        public Button HomeModeButton 
        { 
            get { return btnHomeMode; }
        }

        public Button WoltModeButton
        {
            get { return btnWoltMode; }
        }

        public async Task DeletePosDetail(Items.posd posDetail)
        {
            if (posDetail == null)
                return;

            await ExecuteWithWaitAsync(async () =>
            {
                await _display1Presenter.DeletePosdAsync(posDetail);
                await _display1Presenter.RefreshPoshAsync();
                EnableButtons();
            }, true);
        }

        #endregion

        #region Events
        private async void display1_Load(object sender, EventArgs e)
        {
            await ExecuteWithWaitAsync(async () =>
            {
                systemTimer.Start();

                btnTest.Visible = Session.Develop;
                btnAdvancePayment.Visible = Session.getParam("PRESENTCARD","ALLOW") == "1";
                btnHomeMode.Visible = Session.getParam("HOME", "MODE") == "1";
                btnWoltMode.Visible = Session.getParam("WOLT", "MODE") == "1";

                if (Session.CRM == true)
                {
                    btnVouchers.Visible = true;
                    btnDonation.Visible = true;
                }
                if (DateTime.Now > new DateTime(2017, 10, 1))
                {
                    btnCheque.Visible = false;
                    btnPrice.Location = new Point(556, 0);
                }
                this.Text = "Kasos pardavimo operacija v" + Session.AssemblyVersion;
                SendMessage(edtBarcode.Handle, 0x1501, 1, "Skenuokite barkodą");
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
                DB.POS.UpdateSession("Dirba su kasos aparatu", 2);
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed

                lblUser.Text = Session.Devices.debtorname;
                if (Session.Devices.fiscal == -1)
                {
                    Session.Devices.fiscal = 0;
                    chbFiscal.Enabled = false;
                }
                chbFiscal.Checked = Convert.ToBoolean(Session.Devices.fiscal);
                lblUser.Text += " " + Session.User.DisplayName;

                if (Session.MonitorCount > 1)
                {
                    this.Location = new Point(0, 0);
                    this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
                }

                if (Session.MonitorCount <= 1)
                {
                    this.Bounds = new Rectangle(0, 0, 0, 0);
                    this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
                    Display2Visible = false;
                    chb2display.Enabled = false;
                }

                if (chb2display.Enabled == true)
                {
                    Cursor.Clip = System.Windows.Forms.Screen.PrimaryScreen.Bounds;
                    Display2Visible = Session.display2checked;
                }
                ShowDisplay2();
                await RefreshPosh();
                await _display1Presenter.LoadPersonalPharmacist();
                await _display1Presenter.LoadNBORecommendationIDs();
                await _display1Presenter.LoadMedictionActiveSubstances();
                await _display1Presenter.LoadMedictionNames();
                await _display1Presenter.LoadWoltProductIds();

                if (!Session.Develop)
                {
                    Serilogger.GetLogger().Write(Serilog.Events.LogEventLevel.Information,
                        $"POS version {Session.AssemblyVersion} has been started in pharmacy:" +
                        $" {Session.SystemData.kas_client_id}; device no: {Session.Devices.deviceno}");
                }
            });
        }
        public async void display1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F1)
                btnHelp_Click(new object(), new EventArgs());

            else if (e.KeyCode == Keys.F3)
                btnCard_Click(new object(), new EventArgs());

            else if (e.KeyCode == Keys.F4 && btnPayment.Enabled)
                btnPayment_Click(new object(), new EventArgs());

            else if (e.KeyCode == Keys.F5)
            {
                await RefreshPosh();
            }
            else if (!edtBarcode.Focused && edtBarcode.Enabled)
            {
                if ((int)e.KeyCode >= 48 && (int)e.KeyCode <= 90)
                    edtBarcode.Text += (char)(e.KeyValue);

                if (e.KeyCode >= Keys.NumPad0 && e.KeyCode <= Keys.NumPad9)
                    edtBarcode.Text += (char)(e.KeyValue - 48);
                edtBarcode.Focus();
                edtBarcode.Select(edtBarcode.Text.Length, edtBarcode.Text.Length);
            }
        }
        private async void display1_Closing(object sender, FormClosingEventArgs e)
        {
            if (IsBusy && (Control.ModifierKeys & Keys.Alt) == 0)
                e.Cancel = true;
            else
            {
                Session.FeedbackTerminal.ExecuteAction<Models.FeedbackTerminal.BaseRequest>(Models.FeedbackTerminal.RequestName.CustomerCancel);
                Session.FeedbackTerminal.Dispose();
                this.notifyIcon1.Visible = false;
                await Session.FP550.Close();
            }
        }
        private void systemTimer_Tick(object sender, EventArgs e)
        {
            #region POS update
            var DOposupdate = sysTimerCounter / Settings.Default.TimerSystemUpdate;
            if (1==2 && (DOposupdate % 1) == 0)//TM: atjungio update check
            {
                try
                {
                    UpdateCheckInfo info = null;
                    if (ApplicationDeployment.IsNetworkDeployed)
                    {
                        ApplicationDeployment ad = ApplicationDeployment.CurrentDeployment;
                        info = ad.CheckForDetailedUpdate();
                        if (IsBusy && info.UpdateAvailable && PoshItem.PosdItems?.Count<Items.posd>() <= 0 && DateTime.Now.Hour > 8 && DateTime.Now.Hour < 17)
                        {
                            Boolean doUpdate = true;
                            IsBusy = true;
                            if (!info.IsUpdateRequired)
                            {
                                if (!helpers.alert(Enumerator.alert.confirm, "Galimas POS programos v" + Session.AssemblyVersion + " naujinimas į v" + info.AvailableVersion.ToString() + " versiją! Ar norite atnaujinti programą dabar?", true))
                                {
                                    doUpdate = false;
                                    IsBusy = false;
                                }
                            }
                            else
                            {
                                // Display a message that the app MUST reboot. Display the minimum required version.
                                helpers.alert(Enumerator.alert.info, "Privalomas POS programos v" + Session.AssemblyVersion + " naujinimas į v" + info.AvailableVersion.ToString() + " versiją! \nPrograma bus atnaujinama ir paleidžiama iš naujo.");
                            }

                            if (doUpdate)
                            {
                                ad.Update();
                                IsBusy = false;
                                helpers.alert(Enumerator.alert.info, "POS programa sėkmingai atnaujinta ir bus paleidžiama iš naujo!");
                                Application.Restart();
                            }
                        }
                    }
                }

                catch (DeploymentDownloadException dde)
                {
                    helpers.alert(Enumerator.alert.error, "Naujos versijos parsiųsti nepavyko!\nPatikrinkite interneto ryšį. \nKlaida: " + dde.Message);
                    return;
                }
                catch (InvalidDeploymentException ide)
                {
                    helpers.alert(Enumerator.alert.error, "Neįmanoma susisiekti su atnaujinimo serveriu. POS programos versija yra sugadinta! Prašome perinstaliuoti POS programą. \nKlaida: " + ide.Message);
                    return;
                }
                catch (InvalidOperationException ioe)
                {
                    helpers.alert(Enumerator.alert.error, "Neįmanoma atnaujinti POS programos! Klaida: " + ioe.Message);
                    return;
                }
            }
            #endregion
            #region Clear path
            var DOclearrecipepath = sysTimerCounter / Settings.Default.TimerRecipePathClear;
            if ((DOclearrecipepath % 1) == 0)
            {
                try
                {
                    string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @"\Elektroniniai receptai\";
                    var files = Directory.GetFiles(path);
                    foreach (var el in files)
                    {
                        if (DateTime.Now - File.GetCreationTime(el) > new TimeSpan(1, 0, 0, 0))
                            File.Delete(el);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                }
            }
            #endregion
            #region Enable eRecipe
            var DOerecipe = sysTimerCounter / Settings.Default.TimerCheckBU;
            if ((DOerecipe % 1) == 0)
            {
                ExecuteAsyncAction(async () =>
                {
                    await Program.TaskAsync();
                    this.BeginInvoke(new Action(() =>
                    {
                        EnableButtons();
                    }));
                });

            }
            #endregion
            #region BU
            decimal DObu = sysTimerCounter / Settings.Default.TimerCheckBU;
            if ((DObu % 1) == 0)
            {
                try
                {
                    if (lblPurpose.Text == "")
                        using (var scTamroWS = new SR_TamroWS.TamroWSSoapClient())
                        {
                            SR_TamroWS.PhInfo pp = scTamroWS.GetPharmacyInfoForDate(Session.SystemData.kas_client_id.ToString());
                            if (pp.AC != null && pp.BU != null && pp.Forecast != null)
                            {
                                var AC = pp.AC.Replace('.', ',').ToDecimal();
                                var BU = pp.BU.Replace('.', ',').ToDecimal();
                                var FORECAST = pp.Forecast.Replace('.', ',').ToDecimal();
                                if (BU > 0)
                                {
                                    lblPurpose.Text = (Math.Round(AC / BU * 100)).ToString() + " %";
                                    lblForecast.Text = (Math.Round(FORECAST / BU * 100)).ToString() + " %";
                                }
                            }
                        }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                }
            }
            #endregion
            #region ConnectToFeedbackTerminal
            var DOfeedbackTerminal = sysTimerCounter / Settings.Default.TimerFeedbackTerminalHub;
            if ((DOfeedbackTerminal % 1) == 0)
            {
                Task.Factory.StartNew(async () =>
                {
                    try
                    {
                        await Session.FeedbackTerminal.Connect();
                        Console.WriteLine("Connected");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                });
            }
            #endregion
            sysTimerCounter++;//increase timer
        }
        private void chbFiscal_CheckedChanged(object sender, EventArgs e)
        {
            if ((sender as CheckBox).Checked == true)
                Session.Devices.fiscal = 1;
            else
            {
                if (helpers.alert(Enumerator.alert.confirm, "Ar tikrai norite atjungti kasos aparatą ir pradėti nefiskalinį pardavimą?", true))
                    Session.Devices.fiscal = 0;
                else
                    chbFiscal.Checked = true;
            }
            EnableButtons();
        }
        private void btnLogOff_Click(object sender, EventArgs e)
        {
            //bandom nebeuzdarinet encounteriu
            /*if (EncounterItem.Id != "" && (PractitionerItem.RoleId == 6 || PractitionerItem.RoleId == 7))
            {
                form_wait(true);
                form_action = "btnClose";
                eRecipeUtils.CloseEncounter(EncounterItem.Id, DB.eRecipe.getEncounterStatus(helpers.getDecimal(EncounterItem.Id)), CloseEncounter_cb);
            }
            else*/
            Application.Exit();

            /*login dlg = new login();
            dlg.Location = helpers.middleScreen2(dlg, false);
            dlg.ShowDialog();
            if (dlg.DialogResult == DialogResult.OK)
            {
                dlg.Dispose();
                dlg = null;
                Application.Exit();
                Application.Run(new display1());
            }
            else
            {
                dlg.Dispose();
                dlg = null;
            }
             * */
        }
        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ExecuteWithWait(() =>
            {
                About dlg = new About();
                dlg.Location = helpers.middleScreen(this, dlg);
                dlg.ShowDialog();
                edtBarcode.Select();
                dlg.Dispose();
                dlg = null;
            });
        }
        private void updateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ExecuteWithWait(() =>
            {
                sysTimerCounter = Settings.Default.TimerSystemUpdate;
            });
        }
        private void adminToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ExecuteWithWait(() =>
            {
                if (Session.Admin == false)
                {
                    ProgressDialog dlg = new ProgressDialog("Admin", "Slaptažodis");
                    dlg.ShowDialog();
                    if (dlg.DialogResult == DialogResult.OK)
                    {
                        if (dlg.Result == "llopasss")
                        {
                            Session.Admin = true;
                            statusStrip1.BackColor = Color.Red;
                            adminToolStripMenuItem.Text = "Log out Admin";
                        }
                    }
                    else
                    {
                        helpers.alert(Enumerator.alert.error, "Neteisingas slaptažodis!");
                        dlg.DialogResult = new DialogResult();
                        return;
                    }
                    dlg.Dispose();
                    dlg = null;
                }
                else
                {
                    Session.Admin = false;
                    statusStrip1.BackColor = SystemColors.Control;
                    adminToolStripMenuItem.Text = "Login Admin";
                }
            });
        }
        private void chb2display_CheckedChanged(object sender, EventArgs e)
        {
            ExecuteWithWait(() =>
            {
                CheckBox cb = sender as CheckBox;
                Display2Visible = cb.Checked;
                this.ShowDisplay2();
                edtBarcode.Focus();
            });
        }
        //Prekių info
        private void btnStockInfo_Click(object sender, EventArgs e)
        {
            ExecuteWithWait(() =>
            {
                stock_info dlg = new stock_info();
                dlg.Location = helpers.middleScreen(this, dlg);
                dlg.caller = "none";
                dlg.ShowDialog();
                edtBarcode.Select();
                dlg.Dispose();
                dlg = null;
            });
        }
        //...
        private async void btnSelBarcode_Click(object sender, EventArgs e)
        {
            await ExecuteWithWaitAsync(async () =>
            {
                using (var dlg = new stock_info())
                {
                    dlg.Location = helpers.middleScreen(this, dlg);
                    dlg.caller = "select_barcode";
                    dlg.ShowDialog();
                    if (!dlg.selBarcode.Equals(""))
                    {
                        var bc = "";
                        if (edtBarcode.Text != "")
                            bc = edtBarcode.Text + dlg.selBarcode;
                        else
                            bc = dlg.selQty + "*" + dlg.selBarcode;
                        await SubmitBarcode(new Models.Barcode { BarcodeStr = bc, EnteredByMainFieldInDisplay1 = true });
                    }
                    edtBarcode.Select();
                }
            });
        }
        private async void edtBarcode_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
        {
            edtBarcode.Select();
            switch ((int)e.KeyChar)
            {
                case 29://manualy add group seperator GS
                    TextBox tb = sender as TextBox;
                    int i = tb.SelectionStart;
                    tb.Text = tb.Text.Insert(tb.SelectionStart, Session.BarcodeParser.groupSeperator.ToString());
                    tb.SelectionStart = i + 5;
                    e.Handled = true;
                    break;
                case (char)Keys.Enter:
                    await ExecuteWithWaitAsync(async () =>
                    {
                        await SubmitBarcode(new Models.Barcode { BarcodeStr = edtBarcode.Text, EnteredByMainFieldInDisplay1 = true });
                    });
                    break;
                case ',':
                    e.KeyChar = '.';
                    break;
            }
            e.KeyChar = char.ToUpper(e.KeyChar);
        }
        //Trinti eilutę
        public async void btnDelete_Click(object sender, EventArgs e)
        {
            bool confirmCancel = helpers.alert(Enumerator.alert.confirm, GetDeleteConfirmationMessage("Ar tikrai norite trinti šį įrašą?"), true);

            if ((Session.WoltMode && currentPosdRow.productid == WoltPackingFeeId) || currentPosdRow.is_deposit)
            {
                helpers.alert(Enumerator.alert.error,"Šios eilutės negalima pašalinti");
                return;
            }

            if (confirmCancel) 
            {
                form_action = "btnDelete";
                await ExecuteWithWaitAsync(async () => 
                {
                    await _display1Presenter.DeletePosdAsync(currentPosdRow);
                    await _display1Presenter.RefreshPoshAsync();
                    EnableButtons();
                }, true);
            }
        }
        //Receptas
        private async void btnRecipe_Click(object sender, EventArgs e)
        {
            await ExecuteWithWaitAsync(async () =>
            {
                using (var dlg = new recipe_edit(currentPosdRow))
                {
                    dlg.Location = helpers.middleScreen(this, dlg);
                    dlg.ShowDialog();
                    await RefreshPosh();
                    edtBarcode.Select();
                }
            }, false);
        }
        //Nuolaida
        private async void btnDiscount_Click(object sender, EventArgs e)
        {
            await ExecuteWithWaitAsync(async () =>
            {
                DiscountView dlg = new DiscountView(PoshItem.Id, currentPosdRow.id);
                dlg.Location = helpers.middleScreen(this, dlg);
                dlg.ShowDialog();
                if (dlg.DialogResult == DialogResult.OK)
                    await RefreshPosh();
                edtBarcode.Select();
                dlg.Dispose();
                dlg = null;
            });
        }
        //GSK Čekis
        private async void btnCheque_Click(object sender, EventArgs e)
        {
            await ExecuteWithWaitAsync(async () =>
            {
                cheque dlg = new cheque(currentPosdRow.id);
                dlg.Location = helpers.middleScreen(this, dlg);
                dlg.ShowDialog();
                if (dlg.DialogResult == DialogResult.OK)
                    await RefreshPosh();
                edtBarcode.Select();
                dlg.Dispose();
                dlg = null;
            });
        }
        //Priemoka
        private async void btnPrep_Click(object sender, EventArgs e)
        {
            await ExecuteWithWaitAsync(async () =>
            {

                prep dlg = new prep(PoshItem);
                dlg.Location = helpers.middleScreen(this, dlg);
                dlg.ShowDialog();
                if (dlg.DialogResult == DialogResult.OK)
                    await RefreshPosh();
                edtBarcode.Select();
                dlg.Dispose();
                dlg = null;
            });
        }
        //Kaina
        private async void btnPrice_Click(object sender, EventArgs e)
        {
            await ExecuteWithWaitAsync(async () =>
            {
                PriceView dlg = new PriceView(currentPosdRow.id);
                dlg.Location = helpers.middleScreen(this, dlg);
                dlg.ShowDialog();
                if (dlg.DialogResult == DialogResult.OK)
                    await RefreshPosh();
                edtBarcode.Select();
                dlg.Dispose();
                dlg = null;
            });
        }
        //Paslauga
        private async void btnService_Click(object sender, EventArgs e)
        {
            await ExecuteWithWaitAsync(async () =>
            {
                pos_service dlg = new pos_service(PoshItem.Id);
                dlg.Location = helpers.middleScreen(this, dlg);
                dlg.ShowDialog();
                if (dlg.DialogResult == DialogResult.OK)
                {
                    form_action = "edtBarcode";
                    await RefreshPosh();
                }
                edtBarcode.Select();
                dlg.Dispose();
                dlg = null;
            });
        }
        //Trinti kvitą
        private async void btnCancel_Click(object sender, EventArgs e)
        {
            var confirmCancel = helpers.alert(Enumerator.alert.confirm, GetDeleteConfirmationMessage("Ar tikrai norite panaikinti kvitą?"), true);
            FormWait frmWait = PoshItem.PosdItems != null && PoshItem.PosdItems.Any(val => val.IsSalesOrderProduct) ?
                new FormWait()
                {
                    Notification = "Trinamas pardavimo į namus perkėlimo dokumentas," +
                    "\nprašome palaukti tai gali užtrukti iki keletos minučių..."
                } :
                null;

            if (confirmCancel)
            {
                form_action = "btnCancel";
                await ExecuteWithWaitAsync(async () => 
                {
                    await _display1Presenter.SetHomeMode(false);
                    await _display1Presenter.SetWoltMode(false);
                    await _display1Presenter.CancelPoshAsync();
                    await _display1Presenter.CancelSalesOrder(PoshItem);
                    await _display1Presenter.CancelHomeDeliveryorder(PoshItem);
                    await _display1Presenter.RefreshPoshAsync();
                    currentPageIndex = 1;
                    form_action = "";
                    edtBarcode.Focus();
                    edtBarcode.SelectAll();
                }, true, null, frmWait);
            }
        }
        private void gvDelete_Click(object sender, DataGridViewRowCancelEventArgs e)
        {
            btnDelete_Click(new object(), new EventArgs());
            e.Cancel = true;
        }
        private void gvPosd_MouseLeave(object sender, EventArgs e)
        {
            if (chb2display.Enabled == true && Session.Develop == false)
                Cursor.Clip = System.Windows.Forms.Screen.PrimaryScreen.Bounds;
        }
        private void gvPosd_CurrentCellChanged(object sender, EventArgs e)
        {
            CurrentCellChanged_Event(sender, e);
        }
        private void gvPosd_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            DataGridView dgv = sender as DataGridView;
            if (e.ColumnIndex != -1 && e.RowIndex != -1 && e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                dgv.CurrentCell = dgv.Rows[e.RowIndex].Cells[e.ColumnIndex];
                var relativeMousePosition = dgv.PointToClient(Cursor.Position);
                dgvContextMenu.Show(dgv, relativeMousePosition);
            }
        }
        private void gvPosd_ColumnAdded(object sender, DataGridViewColumnEventArgs e)
        {
            base.OnClick(e);
            e.Column.SortMode = DataGridViewColumnSortMode.NotSortable;
        }
        private async void gvPosd_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (IsBusy)
                return;
            if (e.RowIndex < 0)
                return;

            if (e.ColumnIndex == gvPosd.Rows[e.RowIndex].Cells["have_recipe"].ColumnIndex
                && currentPosdRow.status_insurance != 13
                && currentPosdRow.recipeid == 0
                && currentPosdRow.erecipe_no == 0)
            {
                IsBusy = true;
                int status = 12 - (int)gvPosd.Rows[e.RowIndex].Cells["have_recipe"].Value;
                await DB.cheque.ChangeInsuranceStatus(PoshItem.Id, gvPosd.Rows[e.RowIndex].Cells["id"].Value.ToDecimal(), status);
                await RefreshPosh();
            }

            if (e.ColumnIndex == gvPosd.Rows[e.RowIndex].Cells["apply_insurance"].ColumnIndex
                && (PoshItem.InsuranceItem.Company != "ERG" || PoshItem.ChequeInsuranceSum == 0))
            {
                IsBusy = true;
                int status = 11 + 2 * (int)gvPosd.Rows[e.RowIndex].Cells["apply_insurance"].Value;
                if (status == 11)
                    await DB.cheque.ChangeInsuranceStatus(PoshItem.Id, gvPosd.Rows[e.RowIndex].Cells["id"].Value.ToDecimal(), status);
                else
                    await DB.cheque.UpdateChequeTrans(gvPosd.Rows[e.RowIndex].Cells["id"].Value.ToDecimal(), 0, PoshItem.InsuranceItem.Company, PoshItem.InsuranceItem.CardSessionId, PoshItem.InsuranceItem.CardNoLong, status, "");
                await RefreshPosh();
            }

            if (e.ColumnIndex == gvPosd.Rows[e.RowIndex].Cells["btnDeleteLine"].ColumnIndex)
            {
                btnDelete_Click(sender, new EventArgs());
            }

            if (e.ColumnIndex == gvPosd.Rows[e.RowIndex].Cells["fmd_link"].ColumnIndex)
            {
                btnPosdFMD_Event(this, e);
            }
        }
        //Ankstesnis
        private async void btnPrevious_Click(object sender, EventArgs e)
        {
            await ExecuteWithWaitAsync(async () =>
            {
                currentPageIndex--;
                btnNext.Enabled = true;
                btnLast.Enabled = true;
                await BindGrid();
            });
        }
        //<<
        private async void btnFirst_Click(object sender, EventArgs e)
        {
            await ExecuteWithWaitAsync(async () =>
            {
                currentPageIndex = 1;
                btnPrevious.Enabled = false;
                btnNext.Enabled = true;
                btnLast.Enabled = true;
                btnFirst.Enabled = false;
                await BindGrid();
            });
        }
        //Sekantis
        private async void btnNext_Click(object sender, EventArgs e)
        {
            await ExecuteWithWaitAsync(async () =>
            {
                currentPageIndex++;
                btnFirst.Enabled = true;
                btnPrevious.Enabled = true;
                await BindGrid();
            });
        }
        //>>
        private async void btnLast_Click(object sender, EventArgs e)
        {
            await ExecuteWithWaitAsync(async () =>
            {
                currentPageIndex = (int)Math.Ceiling(Convert.ToDecimal(PoshItem.PosdItems.Count<Items.posd>()) / Settings.Default.grid_page_size);
                btnFirst.Enabled = true;
                btnPrevious.Enabled = true;
                btnNext.Enabled = false;
                btnLast.Enabled = false;
                await BindGrid();
            });
        }
        private async void btnX_Click(object sender, EventArgs e)
        {
            await ExecuteWithWaitAsync(async () =>
            {
                await Session.FP550.PrintReportMiniX();
            });
        }
        private async void btnResetPos_Click(object sender, EventArgs e)
        {
            await ExecuteWithWaitAsync(async () =>
            {
                if (helpers.alert(Enumerator.alert.confirm, "Ar tikrai norite anuliuoti fiskalinį kvitą?", true))
                {
                    await Session.FP550.FiscalSaleReset();
                    await _display1Presenter.SetHomeMode(false);
                    await _display1Presenter.SetWoltMode(false);
                    await _display1Presenter.CancelHomeDeliveryorder(PoshItem);
                }
            });
        }
        private async void btnCard_Click(object sender, EventArgs e)
        {
            await ExecuteWithWaitAsync(async () =>
            {
                string cardNo = "";
                //ProgressDialog dlg = new ProgressDialog("card", "");
                // Pause the thread for one second
                //dlg.Show();

                cardNo = await Session.FP550.ReadCard();
                //await Task.WhenAny(task, Task.Delay(timeout));
                //dlg.Close();
                if (cardNo != "")
                {
                    await SubmitBarcode(new Models.Barcode { BarcodeStr = cardNo });
                    edtBarcode.Text = "";
                }
            });
        }
        private async void btnEcrRep_Click(object sender, EventArgs e)
        {
            await ExecuteWithWaitAsync(async () =>
            {
                using (ECRReportsView dlg = new ECRReportsView()) 
                { 
                    dlg.Location = helpers.middleScreen(this, dlg);
                    dlg.ShowDialog();
                    if (dlg.DialogResult == DialogResult.OK)
                        await RefreshPosh();
                    edtBarcode.Select();
                    if (dlg.SelectedReportIndex == "4")
                    {
                        if (File.Exists(Session.Devices.errorfile))
                        {
                            Process process = new Process();
                            ProcessStartInfo startInfo = new ProcessStartInfo();
                            startInfo.WindowStyle = ProcessWindowStyle.Hidden;
                            startInfo.FileName = "cmd.exe";
                            startInfo.Arguments = @"/C " + Session.Devices.errorfile;
                            process.StartInfo = startInfo;
                            process.Start();
                        }
                    }
                }
            });
        }
        private void btnDrugPrice_Click(object sender, EventArgs e)
        {
            Program.Display2.ShowPrices();
        }
        private void btnPrescriptionCheck_Click(object sender, EventArgs e)
        {
            ExecuteWithWait(() =>
            {
                using (PrescriptionCheckView dlg = new PrescriptionCheckView(currentPosdRow))
                {
                    dlg.Location = helpers.middleScreen(this, dlg);
                    dlg.ShowDialog();
                    edtBarcode.Select();
                }
            });
        }
        private void btnERecipe_Click(object sender, EventArgs e)
        {
            Execute(() =>
            {
                using (Form dlg = Session.getParam("ERECIPE", "V2") == "1" ? (Form)new eRecipeV2() : (Form)new eRecipe())
                {
                    dlg.Location = helpers.middleScreen(this, dlg);
                    dlg.ShowDialog();
                    //await RefreshPosh();
                    edtBarcode.Select();
                }            
            });
        }
        private async void btnInsurance_Click(object sender, EventArgs e)
        {
            await ExecuteWithWaitAsync(async () =>
            {
                Popups.display1_popups.Insurance dlg = new Popups.display1_popups.Insurance(PoshItem, "EPS");
                dlg.Location = helpers.middleScreen(this, dlg);
                dlg.ShowDialog();
                if (dlg.DialogResult == DialogResult.OK)
                    await AddInsuranceCard(dlg.InsuranceType, dlg.CardNo + dlg.Identity);
                dlg.Dispose();
                dlg = null;
            });
            await RefreshPosh();
        }
        private async void btnKAS_Click(object sender, EventArgs e)
        {
            await ExecuteWithWaitAsync(async () =>
            {
                posh dlg = new posh();
                dlg.Location = helpers.middleScreen(this, dlg);
                dlg.ShowDialog();
                if (dlg.DialogResult == DialogResult.OK)
                    await RefreshPosh();
                edtBarcode.Select();
                dlg.Dispose();
                dlg = null;
            });
        }
        private void kopijuotiToolStripMenuItem_Click(object sender, EventArgs e)
        {
            helpers.CopyRowCell(gvPosd);
        }
        private async void btnSettings_Click(object sender, EventArgs e)
        {
            await ExecuteWithWaitAsync(async () =>
            {
                SettingsDialog dlg = new SettingsDialog();
                dlg.Location = helpers.middleScreen(this, dlg);
                dlg.ShowDialog();
                btnVouchers.Visible = Session.CRM;
                btnDonation.Visible = Session.CRM;
                await RefreshPosh();
                edtBarcode.Select();
                dlg.Dispose();
                dlg = null;
            });
        }
        private void btnHelp_Click(object sender, EventArgs e)
        {
            ExecuteWithWait(() =>
            {
                ProcessStartInfo sInfo = new ProcessStartInfo(@"http://lithuania.phoenix.loc/kas-instrukcijos/kas-instrukcijos");
                Process.Start(sInfo);
            });
        }
        private async void btnCheckBalance_Click(object sender, EventArgs e)
        {
            await ExecuteWithWaitAsync(async () =>
            {
                BallanceDialog dlg = new BallanceDialog("");
                dlg.Location = helpers.middleScreen(this, dlg);
                dlg.ShowDialog();
                if (dlg.DialogResult == DialogResult.OK)
                {
                    if (dlg.selectedCard.Value == "LOJALUMAS")
                        await SubmitBarcode(new Models.Barcode { BarcodeStr = dlg.CardNo });
                    else if (Session.Params.Where(x => x.system == "INSURANCE").Where(p => p.par == dlg.selectedCard.Value).Count() > 0)
                        await AddInsuranceCard(dlg.selectedCard.Value, dlg.CardNo + dlg.CardNo2);
                }
                dlg.Dispose();
                dlg = null;
            });
            await RefreshPosh();
        }
        private async void btnVouchers_Click(object sender, EventArgs e)
        {
            await ExecuteWithWaitAsync(async () =>
            {
                using (VouchersView vouchersView = new VouchersView(PoshItem))
                {
                    vouchersView.Location = helpers.middleScreen(this, vouchersView);
                    vouchersView.ShowDialog();
                    if (vouchersView.DialogResult == DialogResult.OK)
                    {
                        string vouchersArray = string.Empty;
                        foreach (var el in vouchersView.Vouchers.Where(vl => vl.Selected == 1))
                        {
                            for (int i = 0; i < el.Qty; i++)
                                vouchersArray += el.Code + ';';
                        }
                        await DB.Loyalty.setManualVouchers(PoshItem.Id, vouchersArray.TrimEnd(';'));
                        await DB.Loyalty.setInstanceDiscount(PoshItem.Id, vouchersView.InstanceDiscount.Checked ? 0 : 1);
                        await RefreshPosh();
                    }
                    edtBarcode.Select();
                }
            });
        }
        private async void btnPayment_Click(object sender, EventArgs e)
        {
            await ExecuteWithWaitAsync(async () =>
            {
               // if (PoshItem.LoyaltyCardType == "BENU")//refresh grid only for BENU clients because of feedback terminal donation functionality!
               //     await RefreshPosh();
                if (!PoshItem.CountStickers())
                    throw new Exception("");
                //if (PoshItem.PosdItems.Any(a => a.Type == "ADVANCEPAYMENT") && PoshItem.PosdItems.Count > 1)
               //     throw new Exception("Avansinis mokejimas turi būti vienas krepšelyje");


                if (_PoshItem.PosdItems.Any(c => c.recipeid == 0))
                {
                    var creditorId = await DB.POS.GetPrepTransCreditor(_PoshItem.Id);
                    if (creditorId != 0 && helpers.alert(Enumerator.alert.confirm, "Kvite yra įskanuotų nekompensuojamų prekių. Ar norite taisyti kvitą?", true))
                    {
                        Close();
                        return;
                    }
                }
                using (PaymentView dlg = new PaymentView(PoshItem))
                {
                    dlg.Location = helpers.middleScreen(this, dlg);
                    dlg.ShowDialog();
                }

                edtBarcode.Select();
                EnableButtons();
            });
        }
        private void btnLabel_Click(object sender, EventArgs e)
        {
            ExecuteWithWait(() =>
            {
                SR_DrugNotes.DrugNotesWSSoapClient drug_notes = new SR_DrugNotes.DrugNotesWSSoapClient();
                string file_path = drug_notes.GetDrugNotesByProductID(currentPosdRow.productid) ?? "";
                if (file_path != "")
                    Process.Start(file_path);
                else
                    throw new Exception("Informacinis lapelis nerastas!");
            });
        }
        private async void btnAdvancePayment_Click(object sender, EventArgs e)
        {
            await ExecuteWithWaitAsync(async () =>
            {
                using (AdvancePaymentView advancePaymentView = new AdvancePaymentView(PoshItem))
                {
                    await advancePaymentView.Init();
                    advancePaymentView.Location = helpers.middleScreen(this, advancePaymentView);
                    advancePaymentView.ShowDialog();
                    if (advancePaymentView.DialogResult == DialogResult.OK)
                    {
                        form_action = "edtBarcode";
                        await RefreshPosh();
                    }
                    edtBarcode.Select();
                }
            });
        }
        private async void btnTest_Click(object sender, EventArgs e)
        {

        }

        private void btnCreateBENUclient_Click(object sender, EventArgs e)
        {
            ExecuteWithWait(() =>
            {
                if (Session.getParam("CRM","SPECIALCARD") == "0")
                {
                    Session.FeedbackTerminal.CreateAccount();
                }
                else
                {
                    using (var dlg = new LoyaltyCardTypeView())
                    {
                        dlg.Location = helpers.middleScreen(this, dlg);
                        dlg.ShowDialog();
                    }
                }
            });
        }

        private void btnFMD_Click(object sender, EventArgs e)
        {
            btnPosdFMDdlg_Event(this, e);
        }
        private async void btnDonation_Click(object sender, EventArgs e)
        {
            await ExecuteWithWaitAsync(async () =>
            {
                using (var dlg = new ProgressDialog("donation", $"Sukauptų taškų suma yra {PoshItem.CRMItem.Account.ActualPoints} EUR.\nĮveskite sumą, kurią klientas nori paaukoti.", this))
                {
                    dlg.Location = helpers.middleScreen(this, dlg);
                    dlg.ShowDialog();
                    if (dlg.DialogResult == DialogResult.OK)
                    {
                        await Session.CRMRestUtils.TransferPoints((float)dlg.Result.ToDecimal(), PoshItem.CRMItem.Account.CardNumber);
                        await RefreshPosh();
                    }
                }
            });
        }
        private async void btnEshop_Click(object sender, EventArgs e)
        {
            await ExecuteWithWaitAsync(async () =>
            {
                var tamroIntranetGateway = new TamroIntranetGateway();
                await tamroIntranetGateway.GetToken();
                if (tamroIntranetGateway.accessToken?.access_token == null)
                    throw new Exception("Nepavyko prisijungti prie Intranet.\nKreipkitės į ServiceDesk!");
                string intranetURL = $"{tamroIntranetGateway.url}/logintoken/{tamroIntranetGateway.accessToken.access_token}/" +
                    $"{tamroIntranetGateway.accessToken.refresh_token}/%2FEShop%2FOrders%3FpharmacyCode={Session.SystemData.kas_client_id}&lang=lt";
                Process.Start("chrome.exe", intranetURL);
            });
        }
        private void lblInfo_TextChanged(object sender, EventArgs e)
        {
            Label lbl = (Label)sender;
            if (lbl.Image != null) return;
            using (Graphics cg = lbl.CreateGraphics())
            {
                SizeF lblsize = new SizeF(lbl.Width, lbl.Height);
                SizeF textsize = cg.MeasureString(lbl.Text, lbl.Font, lblsize);
                while (textsize.Width > lblsize.Width - (lblsize.Width * 0.1))
                {
                    lbl.Font = new Font(lbl.Font.Name, lbl.Font.Size - 1, lbl.Font.Style);
                    textsize = cg.MeasureString(lbl.Text, lbl.Font, lblsize);
                    if (lbl.Font.Size < 6) break;
                }
            }
        }

        private void btnPersonalPharmacist_Click(object sender, EventArgs e)
        {
            ExecuteWithWait(() =>
            {
                using (var personalPharmacistView = new PersonalPharmacistView())
                {
                    switch (personalPharmacistView.ShowDialog()) 
                    {
                        case DialogResult.OK:
                            btnPersonalPharmacist.ForeColor = Color.Green;
                            break;
                        case DialogResult.Abort:
                            btnPersonalPharmacist.ForeColor = Color.Black;
                            break;
                    }
                }
            });
        }

        private async void Display1View_Shown(object sender, EventArgs e)
        {
            await _display1Presenter.LoadUserPrioritesRatio();
            Task.Run(_display1Presenter.LoadCRMData).GetAwaiter();
            await _display1Presenter.SetHomeMode(Session.HomeMode);
            await _display1Presenter.SetWoltMode(Session.WoltMode);
        }

        private void btnCRMDataReload_Click(object sender, EventArgs e)
        {
            Task.Run(_display1Presenter.LoadCRMData).GetAwaiter();
        }
        #endregion

        private async void btnClientSearch_Click(object sender, EventArgs e)
        {
            using (var clientSearchView = new ClientSearchView())
            {
                if (clientSearchView.ShowDialog() == DialogResult.OK)
                {
                    await SubmitBarcode(new Barcode() 
                    { 
                        BarcodeStr = clientSearchView .FocusedClient.CardNumber 
                    });
                    clientSearchView.Close();
                }
            }
        }

        private async void btnHomeMode_Click(object sender, EventArgs e)
        {
            await ExecuteWithWaitAsync(async () =>
            {
                using (var homeModeActivateView = new HomeModeActivateView())
                {
                    homeModeActivateView.Location = helpers.middleScreen(this, homeModeActivateView);
                    homeModeActivateView.ShowDialog();
                    if (homeModeActivateView.DialogResult == DialogResult.OK)
                    {
                        await _display1Presenter.SetHomeMode(true);
                    }
                }
            });
        }

        private async void btnWolt_Click(object sender, EventArgs e)
        {
            var WoltCrmEnabled = Session.getParam("WOLT", "CRM") == "1";
            await ExecuteWithWaitAsync(async () => await _display1Presenter.SetWoltMode(true), false);
            await ExecuteWithWaitAsync(async () => await _display1Presenter.CancelPoshAsync(), false);
            await ExecuteWithWaitAsync(async () => await _display1Presenter.RefreshPoshAsync(), false);
            if (WoltCrmEnabled)
                await ExecuteWithWaitAsync(async () => await SubmitBarcode(new Barcode() { BarcodeStr = WoltLoyaltyCardNumber }), false);
            currentPageIndex = 1;
        }
    }
}

