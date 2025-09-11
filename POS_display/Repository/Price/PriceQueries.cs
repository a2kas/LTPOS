namespace POS_display.Repository.Price
{
    public static class PriceQueries
    {
        public static string GetSalesPriceWithDiscount => @"SELECT get_sales_price_with_discount(@ID, -1)";

        public static string GetCompPriceWithDiscount => string.Format(@"SELECT COALESCE((
                                            SELECT {0} AS pk FROM kas_pricelist k WHERE k.productid=@ID 
                                            AND TRUNC(NOW()) BETWEEN k.validfrom AND k.validtill AND k.pl_type!=2 AND price>0 
                                            ORDER BY k.pl_type DESC, k.confirmationdate DESC, k.hid DESC, k.id DESC LIMIT 1
                                            ), 0.00)", Session.PriceClass);

        public static string GetSalesPriceComp => @"SELECT get_sales_price_comp(@id, @compensation_amount, @priceClass)";

        public static string GetATCCode => @"SELECT code FROM atc WHERE atc.id = (SELECT atcid FROM stock WHERE id = @id)";

        public static string SearchQty => @"SELECT qty2 FROM (SELECT DISTINCT ON (productid, storeid) * FROM search_quantity2 aaa) bbb WHERE productid = @productid AND storeid = @storeid limit 1";

        public static string GetVatFromStock => @"SELECT vatsize FROM taxes WHERE id = (SELECT s.salesvatid FROM stock s WHERE s.id=@id);";

        public static string GetSalesPrice => @"SELECT get_sales_price(@productid)";

        public static string GetProductQty => @"SELECT get_product_qty(@productid)";

        public static string GetProductRatio => @"SELECT u.ratio FROM unit u LEFT JOIN barcode b ON b.unitid = u.id WHERE b.productid = @productId LIMIT 1";

    }
}
