using POS_display.Models.HomeMode;
using System.Windows.Forms;

namespace POS_display.Views.HomeMode
{
    public interface IHomeModeQuantityView
    {
        HomeModeQuantities GetQuantites();

        TextBox RealQuantity { get; set; }

        TextBox HomeQuantity { get; set; }
    }
}
