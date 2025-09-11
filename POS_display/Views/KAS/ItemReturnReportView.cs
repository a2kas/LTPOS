using POS_display.Models.KAS;
using POS_display.Presenters.KAS;
using POS_display.Repository.Pos;
using System;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace POS_display.Views.KAS
{
    public partial class ItemReturnReportView : FormBase, IItemReturnReportView, IBaseView
    {
        #region Members
        private readonly IItemReturnReportPresenter _itemReturnReportPresenter;
        private ReturningItemsData _returningItemsData;
        #endregion

        #region Constructor
        public ItemReturnReportView(ReturningItemsData returningItemsData)
        {
            InitializeComponent();
            _itemReturnReportPresenter =  new ItemReturnReportPresenter(this, new PosRepository(), Session.FP550);
            _returningItemsData = returningItemsData;
        }
    
        #endregion

        #region Properties
        public Label PharmacyNo 
        {
            get { return lblPharmacyNo; }
            set { lblPharmacyNo = value; }
        }

        public Button PrintButton 
        {
            get { return btnPrint; }
            set { btnPrint = value; }
        }

        public Button CloseButton
        {
            get { return btnClose; }
            set { btnClose = value; }
        }

        public TextBox Address
        {
            get { return tbAddress; }
            set { tbAddress = value; }
        }

        public TextBox CashDeskNr
        {
            get { return tbCashDeskNr; }
            set { tbCashDeskNr = value; }
        }

        public TextBox ChequeNr
        {
            get { return tbChequeNr; }
            set { tbChequeNr = value; }
        }

        public TextBox InsuranceCompany
        {
            get { return tbInsuranceCompany; }
            set { tbInsuranceCompany = value; }
        }

        public TextBox Cashier 
        {
            get { return tbCashier; }
            set { tbCashier = value; }
        }

        public TextBox Buyer
        {
            get { return tbBuyer; }
            set { tbBuyer = value; }
        }

        public TextBox ReturnSum
        {
            get { return tbReturnSum; }
            set { tbReturnSum = value; }
        }

        public TextBox Date
        {
            get { return tbDate; }
            set { tbDate = value; }
        }

        public DataGridView ReturningItem 
        {
            get { return dgvReturningItems; }
            set { dgvReturningItems = value; }
        }
        public async Task Init() 
        {
            await ExecuteWithWaitAsync(async () => await _itemReturnReportPresenter.Init(_returningItemsData));
        }

        public async Task SendItemsReturnToCashRegister()
        {
            await ExecuteWithWaitAsync(async () => await _itemReturnReportPresenter.SendItemsReturnToCashRegister());
        }

        public async Task<decimal> GetRoundingValue() 
        {
            if (_returningItemsData == null)
                return 0;

            return await _itemReturnReportPresenter.CalculateRoundingValue();
        }
        
        #endregion

        #region Private methods
        private async void btnPrint_Click(object sender, EventArgs e)
        {
            ExecuteWithWait(() =>_itemReturnReportPresenter.PerformPrint());
            await ExecuteWithWaitAsync(async () => await _itemReturnReportPresenter.SendItemsReturnToCashRegister());
            Close();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void dgvReturningItems_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            if (e.ColumnIndex < 0 ) return;
            DataGridViewColumn column = dgvReturningItems.Columns[e.ColumnIndex];
            if (column.Name != colName.Name && !decimal.TryParse(Convert.ToString(e.FormattedValue), out _))
            {
                e.Cancel = true;
                helpers.alert(Enumerator.alert.error, "Leidžiama įvesti tik skaitinę reikšmę!");
            }
        }
        #endregion

        private async void dgvReturningItems_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            await ExecuteWithWaitAsync(async () => await _itemReturnReportPresenter.RefreshData());
        }
    }
}
