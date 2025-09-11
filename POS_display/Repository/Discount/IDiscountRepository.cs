using POS_display.Models.Discount;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace POS_display.Repository.Discount
{
    public interface IDiscountRepository
    {
        Task<List<DiscountH>> GetDiscountCategories();
        Task<List<DiscountType>> GetDiscountTypes1();
        Task<List<DiscountType>> GetDiscountTypes2(decimal hid, string perfix);
        Task<List<DiscountD>> GetDiscounts(string discountCategory, string perfix);
        Task<bool> CreateDiscount(decimal HID, decimal ID, decimal type1, decimal type2, decimal discount_sum, string discount_type);
    }
}