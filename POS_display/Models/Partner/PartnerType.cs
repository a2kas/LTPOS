using System.Data.Linq.Mapping;

namespace POS_display.Models.Partner
{
    [Table(Name = "type")]
    public class PartnerType
    {
        [Column(Name = "id")]
        public long Id { get; set; }
        [Column(Name = "name")]
        public string Name { get; set; }
    }
}
