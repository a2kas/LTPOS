using Dapper;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace POS_display.Repository.Price
{
    public class PriceRepository : BaseRepository, IPriceRepository
    {
        public async Task<decimal> GetSalesPriceWithDiscount(decimal pid)
        {
            using (var connection = DB_Base.GetConnection())
            {
                return await connection.QueryFirstAsync<decimal>(PriceQueries.GetSalesPriceWithDiscount, new { id = pid });
            }
        }

        public async Task<decimal> GetCompPriceWithDiscount(decimal pid)
        {
            using (var connection = DB_Base.GetConnection())
            {
                return await connection.QueryFirstAsync<decimal>(PriceQueries.GetCompPriceWithDiscount, new { id = pid });
            }
        }

        public async Task<decimal> GetSalesPriceComp(decimal pid, decimal compensation_amount, string priceClass)
        {
            using (var connection = DB_Base.GetConnection())
            {
                return await connection.QueryFirstAsync<decimal>(PriceQueries.GetSalesPriceComp, new { id = pid, compensation_amount, priceClass });
            }
        }

        public async Task<string> GetATCCode(decimal pid)
        {
            using (var connection = DB_Base.GetConnection())
            {
                return await connection.QueryFirstAsync<string>(PriceQueries.GetATCCode, new { id = pid });
            }
        }

        public async Task<decimal> SearchProductQty(decimal pid)
        {
            List<decimal> qtyResults;
            using (var connection = DB_Base.GetConnection())
            {
                var result = await connection.QueryAsync<decimal>(PriceQueries.SearchQty, new { productid = pid, storeid = Session.SystemData.storeid });
                qtyResults = result.ToList();
            }
            return qtyResults.Count == 0 ? 0M : qtyResults.FirstOrDefault();
        }

        public async Task<decimal> GetVatFromStock(decimal pid)
        {
            using (var connection = DB_Base.GetConnection())
            {
                return await connection.QueryFirstAsync<decimal>(PriceQueries.GetVatFromStock, new { id = pid });
            }
        }

        public async Task<decimal> GetSalesPrice(long pid)
        {
            using (var connection = DB_Base.GetConnection())
            {
                return await connection.QueryFirstAsync<decimal>(PriceQueries.GetSalesPrice, new { productid = pid });
            }
        }

        public async Task<decimal?> GetProductQty(long pid)
        {
            using (var connection = DB_Base.GetConnection())
            {
                return await connection.QueryFirstOrDefaultAsync<decimal?>(PriceQueries.GetProductQty, new { productid = pid });
            }
        }

        public async Task<decimal?> GetProductRatio(decimal productId)
        {
            using (var connection = DB_Base.GetConnection())
            {
                return await connection.QueryFirstOrDefaultAsync<decimal?>(PriceQueries.GetProductRatio, new { productid = productId });
            }
        }
    }
}
