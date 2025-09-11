using System.Collections.Generic;
using System.Threading.Tasks;

namespace POS_display.Presenters.AdvancePayment
{
    public interface IAdvancePaymentPresenter
    {
        List<string> CardCache { get; set; }

        Task Init();

        Task Confirm();

        void EnableControls();

        bool PresentCardAleardyExistInBasket();
    }
}
