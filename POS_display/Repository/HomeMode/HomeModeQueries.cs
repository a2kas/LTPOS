namespace POS_display.Repository.HomeMode
{
    public static class HomeModeQueries
    {
        public static string CreateHomeDeliveryOrder => @"SELECT create_home_delivery_order(@posHeaderId, @partnerId, @signature)";

        public static string DeleteHomeDeliveryOrder => @"UPDATE home_delivery_order SET deleted = 1 WHERE posh_id = @posHeaderId";

        public static string SetOrderIdHomeDeliveryOrder => @"UPDATE home_delivery_order SET order_id = @orderId WHERE posh_id = @posHeaderId";

        public static string SetClientOrderNoToHomeDeliveryOrder => @"UPDATE home_delivery_order SET client_order_no = @clientOrderNo WHERE posh_id = @posHeaderId";

        public static string CreateOrderHeader => @"SELECT create_orderh();";

        public static string CreateOrderLine => @"SELECT update_orderd2(@orderHeaderId, @productId, @qty, @supplierItemId, @supplierId);";

        public static string FormOrder => @"SELECT form_order(@orderHeaderId, 1);";

        public static string CommitOrder => @"SELECT commit_orderh(@orderHeaderId, @supplierId, 0, 0);";

        public static string GetOrderNameById => @"SELECT descrip FROM orderh WHERE id = @orderHeaderId";

        public static string GetTamroItemIdByProductId => @"SELECT id FROM purchimport WHERE productid = @productId AND partner = 10000000281 LIMIT 1";

        public static string SetSupplierIdToOrderLines => @"UPDATE orderd SET suplier_id = @supplierId WHERE hid = @orderHeaderId";

        public static string GetPartnerIdByPosHeaderId => "SELECT partner_id FROM home_delivery_order WHERE posh_id = @posHeaderId";

        public static string GetClientSequenceNo => "SELECT nextval('sq_client_order_no');";

        public static string SetOrderName => "UPDATE orderh SET descrip = @orderName WHERE id = @orderId";

        public static string InsertHomeDeliveryDetail => "INSERT INTO home_delivery_order_details VALUES (@posHeaderId, @posDetailId, @realQty, @homeQty, @realQtyByRatio, @homeQtyByRatio);";

        public static string DeleteHomeDeliveryDetail => "DELETE FROM home_delivery_order_details WHERE posh_id = @posHeaderId AND posd_id = @posDetailId;";

        public static string GetHomeDeliveryDetails => @"SELECT posh_id AS PosHeaderId, 
                                                                posd_id AS PosDetailId,
                                                                home_qty AS HomeQty,
                                                                real_qty AS RealQty,
                                                                home_qty_by_ratio AS HomeQtyByRatio,
                                                                real_qty_by_ratio AS RealQtyByRatio
                                                        FROM home_delivery_order_details WHERE posh_id = @posHeaderId";

        public static string GetHomeDeliveryDetailSummarized => @"SELECT pd.productid AS ProductId, SUM(hdod.home_qty) AS HomeQty FROM home_delivery_order_details hdod
                                                                LEFT JOIN posd pd ON pd.id = hdod.posd_id
                                                               WHERE hdod.posh_id = @posHeaderId GROUP BY pd.productid";
    }
}
