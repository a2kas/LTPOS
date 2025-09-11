using System.Threading.Tasks;

namespace POS_display.Repository.SalesOrder
{
    public interface ISalesOrderRepository
    {
        Task<bool> GetSalesOrderProduct(long productID);
        Task<decimal> ImportToPharmacy(decimal productID, decimal qty, decimal kasClientID);
        Task DeleteTransferData(decimal hid);
        Task<string> GetProductName(long productID);
    }
}
