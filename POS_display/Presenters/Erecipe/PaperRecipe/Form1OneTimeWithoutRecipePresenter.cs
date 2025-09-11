using POS_display.Repository.Recipe;
using POS_display.Utils;
using POS_display.Utils.EHealth;
using POS_display.Views.Erecipe.PaperRecipe;
using System;

namespace POS_display.Presenters.Erecipe.PaperRecipe
{
    public class Form1OneTimeWithoutRecipePresenter : BasePaperRecipePresenter
    {
        #region Members
        private readonly IForm1OneTimeWithoutRecipeView _view;
        #endregion

        #region Constructor
        public Form1OneTimeWithoutRecipePresenter(IForm1OneTimeWithoutRecipeView view, IKVAP kvapService, IEHealthUtils eHealthUtils, IRecipeRepository recipeRepository) :
            base(view, kvapService, eHealthUtils, recipeRepository)
        {
            _view = view ?? throw new ArgumentNullException();

            FormCode = "f1v";
            FormDisplay = "1 Forma - vienkartinis išdavimas be galiojančio recepto";
        }
        #endregion

        #region Override
        public override void Init()
        {
            base.Init();
            HideContorls();
        }

        public override void Validate()
        {
            base.Validate();
        }
        #endregion

        #region Private methods
        private void HideContorls() 
        {
            _view.DoctorCode.Visible = false;
            _view.Stamp.Visible = false;
            _view.SveidraID.Visible = false;

            _view.DoctorCodeLabel.Visible = false;
            _view.StampLabel.Visible = false;
            _view.SveidraIDLabel.Visible = false;
        }
        #endregion
    }
}
