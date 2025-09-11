using Dapper;
using POS_display.Models.BarcodeData;
using System.Threading.Tasks;

namespace POS_display.Repository.Barcode
{
    public class BarcodeRepository : BaseRepository, IBarcodeRepository
    {
        public async Task<BarcodeData> GetBarcodeData(string barcode)
        {
            using (var connection = DB_Base.GetConnection())
            {
                return await connection.QueryFirstOrDefaultAsync<BarcodeData>(BarcodeQueries.GetBarcodeData, new { barcode });
            }
        }

        public async Task<bool> HasPriceChange(long productID)
        {
            using (var connection = DB_Base.GetConnection())
            {
                return await connection.QueryFirstOrDefaultAsync<bool>(BarcodeQueries.ExistInBasisQuantity, new { productid = productID });
            }
        }

        public async Task<decimal> GetNpakId7ByProductId(decimal productId)
        {
            using (var connection = DB_Base.GetConnection())
            {
                return await connection.QueryFirstOrDefaultAsync<decimal>(BarcodeQueries.GetNpakId7ByProductId, new
                {
                    productId
                });
            }
        }

        public async Task<string> FindBarcodeByProductId(decimal productId)
        {
            using (var connection = DB_Base.GetConnection())
            {
                return await connection.QueryFirstOrDefaultAsync<string>(BarcodeQueries.FindBarcodeByProductId, new
                {
                    productId
                });
            }
        }
    }
}
