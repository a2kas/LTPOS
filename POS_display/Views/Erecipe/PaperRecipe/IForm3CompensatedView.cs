using System.Windows.Forms;

namespace POS_display.Views.Erecipe.PaperRecipe
{
    public interface IForm3CompensatedView : IPaperRecipeBaseView
    {
        TextBox DiseaseCode { get; set; }
        TextBox AagaSgasNumber { get; set; }
        ComboBox CompensationCode { get; set; }
        TextBox CompensatedSum { get; set; }
        TextBox PrepaymentCompensatedSum { get; set; }
    }
}
