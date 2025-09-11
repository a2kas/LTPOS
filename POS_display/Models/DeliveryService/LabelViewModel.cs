using POS_display.Models.DeliveryService.Shipment;
using System;

namespace POS_display.Models.DeliveryService
{
    public class LabelViewModel
    {
        public Guid Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public Guid ShipmentId { get; set; }
        public Guid? SenderId { get; set; }
        public string TrackingNumber { get; set; }
        public LabelStatusViewModel Status { get; set; }
        public bool IsTrackable { get; set; }
        public int Sequence { get; set; }
    }
}
