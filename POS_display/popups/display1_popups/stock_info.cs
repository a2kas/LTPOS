using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using POS_display.Repository.Pos;
using System.Threading;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Tamroutilities.Client;
using POS_display.Models.TransactionService.Stock;

namespace POS_display
{
    public partial class stock_info : Form
    {
        private bool formWaiting = false;
        private static DataTable dtSource = new DataTable();
        private static int currentPageIndex = 0;
        private static int current_row = -1;
        private static int current_column = -1;
        private static string filter_text = "įveskite paieškos raktą";
        private static bool comensation = false;
        private static int filter_index = 0;
        private static int autocomplete_counter = 0;

        private static AutoCompleteStringCollection productnameCol = new AutoCompleteStringCollection();
        private static AutoCompleteStringCollection allCol = new AutoCompleteStringCollection();
        private static AutoCompleteStringCollection barcodeCol = new AutoCompleteStringCollection();
        private static AutoCompleteStringCollection atccodeCol = new AutoCompleteStringCollection();
        private static AutoCompleteStringCollection atcnameCol = new AutoCompleteStringCollection();
        private static AutoCompleteStringCollection tlkIdCol = new AutoCompleteStringCollection();
        private static AutoCompleteStringCollection productname2Col = new AutoCompleteStringCollection();
        private int lastPageIndex = 0;
        private int pageSize = 10;
        private string compensated = "";

        public decimal selBarcodeId = 0;
        public string selQty = "";
        public string selBarcode = "";
        public string selProductName = "";
        public decimal selProductId = 0;
        public string selATCcode = "";
        public string caller = "";

        private readonly PosRepository _posRepository;

        #region Callbacks
        private void dbSearchStockinfo5(bool success, DataTable t)
        {
            // we are different thread!
            this.BeginInvoke(new MethodInvoker(delegate
            {
                gvFilter.DataSource = null;
                dtSource = t;
                currentPageIndex = 1;
                BindGrid();
                tbFilter.SelectAll();
                form_wait(false);
            }));
        }

        private void dbAutocompleteName(bool success, List<string> data)
        {
            // we are different thread!
            this.Invoke(new MethodInvoker(delegate
            {
                foreach (string value in data)
                {
                    productnameCol.Add(value);
                    allCol.Add(value);
                }
                autocomplete_counter++;  
            }));
        }

        private void dbAutocompleteATCcode(bool success, List<string> data)
        {
            // we are different thread!
            this.Invoke(new MethodInvoker(delegate
            {
                foreach (string value in data)
                {
                    atccodeCol.Add(value);
                    allCol.Add(value);
                }
                autocomplete_counter++;
            }));
        }

        private void dbAutocompleteATCname(bool success, List<string> data)
        {
            // we are different thread!
            this.Invoke(new MethodInvoker(delegate
            {
                foreach (string value in data)
                {
                    atcnameCol.Add(value);
                    allCol.Add(value);
                }
                autocomplete_counter++;
            }));
        }

        private void dbAutocompleteName2(bool success, List<string> data)
        {
            // we are different thread!
            this.Invoke(new MethodInvoker(delegate
            {
                foreach (string value in data)
                {
                    productname2Col.Add(value);
                    allCol.Add(value);
                }
                autocomplete_counter++;
            }));
        }

        private void dbAutocompleteBarcode(bool success, List<string> data)
        {
            // we are different thread!
            this.Invoke(new MethodInvoker(delegate
            {
                foreach (string value in data)
                {
                    barcodeCol.Add(value);
                    allCol.Add(value);
                }
                autocomplete_counter++;
            }));
        }

        private void dbAutocompleteTlkId(bool success, List<string> data)
        {
            // we are different thread!
            this.Invoke(new MethodInvoker(delegate
            {
                foreach (string value in data)
                {
                    tlkIdCol.Add(value);
                }
                autocomplete_counter++;
            }));
        }
        #endregion

        public stock_info()
        {
            InitializeComponent();
            _posRepository = new PosRepository();
        }

        private void stock_info_Load(object sender, EventArgs e)
        {
            form_wait(true);
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            DB.POS.UpdateSession("Prekės informacijos lentelė", 2);
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            Dictionary<string, string> test = new Dictionary<string, string>
            {
                { "productname", "prekės pavadinimą" },
                { "productname:barcode:atccode:atcname:productname2", "pr. pavad., barkodą, atc kodą ir t.t." },
                { "barcode", "barkodą" },
                { "tlkid", "TLK ID" },
                { "atccode", "atc kodą" },
                { "atcname", "atc pavadinimą" },
                { "productname2", "tarptautinį pavadinimą" }
            };

            ddFilter.DataSource = new BindingSource(test, null);
            ddFilter.DisplayMember = "Value";
            ddFilter.ValueMember = "Key";

            tbFilter.Text = filter_text;
            cbComp.Checked = comensation;
            ddFilter.SelectedIndex = filter_index;
            tbFilter.Select();
            enable_navigation();
            BindGrid();
            if (allCol.Count == 0)
            {
                List<Task> tasks = new List<Task>
                {
                    Task.Factory.StartNew(() => _posRepository.AsyncAutoComplete("stock", "name", dbAutocompleteName)),
                    Task.Factory.StartNew(() => _posRepository.AsyncAutoComplete("atc", "code", dbAutocompleteATCcode)),
                    Task.Factory.StartNew(() => _posRepository.AsyncAutoComplete("atc", "name", dbAutocompleteATCname)),
                    Task.Factory.StartNew(() => _posRepository.AsyncAutoComplete("stock", "name2", dbAutocompleteName2)),
                    Task.Factory.StartNew(() => _posRepository.AsyncAutoComplete("barcode", "barcode", dbAutocompleteBarcode)),
                    Task.Factory.StartNew(() => _posRepository.AsyncAutoComplete("tlk_kainos_h", "tlkid", dbAutocompleteTlkId))
                };
                Task.Factory.ContinueWhenAll(tasks.ToArray(), (val) => { Invoke((Action)(() => autocomplete_source())); }); 
            }
            else
                autocomplete_source();
        }

        private void stock_info_Closing(object sender, FormClosingEventArgs e)
        {
            if (formWaiting == true || autocomplete_counter < 6)
                e.Cancel = true;
            else
            {
                //remember values
                filter_index = ddFilter.SelectedIndex;
                filter_text = tbFilter.Text;
                comensation = cbComp.Checked;
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
            if (dtSource.Rows.Count > 0)
            {
                DataTable tmptable = dtSource.Clone();
                int startIndex = (currentPageIndex - 1) * pageSize;
                int endIndex = (currentPageIndex - 1) * pageSize + pageSize;
                if (endIndex > dtSource.Rows.Count)
                {
                    endIndex = dtSource.Rows.Count;
                }
                for (int i = startIndex; i < endIndex; i++)
                {
                    DataRow newRow = tmptable.NewRow();
                    GetNewRow(ref newRow, dtSource.Rows[i]);
                    tmptable.Rows.Add(newRow);
                }
                AssignTamroQty(tmptable);
                gvFilter.DataSource = tmptable;
                lastPageIndex = (int)Math.Ceiling(Convert.ToDecimal(dtSource.Rows.Count) / pageSize);
                lblRecordsStatus.Text = currentPageIndex + " / " + lastPageIndex;
                tbQty.ReadOnly = false;
                tbRatio.ReadOnly = false;
                GridView_order();
            }
            else
            {
                lblRecordsStatus.Text = currentPageIndex + " / " + currentPageIndex;
                currentPageIndex = 0;
                tbQty.ReadOnly = true;
                tbRatio.ReadOnly = true;
            }
                
            enable_navigation();
        }

        private void GridView_order()
        {
            //show columns
            gvFilter.Columns["rusprior"].DisplayIndex = 0;
            gvFilter.Columns["rusprior"].HeaderText = "Pri";
            gvFilter.Columns["rusprior"].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            gvFilter.Columns["barcode"].DisplayIndex = 1;
            gvFilter.Columns["barcode"].HeaderText = "Barkodas";
            gvFilter.Columns["barcode"].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            gvFilter.Columns["vatsize"].DisplayIndex = 2;
            gvFilter.Columns["vatsize"].HeaderText = "PVM";
            gvFilter.Columns["vatsize"].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            gvFilter.Columns["productname"].DisplayIndex = 3;
            gvFilter.Columns["productname"].HeaderText = "Prekės pavadinimas";
            gvFilter.Columns["productname"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            gvFilter.Columns["basicprice"].DisplayIndex = 4;
            gvFilter.Columns["basicprice"].HeaderText = "B. kaina";
            gvFilter.Columns["basicprice"].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            gvFilter.Columns["salesprice"].DisplayIndex = 5;
            gvFilter.Columns["salesprice"].HeaderText = "Pardavimo kaina";
            gvFilter.Columns["salesprice"].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            gvFilter.Columns["retailprice"].DisplayIndex = 6;
            gvFilter.Columns["retailprice"].HeaderText = "M. kaina";
            gvFilter.Columns["retailprice"].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            gvFilter.Columns["qty"].DisplayIndex = 7;
            gvFilter.Columns["qty"].HeaderText = "Likutis";
            gvFilter.Columns["qty"].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            gvFilter.Columns["oficina"].DisplayIndex = 8;
            gvFilter.Columns["oficina"].HeaderText = "Vieta 1";
            gvFilter.Columns["oficina"].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            gvFilter.Columns["oficina_2"].DisplayIndex = 9;
            gvFilter.Columns["oficina_2"].HeaderText = "Vieta 2";
            gvFilter.Columns["oficina_2"].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            gvFilter.Columns["stock"].DisplayIndex = 10;
            gvFilter.Columns["stock"].HeaderText = "Vieta 3";
            gvFilter.Columns["stock"].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            gvFilter.Columns["tamro_qty"].DisplayIndex = 11;
            gvFilter.Columns["tamro_qty"].HeaderText = "Tamro likutis";
            gvFilter.Columns["tamro_qty"].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            //hide columns
            gvFilter.Columns["rus_prior"].Visible = false;
            gvFilter.Columns["id"].Visible = false;
            gvFilter.Columns["barcodeid"].Visible = false;
            gvFilter.Columns["productid"].Visible = false;
            gvFilter.Columns["productname2"].Visible = false;
            gvFilter.Columns["atccode"].Visible = false;
            gvFilter.Columns["atcname"].Visible = false;
            gvFilter.Columns["atcgroup"].Visible = false;
            gvFilter.Columns["salesvatid"].Visible = false;
            gvFilter.Columns["ratio"].Visible = false;
            gvFilter.Columns["tlkid"].Visible = false;
            gvFilter.Columns["info"].Visible = false;
            gvFilter.Columns["pk1"].Visible = false;
            gvFilter.Columns["pk2"].Visible = false;
            gvFilter.Columns["pk3"].Visible = false;
            gvFilter.Columns["pk4"].Visible = false;
            gvFilter.Columns["tamro_qty"].Visible = Session.HomeMode;
        }
        
        private void GetNewRow(ref DataRow newRow, DataRow source)
        {
            foreach (DataColumn col in dtSource.Columns)
            {
                newRow[col.ColumnName] = source[col.ColumnName];
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
            currentPageIndex = (int)Math.Ceiling(Convert.ToDecimal(dtSource.Rows.Count) / pageSize);
            btnFirst.Enabled = true;
            btnPrevious.Enabled = true;
            btnNext.Enabled = false;
            btnLast.Enabled = false;
            BindGrid();
            form_wait(false);
        }

        private void enable_navigation()
        {
            if (currentPageIndex == (int)Math.Ceiling(Convert.ToDecimal(dtSource.Rows.Count) / pageSize))
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

        private void btnFilter_Click(object sender, EventArgs e)
        {
            if (formWaiting)
                return;
            string temp = tbFilter.Text.ToUpper();
            tbFilter.Text = "";
            tbFilter.Text = temp;

            if (tbFilter.Text.Length > 2)
            {
                form_wait(true);
                DB.POS.asyncSearchStockinfo5(((KeyValuePair<string, string>)ddFilter.SelectedItem).Key, tbFilter.Text, compensated, dbSearchStockinfo5);
            }
            else
            {
                helpers.alert(Enumerator.alert.warning, "Įveskite ilgesnę paieškos frazę!");
                tbFilter.Focus();
            }
        }

        private void cbComp_CheckedChanged(object sender, EventArgs e)
        {
            if (cbComp.Checked)
                compensated = " and (basicprice>0) ";
            else
                compensated = "";
        }

        private void gvFilter_CurrentCellChanged(object sender, EventArgs e)
        {
            DataGridView dgv = sender as DataGridView;
            if (dgv.CurrentRow != null)
            {
                current_row = gvFilter.CurrentRow.Index;
                current_column = gvFilter.CurrentCell.ColumnIndex;
                lblATCGroup.Text = gvFilter.Rows[current_row].Cells["atcgroup"].Value.ToString();
                lblATCName.Text = gvFilter.Rows[current_row].Cells["atcname"].Value.ToString();
                lblProductName2.Text = gvFilter.Rows[current_row].Cells["productname2"].Value.ToString();
                lblInfo.Text = gvFilter.Rows[current_row].Cells["info"].Value.ToString();

                double qty = 0;
                if (double.TryParse(tbQty.Text.Replace('.', ','), out qty))
                {
                    double ratio = double.Parse(gvFilter.Rows[current_row].Cells["ratio"].Value.ToString());
                    tbRatio.Text = Math.Round(ratio).ToString();

                    double pk = 0;
                    double.TryParse(gvFilter.Rows[current_row].Cells[Session.PriceClass].Value.ToString(), out pk);
                    double ret_price = 0;
                    double.TryParse(gvFilter.Rows[current_row].Cells["retailprice"].Value.ToString(), out ret_price);
                    double basic_price = 0;
                    double.TryParse(gvFilter.Rows[current_row].Cells["basicprice"].Value.ToString(), out basic_price);

                    tbPay50.Text = calc_compensation(pk, 0.5, ret_price, basic_price, qty).ToString();
                    tbPay80.Text = calc_compensation(pk, 0.8, ret_price, basic_price, qty).ToString();
                    tbPay90.Text = calc_compensation(pk, 0.9, ret_price, basic_price, qty).ToString();
                    tbPay100.Text = calc_compensation(pk, 1, ret_price, basic_price, qty).ToString();
                }
                else
                {
                    tbRatio.Text = "0";
                    tbPay50.Text = "0";
                    tbPay80.Text = "0";
                    tbPay90.Text = "0";
                    tbPay100.Text = "0";
                }
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

        private double calc_compensation(double pk, double comp, double ret_price, double basic_price, double qty)
        {
            double temp = 0;
            double sum = 0;
            if (pk == 0)
                temp = ret_price;
            else
            {
                if (ret_price > 0 && basic_price > 0)
                    temp = pk;
                else
                    temp = 0;
            }
            sum = qty * temp - qty * basic_price * comp;
            if (sum < 0)
                sum = 0;
            else
                sum = Math.Round(sum, 2);

            return sum;
        }

        private void gvFilter_DoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (formWaiting == true)
                return;
            if (e.RowIndex < 0)
                return;
            if (caller.Equals("select_barcode"))
            {
                double qty = 0;
                if (double.TryParse(tbQty.Text.Replace('.', ','), out qty))
                {
                    selBarcodeId = gvFilter.Rows[e.RowIndex].Cells["barcodeid"].Value.ToDecimal();
                    selQty = qty.ToString().Replace(',', '.');
                    selBarcode = gvFilter.Rows[e.RowIndex].Cells["barcode"].Value.ToString();
                    selProductName = gvFilter.Rows[e.RowIndex].Cells["productname"].Value.ToString();
                    selProductId = gvFilter.Rows[e.RowIndex].Cells["productid"].Value.ToDecimal();
                    selATCcode = gvFilter.Rows[e.RowIndex].Cells["atccode"].Value.ToString();
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    helpers.alert(Enumerator.alert.warning, "Neteisingai nurodytas kiekis!");
                    tbQty.Select();
                }
            }
        }

        private void tbQty_TextChanged(object sender, EventArgs e)
        {
            if (formWaiting == true)
                return;
            double qty = 0;
            if (double.TryParse(tbQty.Text.Replace('.', ','), out qty) && qty > 0)
            {
                double ratio = double.Parse(gvFilter.Rows[current_row].Cells["ratio"].Value.ToString()) * qty;
                tbRatio.Text = Math.Round(ratio).ToString();

                double pk = 0;
                double.TryParse(gvFilter.Rows[current_row].Cells[Session.PriceClass].Value.ToString(), out pk);
                double ret_price = 0;
                double.TryParse(gvFilter.Rows[current_row].Cells["retailprice"].Value.ToString(), out ret_price);
                double basic_price = 0;
                double.TryParse(gvFilter.Rows[current_row].Cells["basicprice"].Value.ToString(), out basic_price);

                tbPay50.Text = calc_compensation(pk, 0.5, ret_price, basic_price, qty).ToString();
                tbPay80.Text = calc_compensation(pk, 0.8, ret_price, basic_price, qty).ToString();
                tbPay90.Text = calc_compensation(pk, 0.9, ret_price, basic_price, qty).ToString();
                tbPay100.Text = calc_compensation(pk, 1, ret_price, basic_price, qty).ToString();
            }
            else
            {
                tbRatio.Text = "";
                tbPay50.Text = "0";
                tbPay80.Text = "0";
                tbPay90.Text = "0";
                tbPay100.Text = "0";
            }
        }

        private void tbQty_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (this.formWaiting == true)
                e.Handled = true;
            TextBox tb = (sender as TextBox);
            if (e.KeyChar == '.')
                e.KeyChar = ',';
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != ',' && e.KeyChar != '-')
                e.Handled = true;
            if (e.KeyChar == (char)Keys.Enter)
                SelectNextControl(ActiveControl, true, true, true, true);
            if ((e.KeyChar == ',') && (tb.Text.IndexOf(',') > -1))
                e.Handled = true;
            if ((e.KeyChar == ',') && (tb.Text.Length == 0))
            {
                tb.Text = "0,";
                tb.Select(tb.Text.Length, tb.Text.Length);
                e.Handled = true;
            }
        }

        private void tbRatio_TextChanged(object sender, EventArgs e)
        {
            decimal gqty = tbRatio.Text.ToDecimal();
            decimal ratio = gvFilter.Rows[current_row].Cells["ratio"].Value.ToDecimal();
            decimal qty = 0;
            if (ratio > 0 && gqty > 0)
            {
                qty = Math.Round(gqty / ratio, 3);
                tbQty.Text = qty.ToString();
                tbQty.Select(tbQty.Text.Length, tbQty.Text.Length);
            }
            else
                tbQty.Text = "";
        }

        private void tbRatio_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (this.formWaiting == true)
                e.Handled = true;
            TextBox tb = (sender as TextBox);
            if (e.KeyChar == '.')
                e.KeyChar = ',';
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != ',' && e.KeyChar != '-')
                e.Handled = true;
            if (e.KeyChar == (char)Keys.Enter)
                SelectNextControl(ActiveControl, true, true, true, true);
            if ((e.KeyChar == ','))
                e.Handled = true;
        }

        private void ddFilter_Changed(object sender, EventArgs e)
        {
            if (formWaiting == true)
                return;
            tbFilter.Select();
            autocomplete_source();
        }

        private void autocomplete_source()
        {
            string selected_key = ((KeyValuePair<string, string>)ddFilter.SelectedItem).Key;
            if (selected_key.Equals("productname"))
                tbFilter.AutoCompleteCustomSource = productnameCol;
            if (selected_key.Equals("productname:barcode:atccode:atcname:productname2"))
                tbFilter.AutoCompleteCustomSource = allCol;
            if (selected_key.Equals("barcode"))
                tbFilter.AutoCompleteCustomSource = barcodeCol;
            if (selected_key.Equals("tlkid"))
                tbFilter.AutoCompleteCustomSource = tlkIdCol;
            if (selected_key.Equals("atccode"))
                tbFilter.AutoCompleteCustomSource = atccodeCol;
            if (selected_key.Equals("atcname"))
                tbFilter.AutoCompleteCustomSource = atcnameCol;
            if (selected_key.Equals("productname2"))
                tbFilter.AutoCompleteCustomSource = productname2Col;
            tbFilter.AutoCompleteMode = AutoCompleteMode.Suggest;
            tbFilter.AutoCompleteSource = AutoCompleteSource.CustomSource;
            form_wait(false);
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

        private void AssignTamroQty(DataTable dt)
        {
            if (Session.HomeMode && dt != null)
            {
                var tamroClient = Program.ServiceProvider.GetRequiredService<ITamroClient>();
                var distinctProductIds = dt.AsEnumerable()
                                           .Select(row => row.Field<decimal>("productid").ToString())
                                           .Distinct()
                                           .ToList();

                var task = Task.Run(async () => await tamroClient.GetAsync<List<StockViewModel>>(
                    string.Format(Session.TransactionV3GetLTCountryStocks,
                                  helpers.BuildQueryString("LocalItemCodes=", distinctProductIds) +
                                  "&StockType=WholeSale")).ConfigureAwait(false));

                task.Wait();

                List<StockViewModel> stockList = task.Result;
                var stockDictionary = stockList.ToDictionary(stock => stock.ItemRetailCode, stock => stock);
                decimal.TryParse(Session.getParam("HOMEMODE", "BUFFERSIZE"), out decimal bufferSize);

                foreach (DataRow row in dt.Rows)
                {
                    var productId = row["productid"].ToString();

                    if (stockDictionary.TryGetValue(productId, out var stockModel))
                    {
                        var tamroQty = (stockModel.Wholesale?.FirstOrDefault()?.TotalQty ?? 0) - (double)bufferSize;
                        row["tamro_qty"] = tamroQty > 0 ? tamroQty : 0;
                    }
                }
                dt.AcceptChanges();
            }
        }
    }
}
