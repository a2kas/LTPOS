using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace POS_display.Models.E1Gateway.Order
{
    public class E1OrderCreateViewModel
    {
        public string Country { get; set; }
        public string ClientOrderNo { get; set; }
        public int AddressNumber { get; set; }
        public Guid ClientId { get; set; }
        public TypeViewModel Type { get; set; }
        public Guid? ConsolidationRefId { get; set; }
        public RouteNumberViewModel RouteNumber { get; set; }
        public List<E1OrderLineCreateViewModel> Lines { get; set; }
    }
}
