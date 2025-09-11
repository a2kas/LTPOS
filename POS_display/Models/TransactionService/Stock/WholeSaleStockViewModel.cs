using System;

namespace POS_display.Models.TransactionService.Stock
{
    public class WholesaleStockViewModel
    {
        public double PickingQty { get; set; }
        public double TotalQty { get; set; }
        public DateTime DateStamp { get; set; }
        public DateTime? ShortestExpiryDate { get; set; }
        public decimal BasePrice { get; set; }
    }
}
