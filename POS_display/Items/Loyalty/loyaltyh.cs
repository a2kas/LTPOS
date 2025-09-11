using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace POS_display.Items.Loyalty
{
    public class loyaltyh
    {
        public decimal posh_id { get; set; }
        public string card_type { get; set; }
        public string card_no { get; set; }
        public decimal active { get; set; }
        public decimal status { get; set; }
        public string manual_vouchers { get; set; }
        public decimal accrue_points { get; set; }
    }
}
