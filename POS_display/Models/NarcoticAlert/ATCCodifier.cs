using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace POS_display.Models.NarcoticAlert
{
    public class ATCCodifier
    {
        public string ActiveSubstance { get; set; }
        public string UnitOfMeasurement { get; set; }
        public decimal Amount { get; set; }
        [Browsable(false)]
        public string ATCPatterns { get; set; }
        [Browsable(false)]
        public List<string> ATCCodes
        {
            get 
            {
                return ATCPatterns.Split(new char[] { ',' }, System.StringSplitOptions.RemoveEmptyEntries).ToList();
            }
        } 
    }
}
