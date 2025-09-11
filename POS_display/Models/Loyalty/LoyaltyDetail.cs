using Dapper.ColumnMapper;
using System.Data.Linq.Mapping;

namespace POS_display.Models.Loyalty
{
    [Table(Name = "LoyaltyD")]
    public class LoyaltyDetail
    {
        [ColumnMapping("posh_id")]
        public long PoshID { get; set; }

        [ColumnMapping("posd_id")]
        public long PosdID { get; set; }

        [ColumnMapping("loyalty_type")]
        public string Type { get; set; }

        [ColumnMapping("sum_type")]
        public Enumerator.SumType SumType { get; set; }

        [ColumnMapping("sum")]
        public decimal Sum { get; set; }

        [ColumnMapping("description")]
        public string Description { get; set; }
    }
}