namespace POS_display.Repository.Discount
{
    public static class DiscountQueries
    {
        public static string GetDiscountH => @"SELECT * FROM discounth WHERE valid_from <= current_date AND valid_to >= current_date ORDER BY name;";

        public static string GetDiscountType2 =>
            @"SELECT DISTINCT dd.Hid, dd.Type,
                             CASE 
                             WHEN type='1' THEN 'Procentinė'
                             WHEN type='2' THEN 'Suminė' 
                             WHEN type='3' THEN 'Suminė+pvm' 
                             WHEN type='4' THEN 'Procentinė' 
                             END AS DiscType,
                             dh.perfix
                             FROM discounth dh
                             JOIN discountd dd ON dh.id = dd.hid;";

        public static string GetDiscountD => "SELECT dd.Hid, dd.Type, dd.Value, dh.perfix FROM discounth dh JOIN discountd dd ON dh.id = dd.hid";

        public static string CreateDiscount => "SELECT create_discount(@hid, @id, @type1, @type2, @discount_sum, @discount_type)";
    }
}
