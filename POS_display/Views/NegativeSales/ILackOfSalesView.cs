using POS_display.Models.Pos;
using System.Collections.Generic;

namespace POS_display.Views.NegativeSales
{
    public interface ILackOfSalesView
    {
        List<LackOfSale> LackOfSales { get; set; }
    }
}
