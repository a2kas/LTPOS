using POS_display.Presenters.Erecipe.PaperRecipe;
using POS_display.Repository.Barcode;
using POS_display.Repository.Recipe;

namespace POS_display.Views.Erecipe.PaperRecipe
{
    public partial class Form1OneTimeWithoutRecipeView : PaperRecipeBaseView, IForm1OneTimeWithoutRecipeView
    {
        #region Construtor
        public Form1OneTimeWithoutRecipeView()
        {
            InitializeComponent();
        }
        #endregion

        #region Override
        public override IBasePaperRecipePresenter CreatePresenterInstance() 
        {
            return new Form1OneTimeWithoutRecipePresenter(this, Session.KVAP, Session.eRecipeUtils, new RecipeRepository());
        }
        #endregion
    }
}
