using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS_display.Items
{
    public class Sticker
    {
        public decimal id { get; set; }
        public string name { get; set; }
        public string prefix { get; set; }
        public decimal amount_from { get; set; }
        public decimal amount_to { get; set; }
        public string message_type { get; set; }
        public string message { get; set; }
    }
}
