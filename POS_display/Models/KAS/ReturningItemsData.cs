using System;
using System.Collections.Generic;

namespace POS_display.Models.KAS
{
    public class ReturningItemsData
    {
        public decimal PosHeaderId { get; set; }
        public string PharmacyNo { get; set; }
        public string Address { get; set; }
        public string CashDeskNr { get; set; }
        public string ChequeNr { get; set; }
        public string Cashier { get; set; }
        public decimal ReturnSum { get; set; }
        public DateTime Date { get; set; }
        public string MistakeType { get; set; }
        public string DocumentNo { get; set; }
        public List<ReturningItem> ReturningItems { get; set; }
    }
}
