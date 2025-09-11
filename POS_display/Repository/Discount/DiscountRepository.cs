using Dapper;
using POS_display.Models.Discount;
using POS_display.Utils.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace POS_display.Repository.Discount
{
    public class DiscountRepository : BaseRepository, IDiscountRepository
    {
        #region Members
        private static List<DiscountH> _discountCategories = null;
        private static List<DiscountType> _discountTypes1 = null;
        private static List<DiscountType> _discountTypes2 = null;
        private static List<DiscountD> _discounts = null;
        #endregion

        public async Task<List<DiscountH>> GetDiscountCategories()
        {
            using (var connection = DB_Base.GetConnection())
            {
                if (_discountCategories != null)
                    return _discountCategories;

                var list = await connection.QueryAsync<DiscountH>(DiscountQueries.GetDiscountH);
                _discountCategories = list.ToList();
                return _discountCategories;
            }
        }

        public async Task<List<DiscountType>> GetDiscountTypes1()
        {
            return await Task.FromResult(new List<DiscountType>
            {
                new DiscountType() { DiscType = "Prekei", Type = "2" },
                new DiscountType() { DiscType = "Kvitui", Type = "1" },
            });
        }

        public async Task<List<DiscountType>> GetDiscountTypes2(decimal hid, string perfix)
        {
            using (var connection = DB_Base.GetConnection())
            {
                if (_discountTypes2 == null)
                {
                    var list = await connection.QueryAsync<DiscountType>(DiscountQueries.GetDiscountType2);
                    _discountTypes2 = list.ToList();
                }
                return _discountTypes2.Where(e => e.Hid == hid && e.Perfix == perfix).ToList();
            }
        }

        public async Task<List<DiscountD>> GetDiscounts(string discountCategory, string perfix)
        {
            using (var connection = DB_Base.GetConnection())
            {
                if (_discounts == null)
                {
                    var list = await connection.QueryAsync<DiscountD>(DiscountQueries.GetDiscountD);
                    _discounts = list.ToList();
                }
                return _discounts.Where(e => e.Type == discountCategory.ToCharArray()[0] && e.Perfix == perfix).ToList();
            }
        }

        public async Task<bool> CreateDiscount(decimal HID, decimal ID, decimal type1, decimal type2, decimal discount_sum, string discount_type)
        {
            using (var connection = DB_Base.GetConnection())
            {
                await connection.QueryAsync(DiscountQueries.CreateDiscount,
                    new
                    {
                        hid = HID,
                        id = ID,
                        type1,
                        type2,
                        discount_sum,
                        discount_type
                    });
                return true;
            }
        }
    }
}
