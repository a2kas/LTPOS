using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS_display.Models
{
    public class posd_price
    {
        #region Constructor
        public posd_price(decimal posd_id)
        {
            #pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            DB.POS.UpdateSession("Kainos keitimas ", 2);
            #pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            PosdId = posd_id;
        }
        #endregion
        #region Model
        public decimal PosdId { get; set; }
        public decimal Price { get; set; }
        #endregion
        #region Business Logic
        public async Task<bool> ChangePosdPrice()
        {
            return await DB.POS.asyncChangePosdPrice(PosdId, Price);
        }
        #endregion
    }
}
