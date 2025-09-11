using POS_display.Models.HomeMode;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace POS_display.Repository.HomeMode
{
    public interface IHomeModeRepository
    {
        Task<decimal> CreateHomeDeliveryOrder(decimal posHeaderId, decimal partnerId, string signature);

        Task DeleteHomeDeliveryOrder(decimal posHeaderId);

        Task SetOrderIdToHomeDeliveryOrder(decimal posHeaderId, decimal OrderId);

        Task SetClientOrderNoToHomeDeliveryOrder(decimal posHeaderId, string clientOrderNo);

        Task<decimal> CreateOrder();

        Task<decimal> CreateOrderLine(decimal orderHeaderId, decimal productId, decimal qty, string supplierItemId, decimal supplierId);

        Task SetOrderName(decimal orderId, string orderName);

        Task FormOrder(decimal orderHeaderId);

        Task CommitOrder(decimal orderHeaderId, decimal supplierId);

        Task<string> GetOrderNameById(decimal orderHeaderId);

        Task<decimal> GetTamroItemIdByProductId(decimal productId);

        Task<decimal> GetPartnerIdByPosHeaderId(decimal posHeaderId);

        Task<decimal> GetClientOrderSequenceNo();

        Task CreateHomeDeliveryDetail(decimal posHeaderId, decimal posDetailId, HomeModeQuantities quantities);

        Task DeleteHomeDeliveryDetail(decimal posHeaderId, decimal posDetailId);

        Task<List<HomeModeOrderDetail>> GetHomeDeliveryDetails(decimal posHeaderId);
        Task<List<HomeDeliveryDetailSummarized>> GetHomeDeliveryDetailSummarized(decimal posHeaderId);
    }
}
