using System;

namespace POS_display.Models.TLK
{
    public class BarcodeModel
    {
        public long Id { get; set; }
        public long? DrugId { get; set; }
        public string Code { get; set; }
        public string Type { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
