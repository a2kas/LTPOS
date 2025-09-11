using Dapper;
using POS_display.Models.HomeMode;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace POS_display.Repository.HomeMode
{
    public class HomeModeRepository : BaseRepository, IHomeModeRepository
    {
        public async Task<decimal> CreateHomeDeliveryOrder(decimal posHeaderId, decimal partnerId, string signature)
        {
            using (var connection = DB_Base.GetConnection())
            {
                return await connection.QueryFirstAsync<decimal>(HomeModeQueries.CreateHomeDeliveryOrder,
                    new 
                    {
                        posHeaderId,
                        partnerId,
                        signature 
                    });
            }
        }

        public async Task DeleteHomeDeliveryOrder(decimal posHeaderId)
        {
            using (var connection = DB_Base.GetConnection())
            {
                await connection.ExecuteAsync(HomeModeQueries.DeleteHomeDeliveryOrder, new { posHeaderId  });
            }
        }

        public async Task SetOrderIdToHomeDeliveryOrder(decimal posHeaderId, decimal OrderId)
        {
            using (var connection = DB_Base.GetConnection())
            {
                await connection.ExecuteAsync(HomeModeQueries.SetOrderIdHomeDeliveryOrder, new { posHeaderId, OrderId });
            }
        }

        public async Task SetClientOrderNoToHomeDeliveryOrder(decimal posHeaderId, string clientOrderNo)
        {
            using (var connection = DB_Base.GetConnection())
            {
                await connection.ExecuteAsync(HomeModeQueries.SetClientOrderNoToHomeDeliveryOrder, new { posHeaderId, clientOrderNo });
            }
        }

        public async Task<decimal> CreateOrder()
        {
            using (var connection = DB_Base.GetConnection())
            {
                return await connection.QueryFirstOrDefaultAsync<decimal>(HomeModeQueries.CreateOrderHeader);
            }
        }

        public async Task<decimal> CreateOrderLine(decimal orderHeaderId, decimal productId, decimal qty, string supplierItemId, decimal supplierId)
        {
            using (var connection = DB_Base.GetConnection())
            {
                return await connection.QueryFirstOrDefaultAsync<decimal>(HomeModeQueries.CreateOrderLine,
                    new 
                    { 
                        orderHeaderId,
                        productId,
                        qty,
                        supplierItemId,
                        supplierId
                    });
            }
        }

        public async Task SetOrderName(decimal orderId, string orderName)
        {
            using (var connection = DB_Base.GetConnection())
            {
                await connection.ExecuteAsync(HomeModeQueries.SetOrderName, new { orderId, orderName });
            }
        }

        public async Task FormOrder(decimal orderHeaderId)
        {
            using (var connection = DB_Base.GetConnection())
            {
                await connection.QueryFirstOrDefaultAsync<decimal>(HomeModeQueries.FormOrder, new { orderHeaderId });
            }
        }

        public async Task CommitOrder(decimal orderHeaderId, decimal supplierId)
        {
            using (var connection = DB_Base.GetConnection())
            {
                await connection.QueryFirstOrDefaultAsync<decimal>(HomeModeQueries.CommitOrder, new { orderHeaderId, supplierId });
            }
        }

        public async Task<string> GetOrderNameById(decimal orderHeaderId)
        {
            using (var connection = DB_Base.GetConnection())
            {
                return await connection.QueryFirstOrDefaultAsync<string>(HomeModeQueries.GetOrderNameById, new { orderHeaderId });
            }
        }

        public async Task<decimal> GetTamroItemIdByProductId(decimal productId)
        {
            using (var connection = DB_Base.GetConnection())
            {
                return await connection.QueryFirstOrDefaultAsync<decimal>(HomeModeQueries.GetTamroItemIdByProductId, new { productId });
            }
        }

        public async Task<decimal> GetPartnerIdByPosHeaderId(decimal posHeaderId)
        {
            using (var connection = DB_Base.GetConnection())
            {
                return await connection.QueryFirstOrDefaultAsync<decimal>(HomeModeQueries.GetPartnerIdByPosHeaderId, new { posHeaderId });
            }
        }

        public async Task<decimal> GetClientOrderSequenceNo()
        {
            using (var connection = DB_Base.GetConnection())
            {
                return await connection.QueryFirstOrDefaultAsync<decimal>(HomeModeQueries.GetClientSequenceNo);
            }
        }

        public async Task CreateHomeDeliveryDetail(decimal posHeaderId, decimal posDetailId, HomeModeQuantities quantities)
        {
            using (var connection = DB_Base.GetConnection())
            {
                await connection.ExecuteAsync(HomeModeQueries.InsertHomeDeliveryDetail, 
                    new 
                    {
                        posHeaderId, 
                        posDetailId,
                        homeQty = quantities.HomeQuantity,
                        realQty = quantities.RealQuantity,
                        homeQtyByRatio = quantities.HomeQuantityByRatio,
                        realQtyByRatio = quantities.RealQuantityByRatio,
                    });
            }
        }

        public async Task DeleteHomeDeliveryDetail(decimal posHeaderId, decimal posDetailId)
        {
            using (var connection = DB_Base.GetConnection())
            {
                await connection.ExecuteAsync(HomeModeQueries.DeleteHomeDeliveryDetail, new { posHeaderId, posDetailId });
            }
        }

        public async Task<List<HomeModeOrderDetail>> GetHomeDeliveryDetails(decimal posHeaderId)
        {
            using (var connection = DB_Base.GetConnection())
            {
                return (await connection.QueryAsync<HomeModeOrderDetail>(HomeModeQueries.GetHomeDeliveryDetails, new { posHeaderId })).ToList();
            }            
        }

        public async Task<List<HomeDeliveryDetailSummarized>> GetHomeDeliveryDetailSummarized(decimal posHeaderId)
        {
            using (var connection = DB_Base.GetConnection())
            {
                return (await connection.QueryAsync<HomeDeliveryDetailSummarized>(HomeModeQueries.GetHomeDeliveryDetailSummarized, new { posHeaderId })).ToList();
            }
        }
    }
}
