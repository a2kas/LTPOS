using POS_display.Models.DeliveryService.Receiver;
using System;
using System.Collections.Generic;

namespace POS_display.Models.DeliveryService.Shipment
{
    public class ShipmentWriteViewModel
    {
        public Guid Id { get; set; }
        public long CountryId { get; set; }
        public string ClientOrderNo { get; set; }
        public string Comment { get; set; }
        public BoxSizeViewModel? BoxSize { get; set; }
        public int BoxCount { get; set; }
        public decimal Weight { get; set; }
        public decimal LeftAmount { get; set; }
        public Guid? ManifestId { get; set; }
        public ReceiverViewModel Receiver { get; set; }
        public string TerminalId { get; set; }
        public string TerminalName { get; set; }
        public long? E1AddressNumber { get; set; }
        public ProviderViewModel Provider { get; set; }
        public DestinationTypeViewModel DestinationType { get; set; }
        public StatusWriteModel Status { get; set; }
        public DateTime RowVer { get; set; }
        public bool ConsultationApplied { get; set; }
        public ConsultationTypeViewModel ConsultationType { get; set; }
        public IEnumerable<ShipmentLineWriteViewModel> ShipmentLines { get; set; }
    }
}
