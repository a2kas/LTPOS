using System;

namespace POS_display.Items
{
    public class User
    {
        public decimal id { get; set; }
        public string DisplayName { get; set; }
        public string postname { get; set; }
        public string StampId { get; set; }
        public string Stamp { get; set; }
        public decimal locked { get; set; }
        public DateTime? ExpireDate { get; set; }
    }
}
