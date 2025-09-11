using System.ComponentModel;

namespace POS_display.Models.Partner
{
    public class PartnerViewData
    {
        [Browsable(false)]
        public long Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string ECode { get; set; }
        public string TCode { get; set; }
        public string Address { get; set; }
        public string Agent { get; set; }
        public string Scala { get; set; }
        public string DebtorTypeName { get; set; }
        public string Descrip { get; set; }
        [Browsable(false)]
        public string Email { get; set; }
        [Browsable(false)]
        public string Phone { get; set; }
        public string City { get; set; }
        [Browsable(false)]
        public string PostIndex { get; set; }
    }
}
