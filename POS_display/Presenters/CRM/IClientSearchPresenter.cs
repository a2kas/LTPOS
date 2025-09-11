using System.Threading.Tasks;

namespace POS_display.Presenters.CRM
{
    public interface IClientSearchPresenter
    {
        Task LoadClients();
        void ClearForm();
        void EnableControls();
        string ValidateSearchData();
    }
}
