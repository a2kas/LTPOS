using System.Threading.Tasks;

namespace POS_display.Presenters.Barcode
{
    public interface IBarcodePresenter
    {
        Task ScanBarcode();

        Task GetDataFromBarcode();
    }
}
