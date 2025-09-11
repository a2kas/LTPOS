using POS_display.Models.HomeMode;
using POS_display.Presenters.HomeMode;
using POS_display.Repository.Price;
using System.Windows.Forms;

namespace POS_display.Views.HomeMode
{
    public partial class HomeModeQuantityView : FormBase, IHomeModeQuantityView
    {
        #region Members
        private readonly IHomeModeQuantityPresenter _homeModeQuantityPresenter;
        private HomeModeQuantities _homeModeQuantities;
        private decimal _homeQtyByRatio;
        #endregion

        #region Constructor
        public HomeModeQuantityView(Items.Prices.GenericItem selectedItem)
        {
            InitializeComponent();
            _homeModeQuantityPresenter = new HomeModeQuantityPresenter(this,
                new PriceRepository(),
                selectedItem);
        }
        #endregion

        #region Properties
        public TextBox RealQuantity
        {
            get => tbRealQty;
            set => tbRealQty = value;
        }

        public TextBox HomeQuantity
        {
            get => tbHomeQty;
            set => tbHomeQty = value;
        }
        #endregion

        #region Public methods
        public HomeModeQuantities GetQuantites()
        {
            return _homeModeQuantities;
        }
        #endregion

        #region Private methods
        private async void btnSubmit_Click(object sender, System.EventArgs e)
        {
            await ExecuteWithWaitAsync(async () =>
            {
                await _homeModeQuantityPresenter.Validate();
                _homeModeQuantities = await _homeModeQuantityPresenter.ResolveQuantities();

                DialogResult = DialogResult.OK;
            });
        }
        #endregion
    }
}
