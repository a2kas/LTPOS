using CKas.Contracts.Search;
using System;
using System.Collections.Generic;

namespace POS_display.Models.TransactionService.Stock
{
    public class StockSearchViewModel : SearchViewModel
    {
        public DateTime? StockDate { get; set; }
        public ICollection<string> LocalItemCodes { get; set; }
        public StockTypeViewModel StockType { get; set; }
    }
}
