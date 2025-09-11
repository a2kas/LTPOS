using System;
using System.Data;
using POS_display.Views;
using System.Windows.Forms;
using POS_display.Views.KAS;
using POS_display.Repository.KAS;
using POS_display.Presenters.KAS;
using POS_display.Repository.Partners;
using POS_display.Models.KAS;
using POS_display.Models.Partner;
using POS_display.Views.Partners;

namespace POS_display
{
    public partial class InvoiceView : FormBase, IInvoiceView, IBaseView
    {

        #region Members
        private readonly InvoicePresenter _invoicePresenter;
        private PosHeader _posHeader;
        private long _creditorId = 0;
        #endregion

        #region Constructor
        public InvoiceView(PosHeader posHeader)
        {
            _posHeader = posHeader;
            InitializeComponent();

            _invoicePresenter = new InvoicePresenter(this, new KASRepository(), new PartnerRepository());
        }
        #endregion

        #region Properties

        public long CreditorId
        {
            get { return _creditorId;  }
            set { _creditorId = value; }
        }

        public TextBox CheckNo
        {
            get { return tbCheckNo; }
            set { tbCheckNo = value; }
        }

        public TextBox CheckDate
        {
            get { return tbCheckDate; }
            set { tbCheckDate = value; }
        }

        public TextBox DocumentDate
        {
            get { return tbDocumentDate; }
            set { tbDocumentDate = value; }
        }

        public TextBox DocumentNo
        {
            get { return tbDocumentNo; }
            set { tbDocumentNo = value; }
        }

        public TextBox DebtorEcode
        {
            get { return tbDebtorEcode; }
            set { tbDebtorEcode = value; }
        }

        public Button SelDebtor
        {
            get { return btnSelDebtor; }
            set { btnSelDebtor = value; }
        }

        public TextBox DebtorName
        {
            get { return tbDebtorName; }
            set { tbDebtorName = value; }
        }

        public Button Save
        {
            get { return btnSave; }
            set { btnSave = value; }
        }
        #endregion

        #region Actions
        private async void InvoiceView_Load(object sender, EventArgs e)
        {
            await ExecuteWithWaitAsync(async () =>
            {
                await _invoicePresenter.UpdateSession("Sąskaita ", 2);
                await _invoicePresenter.Init(_posHeader);
                _invoicePresenter.EnableSaving();
            }, false);
        }

        private void InvoiceView_Closing(object sender, FormClosingEventArgs e)
        {
            if (IsBusy)
                e.Cancel = true;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private async void btnSave_Click(object sender, EventArgs e)
        {
            await ExecuteWithWaitAsync(async () =>
            { 
                if (string.IsNullOrEmpty(DocumentNo.Text) || CreditorId == 0)
                    helpers.alert(Enumerator.alert.error, "Neįvesti duomenys!");
                else
                {
                    if (await _invoicePresenter.CheckSFHeaderExist(DocumentNo.Text))
                        helpers.alert(Enumerator.alert.error, "Toks sąskaitos faktūros nr. jau egzistuoja!");
                    else                
                        DialogResult = DialogResult.OK;
                
                }
            });
        }
        
        private void btnSelectPartner_Click(object sender, EventArgs e)
        {
            ExecuteWithWait(() =>
            {
                using (PartnersView dlg = new PartnersView())
                {
                    dlg.Location = helpers.middleScreen(this, dlg);
                    dlg.ShowDialog();
                    if (dlg.DialogResult == DialogResult.OK)
                    {
                        tbDebtorEcode.Text = dlg.FocusedPartner.ECode;
                        tbDebtorName.Text = dlg.FocusedPartner.Name;
                        CreditorId = dlg.FocusedPartner.Id.ToLong();
                        _invoicePresenter.EnableSaving();
                    }
                    tbDocumentNo.Select();
                }
            });
        }

        private async void tbDebtorEcode_Leave(object sender, EventArgs e)
        {
            await ExecuteWithWaitAsync(async () => 
            {
                Partner partner = await _invoicePresenter.LoadPartnerData(tbDebtorEcode.Text);
                if (partner != null)
                    _invoicePresenter.SetPartnerData(partner);
                else
                    btnSelectPartner_Click(new object(), new EventArgs());
                _invoicePresenter.EnableSaving();
            });   
        }

        private void tbDocumentNo_TextChanged(object sender, EventArgs e)
        {
            ExecuteWithWait(() =>
            {
                _invoicePresenter.EnableSaving();
            });
        }

        private void tbDocumentDate_TextChanged(object sender, EventArgs e)
        {
            ExecuteWithWait(() =>
            {
                _invoicePresenter.EnableSaving();
            });
        }

        #endregion
    }
}
