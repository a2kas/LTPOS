using Dapper.ColumnMapper;
using System.Data.Linq.Mapping;

namespace POS_display.Models.Loyalty
{
    [Table(Name = "LoyaltyH")]
    public class LoyaltyHeader
    {
        [ColumnMapping("posh_id")]
        public long PoshID { get; set; }

        [ColumnMapping("card_type")]
        public string CardType { get; set; }

        [ColumnMapping("card_no")]
        public string CardNo { get; set; }

        [ColumnMapping("active")]
        public decimal Active { get; set; }

        [ColumnMapping("status")]
        public decimal Status { get; set; }

        [ColumnMapping("manual_vouchers")]
        public string ManualVouchers { get; set; }

        [ColumnMapping("accrue_points")]
        public decimal AccruePoints { get; set; }

        [ColumnMapping("counter")]
        public decimal Counter { get; set; }
    }
}