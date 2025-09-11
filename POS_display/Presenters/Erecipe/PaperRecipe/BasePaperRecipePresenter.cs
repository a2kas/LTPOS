using Hl7.Fhir.Model;
using POS_display.Exceptions;
using POS_display.Models.Recipe;
using POS_display.Repository.Barcode;
using POS_display.Repository.Recipe;
using POS_display.Utils;
using POS_display.Utils.EHealth;
using POS_display.Views.Erecipe.PaperRecipe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using System.Windows.Forms;
using TamroUtilities.HL7.Models;

namespace POS_display.Presenters.Erecipe.PaperRecipe
{
    public class BasePaperRecipePresenter : IBasePaperRecipePresenter
    {
        #region Members
        private readonly IPaperRecipeBaseView _view;
        private readonly IRecipeRepository _recipeRepository;
        protected readonly IKVAP KVAPService;
        protected readonly IEHealthUtils eHealthUtils;
        protected CreateDispenseRequest CreateRequest;

        private PractitionerDto _recipeCreationAuthor;
        private OrganizationDto _recipeCreationOrganization;
        private MedicationDto _dispensedMedication;
        private readonly Dictionary<string, bool> FormCompensationMap = new Dictionary<string, bool>
        {
            { "f1",   false },
            { "f1v",  false },
            { "f2",   false },
            { "f2b",  false },
            { "f3",   true },
            { "f3b",  false },
            { "f3e",  true },
            { "f3v",  true }
        };
        private const decimal ProducedCompensatedDrugId = 10000117369;
        #endregion

        #region Constructor
        public BasePaperRecipePresenter(IPaperRecipeBaseView view,
            IKVAP kvapService,
            IEHealthUtils eHealthUtils,
            IRecipeRepository recipeRepository)
        {
            _view = view ?? throw new ArgumentNullException();
            this.KVAPService = kvapService ?? throw new ArgumentNullException();
            this.eHealthUtils = eHealthUtils ?? throw new ArgumentNullException();
            _recipeRepository = recipeRepository ?? throw new ArgumentNullException();
        }
        #endregion

        #region Properties
        public string FormCode { get; set; }
        public string FormDisplay { get; set; }
        #endregion

        #region Public methods
        public async Task<PaperRecipeRequestResponse> Save() 
        {
            BindData();

            var eHealthResponse = await eHealthUtils.CreatePaperRecipeDispense<RecipeDispenseDto>(CreateRequest);

            if (eHealthResponse.Issues.Any())
                throw new RecipeException(string.Join("\n", eHealthResponse.Issues.Select(e => e.Display)));

            var confirmDispense = Session.PractitionerItem.Roles.First().Code == "6" ||
                                  Session.PractitionerItem.Roles.First().Code == "7" ? true : false;
            var recipeId = 0l;
            var currentDate = DateTime.Now;
            bool isCompensated = FormCompensationMap[FormCode];
            var hasLowIncome = _view.eRecipeItem?.HasLowIncome?.HasLowIncome ?? false;
            var isCheapest = CheckCheapest(_view.MedicationItem.NpakId7.ToString());
            var hasEligibleSurchargeAndCheapest = _view.eRecipeItem.AllowSurchargeEligible && isCheapest;
            var hasEligibleSurchargeAndProducedDrug = _view.eRecipeItem.AllowSurchargeEligible && 
                _view.PosDetail.productid == ProducedCompensatedDrugId;

            if (isCompensated)
            {
                var compensation = await _recipeRepository.GetCompensationByCode(CreateRequest.PaperPrescriptionData.CompensationCodeCode);
                var recipeData = await _recipeRepository.GetNewRecipeData(_view.PosDetail.id, DateTime.Now, _view.PosDetail.barcode, compensation.Percent.ToString());
                var barcodeRatio = recipeData.BarcodeRatio;
                var productRatio = recipeData.ProductRatio;
                var basicPrice = recipeData.BasicPrice;
                var quantity = _view.DipsenseDosesAmount.Text.ToDecimal();
                var pureQuantity = quantity * barcodeRatio / productRatio;

                var totalSum = CalculateTotalSum(_view.Price.Text.ToDecimal(), quantity, barcodeRatio, productRatio, pureQuantity);
                var compSum = CalculateCompensationSum(compensation.Percent.ToString(), basicPrice, quantity, _view.MedicationItem.NpakId7.ToString(), barcodeRatio, productRatio, pureQuantity, totalSum, recipeData);
                var prepSum = Math.Round(totalSum - compSum, 2);

                var prepCompSumTotal = CalculatePrepaymentCompensationSum(hasLowIncome, hasEligibleSurchargeAndCheapest, hasEligibleSurchargeAndProducedDrug, prepSum);
                var paySum = Math.Round(prepSum - prepCompSumTotal, 2);

                recipeId = await _recipeRepository.CreateRecipe(new CreateRecipeModel()
                {
                    DbCode = Session.SystemData.code,
                    TlkId = _view.MedicationItem.NpakId7.ToString(),
                    BarcodeId = _view.PosDetail.barcodeid,
                    RecSer = "",
                    RecipeNo = eHealthResponse.CompositionId.ToDecimal(),
                    PersCode = "",
                    ClinicId = "0",
                    DeseaseCode = CreateRequest.PaperPrescriptionData.ConditionCodeDisplay,
                    DoctorId = "0",
                    RecipeDate = currentDate,
                    SalesPrice = _view.Price.Text.ToDecimal(),
                    BasicPrice = basicPrice,
                    CompensationId = compensation.Id,
                    Qty = quantity,
                    TotalSum = totalSum,
                    CompSum = compSum,
                    PaySum = prepSum,
                    SalesDate = _view.SalesDate.Value,
                    GQty = pureQuantity,
                    Water = 0m,
                    TaxoLaborum = 0m,
                    Ext = 0,
                    CheckDate = _view.SalesDate.Value,
                    CheckNo = Program.Display1.PoshItem.CheckNo.ToDecimal(),
                    QtyDay = _view.AmountPerDay.Text.ToDecimal(),
                    CountDay = _view.AmountOfDays.Text.ToDecimal(),
                    TillDate = _view.EnoughUntil.Value,
                    KvpDoctorNo = _view.DoctorCode.Text,
                    AagaIsas = CreateRequest.PaperPrescriptionData.AagaSgasNumber,
                    ValidFrom = _view.ExpiryStart.Value,
                    ValidTill = _view.ExpiryEnd.Value,
                    StoreId = Session.SystemData.storeid
                });

                if (_view.eRecipeItem?.AccumulatedSurcharge != null)
                {
                    await _recipeRepository.SetAccumulatedSurchargeData(
                        recipeId,
                        _view.eRecipeItem.AccumulatedSurcharge.SurchargeEligible,
                        _view.eRecipeItem.AccumulatedSurcharge.SurchargeAmount,
                        _view.eRecipeItem.AccumulatedSurcharge.MissingSurchargeAmount,
                        _view.eRecipeItem.AccumulatedSurcharge.ValidTo ?? DateTime.Now);
                }

                await _recipeRepository.UpdateRecipeCompensationData(
                    recipeId,
                    prepCompSumTotal,
                    true,
                    hasLowIncome,
                    hasLowIncome || hasEligibleSurchargeAndCheapest || hasEligibleSurchargeAndProducedDrug);

                await _recipeRepository.UpdatePosDetailByRecipe(new PosDetailUpdateByRecipeModel() 
                {
                    PosdId = _view.PosDetail.id,
                    BarcodeId = _view.PosDetail.barcodeid,
                    Qty = quantity,
                    NewPaySum = paySum / quantity,
                    NewHId = recipeId,
                    PaySum = paySum
                });
            }

            var erecipeId = await _recipeRepository.CreateErecipe(new CreateErecipeModel()
            {
                PoshId = _view.PosDetail.hid,
                PosdId = _view.PosDetail.id,
                ProductId = _view.PosDetail.productid,
                UserId = Session.User.id,
                RecipeNo = eHealthResponse.CompositionId.ToDecimal(),
                EncounterId = _view.eRecipeItem.Encounter.EncounterId.ToDecimal(),
                RecipeId = recipeId,
                RecipeDate = currentDate,
                SalesDate = _view.SalesDate.Value,
                TillDate = _view.EnoughUntil.Value
            });

            if (!string.IsNullOrEmpty(CreateRequest.PaperPrescriptionData.ConditionCodeDisplay))
                await _recipeRepository.SetERecipeDiseaseCode(erecipeId, CreateRequest.PaperPrescriptionData.ConditionCodeDisplay);

            await _recipeRepository.UpdateERecipe(new UpdateErecipeModel
            {
                Id = erecipeId,
                CompositionId = eHealthResponse.CompositionId.ToDecimal(),
                CompositionRef = eHealthResponse.CompositionRef,
                CompositionStatus = "final",
                MedicationDispenseId = eHealthResponse.MedicationDispenseId.ToDecimal(),
                MedicationDispenseStatus = "completed",
                Active = 1,
                Confirmed = confirmDispense ? 1 : 0,
                DocumentStatus = "final",
                Info = ""
            });

            return new PaperRecipeRequestResponse()
            {
                CompositionId = eHealthResponse.CompositionId
            };
        }

        public virtual void BindData() 
        {
            CreateRequest = new CreateDispenseRequest
            {
                PaperPrescriptionData = new PaperPrescriptionDto()
            };

            var selectedPharmaceuticalFormMeasureUnit = (KeyValuePair<string, string>)((BindingSource)_view.PharmaceuticalFormMeasureUnit.DataSource).Current;
            var selectedRoute = (KeyValuePair<string, string>)((BindingSource)_view.Route.DataSource).Current;
            var selectedTimesPerSelection = _view.TimesPerSelection.SelectedItem as string;

            CreateRequest.DataModelVersionValue = "http://esveikata.lt/Profile/MedicationDispense/v29";
            CreateRequest.PractitionerId = Session.PractitionerItem.PractitionerId.ToLong();
            CreateRequest.PractitionerRef = Session.PractitionerItem.PractitionerRef;
            CreateRequest.Qualification = Session.PractitionerItem.Qualification;
            CreateRequest.PatientRef = _view.eRecipeItem.Patient.PatientRef;
            CreateRequest.PickedUpByRef = _view.eRecipeItem.Patient.PatientRef;
            CreateRequest.ConfirmationPractitionerRef = Session.PractitionerItem.PractitionerRef;
            CreateRequest.OrganizationRef = Session.OrganizationItem.OrganizationRef;
            CreateRequest.EncounterRef = _view.eRecipeItem.Encounter.EncounterRef;
            CreateRequest.ConfirmDispense = true;
            CreateRequest.PrescriptionCompositionId = string.Empty;
            CreateRequest.MedicationPrescriptionId = string.Empty;            
            CreateRequest.PrescriptionStatusCode = "completed";
            CreateRequest.CompensationTag = true;
            CreateRequest.DispenseDueDate = _view.EnoughUntil.Value;
            CreateRequest.PriceRetail = new CurrencyDto() { Value = _view.Price.Text.ToDecimal() };
            CreateRequest.PricePaid = new CurrencyDto() { Value = 0 };
            CreateRequest.PriceCompensated = new CurrencyDto() { Value = 0 };
            CreateRequest.Quantity = new QuantityDto() 
            {
                Value = _view.DipsenseDosesAmount.Text.ToDecimal(),
                Units = "Vienetas",
                Code = "vnt.",
            };
            CreateRequest.MedicationRef = _dispensedMedication?.MedicationRef ?? string.Empty;
            CreateRequest.DispenseDate = _view.SalesDate.Value;
            CreateRequest.AdditionalInstructionsForPatient = _view.eRecipeItem.AdditionalInstructionsForPatient;
            CreateRequest.LowIncomeTag = _view.eRecipeItem.HasLowIncome?.HasLowIncome ?? false;
            CreateRequest.LowIncomeSurchargeCompensated = new CurrencyDto() { Value = 0 };
            CreateRequest.IsLowestPriceTag = _view.MedicationItem.IsCheapest == 1;
            CreateRequest.PrescriptionType = "pp";

            CreateRequest.PaperPrescriptionData.NPNumber = string.Empty;
            CreateRequest.PaperPrescriptionData.NPLotNumber = string.Empty;
            CreateRequest.PaperPrescriptionData.CompNumber = string.Empty;
            CreateRequest.PaperPrescriptionData.CompLotNumber = string.Empty;

            CreateRequest.PaperPrescriptionData.TypeCode = _dispensedMedication?.MedicationGroup == "Vaistinis preparatas" ? "va" : "mpp";
            CreateRequest.PaperPrescriptionData.TypeDisplay = _dispensedMedication?.MedicationGroup == "Vaistinis preparatas" ? "Vaistas" : "Medicininės pagalbos priemonės";

            CreateRequest.PaperPrescriptionData.FormCode = FormCode;
            CreateRequest.PaperPrescriptionData.FormDisplay = FormDisplay;

            CreateRequest.PaperPrescriptionData.Date = _view.SalesDate.Value;
            CreateRequest.PaperPrescriptionData.ValidityPeriodStart = _view.ExpiryStart.Value;
            CreateRequest.PaperPrescriptionData.ValidityPeriodEnd = _view.ExpiryEnd.Value;

            CreateRequest.PaperPrescriptionData.Series = string.Empty;
            CreateRequest.PaperPrescriptionData.Number = string.Empty;
            CreateRequest.PaperPrescriptionData.Form2Tag = false;
            CreateRequest.PaperPrescriptionData.Form2Series = string.Empty;
            CreateRequest.PaperPrescriptionData.Form2Number = string.Empty;

            CreateRequest.PaperPrescriptionData.ConditionCodeSystem = string.Empty;
            CreateRequest.PaperPrescriptionData.ConditionCodeCode = string.Empty;
            CreateRequest.PaperPrescriptionData.ConditionCodeDisplay = string.Empty;

            CreateRequest.PaperPrescriptionData.CompensationCodeSystem = string.Empty;
            CreateRequest.PaperPrescriptionData.CompensationCodeCode = string.Empty;
            CreateRequest.PaperPrescriptionData.CompensationCodeDisplay = string.Empty;

            CreateRequest.PaperPrescriptionData.AagaSgasNumber = string.Empty;
            CreateRequest.PaperPrescriptionData.AuthorLabelNumber = _view.DoctorCode.Text;
            CreateRequest.PaperPrescriptionData.AuthorRef = Session.PractitionerItem.PractitionerRef;
            CreateRequest.PaperPrescriptionData.AuthorDepartmentRef = Session.OrganizationItem.OrganizationRef;
            CreateRequest.PaperPrescriptionData.DispenserNote = _view.InfoForSpecialist.Text;

            CreateRequest.PaperPrescriptionData.Quantity = new QuantityDto();
            CreateRequest.PaperPrescriptionData.TagsGkTag = _view.GKDecision.Checked;
            CreateRequest.PaperPrescriptionData.TagsSpecialTag = _view.SpecialPrescription.Checked;
            CreateRequest.PaperPrescriptionData.TagsSpecialistDecisionTag = _view.SpecialistDecision.Checked;
            CreateRequest.PaperPrescriptionData.TagsLabelingExemptionTag = _view.LabelingExemption.Checked;
            CreateRequest.PaperPrescriptionData.TagsNominalTag = _view.BrandName.Checked;
            CreateRequest.PaperPrescriptionData.TagsNominalConfirmTag = _view.NominalConfirm.SelectedIndex == 1;
            CreateRequest.PaperPrescriptionData.TagsNominalDeclarationValid = _view.NominalDeclarationValid.Value;

            int.TryParse(_dispensedMedication?.Description, out int unitValue);
            CreateRequest.PaperPrescriptionData.MedicationRef = _dispensedMedication?.MedicationRef ?? string.Empty;
            CreateRequest.PaperPrescriptionData.MedicationActiveSubstances = _dispensedMedication?.GenericName ?? string.Empty;
            CreateRequest.PaperPrescriptionData.MedicationName = _view.CertainName.Checked ? 
                _dispensedMedication?.ProprietaryName ?? string.Empty : _view.MedicationItem.ShortName;
            CreateRequest.PaperPrescriptionData.MedicationStrength = _dispensedMedication?.Strength ?? string.Empty;
            CreateRequest.PaperPrescriptionData.MedicationFormSystem = "http://esveikata.lt/classifiers/PharmaceuticalForm";
            CreateRequest.PaperPrescriptionData.MedicationFormCode = _dispensedMedication?.PharmaceuticalFormCode ?? string.Empty;
            CreateRequest.PaperPrescriptionData.MedicationFormDisplay = _dispensedMedication?.PharmaceuticalForm ?? string.Empty;
            CreateRequest.PaperPrescriptionData.MedicationPackageName = _dispensedMedication?.Description ?? string.Empty;
            CreateRequest.PaperPrescriptionData.MedicationPackageSize = new QuantityDto
            {
                Value = unitValue,
                Units = selectedPharmaceuticalFormMeasureUnit.Value,
                Code = selectedPharmaceuticalFormMeasureUnit.Value,
            };

            CreateRequest.PaperPrescriptionData.ExtemporaneousDescription = string.Empty;
            CreateRequest.PaperPrescriptionData.ExtemporaneousUrgencyTag = string.Empty;

            CreateRequest.PaperPrescriptionData.MedicationInstructionAdministrationMethod = string.Empty;
            CreateRequest.PaperPrescriptionData.MedicationInstructionDosePerDayQuantity = new QuantityDto();
            CreateRequest.PaperPrescriptionData.MedicationInstructionDosePerDayText = string.Empty;
            CreateRequest.PaperPrescriptionData.MedicationInstructionTimeQuantity = string.Empty;

            CreateRequest.PaperPrescriptionData.UsageTagsMorning = _view.InMorning.Checked;
            CreateRequest.PaperPrescriptionData.UsageTagsNoon = _view.DuringLunch.Checked;
            CreateRequest.PaperPrescriptionData.UsageTagsEvening = _view.InEvening.Checked;
            CreateRequest.PaperPrescriptionData.UsageTagsBeforeMeal = _view.BeforeEating.Checked;
            CreateRequest.PaperPrescriptionData.UsageTagsDuringMeal = _view.DuringEating.Checked;
            CreateRequest.PaperPrescriptionData.UsageTagsAfterMeal = _view.AfterEating.Checked;
            CreateRequest.PaperPrescriptionData.UsageTagsIndependMeal = _view.RegardlessEating.Checked;
            CreateRequest.PaperPrescriptionData.UsageTagsAsNeeded = _view.AsNeeded.Checked;
            CreateRequest.PaperPrescriptionData.UsageTagsBeforeSleep = _view.BeforeSleeping.Checked;

            CreateRequest.PaperPrescriptionData.MedicationInstructionTimingScheduleFrequency = _view.TimesPer.Text;
            CreateRequest.PaperPrescriptionData.MedicationInstructionTimingScheduleDuration = "1";
            CreateRequest.PaperPrescriptionData.MedicationInstructionTimingScheduleUnits = GetTimesPerUnitAlias(selectedTimesPerSelection);
            CreateRequest.PaperPrescriptionData.MedicationInstructionRouteSystem = "http://esveikata.lt/classifiers/Route";
            CreateRequest.PaperPrescriptionData.MedicationInstructionRouteCode = selectedRoute.Key;
            CreateRequest.PaperPrescriptionData.MedicationInstructionRouteDisplay = selectedRoute.Value;
            CreateRequest.PaperPrescriptionData.MedicationInstructionDoseQuantity = new QuantityDto()
            {
                Value = !string.IsNullOrWhiteSpace(_view.OneTimeDose.Text) ? _view.OneTimeDose.Text.ToDecimal() : 0,
                Units = selectedPharmaceuticalFormMeasureUnit.Value,
                Code = selectedPharmaceuticalFormMeasureUnit.Key,
            };

            CreateRequest.PaperPrescriptionData.MedicalAidsGroupCode = string.Empty;
            CreateRequest.PaperPrescriptionData.MedicalAidsGroupName = string.Empty;
            CreateRequest.PaperPrescriptionData.MedicalAidsName = string.Empty;
        }

        public virtual void Init() 
        {

            Dictionary<string, string> pharmaceuticalFormMeasureUnitDictionary = Session.PharmaceuticalFormMeasureUnitClassifiers.Any() ?
                Session.PharmaceuticalFormMeasureUnitClassifiers
                .GroupBy(rc => rc.DisplayCode)
                .ToDictionary(
                    g => g.Key,
                    g => g.Last().DisplayName
                ) : new Dictionary<string, string>();

            Dictionary<string, string> routeDictionary = Session.RouteClassifiers
                .Where(rc => rc.Names.Any(n => n.Language == "lt"))
                .ToDictionary(
                    rc => rc.Code,
                    rc => rc.Names.First(n => n.Language == "lt").Name
                );

            
            _view.PharmaceuticalForm.DataSource = new BindingSource(pharmaceuticalFormMeasureUnitDictionary.OrderBy(e=>e.Value), null);
            _view.PharmaceuticalForm.DisplayMember = "Value";
            _view.PharmaceuticalForm.ValueMember = "Key";
            _view.PharmaceuticalForm.SelectedIndex = FindIndexByKey(_view.PharmaceuticalForm, "vnt.");

            _view.PharmaceuticalFormMeasureUnit.DataSource = new BindingSource(pharmaceuticalFormMeasureUnitDictionary.OrderBy(e => e.Value), null);
            _view.PharmaceuticalFormMeasureUnit.DisplayMember = "Value";
            _view.PharmaceuticalFormMeasureUnit.ValueMember = "Key";
            _view.PharmaceuticalFormMeasureUnit.SelectedIndex = FindIndexByKey(_view.PharmaceuticalFormMeasureUnit, "doz.");

            _view.Route.DataSource = new BindingSource(routeDictionary.OrderBy(e => e.Value), null);
            _view.Route.DisplayMember = "Value";
            _view.Route.ValueMember = "Key";
            _view.Route.SelectedIndex = FindIndexByKey(_view.Route, "25");

            _view.CreationDate.Value = DateTime.Now; 
            _view.ExpiryStart.Value = DateTime.Now;
            _view.ExpiryEnd.Value = _view.ExpiryStart.Value.AddDays(30);
            _view.ValidityPeriod.Text = _view.ExpiryEnd.Value.Subtract(_view.ExpiryStart.Value).Days.ToString();

            _view.TimesPerSelection.SelectedIndex = 0;

            _view.Medication.Text = _view.MedicationItem?.ShortName ?? string.Empty;
            _view.DispensedMedication.Text = _view.MedicationItem?.ShortName ?? string.Empty;
            _view.DipsenseDosesAmount.Text = _view.MedicationItem?.BarcodeRatio.ToString() ?? string.Empty;
            _view.Price.Text = _view.MedicationItem?.RetailPrice.ToString(System.Globalization.CultureInfo.InvariantCulture) ?? string.Empty;
            _view.PrescriptionDosesAmount.Text = _view.MedicationItem?.Qty.ToString() ?? string.Empty;

            var medicationList = eHealthUtils.GetMedicationList<MedicationListDto>("NPAKID7", _view.MedicationItem?.NpakId7.ToString())?.MedicationList;
            if (medicationList?.Count > 0)
                _dispensedMedication = medicationList.First();
        }

        public void RetriveDoctorDataByDoctorCode(string SPSTPL) 
        {
            _view.Stamp.Text = string.Empty;
            _view.SveidraID.Text = string.Empty;

            if (string.IsNullOrWhiteSpace(SPSTPL))
                throw new RecipeException("Blogai nurodytas gydytojo kodas!");

            var SPSTPLDetails = KVAPService.GetSPSTPLDetails(SPSTPL) ??
                throw new RecipeException($"Nepavyko gauti informacijos apie gydytoją pagal {SPSTPL} kodą iš TLK!");

            _recipeCreationAuthor = eHealthUtils.GetPractitioner<PractitionerDto>(SPSTPLDetails.SPAUDO_NUMERIS);

            if (_recipeCreationAuthor == null)
                throw new RecipeException($"Nepavyko gauti informacijos apie gydytoją su '{SPSTPLDetails.SPAUDO_NUMERIS}' Spaudo numeriu iš E-Sveikatos!");

            _recipeCreationOrganization = eHealthUtils.GetOrganization<OrganizationDto>(SPSTPLDetails.SVEIDRA_ID);

            if (_recipeCreationOrganization == null)
                throw new RecipeException($"Nepavyko gauti informacijos apie gydytojo įstaigą su '{SPSTPLDetails.SVEIDRA_ID}' Sveidros numeriu iš E-Sveikatos!");

            _view.Stamp.Text = SPSTPLDetails.SPAUDO_NUMERIS;
            _view.SveidraID.Text = SPSTPLDetails.SVEIDRA_ID;
        }

        public virtual void Validate()
        {
            if (string.IsNullOrWhiteSpace(_view.ValidityPeriod.Text))
                throw new RecipeException("Popierinio recepto duomenys -> 'Galiojimo trukmė' privalo būti nurodytą!");

            if (string.IsNullOrWhiteSpace(_view.PrescriptionDosesAmount.Text))
                throw new RecipeException("Skiriamo vaisto duomenys -> 'Dozuočių skaičius' privalo būti nurodytas!");

            if (string.IsNullOrWhiteSpace(_view.DipsenseDosesAmount.Text))
                throw new RecipeException("Išdavimo informacija -> 'Dozuočių skaičius' privalo būti nurodytas!");

            if (string.IsNullOrWhiteSpace(_view.AmountPerDay.Text))
                throw new RecipeException("Išdavimo informacija -> 'Kiekis d.' privalo būti nurodytas!");

            if (string.IsNullOrWhiteSpace(_view.AmountOfDays.Text))
                throw new RecipeException("Išdavimo informacija -> 'Dienų skaičius' privalo būti nurodytas!");

            if (string.IsNullOrWhiteSpace(_view.Price.Text))
                throw new RecipeException("Išdavimo informacija -> 'Kaina' privalo būti nurodyta!");

        }

        public void ValidateValues() 
        {
            if (_view.ExpiryStart.Value > _view.ExpiryEnd.Value)
            {
                ResetValues();
                throw new RecipeException("Galiojimo pabaiga negali būti ankstesnė nei galiojimo pradžia");
            }
        }

        public void RecalculateValuesByExpiryStart()
        {
            _view.IsFieldValueChanging = true;
            _view.ValidityPeriod.Text = _view.ExpiryEnd.Value.Subtract(_view.ExpiryStart.Value).Days.ToString();
            _view.IsFieldValueChanging = false;
        }

        public void RecalculateValuesByExpiryEnd()
        {
            _view.IsFieldValueChanging = true;
            _view.ValidityPeriod.Text = _view.ExpiryEnd.Value.Subtract(_view.ExpiryStart.Value).Days.ToString();
            _view.IsFieldValueChanging = false;
        }

        public void RecalculateValuesByValidPeriod()
        {
            if (int.TryParse(_view.ValidityPeriod.Text, out int val))
            {
                _view.IsFieldValueChanging = true;
                _view.ExpiryEnd.Value = _view.ExpiryStart.Value.AddDays(val);
                _view.IsFieldValueChanging = false;
            }
        }

        public void RecalculateValuesBySalesDate()
        {
            _view.IsFieldValueChanging = true;
            _view.AmountPerDay.Text = _view.SalesDate.Value.Subtract(_view.EnoughUntil.Value).Days.ToString();
            _view.IsFieldValueChanging = false;
        }

        public void RecalculateValuesByAmountOfDays()
        {
            if (int.TryParse(_view.AmountOfDays.Text, out int val))
            {
                _view.IsFieldValueChanging = true;
                _view.EnoughUntil.Value = _view.SalesDate.Value.AddDays(val);
                _view.IsFieldValueChanging = false;
            }
        }

        public void RecalculateValuesByEnoughUntil()
        {
            _view.IsFieldValueChanging = true;
            _view.AmountPerDay.Text = _view.SalesDate.Value.Subtract(_view.EnoughUntil.Value).Days.ToString();
            _view.IsFieldValueChanging = false;
        }
        #endregion

        #region Private methods
        private void ResetValues() 
        {
            _view.IsFieldValueChanging = true;
            _view.ExpiryStart.Value = DateTime.Now;
            _view.ExpiryEnd.Value = _view.ExpiryStart.Value.AddDays(30);
            _view.ValidityPeriod.Text = _view.ExpiryEnd.Value.Subtract(_view.ExpiryStart.Value).Days.ToString();
            _view.IsFieldValueChanging = false;
        }

        private string GetTimesPerUnitAlias(string value)
        {
            switch (value)
            {
                case "Diena":
                    return Schedule.UnitsOfTime.D.ToString();
                case "Metai":
                    return Schedule.UnitsOfTime.A.ToString();
                case "Mėnuo":
                    return Schedule.UnitsOfTime.Mo.ToString();
                case "Savaitė":
                    return Schedule.UnitsOfTime.Wk.ToString();
                case "Sekundė":
                    return Schedule.UnitsOfTime.S.ToString();
                case "Valanda":
                    return Schedule.UnitsOfTime.H.ToString();
                default:
                    return Schedule.UnitsOfTime.D.ToString();
            }
        }

        private int FindIndexByKey(ComboBox comboBox, string key)
        {
            try
            {
                for (int i = 0; i < comboBox.Items.Count; i++)
                {
                    if (comboBox.Items[i] is KeyValuePair<string, string> item && item.Key == key)
                        return i;
                }
                return 0;
            }
            catch 
            {
                return 0;
            }
        }
        private bool CheckCheapest(string npakid7)
        {
            return
                Session.TLKCheapests.Where(val => val.StartDate <= DateTime.Now && val.Npakid7 == npakid7)
                .OrderByDescending(val => val.PriceListVersion)
                .Select(val => val.IsCheapest)
                .FirstOrDefault();
        }

        private decimal CalculateTotalSum(decimal salesPrice, decimal quantity, decimal barcodeRatio, decimal productRatio, decimal pureQuantity)
        {
            if (barcodeRatio > 0 && productRatio > 0)
                return Math.Round(((salesPrice / (barcodeRatio / productRatio)) * pureQuantity), 2);

            return Math.Round(salesPrice * quantity, 2);
        }

        private decimal CalculateCompensationSum(string compPercent, decimal basicPrice, decimal quantity, string tlkId, decimal barcodeRatio, decimal productRatio, decimal pureQuantity, decimal totalSum, NewRecipeData recipeData)
        {
            var compensationAmount = GetCompensationAmount(compPercent.ToDecimal(), recipeData);
            decimal compSum = compensationAmount;

            if (barcodeRatio > 0)
            {
                decimal c3 = Math.Round(compSum / barcodeRatio * pureQuantity, 3);
                compSum = c3;
            }

            decimal rec_comp_old = 0;
            if (tlkId.StartsWith("M") || tlkId.StartsWith("G"))
                rec_comp_old = quantity * basicPrice;
            else
                rec_comp_old = quantity * compensationAmount;

            decimal compS_old = rec_comp_old;
            if (quantity > 0)
                compS_old = compS_old / quantity;

            decimal sumaminusprimoka = compPercent.ToDecimal() / 100 * basicPrice;

            if (tlkId.StartsWith("M") || tlkId.StartsWith("G") || CheckForSpecialTlkId(tlkId) || compS_old - sumaminusprimoka <= 0.01M)
                compSum = compPercent.ToDecimal() / 100 * basicPrice * quantity;

            if (compSum == 0)
                compSum = compPercent.ToDecimal() / 100 * basicPrice * quantity;

            if (compSum > totalSum)
                compSum = totalSum;

            return Math.Round(compSum, 2, MidpointRounding.AwayFromZero);
        }

        private decimal CalculatePrepaymentCompensationSum(bool hasLowIncome, bool hasEligibleSurchargeAndCheapest, bool hasEligibleSurchargeAndProducedDrug, decimal prepSum)
        {
            if (hasLowIncome || hasEligibleSurchargeAndCheapest || hasEligibleSurchargeAndProducedDrug)
                return prepSum;
            else
                return 0;
        }

        private decimal GetCompensationAmount(decimal compensationPercent, NewRecipeData recipeData)
        {
            switch (compensationPercent)
            {
                case 50:
                    return recipeData.Comp50;
                case 80:
                    return recipeData.Comp80;
                case 90:
                    return recipeData.Comp90;
                case 100:
                    return recipeData.Comp100;
                default:
                    return 0;
            }
        }

        private bool CheckForSpecialTlkId(string tlkId)
        {
            return tlkId == "4342" || tlkId == "3918" || tlkId == "3838" ||
                   tlkId == "3357" || tlkId == "3671" || tlkId == "3672" ||
                   tlkId == "1391" || tlkId == "0487" || tlkId == "2926" ||
                   tlkId == "4889";
        }
        #endregion
    }
}
