using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS_display.Presenters.ECRReports
{
    public interface IECRReportsPresenter
    {
        Task InitOperationsData();
        Task<decimal> PerformExecute(string selectedIndex);
        void EnableControls(string selectedIndex);
        void ChangeTextChanged(object sender);
    }
}
