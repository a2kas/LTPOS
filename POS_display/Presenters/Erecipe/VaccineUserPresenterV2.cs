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
    public class VaccineUserPresenterV2 : ErrorHandling
    {
        private readonly IBusy _busy;
        private readonly IVaccineUserV2 _view;
        private const string CancellationReason = "Atšauktas dokumentas";

        public VaccineUserPresenterV2(IBusy busy, IVaccineUserV2 view)
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
                    else 
                        responseMessage = $"Skiepų išdavimo E063 dokumentas sėkmingai atšauktas. Dok. nr: {dispensationCancelResult.CompositionId}";
                    
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

                var vaccinationDispensationDTO = await Session.eRecipeUtils.GetVaccinationData<VaccinationDataListDto>(
                    _view.Patient.PatientId,
                    string.Empty,
                    string.Empty,
                    (int)Enumerator.VaccineDocFilterValue.Dispensation,
                    string.Empty,
                    string.Empty,
                    Session.VaccineGridSize,
                    _view.PageIndex,
                    string.Empty,
                    string.Empty,
                    string.Empty);

                foreach (var vpo in vaccinationDispensationDTO?.VaccinationDataList)
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

                _view.Total = vaccinationDispensationDTO?.Total.ToInt() ?? 0;
                _view.VaccineEntries = vaccinationEntries; ;
                if (_view.VaccineEntries?.Count <= 0)
                {
                    helpers.alert(Enumerator.alert.warning, "Pacientas neturi jokių skiepų įrašų!");
                }
            },false);
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
            _view.IsActiveCancelVaccine = false;
           // _view.IsActiveSellVaccine = false;
        }

        private void HandleControlsActivity()
        {
            if (_view.Patient == null || string.IsNullOrWhiteSpace(_view.Patient?.PatientId))
            {
                _view.IsActiveCancelVaccine = false;
                _view.IsActiveSellVaccine = false;
                throw new Exception("Įvestas neteisingas asmens kodas!");
            }
            else
                _view.IsActiveSellVaccine = true;
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
