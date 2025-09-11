using POS_display.Models.Discount;
using System.Collections.Generic;
using System.Windows.Forms;

namespace POS_display.Views.Discount
{
    public interface IDiscountView
    {
        IList<DiscountH> DiscountCategories { get; set; }
        IList<DiscountType> DiscountTypes1 { get; set; }
        IList<DiscountType> DiscountTypes2 { get; set; }
        DiscountH SelectedDiscountCategory { get; set; }
        DiscountType SelectedDiscountType2 { get; set; }
        DiscountType SelectedDiscountType1 { get; set; }
        TextBox CardNoTextBox { get; set; } 
        Button CalcButton { get; set; }
        ComboBox DiscountSum { get; set; }
    }
}
