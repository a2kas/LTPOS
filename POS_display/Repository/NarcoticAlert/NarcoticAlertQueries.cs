using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS_display.Repository.NarcoticAlert
{
    public static class NarcoticAlertQueries
    {
        public static string GetATCCodifiers => "SELECT * FROM narcotic_alert";
    }
}
