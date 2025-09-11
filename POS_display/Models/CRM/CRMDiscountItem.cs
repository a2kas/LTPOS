namespace POS_display.Models.CRM
{
    public class CRMDiscountItem
    {
        public string DiscountCode { get; set; }

        public decimal DiscountValue { get; set; }

        public decimal DiscountPercent { get; set; }

        public string Description { get; set; }

        public string BillItemId { get; set; }
    }
}
