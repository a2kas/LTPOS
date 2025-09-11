using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml.Linq;

namespace POS_display
{
    public partial class ucRecipeEdit_notComp : UserControl
    {
        private bool formWaiting = false;
        public decimal hd_qty2 = 0;
        public decimal hd_prodratio = 0;
        public decimal hd_barratio = 0;
        private Items.eRecipe.Recipe in_erecipe;
        private Items.posd in_posd;
        public string out_form_action = "";

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == (Keys.Enter))
            {
                SendKeys.Send("{TAB}");
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }
        public ucRecipeEdit_notComp(Items.eRecipe.Recipe erecipe, Items.posd posd_item)
        {
            InitializeComponent();
            in_erecipe = erecipe;
            in_posd = posd_item;
        }

        private void recipe_nonComp_Load(object sender, EventArgs e)
        {
            DateTime current_date = DateTime.Now;
            tbStoreName.Text = Session.SystemData.storename;
            if (in_posd != null)//from POS
            {
                Barcode = in_posd.barcode;
                BarcodeName = in_posd.barcodename;
                Qty = in_posd.qty;
                GQty = in_posd.gqty;
                hd_qty2 = in_posd.retail_price;
                var temp = DB.recipe.getDataFromBarcode2(Barcode);
                hd_prodratio = temp.Rows[0]["prodratio"].ToDecimal();
                hd_barratio = temp.Rows[0]["barratio"].ToDecimal();
                Price = in_posd.sum;
                TotalSum = in_posd.sum;
                Doses = Math.Round(hd_qty2 * Qty);
            }
            else//not from POS
                tbBarcode.Select();
            //e receptas
            if (in_erecipe.eRecipe_PrescriptionTagsLongTag && in_erecipe.DispenseCount > 0)//jei gyd. tęsti ir išdavimas ne pirmas
            {
                Ext = 1;
                if (current_date > in_erecipe.PastValidTo.AddDays(in_erecipe.ValidationPeriod))
                    helpers.alert(Enumerator.alert.info, "Praleistas prieš tai galiojęs išdavimas.\nPrašome patikrint galiojimo laikotarpius!");
            }
            RecipeNo = in_erecipe.eRecipe_RecipeNumber;
            RecipeDate = in_erecipe.eRecipe_DateWritten;
            if (Ext == 0)
            {
                ValidFrom = in_erecipe.eRecipe_ValidFrom;
                if (in_erecipe.eRecipe_PrescriptionTagsLongTag)//jei gydymui testi pirmas isdavimas
                    ValidTill = ValidFrom.AddDays(29);
                else
                    ValidTill = in_erecipe.eRecipe_ValidTo;
            }
            else
            {
                int counter = 1;
                ValidFrom = in_erecipe.PastValidTo.AddDays(-4);
                ValidTill = ValidFrom.AddDays(RecipeValidityPeriod);
                while (current_date > ValidTill && ValidTill < in_erecipe.eRecipe_ValidTo)
                {
                    ValidFrom = ValidFrom.AddDays(counter * in_erecipe.ValidationPeriod);
                    ValidTill = ValidFrom.AddDays(RecipeValidityPeriod);
                    counter++;
                }
                if (ValidTill > in_erecipe.eRecipe_ValidTo)
                    ValidTill = in_erecipe.eRecipe_ValidTo;
                PastTillDate = in_erecipe.PastValidTo;
            }
            QtyDay = in_erecipe.eRecipe_DosePerDayQuantityValue;
            SalesDate = current_date;
            form_wait(false);
            ChangedValuesEvent("NewRecipeData_cb", new EventArgs());
        }

        public void form_wait(bool wait)
        {
            if (wait == true)
            {
                this.UseWaitCursor = true;
                this.Cursor = Cursors.WaitCursor;
                this.formWaiting = true;
            }
            else
            {
                this.UseWaitCursor = false;
                this.Cursor = Cursors.Default;
                this.formWaiting = false;
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            if (this.formWaiting == true)
                return;
            out_form_action = "recipe_closed";
            this.Parent.Controls.Remove(this);
        }

        private async void btnSave_Click(object sender, EventArgs e)
        {
            if (formWaiting == true)
                return;
            if (BarcodeName == "" || RecipeNo == 0)
            {
                helpers.alert(Enumerator.alert.warning, "Nepasirinkta prekė arba neužpildytas recepto numeris!");
                return;
            }
            form_wait(true);
            if (tbTillDate.Text != "")
            {
                await DB.eRecipe.CreateErecipe(in_posd?.hid ?? 0, in_posd?.id ?? 0, RecipeNo, "nekompensuojamas", 0, RecipeDate, SalesDate, TillDate);
                out_form_action = "recipe_saved";
                this.Parent.Controls.Remove(this);
            }
        }

        private void tbBarcode_Leave(object sender, EventArgs e)
        {
            TextBox tb = sender as TextBox;
            if (formWaiting == true)
                return;
            if (tb.ReadOnly == true)
                return;
            if (tb.Text == "" && tbBarcodeName.Text == "")
                return;
            form_wait(true);
            DataTable temp = null;
            temp = DB.recipe.getDataFromBarcode2(Barcode);
            if (temp != null && temp.Rows.Count > 0)
            {
                //hd_barcodeId = temp.Rows[0]["id"].ToDecimal());
                decimal productId = temp.Rows[0]["productid"].ToDecimal();
                hd_prodratio = temp.Rows[0]["prodratio"].ToDecimal();
                hd_barratio = temp.Rows[0]["barratio"].ToDecimal();
                hd_qty2 = hd_barratio;
                GQty = hd_barratio;
                Doses = hd_barratio;
                Barcode = tb.Text;
                BarcodeName = temp.Rows[0]["name"].ToString();
                Qty = 1;
                TotalSum = temp.Rows[0]["price"].ToDecimal();
                Price = temp.Rows[0]["price"].ToDecimal();
                selectBarcode(productId, temp.Rows[0]["atc_code"].ToString());
            }
            else
            {
                form_wait(false);
                btnSelBarcode_Click(sender, e);
                return;
            }
        }

        private void selectBarcode(decimal product_id, string atc_code)
        {
            try
            {
                if (Session.RecipesOnly == true)
                {
                    tbTotalSum.ReadOnly = false;
                    string atc_short = atc_code;
                    if (atc_short.Length >= 4)
                        atc_short = atc_short.Substring(0, 4);
                    if (in_erecipe.eRecipe.AtcCode != "" && !in_erecipe.eRecipe.AtcCode.StartsWith(atc_short))
                    {
                        ProgressDialog dlg = new ProgressDialog("dispense_warning", "Prekės ATC kodas: " + atc_code + " nesutampa su e-recepto ATC kodu: " + in_erecipe.eRecipe.AtcCode);
                        dlg.Location = helpers.middleScreen(this.ParentForm, dlg);
                        dlg.ShowDialog();
                        if (dlg.DialogResult != DialogResult.OK)
                            throw new Exception("");
                        dlg.Dispose();
                        dlg = null;
                    }
                    string NpakId7 = "";
                    IEnumerable<Items.Prices.GenericItem> items = from item in Session.Generics.ItemsAllList
                                                                  where item.kas_id == product_id
                                                                  select item;
                    if (items.Count() > 0)
                        NpakId7 = items.First().NpakId7.ToString();
                    if (NpakId7 == "1000000")//gaminami vaistai
                        NpakId7 = "";
                    eRecipeWS.DTO.MedicationDto product_medication = new eRecipeWS.DTO.MedicationDto();
                    eRecipeWS.DTO.MedicationListDto contained_medication_list = new eRecipeWS.DTO.MedicationListDto();
                    if (NpakId7 != "")
                    {
                        product_medication = Session.eRecipeUtils.GetMedicationList<eRecipeWS.DTO.MedicationListDto>("NPAKID7", NpakId7).MedicationList.First();
                        if (!String.IsNullOrWhiteSpace(in_erecipe.eRecipe.MedicationId))
                            contained_medication_list = Session.eRecipeUtils.GetMedicationList<eRecipeWS.DTO.MedicationListDto>("ContainedMedicationId", in_erecipe.eRecipe.MedicationId);
                    }
                    else if (in_erecipe.eRecipe.Type == "ev")//ekstemporalus vaistas
                    {
                        product_medication.MedicationId = "1";
                        product_medication.MedicationRef = "#1";
                    }
                    else
                        throw new Exception("Nerastas prekės TLK ID!");
                    if (product_medication != null && product_medication.MedicationId != "")
                    {
                        if (!String.IsNullOrWhiteSpace(in_erecipe.eRecipe.MedicationId) && contained_medication_list.MedicationList.Where(cml => cml.MedicationId == product_medication.MedicationId).Count() == 0)
                            throw new Exception("Elektroniniame recepte išrašytas firminis produktas nesutampa su parduodamu produktu!");
                        in_erecipe.Medication = product_medication;
                    }
                    else
                        helpers.alert(Enumerator.alert.error, "Negauta prekės " + NpakId7 + " informacija iš Registrų centro");
                }
                tbRecipeNo.Select();
                form_wait(false);
            }
            catch (Exception ex)
            {
                tbBarcode.ReadOnly = false;
                btnSelBarcode.Enabled = true;
                helpers.alert(Enumerator.alert.error, ex.Message);
                form_wait(false);
            }
        }

        private void dtpRecipeDate_ValueChanged(object sender, EventArgs e)
        {
            DateTimePicker dtp = sender as DateTimePicker;
            tbRecipeDate.Text = dtp.Value.ToString("yyyy-MM-dd");
        }

        private void tbString_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (this.formWaiting == true)
                e.Handled = true;
        }

        private void tbNumber_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (this.formWaiting == true)
                e.Handled = true;
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
                e.Handled = true;
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
            if ((e.KeyChar == ',') && (tb.Text.IndexOf(',') > -1))
                e.Handled = true;
            if ((e.KeyChar == ',') && (tb.Text.Length == 0))
            {
                tb.Text = "0,";
                tb.Select(tb.Text.Length, tb.Text.Length);
                e.Handled = true;
            }
            /*
            if (!char.IsControl(e.KeyChar) && tb.Text.Length >= 3 && tb.Text.IndexOf(',') < 0 && e.KeyChar != ',')
            {
                tb.Text += "," + e.KeyChar;
                tb.Select(tb.Text.Length, tb.Text.Length);
                e.Handled = true;
            }
            */
        }

        private void tbSalesDate_Leave(object sender, EventArgs e)
        {
            TextBox tb = sender as TextBox;
            SalesDate = helpers.format_date(tb.Text);
        }

        private void tbTillDate_Leave(object sender, EventArgs e)
        {
            TextBox tb = sender as TextBox;
            TillDate = helpers.format_date(tb.Text);
        }

        private void tbValidTill_Leave(object sender, EventArgs e)
        {
            TextBox tb = sender as TextBox;
            ValidTill = helpers.format_date(tb.Text);
        }

        private void tbValidFrom_Leave(object sender, EventArgs e)
        {
            TextBox tb = sender as TextBox;
            ValidFrom = helpers.format_date(tb.Text);
            dtpValidFrom_Leave(sender, e);
        }

        private void tbTotalSum_Leave(object sender, EventArgs e)
        {
            TextBox tb = sender as TextBox;
            if (Qty > 0)
                Price = Math.Round(tb.Text.ToDecimal() / Qty, 2);
        }

        private void dtpSalesDate_ValueChanged(object sender, EventArgs e)
        {
            DateTimePicker dtp = sender as DateTimePicker;
            tbSalesDate.Text = dtp.Value.ToString("yyyy-MM-dd");
        }

        private void dtpTillDate_ValueChanged(object sender, EventArgs e)
        {
            DateTimePicker dtp = sender as DateTimePicker;
            tbTillDate.Text = dtp.Value.ToString("yyyy-MM-dd");
        }

        private void dtpValidTill_ValueChanged(object sender, EventArgs e)
        {
            DateTimePicker dtp = sender as DateTimePicker;
            tbValidTill.Text = dtp.Value.ToString("yyyy-MM-dd");
        }

        private void dtpValidFrom_ValueChanged(object sender, EventArgs e)
        {
            DateTimePicker dtp = sender as DateTimePicker;
            tbValidFrom.Text = dtp.Value.ToString("yyyy-MM-dd");
        }

        private void dtpValidFrom_Leave(object sender, EventArgs e)
        {
            ValidTill = ValidFrom.AddDays(RecipeValidityPeriod);
            ChangedValuesEvent("dtpValidFrom_Leave", new EventArgs());
        }

        private void btnSelStore_Click(object sender, EventArgs e)
        {
            if (formWaiting == true)
                return;
            form_wait(true);
            store dlg = new store();
            dlg.Location = helpers.middleScreen(this.ParentForm, dlg);
            dlg.ShowDialog();
            if (dlg.DialogResult == DialogResult.OK)
            {
                //hd_storeId = dlg.storeId;
                tbStoreName.Text = dlg.storeName;
            }
            dlg.Dispose();
            dlg = null;
            form_wait(false);
        }

        private void btnSelBarcode_Click(object sender, EventArgs e)
        {
            if (formWaiting == true)
                return;
            stock_info dlg = new stock_info();
            dlg.Location = helpers.middleScreen(this.ParentForm, dlg);
            dlg.caller = "select_barcode";
            dlg.ShowDialog();
            if (!dlg.selBarcode.Equals(""))
            {
                tbBarcode.Text = dlg.selBarcode;
                tbBarcode_Leave(tbBarcode, e);//form_wait nereikia, nes sitoj funkcijoj yra
            }
            dlg.Dispose();
            dlg = null;
        }

        #region Variables
        public string Barcode
        {
            get { return tbBarcode.Text; }
            set
            {
                tbBarcode.Text = value;
                tbBarcode.ReadOnly = true;
                btnSelBarcode.Enabled = false;
            }
        }

        public string BarcodeName
        {
            get { return tbBarcodeName.Text; }
            set
            {
                tbSalesDate.ReadOnly = true;
                dtpSalesDate.Enabled = false;
                tbBarcodeName.Text = value;
            }
        }

        public decimal RecipeNo
        {
            get { return tbRecipeNo.Text.ToDecimal(); }
            set { tbRecipeNo.Text = value.ToString(); }
        }

        public decimal Qty
        {
            get { return tbQty.Text.ToDecimal(); }
            set { tbQty.Text = Math.Round(value, 3).ToString(); }
        }

        public decimal GQty
        {
            get { return tbGQty.Text.ToDecimal(); }
            set { tbGQty.Text = Math.Round(value).ToString(); }
        }

        public decimal Doses
        {
            get { return tbDoses.Text.ToDecimal(); }
            set { tbDoses.Text = Math.Round(value).ToString(); }
        }

        public decimal QtyDay
        {
            get { return tbQtyDay.Text.ToDecimal(); }
            set { tbQtyDay.Text = value.ToString(); }
        }

        public int CountDay
        {
            get
            {
                int temp;
                if (int.TryParse(tbCountDay.Text, out temp))
                    return temp;
                else
                    return 0;
            }
            set { tbCountDay.Text = value.ToString(); }
        }

        public DateTime TillDate
        {
            get { return Convert.ToDateTime(tbTillDate.Text); }
            set { tbTillDate.Text = value.ToString("yyyy-MM-dd"); }
        }

        public DateTime ValidFrom
        {
            get { return Convert.ToDateTime(tbValidFrom.Text); }
            set { tbValidFrom.Text = value.ToString("yyyy-MM-dd"); }
        }

        public DateTime SalesDate
        {
            get { return Convert.ToDateTime(tbSalesDate.Text); }
            set { tbSalesDate.Text = value.ToString("yyyy-MM-dd"); }
        }

        public DateTime RecipeDate
        {
            get { return dtpRecipeDate.Value; }
            set { dtpRecipeDate.Value = value; }
        }

        public DateTime ValidTill
        {
            get { return dtpValidTill.Value; }
            set { dtpValidTill.Value = value; }
        }

        public decimal TotalSum
        {
            get { return tbTotalSum.Text.ToDecimal(); }
            set { tbTotalSum.Text = value.ToString(); }
        }

        public decimal Price { get; set; }

        public int Ext
        {
            get
            {
                if (chbExt.Checked == true)
                    return 1;
                else
                    return 0;
            }
            set
            {
                if (value == 1)
                    chbExt.Checked = true;
                else
                    chbExt.Checked = false;
            }
        }

        private decimal RecipeValid
        {
            get { return tbRecipeValid.Text.ToDecimal(); }
            set { tbRecipeValid.Text = value.ToString(); }
        }

        private int RecipeValidityPeriod
        {
            get
            {
                int period = 30;//default
                if (chbExt.Checked == true)
                {
                    if (in_erecipe != null)
                        period = in_erecipe.ValidationPeriod;
                    else
                        period = 35;
                }
                return period - 1;//include this day
            }
        }

        DateTime PastTillDate { get; set; }
        #endregion

        private void ChangedValuesEvent(object sender, EventArgs e)
        {
            if (Barcode == "")
                return;
            if (formWaiting)
                return;
            TextBox tb = sender as TextBox;
            if (tb?.Name == "tbGQty" && hd_barratio > 0)
                Qty = GQty * hd_prodratio / hd_barratio;
            if (tb?.Name == "tbQty" && hd_prodratio > 0)
                GQty = Math.Round(Qty * hd_barratio / hd_prodratio);
            //if (tb?.Name != "tbDoses")
            //    Doses = Math.Round(hd_qty2 * Qty);
            if (tb?.Name == "tbCountDay" && CountDay > 0)
                QtyDay = Math.Truncate(1000 * Doses / CountDay) / 1000;
            if (QtyDay > 0)
                CountDay = (int)Math.Floor(Doses / QtyDay);
            TotalSum = Math.Round(Qty * Price, 2);
            if (Ext == 1 && SalesDate <= PastTillDate)
                TillDate = PastTillDate.AddDays(CountDay + helpers.get_interval(SalesDate, ValidFrom));
            else
                TillDate = SalesDate.AddDays(CountDay + helpers.get_interval(SalesDate, ValidFrom) - 1);
            RecipeValid = helpers.betweenday(ValidFrom.Date, ValidTill.Date) + 1;
            CheckValuesEvent(sender, e);
        }

        private void CheckValuesEvent(object sender, EventArgs e)
        {
            try
            {
                if (in_erecipe.eRecipe_PrescriptionTagsLongTag && Ext == 0 //pirmas isdavimas
                                    && (DateTime.Now.AddDays(1) - in_erecipe.eRecipe_ValidFrom).TotalDays > 30)
                    throw new Exception("Gydymui tęsti recepto pirmą išdavimą galima išduoti tik per 30 dienų nuo galiojimo datos!", new Exception(Enumerator.alert.error.ToString()));
                if (helpers.betweenday(ValidFrom, SalesDate) > RecipeValid - 1)
                    throw new Exception("Receptas negalioja!!!");
                if (helpers.betweenday2(SalesDate.Date, ValidFrom.Date) > 0)
                    throw new Exception("Receptas negalioja, per anksti!!!");
                if (helpers.betweenday2(RecipeDate.Date, ValidFrom.Date) > 13 && Ext == 0)
                    throw new Exception("Receptas negalioja, gydytojo klaida?");
            }
            catch (Exception ex)
            {
                if (ex.Message != "")
                {
                    Enumerator.alert alert_type = ex.InnerException?.Message?.ToEnum<Enumerator.alert>() ?? Enumerator.alert.warning;
                    helpers.alert(alert_type, ex.Message);
                    if (alert_type == Enumerator.alert.error)//critical error = close window
                        btnClose_Click(btnClose, new EventArgs());
                }
            }
        }
    }
}
