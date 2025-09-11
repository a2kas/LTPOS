using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace POS_display.Items.KAS
{
    public class posh
    {
        public decimal id { get; set; }
        public string type { get; set; }
        public string status { get; set; }
        public decimal totalsum { get; set; }
        public decimal vat { get; set; }
        public DateTime documentdate { get; set; }
        public string documentdate2 { get; set; }
        public string debtorname { get; set; }
        public decimal sumincvat { get; set; }
        public string deviceno { get; set; }
        public string checkno { get; set; }
        public string sf { get; set; }
    }
}
