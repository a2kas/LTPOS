using System.Threading.Tasks;

namespace POS_display.Presenters.Price
{
    public interface IDrugPricesPresenter
    {
        Task<string> GetBarcodeByActiveSubstance(string activeSubstance);

        Task<string> GetBarcodeByGenericName(string genericName);
    }
}
