using System.Windows.Controls;

namespace POS_display.wpf.View
{
    /// <summary>
    /// Interaction logic for FMDAlertReport.xaml
    /// </summary>
    public partial class FMDAlertReport : UserControl, IFMDAlertReportView
    {
        public FMDAlertReport()
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
