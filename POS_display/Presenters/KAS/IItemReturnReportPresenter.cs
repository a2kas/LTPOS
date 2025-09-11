using POS_display.Models.KAS;
using System.Threading.Tasks;

namespace POS_display.Presenters.KAS
{
    public interface IItemReturnReportPresenter
    {
        Task Init(ReturningItemsData returningItemsData);
        Task RefreshData();
        Task PerformPrint();
        Task<decimal> CalculateRoundingValue();
        Task SendItemsReturnToCashRegister();
    }
}
