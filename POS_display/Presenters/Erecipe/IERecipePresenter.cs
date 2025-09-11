using POS_display.Models.HomeMode;
using System.Threading.Tasks;

namespace POS_display.Presenters.Erecipe
{
    public interface IERecipePresenter
    {
        Task FindPatient();

        Task CreateEncounter(string PatientRef);

        Task GetAllergies();

        Task GetRepresentedPersons();

        Task GetRecipeList(int gridSize, int PageIndex, string type);

        Task GetRecipeDispenseCount();

        Task GetDispenseCount();

        Task<bool> IsPossibleApplyUkrainianReffugeInsurance();

        Task<string> GetBarcodeByProductId(decimal productId);

        Task CreateHomeDeliveryDetail(decimal posHeaderId, decimal posDetailId, HomeModeQuantities quantities);

        Task DeleteHomeDeliveryDetail(decimal posHeaderId, decimal posDetailId);

        Task<bool> IsDiagnosisValidByNpakid7(decimal npakid7);
    }
}
