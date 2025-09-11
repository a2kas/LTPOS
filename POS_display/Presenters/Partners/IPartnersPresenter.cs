using POS_display.Models.Partner;
using System.Threading.Tasks;

namespace POS_display.Presenters.Partners
{
    public interface IPartnersPresenter
    {
        Task Init();

        Task LoadPartners();

        void LoadFilterAutoCompleteData();

        void SetFilterAutoCompleteAvailability();

        void SetNextPage();

        void SetPreviousPage();

        void SetFirstPage();

        void SetLastPage();

        void Reset();

        void EnableControls();

        void FocusPartner(long partnerId);

        void ClearFilter();

        PartnerViewData GetFocusedPartner();
    }
}
