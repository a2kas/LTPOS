namespace POS_display.Models.Price
{
    public class TLKPriceDetail
    {
        public decimal ProductId { get; set; }
        public string Npakid7 { get; set; }
        public decimal Compensation { get; set; }
        public decimal Percent { get; set; }
        public decimal RetailPrice { get; set; }
        public byte IsCheapest { get; set; }
    }
}
