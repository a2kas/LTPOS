using System;
using System.Collections.Generic;

namespace POS_display.Models.DeliveryService.Manifest
{
    public class ManifestViewModel
    {
        public ManifestViewModel()
        {
            Shipments = new List<ShipmentViewModel>();
        }

        public Guid Id { get; set; }
        public string ReferenceId { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public Guid SenderId { get; set; }
        public List<ShipmentViewModel> Shipments { get; set; }
    }
}
