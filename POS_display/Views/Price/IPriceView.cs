using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace POS_display.Views.Price
{
    public interface IPriceView
    {
        TextBox Price { get; set; }
        Button CalcButton { get; set; }
    }
}
