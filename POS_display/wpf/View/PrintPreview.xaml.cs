using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

namespace POS_display.wpf.View
{
    /// <summary>
    /// Interaction logic for PrintPreview.xaml
    /// </summary>
    public partial class PrintPreview : UserControl
    {
        public string Description { get; set; }

        public PrintPreview(FlowDocument document)
        {
            InitializeComponent();
            FlowDocumentView.Document = document;
        }

        private void PrintDocument_Click(object sender, RoutedEventArgs e)
        {
            PrintDialog printDlg = new PrintDialog();
            FlowDocumentView.Document.Name = Description.Replace(" ","");
            IDocumentPaginatorSource idpSource = FlowDocumentView.Document;
            printDlg.PrintDocument(idpSource.DocumentPaginator, Description);
            (DataContext as ViewModel.BaseViewModel)?.CloseCommand?.Execute(null);
        }
    }

}
