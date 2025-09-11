using System;

namespace POS_display.Models.KAS
{
    public class PosHeader
    {
        public decimal Id { get; set; }
        public string Type { get; set; }
        public string Status { get; set; }
        public decimal TotalSum { get; set; }
        public decimal VAT { get; set; }
        public DateTime DocumentDate { get; set; }
        public string DocumentDate2 { get; set; }
        public string DebtorName { get; set; }
        public decimal SumIncVat { get; set; }
        public string DeviceNo { get; set; }
        public string CheckNo { get; set; }
        public string SF { get; set; }
    }
}
