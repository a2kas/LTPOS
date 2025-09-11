using POS_display.Models.Partner;
using System.Threading.Tasks;

namespace POS_display.Presenters.Partners
{
    public interface IPartnerEditorPresenter
    {
        Task Init();
        Task Load(decimal partnerId);
        Task Save();
    }
}
