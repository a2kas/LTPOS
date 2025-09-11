using System;

namespace POS_display.Models.E1Gateway.Order
{
    public class E1OrderLineViewModel
    {
        public Guid E1OrderLineId { get; set; }
        public Guid E1OrderHeaderId { get; set; }
        public string ItemCode { get; set; }
        public int Quantity { get; set; }
        public int CanceledQuantity { get; set; }
        public int ShippedQuantity { get; set; }
    }
}
