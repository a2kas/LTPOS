using System.Windows.Forms;

namespace POS_display.Views.Erecipe.Dispense
{
    public interface IDispensesView
    {
        DataGridView Dispenses { get; set; }

        string DispensesInfo { get; set; }

        string FormHeaderText { get; set; }

        void SetData(Items.eRecipe.Recipe eRecipeItem);
    }
}
