using Dapper.ColumnMapper;
using System.Data.Linq.Mapping;

namespace POS_display.Models.ECRReports
{
    [Table(Name = "settings")]
    public class DeviceSettingValue
    {
        [ColumnMapping("id")]
        public int Id { get; set; }

        [ColumnMapping("key")]
        public int Key { get; set; }

        [ColumnMapping("value")]
        public string Value { get; set; }

        [ColumnMapping("description")]
        public string Description { get; set; }
    }
}
