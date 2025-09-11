using POS_display.Models.BarcodeData;
using System.Threading.Tasks;

namespace POS_display.Repository.Barcode
{
    public interface IBarcodeRepository
    {
        Task<BarcodeData> GetBarcodeData(string barcode);

        Task<bool> HasPriceChange(long productID);

        Task<decimal> GetNpakId7ByProductId(decimal productId);

        Task<string> FindBarcodeByProductId(decimal productId);
    }
}
