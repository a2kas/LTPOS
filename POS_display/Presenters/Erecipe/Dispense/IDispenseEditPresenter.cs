using System.Threading.Tasks;

namespace POS_display.Presenters.Erecipe.Dispense
{
    public interface IDispenseEditPresenter
    {
        Task Save();
        Task Init(string compositionId);
        string Validate();
    }
}
