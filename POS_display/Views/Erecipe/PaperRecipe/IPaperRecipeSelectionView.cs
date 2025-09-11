using POS_display.Items.eRecipe;
using POS_display.Items.Prices;
using System.Windows.Forms;

namespace POS_display.Views.Erecipe.PaperRecipe
{
    public interface IPaperRecipeSelectionView
    {
        ComboBox RecipeForms { get; set; }
        Button Apply { get; set; }
        void SetGenericItem(GenericItem genericItem);
        void SetERecipeItem(Recipe eRecipeItem);
        void SetPosDetail(Items.posd posDetail);
    }
}
