namespace POS_display.Views
{
    public interface IBarcodeView
    {
        string BarcodeString { get; set; }
        bool Display2Visible { get; set; }
        int display2_timer { get; set; }
        Items.posh PoshItem { get; set; }
    }
}
