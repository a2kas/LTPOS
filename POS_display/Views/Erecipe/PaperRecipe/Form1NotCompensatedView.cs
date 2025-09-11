using POS_display.Presenters.Erecipe.PaperRecipe;
using POS_display.Repository.Barcode;
using POS_display.Repository.Recipe;

namespace POS_display.Views.Erecipe.PaperRecipe
{
    public partial class Form1NotCompensatedView : PaperRecipeBaseView, IForm1NotCompensatedView
    {
        #region Construtor
        public Form1NotCompensatedView()
        {
            InitializeComponent();
        }
        #endregion

        #region Override
        public override IBasePaperRecipePresenter CreatePresenterInstance() 
        {
            return new Form1NotCompensatedPresenter(this, Session.KVAP, Session.eRecipeUtils, new RecipeRepository());
        }
        #endregion
    }
}
