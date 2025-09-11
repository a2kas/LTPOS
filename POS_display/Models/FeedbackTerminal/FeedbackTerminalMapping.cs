using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS_display.Models.FeedbackTerminal
{
    public class FeedbackTerminalMapping
    {
        public string Company { get; set; }
        public string PharmacyCode { get; set; }
        public string TerminalIP { get; set; }
        public string TerminalSN { get; set; }
        public string ComputerIP { get; set; }
        public string NumberOfCashDesk { get; set; }
        public string Employee { get; set; }
        public string UserName { get; set; }
        public string FeedbackTerminalAddress
        {
            get
            {
                return $"ws://{TerminalIP}:8080/";
            }
        }
    }
}
