using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS_display.Presenters
{
    public class posd_price
    {
        private Views.Iposd_price _view;

        public posd_price(Views.Iposd_price view)
        {
            _view = view;
        }

        public async Task<bool> ChangePosdPrice()
        {
            return await _view.posd_price_model.ChangePosdPrice();
        }
    }
}
