using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Linq;
using POS_display.Repository.Price;
using POS_display.Repository.Recipe;
using POS_display.Utils.Logging;
using POS_display.wpf;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Tamroutilities.Client;
using TamroUtilities.HL7.Models;

namespace POS_display
{
    public partial class ucRecipeEditV2 : ucRecipeEditBase, IRecipeEdit
    {
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
        private readonly RecipeRepository recipeRepository;
        private readonly PriceRepository priceRepository;
        private decimal discount = 0;
        private decimal price = 0;
        private string hd_note2 = string.Empty;
        private const string DataModelVersionValue = "v29";
        private decimal _previousQtyDay = 0;

        private const decimal ProducedCompensatedDrugId = 10000117369;
        #region Callbacks
        private async Task GetRecipeData(decimal posId, DateTime checkDate, string barcode, string compensationPercent)
        {
            var t = await DB.recipe.asyncNewRecipeData(posId, checkDate.Date, barcode, compensationPercent);
            if (t.Rows.Count > 0)
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
                    btnAgeInput.Enabled = false;
                    tbPersonalCode.ReadOnly = true;
                    tbPersonalCode.Text = in_erecipe.Patient.PersonalCode;
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
                hd_note2 = t.Rows[0]["note2"].ToString();
                bool isNotCompensated = false;
                var priceRestrictions = await recipeRepository.GetPriceRestrictions(tlkId);
                foreach (var priceRestriction in priceRestrictions)
                {
                    if (ValidFrom >= priceRestriction.StartDate && ValidFrom < priceRestriction.EndDate)
                    {
                        if (!helpers.alert(Enumerator.alert.confirm, "Pagal šį receptą išduodamas vaistas negali būti kompensuojamas.\nAr norite parduoti be kompensacijos?", true))
                            throw new Exception("");
                        isNotCompensated = true;
                    }
                }

                if (in_erecipe != null && !await CheckCompensation(tlkId, CompCode) && (hd_productId != 10000084821 && hd_productId != 10000116398 && hd_productId != 10000116399 && hd_productId != 10000117369))//ne gaminami vaistai
                {
                    if (!helpers.alert(Enumerator.alert.confirm, "Šis kompensuojamas vaistas negali būti parduodamas su šiuo kompensacijos kodu.\n" +
                        " Ar norite tęsti pardavimą?", true))
                        throw new Exception("");
                    isNotCompensated = true;
                }
                if (isNotCompensated)
                {
                    CompCode = 4;
                    if (in_erecipe != null)
                    {
                        in_erecipe.eRecipe_CompensationCode = CompCode;
                        in_erecipe.eRecipe_CompensationTag = false;
                        in_erecipe.AdditionalInstructionsForPatient = "Pacientas pats pasirinko pirkti nekompensuojamą prekę.";
                    }
                }
                if (hd_posdPriceDiscounted == 0 || (maxmkaina < hd_posdPriceDiscounted && maxmkaina != 0))
                    SalesPrice = maxmkaina;
                else
                    SalesPrice = hd_posdPriceDiscounted;
                if (hd_prodratio > 0)
                    GQty = Qty * hd_barratio / hd_prodratio;
                if (CompCode == 4)
                {
                    LowIncomeTag = false;
                    EligibleSurcharge = false;
                }
                if (LowIncomeTag || AllowEligibleSurcharge)
                    PrepCompSum = t.Rows[0]["padeng_priemoka"].ToDecimal();
                else
                    PrepCompSum = 0;

                await tbCompCode_Change();
                form_wait(false);
                ChangedValuesEvent("NewRecipeData_cb", new EventArgs());
            }
            else
                throw new Exception("prekė yra nekompensuojama, tolimesni veiksmai negalimi!");
        }

        private async Task<bool> CheckCompensation(string npakid7, decimal compensationCode)
        {
            try
            {
                var tamroClient = Program.ServiceProvider.GetRequiredService<ITamroClient>();
                var response = await tamroClient.GetAsync<dynamic>
                    (string.Format(Session.CKasV1GetVlkPricesCheckCompensation, npakid7, compensationCode));

                JArray jsonArray = JArray.Parse(response.ToString());
                if (jsonArray == null || !jsonArray.Any())                
                    return false;                

                var compensations = jsonArray
                    .FirstOrDefault(obj => obj["compensationTypes"] is JArray compensationTypes &&
                    compensationTypes.Count > 0);

                return compensations != null;
            }
            catch (Exception ex) 
            {
                Serilogger.GetLogger().Information($"[CheckCompensation] Pharmacy: {Session.SystemData.kas_client_id}; Error: {ex.Message}");
                return false;
            }
        }

        private bool CheckCheapest(string npakid7)
        {
            try
            {
                bool isCheapest =
                    Session.TLKCheapests.Where(val => val.StartDate <= DateTime.Now && val.Npakid7 == npakid7)
                    .OrderByDescending(val => val.PriceListVersion)
                    .Select(val => val.IsCheapest)
                    .FirstOrDefault();
                return isCheapest;
            }
            catch (Exception ex)
            {
                Serilogger.GetLogger().Information($"[CheckCheapest] Pharmacy: {Session.SystemData.kas_client_id}; Error: {ex.Message}");
                return false;
            }
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
                    PrepCompSumTotal = t.Rows[0]["prepayment_compensation"].ToDecimal();
                    PrepCompSum = Math.Round(PrepCompSumTotal / Qty, 2, MidpointRounding.AwayFromZero);
                    LowIncomeTag = t.Rows[0]["is_prepayment_compensation"].ToDecimal() == 1 && CompCode > 0;
                    EligibleSurcharge = t.Rows[0]["surcharge_eligible"].ToDecimal() == 1 && CompCode > 0;
                    hd_note2 = t.Rows[0]["note2"].ToString();
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
                    //ChangedValuesEvent("LoadRecipeData_cb", new EventArgs());
                    tbPersonalCode.Select();
                }
            }));
        }
        #endregion
        public ucRecipeEditV2(Items.eRecipe.Recipe erecipe_item, Items.posd posd_item, string PickedUpByRef = "")
        {
            InitializeComponent();
            recipeRepository = new RecipeRepository();
            priceRepository = new PriceRepository();
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
                discount = posd_item.discount;
                price = posd_item.price;
            }
            in_PickedUpByRef = PickedUpByRef;
            btnAgeInput.Visible = Session.Params.FirstOrDefault(x => x.system == "POS" && x.par == "PADENG_PRIEMOK_VISIBILITY")?.value == "1";
            tbSalesPrice.ReadOnly = discount != 0;
        }

        private async void recipe_edit_Load(object sender, EventArgs e)
        {
            try
            {
                if (!Session.User.postname.StartsWith("Vaist") && !Session.User.postname.StartsWith("Farma") && !Session.User.postname.StartsWith("Ved") && !Session.User.postname.StartsWith("Vad"))
                    throw new Exception(Session.User.postname + " negali parduoti kompensuojamų receptų!");
                form_wait(true);
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
                DB.POS.UpdateSession("Receptai", 2);
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
                if (Session.RecipesOnly == false && Session.Admin == false)
                {
                    tbQty.ReadOnly = true;
                    tbGQty.ReadOnly = true;
                }
                LowIncomeTag = in_erecipe?.HasLowIncome?.HasLowIncome ?? false;
                EligibleSurcharge = in_erecipe?.AccumulatedSurcharge?.SurchargeEligible ?? false;
                if (hd_recipeId == 0)//new recipe
                {
                    hd_storeId = Session.SystemData.storeid;
                    StoreName = Session.SystemData.storename;
                    if (hd_posdId > 0)//from POS
                    {
                        await GetRecipeData(hd_posdId, CheckDate.Date, Barcode, "100");
                        tbPersonalCode.Select();
                        form_wait(false);
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


                if (in_erecipe.IsDispenseBySubtances)
                {
                    var group_id = await recipeRepository.GetPartialDispenseGroupIdByPosHeaderId(hd_poshId);
                    in_erecipe.DispenseBySubstancesGroupId = string.IsNullOrEmpty(group_id) ? Guid.NewGuid().ToString() : group_id;
                    cbOverLimitQty.Enabled = string.IsNullOrEmpty(group_id) && hd_note2 != "NARC" && hd_note2 != "PSYC";
                }
                else
                {
                    cbOverLimitQty.Enabled = hd_note2 != "NARC" && hd_note2 != "PSYC";
                }
            }
            catch (Exception ex)
            {
                if (!IsDisposed)
                {
                    form_wait(false);
                    helpers.alert(Enumerator.alert.error, ex.Message);
                    btnClose_Click("recipe_edit_Load", new EventArgs());
                }
            }
        }

        public new void form_wait(bool wait)
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

        private async void btnSave_Click(object sender, EventArgs e)
        {
            if (formWaiting)
                return;
            form_wait(true);
            try
            {
                if (DoctorCode != "" && DoctorName == "")
                    throw new Exception("Neteisingas gydytojo kodas!");
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
                if (BasicPrice == 0 && CompCode != 4)
                    throw new Exception("BAZINĖ KAINA = 0!");
                if (Qty <= 0)
                    throw new Exception("Neteisingas kiekis!");
                if (!IsOverLimitQty && CountDay > 90 && (hd_note2 != "PSYC" && hd_note2 != "NARC"))
                {
                    if (!helpers.alert(Enumerator.alert.warning, "Išduodamas kiekis ilgesniam nei 90 dienų vartojimui,\n" +
                        "bet nepažymėjote, kad išduodamas didesnis vaisto kiekis.\nAr norite tęsti?", true))
                    {
                        throw new Exception();
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

                if (Session.RecipesOnly && in_erecipe.eRecipe_CompensationTag && in_erecipe.eRecipe.Type.ToLowerInvariant() == "mpp")
                    in_erecipe.MppBarcode = Barcode;

                in_erecipe.PrescriptionStatus = prescriptionStatus;

                DataTable t = null;
                t = DB.recipe.getInvalidRecipe(tbRecipeNo.Text, tbKVPDoctorNo.Text);
                if (t != null && t.Rows.Count > 0)
                    throw new Exception("DĖMESIO! Gydytojo numeris arba recepto numeris yra falsifikuotų sąraše. Receptas neišsaugomas.", new Exception(Enumerator.alert.error.ToString()));
                //KVAP check if the recipe is unused
                var validations = new List<check_recipe_kvr.validation>();
                var personalCode = in_erecipe?.Patient?.PersonalCode != null ? in_erecipe?.Patient?.PersonalCode : tbPersonalCode.Text;
                if (CompSum > 0)
                {
                    if (string.IsNullOrWhiteSpace(personalCode))
                    {
                        tbPersonalCode.Select();
                        throw new Exception("Nenurodytas asmens kodas!");
                    }
                    validations.Add(check_recipe_kvr.validation.RecipeUnused);
                }
                if (Session.RecipeParm.check_ == 1)
                    validations.Add(check_recipe_kvr.validation.KvrCheck);
                if (validations.Any())
                {
                    var npakid7 = await DB.recipe.getNpakId7(hd_productId);
                    using (var dlg = new check_recipe_kvr(RecipeNo, KVPDoctorNo, SalesDate, RecipeDate, E_Recipe, validations, npakid7, personalCode))
                    {
                        await dlg.ValidateRecipe();
                        if (dlg.RecipeErrors.Any())
                        {
                            dlg.Location = helpers.middleScreen(this.ParentForm, dlg);
                            dlg.ShowDialog();
                            if (dlg.RecipeErrors.Any())//TM: need to double check this, becouse inside dialog window there is manual error clearance button Ignore
                                throw new Exception("");
                        }
                        else if (!dlg.IsSuccessfulCheck && !helpers.alert(Enumerator.alert.warning, "TLK automatinė receptų patikra nepavyko.\n" +
                                "Receptą reikia patikrinti rankiniu būdu.\n" +
                                "Ar norite išsaugoti nepatikrintą receptą?", true)) 
                        {
                            throw new Exception("");
                        }
                    }
                }
                bool success = false;
                if (hd_recipeId == 0)
                {
                    hd_recipeId = await DB.recipe.asyncCreateRecipe(tlkId, hd_barcodeId, "", RecipeNo, "", hd_clinicId.ToString(), DeseaseCode, hd_doctorId.ToString(), RecipeDate.Date, SalesPrice, BasicPrice, hd_compensationId, Qty, TotalSum, CompSum, PaySum, SalesDate.Date, GQty, Water.ToDecimal(), Taxolaborum, Ext, CheckDate.Date, CheckNo, QtyDay, CountDay, TillDate.Date, KVPDoctorNo, AAGA_ISAS, ValidFrom.Date, ValidTill.Date, hd_storeId);
                    success = hd_recipeId > 0;
                    //this method exists because the create method has a limit on variables passed to it.
                }
                else
                    success = await DB.recipe.asyncUpdateRecipe(hd_recipeId, tlkId, hd_barcodeId, "", RecipeNo, "", hd_clinicId.ToString(), DeseaseCode, hd_doctorId.ToString(), RecipeDate.Date, SalesPrice, BasicPrice, hd_compensationId, Qty, TotalSum, CompSum, PaySum, SalesDate.Date, GQty, Water.ToDecimal(), Taxolaborum, Ext, CheckDate.Date, CheckNo, QtyDay, CountDay, TillDate.Date, KVPDoctorNo, AAGA_ISAS, ValidFrom.Date, ValidTill.Date, hd_storeId);
                
                if (success)
                {
                    var isCheapest = CheckCheapest(tlkId);
                    var hasLowIncome = LowIncomeTag;
                    var hasEligibleSurchargeAndCheapest = AllowEligibleSurcharge && isCheapest;
                    var hasEligibleSurchargeAndProducedDrug = AllowEligibleSurcharge && hd_productId == ProducedCompensatedDrugId;

                    await recipeRepository.UpdateRecipeCompensationData(
                        hd_recipeId,
                        PrepCompSumTotal,
                        in_erecipe?.eRecipe?.PrescriptionTagsFirstPrescribingTag?.ToLower() == "true" ? true : false,
                        LowIncomeTag,
                        hasLowIncome || hasEligibleSurchargeAndCheapest || hasEligibleSurchargeAndProducedDrug);

                    await recipeRepository.SetAdditionalInstructions(
                        hd_recipeId.ToLong(),
                        in_erecipe?.AdditionalInstructionsForPatient);

                    if (in_erecipe?.AccumulatedSurcharge != null)
                    {
                        await recipeRepository.SetAccumulatedSurchargeData(
                            hd_recipeId.ToLong(),
                            in_erecipe.AccumulatedSurcharge.SurchargeEligible,
                            in_erecipe.AccumulatedSurcharge.SurchargeAmount,
                            in_erecipe.AccumulatedSurcharge.MissingSurchargeAmount,
                            in_erecipe.AccumulatedSurcharge.ValidTo ?? DateTime.Now);
                    }
                }
                if (success && hd_posdId != 0)
                {
                     success = await DB.recipe.asyncUpdatePosdRecipe(hd_posdId,
                        hd_barcodeId,
                        Qty,
                        PaySum / Qty,
                        hd_recipeId,
                        PaySum);
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

                if (E_Recipe && in_erecipe != null)
                {
                    in_erecipe.IsOverLimitQty = IsOverLimitQty;
                    var erecipeId = in_erecipe.IsDispenseBySubtances
                        ? await Controllers.eRecipe.CreateGroupDipsenseEntry(
                            in_erecipe,
                            in_PickedUpByRef,
                            hd_poshId,
                            hd_posdId,
                            hd_productId,
                            hd_recipeId,
                            TillDate,
                            RecipeDate,
                            SalesDate,
                            TotalSum,
                            PaySum,
                            CompSum,
                            GQty,
                            PrepCompSumTotal,
                            CountDay)
                        : await Controllers.eRecipe.CreateERecipe(
                            in_erecipe,
                            in_PickedUpByRef,
                            hd_poshId,
                            hd_posdId,
                            hd_productId,
                            hd_recipeId,
                            TillDate,
                            RecipeDate,
                            SalesDate,
                            TotalSum,
                            PaySum,
                            CompSum,
                            GQty,
                            PrepCompSumTotal,
                            CountDay);

                    success = erecipeId > 0;

                    if (success)
                    {
                        await recipeRepository.SetPartialDispenseGroupId(erecipeId.ToLong(), in_erecipe?.DispenseBySubstancesGroupId);
                        await recipeRepository.SetERecipeDiseaseCode(erecipeId.ToLong(), in_erecipe?.eRecipe?.ReasonCode);
                        btnSave.Enabled = false;
                    }
                }

                if ((Session.RecipeParm.send_to_tlk == 1 //siusti visus
                    || (Session.RecipeParm.send_to_tlk == 2 && !E_Recipe) //tik popierinius
                    || (Session.RecipeParm.send_to_tlk == 3 && E_Recipe)) //tik elektroninius
                    && CompCode != 4)
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
                if (success)
                    CloseWindow(DialogResult.OK);
                else
                    CloseWindow(DialogResult.Cancel);
            }
            catch (Exception ex)
            {
                form_wait(false);
                Enumerator.alert alert_type = ex.InnerException?.Message?.ToEnum<Enumerator.alert>() ?? Enumerator.alert.warning;
                helpers.alert(alert_type, ex.Message);
                if (alert_type == Enumerator.alert.error)//critical error = close window
                    btnClose_Click("btnSave_Click", new EventArgs());
                Serilogger.GetLogger().Error(ex, ex.Message);
            }
        }

        private async void btnCommit_Click(object sender, EventArgs e)
        {
            if (formWaiting)
                return;

            form_wait(true);
            bool success = await DB.recipe.asyncCommitRecipe(hd_recipeId, tlkId, hd_barcodeId, "", RecipeNo, "", hd_clinicId.ToString(), DeseaseCode, hd_doctorId.ToString(), RecipeDate.Date, SalesPrice, BasicPrice, hd_compensationId, Qty, TotalSum, CompSum, PaySum, SalesDate.Date, GQty, Water.ToDecimal(), Taxolaborum, Ext, CheckDate.Date, CheckNo, QtyDay, CountDay, TillDate.Date, KVPDoctorNo, AAGA_ISAS, ValidFrom.Date, ValidTill.Date);
            form_wait(false);

            if (success)
                btnClose_Click("CommitRecipe_cb", new EventArgs());
            else
                helpers.alert(Enumerator.alert.warning, "Nepavyko patvirtinti recepto.");
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            if (formWaiting)
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
            if (formWaiting)
                return;
            if (helpers.alert(Enumerator.alert.confirm, "Receptas patvirtintas! Ar tikrai norite jį atšaukti?", true))
            {
                form_wait(true);
                bool success = await DB.recipe.asyncRepairRecipe(hd_recipeId);
                form_wait(false);

                if (success)
                    btnClose_Click("RepairRecipe_cb", new EventArgs());
                else
                    helpers.alert(Enumerator.alert.warning, "Nepavyko koreguoti recepto.");
            }
        }

        private bool CheckIfAgeIsOver75FromPersonalCode(string personalCode)
        {
            btnAgeInput.Enabled = false;
            if (personalCode.Length == 11)
            {
                try
                {
                    var dateTimeNow = DateTime.Now;
                    int year;
                    switch (personalCode[0])
                    {
                        case '3':
                        case '4':
                            year = 1900 + personalCode.Substring(1, 2).ToInt();
                            break;
                        case '5':
                        case '6':
                            year = 2000 + personalCode.Substring(1, 2).ToInt();
                            break;
                        default:
                            throw new FormatException();
                    }
                    var month = personalCode.Substring(3, 2).ToInt();
                    var day = personalCode.Substring(5, 2).ToInt();

                    bool isOver75 = (dateTimeNow.Year - year > 75 ||
                    dateTimeNow.Year - year == 75 && month < dateTimeNow.Month ||
                    dateTimeNow.Year - year == 75 && month == dateTimeNow.Month && day <= dateTimeNow.Day);
                    return isOver75;
                }
                catch (Exception)
                {
                    btnAgeInput.Enabled = true;
                    return false;
                }

            }
            else return false;
        }
        private async void btnAgeInput_Click(object sender, EventArgs e)//todo arm remove
        {
            if (formWaiting == true)
                return;

            ageInputPopup ageInputDialog = new ageInputPopup();
            ageInputDialog.Location = helpers.middleScreen(this.ParentForm, ageInputDialog);
            if (ageInputDialog.ShowDialog() == DialogResult.OK)
            {
                LowIncomeTag = ageInputDialog.isOver75;
                form_wait(true);
                await this.tbCompCode_Change(true);
                this.ChangedValuesEvent("", new EventArgs());
                form_wait(false);
            }
            ageInputDialog.Dispose();
        }

        private async void btnCheck_Click(object sender, EventArgs e)
        {
            if (formWaiting == true)
                return;
            form_wait(true);
            var npakid7 = await DB.recipe.getNpakId7(hd_productId);
            var personalCode = in_erecipe?.Patient?.PersonalCode != null ? in_erecipe?.Patient?.PersonalCode : tbPersonalCode.Text;
            var validations = new List<check_recipe_kvr.validation>
            {
                check_recipe_kvr.validation.RecipeUnused,
                check_recipe_kvr.validation.KvrCheck
            };
            using (var dlg = new check_recipe_kvr(RecipeNo, KVPDoctorNo, SalesDate, RecipeDate, E_Recipe, validations, npakid7, personalCode))
            {
                await dlg.ValidateRecipe();
                dlg.Location = helpers.middleScreen(this.ParentForm, dlg);
                dlg.ShowDialog();
            }
            form_wait(false);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            if (formWaiting == true)
                return;
            form_wait(true);
            CloseWindow(DialogResult.Cancel);
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

        private async void selectBarcode(string atc_code)
        {
            try
            {
                if (Session.RecipesOnly == true)
                {
                    string NpakId7 = await DB.recipe.getNpakId7(hd_productId);
                    if (NpakId7 == "1000000")//gaminami vaistai
                        NpakId7 = "";
                    MedicationDto product_medication = new MedicationDto();
                    MedicationListDto contained_medication_list = new MedicationListDto();
                    if (NpakId7 != "")
                    {
                        product_medication = Session.eRecipeUtils.GetMedicationList<MedicationListDto>("NPAKID7", NpakId7).MedicationList.First();
                        if (!String.IsNullOrWhiteSpace(in_erecipe.eRecipe.MedicationId))
                            contained_medication_list = Session.eRecipeUtils.GetMedicationList<MedicationListDto>("ContainedMedicationId", in_erecipe.eRecipe.MedicationId);
                    }
                    else if (in_erecipe.eRecipe.Type == "ev")//ekstemporalus vaistas
                    {
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
                        if (CompPercent == "")
                            await GetRecipeData(hd_posdId, CheckDate.Date, Barcode, "100");
                        else
                            await GetRecipeData(hd_posdId, CheckDate.Date, Barcode, CompPercent);
                    }
                    else
                        throw new Exception("Negauta prekės " + NpakId7 + " informacija iš Registrų centro");
                }
                else
                {
                    if (CompPercent == "")
                        await GetRecipeData(hd_posdId, CheckDate.Date, Barcode, "100");
                    else
                        await GetRecipeData(hd_posdId, CheckDate.Date, Barcode, CompPercent);
                }
                tbPersonalCode.Select();
            }
            catch (Exception ex)
            {
                tbBarcode.ReadOnly = false;
                btnSelBarcode.Enabled = true;
                helpers.alert(Enumerator.alert.error, ex.Message);
                btnClose_Click("NewRecipeData_cb", new EventArgs());
            }
            finally
            {
                form_wait(false);
            }
        }

        private async void tbCompCode_Leave(object sender, EventArgs e)
        {
            if (formWaiting == true)
                return;
            await tbCompCode_Change();
            ChangedValuesEvent("tbCompCode_Change", new EventArgs());
        }

        private async Task tbCompCode_Change(bool recalcBecauseLowIncomeChanged = false)
        {
            PriceRepository priceRpository = new PriceRepository();
            if (tbCompCode.Text == "" && tbCompPercent.Text == "" && !recalcBecauseLowIncomeChanged)
                return;
            try
            {
                form_wait(true);
                if (CompCode == 1 || CompCode == 2 || CompCode == 6 || CompCode == 7)
                    helpers.alert(Enumerator.alert.warning, "Recepte nurodytas senas komp. rūšies kodas, informuokite receptą išrašiusią gydymo įstaigą. Recepto išduoti negalima.");
                if (CompCode == 10 || CompCode == 11)
                {
                    decimal hd_compenscentrid = await DB.recipe.getCentrDebtorid();
                    if (hd_compenscentrid == 0)
                        throw new Exception("Centralizuoti rec. neaptarnaujami!");
                    else
                        tlkId = tlkId.Substring(0, 4) + "AA";
                }
                if (/*CompCode == 4 || */CompCode == 5)//TM praleidziam 4 koda su 0
                {
                    if (E_Recipe == true && in_erecipe != null)
                        throw new Exception("Kompensacijos kodas nebegalioja!");
                    else
                        helpers.alert(Enumerator.alert.warning, "Kompensacijos kodas nebegalioja!");
                }
                CompPercent = "";
                DataTable temp = null;
                temp = await DB.recipe.getCompPercent(CompCode);
                if (temp != null && temp.Rows.Count > 0)
                {
                    hd_compensationId = temp.Rows[0]["id"].ToDecimal();
                    CompPercent = temp.Rows[0]["comppercent"].ToString();
                    //tbRecipeDate.Select();
                }
                if (CompPercent == "" && !recalcBecauseLowIncomeChanged)
                {
                    form_wait(false);
                    await SelCompensation();
                    return;
                }
                if (E_Recipe)
                {
                    tbCompCode.ReadOnly = true;
                    btnSelCompensation.Enabled = false;
                }
                //loadRecipeData();
                DataTable t = null;
                if (hd_productId > 0 && CompPercent.ToInt() > 0)
                {
                    t = await DB.recipe.getTLKPrice(hd_productId, CompPercent, CheckDate);
                    if (t.Rows.Count > 0)
                    {
                        maxmkaina = t.Rows[0]["mmkaina"].ToDecimal();
                        maxbkaina = t.Rows[0]["mbkaina"].ToDecimal();
                    }

                    if (Session.PriceClass != "")
                    {
                        decimal sales_price_comp = 0;
                        var gr4 = await DB.POS.getGr4(hd_productId);
                        var priceClass = Session.PriceClass;
                        if (LowIncomeTag && gr4 == "MEDPK")
                        {
                            priceClass = "pk3";
                        }
                        sales_price_comp = await priceRepository.GetSalesPriceComp(hd_productId, get_compensation(CompPercent.ToDecimal()), priceClass);

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

        private async void btnSelCompensation_Click(object sender, EventArgs e)
        {
            await SelCompensation();
        }

        private async Task SelCompensation()
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
                await tbCompCode_Change();
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

        public DateTime RecipeDate
        {
            get { return dtpRecipeDate.Value; }
            set { dtpRecipeDate.Value = value; }
        }

        public DateTime ValidFrom
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

        private decimal HiddenQtyDay { get; set; }

        public DateTime TillDate
        {
            get { return dtpTillDate.Value; }
            set { dtpTillDate.Value = value; }
        }

        public DateTime ValidTill
        {
            get { return dtpValidTill.Value; }
            set { dtpValidTill.Value = value; }
        }

        public DateTime PastTillDate
        {
            get
            {
                DateTime date;
                if (DateTime.TryParse(tbPastTillDate.Text, out date))
                    return date;
                else
                    return DateTime.Now;
            }
            set { tbPastTillDate.Text = value.Date.ToString().Substring(0, value.Date.ToString().IndexOf(' ')); }
        }

        private DateTime ContValidFrom
        {
            get { return DateTime.Parse(tbContValidFrom.Text); }
            set { tbContValidFrom.Text = value.Date.ToString().Substring(0, value.Date.ToString().IndexOf(' ')); }
        }

        private DateTime ContValidTill
        {
            get { return DateTime.Parse(tbContValidTill.Text); }
            set { tbContValidTill.Text = value.Date.ToString().Substring(0, value.Date.ToString().IndexOf(' ')); }
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

        public int Ext
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

        public DateTime SalesDate
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
            get { return discount != 0 ? Qty * price : tbPaySum.Text.ToDecimal(); }
            set { tbPaySum.Text = value.ToString(); }
        }

        public decimal TotalSum
        {
            get { return tbTotalSum.Text.ToDecimal(); }
            set { tbTotalSum.Text = value.ToString(); }
        }

        public decimal RecipeValid
        {
            get { return tbRecipeValid.Text.ToDecimal(); }
            set { tbRecipeValid.Text = value.ToString(); }
        }

        public decimal PrepCompSum { get; set; }
        public decimal PrepCompSumTotal
        {
            get { return tbPrepCompSum.Text.ToDecimal(); }
            set { tbPrepCompSum.Text = value.ToString(); }
        }

        private bool lowIncomeTag;
        public bool LowIncomeTag
        {
            get
            {
                return lowIncomeTag;
            }
            set
            {
                lowIncomeTag = value;
                lblIsPrepComp.Visible = lowIncomeTag;
            }
        }


        private bool _eligibleSurcharge;
        public bool EligibleSurcharge
        {
            get
            {
                return _eligibleSurcharge;
            }
            set
            {
                _eligibleSurcharge = value;
                lblIsPrepComp.Visible = _eligibleSurcharge;
            }
        }

        public bool AllowEligibleSurcharge 
        {
            get { return _eligibleSurcharge && in_erecipe?.eRecipe?.Type != "mpp"; }
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
                tbKVPDoctorNo.ReadOnly = value;
            }
        }

        private bool IsOverLimitQty
        {
            get
            {
                return cbOverLimitQty.Checked;
            }
            set
            {
                cbOverLimitQty.Checked = value;
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
            if (tb?.Name != "tbDoses")
                Doses = Math.Round(hd_qty2 * Qty);
            if (tb?.Name == "tbCountDay" && CountDay > 0)
            {
                QtyDay = Math.Truncate(1000 * Doses / CountDay) / 1000;
                HiddenQtyDay = (1000 * Doses / CountDay) / 1000;
                _previousQtyDay = QtyDay;
            }
            if (QtyDay > 0 && tb?.Name != "tbCountDay")
            {
                CountDay = (int)Math.Round(Doses / (_previousQtyDay != QtyDay ? QtyDay : HiddenQtyDay));
            }
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
            var prepSum = Math.Round(TotalSum - CompSum, 2);

            var isCheapest = CheckCheapest(tlkId);
            var hasLowIncome = LowIncomeTag;
            var hasEligibleSurchargeAndCheapest = AllowEligibleSurcharge && CheckCheapest(tlkId);
            var hasEligibleSurchargeAndProducedDrug = AllowEligibleSurcharge && hd_productId == ProducedCompensatedDrugId;

            if (hasLowIncome || hasEligibleSurchargeAndCheapest || hasEligibleSurchargeAndProducedDrug)
                PrepCompSumTotal = prepSum;
            else
                PrepCompSumTotal = 0;
            PaySum = Math.Round(prepSum - PrepCompSumTotal, 2);

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
                    if ((DateTime.Now.AddDays(1) - in_erecipe.eRecipe_ValidFrom).TotalDays > 30 && 
                        !in_erecipe.eRecipe_PrescriptionTagsByDemandTag && 
                        !in_erecipe.eRecipe_PrescriptionTagsLabelingExemptionTag && 
                        !in_erecipe.eRecipe_PrescriptionTagsNominalTag && in_erecipe.DispenseCount == 0)
                        throw new Exception("Nuo vaisto įsigaliojimo praėjo daugiau nei 30 dienų ir nebuvo atliktas nei vienas išdavimas. Išdavimas negalimas.", new Exception(Enumerator.alert.error.ToString()));
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
                CheckValues(this);
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

        internal override void tbMoney_KeyPress(object sender, KeyPressEventArgs e)
        {
            base.tbMoney_KeyPress(sender, e);
        }
        #endregion

        private void tbPersonalCode_TextChanged(object sender, EventArgs e)
        {
            if (!E_Recipe)
                LowIncomeTag = CheckIfAgeIsOver75FromPersonalCode(tbPersonalCode.Text);
        }
    }
}
