using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Forms;
using POS_display.Models;
using POS_display.Models.General;
using POS_display.Presenters.SalesOrder;
using POS_display.Repository.SalesOrder;

namespace POS_display.Views.SalesOrder
{
    public partial class SalesOrderView : FormBase, ISalesOrderView
    {
        #region Members
        private readonly SalesOrderPresenter _salesOrderPresenter;
        private Barcode _barcodeModel;
        private ClientData _focusedClient;
        private Items.posh _poshItem;
        #endregion

        #region Constructor
        public SalesOrderView(Barcode barcodeModel, Items.posh poshItem)
        {
            InitializeComponent();
            _barcodeModel = barcodeModel;
            _poshItem = poshItem;
            _salesOrderPresenter = new SalesOrderPresenter(this, new SalesOrderRepository());
            _salesOrderPresenter.EnableControls();
        }

        #endregion

        #region Properties
        public TextBox ClientName
        {
            get => tbName;
            set => tbName = value;
        }

        public TextBox Surename
        {
            get => tbSurename;
            set => tbSurename = value;
        }

        public TextBox Phone
        {
            get => tbPhone;
            set => tbPhone = value;
        }

        public TextBox Email
        {
            get => tbEmail;
            set => tbEmail = value;
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

        public TextBox PostCode
        {
            get => tbPostCode;
            set => tbPostCode= value;
        }

        public ComboBox Country
        {
            get => cbCountry;
            set => cbCountry = value;
        }

        public TextBox Comment
        {
            get => tbComment;
            set => tbComment = value;
        }

        public Button EditClientData
        {
            get => btnEditClientData;
            set => btnEditClientData = value;
        }

        public Button PrintAgreement
        {
            get => btnPrintAgreement;
            set => btnPrintAgreement = value;
        }

        public Button SendToTerminal
        {
            get => btnSendToTerminal;
            set => btnSendToTerminal = value;
        }

        public List<ClientData> Clients
        {
            get => clientDataGridView.DataSource as List<ClientData>;
            set => clientDataGridView.DataSource = value;
        }

        public Barcode BarcodeModel
        {
            get => _barcodeModel;
            set => _barcodeModel = value;
        }

        public ClientData FocusedClient
        {
            get => _focusedClient;
            set => _focusedClient = value;
        }

        public Items.posh PoshItem
        {
            get => _poshItem;
            set => _poshItem = value;
        }

        public string CustomerSignature { get; set; }
        public decimal QtyToTransfer { get; set; }

        #endregion

        #region Public methods

        public async Task UpdateCurrentSalesOrderPosDetail(long posDetailId) 
        {
            await ExecuteWithWaitAsync(async () =>
            {
                await _salesOrderPresenter.UpdatePosDetailID(posDetailId);

            });
        } 

        #endregion

        #region Actions
        private async void btnConfirm_Click(object sender, EventArgs e)
        {
            await ExecuteWithWaitAsync(async () =>
            {
                var errorMessage = _salesOrderPresenter.ValidateClientData();
                if (!string.IsNullOrEmpty(errorMessage))
                {
                    helpers.alert(Enumerator.alert.warning, errorMessage);
                    return;
                }

                await _salesOrderPresenter.TransferFromRemotePharmacy();
                DialogResult = DialogResult.OK;

            });
        }

        private void tbPhone_KeyPress(object sender, KeyPressEventArgs e)
        {
            HandleDigit(e);
        }
        #endregion

        private void tbName_KeyPress(object sender, KeyPressEventArgs e)
        {
            HandleChar(e);
        }

        private void tbSurename_KeyPress(object sender, KeyPressEventArgs e)
        {
            HandleChar(e);
        }

        private void tbCity_KeyPress(object sender, KeyPressEventArgs e)
        {
            HandleChar(e);
        }

        private static void HandleChar(KeyPressEventArgs e)
        {
            if (char.IsLetter(e.KeyChar) || char.IsControl(e.KeyChar)) return;
                e.Handled = true;
        }

        private static void HandleDigit(KeyPressEventArgs e)
        {
            if (char.IsControl(e.KeyChar) || char.IsDigit(e.KeyChar) || e.KeyChar == '+') return;
                e.Handled = true;
        }

        private async void btnFindClient_Click(object sender, EventArgs e)
        {
            await ExecuteWithWaitAsync(async () =>
            {
                await _salesOrderPresenter.LoadClientData(new
                {
                    method = "searchclient",
                    name = ClientName.Text,
                    surename = Surename.Text,
                    phone = Phone.Text,
                    email = Email.Text,
                    address = Address.Text,
                    city = City.Text,
                    postcode = PostCode.Text,
                    country = Country.SelectedItem?.ToString() ?? string.Empty,
                    comment = Comment.Text

                }.ToJsonString());

                if (Clients != null && Clients.Count == 0)
                    helpers.alert(Enumerator.alert.warning, "Pagal nustatytus kriterijus nepavyko rasti nei vieno egzistuojančio kliento!");
                else
                    _salesOrderPresenter.ClearCientDataForm();
            });
        }

        private void clientDataGridView_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            ExecuteWithWait(() =>
            {
                if (e.RowIndex < 0) return;
                var selectedRow = clientDataGridView.Rows[e.RowIndex];
                if (!(selectedRow.DataBoundItem is ClientData cd)) return;
                _focusedClient = cd;
                ClientName.Text = _focusedClient.Name;
                Surename.Text = _focusedClient.Surename;
                Phone.Text = _focusedClient.Phone;
                Email.Text = _focusedClient.Email;
                Address.Text = _focusedClient.Address;
                City.Text = _focusedClient.City;
                PostCode.Text = _focusedClient.PostCode;
                Country.SelectedItem = _focusedClient.Country;
                Comment.Text = _focusedClient.Comment;
                _salesOrderPresenter.EnableInputFields(false);
                _salesOrderPresenter.EnableControls();
            });
        }

        private void btnEditClientData_Click(object sender, EventArgs e)
        {
            ExecuteWithWait(() =>
            {
                _salesOrderPresenter.EnableInputFields(true);
            });
        }

        private void btnPrintAgreement_Click(object sender, EventArgs e)
        {
            ExecuteWithWait(() =>
            {
                var agreementDoc = _salesOrderPresenter.CreateAgreementDocument();
                var wpf = new wpf.View.PrintPreview(agreementDoc)
                {
                    Description = "Sutikimo forma",
                    DataContext = new wpf.ViewModel.BaseViewModel()
                };

                using (Popups.wpf_dlg pfForm = new Popups.wpf_dlg(wpf, "Failo peržiūra"))
                {
                    pfForm.Location = helpers.middleScreen(Program.Display1, pfForm);
                    pfForm.Activate();
                    pfForm.BringToFront();
                    pfForm.ShowDialog();
                }
            });
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            ExecuteWithWait(() =>
            {
                Clients = new List<ClientData>();
                FocusedClient = null;
                _salesOrderPresenter.ClearCientDataForm();
                _salesOrderPresenter.EnableControls();
                _salesOrderPresenter.EnableInputFields(true);
            });
        }

        private void btnSendToTerminal_Click(object sender, EventArgs e)
        {
            using (var salesOrderFeedbackView = new SalesOrderFeedbackView())
            {
                var agreementDoc = _salesOrderPresenter.CreateAgreementDocument();
                var wpfCustomerInfo = new wpf.View.display2.wpfCustomerInfo(agreementDoc);
                Program.Display2.ChangeScreen(wpfCustomerInfo);

                if (DialogResult.OK == salesOrderFeedbackView.ShowDialog())
                {
                    CustomerSignature = salesOrderFeedbackView.CustomerSignature;
                }
                Program.Display2.ChangeScreen();
            }
        }

        private async void btnSaveClientData_Click(object sender, EventArgs e)
        {
            await ExecuteWithWaitAsync(async () =>
            {
                var errorMessage = _salesOrderPresenter.ValidateClientData();
                if (!string.IsNullOrEmpty(errorMessage))
                {
                    helpers.alert(Enumerator.alert.warning, errorMessage);
                }
                else
                {
                    var clientId = await _salesOrderPresenter.SaveClientData();
                    if (clientId != "0")
                    {
                        await _salesOrderPresenter.LoadClientData(new
                        {
                            method = "searchclient",
                            clientid = clientId
                        }.ToJsonString());

                        _salesOrderPresenter.SetFocusedClient(Clients?.FirstOrDefault());
                        _salesOrderPresenter.EnableInputFields(false);
                        _salesOrderPresenter.EnableControls();
                    }
                }
            });
        }
    }
}
