using POS_display.Models.Partner;
using System.Threading.Tasks;
using System.Windows.Documents;

namespace POS_display.Presenters.HomeMode
{
    public interface IHomeModePresenter
    {
        Task SetPartner(PartnerViewData partner);

        PartnerViewData GetPartner();

        Task CancelHomeDeliverOrder();

        void EnableButtons();

        void Validate();

        void SetSignature(string signature);

        Task CreateHomeDeliverOrder();

        Task SetPartnerAgreementToSaveData();

        FlowDocument CreateAgreementDocument();
    }
}
