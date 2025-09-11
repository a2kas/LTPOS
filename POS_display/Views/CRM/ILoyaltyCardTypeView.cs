using System.Windows.Forms;

namespace POS_display.Views.CRM
{
    public interface ILoyaltyCardTypeView
    {
        Button Create { get; set; }

        RadioButton LoyaltyCard { get; set; }

        RadioButton B2BCard { get; set; }
    }
}
