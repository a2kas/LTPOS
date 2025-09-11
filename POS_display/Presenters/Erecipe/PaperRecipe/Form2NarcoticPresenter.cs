using POS_display.Exceptions;
using POS_display.Repository.Recipe;
using POS_display.Utils;
using POS_display.Utils.EHealth;
using POS_display.Views.Erecipe.PaperRecipe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace POS_display.Presenters.Erecipe.PaperRecipe
{
    public class Form2NarcoticPresenter : BasePaperRecipePresenter
    {
        #region Members
        private readonly IForm2NarcoticView _view;
        #endregion

        #region Constructor
        public Form2NarcoticPresenter(IForm2NarcoticView view, IKVAP kvapService, IEHealthUtils eHealthUtils, IRecipeRepository recipeRepository) 
            : base (view, kvapService, eHealthUtils, recipeRepository)
        {
            _view = view ?? throw new ArgumentNullException();

            FormCode = "f2";
            FormDisplay = "2 Forma";
        }
        #endregion

        #region Override
        public override void BindData()
        {
            base.BindData();

            var selectedCompensation = (KeyValuePair<string, string>)((BindingSource)_view.CompensationCode.DataSource).Current;
            CreateRequest.PaperPrescriptionData.CompensationCodeCode = selectedCompensation.Key;
            CreateRequest.PaperPrescriptionData.CompensationCodeDisplay = selectedCompensation.Value;
            CreateRequest.PaperPrescriptionData.Form2Number = _view.RecipeNumber.Text;
            CreateRequest.PaperPrescriptionData.Form2Series = _view.RecipeSerial.Text;
            CreateRequest.PaperPrescriptionData.Form2Tag = true;
        }

        public override void Init()
        {
            Dictionary<string, string> compensationTypesDictionary = Session.CompensationTypeClassifiers
            .ToDictionary(
                rc => rc.Code,
                rc => rc.DisplayValue
            );

            _view.CompensationCode.DataSource = new BindingSource(compensationTypesDictionary, null);
            _view.CompensationCode.DisplayMember = "Value";
            _view.CompensationCode.ValueMember = "Key";

            base.Init();
        }

        public override void Validate()
        {
            if (string.IsNullOrWhiteSpace(_view.RecipeSerial.Text))
                throw new RecipeException("Popierinio recepto duomenys -> 'Recepto serija' privalo būti nurodytą!");

            if (string.IsNullOrWhiteSpace(_view.RecipeNumber.Text))
                throw new RecipeException("Popierinio recepto duomenys -> 'Recepto numeris' privalo būti nurodytas!");

            //if (string.IsNullOrWhiteSpace(_view.DoctorCode.Text))
            //    throw new RecipeException("Popierinio recepto duomenys -> 'KVP gydytojo kodas' privalo būti nurodytas!");

            base.Validate();
        }
        #endregion
    }
}
