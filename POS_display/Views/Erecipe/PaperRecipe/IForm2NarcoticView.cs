using System.Windows.Forms;

namespace POS_display.Views.Erecipe.PaperRecipe
{
    public interface IForm2NarcoticView : IPaperRecipeBaseView
    {
        TextBox RecipeSerial { get; set; }
        TextBox RecipeNumber { get; set; }
        ComboBox CompensationCode { get; set; }
    }
}
