using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS_display.Items
{
    public class TamroIntranetGatewayAccessToken
    {
        public string access_token { get; set; }
        public string refresh_token { get; set; }
        public DateTime created_at { get; set; } = DateTime.Now;
    }
}
