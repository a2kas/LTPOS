using System;

namespace POS_display.Models.DeliveryService.Shipment
{
    public class ShipmentLineCollectionViewModel
    {
        public Guid ShipmentLineCollectionId { get; set; }
        public string ItemId { get; set; }
        public string ItemName { get; set; }
        public decimal Price { get; set; }
        public string LotNo { get; set; }
        public string Barcode { get; set; }
        public DateTime LotExpiryDate { get; set; }
        public Guid TraceId { get; set; }
        public string ContainerId { get; set; }
        public Guid? SortingLocationId { get; set; }
        public Guid? ShipmentLineId { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime RowVer { get; set; }
        public bool IsDeleted { get; set; }
    }
}
