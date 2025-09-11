using Microsoft.Extensions.DependencyInjection;
using POS_display.Models.KAS;
using POS_display.Presenters.HomeMode;
using POS_display.Repository.HomeMode;
using POS_display.Repository.Pos;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Xml;
using Tamroutilities.Client;

namespace POS_display
{
    public partial class posh : FormBase
    {
        private bool formWaiting = false;
        private List<PosHeader> DS_list = new List<PosHeader>();
        private static int currentPageIndex = 0;
        private static int current_row = -1;
        private static int current_column = -1;
        private static string filter_text = DateTime.Now.ToString("yyyy-MM-dd");
        private static int filter_index = 0;

        private int lastPageIndex = 0;
        private int pageSize = 20;

        public posh()
        {
            InitializeComponent();
        }

        private void posh_Load(object sender, EventArgs e)
        {
            form_wait(true);
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            DB.POS.UpdateSession("Išorinių įrenginių lentelė", 2);
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            Dictionary<string, string> test = new Dictionary<string, string>();
            test.Add("documentdate2", "datą");
            test.Add("id:checkno:deviceno:debtorname:documentdate2:totalsum:type:status", "oper.nr,kvitą,kasą,pirkėją,datą,sumą,tipą,statusą");
            test.Add("id", "oper.nr");
            test.Add("checkno", "kvitą");
            test.Add("deviceno", "kasą");
            test.Add("debtorname", "pirkėją");
            test.Add("totalsum", "sumą");
            test.Add("type", "tipą");
            test.Add("status", "statusą");

            ddFilter.DataSource = new BindingSource(test, null);
            ddFilter.DisplayMember = "Value";
            ddFilter.ValueMember = "Key";

            BindGrid();
            tbFilter.Text = filter_text;
            ddFilter.SelectedIndex = filter_index;
            tbFilter.Select();
            enable_navigation();
            /*
            if (allCol.Count == 0)
                DB.POS.asyncAutocomplete("search_posh_new", "id, checkno, deviceno, debtorname, documentdate2, totalsum, type, status", dbAutocomplete);
            else
            {
                autocomplete_source();
                form_wait(false);
            }*/
            btnGlobalBlue.Visible = Session.getParam("GLOBALBLUE", "ENABLED") == "1";
            form_wait(false);
            if (tbFilter.Text != "" && gvFilter.DataSource == null)
                btnFilter_Click(new object(), new EventArgs());
        }

        private void posh_Closing(object sender, FormClosingEventArgs e)
        {
            if (formWaiting == true)
                e.Cancel = true;
            else
            {
                //remember values
                filter_index = ddFilter.SelectedIndex;
                filter_text = tbFilter.Text;
            }
        }

        private void form_wait(bool wait)
        {
            if (wait == true)
            {
                this.UseWaitCursor = true;
                this.Cursor = Cursors.WaitCursor;
                this.formWaiting = true;
                this.gvFilter.Cursor = Cursors.WaitCursor;
            }
            else
            {
                this.UseWaitCursor = false;
                this.Cursor = Cursors.Default;
                this.formWaiting = false;
                this.gvFilter.Cursor = Cursors.Hand;
            }
        }

        public void BindGrid()
        {
            int row_before = current_row;
            if (DS_list != null && DS_list.Count<PosHeader>() > 0)
            {
                int startIndex = (currentPageIndex - 1) * pageSize;
                int endIndex = (currentPageIndex - 1) * pageSize + pageSize;
                if (endIndex > DS_list.Count<PosHeader>())
                    endIndex = DS_list.Count<PosHeader>();
                gvFilter.AutoGenerateColumns = true;//workaround
                gvFilter.DataSource = DS_list.Skip(startIndex).Take(endIndex).ToList<PosHeader>();
                gvFilter.AutoGenerateColumns = false;//workaround
                GridView_order();
                lastPageIndex = (int)Math.Ceiling(Convert.ToDecimal(DS_list.Count<PosHeader>()) / pageSize);
                lblRecordsStatus.Text = currentPageIndex + " / " + lastPageIndex;
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
            //show columns
            gvFilter.Columns["id"].DisplayIndex = 0;
            gvFilter.Columns["deviceno"].DisplayIndex = 1;
            gvFilter.Columns["checkno"].DisplayIndex = 2;
            gvFilter.Columns["documentdate2"].DisplayIndex = 3;
            gvFilter.Columns["debtorname"].DisplayIndex = 4;
            gvFilter.Columns["totalsum"].DisplayIndex = 5;
            gvFilter.Columns["vat"].DisplayIndex = 6;
            gvFilter.Columns["sumincvat"].DisplayIndex = 7;
            gvFilter.Columns["type"].DisplayIndex = 8;
            gvFilter.Columns["status"].DisplayIndex = 9;
            gvFilter.Columns["sf"].DisplayIndex = 10;
            //hide columns
            gvFilter.Columns["documentdate"].Visible = false;
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
            currentPageIndex = (int)Math.Ceiling(Convert.ToDecimal(DS_list.Count<PosHeader>()) / pageSize);
            btnFirst.Enabled = true;
            btnPrevious.Enabled = true;
            btnNext.Enabled = false;
            btnLast.Enabled = false;
            BindGrid();
            form_wait(false);
        }

        private void enable_navigation()
        {            
            if (currentPageIndex == (int)Math.Ceiling(Convert.ToDecimal(DS_list.Count<PosHeader>()) / pageSize))
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

        private async void btnFilter_Click(object sender, EventArgs e)
        {
            if (formWaiting == true)
                return;
            string temp = tbFilter.Text.ToUpper();
            tbFilter.Text = "";
            tbFilter.Text = temp;

            if (tbFilter.Text.Length > 2)
            {
                form_wait(true);
                DS_list = await DB.KAS.asyncSearchPosh<PosHeader>(((KeyValuePair<string, string>)ddFilter.SelectedItem).Key, tbFilter.Text);
                currentPageIndex = 1;
                BindGrid();
                tbFilter.SelectAll();
                form_wait(false);
            }
            else
            {
                helpers.alert(Enumerator.alert.warning, "Įveskite ilgesnę paieškos frazę!");
                tbFilter.Focus();
            }
        }

        private void gvFilter_CurrentCellChanged(object sender, EventArgs e)
        {
            DataGridView dgv = sender as DataGridView;
            if (dgv.CurrentRow != null)
            {
                current_row = gvFilter.CurrentRow.Index;
                current_column = gvFilter.CurrentCell.ColumnIndex;
                btnInvoice.Enabled = (gvFilter.Rows[current_row].DataBoundItem as PosHeader)?.Status != "N";
                btnGlobalBlue.Enabled = (gvFilter.Rows[current_row].DataBoundItem as PosHeader)?.Status != "N";
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

        private void gvFilter_DoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (formWaiting == true)
                return;
            if (e.RowIndex < 0)
                return;
            OpenPosd((PosHeader)gvFilter.Rows[e.RowIndex].DataBoundItem);
        }
        
        private void ddFilter_Changed(object sender, EventArgs e)
        {
            if (formWaiting == true)
                return;
            tbFilter.Select();
            //autocomplete_source();
        }
        
        private void tbFilter_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                btnFilter_Click(sender, new EventArgs());
            if (e.Control && e.KeyCode == Keys.A)
            {
                tbFilter.SelectAll();
                e.SuppressKeyPress = true;
            }
        }

        private void gvFilter_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && gvFilter.RowCount > 0)
                if (gvFilter.CurrentCell == null)
                    gvFilter_DoubleClick(sender, new DataGridViewCellEventArgs(0, 0));
                else
                    gvFilter_DoubleClick(sender, new DataGridViewCellEventArgs(gvFilter.CurrentCell.ColumnIndex, gvFilter.CurrentCell.RowIndex));
        }

        private void kopijuotiToolStripMenuItem_Click(object sender, EventArgs e)
        {
            helpers.CopyRowCell(gvFilter, current_row, current_column);
        }

        private void btnCheque_Click(object sender, EventArgs e)
        {
            if (formWaiting == true)
                return;
            if (current_row < 0)
                return;

            OpenPosd((PosHeader)gvFilter.Rows[current_row].DataBoundItem);
        }

        private async void OpenPosd(PosHeader posh_item)
        {
            await ExecuteWithWaitAsync(async () =>
            {
                using (posd dlg = new posd(posh_item,
                        new PosRepository(),
                        Program.ServiceProvider.GetRequiredService<ITamroClient>(),
                        Session.FP550,
                        new HomeModeRepository()))
                {
                    await dlg.Init();
                    dlg.Location = helpers.middleScreen(this, dlg);
                    dlg.ShowDialog();
                }
            });
        }

        private void btnInvoice_Click(object sender, EventArgs e)
        {
            if (formWaiting == true)
                return;
            PosHeader current_posh_row = (PosHeader)gvFilter.Rows[current_row].DataBoundItem;
            DataTable sfh = DB.KAS.getSFH(current_posh_row.Id, "K");
            if (sfh.Rows.Count > 0)
            {
                string url = String.Format(@"http://{0}/kas/centras_devel/kasos/ataskaitos/sale_invoice.php?buttons=1&hid={1}&partnerid={2}&checkno={3}&documentdate={4}&documentnr={5}&comment={6}&login_dbid={7}",
                                                Session.Develop == true ? "srvdevel" : Session.ServerIP,
                                                sfh.Rows[0]["check_id"].ToString(), 
                                                sfh.Rows[0]["buyer_id"].ToString(), 
                                                sfh.Rows[0]["check_no"].ToString(), 
                                                sfh.Rows[0]["documentdate"].ToString().Substring(0, 10).Replace('-', '.'), 
                                                sfh.Rows[0]["sask_nr"].ToString(),
                                                sfh.Rows[0]["comment"].ToString(),
                                                Session.DBId);
                WebPage wb = new WebPage(url);
                wb.Location = helpers.middleScreen(this, wb);
                wb.ShowDialog();
                if (wb.DialogResult == DialogResult.OK)
                {

                }
                wb.Dispose();
                wb = null;
            }
            else
            {
                InvoiceView dlg = new InvoiceView(current_posh_row);
                dlg.Location = helpers.middleScreen(this, dlg);
                dlg.ShowDialog();
                if (dlg.DialogResult == DialogResult.OK)
                {
                    string card_no = "";
                    DataTable dt_BENUM = DB.KAS.getBENUMtransaction(current_posh_row.Id, 7419);
                    if (dt_BENUM.Rows.Count > 0)
                        card_no = dt_BENUM.Rows[0]["chnumber"].ToString();
                    string url = String.Format(@"http://{0}/kas/centras_devel/kasos/ataskaitos/sale_invoice.php?hid={1}&partnerid={2}&checkno={3}&checkdate={4}&documentdate={5}&documentnr={6}&comment={7}&login_dbid={8}",
                                Session.Develop == true ? "srvdevel" : Session.ServerIP,
                                current_posh_row.Id,
                                dlg.CreditorId,
                                dlg.CheckNo.Text,
                                dlg.CheckDate.Text,
                                dlg.DocumentDate.Text.Replace('-', '.'),
                                dlg.DocumentNo.Text,
                                card_no,
                                Session.DBId);
                    WebPage wb = new WebPage(url);
                    wb.Location = helpers.middleScreen(this, wb);
                    wb.ShowDialog();
                    if (wb.DialogResult == DialogResult.OK)
                    {

                    }
                    wb.Dispose();
                    wb = null;
                }
                dlg.Dispose();
                dlg = null;
            }
        }

        private async void btnGlobalBlue_Click(object sender, EventArgs e)
        {
            await ExecuteWithWaitAsync(async () =>
            {
                string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                PosHeader currentPosHeader = (PosHeader)gvFilter.Rows[current_row].DataBoundItem;
                var posDetails = await new PosRepository().GetPosDetails(currentPosHeader.Id);

                XmlDocument xmlDoc = new XmlDocument();
                XmlElement globalBlueElement = xmlDoc.CreateElement("GlobalBlue");
                XmlElement purchaseDetailsElement = xmlDoc.CreateElement("PurchaseDetails");
                globalBlueElement.AppendChild(purchaseDetailsElement);

                XmlElement receiptsElement = xmlDoc.CreateElement("Receipts");
                purchaseDetailsElement.AppendChild(receiptsElement);

                XmlElement receiptElement = xmlDoc.CreateElement("Receipt");
                receiptsElement.AppendChild(receiptElement);

                XmlElement receiptDateElement = xmlDoc.CreateElement("ReceiptDate");
                receiptDateElement.InnerText = currentPosHeader.DocumentDate.ToString("yyyy-MM-dd");
                receiptElement.AppendChild(receiptDateElement);

                XmlElement receiptNumberElement = xmlDoc.CreateElement("ReceiptNumber");
                receiptNumberElement.InnerText = currentPosHeader.CheckNo;
                receiptElement.AppendChild(receiptNumberElement);

                foreach (var posDetail in posDetails)
                {
                    XmlElement purchaseItemElement = xmlDoc.CreateElement("PurchaseItem");
                    AddElement(xmlDoc, purchaseItemElement, "VatRate", posDetail.vatsize.ToString().Replace(",","."));
                    AddElement(xmlDoc, purchaseItemElement, "GrossAmount", posDetail.sum.ToString().Replace(",", "."));
                    AddElement(xmlDoc, purchaseItemElement, "Quantity", posDetail.qty.ToString().Replace(",", "."));
                    AddElement(xmlDoc, purchaseItemElement, "GoodDescription", posDetail.barcodename.Length <= 50 ?
                        posDetail.barcodename:
                        posDetail.barcodename.Substring(0, 50));
                    receiptElement.AppendChild(purchaseItemElement);
                }

                xmlDoc.AppendChild(globalBlueElement);

                string xmlFilePath = Path.Combine(desktopPath, "globalblue.xml");
                xmlDoc.Save(xmlFilePath);
                helpers.alert(Enumerator.alert.info, $"Kvitas {currentPosHeader.CheckNo} eksportuotas");
            });
        }

        private void AddElement(XmlDocument xmlDoc, XmlElement parentElement, string elementName, string elementValue)
        {
            XmlElement newElement = xmlDoc.CreateElement(elementName);
            newElement.InnerText = elementValue;
            parentElement.AppendChild(newElement);
        }
    }
}
