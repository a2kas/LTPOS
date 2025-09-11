using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using POS_display.Models.ECRReports;
using POS_display.Presenters.ECRReports;
using POS_display.Repository.ECRReports;
using POS_display.Repository.Pos;
using POS_display.Views;
using POS_display.Views.ECRReports;
using System;
using System.Windows.Forms;

namespace POS_display
{
    public partial class ECRReportsView : FormBase, IECRReportsView, IBaseView
    {
        #region Members
        private readonly ECRReportsPresenter _ecrReportsPresenter;
        #endregion

        #region Constructor
        public ECRReportsView()
        {
            _ecrReportsPresenter = new ECRReportsPresenter(this, 
                new PosRepository(),
                new ECRReportsRepository(),
                Program.ServiceProvider.GetRequiredService<IMapper>());
            InitializeComponent();
        }
        #endregion

        #region Properties
        public DateTimePicker DateFrom
        {
            get { return dtpDateFrom; }
            set { dtpDateFrom = value; }
        }

        public DateTimePicker DateTo
        {
            get { return dtpDateTo; }
            set { dtpDateTo = value; }
        }

        public DateTimePicker SetDate
        {
            get { return dtpSetDate; }
            set { dtpSetDate = value; }
        }

        public DateTimePicker SetTime
        {
            get { return dtpSetTime; }
            set { dtpSetTime = value; }
        }

        public TextBox Change
        {
            get { return tbChange; }
            set { tbChange = value; }
        }

        public Button Calc
        {
            get { return btnCalc; }
            set { btnCalc = value; }
        }

        public ComboBox Report
        {
            get { return cmbReport; }
            set { cmbReport = value; }
        }

        public string SelectedReportIndex
        {
            get { return (Report.SelectedItem as ECRReport).Id; }
        }

        public DialogResult FormDialogResult 
        {
            get { return DialogResult; }
            set { DialogResult = value; }
        }
        #endregion


        #region Private methods
        private async void ECRReportsView_Load(object sender, EventArgs e)
        {
            await ExecuteWithWaitAsync(async () =>
            {
                await _ecrReportsPresenter.UpdateSession("Kasos aparato ataskaitų spausdinimas ", 2);
                await _ecrReportsPresenter.InitOperationsData();
                _ecrReportsPresenter.EnableControls((Report.SelectedItem as ECRReport).Id);
            }, 
            false);

            if (Session.Devices.fiscal != 1)
                if (!helpers.alert(Enumerator.alert.confirm, "Nerastas, arba atjungtas kasos aparatas.\n" +
                    "Ar tikrai norite vykdyti fiskalines operacijas?\n" +
                    "Visos fiskalinės operacijos bus įvykdytos tik programoje!", true))
                    this.DialogResult = DialogResult.Cancel;
        }

        private void ECRReports_Closing(object sender, FormClosingEventArgs e)
        {
              if (IsBusy)
                e.Cancel = true;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private async void btnCalc_Click(object sender, EventArgs e)
        {
            await ExecuteWithWaitAsync(async () =>
            {
                decimal poshId = await _ecrReportsPresenter.PerformExecute((Report.SelectedItem as ECRReport).Id);
                if (poshId > 0)
                    DialogResult = DialogResult.Cancel;
                else
                    helpers.alert(Enumerator.alert.error, $"Nepavyko atlikti '{(Report.SelectedItem as ECRReport).Command}' operacijos!");
            });
        }

        private void cmbReport_Changed(object sender, EventArgs e)
        {
            ExecuteWithWait(() =>
            {
                _ecrReportsPresenter.EnableControls((Report.SelectedItem as ECRReport).Id);
            });
        }
        #endregion

        #region Override
        protected override bool ProcessDialogKey(Keys keyData)
        {
            if (keyData == Keys.Tab)
            {
                SendKeys.SendWait("{RIGHT}");
                return true;
            }
            else
                return base.ProcessDialogKey(keyData);
        }
        #endregion

        #region Actions
        private void tbChange_KeyPress(object sender, KeyPressEventArgs e)
        {
            helpers.tb_KeyPress(sender, e);
        }

        private void tbChange_TextChanged(object sender, EventArgs e)
        {
            ExecuteWithWait(() =>
            {
                _ecrReportsPresenter.ChangeTextChanged(sender);
            });
        }
        #endregion     
    }
}