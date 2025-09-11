using POS_display.wpf.View.KAS;
using System.Windows.Controls;

namespace POS_display.wpf.View
{
    /// <summary>
    /// Interaction logic for FMDAlertReport.xaml
    /// </summary>
    public partial class ItemReturnReport : UserControl, IItemReturnReportView
    {
        public ItemReturnReport()
        {
            InitializeComponent();
        }

        public FlowDocumentScrollViewer DocumentScrollViewer 
        {
            get { return FlowDocumentView; }
            set { FlowDocumentView = value; }
        }
    }
}
