using TamroUtilities.HL7.Models;

namespace POS_display.Views.ERecipe
{
    public interface IERecipe
    {
        string PersonalCode { get; set; }
        Items.eRecipe.Recipe eRecipeItem { get; set; }
        Items.eRecipe.Recipe dispenseItem { get; set; }
        string PickedUpByRef { get; set; }
        string PickedForUserId { get; set; }
        AllergyIntoleranceDto Allergies { get; set; }
        RepresentedPersonsDto RepresentedPersons { get; set; }
        RecipeListDto RecipeList { get; set; }
    }
}
