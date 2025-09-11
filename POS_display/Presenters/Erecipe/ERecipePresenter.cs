using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Linq;
using POS_display.Models.HomeMode;
using POS_display.Presenters.Erecipe;
using POS_display.Properties;
using POS_display.Repository.HomeMode;
using POS_display.Repository.Pos;
using POS_display.Repository.Recipe;
using POS_display.Utils.Logging;
using POS_display.Views.ERecipe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tamroutilities.Client;
using TamroUtilities.HL7.Models;
using TamroUtilities.HL7.Models.AccumulatedSurcharge;
using TamroUtilities.HL7.Models.Encounter;
using static POS_display.Enumerator;

namespace POS_display.Presenters.ERecipe
{
    public class ERecipePresenter : IERecipePresenter
    {
        private IERecipe _view;
        private const int AdultAge = 18;
        private readonly IPosRepository _posRepository;
        private readonly IHomeModeRepository _homeModeRepository;
        private readonly IRecipeRepository _recipeRepository;

        public ERecipePresenter(IERecipe view, IPosRepository posRepository, IHomeModeRepository homeModeRepository, IRecipeRepository recipeRepository)
        {
            _view = view;
            _posRepository = posRepository;
            _homeModeRepository = homeModeRepository;
            _recipeRepository = recipeRepository;
        }

        #region Public methods
        public async Task FindPatient()
        {
            var eRecipeItem = new Items.eRecipe.Recipe();
            var patient_dto = await Session.eRecipeUtils.GetPatient<PatientDto>(_view.PersonalCode, _view.PickedForUserId);
            if (string.IsNullOrWhiteSpace(patient_dto?.PatientId))
                return;
            eRecipeItem.Patient = patient_dto;
            eRecipeItem.HasLowIncome = await Session.eRecipeUtils.GetLowIncome<LowIncomeDto>(eRecipeItem.Patient.PersonalCode);
            if (Session.getParam("ERECIPE", "SURCHARGEELIGABLE") == "1")
            {
                eRecipeItem.AccumulatedSurcharge = await Session.eRecipeUtils.GetAccumulatedSurcharge<AccumulatedSurchargeDto>(
                eRecipeItem.Patient.PersonalCode,
                eRecipeItem.Patient.DIK,
                DateTime.Now.ToString("yyyy-MM-dd"));
            }
            _view.eRecipeItem = eRecipeItem;//update view
            _view.PickedForUserId = "";
            var valid_encounter = Session.eRecipeEncounterList.FirstOrDefault(w => w.PatientId == eRecipeItem?.Patient?.PatientId);
            if (valid_encounter == null)
                await CreateEncounter(_view.eRecipeItem.Patient.PatientRef);
            else
                eRecipeItem.Encounter = valid_encounter.EncounterItem;

            if (_view?.eRecipeItem?.Encounter?.EncounterId == "0") 
            {
                Serilogger.GetLogger().Information($"Failed to create encounter. {_view?.eRecipeItem?.Encounter?.EncounterRef}");
            }
        }

        public async Task CreateEncounter(string PatientRef)
        {
            var encounter = await Session.eRecipeUtils.CreateEncounter(_view.eRecipeItem.Patient.PatientId,
                PatientRef,
                Session.PractitionerItem.PractitionerRef,
                Session.PractitionerItem.OrganizationRef,
                string.Empty,
                null,
                EncounterReason.Prescription);
            _view.eRecipeItem.Encounter = encounter;
        }

        public async Task GetAllergies()
        {
            if (_view.eRecipeItem?.Patient?.PatientId != "")
            {
                var allergyIntoleranceDto = await Session.eRecipeUtils.GetAllergies<AllergyIntoleranceDto>(_view.eRecipeItem.Patient.PatientId);
                if (Session.getParam("ERECIPE", "V2") == "1")
                {
                    allergyIntoleranceDto.Allergies = allergyIntoleranceDto.Allergies?.Where(e => e.Status == "active").ToList()
                     ?? new List<Allergy>();
                }
                _view.Allergies = allergyIntoleranceDto;
            }
        }

        public async Task GetRepresentedPersons()
        {
            if (_view.eRecipeItem?.Patient?.PatientId != "")
                _view.RepresentedPersons = await Session.eRecipeUtils.GetRepresentedPersons<RepresentedPersonsDto>(_view.eRecipeItem.Patient.PatientId);
        }

        public async Task GetRecipeList(int gridSize, int PageIndex, string type)
        {
             _view.RecipeList = AppylyRecipeFilter(await Session.eRecipeUtils.GetRecipeList<RecipeListDto>(_view.eRecipeItem.Patient.PatientId, gridSize, PageIndex, type));
        }

        public async Task GetRecipeDispenseCount()
        {
            var dispense_count = await Session.eRecipeUtils.GetDispenseList<DispenseListDto>(_view.eRecipeItem.eRecipe.PatientId,
                "", "", "completed", "", "", 100, 1, _view.eRecipeItem.eRecipe.MedicationPrescriptionId.ToString(), new List<string>());
            _view.eRecipeItem.DispenseList = dispense_count;
            _view.eRecipeItem.DispenseCount = dispense_count?.Total ?? 0;
            _view.eRecipeItem.DispensedQty = dispense_count.DispenseList?.Sum(d => d.QuantityValue.ToDecimal()) ?? 0;
            _view.eRecipeItem.PastValidTo = helpers.getDateTimeXML(dispense_count.DispenseList?.FirstOrDefault()?.DueDate ?? "");
            _view.eRecipeItem.CompositionId = dispense_count.DispenseList?.FirstOrDefault()?.CompositionId.ToDecimal() ?? 0;
            if (_view.eRecipeItem.eRecipe_PrescriptionTagsLongTag)
                _view.eRecipeItem.ValidationPeriod = (int)(_view.eRecipeItem.eRecipe_ValidTo.AddDays(1) - _view.eRecipeItem.eRecipe_ValidFrom).TotalDays / _view.eRecipeItem.eRecipe_NumberOfRepeatsAllowed;
            else
                _view.eRecipeItem.ValidationPeriod = (int)(_view.eRecipeItem.eRecipe_ValidTo - _view.eRecipeItem.eRecipe_ValidFrom).TotalDays + 1;
        }

        public async Task GetDispenseCount()
        {
            var dispense_count = await Session.eRecipeUtils.GetDispenseList<DispenseListDto>("", "", "", "completed", "", "", 100, 1, _view.dispenseItem.eRecipe.MedicationPrescriptionId.ToString(), new List<string>());
            _view.dispenseItem.DispenseCount = dispense_count?.Total ?? 0;
            _view.dispenseItem.DispensedQty = dispense_count.DispenseList?.Sum(d => d.QuantityValue.ToDecimal()) ?? 0;
            _view.dispenseItem.PastValidTo = helpers.getDateTimeXML(dispense_count.DispenseList?.FirstOrDefault()?.DueDate ?? "");
            _view.dispenseItem.CompositionId = dispense_count.DispenseList?.FirstOrDefault()?.CompositionId.ToDecimal() ?? 0;
            if (_view.dispenseItem.eRecipe_PrescriptionTagsLongTag)
                _view.dispenseItem.ValidationPeriod = (int)(_view.dispenseItem.eRecipe_ValidTo.AddDays(1) - _view.dispenseItem.eRecipe_ValidFrom).TotalDays / _view.dispenseItem.eRecipe_NumberOfRepeatsAllowed;
            else
                _view.dispenseItem.ValidationPeriod = (int)(_view.dispenseItem.eRecipe_ValidTo - _view.dispenseItem.eRecipe_ValidFrom).TotalDays + 1;
        }

        public async Task<bool> IsPossibleApplyUkrainianReffugeInsurance()
        {
            if (Session.getParam("SAM", "ON") == "0")
                return false;

            string esiNr = _view?.eRecipeItem?.Patient?.ESI;

            if (string.IsNullOrEmpty(esiNr))
            {
                Serilogger.GetLogger().Information($"IsPossibleApplyUkrainianReffugeInsurance: ESI number is empty");
                return false;
            }

            string encryptedEsiNr = helpers.Encrypt(esiNr,Settings.Default.SecretKey);
            ESIIdentityDto eSIIdentityDto = await Session.eRecipeUtils.CheckESI<ESIIdentityDto>(esiNr);

            Serilogger.GetLogger().Information($"IsPossibleApplyUkrainianReffugeInsurance: {encryptedEsiNr};" +
                $" ESIIdentity: {eSIIdentityDto?.ToJsonString()};" +
                $" HasLowIncome: {_view?.eRecipeItem?.HasLowIncome?.ToJsonString()}");

            if (eSIIdentityDto == null || eSIIdentityDto.Exists == 0)
                return false;

            if (_view?.eRecipeItem?.HasLowIncome?.PatientInsured ?? false)
                return false;

            Serilogger.GetLogger().Information($"IsPossibleApplyUkrainianReffugeInsurance: {encryptedEsiNr} can be applied insurance");
            return true;
        }

        public async Task<string> GetBarcodeByProductId(decimal productId) 
        {
           return await _posRepository.GetBarcodeByProductId(productId);
        }

        public async Task CreateHomeDeliveryDetail(decimal posHeaderId, decimal posDetailId, HomeModeQuantities quantities)
        {
            await _homeModeRepository.CreateHomeDeliveryDetail(posHeaderId, posDetailId, quantities);
        }

        public async Task DeleteHomeDeliveryDetail(decimal posHeaderId, decimal posDetailId)
        {
            await _homeModeRepository.DeleteHomeDeliveryDetail(posHeaderId, posDetailId);
        }

        public async Task<bool> IsDiagnosisValidByNpakid7(decimal npakid7)
        {
            try
            {
                var tamroClient = Program.ServiceProvider.GetRequiredService<ITamroClient>();
                var diseaseCode = _view?.eRecipeItem?.DiagnosisCode ?? string.Empty;
                var response = await tamroClient.GetAsync<dynamic>
                    (string.Format(Session.CKasV1GetDiseases, npakid7, diseaseCode));

                var isValid = false;
                JArray jsonArray = JArray.Parse(response.ToString());
                if (jsonArray == null || !jsonArray.Any())
                    isValid = false;
                else
                    isValid = true;

                Serilogger.GetLogger().Information($"[IsDiagnosisValidByNpakid7]" +
                    $" Pharmacy: {Session.SystemData.kas_client_id};" +
                    $" Recipe No: {_view?.eRecipeItem?.eRecipe?.RecipeNumber};" +
                    $" Npkaid7: {npakid7};" +
                    $" Disease code: {diseaseCode};" +
                    $" IsValid: {isValid}");

                return isValid;
            }
            catch (Exception ex)
            {
                Serilogger.GetLogger().Information($"[IsDiagnosisValidByNpakid7] Pharmacy: {Session.SystemData.kas_client_id}; Error: {ex.Message}");
                return false;
            }
        }
        #endregion

        #region Private methods
        private RecipeListDto AppylyRecipeFilter(RecipeListDto RecipeListDto) 
        {
            if (FilterValues.Count == 0)
                return RecipeListDto;

            foreach (var filterValue in FilterValues)
            {
                switch (filterValue.Key)
                {
                    case RecipeFilterValue.ActiveSubstance:
                        RecipeListDto.RecipeList = RecipeListDto.RecipeList
                            .Where(r => r.GenericName.Equals(filterValue.Value, StringComparison.OrdinalIgnoreCase)).ToList();
                        break;
                }
            }

            return new RecipeListDto
            {
                Total = RecipeListDto.RecipeList.Count,
                PageSize = 100, 
                Page = 1,
                RecipeList = RecipeListDto.RecipeList
            };
        }
        #endregion

        #region Properties
        public Dictionary<RecipeFilterValue, string> FilterValues = new Dictionary<RecipeFilterValue, string>();
        #endregion
    }
}
