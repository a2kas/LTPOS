using POS_display.Models.Pos;
using System.Collections.Generic;

namespace POS_display.Views.NegativeSales
{
    public partial class LackOfSalesView : FormBase, ILackOfSalesView
    {
        #region Constructor
        public LackOfSalesView()
        {
            InitializeComponent();
        }
        #endregion

        #region Properties
        public List<LackOfSale> LackOfSales
        {
            get => lackOfSalesDataGridView.DataSource as List<LackOfSale>;
            set => lackOfSalesDataGridView.DataSource = value;
        }
        #endregion
    }
}
