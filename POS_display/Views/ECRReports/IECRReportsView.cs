using System.Windows.Forms;

namespace POS_display.Views.ECRReports
{
    public interface IECRReportsView
    {
        DateTimePicker DateFrom { get; set; }
        DateTimePicker DateTo { get; set; }
        DateTimePicker SetDate { get; set; }
        DateTimePicker SetTime { get; set; }
        TextBox Change { get; set; }
        Button Calc { get; set; }
        ComboBox Report { get; set; }
        DialogResult FormDialogResult { get; set; }
    }
}
