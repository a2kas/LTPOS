using AutoMapper;
using CKas.Contracts.PresentCards;
using Microsoft.Extensions.DependencyInjection;
using POS_display.Models.CRM;
using POS_display.Models.KAS;
using POS_display.Models.Loyalty;
using POS_display.Models.Pos;
using POS_display.Repository.HomeMode;
using POS_display.Repository.Pos;
using POS_display.Utils;
using POS_display.Utils.Logging;
using POS_display.Views.KAS;
using POS_display.wpf.View.KAS;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using System.Windows.Forms;
using Tamroutilities.Client;

namespace POS_display
{
    public partial class posd : FormBase
    {
        private bool formWaiting = false;
        private List<Items.KAS.posd> posd_list = new List<Items.KAS.posd>();
        private static int currentPageIndex = 0;
        private static int current_row = -1;
        private static int current_column = -1;
        private int whole_bill = 0;
        Items.Insurance insurance_item;
        private DataTable dt_BENUM;
        private readonly PosHeader posh_item;
        private const decimal LoyaltyPointsProductId = 10000109134;
        private const decimal InsuranceProductId = 10000109105;
        private const decimal PresentCardProductId = 10000100596;
        private const decimal RoundingItemProductId = 10000121707;

        private int lastPageIndex = 0;
        private int pageSize = 10;
        private readonly IPosRepository _posRepository;
        private readonly ITamroClient _tamroClient;
        private readonly IPOSUtils _posUtils;
        private readonly IHomeModeRepository _homeModeRepository;
        public posd(PosHeader posh_data, 
            IPosRepository posRepository,
            ITamroClient tamroClient,
            IPOSUtils posUtils,
            IHomeModeRepository homeModeRepository)
        {
            InitializeComponent();
            form_wait(true);
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            DB.POS.UpdateSession("Kvito lentelė", 2);
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            posh_item = posh_data;

            var data = new Dictionary<string, string>
            {
                { "1", "Techninė klaida" },
                { "2", "Brokuota pakuotė" }
            };
            cmbReason.DataSource = new BindingSource(data, null);
            cmbReason.DisplayMember = "Value";
            cmbReason.ValueMember = "Key";
            cmbReason.SelectedIndex = 0;
            cmbReason.Visible = Session.getParam("ITEMRETURNFORM", "ENABLED") == "1";

            _posRepository = posRepository ?? throw new ArgumentNullException();
            _tamroClient = tamroClient ?? throw new ArgumentNullException();
            _posUtils = posUtils ?? throw new ArgumentNullException();
            _homeModeRepository = homeModeRepository ?? throw new ArgumentNullException();
        }

        public async Task Init()
        {
            await ExecuteWithWaitAsync(async () =>
            {
                DataTable dt_insurance = await DB.cheque.GetInsuranceData(posh_item.Id);
                dt_BENUM = DB.KAS.getBENUMtransaction(posh_item.Id, 7419);
                var hasChequePresentCard = await _posRepository.HasChequePresentCard(posh_item.Id);
                var hasChequeInsurance = dt_insurance.Rows.Count > 0;

                if (hasChequeInsurance)
                {
                    string cheque_from = dt_insurance.Rows[0]["cheque_from"].ToString();
                    string card_no = dt_insurance.Rows[0]["card_no"].ToString();
                    insurance_item = new Items.Insurance(cheque_from, card_no);
                    insurance_item.CardSessionId = dt_insurance.Rows[0]["info"].ToString();
                    insurance_item.Receipt = dt_insurance;
                }

                if (hasChequePresentCard || hasChequeInsurance)
                    whole_bill = 1;

                await search_posd();
            });
        }

        private async Task search_posd()
        {
            form_wait(true);
            var list = await DB.KAS.asyncSearchPosd<Items.KAS.posd>(posh_item.Id, whole_bill);
            //kam sito reikia? o tam, kad klientui grąžintume tik tą sumą kurią mokėjo grynais
            foreach (var l in list.Where(l => l.productid == LoyaltyPointsProductId))
            {
                decimal point_sum = -1 * l.sumincvat_orig;
                decimal total_sum = list.Where(el => el.barcodeid != 0 && el.recipeid == 0 && !el.barcode.StartsWith("DEPOZITAS") &&/*el.discount_sum == 0 &&*/ el.vatsize == l.vatsize).Sum(el => el.sumincvat_orig);
                int count = list.Where(el => el.barcodeid != 0 && el.recipeid == 0 && !el.barcode.StartsWith("DEPOZITAS") &&/*el.discount_sum == 0 &&*/ el.vatsize == l.vatsize).Count();
                decimal rest_points = point_sum;
                int i = 0;
                foreach (var pd in list.Where(el => el.barcodeid != 0 && el.recipeid == 0 && !el.barcode.StartsWith("DEPOZITAS") &&/*el.discount_sum == 0*/  el.vatsize == l.vatsize))
                {
                    if (point_sum > 0)
                    {
                        i++;
                        decimal discount_sum = Math.Round(point_sum * pd.sumincvat_orig / total_sum, 2, MidpointRounding.AwayFromZero);
                        if (i == count)
                            discount_sum = rest_points;
                        pd.sumincvat = Math.Round(((pd.sumincvat_orig - discount_sum) / pd.qty_orig) * pd.qty, 2, MidpointRounding.AwayFromZero);
                        pd.sumincvat_orig = pd.sumincvat;
                        pd.pricediscounted = Math.Round(pd.sumincvat / ((100 + pd.vatsize) / 100), 2, MidpointRounding.AwayFromZero);
                        pd.discount = (pd.qty * pd.priceincvat) != 0  ? Math.Round(100 - 100 * pd.sumincvat / (pd.qty * pd.priceincvat), 2, MidpointRounding.AwayFromZero) : 0;
                        pd.vat = Math.Round(pd.sumincvat * pd.vatsize / (100 + pd.vatsize), 2, MidpointRounding.AwayFromZero);
                        pd.vat_orig = pd.vat;
                        rest_points -= discount_sum;
                    }
                }
            }
            foreach (var l in list)
            {
                l.fmd_model = (await DB.POS.GetFMDitem<wpf.Model.fmd>(l.hid, l.id));
            }
            posd_list = list.Where(l => l.productid != LoyaltyPointsProductId).ToList();//nerodyti lojalumo taškų, nes jie jau paskirstyti paeilučiui, tam kad grąžinamos sumos būtų teisingos
            currentPageIndex = 1;
            form_wait(false);
        }

        private void posd_Closing(object sender, FormClosingEventArgs e)
        {
            if (formWaiting)
                e.Cancel = true;
        }

        private void form_wait(bool wait)
        {
            if (wait)
            {
                UseWaitCursor = true;
                Cursor = Cursors.WaitCursor;
                formWaiting = true;
                gvFilter.Cursor = Cursors.WaitCursor;
            }
            else
            {
                UseWaitCursor = false;
                Cursor = Cursors.Default;
                formWaiting = false;
                gvFilter.Cursor = Cursors.Hand;
            }
        }

        public void BindGrid()
        {
            int row_before = current_row;
            if (posd_list != null && posd_list.Count<Items.KAS.posd>() > 0)
            {
                int startIndex = (currentPageIndex - 1) * pageSize;
                int endIndex = (currentPageIndex - 1) * pageSize + pageSize;
                if (endIndex > posd_list.Count<Items.KAS.posd>())
                    endIndex = posd_list.Count<Items.KAS.posd>();
                gvFilter.DataSource = posd_list;//.Skip(startIndex).Take(endIndex).ToList<Items.KAS.posd>();
                pageSize = posd_list.Count<Items.KAS.posd>();
                lastPageIndex = (int)Math.Ceiling(Convert.ToDecimal(posd_list.Count<Items.KAS.posd>()) / pageSize);
                lblRecordsStatus.Text = currentPageIndex + " / " + lastPageIndex;
                gvFilter.Columns["selected"].ReadOnly = false;
                GridView_order();
                foreach (DataGridViewRow dr in gvFilter.Rows)
                {
                    var posd_item = (Items.KAS.posd)dr.DataBoundItem;
                    if (((decimal)dr.Cells["barcodeid"].Value == 0 && posd_item.qty == 0) || posd_item.qty <= posd_item.returnedqty || posd_item.productid == RoundingItemProductId)
                        dr.Cells["selected"].ReadOnly = true;
                    if (Session.Params.FirstOrDefault(x => x.system == "FMD" && x.par == "ENABLED")?.value == "1"
                    && posd_item.fmd_required
                    && posd_item.fmd_is_valid_for_sale == false)
                    {
                        dr.Cells["fmd_link"].Style.BackColor = Color.Red;
                        dr.Cells["fmd_link"].Style.SelectionBackColor = Color.Red;
                    }
                    else
                    {
                        dr.Cells["fmd_link"].Style.BackColor = Color.White;
                        dr.Cells["fmd_link"].Style.SelectionBackColor = Color.White;
                    }
                }
                gvFilter.Refresh();
            }
            else
            {
                gvFilter.DataSource = null;
                lblRecordsStatus.Text = currentPageIndex + " / " + currentPageIndex;
                currentPageIndex = 0;
            }

            enable_navigation();
        }

        private void GridView_order()
        {
            string[] array = new string[13]
            {
                "selected",
                "fmd_link",
                "id",
                "barcode",
                "barcodename",
                "qty",
                "price",
                "discount",
                "pricediscounted",
                "vat",
                "sumincvat",
                "recipe2",
                "returnedqty"
            };
            //show columns
            for (int i = 0; i < array.Count(); i++)
                gvFilter.Columns[array[i]].DisplayIndex = i;

            //hide columns
            foreach (DataGridViewColumn dc in gvFilter.Columns)
            {
                if (!array.Contains(dc.DataPropertyName))
                    dc.Visible = false;
                if (dc.DataPropertyName == "fmd_link" && Session.Params.FirstOrDefault(x => x.system == "FMD" && x.par == "ENABLED")?.value != "1")
                    dc.Visible = false;
            }
        }

        private void btnPrevious_Click(object sender, EventArgs e)
        {
            form_wait(true);
            currentPageIndex--;
            btnNext.Enabled = true;
            btnLast.Enabled = true;
            BindGrid();
            form_wait(false);
        }

        private void btnFirst_Click(object sender, EventArgs e)
        {
            form_wait(true);
            currentPageIndex = 1;
            btnPrevious.Enabled = false;
            btnNext.Enabled = true;
            btnLast.Enabled = true;
            btnFirst.Enabled = false;
            BindGrid();
            form_wait(false);
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            form_wait(true);
            currentPageIndex++;
            btnFirst.Enabled = true;
            btnPrevious.Enabled = true;
            BindGrid();
            form_wait(false);
        }

        private void btnLast_Click(object sender, EventArgs e)
        {
            form_wait(true);
            currentPageIndex = (int)Math.Ceiling(Convert.ToDecimal(posd_list.Count<Items.KAS.posd>()) / pageSize);
            btnFirst.Enabled = true;
            btnPrevious.Enabled = true;
            btnNext.Enabled = false;
            btnLast.Enabled = false;
            BindGrid();
            form_wait(false);
        }

        private void enable_navigation()
        {
            if (currentPageIndex == (int)Math.Ceiling(Convert.ToDecimal(posd_list.Count<Items.KAS.posd>()) / pageSize))
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

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void gvFilter_CurrentCellChanged(object sender, EventArgs e)
        {
            DataGridView dgv = sender as DataGridView;
            if (dgv.CurrentRow != null)
            {
                current_row = dgv.CurrentRow.Index;
                current_column = dgv.CurrentCell.ColumnIndex;
            }
        }

        private void gvFilter_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            DataGridView dgv = sender as DataGridView;
            if (e.ColumnIndex != -1 && e.RowIndex != -1 && e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                dgv.CurrentCell = dgv.Rows[e.RowIndex].Cells[e.ColumnIndex];
                var relativeMousePosition = dgv.PointToClient(Cursor.Position);
                dgvContextMenu.Show(dgv, relativeMousePosition);
            }
        }

        private void gvFilter_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            if (whole_bill == 1)
                e.Cancel = true;
        }

        private void gvFilter_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (formWaiting)
                return;

            if (e.RowIndex > -1 && e.ColumnIndex == gvFilter.Rows[e.RowIndex].Cells["selected"].ColumnIndex)
            {
                if (gvFilter.Rows[e.RowIndex].Cells["selected"].Value.ToDecimal() == 1 && insurance_item == null)
                    gvFilter.Rows[e.RowIndex].Cells["qty"].ReadOnly = false;
                else
                    gvFilter.Rows[e.RowIndex].Cells["qty"].ReadOnly = true;
            }
            if (e.RowIndex > -1 && e.ColumnIndex == gvFilter.Rows[e.RowIndex].Cells["qty"].ColumnIndex)
            {
                Items.KAS.posd gv_item = (Items.KAS.posd)gvFilter.Rows[e.RowIndex].DataBoundItem;
                if (gv_item.qty <= 0 || gv_item.qty > gv_item.qty_orig)
                {
                    helpers.alert(Enumerator.alert.warning, "Grąžinamas kiekis negali būti didesnis nei parduotas kiekis.");
                    gvFilter.Rows[e.RowIndex].Cells["qty"].Value = gv_item.qty_orig;
                    gvFilter.Rows[e.RowIndex].Cells["vat"].Value = gv_item.vat_orig;
                    gvFilter.Rows[e.RowIndex].Cells["sumincvat"].Value = gv_item.sumincvat_orig;
                }
                else
                {
                    decimal suma_vnt = gv_item.sumincvat_orig / gv_item.qty_orig;
                    decimal suma_new_qty = Math.Round(gv_item.qty * suma_vnt, 2, MidpointRounding.AwayFromZero);
                    decimal new_vat = Math.Round(suma_new_qty * gv_item.vatsize / (100 + gv_item.vatsize), 2, MidpointRounding.AwayFromZero);
                    gvFilter.Rows[e.RowIndex].Cells["vat"].Value = new_vat;
                    gvFilter.Rows[e.RowIndex].Cells["sumincvat"].Value = suma_new_qty;
                }
            }
        }
        private void gvFilter_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (formWaiting == true)
                return;
            if (e.RowIndex > -1
                && Session.Params.FirstOrDefault(x => x.system == "FMD" && x.par == "ENABLED")?.value == "1"
                && e.ColumnIndex == gvFilter.Rows[e.RowIndex].Cells["fmd_link"].ColumnIndex
                && gvFilter.Rows[e.RowIndex].Cells["selected"].Value.ToDecimal() == 1)
            {
                Items.KAS.posd pd = (Items.KAS.posd)gvFilter.Rows[e.RowIndex].DataBoundItem;
                var vm = new wpf.ViewModel.FMDkas_posd()
                {
                    CurrentPosdRow = new Items.posd()
                    {
                        id = pd.id,
                        hid = pd.hid,
                        productid = pd.productid,
                        barcode = pd.barcode,
                        fmd_model = pd?.fmd_model?.Where(w => w.type == "decommission")?.ToList()
                    },
                    fmd_models_log = pd?.fmd_model?.Where(w => w.type != "decommission")?.ToList()
                };
                var wpf = new wpf.View.FMDposd()
                {
                    DataContext = vm
                };
                using (var dlg = new POS_display.Popups.wpf_dlg(wpf, "FMD eilutės langas"))
                {
                    dlg.Location = helpers.middleScreen(this, dlg);
                    dlg.ShowDialog();
                }
                pd.fmd_model = new List<POS_display.wpf.Model.fmd>();
                pd.fmd_model.AddRange(vm.fmd_models_log);
                pd.fmd_model.AddRange(vm.CurrentPosdRow.fmd_model);
                BindGrid();
            }
        }

        private void kopijuotiToolStripMenuItem_Click(object sender, EventArgs e)
        {
            helpers.CopyRowCell(gvFilter, current_row, current_column);
        }

        private async Task<bool> IsEnoughCashToReturn(decimal returnSum)
        {
            if (Session.Devices.fiscal == 0m)
                return true;
            var cashAmount = await _posUtils.GetCashAmount();
            return returnSum <= cashAmount;
        }

        private bool CanReturnSingleItem(int selectedAmount)
        {
            var isLoyaltyOrInsurance = posd_list.Any(l => l.productid == LoyaltyPointsProductId || l.productid == InsuranceProductId);
            if (!isLoyaltyOrInsurance) 
                return true;
            var needToSelectCount = gvFilter.Rows.Cast<DataGridViewRow>().Count(row => Convert.ToDecimal(row.Cells["barcodeid"].Value) != 0m);
            return selectedAmount == needToSelectCount;
        }

        private async void btnReturn_Click(object sender, EventArgs e)
        {
            if (formWaiting)
                return;
            int selected = 0;
            foreach (DataGridViewRow dr in gvFilter.Rows)
                selected += dr.Cells["selected"].Value.ToInt();

            if (selected <= 0)
            {
                helpers.alert(Enumerator.alert.error, "Nepasirinkta nei viena prekė, kurią norite grąžinti.");
                return;
            }

            if (!CanReturnSingleItem(selected))
            {
                helpers.alert(Enumerator.alert.error, "Privalo būti pažymėtos visos prekės esančios kvite!");
                return;
            }

            var mapper = Program.ServiceProvider.GetRequiredService<IMapper>();
            var selectedReason = (KeyValuePair<string, string>)cmbReason.SelectedItem;
            var selectedItemsToReturn = mapper.Map<List<ReturningItem>>(posd_list.Where(l => l.productid != LoyaltyPointsProductId && l.selected == 1).ToList());
            var presentCardSum = posd_list.Where(val => val.productid == PresentCardProductId).Sum(val => val.priceincvat);
            var totalSumToReturn = selectedItemsToReturn.Sum(val => val.SumWithVAT).ToDecimal();
            totalSumToReturn -= presentCardSum;

            if (!await IsEnoughCashToReturn(totalSumToReturn))
            {
                helpers.alert(Enumerator.alert.error,
                    "Grąžinimo komandos atlikti negalima, nes nepakankamas grynųjų pinigų likutis kasoje.\n" +
                    "Atlikite pinigų įdėjimo operaciją.");
                return;
            }

            form_wait(true);
            /*List<Items.Loyalty.loyaltyh> loyalty_item = DB.Loyalty.GetLoyaltyh<Items.Loyalty.loyaltyh>(posh_item.id);
            if (loyalty_item.Count > 0)
            {
                //todo
            }*/
            string msg = string.Empty;
            if (insurance_item != null)
            {
                var poshItem = new Items.posh
                {
                    Id = posh_item.Id,
                    InsuranceItem = insurance_item,
                    PosdItems = (from el in posd_list
                                 select new Items.posd
                                 {
                                     id = el.id,
                                     status_insurance = ((DataTable)insurance_item.Receipt).AsEnumerable().FirstOrDefault(w => w["posd_id"].ToDecimal() == el.id)?["status"]?.ToInt() ?? -1,
                                     InsuranceInfo = el.info
                                 }).ToList()
                };
                if (await insurance_item.Utils.VoidInsuranceCompensation(poshItem))
                    msg += "\nDraudimo kompanijai pranešta apie grąžinimą!";
                else
                {
                    await DB.cheque.VoidChequeTrans(posh_item.Id);
                    msg += "\nNepamirškite informuoti draudimo kompaniją apie grąžinimo operaciją!";
                }
            }

            decimal saleh_id = await _posRepository.CreateSaleHeader();
            string doumentNo = await DB.POS.getNextSalehNo("KP" + DateTime.Now.ToString("yyMMdd") + "G");

            ReturningItemsData returningItemsData = new ReturningItemsData()
            {
                PosHeaderId = posh_item.Id,
                PharmacyNo = $"BENU {Session.SystemData.prodcustid}",
                Address = Session.SystemData.address,
                CashDeskNr = Session.Devices.deviceno,
                ChequeNr = posh_item.CheckNo,
                Cashier = Session.User.DisplayName,
                Date = DateTime.Now,
                MistakeType = selectedReason.Value,
                DocumentNo = doumentNo,
                ReturningItems = selectedItemsToReturn
            };

            ItemReturnReportView itemReturnReportView = new ItemReturnReportView(returningItemsData);
            await itemReturnReportView.Init();
            var roundingValue = await itemReturnReportView.GetRoundingValue();

            if (roundingValue != 0m) 
            {
                await _posRepository.CreateSaleDetail(
                    saleh_id,
                    RoundingItemProductId,
                    0,
                    0,
                    (-1),
                    roundingValue,
                    0,
                    roundingValue,
                    roundingValue,
                    0,
                    0,
                    0,
                    0,
                    0,
                    0,
                    0);
            }

            foreach (DataGridViewRow dr in gvFilter.Rows)
            {
                Items.KAS.posd pd = (Items.KAS.posd)dr.DataBoundItem;
                if (pd.selected == 1)
                {
                    await _posRepository.CreateSaleDetail(
                        saleh_id,
                        pd.productid,
                        pd.barcodeid,
                        pd.serialid,
                        (-1) * pd.qty,
                        pd.priceincvat * 100 / (100 + pd.vatsize),
                        pd.discount,
                        pd.pricediscounted,
                        pd.sumincvat - pd.vat,
                        pd.vat,
                        pd.f_cost_price,
                        0,
                        0,
                        0,
                        pd.vatsize,
                        pd.id);
                }
                await _posRepository.UpdateSaleHeader(saleh_id, "G", doumentNo, DateTime.Now, 1, 0, 0, 0, 0, 0, 0, 0);
                //FMD
                if (Session.Params.FirstOrDefault(x => x.system == "FMD" && x.par == "ENABLED")?.value == "1"
                    && helpers.betweenday(posh_item.DocumentDate, DateTime.Now) < (Session.Params.FirstOrDefault(x => x.system == "FMD" && x.par == "RETURN_DAYS")?.value?.ToInt() ?? 0))
                {
                    var vm = new wpf.ViewModel.FMDkas_posd()
                    {
                        CurrentPosdRow = new Items.posd()
                        {
                            id = pd.id,
                            hid = pd.hid,
                            productid = pd.productid,
                            barcode = pd.barcode
                        }
                    };
                    foreach (var f in pd.fmd_model.Where(w => w.type == "decommission" && string.IsNullOrWhiteSpace(w.Response.operationCode)))
                        await vm.ChangeStateSinglePackAsync(f, FMD.Model.State.Active, doumentNo);
                }
            }
            decimal confirmed = 0;
            confirmed = await _posRepository.ReturnSaleHeader(saleh_id);

            if (dt_BENUM.Rows.Count > 0)
            {
                //todo
                SR_Privat.Items benum_return = new SR_Privat.Items();
                benum_return.It = new List<SR_Privat.Item>();
                foreach (DataGridViewRow dr in gvFilter.Rows)
                {
                    Items.KAS.posd pd = (Items.KAS.posd)dr.DataBoundItem;
                    if (pd.selected == 1)
                    {
                        SR_Privat.Item it = new SR_Privat.Item();
                        it.PosdId = pd.id;
                        it.ProductId = pd.productid;
                        it.Qty = (double)pd.qty;
                        it.Sum = (double)pd.sumincvat;
                        benum_return.It.Add(it);
                    }
                }
                using (SR_Privat.PrivatSoapClient scPrivat = new SR_Privat.PrivatSoapClient())
                    scPrivat.CreateTransaction(dt_BENUM.Rows[0]["chnumber"].ToString(), DateTime.Now, benum_return.It.Sum<SR_Privat.Item>(i => i.Sum), Session.SystemData.kas_client_id, 3, 0, 0, posh_item.Id, benum_return, !Session.Develop);

                DataTable sfh = DB.KAS.getSFH(posh_item.Id, "K");//ar yra israsyta saskaita faktura kvitui
                if (sfh.Rows.Count > 0)
                {
                    using (InvoiceView dlg = new InvoiceView(posh_item))
                    {
                        dlg.Location = helpers.middleScreen(this, dlg);
                        dlg.ShowDialog();
                        if (dlg.DialogResult == DialogResult.OK)
                        {
                            string url = String.Format(@"http://{0}/kas/centras_devel/pardavimai/ataskaitos/sale_invoice_return_benum.php?hid={1}&partnerid={2}&checkno={3}&checkdate={4}&documentdate={5}&documentnr={6}&comment={7}",
                                        Session.Develop ? "srvdevel" : Session.ServerIP,
                                        saleh_id,
                                        dlg.CreditorId,
                                        dlg.CheckNo,
                                        dlg.CheckDate,
                                        dlg.DocumentDate,
                                        dlg.DocumentNo,
                                        sfh.Rows[0]["sask_nr"].ToString());
                            if (Session.Develop)
                                url += "&login_dbid=626";
                            else
                                url += "&login_dbid=66";

                            using (WebPage wb = new WebPage(url))
                            {
                                wb.Location = helpers.middleScreen(this, wb);
                                wb.ShowDialog();
                            }
                        }
                    }
                }
            }

            if (confirmed > 0)
            {
                await _posRepository.SetSaleHeaderTransId(saleh_id, posh_item.Id);
                List<Items.Loyalty.loyaltyh> loyaltyItems = await DB.Loyalty.GetLoyaltyh<Items.Loyalty.loyaltyh>(posh_item.Id);
                Items.Loyalty.loyaltyh loyaltyHeader = loyaltyItems.FirstOrDefault();
                if (loyaltyHeader?.card_type == "BENU" && loyaltyHeader?.accrue_points == 1m)
                {
                    var posDetails = await _posRepository.GetPosDetails(posh_item.Id);
                    var returningItems = posDetails.Where(val => returningItemsData.ReturningItems.Any(ri => ri.PosDetailId == val.id));
                    var posHeader = new Items.posh()
                    {
                        Id = posh_item.Id,
                        CheckNo = posh_item.CheckNo,
                        PosdItems = returningItems.ToList()
                    };
                    await LoadPOSHeaderCRMItem(posHeader, loyaltyHeader);
                    await Session.CRMRestUtils.SendPurchase(posHeader, true);
                }

                var posPayments = await _posRepository.GetPosPayment(posh_item.Id);
                foreach (var posPayment in posPayments)
                {
                    if (posPayment.PaymentType == PosPaymentType.PRESENTCARD)
                        await InvokePresentCard(posPayment.Code);

                    await _posRepository.VoidPosPayment(posPayment);
                }

                await _homeModeRepository.DeleteHomeDeliveryOrder(posh_item.Id);
                if (Session.getParam("ITEMRETURNFORM", "ENABLED") == "1")
                {
                    itemReturnReportView.Location = helpers.middleScreen(this, itemReturnReportView);
                    itemReturnReportView.ShowDialog();
                }
                else
                {
                    await itemReturnReportView.SendItemsReturnToCashRegister();
                }
                itemReturnReportView.Dispose();
                form_wait(false);
                helpers.alert(Enumerator.alert.info, "Grąžinimo operacija įvykdyta sėkmingai.\n" + msg);
            }
            DialogResult = DialogResult.OK;
        }

        private async Task LoadPOSHeaderCRMItem(Items.posh posHeader, Items.Loyalty.loyaltyh loyaltyHeader)
        {
            posHeader.CRMItem = new CRMData
            {
                ManualVouchers = new List<ManualVoucher>()
            };

            if (loyaltyHeader.manual_vouchers != "")
            {
                var vouchers_array = loyaltyHeader.manual_vouchers.Split(';');
                for (int i = 0; i < vouchers_array.Length; i++)
                    posHeader.CRMItem.ManualVouchers.Add(new ManualVoucher() { Code = vouchers_array[i] });
            }

            if (loyaltyHeader.card_type == "BENU")
                posHeader.CRMItem.Account = await Session.CRMRestUtils.CollectClientData(loyaltyHeader.card_no);


            posHeader.LoyaltyCardType = loyaltyHeader.card_type;
            posHeader.CRMItem.AccruePoints = (int)loyaltyHeader.accrue_points;
        }

        private async Task InvokePresentCard(string presentCardNumbers)
        {
            try
            {
                var codes = presentCardNumbers.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                var presentCards = await _tamroClient.GetAsync<List<PresentCardViewModel>>
                    (string.Format(Session.CKasV1GetPresentCard, helpers.BuildQueryString("CardNumbers=", codes.ToList())));

                if (presentCards?.Count > 0)
                {
                    foreach (var presentCard in presentCards)
                    {
                        await _tamroClient.DeleteAsync<PresentCardSellerViewModel>
                            (string.Format(Session.CKasV1DeletePresentCardSeller, presentCard.Id), new { soldByPharmacyId = 0 });
                    }
                }

            }
            catch (Exception ex)
            {
                Serilogger.GetLogger().Error(ex, ex.Message);
            }
        }

        private void posd_Load(object sender, EventArgs e)
        {
            BindGrid();
        }
    }
}
