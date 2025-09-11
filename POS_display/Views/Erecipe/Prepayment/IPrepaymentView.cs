using POS_display.Helpers;
using System.Windows.Forms;

namespace POS_display.Views.Erecipe.Prepayment
{
    public interface IPrepaymentView
    {
        void SetData(Items.eRecipe.Recipe eRecipeData);
        Button CloseButton { get; set; }
        RichTextBoxEx Content { get; set; }
    }
}
