using System.Collections.Generic;

namespace POS_display.Models.TransactionService.Stock
{
    public class StockViewModel
    {
        public string ItemRetailCode { get; set; }
        public List<RetailStockViewModel> Retail { get; set; }
        public List<WholesaleStockViewModel> Wholesale { get; set; }
    }
}
