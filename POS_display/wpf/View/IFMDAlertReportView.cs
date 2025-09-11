using System.Windows.Controls;
using System.Windows.Threading;

namespace POS_display.wpf.View
{
    public interface IFMDAlertReportView
    {
        FlowDocumentScrollViewer DocumentScrollViewer { get; set; }
    }
}
