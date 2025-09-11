using Dapper.ColumnMapper;
using System.Data.Linq.Mapping;

namespace POS_display.Models.Pos
{
    [Table(Name = "payment")]
    public class PaymentMethod
    {
        [ColumnMapping("id")]
        public decimal Id { get; set; }

        [ColumnMapping("name")]
        public string Name { get; set; }

        [ColumnMapping("fiscal")]
        public string Fiscal { get; set; }

        [ColumnMapping("fiscal_rank")]
        public int FiscalRank { get; set; }

        [ColumnMapping("rank")]
        public int Rank { get; set; }

        [ColumnMapping("enabled")]
        public bool Enabled { get; set; }

        [ColumnMapping("core_required")]
        public bool CodeRequired { get; set; }

        [ColumnMapping("return_allowed")]
        public bool ReturnAllowed { get; set; }

        [ColumnMapping("read_only")]
        public bool ReadOnly { get; set; }

        [ColumnMapping("code")]
        public string Code { get; set; }

        [ColumnMapping("amount")]
        public decimal Amount { get; set; }
    }
}
