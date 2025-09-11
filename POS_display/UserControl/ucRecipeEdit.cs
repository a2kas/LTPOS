using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;

namespace POS_display
{
    public partial class ucRecipeEdit : UserControl
    {
        public bool formWaiting = false;
        //input variables
        private Items.eRecipe.Recipe in_erecipe;

        //output variables
        public string out_form_action = "";

        //hidden variables
        private decimal maxmkaina = 0;
        private decimal maxbkaina = 0;
        private decimal hd_storeId = 0;
        private Dictionary<decimal, decimal> hd_compensation = new Dictionary<decimal, decimal>();
        private decimal hd_qty2 = 0;
        private decimal hd_barratio = 0;
        private decimal hd_prodratio = 0;
        public decimal hd_poshId = 0;
        public decimal hd_posdId = 0;
        public decimal hd_posdPriceDiscounted = 0;
        public decimal hd_productId = 0;
        public decimal hd_recipeId = 0;
        private int hd_tlk_status = 0;
        private decimal hd_doctorId = 0;
        private decimal hd_clinicId = 0;
        private decimal hd_compensationId = 0;
        public decimal hd_barcodeId = 0;

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == (Keys.Enter))
            {
                SendKeys.Send("{TAB}");
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }
        #region Callbacks
        private void NewRecipeData_cb(bool success, DataTable t)
        {
            // we are different thread!
            this.BeginInvoke(new MethodInvoker(delegate
            {
                try
                {
                    if (success && t != null && t.Rows.Count > 0)
                    {
                        DateTime current_date = DateTime.Now;
                        if (in_erecipe != null)//e receptas
                        {
                            if (in_erecipe.eRecipe_PrescriptionTagsLongTag && in_erecipe.DispenseCount > 0)//jei gyd. tęsti ir išdavimas ne pirmas
                                Ext = 1;
                            RecipeNo = in_erecipe.eRecipe_RecipeNumber;
                            KVPDoctorNo = "E";
                            CompCode = in_erecipe.eRecipe_CompensationCode;
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
                            DeseaseCode = in_erecipe.eRecipe.ReasonCode;
                            AAGA_ISAS = in_erecipe.eRecipe.AagaSgasNumber;
                        }
                        else
                        {
                            RecipeDate = current_date;
                            ValidFrom = current_date;
                            ValidTill = ValidFrom.AddDays(RecipeValidityPeriod);
                        }
                        SalesDate = current_date;
                        CheckDate = current_date;
                        RecipeValid = helpers.betweenday(ValidFrom.Date, ValidTill.Date) + 1;
                        BasicPrice = t.Rows[0]["basicprice"].ToDecimal();
                        SalesPrice = t.Rows[0]["newsalesprice"].ToDecimal();
                        hd_compensation[0] = 0;
                        hd_compensation[50] = t.Rows[0]["c50"].ToDecimal();
                        hd_compensation[80] = t.Rows[0]["c80"].ToDecimal();
                        hd_compensation[90] = t.Rows[0]["c90"].ToDecimal();
                        hd_compensation[100] = t.Rows[0]["c100"].ToDecimal();
                        tlkId = t.Rows[0]["code2"].ToString();
                        hd_qty2 = t.Rows[0]["retailpr"].ToDecimal();
                        Doses = Math.Round(hd_qty2 * Qty);
                        hd_barratio = t.Rows[0]["barratio"].ToDecimal();
                        hd_prodratio = t.Rows[0]["prodratio"].ToDecimal();
                        if (hd_posdPriceDiscounted == 0 || (maxmkaina < hd_posdPriceDiscounted && maxmkaina != 0))
                            SalesPrice = maxmkaina;
                        else
                            SalesPrice = hd_posdPriceDiscounted;
                        if (hd_prodratio > 0)
                            GQty = Math.Round(Qty * hd_barratio / hd_prodratio);
                        tbCompCode_Change();
                        form_wait(false);
                        ChangedValuesEvent("NewRecipeData_cb", new EventArgs());
                    }
                    else
                        throw new Exception("prekė yra nekompensuojama, tolimesni veiksmai negalimi!");
                }
                catch (Exception ex)
                {
                    form_wait(false);
                    helpers.alert(Enumerator.alert.error, ex.Message);
                    btnClose_Click("NewRecipeData_cb", new EventArgs());
                }
            }));
        }

        private void LoadRecipeData_cb(bool success, DataTable t)
        {
            // we are different thread!
            this.BeginInvoke(new MethodInvoker(delegate
            {
                if (success && t != null && t.Rows.Count > 0)
                {
                    btnPrint.Enabled = true;
                    StoreName = t.Rows[0]["storename"].ToString();
                    Barcode = t.Rows[0]["barcode"].ToString();
                    hd_barcodeId = t.Rows[0]["barcodeid"].ToDecimal();
                    BarcodeName = t.Rows[0]["productname"].ToString();
                    RecipeNo = t.Rows[0]["recipeno"].ToDecimal();
                    KVPDoctorNo = t.Rows[0]["kvpdoctorno"].ToString();
                    CompCode = t.Rows[0]["compensationcode"].ToDecimal();
                    CompPercent = t.Rows[0]["comppercent"].ToString();
                    hd_compensationId = t.Rows[0]["compensationid"].ToDecimal();
                    RecipeDate = DateTime.Parse(t.Rows[0]["recipedate"].ToString());
                    ValidTill = DateTime.Parse(t.Rows[0]["valid_till"].ToString());
                    ValidFrom = DateTime.Parse(t.Rows[0]["valid_from"].ToString());
                    Doses = t.Rows[0]["qty2"].ToDecimal();
                    hd_qty2 = t.Rows[0]["qty2"].ToDecimal();
                    QtyDay = t.Rows[0]["qtyday"].ToDecimal();
                    CountDay = (int)t.Rows[0]["countday"].ToDecimal();
                    TillDate = DateTime.Parse(t.Rows[0]["till_date"].ToString());
                    DeseaseCode = t.Rows[0]["deseasecode"].ToString();
                    AAGA_ISAS = t.Rows[0]["aaga_isas"].ToString();
                    Water = t.Rows[0]["water"].ToString();
                    DoctorCode = t.Rows[0]["doctorcode2"].ToString();
                    hd_doctorId = t.Rows[0]["doctorcode"].ToDecimal();
                    DoctorName = t.Rows[0]["doctorname"].ToString();
                    Taxolaborum = t.Rows[0]["taxolaborum"].ToDecimal();
                    ClinicCode = t.Rows[0]["cliniccode2"].ToDecimal();
                    hd_clinicId = t.Rows[0]["cliniccode"].ToDecimal();
                    ClinicName = t.Rows[0]["clinicname"].ToString();
                    Ext = t.Rows[0]["ext"].ToInt();
                    Qty = t.Rows[0]["qty"].ToDecimal();
                    GQty = t.Rows[0]["gqty"].ToDecimal();
                    SalesDate = DateTime.Parse(t.Rows[0]["salesdate"].ToString());
                    CheckDate = DateTime.Parse(t.Rows[0]["checkdate"].ToString());
                    SalesPrice = t.Rows[0]["salesprice"].ToDecimal();
                    BasicPrice = t.Rows[0]["basicprice"].ToDecimal();
                    tlkId = t.Rows[0]["tlkid"].ToString();
                    CheckNo = t.Rows[0]["checkno"].ToDecimal();
                    CompSum = t.Rows[0]["compensationsum"].ToDecimal();
                    PaySum = t.Rows[0]["paysum"].ToDecimal();
                    TotalSum = t.Rows[0]["totalsum"].ToDecimal();
                    RecipeValid = helpers.betweenday(ValidFrom.Date, ValidTill.Date) + 1;
                    hd_recipeId = t.Rows[0]["id"].ToDecimal();
                    hd_storeId = t.Rows[0]["store_id"].ToDecimal();
                    hd_barratio = t.Rows[0]["barratio"].ToDecimal();
                    hd_prodratio = t.Rows[0]["prodratio"].ToDecimal();
                    if (t.Rows[0]["confirmed"].ToInt() == 0)//not confirmed
                        btnRepair.Enabled = false;
                    else//confirmed
                    {
                        //btnRepair.Enabled = true;
                        tbQty.ReadOnly = true;
                        tbGQty.ReadOnly = true;
                        tbCheckDate.Enabled = false;
                        dtpCheckDate.Enabled = false;
                        tbSalesPrice.ReadOnly = true;
                        tbBasicPrice.ReadOnly = true;
                        tbCheckNo.ReadOnly = true;
                        tbRecipeValid.ReadOnly = true;
                    }
                    hd_tlk_status = t.Rows[0]["tlk_status"].ToInt();
                    //hd_productId = 0;
                    //hd_qty2 = 0;
                    // pritraukti kompens reiksmes priklauomai nuo pardavimo datos
                    DataTable temp = null;
                    temp = DB.recipe.getRecipeCompPrice(hd_recipeId);
                    if (temp != null && temp.Rows.Count > 0)
                    {
                        hd_compensation[0] = 0;
                        hd_compensation[50] = temp.Rows[0]["c50"].ToDecimal();
                        hd_compensation[80] = temp.Rows[0]["c80"].ToDecimal();
                        hd_compensation[90] = temp.Rows[0]["c90"].ToDecimal();
                        hd_compensation[100] = temp.Rows[0]["c100"].ToDecimal();
                    }
                    form_wait(false);
                    ChangedValuesEvent("LoadRecipeData_cb", new EventArgs());
                    tbRecipeNo.Select();
                }
            }));
        }

        private void checkRecipe_cb(bool success, string result)
        {
            // we are different thread!
            int cl1 = 0;
            int cl2 = 0;
            int res = 0;
            bool cl5 = false;
            if (success)
            {
                XmlNode data = helpers.ReadXML(result, "//ns2:SOAP_REZULTATAS", "ns2", "http://kvapws.algoritmusistemos.com/");
                if (data["REZULTATAI"]["KV_RECEPTAI"]["ROW"].HasChildNodes)
                {
                    for (int j = 0; j < data["REZULTATAI"]["KV_RECEPTAI"]["ROW"].ChildNodes.Count; j++)
                    {
                        if (data["REZULTATAI"]["KV_RECEPTAI"]["ROW"].ChildNodes[j].Name == "KV_RECEPTO_KLAIDOS")
                        {
                            cl1 = 2;
                            if (data["REZULTATAI"]["KV_RECEPTAI"]["ROW"]["KV_RECEPTO_KLAIDOS"].HasChildNodes)
                            {
                                int count = data["REZULTATAI"]["KV_RECEPTAI"]["ROW"]["KV_RECEPTO_KLAIDOS"].ChildNodes.Count;
                                for (int i = 0; i < count; i++)
                                {
                                    if (data["REZULTATAI"]["KV_RECEPTAI"]["ROW"]["KV_RECEPTO_KLAIDOS"].ChildNodes[i]["KRITISKUMAS_MEAN"].InnerText != "")
                                        cl2 = 2;
                                }
                            }
                        }
                    }
                }
                else
                    cl5 = true;
                res = cl1 + cl2;
                if (cl5 == true)
                    res = 1;
                if (res > 1)
                {
                    form_wait(false);
                    check_recipe_kvr dlg = new check_recipe_kvr();
                    dlg.Location = helpers.middleScreen(this.ParentForm, dlg);
                    dlg.recipeNo = RecipeNo;
                    dlg.doctorNo = KVPDoctorNo;
                    dlg.recipeDate = RecipeDate;
                    dlg.ShowDialog();
                    dlg.Dispose();
                    dlg = null;
                    return;
                }
                if (res == 1)
                {
                    this.BeginInvoke(new MethodInvoker(delegate
                    {
                        helpers.alert(Enumerator.alert.warning, "Receptas nepatikrintas dėl ryšio problemos, tačiau išsaugotas. Pasitikrinkite ar veikia internetas!");
                    }));
                }
                if (hd_posdId > 0)
                {
                    createRecipe();
                }
            }
            else
            {
                form_wait(false);
                this.BeginInvoke(new MethodInvoker(delegate
                {
                    helpers.alert(Enumerator.alert.error, "Nepavyksta patikrinti recepto duomenų TLK.");
                }));
            }
        }
        #endregion
        public ucRecipeEdit(Items.eRecipe.Recipe erecipe_item, Items.posd posd_item)
        {
            InitializeComponent();
            in_erecipe = erecipe_item;
            if (posd_item != null)
            {
                hd_posdId = posd_item.id;
                hd_poshId = posd_item.hid;
                hd_productId = posd_item.productid;
                Barcode = posd_item.barcode;
                hd_barcodeId = posd_item.barcodeid;
                BarcodeName = posd_item.barcodename;
                Qty = posd_item.qty;
                hd_recipeId = posd_item.recipeid;
                hd_posdPriceDiscounted = posd_item.pricediscounted;
            }
        }

        private void recipe_edit_Load(object sender, EventArgs e)
        {
            try
            {
                if (!Session.user.postname.StartsWith("Vaist") && !Session.user.postname.StartsWith("Farma") && !Session.user.postname.StartsWith("Ved") && !Session.user.postname.StartsWith("Vad"))
                    throw new Exception(Session.user.postname + " negali parduoti kompensuojamų receptų!");
                form_wait(true);
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
                DB.POS.UpdateSession("Receptai", 2);
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
                if (hd_recipeId == 0)//new recipe
                {
                    hd_storeId = Session.SystemData.storeid;
                    StoreName = Session.SystemData.storename;
                    if (hd_posdId > 0)//from POS
                    {
                        DataTable temp = null;
                        temp = DB.recipe.getTLKPrice(hd_productId, "100", DateTime.Now);
                        if (temp != null && temp.Rows.Count > 0)
                        {
                            maxmkaina = temp.Rows[0]["mmkaina"].ToDecimal();
                            maxbkaina = temp.Rows[0]["mbkaina"].ToDecimal();
                        }
                        DB.recipe.asyncNewRecipeData(hd_posdId, CheckDate.Date, Barcode, "100", NewRecipeData_cb);
                        tbRecipeNo.Select();
                    }
                    else//not from POS
                    {
                        //TODO
                        tbBarcode.Select();
                        form_wait(false);
                    }
                }
                else//load recipe
                {
                    DB.recipe.asyncLoadRecipeData(hd_recipeId, LoadRecipeData_cb);
                }
            }
            catch (Exception ex)
            {
                form_wait(false);
                helpers.alert(Enumerator.alert.error, ex.Message);
                btnClose_Click("recipe_edit_Load", new EventArgs());
            }
        }

        public void form_wait(bool wait)
        {
            if (wait == true)
            {
                this.UseWaitCursor = true;
                this.Cursor = Cursors.WaitCursor;
                this.formWaiting = true;
                tbRecipeDate.ReadOnly = true;
                dtpRecipeDate.Enabled = false;
                tbValidFrom.ReadOnly = true;
                dtpValidFrom.Enabled = false;
                tbTillDate.ReadOnly = true;
                dtpTillDate.Enabled = false;
                tbValidTill.ReadOnly = true;
                dtpValidTill.Enabled = false;
            }
            else
            {
                this.UseWaitCursor = false;
                this.Cursor = Cursors.Default;
                this.formWaiting = false;
                tbRecipeDate.ReadOnly = false;
                dtpRecipeDate.Enabled = true;
                tbValidFrom.ReadOnly = false;
                dtpValidFrom.Enabled = true;
                tbTillDate.ReadOnly = false;
                dtpTillDate.Enabled = true;
                tbValidTill.ReadOnly = false;
                dtpValidTill.Enabled = true;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (formWaiting == true)
                return;
            form_wait(true);
            try
            {
                if (DoctorCode != "" && DoctorName == "")
                    throw new Exception("Neteisingas gydytojo kodas!");
                bool valid_clinic = true;
                if (ClinicCode > 0 && ClinicName == "")
                    throw new Exception("Neteisingas gydymo įstaigos kodas!");
                if (BarcodeName == "")
                    throw new Exception("Nepasirinkta prekė");
                if (RecipeNo == 0)
                    throw new Exception("Neužpildytas recepto numeris!");
                if (CompPercent == "")
                    throw new Exception("Nepasirinktas kompensacijos procentas!");
                if (hd_tlk_status == 1)
                    throw new Exception("Recepto išsaugoti nepavyko, nes jis jau priduotas į TLK!\nAtšaukite receptą iš TLK ir bandykite dar kartą.");
                if (BasicPrice == 0 && (CompCode != 10 || CompCode != 11))
                    throw new Exception("BAZINĖ KAINA = 0!");
                if (Qty <= 0)
                    throw new Exception("Neteisingas kiekis!");
                DataTable t = null;
                t = DB.recipe.getInvalidRecipe(tbRecipeNo.Text, tbKVPDoctorNo.Text);
                if (t != null && t.Rows.Count > 0)
                    throw new Exception("DĖMESIO! Gydytojo numeris arba recepto numeris yra falsifikuotų sąraše. Receptas neišsaugomas.", new Exception(Enumerator.alert.error.ToString()));
                if (Session.RecipeParm.check_ == 1 && E_Recipe == false)
                {
                    Session.KVAPSOAP.checkRecipe(RecipeNo, KVPDoctorNo, RecipeDate, checkRecipe_cb);
                }
                else
                {
                    createRecipe();
                }
            }
            catch (Exception ex)
            {
                form_wait(false);
                if (ex.Message != "")
                {
                    Enumerator.alert alert_type = ex.InnerException?.Message?.ToEnum<Enumerator.alert>() ?? Enumerator.alert.warning;
                    helpers.alert(alert_type, ex.Message);
                    if (alert_type == Enumerator.alert.error)//critical error = close window
                        btnClose_Click("btnSave_Click", new EventArgs());
                }
            }
        }

        private async void btnCommit_Click(object sender, EventArgs e)
        {
            if (formWaiting == true)
                return;
            form_wait(true);
            bool success = await DB.recipe.asyncCommitRecipe(hd_recipeId, tlkId, hd_barcodeId, "", RecipeNo, "", hd_clinicId.ToString(), DeseaseCode, hd_doctorId.ToString(), RecipeDate.Date, SalesPrice, BasicPrice, hd_compensationId, Qty, TotalSum, CompSum, PaySum, SalesDate.Date, GQty, Water.ToDecimal(), Taxolaborum, Ext, CheckDate.Date, CheckNo, QtyDay, CountDay, TillDate.Date, KVPDoctorNo, AAGA_ISAS, ValidFrom.Date, ValidTill.Date);
            form_wait(false);
            if (success == true)
                btnClose_Click("CommitRecipe_cb", new EventArgs());
            else
                helpers.alert(Enumerator.alert.warning, "Nepavyko patvirtinti recepto.");
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            if (formWaiting == true)
                return;
            print_recipe dlg = new print_recipe();
            dlg.Location = helpers.middleScreen(this.ParentForm, dlg);
            dlg.recipeId = hd_recipeId;
            dlg.ShowDialog();
            dlg.Dispose();
            dlg = null;
        }

        private async void btnRepair_Click(object sender, EventArgs e)
        {
            if (formWaiting == true)
                return;
            if (helpers.alert(Enumerator.alert.confirm, "Receptas patvirtintas! Ar tikrai norite jį atšaukti?", true))
            {
                form_wait(true);
                bool success = await DB.recipe.asyncRepairRecipe(hd_recipeId);
                form_wait(false);
                if (success == true)
                    btnClose_Click("RepairRecipe_cb", new EventArgs());
                else
                    helpers.alert(Enumerator.alert.warning, "Nepavyko koreguoti recepto.");
            }
        }

        private void btnCheck_Click(object sender, EventArgs e)
        {
            if (formWaiting == true)
                return;
            check_recipe_kvr dlg = new check_recipe_kvr();
            dlg.Location = helpers.middleScreen(this.ParentForm, dlg);
            dlg.recipeNo = RecipeNo;
            dlg.doctorNo = KVPDoctorNo;
            dlg.recipeDate = RecipeDate;
            dlg.ShowDialog();
            dlg.Dispose();
            dlg = null;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            if (formWaiting == true)
                return;
            form_wait(true);
            out_form_action = "recipe_closed";
            this.Parent.Controls.Remove(this);
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
                hd_storeId = dlg.storeId;
                StoreName = dlg.storeName;
            }
            dlg.Dispose();
            dlg = null;
            form_wait(false);
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
                hd_barcodeId = temp.Rows[0]["id"].ToDecimal();
                hd_productId = temp.Rows[0]["productid"].ToDecimal();
                Barcode = tb.Text;
                BarcodeName = temp.Rows[0]["name"].ToString();
                //Qty = 1;
                selectBarcode(temp.Rows[0]["atc_code"].ToString());
            }
            else
            {
                form_wait(true);
                btnSelBarcode_Click(sender, e);
            }
        }

        private void btnSelBarcode_Click(object sender, EventArgs e)
        {
            if (formWaiting == true)
                return;
            form_wait(true);
            stock_info dlg = new stock_info();
            dlg.Location = helpers.middleScreen(this.ParentForm, dlg);
            dlg.caller = "select_barcode";
            dlg.ShowDialog();
            if (!dlg.selBarcode.Equals(""))
            {
                hd_barcodeId = dlg.selBarcodeId;
                hd_productId = dlg.selProductId;
                Barcode = dlg.selBarcode;
                BarcodeName = dlg.selProductName;
                selectBarcode(dlg.selATCcode);
            }
            else
                form_wait(false);
            dlg.Dispose();
            dlg = null;
        }

        private async void createRecipe()
        {
            if (hd_recipeId == 0)
                hd_recipeId = await DB.recipe.asyncCreateRecipe(tlkId, hd_barcodeId, "", RecipeNo, "", hd_clinicId.ToString(), DeseaseCode, hd_doctorId.ToString(), RecipeDate.Date, SalesPrice, BasicPrice, hd_compensationId, Qty, TotalSum, CompSum, PaySum, SalesDate.Date, GQty, Water.ToDecimal(), Taxolaborum, Ext, CheckDate.Date, CheckNo, QtyDay, CountDay, TillDate.Date, KVPDoctorNo, AAGA_ISAS, ValidFrom.Date, ValidTill.Date, hd_storeId);
            else
                await DB.recipe.asyncUpdateRecipe(hd_recipeId, tlkId, hd_barcodeId, "", RecipeNo, "", hd_clinicId.ToString(), DeseaseCode, hd_doctorId.ToString(), RecipeDate.Date, SalesPrice, BasicPrice, hd_compensationId, Qty, TotalSum, CompSum, PaySum, SalesDate.Date, GQty, Water.ToDecimal(), Taxolaborum, Ext, CheckDate.Date, CheckNo, QtyDay, CountDay, TillDate.Date, KVPDoctorNo, AAGA_ISAS, ValidFrom.Date, ValidTill.Date, hd_storeId);
            if (hd_posdId != 0)
                await DB.recipe.asyncUpdatePosdRecipe(hd_posdId, hd_barcodeId, Qty, PaySum / Qty, hd_recipeId, PaySum);
            if ((Session.RecipeParm.send_to_tlk == 1 || Session.RecipeParm.send_to_tlk == 2) && E_Recipe == false)
            {
                send_recipe_kvr dlg_send = new send_recipe_kvr(hd_recipeId);
                dlg_send.Location = helpers.middleScreen(this.ParentForm, dlg_send);
                dlg_send.ShowDialog();
                if (dlg_send.DialogResult == DialogResult.Cancel)
                {
                    DB.recipe.asyncLoadRecipeData(hd_recipeId, LoadRecipeData_cb);
                    return;
                }
                dlg_send.Dispose();
                dlg_send = null;
            }

            if (Session.RecipeParm.print_on_save == 1)
            {
                if (helpers.alert(Enumerator.alert.confirm, "Ar spausdinti receptą?", true))
                {
                    print_recipe dlg_print = new print_recipe();
                    dlg_print.Location = helpers.middleScreen(this.ParentForm, dlg_print);
                    dlg_print.recipeId = hd_recipeId;
                    dlg_print.ShowDialog();
                    dlg_print.Dispose();
                    dlg_print = null;
                }
            }

            if (E_Recipe == true && in_erecipe != null)
            {
                await DB.eRecipe.CreateErecipe(hd_poshId, hd_posdId, RecipeNo, "kompensuojamas", hd_recipeId, RecipeDate, SalesDate, TillDate);//todo maybe in create_recipe()-nooo postgresql function?
                out_form_action = "recipe_saved";
                this.Parent.Controls.Remove(this);
            }
            else
            {
                form_wait(false);
                this.ParentForm.DialogResult = DialogResult.OK;
            }
        }

        private async void selectBarcode(string atc_code)
        {
            try
            {
                if (Session.RecipesOnly == true)
                {
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
                    string NpakId7 = await DB.recipe.getNpakId7(hd_productId);
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
                        if (CompPercent == "")
                                DB.recipe.asyncNewRecipeData(hd_posdId, CheckDate.Date, Barcode, "100", NewRecipeData_cb);
                            else
                                DB.recipe.asyncNewRecipeData(hd_posdId, CheckDate.Date, Barcode, CompPercent, NewRecipeData_cb);
                    }
                    else
                        throw new Exception("Negauta prekės " + NpakId7 + " informacija iš Registrų centro");
                }
                else
                {
                    if (CompPercent == "")
                        DB.recipe.asyncNewRecipeData(hd_posdId, CheckDate.Date, Barcode, "100", NewRecipeData_cb);
                    else
                        DB.recipe.asyncNewRecipeData(hd_posdId, CheckDate.Date, Barcode, CompPercent, NewRecipeData_cb);
                }
                tbRecipeNo.Select();
            }
            catch (Exception ex)
            {
                tbBarcode.ReadOnly = false;
                btnSelBarcode.Enabled = true;
                helpers.alert(Enumerator.alert.error, ex.Message);
                form_wait(false);
            }
        }

        private void tbCompCode_Leave(object sender, EventArgs e)
        {
            if (formWaiting == true)
                return;
            tbCompCode_Change();
            ChangedValuesEvent("tbCompCode_Change", new EventArgs());
        }

        private async void tbCompCode_Change()
        {
            /*if (tbCompCode.ReadOnly == true)
                return;*/
            if (tbCompCode.Text == "" && tbCompPercent.Text == "")
                return;
            try
            {
                form_wait(true);
                if (CompCode == 10 || CompCode == 11)
                {
                    decimal hd_compenscentrid = await DB.recipe.getCentrDebtorid();
                    if (hd_compenscentrid == 0)
                        throw new Exception("Centralizuoti rec. neaptarnaujami!");
                    else
                        tlkId = tlkId.Substring(0, 4) + "AA";
                }
                if (CompCode == 4 || CompCode == 5)
                {
                    if (E_Recipe == true && in_erecipe != null)
                        throw new Exception("Kompensacijos kodas nebegalioja!");
                    else
                        helpers.alert(Enumerator.alert.warning, "Kompensacijos kodas nebegalioja!");
                }
                CompPercent = "";
                DataTable temp = null;
                temp = DB.recipe.getCompPercent(CompCode);
                if (temp != null && temp.Rows.Count > 0)
                {
                    hd_compensationId = temp.Rows[0]["id"].ToDecimal();
                    CompPercent = temp.Rows[0]["comppercent"].ToString();
                    //tbRecipeDate.Select();
                }
                if (CompPercent == "")
                {
                    form_wait(false);
                    SelCompensation();
                    return;
                }
                //tbCompCode.ReadOnly = true;
                //btnSelCompensation.Enabled = false;
                //loadRecipeData();
                DataTable t = null;
                if (hd_productId > 0)
                {
                    t = DB.recipe.getTLKPrice(hd_productId, CompPercent, CheckDate);
                    if (t.Rows.Count > 0)
                    {
                        maxmkaina = t.Rows[0]["mmkaina"].ToDecimal();
                        maxbkaina = t.Rows[0]["mbkaina"].ToDecimal();
                    }

                    if (Session.PriceClass != "")
                    {
                        decimal sales_price_comp = 0;
                        sales_price_comp = await DB.prices.GetSalesPriceComp(hd_productId, get_compensation(CompPercent.ToDecimal()));
                        if (sales_price_comp > 0)
                            SalesPrice = sales_price_comp;

                        if (SalesPrice > maxmkaina && maxmkaina > 0)
                            SalesPrice = maxmkaina;

                        if (BasicPrice != maxbkaina && maxbkaina > 0)
                            BasicPrice = maxbkaina;
                    }
                }
                form_wait(false);
            }
            catch (Exception ex)
            {
                form_wait(false);
                helpers.alert(Enumerator.alert.error, ex.Message);
                btnClose_Click("tbCompCode_Change", new EventArgs());
            }
        }

        private void btnSelCompensation_Click(object sender, EventArgs e)
        {
            SelCompensation();
        }

        private void SelCompensation()
        {
            if (formWaiting == true)
                return;
            compensation dlg = new compensation();
            dlg.Location = helpers.middleScreen(this.ParentForm, dlg);
            dlg.ShowDialog();
            if (dlg.DialogResult == DialogResult.OK)
            {
                hd_compensationId = dlg.Id;
                CompCode = dlg.compCode.ToDecimal();
                CompPercent = dlg.compPercent;
                tbCompCode_Change();
                ChangedValuesEvent("SelCompensation", new EventArgs());
            }
            dlg.Dispose();
            dlg = null;
        }

        private void tbRecipeDate_Leave(object sender, EventArgs e)
        {
            TextBox tb = sender as TextBox;
            RecipeDate = helpers.format_date(tb.Text);
        }

        private void dtpRecipeDate_ValueChanged(object sender, EventArgs e)
        {
            DateTimePicker dtp = sender as DateTimePicker;
            tbRecipeDate.Text = dtp.Value.ToString("yyyy-MM-dd");
        }

        private void tbValidFrom_Leave(object sender, EventArgs e)
        {
            TextBox tb = sender as TextBox;
            ValidFrom = helpers.format_date(tb.Text);
            dtpValidFrom_Leave(sender, e);
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

        private void tbTilLDate_Leave(object sender, EventArgs e)
        {
            TextBox tb = sender as TextBox;
            TillDate = helpers.format_date(tb.Text);
        }

        private void dtpTillDate_ValueChanged(object sender, EventArgs e)
        {
            DateTimePicker dtp = sender as DateTimePicker;
            tbTillDate.Text = dtp.Value.ToString("yyyy-MM-dd");
            //calc_cont_recipe_date()
            ContValidFrom = TillDate.Date.AddDays(-4);
            if (in_erecipe != null)
                ContValidTill = ContValidFrom.Date.AddDays(in_erecipe.ValidationPeriod - 1);
            else
                ContValidTill = TillDate.Date.AddDays(30);
        }

        private void tbValidTill_Leave(object sender, EventArgs e)
        {
            TextBox tb = sender as TextBox;
            ValidTill = helpers.format_date(tb.Text);
        }

        private void dtpValidTill_ValueChanged(object sender, EventArgs e)
        {
            DateTimePicker dtp = sender as DateTimePicker;
            tbValidTill.Text = dtp.Value.ToString("yyyy-MM-dd");
        }

        private void chbExt_CheckedChanged(object sender, EventArgs e)
        {
            if (chbExt.Checked == true)
            {
                lblPastTillDate.Visible = true;
                tbPastTillDate.Visible = true;
                PastTillDate = ValidFrom.Date.AddDays(4);
            }
            else
            {
                lblPastTillDate.Visible = false;
                tbPastTillDate.Visible = false;
            }
            ValidTill = ValidFrom.AddDays(RecipeValidityPeriod);
            ChangedValuesEvent("chbExt_CheckedChanged", new EventArgs());
        }

        private void tbDeseaseCode_KeyPress(object sender, KeyPressEventArgs e)
        {
            tbString_KeyPress(sender, e);
            e.KeyChar = Char.ToUpper(e.KeyChar);
        }

        private async void tbDeseaseCode_Leave(object sender, EventArgs e)
        {
            TextBox tb = sender as TextBox;
            if (formWaiting == true)
                return;
            if (tb.Text == "")
                return;
            form_wait(true);
            if (DeseaseCode.Length > 3 && DeseaseCode.Substring(3, 1) != ".")
                DeseaseCode = DeseaseCode.Insert(3, ".");
            decimal deseaseID = await DB.recipe.getDeseaseId(DeseaseCode, SalesDate.Date);
            if (deseaseID == 0)
            {
                helpers.alert(Enumerator.alert.warning, "Tokio ligos kodo " + DeseaseCode + " nėra klasifikatoriuje arba jis negalioja!!!");
                DeseaseCode = "";
                tbDeseaseCode.Select();
            }
            form_wait(false);
        }

        private void tbDoctorCode_Leave(object sender, EventArgs e)
        {
            TextBox tb = sender as TextBox;
            if (formWaiting == true)
                return;
            if (tb.Text != "")
            {
                form_wait(true);
                DoctorName = "";
                DataTable temp = null;
                temp = DB.recipe.getDoctorName(DoctorCode);
                if (temp != null && temp.Rows.Count > 0)
                {
                    hd_doctorId = temp.Rows[0]["id"].ToDecimal();
                    DoctorName = temp.Rows[0]["name"].ToString();
                }
                else
                {
                    hd_doctorId = 0;
                    DoctorName = "";
                }
                form_wait(false);
                if (DoctorName == "" && tb.Text != "")
                {
                    btnSelDoctor_Click(sender, e);
                    return;
                }
            }
        }

        private void btnSelDoctor_Click(object sender, EventArgs e)
        {
            if (formWaiting == true)
                return;
            form_wait(true);
            doctor dlg = new doctor();
            dlg.Location = helpers.middleScreen(this.ParentForm, dlg);
            dlg.ShowDialog();
            if (dlg.DialogResult == DialogResult.OK)
            {
                hd_doctorId = dlg.Id;
                DoctorCode = dlg.dCode;
                DoctorName = dlg.dName;
            }
            dlg.Dispose();
            dlg = null;
            form_wait(false);
        }

        private void tbClinicCode_Leave(object sender, EventArgs e)
        {
            TextBox tb = sender as TextBox;
            if (formWaiting == true)
                return;
            if (tb.Text == "")
            {
                form_wait(true);
                ClinicName = "";
                DataTable temp = null;
                temp = DB.recipe.getHospitalName(ClinicCode);
                if (temp != null && temp.Rows.Count > 0)
                {
                    hd_clinicId = temp.Rows[0]["id"].ToDecimal();
                    ClinicName = temp.Rows[0]["name"].ToString();
                }
                else
                {
                    hd_clinicId = 0;
                    ClinicName = "";
                }
                form_wait(false);
                if (ClinicName == "" && tb.Text != "")
                {
                    btnSelClinic_Click(sender, e);
                    return;
                }
            }
        }

        private void btnSelClinic_Click(object sender, EventArgs e)
        {
            if (formWaiting == true)
                return;
            hospital dlg = new hospital();
            dlg.Location = helpers.middleScreen(this.ParentForm, dlg);
            dlg.ShowDialog();
            if (dlg.DialogResult == DialogResult.OK)
            {
                ClinicCode = dlg.hCode.ToDecimal();
                ClinicName = dlg.Name;
                hd_clinicId = dlg.Id;
            }
            dlg.Dispose();
            dlg = null;
        }
        
        private void tbSalesDate_Leave(object sender, EventArgs e)
        {
            TextBox tb = sender as TextBox;
            SalesDate = helpers.format_date(tb.Text);
        }

        private void dtpSalesDate_ValueChanged(object sender, EventArgs e)
        {
            DateTimePicker dtp = sender as DateTimePicker;
            tbSalesDate.Text = dtp.Value.ToString("yyyy-MM-dd");
        }

        private void tbCheckDate_Leave(object sender, EventArgs e)
        {
            TextBox tb = sender as TextBox;
            CheckDate = helpers.format_date(tb.Text);
        }

        private void dtpCheckDate_ValueChanged(object sender, EventArgs e)
        {
            DateTimePicker dtp = sender as DateTimePicker;
            tbCheckDate.Text = dtp.Value.ToString("yyyy-MM-dd");
        }

        private bool check_for_tlkid()
        {
            bool rez = false;
            if (tlkId == "4342")
                rez = true;
            if (tlkId == "3918")
                rez = true;
            if (tlkId == "3838")
                rez = true;
            if (tlkId == "3357")
                rez = true;
            if (tlkId == "3671")
                rez = true;
            if (tlkId == "3672")
                rez = true;
            if (tlkId == "1391")
                rez = true;
            if (tlkId == "0487")
                rez = true;
            if (tlkId == "2926")
                rez = true;
            if (tlkId == "4889")
                rez = true;
            return rez;
        }

        private void tbNumber_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (this.formWaiting == true)
            {
                e.Handled = true;
                return;
            }
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
                e.Handled = true;
        }

        private void tbString_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (this.formWaiting == true)
            {
                e.Handled = true;
                return;
            }
        }

        private void tbMoney_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (this.formWaiting == true)
            {
                e.Handled = true;
                return;
            }
            helpers.tb_KeyPress(sender, e);
        }

        private void tbQty_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (this.formWaiting == true)
            {
                e.Handled = true;
                return;
            }
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
        #region VARIABLES

        private string StoreName
        {
            get { return tbStoreName.Text; }
            set { tbStoreName.Text = value; }
        }

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
                tbCheckDate.ReadOnly = true;
                dtpCheckDate.Enabled = false;
                tbBarcodeName.Text = value;
            }
        }

        private decimal RecipeNo
        {
            get { return tbRecipeNo.Text.ToDecimal(); }
            set { tbRecipeNo.Text = value.ToString(); }
        }

        private string KVPDoctorNo
        {
            get { return tbKVPDoctorNo.Text; }
            set 
            {
                if (value.ToUpper() == "E")
                    E_Recipe = true;
                else
                    E_Recipe = false;
                tbKVPDoctorNo.Text = value;
            }
        }

        private decimal CompCode
        {
            get { return tbCompCode.Text.ToDecimal(); }
            set { tbCompCode.Text = value.ToString(); }
        }

        private string CompPercent
        {
            get { return tbCompPercent.Text; }
            set { tbCompPercent.Text = value; }
        }

        private DateTime RecipeDate
        {
            get { return dtpRecipeDate.Value; }
            set { dtpRecipeDate.Value = value; }
        }

        private DateTime ValidFrom
        {
            get { return dtpValidFrom.Value; }
            set { dtpValidFrom.Value = value; }
        }

        private decimal Doses
        {
            get { return tbDoses.Text.ToDecimal(); }
            set { tbDoses.Text = Math.Round(value).ToString(); }
        }

        private decimal QtyDay
        {
            get { return tbQtyDay.Text.ToDecimal(); }
            set { tbQtyDay.Text = value.ToString(); }
        }

        private int CountDay
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

        private DateTime ValidTill
        {
            get { return dtpValidTill.Value; }
            set { dtpValidTill.Value = value; }
        }

        private DateTime PastTillDate
        {
            get
            {
                DateTime date;
                if (DateTime.TryParse(tbPastTillDate.Text, out date))
                    return date;
                else
                    return DateTime.Now;
            }
            set { tbPastTillDate.Text = value.Date.ToString().Substring(0, 10); }
        }

        private DateTime ContValidFrom
        {
            get { return DateTime.Parse(tbContValidFrom.Text); }
            set { tbContValidFrom.Text = value.Date.ToString().Substring(0, 10); }
        }

        private DateTime ContValidTill
        {
            get { return DateTime.Parse(tbContValidTill.Text); }
            set { tbContValidTill.Text = value.Date.ToString().Substring(0, 10); }
        }

        private string DeseaseCode
        {
            get { return tbDeseaseCode.Text; }
            set { tbDeseaseCode.Text = value; }
        }

        private string AAGA_ISAS
        {
            get { return tbAAGA_ISAS.Text; }
            set { tbAAGA_ISAS.Text = value; }
        }

        private bool PrescriptionTagsLongTag
        {
            get { return chbExt.Checked; }
            set { chbExt.Checked = value; }
        }

        private string Water
        {
            get { return tbWater.Text; }
            set { tbWater.Text = value; }
        }

        private string DoctorCode
        {
            get { return tbDoctorCode.Text; }
            set 
            {
                tbDoctorCode.Text = value; 
            }
        }

        private string DoctorName
        {
            get { return tbDoctorName.Text; }
            set { tbDoctorName.Text = value; }
        }

        private decimal Taxolaborum
        {
            get { return tbTaxolaborum.Text.ToDecimal(); }
            set { tbTaxolaborum.Text = value.ToString(); }
        }

        private decimal ClinicCode
        {
            get { return tbClinicCode.Text.ToDecimal(); }
            set
            {
                if (value == 0)
                    tbClinicCode.Text = "";
                else
                    tbClinicCode.Text = value.ToString();
            }
        }

        private string ClinicName
        {
            get { return tbClinicName.Text; }
            set { tbClinicName.Text = value; }
        }

        private int Ext
        {
            get { return Convert.ToInt32(chbExt.Checked); }
            set { chbExt.Checked = Convert.ToBoolean(value); }
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

        private DateTime SalesDate
        {
            get { return dtpSalesDate.Value; }
            set { dtpSalesDate.Value = value; }
        }

        private DateTime CheckDate
        {
            get { return dtpCheckDate.Value; }
            set { dtpCheckDate.Value = value; }
        }

        private decimal SalesPrice
        {
            get { return tbSalesPrice.Text.ToDecimal(); }
            set { tbSalesPrice.Text = value.ToString(); }
        }

        private decimal BasicPrice
        {
            get { return tbBasicPrice.Text.ToDecimal(); }
            set { tbBasicPrice.Text = value.ToString(); }
        }

        private string tlkId
        {
            get { return tbtlkId.Text; }
            set { tbtlkId.Text = value; }
        }

        private decimal CheckNo
        {
            get { return tbCheckNo.Text.ToDecimal(); }
            set { tbCheckNo.Text = value.ToString(); }
        }

        public decimal CompSum
        {
            get { return tbCompSum.Text.ToDecimal(); }
            set { tbCompSum.Text = value.ToString(); }
        }

        public decimal PaySum
        {
            get { return tbPaySum.Text.ToDecimal(); }
            set { tbPaySum.Text = value.ToString(); }
        }

        public decimal TotalSum
        {
            get { return tbTotalSum.Text.ToDecimal(); }
            set { tbTotalSum.Text = value.ToString(); }
        }

        private decimal RecipeValid
        {
            get { return tbRecipeValid.Text.ToDecimal(); }
            set { tbRecipeValid.Text = value.ToString(); }
        }

        private bool E_Recipe
        {
            get 
            {
                if (KVPDoctorNo.ToUpper() == "E")
                    return true;
                else
                    return false;
            }
            set 
            {
                if (value == true)
                {
                    btnCheck.Enabled = false;
                    tbKVPDoctorNo.ReadOnly = true;
                }
                else
                {
                    btnCheck.Enabled = true;
                    tbKVPDoctorNo.ReadOnly = false;
                }
            }
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

        #endregion
        private decimal get_compensation(decimal compensation_percent)
        {
            decimal res = 0;
            hd_compensation.TryGetValue(compensation_percent, out res);
            return res;
        }
        private void ChangedValuesEvent(object sender, EventArgs e)
        {
            if (hd_productId == 0)
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
            if (hd_barratio > 0 && hd_prodratio > 0)
                TotalSum = Math.Round(((SalesPrice / (hd_barratio / hd_prodratio)) * GQty), 2);

            // pagal komp. sumas, ne pagal % nuo bazines kainos !!!!
            decimal rec_comp_old = 0;
            if (tlkId.StartsWith("M") || tlkId.StartsWith("G"))
                rec_comp_old = Qty * BasicPrice;
            else
                rec_comp_old = Qty * get_compensation(CompPercent.ToDecimal());
            CompSum = get_compensation(CompPercent.ToDecimal());

            decimal compS = CompSum;
            decimal compS_old = rec_comp_old;
            if (Qty > 0)
                compS_old = compS_old / Qty;

            decimal sumaminusprimoka = CompPercent.ToDecimal() / 100 * BasicPrice;
            decimal c3 = Math.Round(compS / hd_barratio * GQty, 3);
            compS = c3;

            if (tlkId.StartsWith("M") || tlkId.StartsWith("G") || check_for_tlkid() || compS_old - sumaminusprimoka <= 0.01M)
                compS_old = CompPercent.ToDecimal() / 100 * BasicPrice * Qty;
            compS_old = Math.Round(compS_old, 2);

            if (compS == 0)
                compS = CompPercent.ToDecimal() / 100 * BasicPrice * Qty;
            if (compS > TotalSum)
                compS = TotalSum;
            CompSum = Math.Round(compS, 2, MidpointRounding.AwayFromZero);
            PaySum = Math.Round(TotalSum - CompSum, 2);
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
                //VALIDATION
                if (in_erecipe != null)//only for erecipes
                {
                    if ((in_erecipe.eRecipe_ValidTo.AddDays(1) - in_erecipe.eRecipe_ValidFrom).TotalDays > 180)
                        throw new Exception("Elektroninio recepto galiojimo laikas negali būti didesnis nei 180 dienų!", new Exception(Enumerator.alert.error.ToString()));
                    if (in_erecipe.eRecipe_PrescriptionTagsLongTag && Ext == 0 //pirmas isdavimas
                                    && (DateTime.Now.AddDays(1) - in_erecipe.eRecipe_ValidFrom).TotalDays > 30)
                        throw new Exception("Gydymui tęsti recepto pirmą išdavimą galima išduoti tik per 30 dienų nuo galiojimo datos!", new Exception(Enumerator.alert.error.ToString()));
                    if (sender as string == "NewRecipeData_cb" 
                        && in_erecipe.eRecipe_PrescriptionTagsLongTag == true 
                        && in_erecipe.DispenseCount > 0 
                        && DateTime.Now.Date > in_erecipe.PastValidTo.AddDays(in_erecipe.ValidationPeriod))
                        helpers.alert(Enumerator.alert.info, "Praleistas prieš tai galiojęs išdavimas.\nPrašome patikrint galiojimo laikotarpius!");
                }
                else//only for paper recipes
                {
                    decimal tmp = 0;
                    if (Ext == 1)
                    {
                        if (ValidFrom.Date != ValidTill.Date.AddDays(-1 * RecipeValidityPeriod))
                            throw new Exception("Tęstintio recepto galiojimas turi būti 35 d. Šiuo metu yra " + RecipeValid + " d.");
                        int anksti = helpers.get_interval(SalesDate.Date, PastTillDate.Date);
                        int velai = helpers.get_interval(PastTillDate.Date, SalesDate.Date);
                        if (anksti >= 10)
                            helpers.alert(Enumerator.alert.warning, "Iki praeito recepto pakanka iki dienos liko " + (anksti + 1) + " d.");
                        if (velai > 30)
                            helpers.alert(Enumerator.alert.warning, "Nuo praeito recepto pakanka iki dienos praėjo " + velai + " d.");
                        tmp = 4;
                    }
                    if (helpers.betweenday2(RecipeDate.Date, ValidFrom.Date) + tmp < 0)
                        throw new Exception("Neteisingos datos!!!");
                    int recipe_valid = 30 + (chbExt.Checked ? 5 : 0);
                    if (RecipeValid < recipe_valid)
                        helpers.alert(Enumerator.alert.warning, "Neteisingas recepto galiojimo laikotarpis!");
                }
                //for all recipes
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
                    {
                        form_wait(false);
                        btnClose_Click(btnClose, new EventArgs());
                    }
                }
            }
        }
    }
}
