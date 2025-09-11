namespace POS_display.Repository.SalesOrder
{
    public static class SalesOrderQueries
    {
        public static string IsSalesOrderProduct => "SELECT COUNT(productid) <> 0 FROM sales_order_product WHERE productid = @productid";
        public static string ImportToPharmacy => "SELECT import_to_pharmacy (@productID, @qty, @kasClientID)";
        public static string DeleteStockDocument => "SELECT delete_stockh_force(@hid)";
        public static string GetProductName => "SELECT name FROM barcode WHERE productid = @productid";
        
    }
}
