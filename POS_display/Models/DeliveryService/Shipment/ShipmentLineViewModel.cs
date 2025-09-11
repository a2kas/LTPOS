using System;
using System.Collections.Generic;

namespace POS_display.Models.DeliveryService.Shipment
{
    public class ShipmentLineViewModel
    {
        public Guid ShipmentLineId { get; set; }
        public Guid ShipmentId { get; set; }
        public string ItemId { get; set; }
        public string ItemName { get; set; }
        public int Quantity { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime RowVer { get; set; }
        public bool IsDeleted { get; set; }
        public IEnumerable<ShipmentLineCollectionViewModel> ShipmentLineCollections { get; set; }
    }
}
