using Microsoft.Extensions.DependencyInjection;
using POS_display.Presenters.AdvancePayment;
using POS_display.Repository.Pos;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Forms;
using Tamroutilities.Client;

namespace POS_display.Views.AdvancePayment
{
    public partial class AdvancePaymentView : FormBase, IAdvancePaymentView
    {
        #region Members
        private readonly Items.posh _posHeader;
        private readonly IAdvancePaymentPresenter _advancePaymentPresenter;
        #endregion

        #region Constructor
        public AdvancePaymentView(Items.posh posHeader)
        {
            InitializeComponent();
            _advancePaymentPresenter = new AdvancePaymentPresenter(this,
                new PosRepository(),
                Program.ServiceProvider.GetRequiredService<ITamroClient>());
            _posHeader = posHeader;
        }
        #endregion

        #region Properties
        public string SelectedAdvancePaymentType
        {
            get { return ((KeyValuePair<string, string>)AdvancePaymentType.SelectedItem).Key; }
        }

        public ComboBox AdvancePaymentType 
        {
            get { return cbAdvancePaymentType; }
            set { cbAdvancePaymentType = value;  }
        }

        public TextBox OrderNumber
        {
            get { return tbOrderNumber; }
            set { tbOrderNumber = value; }
        }

        public TextBox AdvanceSum
        {
            get { return tbAdvanceSum; }
            set { tbAdvanceSum = value; }
        }

        public Button Close
        {
            get { return btnClose; }
            set { btnClose = value; }
        }

        public Items.posh PosHeader
        {
            get { return _posHeader; }
        }
        #endregion

        #region Public methods
        public async Task Init()
        {
            await ExecuteWithWaitAsync(async () => await _advancePaymentPresenter.Init());
        }
        #endregion

        #region Private methods
        private void btnClose_Click(object sender, System.EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }
        #endregion

        private void cbAdvancePaymentType_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            ExecuteWithWait(() => _advancePaymentPresenter.EnableControls());
        }

        private async void tbOrderNumber_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                await ExecuteWithWaitAsync(async () => await _advancePaymentPresenter.Confirm());
                await ExecuteWithWaitAsync(async () => await Program.Display1.RefreshPosh());
            }
        }
    }
}
