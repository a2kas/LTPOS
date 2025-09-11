using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using POS_display.Views.NarcoticAlert;
using POS_display.Presenters.NarcoticAlert;
using POS_display.Helpers;
using POS_display.Repository.NarcoticAlert;

namespace POS_display.Views.NarcoticAlert
{
    public partial class NarcoticAlertView : FormBase, INarcoticAlertView, IBaseView
    {
        #region Members
        private readonly NarcoticAlertPresenter _narcoticAlertPresenter;
        private Enumerator.DrugType? _drugType = null;
        private string _atc = string.Empty;
        #endregion

        #region Constructor
        public NarcoticAlertView(Enumerator.DrugType drugType, string atc)
        {
            InitializeComponent();
            _narcoticAlertPresenter = new NarcoticAlertPresenter(this, new NarcoticAlertRepository());
            _drugType = drugType;
            _atc = atc;
            LoadData();

        }
        #endregion

        #region Properties
        public Label Header
        {
            get { return lblHeader; }
            set { lblHeader = value; }
        }
        public RichTextBoxEx Notification
        {
            get { return rtbNotification; }
            set { rtbNotification = value; }
        }
        public DataGridView DrugMaterials
        {
            get { return dataGridView_drugMaterials; }
            set { dataGridView_drugMaterials = value; }
        }
        #endregion

        #region Actions

        private async void LoadData() 
        {
            if (_narcoticAlertPresenter != null)
                await _narcoticAlertPresenter.Init(_drugType.Value, _atc);
        }
        private void btnClose_Click(object sender, EventArgs e)
        {
            if (!IsBusy)
                Close();
        }
        private void RtbNotification_LinkClicked(object sender, System.Windows.Forms.LinkClickedEventArgs e)
        {
            ExecuteWithWait(() =>
            {
                string listUrl = Session.getParam("INTRANET", "EXCEPTION_LIST_URL");
                if (string.IsNullOrEmpty(listUrl))
                    helpers.alert(Enumerator.alert.warning, "Išimčių sąrašas tuščias");
                else
                    System.Diagnostics.Process.Start(listUrl);
            });
        }
        #endregion
    }
}
