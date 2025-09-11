using System;

namespace POS_display.Models.TransactionService.Stock
{
    public class RetailStockViewModel
    {
        public string PharmacyCode { get; set; }
        public double TotalQty { get; set; }
        public DateTime DateStamp { get; set; }
        public DateTime ShortestExpiryDate { get; set; }
    }
}
