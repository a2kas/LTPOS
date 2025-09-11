using POS_display.Models.Discount;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace POS_display.Repository.Price
{
    public interface IPriceRepository
    {
        Task<decimal> GetSalesPriceWithDiscount(decimal pid);
        Task<decimal> GetCompPriceWithDiscount(decimal pid);
        Task<decimal> GetSalesPriceComp(decimal pid, decimal compensation_amount, string priceClass);
        Task<string> GetATCCode(decimal pid);
        Task<decimal> SearchProductQty(decimal pid);
        Task<decimal> GetVatFromStock(decimal pid);
        Task<decimal?> GetProductQty(long pid);
        Task<decimal?> GetProductRatio(decimal productId);
    }
}