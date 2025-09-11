using System.Windows.Forms;

namespace POS_display.Views.HomeMode
{
    public interface IHomeModeAcitvateView
    {
        TextBox BuyerName { get; set; }

        TextBox Address { get; set; }

        TextBox City { get; set; }

        TextBox PhoneNumber { get; set; }

        TextBox Email { get; set; }

        TextBox PostIndex { get; set; }

        TextBox CountryCode { get; set; }

        Button StartProcess { get; }

        Button SendToTerminal { get; }
    }
}
