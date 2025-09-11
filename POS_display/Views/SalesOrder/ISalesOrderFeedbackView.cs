using POS_display.Helpers;
using System.Windows.Forms;

namespace POS_display.Views.SalesOrder
{
    public interface ISalesOrderFeedbackView
    {
        Button ButtonApprove { get; set; }
        Button ButtonCancel { get; set; }
        LoaderUserControl LoaderControl { get; }
        Label InfoField { get; set; }
    }
}
