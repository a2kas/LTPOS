using POS_display.Models.Recipe;
using POS_display.Views;
using POS_display.Views.ERecipe;
using TamroUtilities.HL7.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace POS_display.Presenters.ERecipe
{
    public class VaccineUserPresenter : ErrorHandling
    {
        private readonly IBusy _busy;
        private readonly IVaccineUser _view;
        private const string CancellationReason = "Atšauktas dokumentas";

        public VaccineUserPresenter(IBusy busy, IVaccineUser view)
        {
            _busy = busy;
            _view = view;
        }

        public async Task FindPatient()
        {
            await ExecuteWithWaitAsync(async () =>
            {
                if (_view.PersonalCode == "")
                    throw new Exception("");

                Clear();
                Session.eRecipeUtils.cts = new CancellationTokenSource();

                _view.Patient = await Session.eRecipeUtils.GetPatient<PatientDto>(_view.PersonalCode, "");
                _view.LowIncome = await Session.eRecipeUtils.GetLowIncome<LowIncomeDto>(_view.PersonalCode);
                HandleControlsActivity();
                SetPatient(_view.Patient);
            });
        }

        public async Task CancelVaccination(VaccinationEntry vaccineEntry)
        {
            await ExecuteWithWaitAsync(async () =>
            {
                if (!helpers.alert(Enumerator.alert.info, GetCancellationRequestMessage(vaccineEntry), true))
                    return;

                string responseMessage = string.Empty;
                VaccineOrderResponseDto dispensationCancelResult = null;
                if (vaccineEntry.Dispensation != null)
                {
                    dispensationCancelResult = await Session.eRecipeUtils.CancelVaccinationDispensation<VaccineOrderResponseDto>(
                        vaccineEntry.Dispensation.CompositionId,
                        CancellationReason);

                    if (dispensationCancelResult == null)
                        throw new Exception("Atšaukimas nepavyko");
                }

                VaccineOrderResponseDto prescriptionCancelResult = await Session.eRecipeUtils.CancelVaccinationPrescription<VaccineOrderResponseDto>(
                    vaccineEntry.Prescription.CompositionId,
                    CancellationReason);

                if (prescriptionCancelResult != null)
                {
                    responseMessage = $"Skiepų skyrimo E025 dokumentas sėkmingai atšauktas. Dok. nr: {prescriptionCancelResult.CompositionId}";
                    responseMessage += dispensationCancelResult != null ?
                    $"\nSkiepų išdavimo E063 dokumentas sėkmingai atšauktas. Dok. nr: {dispensationCancelResult.CompositionId}" : string.Empty;

                    helpers.alert(Enumerator.alert.info, responseMessage);
                    await LoadVaccineEntries();

                }
            });
        }

        public async Task LoadVaccineEntries()
        {
            await ExecuteWithWaitAsync(async () =>
            {
                if (_view.Patient == null || string.IsNullOrEmpty(_view.Patient.PatientId))
                    throw new Exception("");
                
                Session.eRecipeUtils.cts = new CancellationTokenSource();
                List<VaccinationEntry> vaccinationEntries = new List<VaccinationEntry>();

                // Find patient's prescriptions
                var vaccinationPrescriptionDTO = await Session.eRecipeUtils.GetVaccinationData<VaccinationDataListDto>(
                    _view.Patient.PatientId,
                    string.Empty,
                    string.Empty,
                    (int)Enumerator.VaccineDocFilterValue.Prescription,
                    string.Empty,
                    string.Empty,
                    Session.VaccineGridSize,
                    _view.PageIndex,
                    string.Empty,
                    string.Empty,
                    string.Empty);

                // Find patient's dispensations by prescription order ids
                var orderIds = string.Join(",", vaccinationPrescriptionDTO?.VaccinationDataList?.Select(e => e.OrderId));
                var vaccinationDispensationDTO = await Session.eRecipeUtils.GetVaccinationData<VaccinationDataListDto>(
                    _view.Patient.PatientId,
                    string.Empty,
                    string.Empty,
                    (int)Enumerator.VaccineDocFilterValue.Dispensation,
                    string.Empty,
                    string.Empty,
                    100,
                    1,
                    string.Empty,
                    string.Empty,
                    orderIds);


                // Combine prescription and dispensation data
                foreach (var vpo in vaccinationPrescriptionDTO?.VaccinationDataList)
                {
                    var ve = new VaccinationEntry()
                    {
                        OrderId = vpo.OrderId.ToLong(),
                        Prescription = vpo
                    };

                    var vdo = vaccinationDispensationDTO?.VaccinationDataList.FirstOrDefault(e => e.OrderId == vpo.OrderId);
                    if (vdo != null)
                        ve.Dispensation = vdo;

                    vaccinationEntries.Add(ve);
                }

                _view.Total = vaccinationPrescriptionDTO?.Total.ToInt() ?? 0;
                _view.VaccineEntries = vaccinationEntries;
                if (_view.VaccineEntries?.Count <= 0)
                {
                    helpers.alert(Enumerator.alert.warning, "Pacientas neturi jokių skiepų įrašų!");
                }
            });
        }

        public void LoadVaccineUserControl()
        {
            if (Session.Develop)
            {
                _view.PersonalCode = "34010230353";
            }
        }

        #region Private methods
        private void Clear()
        {
            DisableControls();
            SetPatient();
        }

        private void SetPatient(PatientDto patient = null)
        {
            _view.PatientName = patient?.GivenName?.First();
            _view.PatientSurname = patient?.FamilyName.First();
            _view.PatientBirthDate = patient?.BirthDate;
        }

        private void DisableControls()
        {
            _view.IsActiveCreateVaccine = false;
            _view.IsActiveCancelVaccine = false;
            _view.IsActiveSellVaccine = false;
        }

        private void HandleControlsActivity()
        {
            if (_view.Patient == null || string.IsNullOrWhiteSpace(_view.Patient?.PatientId))
            {
                _view.IsActiveCreateVaccine = false;
                _view.IsActiveCancelVaccine = false;
                _view.IsActiveSellVaccine = false;
                throw new Exception("Įvestas neteisingas asmens kodas!");
            }
            else         
                _view.IsActiveCreateVaccine = true;
        }

        private string GetCancellationRequestMessage(VaccinationEntry ve) 
        {
            string meesage = string.Empty;

            if (ve == null) 
                return meesage;

            if (ve.Prescription != null)
                meesage += $"Ar tikrai norite atšaukti skiepų skyrimo dokumentą nr: {ve.Prescription.CompositionId}";

            if (ve.Dispensation != null)
                meesage += $" ir skiepų išdavimo dokumentą nr: {ve.Dispensation.CompositionId}";

            return meesage += "?";
        }

        public override bool IsBusy
        {
            get => _busy.IsBusy;
            set => _busy.IsBusy = value;
        }
        #endregion
    }
}
