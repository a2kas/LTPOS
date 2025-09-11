using Dapper.ColumnMapper;
using System.Data.Linq.Mapping;

namespace POS_display.Models.ECRReports
{
    [Table(Name = "z_report")]
    public class ZReportEntry
    {
        [ColumnMapping("id")]
        public int Id { get; set; }

        [ColumnMapping("ekj_id")]
        public int EkjId { get; set; }

        [ColumnMapping("paid_p")]
        public int PaidP { get; set; }

        [ColumnMapping("paid_n")]
        public int PaidN { get; set; }

        [ColumnMapping("paid_c")]
        public int PaidC { get; set; }

        [ColumnMapping("paid_k")]
        public int PaidK { get; set; }

        [ColumnMapping("discount_count")]
        public int DiscountCount { get; set; }

        [ColumnMapping("discount_total")]
        public int DiscountTotal { get; set; }

        [ColumnMapping("financial_in_p")]
        public int FinancialInP { get; set; }

        [ColumnMapping("financial_out_p")]
        public int FinancialOutP { get; set; }

        [ColumnMapping("paid_i")]
        public int PaidI { get; set; }

        [ColumnMapping("paid_j")]
        public int PaidJ { get; set; }

        [ColumnMapping("financial_in_a")]
        public int FinancialInA { get; set; }

        [ColumnMapping("financial_in_c")]
        public int FinancialInC { get; set; }

        [ColumnMapping("drawer_cash")]
        public int DrawerCash { get; set; }

        [ColumnMapping("a_tax_percent")]
        public int ATaxPercent { get; set; }

        [ColumnMapping("a_total")]
        public int ATotal { get; set; }

        [ColumnMapping("a_total_tax")]
        public int ATotalTax { get; set; }

        [ColumnMapping("a_total_w_tax")]
        public int ATotalWTax { get; set; }

        [ColumnMapping("c_tax_percent")]
        public int CTaxPercent { get; set; }

        [ColumnMapping("c_total")]
        public int CTotal { get; set; }

        [ColumnMapping("c_total_tax")]
        public int CTotalTax { get; set; }

        [ColumnMapping("c_total_w_tax")]
        public int CTotalWTax { get; set; }

        [ColumnMapping("total_income_w_tax")]
        public int TotalIncomeWTax { get; set; }

        [ColumnMapping("b_tax_percent")]
        public int BTaxPercent { get; set; }

        [ColumnMapping("b_total")]
        public int BTotal { get; set; }

        [ColumnMapping("b_total_tax")]
        public int BTotalTax { get; set; }

        [ColumnMapping("b_total_w_tax")]
        public int BTotalWTax { get; set; }
    }
}
