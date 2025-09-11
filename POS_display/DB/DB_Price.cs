﻿using Npgsql;
using System;
using System.Threading.Tasks;

namespace POS_display
{
    public class DB_prices : DB_Base
    {
        [ObsoleteAttribute("Use same method from 'Price' repository", false)]
        public async Task<decimal> GetSalesPriceWithDiscount(decimal pid)
        {
            NpgsqlCommand cmd = new NpgsqlCommand();
            cmd.CommandText = "SELECT get_sales_price_with_discount(@ID, -1)";
            cmd.Parameters.AddWithValue("@ID", pid);

            return await DoSelectValue<decimal>(cmd);
        }

        [ObsoleteAttribute("Use same method from 'Price' repository", false)]
        public async Task<decimal> GetCompPriceWithDiscount(decimal pid, string pricegroup)
        {
            NpgsqlCommand cmd = new NpgsqlCommand();
            cmd.CommandText = string.Format(@"SELECT COALESCE((
                                            SELECT {0} AS pk FROM kas_pricelist k WHERE k.productid=@ID 
                                            AND TRUNC(NOW()) BETWEEN k.validfrom AND k.validtill AND k.pl_type!=2 AND price>0 
                                            ORDER BY k.pl_type DESC, k.confirmationdate DESC, k.hid DESC, k.id DESC LIMIT 1
                                            ), 0.00)", Session.PriceClass);
            cmd.Parameters.AddWithValue("@ID", pid);

            return await DoSelectValue<decimal>(cmd);
        }

        [ObsoleteAttribute("Use same method from 'Price' repository", false)]
        public async Task<decimal> GetSalesPriceComp(decimal pid, decimal compensation_amount, string priceClass)
        {
            NpgsqlCommand cmd = new NpgsqlCommand();
            cmd.CommandText = @"SELECT get_sales_price_comp(@pid, @compensation_amount, @priceClass);";
            cmd.Parameters.AddWithValue("@pid", pid);
            cmd.Parameters.AddWithValue("@compensation_amount", compensation_amount);
            cmd.Parameters.AddWithValue("@priceClass", priceClass);

            return await DoSelectValue<decimal>(cmd);
        }

        [ObsoleteAttribute("Use same method from 'Price' repository", false)]
        public async Task<string> GetATCCode(decimal pid)
        {
            NpgsqlCommand cmd = new NpgsqlCommand();
            cmd.CommandText = "SELECT code FROM atc WHERE atc.id=(SELECT atcid FROM stock WHERE id=@ID)";
            cmd.Parameters.AddWithValue("@ID", pid);

            return await DoSelectValue<string>(cmd);
        }

        [ObsoleteAttribute("Use same method from 'Price' repository", false)]
        public async Task<decimal> GetQty(decimal pid)
        {
            NpgsqlCommand cmd = new NpgsqlCommand();
            cmd.CommandText = "SELECT qty2 FROM (SELECT DISTINCT ON (productid, storeid) * FROM search_quantity2 aaa)bbb WHERE productid=@productid AND storeid=@storeid limit 1";
            cmd.Parameters.AddWithValue("@productid", pid);
            cmd.Parameters.AddWithValue("@storeid", Session.SystemData.storeid);

            return await DoSelectValue<decimal>(cmd);
        }

        [ObsoleteAttribute("Use same method from 'Price' repository", false)]
        public async Task<decimal> GetVatFromStock(decimal pid)
        {
            NpgsqlCommand cmd = new NpgsqlCommand();
            cmd.CommandText = "SELECT vatsize FROM taxes WHERE id=(SELECT s.salesvatid FROM stock s WHERE s.id=@pid)";
            cmd.Parameters.AddWithValue("@pid", pid);

            return await DoSelectValue<decimal>(cmd);
        }
    }
}