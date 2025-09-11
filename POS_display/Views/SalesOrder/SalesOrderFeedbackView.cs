using Newtonsoft.Json;
using POS_display.Helpers;
using POS_display.Models.Partner;
using System;
using System.Windows.Forms;

namespace POS_display.Views.SalesOrder
{
    public partial class SalesOrderFeedbackView : FormBase, ISalesOrderFeedbackView
    {
        #region Variables
        private string _customerSignature;
        private PartnerViewData _partnerData;
        #endregion

        #region Consturctor
        public SalesOrderFeedbackView()
        {
            InitializeComponent();
            FormBorderStyle = FormBorderStyle.FixedSingle;
            ControlBox = false;
            ButtonApprove.Enabled = false;
            LoaderControl.IsLoading = true;
        }

        public SalesOrderFeedbackView(PartnerViewData partner)
        {
            InitializeComponent();
            FormBorderStyle = FormBorderStyle.FixedSingle;
            ControlBox = false;
            ButtonApprove.Enabled = false;
            LoaderControl.IsLoading = true;
            _partnerData = partner;
        }
        #endregion

        #region Properties
        public Button ButtonApprove
        {
            get => btnApprove;
            set => btnApprove = value;
        }

        public Button ButtonCancel
        {
            get => btnCancel;
            set => btnCancel = value;
        }

        public LoaderUserControl LoaderControl
        {
            get => ucLoader;
        }

        public Label InfoField
        {
            get => lblInfoField;
            set => lblInfoField = value;
        }

        public string CustomerSignature
        {
            get => _customerSignature;
            set => _customerSignature = value;
        }
        #endregion

        #region Private methods
        private void FeedbackTerminal_SignatureSubmitEvent(string signatureData)
        {
            if (!string.IsNullOrEmpty(signatureData))
            {
                _customerSignature = signatureData;
                BeginInvoke(new Action(() =>
                {
                    lblInfoField.Text = "Kliento sutikimas gautas";
                    lblInfoField.ForeColor = System.Drawing.Color.Green;
                    ButtonApprove.Enabled = true;
                    LoaderControl.IsLoading = false;
                }));
            }
        }
        #endregion

        #region Actions
        private void btnApprove_Click(object sender, System.EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }

        private void btnCancel_Click(object sender, System.EventArgs e)
        {
            Close();
        }

        private void SalesOrderFeedbackView_Shown(object sender, System.EventArgs e)
        {
            Session.FeedbackTerminal.SignatureSubmitEvent += FeedbackTerminal_SignatureSubmitEvent;
            if (_partnerData == null)
                Session.FeedbackTerminal.ExecuteAction<Models.FeedbackTerminal.BaseRequest>(Models.FeedbackTerminal.RequestName.CustomerAgreement);
            else
            {
                var requestModel = new Models.FeedbackTerminal.CustomerAgreementRequest
                {
                    Name = _partnerData.Name,
                    Phone = _partnerData.Phone,
                    Email = _partnerData.Email,
                    Address = _partnerData.Address,
                    City = _partnerData.City,
                    PostIndex = _partnerData.PostIndex
                };
                Session.FeedbackTerminal.ExecuteAction<Models.FeedbackTerminal.BaseRequest>(Models.FeedbackTerminal.RequestName.CustomerAgreement, requestModel);
            }
        }

        private void SalesOrderFeedbackView_FormClosing(object sender, FormClosingEventArgs e)
        {
            Session.FeedbackTerminal.SignatureSubmitEvent -= FeedbackTerminal_SignatureSubmitEvent;
            Session.FeedbackTerminal.ExecuteAction<Models.FeedbackTerminal.BaseRequest>(Models.FeedbackTerminal.RequestName.CustomerWelcome);
        }
        #endregion
    }
}
