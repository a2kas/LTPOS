using TamroUtilities.HL7.Models;

namespace POS_display.Presenters.Erecipe.Dispense
{
    public interface IDispensesInfoPresenter
    {
        void SetData(Items.eRecipe.Recipe eRecipeItem);
    }
}
