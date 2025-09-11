namespace POS_display.Repository.Loyalty
{
    public static class LoyaltyQueries
    {
        public static string CreateLoyaltyH => "SELECT create_loyaltyh(@posh_id, @type, @card_no, @active, @status, @accrue_points)";
        public static string CreateLoyaltyD => "SELECT create_loyaltyd(@posh_id, @posd_id, @type, @sum_type, @sum, @description)";
        public static string DeleteLoyaltyD => "SELECT delete_loyaltyd(@posh_id, @posd_id)";
        public static string ChangeLoyaltyHeaderStatus => "UPDATE loyaltyh SET active=@active, status=@status, counter=counter+@status WHERE posh_id=@posh_id)";
        public static string GetCounter => "SELECT counter FROM loyaltyh WHERE posh_id=@posh_id";
        public static string SetInstanceDiscount => "UPDATE loyaltyh SET accrue_points=@accrue_points WHERE posh_id=@posh_id";
        public static string SetManualVouchers => "UPDATE loyaltyh SET manual_vouchers=@vouchers WHERE posh_id=@posh_id";
        public static string GetLoyaltyh => "SELECT * FROM loyaltyh WHERE posh_id=@posh_id";
        public static string DeleteLoyaltyDetailsByPosHeaderIdAndTypes => "DELETE FROM loyaltyd WHERE posh_id = {0} AND loyalty_type IN ({1})";
        public static string DeleteLoyaltyDetailsByPosHeaderId => "DELETE FROM loyaltyd WHERE posh_id = @posh_id";
        public static string GetLoyaltyDetailsByPosHeaderId => "SELECT * FROM loyaltyd WHERE posh_id = @posh_id";
        public static string CreateOrUpdateLoyaltyDetail => "SELECT create_update_loyaltyd(@posh_id, @posd_id, @type, @sum_type, @sum, @description)";
    }
}
