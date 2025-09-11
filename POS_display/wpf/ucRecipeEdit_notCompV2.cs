using Hl7.Fhir.Model;
using POS_display.Repository.Recipe;
using POS_display.Views.ERecipe;
using POS_display.wpf;
using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using TamroUtilities.HL7.Models;

namespace POS_display
{
    public partial class ucRecipeEdit_notCompV2 : ucRecipeEditBase, IRecipeEdit
    {
        public decimal hd_qty2 = 0;
        public decimal hd_productid = 0;
        public decimal hd_prodratio = 0;
        public decimal hd_barratio = 0;
        public string hd_note2 = "";
        private readonly RecipeRepository _recipeRepository;
        private const string DataModelVersionValue = "v29";

        public ucRecipeEdit_notCompV2(Items.eRecipe.Recipe erecipe, Items.posd posd_item, string PickedUpByRef)
        {
            InitializeComponent();
            _recipeRepository = new RecipeRepository();
            in_erecipe = erecipe;
            in_posd = posd_item;
            in_PickedUpByRef = PickedUpByRef;
        }

        private async void recipe_nonComp_Load(object sender, EventArgs e)
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
                hd_productid = temp.Rows[0]["productid"].ToDecimal();
                hd_prodratio = temp.Rows[0]["prodratio"].ToDecimal();
                hd_barratio = temp.Rows[0]["barratio"].ToDecimal();
                hd_note2 = temp.Rows[0]["note2"].ToString();
                Price = in_posd.price;
                TotalSum = in_posd.sum;
                Doses = Math.Round(hd_qty2 * Qty);
            }
            else//not from POS
                tbBarcode.Select();
            if (Session.RecipesOnly == false && Session.Admin == false)
            {
                tbQty.ReadOnly = true;
                tbGQty.ReadOnly = true;
            }
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
            if (in_erecipe.eRecipe.Type == "ev")//ekstemporalus vaistas
                tbTotalSum.ReadOnly = false;

            if (in_erecipe.IsDispenseBySubtances)
            {
                var group_id = await _recipeRepository.GetPartialDispenseGroupIdByPosHeaderId(in_posd.hid);
                in_erecipe.DispenseBySubstancesGroupId = string.IsNullOrEmpty(group_id) ? Guid.NewGuid().ToString() : group_id;
            }

            EnableOverLimitQty(
                in_erecipe?.IsDispenseBySubtances ?? false,
                in_erecipe?.DispenseBySubstancesGroupId,
                hd_note2);

            form_wait(false);
            ChangedValuesEvent("NewRecipeData_cb", new EventArgs());
        }

        private void EnableOverLimitQty(bool isDispenseBySubtances, string dispenseBySubstancesGroupId, string note2)
        {
            if (isDispenseBySubtances) 
            {
                cbOverLimitQty.Enabled = string.IsNullOrEmpty(dispenseBySubstancesGroupId) && note2 != "NARC" && note2 != "PSYC";
            }
            else
            {
                cbOverLimitQty.Enabled = note2 != "NARC" && note2 != "PSYC";
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            if (this.formWaiting == true)
                return;
            CloseWindow(DialogResult.Cancel);
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
            if (!cbOverLimitQty.Checked && CountDay > 90 && (hd_note2 != "PSYC" && hd_note2 != "NARC"))
            {
                if (!helpers.alert(Enumerator.alert.warning, "Išduodamas kiekis ilgesniam nei 90 dienų vartojimui,\n" +
                    "bet nepažymėjote, kad išduodamas didesnis vaisto kiekis.\nAr norite tęsti?", true))
                {
                    throw new Exception("");
                }
            }

            var prescriptionStatus = "completed";
            if (!in_erecipe.IsValidToDispenseWithoutValidPrescription())
            {
                prescriptionStatus = ResolveStatusByDispensedQuantity(GQty);
                if (string.IsNullOrEmpty(prescriptionStatus))
                {
                    bool isMPP = string.IsNullOrEmpty(in_erecipe?.Medication?.MedicationGroup);
                    var message = isMPP ?
                        "Bendras išduodamas kiekis viršys išrašytą kiekį.\nAr norite tęsti?"
                        : "Išduodamas kiekis viršys 15% išrašyto kiekio.\nAr norite tęsti?";
                    if (!helpers.alert(Enumerator.alert.warning, message, true))
                    {
                        return;
                    }
                }
            }

            in_erecipe.PrescriptionStatus = prescriptionStatus;

            if (Session.RecipesOnly && in_erecipe.eRecipe_CompensationTag && in_erecipe.eRecipe.Type.ToLowerInvariant() == "mpp")
                in_erecipe.MppBarcode = Barcode;

            if (tbTillDate.Text != "")
            {
                form_wait(true);
                if ((in_posd?.id ?? 0) > 0)
                    await DB.POS.asyncChangePosdPrice(in_posd.id, Price);

                in_erecipe.IsOverLimitQty = cbOverLimitQty.Checked;
                var erecipeId = in_erecipe.IsDispenseBySubtances
                    ? await Controllers.eRecipe.CreateGroupDipsenseEntry(
                        in_erecipe,
                        in_PickedUpByRef,
                        in_posd?.hid ?? 0,
                        in_posd?.id ?? 0,
                        hd_productid,
                        0,
                        TillDate,
                        RecipeDate,
                        SalesDate,
                        TotalSum,
                        0,
                        0,
                        GQty,
                        0,
                        CountDay)
                    : await Controllers.eRecipe.CreateERecipe(
                        in_erecipe,
                        in_PickedUpByRef,
                        in_posd?.hid ?? 0,
                        in_posd?.id ?? 0,
                        hd_productid,
                        0,
                        TillDate,
                        RecipeDate,
                        SalesDate,
                        TotalSum,
                        0,
                        0,
                        GQty,
                        0,
                        CountDay);

                if (erecipeId > 0)
                {
                    await _recipeRepository.SetPartialDispenseGroupId(erecipeId.ToLong(), in_erecipe?.DispenseBySubstancesGroupId);
                    await _recipeRepository.SetERecipeDiseaseCode(erecipeId.ToLong(), in_erecipe?.eRecipe?.ReasonCode);
                    CloseWindow(DialogResult.OK);
                }
                else
                    CloseWindow(DialogResult.Cancel);
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
                hd_productid = temp.Rows[0]["productid"].ToDecimal();
                hd_prodratio = temp.Rows[0]["prodratio"].ToDecimal();
                hd_barratio = temp.Rows[0]["barratio"].ToDecimal();
                hd_note2 = temp.Rows[0]["note2"].ToString();
                hd_qty2 = hd_barratio;
                GQty = hd_barratio;
                Doses = hd_barratio;
                Barcode = tb.Text;
                BarcodeName = temp.Rows[0]["name"].ToString();
                Qty = 1;
                TotalSum = temp.Rows[0]["price"].ToDecimal();
                Price = temp.Rows[0]["price"].ToDecimal();
                selectBarcode(hd_productid);
                EnableOverLimitQty(
                    in_erecipe?.IsDispenseBySubtances ?? false,
                    in_erecipe?.DispenseBySubstancesGroupId,
                    hd_note2);
            }
            else
            {
                form_wait(false);
                btnSelBarcode_Click(sender, e);
                return;
            }
        }

        private async void selectBarcode(decimal product_id)
        {
            try
            {
                if (Session.RecipesOnly)
                {
                    tbTotalSum.ReadOnly = false;
                    decimal NpakId7 = await Session.SamasUtils.GetNpakid7ByProductId(product_id);         
                    if (NpakId7.ToString() == "1000000")//gaminami vaistai
                        NpakId7 = 0;
                    MedicationDto product_medication = new MedicationDto();
                    MedicationListDto contained_medication_list = new MedicationListDto();
                    if (NpakId7 != 0)
                    {
                        product_medication = Session.eRecipeUtils.GetMedicationList<MedicationListDto>("NPAKID7", NpakId7.ToString()).MedicationList.First();
                        if (!String.IsNullOrWhiteSpace(in_erecipe.eRecipe.MedicationId))
                            contained_medication_list = Session.eRecipeUtils.GetMedicationList<MedicationListDto>("ContainedMedicationId", in_erecipe.eRecipe.MedicationId);
                    }
                    else if (in_erecipe.eRecipe.Type == "ev")//ekstemporalus vaistas
                    {
                        tbTotalSum.ReadOnly = false;
                        product_medication.MedicationId = 1;
                        product_medication.MedicationRef = "#1";
                    }
                    else
                        throw new Exception("Nerastas prekės TLK ID!");
                    if (product_medication != null && product_medication.MedicationId != 0)
                    {
                        if (in_erecipe.eRecipe.Type == "va" && !String.IsNullOrWhiteSpace(in_erecipe.eRecipe.MedicationId) && (contained_medication_list?.MedicationList?.Where(cml => cml.MedicationId == product_medication.MedicationId).Count() ?? 0) == 0)
                        {
                            if (!helpers.alert(Enumerator.alert.confirm, "Elektroniniame recepte išrašytas firminis produktas nesutampa su parduodamu produktu!\nAr vistiek norite tęsti pardavimą?", true))
                                throw new Exception("");
                        }
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
            get { return dtpTillDate.Value; }
            set { dtpTillDate.Value = value; }
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

        public decimal RecipeValid
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

        public DateTime PastTillDate { get; set; }
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
            if (tb?.Name == "tbCountDay" && CountDay > 0)
                QtyDay = Math.Truncate(1000 * Doses / CountDay) / 1000;
            if (QtyDay > 0)
                CountDay = (int)Math.Floor(Doses / QtyDay);
            TotalSum = Math.Round(Qty * Price, 2);


            if (in_erecipe?.eRecipe?.DataModelVersionValue?.Contains(DataModelVersionValue) == true)
            {
                TillDate = (in_erecipe.DispenseCount == 0)
                           ? SalesDate.AddDays(CountDay - 1)
                           : (SalesDate.Date <= in_erecipe.LatestDueDate.Date)
                             ? in_erecipe.LatestDueDate.AddDays(CountDay)
                             : SalesDate.AddDays(CountDay - 1);
            }
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
                    throw new ArgumentException("Gydymui tęsti recepto pirmą išdavimą galima išduoti tik per 30 dienų nuo galiojimo datos!", new Exception(Enumerator.alert.error.ToString()));
                if ((DateTime.Now.AddDays(1) - in_erecipe.eRecipe_ValidFrom).TotalDays > 30 &&
                    !in_erecipe.eRecipe_PrescriptionTagsByDemandTag &&
                    !in_erecipe.eRecipe_PrescriptionTagsLabelingExemptionTag &&
                    !in_erecipe.eRecipe_PrescriptionTagsNominalTag && in_erecipe.DispenseCount == 0)
                    throw new Exception("Nuo vaisto įsigaliojimo praėjo daugiau nei 30 dienų ir nebuvo atliktas nei vienas išdavimas. Išdavimas negalimas.", new Exception(Enumerator.alert.error.ToString()));
                CheckValues(this);
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

        #region Workaround
        internal override void tbString_KeyPress(object sender, KeyPressEventArgs e)
        {
            base.tbString_KeyPress(sender, e);
        }

        internal override void tbNumber_KeyPress(object sender, KeyPressEventArgs e)
        {
            base.tbNumber_KeyPress(sender, e);
        }

        internal override void tbQty_KeyPress(object sender, KeyPressEventArgs e)
        {
            base.tbQty_KeyPress(sender, e);
        }
        #endregion
    }
}
