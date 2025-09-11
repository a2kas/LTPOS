using Dapper.ColumnMapper;
using System;
using System.Data.Linq.Mapping;

namespace POS_display.Models.ECRReports
{
    [Table(Name = "ekj")]
    public class EKJEntry
    {
        [ColumnMapping("id")]
        public int Id { get; set; }

        [ColumnMapping("date")]
        public DateTime Date { get; set; }

        [ColumnMapping("z_number")]
        public int ZNumber { get; set; }

        [ColumnMapping("device_serial")]
        public string DeviceSerial { get; set; }
    }
}
