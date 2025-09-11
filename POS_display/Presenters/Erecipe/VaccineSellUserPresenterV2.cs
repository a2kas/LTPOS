using POS_display.Repository.Recipe;
using POS_display.Views;
using POS_display.Views.ERecipe;
using System;
using System.Threading;
using System.Threading.Tasks;
using TamroUtilities.HL7.Models;
using TamroUtilities.HL7.Models.Encounter;

namespace POS_display.Presenters.ERecipe
{
    public class VaccineSellUserPresenterV2 : ErrorHandling
    {
        #region Members
        private readonly IBusy _busy;
        private readonly IVaccineSellUserV2 _view;
        private readonly IRecipeRepository _recipeRepository = null;
        #endregion

        #region Constructor
        public VaccineSellUserPresenterV2(IBusy busy, IVaccineSellUserV2 view, IRecipeRepository recipeRepository)
        {
            _busy = busy;
            _view = view;
            _recipeRepository = recipeRepository;
        }
        #endregion

        #region Override
        public override bool IsBusy
        {
            get => base.IsBusy;
            set
            {
                _busy.IsBusy = value;
                base.IsBusy = value;
            }
        }
        #endregion

        #region Public methods
        public async Task CreateVaccineDispensation(Items.posd posd)
        {
            await ExecuteWithWaitAsync(async () =>
            {
                var encounter = await Session.eRecipeUtils.CreateEncounter(_view.Patient.PatientId,
                                                           _view.Patient.PatientRef,
                                                            Session.ExtendedPracticePractitioner.PractitionerRef,
                                                            Session.ExtendedPracticePractitioner.OrganizationRef,
                                                            _view.Patient.ESI,
                                                            _view.LowIncome?.PatientInsured ?? false,
                                                            EncounterReason.Vaccination);

                if (string.IsNullOrEmpty(encounter?.EncounterRef))
                    throw new Exception($"Nepavyko užregistruoti paciento apsilankymo E-Sveikatos sistemoje");

                Session.eRecipeUtils.cts = new CancellationTokenSource();
                VaccineOrderResponseDto dispensationResult =  await Session.eRecipeUtils.MakeVaccinationDispensationV2<VaccineOrderResponseDto> (
                                                                _view.Patient.PatientRef,
                                                                _view.Medication.MedicationRef,
                                                                encounter?.EncounterRef,
                                                                string.Empty,
                                                                _view.InfectiousDiseaseCode,
                                                                _view.InfectiousDisease,
                                                                _view.Medication.ProprietaryName,
                                                                _view.Medication.NPAKID7.ToString(),
                                                                (_view.VaccinationTypeindex + 1).ToString(),
                                                                _view.VaccinationType != null ? _view.VaccinationType.ToString() : string.Empty,
                                                                _view.SerialNumber,
                                                                !string.IsNullOrEmpty(_view.VaccinatedDose) ? int.Parse(_view.VaccinatedDose) : 0,
                                                                _view.VaccineExpiryDate);

                if (dispensationResult != null)
                {
                    helpers.alert(
                        Enumerator.alert.info,
                        $"Skiepų išdavimas sėkmingai atliktas. Išdavimo dok. E063 nr: {dispensationResult.CompositionId}");


                    await _recipeRepository.CreateVaccination(posd.hid.ToLong(),
                                                              posd.id.ToLong(),
                                                              0,
                                                              0,
                                                              string.Empty,
                                                              dispensationResult.CompositionId.ToLong(),
                                                              dispensationResult.CompositionRef,
                                                              _view.Patient.PatientId.ToLong());
                }

                _view.RaiseCloseEvent(null, Enumerator.VaccineOrderEvent.Closed);
            });
        }
        #endregion
    }
}
