using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace POS_display
{
    public partial class hospital : Form
    {
        private bool formWaiting = false;
        private static DataTable dtSource = new DataTable();
        private static int currentPageIndex = 0;
        private static int current_row = -1;
        private static int current_column = -1;
        private static string filter_text = "įveskite paieškos raktą";
        private static int filter_index = 0;

        private static AutoCompleteStringCollection nameCol = new AutoCompleteStringCollection();
        private static AutoCompleteStringCollection ecodeCol = new AutoCompleteStringCollection();
        private static AutoCompleteStringCollection codeCol = new AutoCompleteStringCollection();
        private static AutoCompleteStringCollection allCol = new AutoCompleteStringCollection();
        private int lastPageIndex = 0;
        private int pageSize = 15;

        public string hCode = "";
        public string hName = "";
        public decimal Id = 0;

        #region Callbacks
        private void dbHospital(bool success, DataTable t)
        {
            // we are different thread!
            this.BeginInvoke(new MethodInvoker(delegate
            {
                gvFilter.DataSource = null;
                dtSource = t;
                currentPageIndex = 1;
                BindGrid();
                form_wait(false);
            }));
        }

        private void dbAutocompleteCode(bool success, DataTable t)
        {
            // we are different thread!
            this.Invoke(new MethodInvoker(delegate
            {
                foreach (DataRow row in t.Rows)
                {
                    codeCol.Add(Convert.ToString(row["code"]));
                    allCol.Add(Convert.ToString(row["code"]));
                }
                DB.POS.asyncAutocomplete("hospital", "ecode", dbAutocompleteECode);
            }));
        }

        private void dbAutocompleteECode(bool success, DataTable t)
        {
            // we are different thread!
            this.Invoke(new MethodInvoker(delegate
            {
                foreach (DataRow row in t.Rows)
                {
                    ecodeCol.Add(Convert.ToString(row["ecode"]));
                    allCol.Add(Convert.ToString(row["ecode"]));
                }
                DB.POS.asyncAutocomplete("hospital", "name", dbAutocompleteName);
            }));
        }

        private void dbAutocompleteName(bool success, DataTable t)
        {
            // we are different thread!
            this.Invoke(new MethodInvoker(delegate
            {
                foreach (DataRow row in t.Rows)
                {
                    nameCol.Add(Convert.ToString(row["name"]));
                    allCol.Add(Convert.ToString(row["name"]));
                }
                autocomplete_source();

                if (dtSource.Rows.Count == 0)
                {
                    tbFilter.Text = "";
                    DB.recipe.asyncHospital(((KeyValuePair<string, string>)ddFilter.SelectedItem).Key, tbFilter.Text, dbHospital);
                }
                else
                    form_wait(false);
            }));
        }
        #endregion

        public hospital()
        {
            InitializeComponent();
        }

        private void hospital_Load(object sender, EventArgs e)
        {
            form_wait(true);
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            DB.POS.UpdateSession("Gydymo įstaigų lentelė", 2);
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            Dictionary<string, string> test = new Dictionary<string, string>();
            test.Add("code:ecode:name", "Pavadinimą, kodą, įmonės kodą");
            test.Add("code", "Kodą");
            test.Add("ecode", "Įmonės kodą");
            test.Add("name", "Pavadinimą");

            ddFilter.DataSource = new BindingSource(test, null);
            ddFilter.DisplayMember = "Value";
            ddFilter.ValueMember = "Key";

            tbFilter.Text = filter_text;
            ddFilter.SelectedIndex = filter_index;
            tbFilter.Select();
            enableButtons();

            if (allCol.Count == 0)
                DB.POS.asyncAutocomplete("hospital", "code", dbAutocompleteCode);
            else
            {
                BindGrid();
                autocomplete_source();
                form_wait(false);
            }
        }

        private void hospital_Closing(object sender, FormClosingEventArgs e)
        {
            if (this.formWaiting == true)
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
                this.Cursor = Cursors.WaitCursor;
                this.formWaiting = true;
                this.gvFilter.Cursor = Cursors.WaitCursor;
            }
            else
            {
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
                gvFilter.DataSource = tmptable;
                lastPageIndex = (int)Math.Ceiling(Convert.ToDecimal(dtSource.Rows.Count) / pageSize);
                lblRecordsStatus.Text = currentPageIndex + " / " + lastPageIndex;
                GridView_order();
            }
            else
            {
                lblRecordsStatus.Text = currentPageIndex + " / " + currentPageIndex;
                currentPageIndex = 0;
            }
            enableButtons();
        }

        private void GridView_order()
        {
            //show columns
            gvFilter.Columns["code"].DisplayIndex = 0;
            gvFilter.Columns["code"].HeaderText = "Kodas";
            gvFilter.Columns["code"].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            gvFilter.Columns["ecode"].DisplayIndex = 1;
            gvFilter.Columns["ecode"].HeaderText = "Įmonės kodas";
            gvFilter.Columns["ecode"].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            gvFilter.Columns["name"].DisplayIndex = 2;
            gvFilter.Columns["name"].HeaderText = "Pavadinimas";
            gvFilter.Columns["name"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            //hide columns
            gvFilter.Columns["id"].Visible = false;
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

        private void enableButtons()
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
            if (formWaiting == true)
                return;
            form_wait(true);
            string temp = tbFilter.Text.ToUpper();
            tbFilter.Text = "";
            tbFilter.Text = temp;

            DB.recipe.asyncHospital(((KeyValuePair<string, string>)ddFilter.SelectedItem).Key, tbFilter.Text, dbHospital);
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
            current_column = e.ColumnIndex;
            current_row = e.RowIndex;
        }

        private void gvFilter_DoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (formWaiting == true)
                return;
            if (e.RowIndex < 0)
                return;
            hCode = gvFilter.Rows[e.RowIndex].Cells["code"].Value.ToString();
            hName = gvFilter.Rows[e.RowIndex].Cells["name"].Value.ToString();
            Id = gvFilter.Rows[e.RowIndex].Cells["id"].Value.ToDecimal();
            this.DialogResult = DialogResult.OK;
            this.Close();
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
            if (((KeyValuePair<string, string>)ddFilter.SelectedItem).Key.Equals("code"))
                tbFilter.AutoCompleteCustomSource = codeCol;
            if (((KeyValuePair<string, string>)ddFilter.SelectedItem).Key.Equals("ecode"))
                tbFilter.AutoCompleteCustomSource = ecodeCol;
            if (((KeyValuePair<string, string>)ddFilter.SelectedItem).Key.Equals("name"))
                tbFilter.AutoCompleteCustomSource = nameCol;
            if (((KeyValuePair<string, string>)ddFilter.SelectedItem).Key.Equals("code:ecode:name"))
                tbFilter.AutoCompleteCustomSource = allCol;
            tbFilter.AutoCompleteMode = AutoCompleteMode.Suggest;
            tbFilter.AutoCompleteSource = AutoCompleteSource.CustomSource;
        }

        private void tbFilter_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.A)
            {
                tbFilter.SelectAll();
                e.SuppressKeyPress = true;
            }
        }

        private void gvFilter_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                gvFilter_DoubleClick(sender, new DataGridViewCellEventArgs(gvFilter.CurrentCell.ColumnIndex, gvFilter.CurrentCell.RowIndex));
        }

        private void kopijuotiToolStripMenuItem_Click(object sender, EventArgs e)
        {
            helpers.CopyRowCell(gvFilter, current_row, current_column);
        }
    }
}
