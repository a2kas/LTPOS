using POS_display.Models.CRM;
using POS_display.Presenters.CRM;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace POS_display.Views.CRM
{
    public partial class ClientSearchView : FormBase, IClientSearchView
    {
        #region Members
        private CRMClientData _focusedClient;
        private readonly ClientSearchPresenter _clientSearchPresenter;
        #endregion

        #region Constructor
        public ClientSearchView()
        {
            InitializeComponent();
            _clientSearchPresenter = new ClientSearchPresenter(this);
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

        public MaskedTextBox BirthDate
        {
            get => mtbBirthdate;
            set => mtbBirthdate = value;
        }

        public List<CRMClientData> Clients
        {
            get => clientDataGridView.DataSource as List<CRMClientData>;
            set => clientDataGridView.DataSource = value;
        }

        public CRMClientData FocusedClient
        {
            get => _focusedClient;
            set => _focusedClient = value;
        }

        public Button ConfirmButton
        {
            get => btnConfirm;
            set => btnConfirm = value;
        }

        public Label BirthDateNote
        {
            get => lblBirthdate;
            set => lblBirthdate = value;
        }
        #endregion

        #region Actions
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
            if (char.IsControl(e.KeyChar) || char.IsDigit(e.KeyChar)) return;
                e.Handled = true;
        }

        private async void btnFindClient_Click(object sender, EventArgs e)
        {
            await ExecuteWithWaitAsync(async () =>
            {
                var errorMessage = _clientSearchPresenter.ValidateSearchData();
                if (!string.IsNullOrEmpty(errorMessage))
                {
                    helpers.alert(Enumerator.alert.warning, errorMessage);
                    return;
                }
                await _clientSearchPresenter.LoadClients();
            });
        }

        private void clientDataGridView_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            ExecuteWithWait(() =>
            {
                if (e.RowIndex < 0) return;
                var selectedRow = clientDataGridView.Rows[e.RowIndex];
                if (!(selectedRow.DataBoundItem is CRMClientData cd)) return;
                _focusedClient = cd;
                DialogResult = DialogResult.OK;
            });
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            ExecuteWithWait(() => _clientSearchPresenter.ClearForm());
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            ExecuteWithWait(() => DialogResult = DialogResult.OK);
        }

        private void clientDataGridView_SelectionChanged(object sender, EventArgs e)
        {
            ExecuteWithWait(() =>
            {
                if (clientDataGridView.SelectedRows.Count != 1) return;
                var selectedRow = clientDataGridView.SelectedRows[0];
                if (!(selectedRow.DataBoundItem is CRMClientData cd)) return;
                _focusedClient = cd;
                _clientSearchPresenter.EnableControls();
            },false);
        }
    }
}
