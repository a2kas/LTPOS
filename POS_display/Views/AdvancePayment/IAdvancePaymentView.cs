using System.Windows.Forms;

namespace POS_display.Views.AdvancePayment
{
    public interface IAdvancePaymentView
    {
        string SelectedAdvancePaymentType { get; }

        ComboBox AdvancePaymentType { get; set; }

        TextBox OrderNumber { get; set; }

        TextBox AdvanceSum { get; set; }

        Button Close { get; set; }

        Items.posh PosHeader  { get; }
    }
}
