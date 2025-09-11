using POS_display.Views;
using POS_display.Views.ERecipe;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TamroUtilities.HL7.Models;
using TamroUtilities.HL7.Models.Encounter;

namespace POS_display.Presenters.ERecipe
{
    public class VaccineCreateUserPresenter : ErrorHandling
    {
        #region Members
        private readonly IBusy _busy;
        private readonly IVaccineCreateUser _view;
        #endregion

        #region Constructor
        public VaccineCreateUserPresenter(IBusy busy, IVaccineCreateUser view)
        {
            _busy = busy;
            _view = view;
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
        public void FindMedicine(string npakid7)
        {          
            ExecuteWithWait(() =>
            {
                MedicationDto medication = null;
                var medication_list = Session.eRecipeUtils.GetMedicationList<MedicationListDto>("NPAKID7", npakid7)?.MedicationList;
                if (medication_list?.Count > 0)
                {
                    medication = medication_list.FirstOrDefault();
                    if (medication != null)
                    {
                        _view.Medicine = medication.ProprietaryName;
                        _view.NPAKID7 = medication.NPAKID7.ToString();
                    }
                }
            });
        }

        public async Task CreateVaccinePescription()
        {
            await ExecuteWithWaitAsync(async () =>
            {
                if (string.IsNullOrEmpty(_view.Medicine))
                    throw new Exception("Imuninis vaistinis prepratas privalo būti nurodytas!");

                if (string.IsNullOrEmpty(_view.DoseNumber))
                    throw new Exception("Dozės numeris privalo būti nurodytas!");

                Session.eRecipeUtils.cts = new CancellationTokenSource();
                var encounter = await Session.eRecipeUtils.CreateEncounter(_view.Patient.PatientId,
                                                                           _view.Patient.PatientRef,
                                                                            Session.ExtendedPracticePractitioner.PractitionerRef,
                                                                            Session.ExtendedPracticePractitioner.OrganizationRef,
                                                                             _view.Patient.ESI,
                                                                             _view.LowIncome?.PatientInsured ?? false,
                                                                            EncounterReason.Vaccination);

                var prescriptionResult = await Session.eRecipeUtils.MakeVaccinationPrescription<VaccineOrderResponseDto>(_view.Patient.PatientRef,
                                                                                                                      encounter?.EncounterRef,
                                                                                                                      _view.InfectiousDiseaseCode,
                                                                                                                      _view.InfectiousDisease,
                                                                                                                      _view.Medicine,
                                                                                                                      _view.NPAKID7,
                                                                                                                      _view.DoseNumber.ToInt(),
                                                                                                                      _view.Notes);
                if (prescriptionResult != null)
                {
                    string message = $"Skiepų skyrimas sėkmingai atliktas. Skyrimo dok. E025 nr: {prescriptionResult.CompositionId}. Ar norite atlikti skiepų išdavimą?";
                    if (!helpers.alert(Enumerator.alert.info, message, true))
                        _view.RaiseCloseEvent(null, Enumerator.VaccineOrderEvent.Created);
                    else
                    {
                        Models.Recipe.VaccineDispenseArgs args = new Models.Recipe.VaccineDispenseArgs()
                        {
                            CompositionId = prescriptionResult.CompositionId,
                            BarcodeModel = _view.BarcodeModel,
                            DoseNumber = _view.DoseNumber
                        };
                        _view.RaiseCloseEvent(args, Enumerator.VaccineOrderEvent.CreatedToDispense);
                    }
                }
            });
        }
        #endregion
    }
}
