using System;

namespace POS_display.Models.TLK
{
    public class TLKCheapest
    {
        public int PriceListVersion { get; set; }
        public DateTime StartDate { get; set; }
        public string Npakid7 { get; set; }
        public bool IsCheapest { get; set; }
    }
}
