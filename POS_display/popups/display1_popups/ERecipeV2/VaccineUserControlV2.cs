using POS_display.Helpers;
using POS_display.Models.Recipe;
using POS_display.Presenters.ERecipe;
using POS_display.Views.ERecipe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using TamroUtilities.HL7.Models;

namespace POS_display.Popups.display1_popups.ERecipe
{
    public partial class VaccineUserControlV2 : UserControlBase, IVaccineUserV2
    {
        #region Members
        private readonly VaccineUserPresenterV2 _vaccineUserPresenter;
        private VaccinationEntry _selectedVaccineEntry;
        private wpf.View.Navigation _orderListNavigation;
        private wpf.View.VaccineEntriesList _vaccineList;
        private wpf.ViewModel.BaseCommand OrderListNavigationRefreshCommand;
        #endregion

        #region Event/Delegates
        public event EventHandler PanelChangedEvent;
        public delegate void SelectionChangedHandler(VaccinationEntry vaccineEntry);
        public event SelectionChangedHandler SelectionChanged;
        public delegate void OperationAbortedHandler(object sender, object data);
        public event OperationAbortedHandler OperationAborted;
        #endregion

        #region Constructor
        public VaccineUserControlV2()
        {
            InitializeComponent();
            _vaccineUserPresenter = new VaccineUserPresenterV2(this, this);
            _orderListNavigation = new wpf.View.Navigation();
            _vaccineEntries = new List<VaccinationEntry>();
        }

        private void VaccineUserControl_Load(object sender, EventArgs e)
        {
            TbPersonalCode.Select();
        }
        #endregion

        #region Override
        public override bool IsBusy
        { 
            get => base.IsBusy;
            set 
            {
                if(_vaccineList != null)
                    _vaccineList.IsBusy = value;
                base.IsBusy = value;
            }
        }
        #endregion

        #region Properties               
        public VaccinationEntry SelectedVaccineEntry
        {
            get { return _selectedVaccineEntry; }
            set { _selectedVaccineEntry = value; }
        }

        public PatientDto Patient { get; set; }
        public LowIncomeDto LowIncome { get; set; }
        public string PersonalCode
        {
            get => TbPersonalCode.Text;
            set => TbPersonalCode.Text = value;
        }

        public string PatientName
        {
            get => TbPatientName.Text;
            set => TbPatientName.Text = value;
        }

        public bool IsActiveSellVaccine
        {
            get => BtnSellVaccine.Enabled;
            set => BtnSellVaccine.Enabled = value;
        }

        public bool IsActiveCancelVaccine
        {
            get => BtnVaccineCancel.Enabled;
            set => BtnVaccineCancel.Enabled = value;
        }

        public string PatientSurname
        {
            get => TbSurname.Text;
            set => TbSurname.Text = value;
        }

        public int PageIndex
        {
            get => _orderListNavigation.PageIndex;
        }
        public int Total { get; set; }
        private List<VaccinationEntry> _vaccineEntries;
        public List<VaccinationEntry> VaccineEntries
        {
            get
            {
                return _vaccineEntries;
            }
            set
            {
                _vaccineEntries = value;
                if (DesignMode) return;
                int count = Total != 0 ? Total : _vaccineEntries?.Count ?? 0;
                if (count < Session.VaccineGridSize)
                    count = Session.VaccineGridSize;
                _orderListNavigation.PageCount = (int)Math.Ceiling((decimal)count / Session.VaccineGridSize);
                _orderListNavigation.PageIndex = _orderListNavigation.PageIndex;
                _vaccineList = new wpf.View.VaccineEntriesList(_vaccineEntries);
                _vaccineList.SelectionChanged_Event += gvOrderList_SelectionChanged;
                ehVaccineOrderList.Child = _vaccineList;
            }
        }

        public string PatientBirthDate
        {
            get => TbBirthDate.Text;
            set => GetBirthDate(value);
        }
        #endregion

        #region Public methods
        public void ClearVaccineEntriesList() 
        {
            VaccineEntries = new List<VaccinationEntry>();
            OrderListNavigationRefreshCommand = new wpf.ViewModel.BaseCommand(btnOrderListNavigation_Click);
            _orderListNavigation.NavigationClick = OrderListNavigationRefreshCommand;
            ehOrderListNavigation.Child = _orderListNavigation;
            SelectionChanged?.Invoke(null);
        }
        public void ResetControls() 
        {
            List<Control> controls = new List<Control>();
            foreach (Control c in PnlVaccine.Controls)
            {
                if (c is VaccineSellUserControlV2 || c is VaccineSellUserControlV2)
                    controls.Add(c);
            }

            foreach (Control c in controls)            
                PnlVaccine.Controls.Remove(c);
            
        }
        #endregion

        #region Private methods
        private async void btnOrderListNavigation_Click(object obj)
        {
            await _vaccineUserPresenter.LoadVaccineEntries();
           
        }
        private void gvOrderList_SelectionChanged(object sender, EventArgs e)
        {
            List<VaccinationEntry> selectedEntries = sender as List<VaccinationEntry>;
            if (selectedEntries?.Count == 1)
            {
                SelectedVaccineEntry = selectedEntries.First();
                if (SelectionChanged != null)
                    SelectionChanged(SelectedVaccineEntry);

                IsActiveCancelVaccine = !string.Equals(SelectedVaccineEntry.OrderStatus, "cancelled", StringComparison.InvariantCultureIgnoreCase) &&
                    !string.Equals(SelectedVaccineEntry.OrderStatus, "aborted", StringComparison.InvariantCultureIgnoreCase);
            }
        }
        private void GetBirthDate(string value)
        {
            if (!string.IsNullOrWhiteSpace(value))
            {
                var now = DateTime.Now;
                var birthDate = helpers.getXMLDateOnly(value);
                TbBirthDate.Text = birthDate.ToString("yyyy-MM-dd");
                var age = now.Year - birthDate.Year;
                if (now.Month < birthDate.Month || (now.Month == birthDate.Month && now.Day < birthDate.Day))//not had bday this year yet
                    age--;
                TbAge.Text = age.ToString();
            }
            else
            {
                TbBirthDate.Text = value;
                TbAge.Text = value;
            }
        }

        private async void BtnFind_Click(object sender, EventArgs e)
        {
            ClearVaccineEntriesList();
            await _vaccineUserPresenter.FindPatient();
            await _vaccineUserPresenter.LoadVaccineEntries();
        }

        private void BtnSellVaccine_Click(object sender, EventArgs e)
        {
            var vaccineSellUserControl = new VaccineSellUserControlV2(Patient, LowIncome);
            vaccineSellUserControl.CloseEvent += VaccineUserControlOnCloseEvent;
            ChangePanel(vaccineSellUserControl);
            PanelChangedEvent?.Invoke(vaccineSellUserControl, e);
        }

        private void TbPersonalCode_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
                BtnFind_Click(BtnFind, new EventArgs());
        }

        private void Vaccine_Load(object sender, EventArgs e)
        {
            _vaccineUserPresenter.LoadVaccineUserControl();
        }

        private async void VaccineUserControlOnCloseEvent(object sender, object data, Enumerator.VaccineOrderEvent evt)
        {
            try
            {
                SuspendLayout();
                if (evt == Enumerator.VaccineOrderEvent.Created)
                {
                    ResetControls();
                    await _vaccineUserPresenter.LoadVaccineEntries();
                }
                else if (evt == Enumerator.VaccineOrderEvent.Closed)
                {
                    ResetControls();
                    await _vaccineUserPresenter.LoadVaccineEntries();
                }
                else if (evt == Enumerator.VaccineOrderEvent.Aborted)
                {
                    ResetControls();
                    OperationAborted?.Invoke(sender, data);
                }
                else if (evt == Enumerator.VaccineOrderEvent.CreatedToDispense)
                {
                    if (data == null || !(data is VaccineDispenseArgs))
                        return;

                    VaccineDispenseArgs args = data as VaccineDispenseArgs;
                    await Program.Display1.SubmitBarcode(args.BarcodeModel);

                    if (args.BarcodeModel?.PosdId < 0)
                        return;

                    await _vaccineUserPresenter.LoadVaccineEntries();
                    VaccinationEntry vaccineEntry = _vaccineEntries.FirstOrDefault(e => e.Prescription.CompositionId == args.CompositionId);
                    if (vaccineEntry != null)
                    {
                        var vaccineSellUserControl = new VaccineSellUserControl(Patient, vaccineEntry);
                        vaccineSellUserControl.CloseEvent += VaccineUserControlOnCloseEvent;

                        Items.posd posd =  Program.Display1.PoshItem?.PosdItems[0];
                        vaccineSellUserControl.FillVaccineData(posd, args.BarcodeModel.SerialNumber, args.DoseNumber);
                        ChangePanel(vaccineSellUserControl);
                    }
                }
            }
            finally 
            {
                ResumeLayout();
            }
        }        

        private void ChangePanel(UserControl userControl)
        {
            userControl.Dock = DockStyle.Fill;
            PnlVaccine.Controls.Add(userControl);
            userControl.BringToFront();
        }

        private async void BtnVaccineCancel_Click(object sender, EventArgs e)
        {
            if (SelectedVaccineEntry == null) return;
            await _vaccineUserPresenter.CancelVaccination(SelectedVaccineEntry);
        }
        #endregion
    }
}
