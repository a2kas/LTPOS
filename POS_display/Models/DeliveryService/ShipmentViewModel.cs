using POS_display.Models.DeliveryService.Manifest;
using POS_display.Models.DeliveryService.Receiver;
using POS_display.Models.DeliveryService.Shipment;
using POS_display.Models.DeliveryService.SortingLocation;
using POS_display.Models.E1Gateway.Order;
using System;
using System.Collections.Generic;

namespace POS_display.Models.DeliveryService
{
    public class ShipmentViewModel
    {
        public ShipmentViewModel()
        {
            Labels = new List<LabelViewModel>();
        }

        public Guid Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public string ClientOrderNo { get; set; }
        public string Comment { get; set; }
        public string BoxSize { get; set; }
        public int BoxCount { get; set; }
        public decimal Weight { get; set; }
        public decimal LeftAmount { get; set; }
        public long CountryId { get; set; }
        public Guid? ManifestId { get; set; }
        public ManifestViewModel Manifest { get; set; }
        public ReceiverViewModel Receiver { get; set; }
        public List<LabelViewModel> Labels { get; set; }
        public string TerminalId { get; set; }
        public string TerminalName { get; set; }
        public long? E1AddressNumber { get; set; }
        public ProviderViewModel Provider { get; set; }
        public DestinationTypeViewModel DestinationType { get; set; }
        public StatusViewModel Status { get; set; }
        public IEnumerable<ShipmentLineViewModel> ShipmentLines { get; set; }
        public SortingLocationShipmentViewModel SortingLocationShipment { get; set; }
        public bool ConsultationApplied { get; set; }
        public ConsultationTypeViewModel ConsultationType { get; set; }
        public DateTime RowVer { get; set; }
    }
}
