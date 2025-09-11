using POS_display.Repository.Recipe;
using POS_display.Utils;
using POS_display.Utils.EHealth;
using POS_display.Views.Erecipe.PaperRecipe;
using System;

namespace POS_display.Presenters.Erecipe.PaperRecipe
{
    public class Form1NotCompensatedPresenter : BasePaperRecipePresenter
    {
        #region Members
        private readonly IForm1NotCompensatedView _view;
        #endregion

        #region Constructor
        public Form1NotCompensatedPresenter(IForm1NotCompensatedView view, IKVAP kvapService, IEHealthUtils eHealthUtils, IRecipeRepository recipeRepository) 
            : base(view, kvapService, eHealthUtils, recipeRepository)
        {
            _view = view ?? throw new ArgumentNullException();

            FormCode = "f1";
            FormDisplay = "1 Forma";
        }
        #endregion

        #region Override
        public override void Init()
        {
            base.Init();
        }

        public override void Validate()
        {
            //if (string.IsNullOrWhiteSpace(_view.DoctorCode.Text))
            //    throw new RecipeException("Popierinio recepto duomenys -> 'KVP gydytojo kodas' privalo būti nurodytas!");

            base.Validate();
        }
        #endregion
    }
}
