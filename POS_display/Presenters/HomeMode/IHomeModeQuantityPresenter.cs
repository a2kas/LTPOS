using POS_display.Models.HomeMode;
using System.Threading.Tasks;

namespace POS_display.Presenters.HomeMode
{
    public interface IHomeModeQuantityPresenter
    {
        Task<HomeModeQuantities> ResolveQuantities();
        Task Validate();
    }
}
