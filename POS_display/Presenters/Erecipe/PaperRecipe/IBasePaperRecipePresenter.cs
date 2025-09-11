using POS_display.Models.Recipe;
using System.Threading.Tasks;

namespace POS_display.Presenters.Erecipe.PaperRecipe
{
    public interface IBasePaperRecipePresenter
    {
        void Init();
        Task<PaperRecipeRequestResponse> Save();
        void RetriveDoctorDataByDoctorCode(string SPSTPL);
        void Validate();
        void ValidateValues();
        void RecalculateValuesByExpiryStart();
        void RecalculateValuesByExpiryEnd();
        void RecalculateValuesByValidPeriod();
        void RecalculateValuesBySalesDate();
        void RecalculateValuesByAmountOfDays();
        void RecalculateValuesByEnoughUntil();
    }
}
