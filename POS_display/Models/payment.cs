using POS_display.Utils.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace POS_display.Models
{
    public class payment
    {
        public decimal id { get; set; }
        public string name { get; set; }
        public string fiscal { get; set; }
        public int fiscal_rank { get; set; }
        public int rank { get; set; }
        public bool enabled { get; set; }
        public bool code_required { get; set; }
        public bool return_allowed { get; set; }
        public bool read_only { get; set; }
        public string code { get; set; }
        public decimal amount { get; set; }
        public string Buyer { get; set; }
        public decimal LeftPay { get; set; } = 0.0m;
        [Browsable(false)]
        public List<decimal> ExternalIds { get; set; } = new List<decimal>();
    }
}
