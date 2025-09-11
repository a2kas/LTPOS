using System;

namespace POS_display.Models.Discount
{
    public class DiscountH
    {
        public decimal Id { get; set; }
        public string Name { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime ValidTo { get; set; }
        public string Perfix { get; set; }
    }
}
