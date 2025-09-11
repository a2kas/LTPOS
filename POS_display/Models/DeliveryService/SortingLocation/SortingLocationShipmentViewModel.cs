using System;

namespace POS_display.Models.DeliveryService.SortingLocation
{
    public class SortingLocationShipmentViewModel
    {
        public Guid SortingLocationShipmentId { get; set; }
        public Guid SortingLocationId { get; set; }
        public Guid ShipmentId { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime RowVer { get; set; }
    }
}
