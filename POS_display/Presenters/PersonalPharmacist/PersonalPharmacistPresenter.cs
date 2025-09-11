using POS_display.Views.PersonalPharmacist;
using POS_display.Repository.PersonalPharmacist;
using System.Threading.Tasks;
using POS_display.Models.PersonalPharmacist;

namespace POS_display.Presenters.PersonalPharmacist
{
    public class PersonalPharmacistPresenter : BasePresenter
    {
        #region Members
        private readonly IPersonalPharmacistView _view;
        private readonly IPersonalPharmacistRepository _personalPharmacistRepository;
        #endregion

        #region Constructor
        public PersonalPharmacistPresenter(IPersonalPharmacistView view, IPersonalPharmacistRepository personalPharmacistRepository)
        {
            _view = view;
            _personalPharmacistRepository = personalPharmacistRepository;
        }
        #endregion

        #region Public methods
        public async void Init()
        {
            if (Session.PersonalPharmacistData != null)
            {
                _view.ClientPersonalCode.Text = Session.PersonalPharmacistData.ClientPersonalCode;
                _view.DueDate = Session.PersonalPharmacistData.DueDate;
                _view.DoctorID.Text = Session.PersonalPharmacistData.DoctorID;
                _view.DoctorName.Text = Session.PersonalPharmacistData.DoctorName;
                _view.DoctorSurename.Text = Session.PersonalPharmacistData.DoctorSurename;
                _view.Hospital.Text = Session.PersonalPharmacistData.HospitalName;
                HandeButtons(true);
            }
            else
                HandeButtons(false);

            await PopulateHospitals();
        }

        public bool CheckCardAvailability() 
        {
            return !string.IsNullOrEmpty(Program.Display1.PoshItem.CRMItem.Account.CardNumber);
        }

        public void HandeButtons(bool apply) 
        {
            _view.Apply.Enabled = !apply;
            _view.Cancel.Enabled = apply;
        }

        public async Task SavePersonalPharmacistData(long posHeaderID)
        {
            Session.PersonalPharmacistData = new PersonalPharmacistData
            {
                PharmacyName = Session.SystemData.name,
                ClientPersonalCode = _view.ClientPersonalCode.Text,
                DueDate = _view.DueDate,
                DoctorID = _view.DoctorID.Text,
                DoctorName = _view.DoctorName.Text,
                DoctorSurename = _view.DoctorSurename.Text,
                HospitalName = _view.Hospital.Text
            };

            await _personalPharmacistRepository.InsertPersonalPharmacistData(posHeaderID, Session.PersonalPharmacistData);
        }

        public async Task DeletePersonalPharmacistData(long posHeaderID)
        {
            await _personalPharmacistRepository.DeletePersonalPharmacistByPoshID(posHeaderID);
            Session.PersonalPharmacistData = null;
        }
        #endregion

        private async Task PopulateHospitals() 
        {
            foreach (string val in await _personalPharmacistRepository.GetPersonalPharmacistHospitals())            
                _view.Hospital.Items.Add(val);

            if (_view.Hospital.Items.Count != 0)
                _view.Hospital.SelectedIndex = 0;
        }

    }
}
