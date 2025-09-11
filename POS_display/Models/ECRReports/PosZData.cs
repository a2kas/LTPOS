using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace POS_display.Models.ECRReports
{
    [XmlRoot("FiscalDevice")]
    public class PosZData
    {
        [XmlElement(ElementName = "Line")]
        public PosZLine[] Lines { get; set; }
    }
}
