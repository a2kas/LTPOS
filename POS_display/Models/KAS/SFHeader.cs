using System;
using System.Data.Linq.Mapping;

namespace POS_display.Models.KAS
{
    public class SFHeader
    {
        [Column(Name = "check_id")]
        public long CheckId { get; set; }

        [Column(Name = "buyer_id")]
        public long BuyerId { get; set; }

        [Column(Name = "check_no")]
        public string  CheckNo { get; set; }

        [Column(Name = "documentdate")]
        public DateTime DocumentDate { get; set; }

        [Column(Name = "sask_nr")]
        public string InvoiceNo { get; set; }

        [Column(Name = "comment")]
        public string Comment { get; set; }
    }
}
