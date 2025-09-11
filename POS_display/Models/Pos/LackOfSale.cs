namespace POS_display.Models.Pos
{
    public class LackOfSale
    {
        public string Barcode { get; set; }

        public string Name { get; set; }

        public decimal Qty { get; set; }

        public decimal QtyLeft { get; set; }

        public decimal QtyLack { get; set; }
    }
}
