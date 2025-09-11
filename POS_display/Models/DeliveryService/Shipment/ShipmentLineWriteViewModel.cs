using System;

namespace POS_display.Models.DeliveryService.Shipment
{
    public class ShipmentLineWriteViewModel
    {
        public Guid ShipmentLineId { get; set; }
        public string ItemId { get; set; }
        public string ItemName { get; set; }
        public int Quantity { get; set; }
        public DateTime RowVer { get; set; }
    }
}
