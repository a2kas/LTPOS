using Dapper;
using System.Threading.Tasks;

namespace POS_display.Repository.SalesOrder
{
    public class SalesOrderRepository : BaseRepository, ISalesOrderRepository
    {
        public async Task<bool> GetSalesOrderProduct(long productID)
        {
            using (var connection = DB_Base.GetConnection())
            {
                return await connection.QueryFirstOrDefaultAsync<bool>(SalesOrderQueries.IsSalesOrderProduct, new { productid = productID });
            }
        }

        public async Task<decimal> ImportToPharmacy(decimal productID, decimal qty, decimal kasClientID)
        {
            using (var connection = DB_Base.GetConnection())
            {
                return await connection.QueryFirstAsync<decimal>(SalesOrderQueries.ImportToPharmacy, new
                {
                    productID,
                    qty,
                    kasClientID
                });
            }
        }

        public async Task DeleteTransferData(decimal hid)
        {
            using (var connection = DB_Base.GetConnectionWithoutCommandTimeout())
            {
                await connection.ExecuteAsync(SalesOrderQueries.DeleteStockDocument, new { hid });
            }
        }

        public async Task<string> GetProductName(long productID)
        {
            using (var connection = DB_Base.GetConnection())
            {
                return await connection.QueryFirstOrDefaultAsync<string>(SalesOrderQueries.GetProductName, new { productid = productID });
            }
        }
    }
}
