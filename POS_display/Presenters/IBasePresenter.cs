using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using POS_display.Repository.Pos;

namespace POS_display.Presenters
{
    public interface IBasePresenter
    {
        IPosRepository PosRepository { get; set; }
    }
}
