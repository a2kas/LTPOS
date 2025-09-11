using System.Threading.Tasks;

namespace POS_display.Presenters.Display
{
    public interface IDisplay1Presenter
    {
        Task LoadRecommendations();

        Task LoadCRMData();

        Task<decimal> GetQty(long productID);

        Task CancelSalesOrder(Items.posh poshItem);

        Task LoadUserPrioritesRatio();

        Task LoadMedictionActiveSubstances();

        Task LoadMedictionNames();

        bool IsDoctorCard(string cardNo);

        Task SetHomeMode(bool apply);
        Task SetWoltMode(bool apply);

        Task CancelHomeDeliveryorder(Items.posh poshItem);
    }
}
