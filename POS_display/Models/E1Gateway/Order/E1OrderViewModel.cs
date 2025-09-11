using System;
using System.Collections.Generic;

namespace POS_display.Models.E1Gateway.Order
{
    public class E1OrderViewModel
    {
        public Guid E1OrderHeaderId { get; set; }
        public DateTime OrderDate { get; set; } = DateTime.UtcNow;
        public string Country { get; set; }
        public string ClientOrderNo { get; set; }
        public StatusViewModel Status { get; set; }
        public int AddressNumber { get; set; }
        public long CounterValue { get; set; }
        public Guid ClientId { get; set; }
        public TypeViewModel Type { get; set; }
        public Guid? ConsolidationRefId { get; set; }
        public List<E1OrderLineViewModel> Lines { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public DateTime RowVer { get; set; }
        public List<string> PartialOrderNumbers { get; set; }
    }
}
