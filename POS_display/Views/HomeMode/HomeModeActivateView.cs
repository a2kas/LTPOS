using Microsoft.Extensions.DependencyInjection;
using POS_display.Models.Partner;
using POS_display.Presenters.HomeMode;
using POS_display.Repository.HomeMode;
using POS_display.Repository.Partners;
using POS_display.Views.HomeMode;
using POS_display.Views.Partners;
using POS_display.Views.SalesOrder;
using System.Windows.Forms;
using Tamroutilities.Client;

namespace POS_display.Views.HomeDelivery
{
    public partial class HomeModeActivateView : FormBase, IHomeModeAcitvateView
    {
        #region Members
        private readonly IHomeModePresenter _homeModePresenter;
        #endregion

        #region Constructor
        public HomeModeActivateView()
        {
            InitializeComponent();
            _homeModePresenter = new HomeModePresenter(
                this, 
                new HomeModeRepository(),
                new PartnerRepository(),
                Program.ServiceProvider.GetRequiredService<ITamroClient>());
        }
        #endregion

        #region Properties
        public TextBox BuyerName
        {
            get => tbName;
            set => tbName = value;
        }

        public TextBox Address
        {
            get => tbAddress;
            set => tbAddress = value;
        }

        public TextBox City
        {
            get => tbCity;
            set => tbCity = value;
        }

        public TextBox PhoneNumber
        {
            get => tbPhone;
            set => tbPhone = value;
        }

        public TextBox PostIndex
        {
            get => tbPostIndex;
            set => tbPostIndex = value;
        }

        public TextBox CountryCode
        {
            get => tbCountryCode;
            set => tbCountryCode = value;
        }

        public TextBox Email
        {
            get => tbEmail;
            set => tbEmail = value;
        }

        public Button StartProcess
        {
            get => btnStart;
        }

        public Button SendToTerminal
        {
            get => btnSendToTerminal;
        }
        #endregion

        #region Private methods
        private void btnSelDebtor_Click(object sender, System.EventArgs e)
        {
            ExecuteWithWait(() =>
            {
                using (PartnersView dlg = new PartnersView())
                {
                    dlg.PartnerEditConfig = new PartnerEditConfig { CityValueReadOnly = false };
                    var partner = _homeModePresenter.GetPartner();
                    dlg.SetFocusPartner(partner?.Id ?? 0);

                    dlg.Location = helpers.middleScreen(this, dlg);
                    dlg.ShowDialog();
                    if (dlg.DialogResult == DialogResult.OK)
                    {
                        _homeModePresenter.SetPartner(dlg.FocusedPartner);
                        _homeModePresenter.EnableButtons();
                    }
                }
            });
        }

        private async void btnSendToTerminal_Click(object sender, System.EventArgs e)
        {
            await ExecuteWithWaitAsync(async () =>
            {
                _homeModePresenter.Validate();
                var partnerData = _homeModePresenter.GetPartner();
                using (var salesOrderFeedbackView = new SalesOrderFeedbackView(partnerData))
                {
                    if (DialogResult.OK == salesOrderFeedbackView.ShowDialog())
                    {       
                        _homeModePresenter.SetSignature(salesOrderFeedbackView.CustomerSignature);
                        await _homeModePresenter.SetPartnerAgreementToSaveData();
                        await _homeModePresenter.CreateHomeDeliverOrder();
                        _homeModePresenter.EnableButtons();
                    }
                }
            });
        }

        private void btnStart_Click(object sender, System.EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }

        private async void btnClose_Click(object sender, System.EventArgs e)
        {
            await ExecuteWithWaitAsync(async () => await _homeModePresenter.CancelHomeDeliverOrder());
            Close();
        }

        private void HomeModeActivateView_Shown(object sender, System.EventArgs e)
        {
            _homeModePresenter.EnableButtons();
        }
        #endregion
    }
}
