using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using eRecipeWS.DTO;
using POS_display.Views;
using POS_display.Views.ERecipe;

namespace POS_display.Presenters.ERecipe
{
    public class VaccinePresenter : ErrorHandling
    {
        private readonly IBusy _busy;
        private readonly IVaccine _view;
        public VaccinePresenter(IBusy busy, IVaccine view)
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
                SetPatient();
                Session.eRecipeUtils.cts = new CancellationTokenSource();

                _view.Patient = await Session.eRecipeUtils.GetPatient<eRecipeWS.DTO.PatientDto>(_view.PersonalCode, "");

                SetPatient(_view.Patient);

                if (string.IsNullOrWhiteSpace(_view.Patient?.PatientId))
                    return;

                if (_view.Patient == null)
                    throw new Exception("Įvestas neteisingas asmens kodas!");

                _view.IsActiveCreateVaccine = true;
                //IsActive selectedElement != null
                _view.IsActiveSellVaccine = true;
            });
        }

        public void LoadVaccineUserControl()
        {
            if (Session.Develop == true)
            {
                _view.PersonalCode = "34010230353";
            }
        }

        private void SetPatient(PatientDto patient = null)
        {
            _view.PatientName = patient?.GivenName?.First();
            _view.PatientSurname = patient?.FamilyName.First();
            _view.PatientBirthDate = patient?.BirthDate;
        }

        public override bool IsBusy
        {
            get => _busy.IsBusy;
            set => _busy.IsBusy = value;
        }
    }
}
