using POS_display.Exceptions;
using POS_display.Repository.Recipe;
using POS_display.Utils;
using POS_display.Utils.EHealth;
using POS_display.Views.Erecipe.PaperRecipe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using TamroUtilities.HL7.Models;

namespace POS_display.Presenters.Erecipe.PaperRecipe
{
    public class Form3CompensatedPresenter : BasePaperRecipePresenter
    {
        #region Members
        private readonly IForm3CompensatedView _view;
        private readonly AutoCompleteStringCollection _diseaseCodeAutoCompleteCollection;
        #endregion

        #region Constructor
        public Form3CompensatedPresenter(IForm3CompensatedView view, IKVAP kvapService, IEHealthUtils eHealthUtils, IRecipeRepository recipeRepository) :
            base(view, kvapService, eHealthUtils, recipeRepository)
        {
            _view = view ?? throw new ArgumentNullException();
            _diseaseCodeAutoCompleteCollection = new AutoCompleteStringCollection();

            FormCode = "f3";
            FormDisplay = "3 Forma";
        }
        #endregion

        #region Override
        public override void BindData()
        {
            base.BindData();

            var diseaseArgs = _view.DiseaseCode.Text.Split(new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries);
            if (diseaseArgs.Length > 1)
            {
                CreateRequest.PaperPrescriptionData.ConditionCodeCode = diseaseArgs[0];
                CreateRequest.PaperPrescriptionData.ConditionCodeDisplay = diseaseArgs[1];
            }

            var selectedCompensation = (KeyValuePair<string, string>)((BindingSource)_view.CompensationCode.DataSource).Current;
            CreateRequest.PaperPrescriptionData.CompensationCodeCode = selectedCompensation.Key;
            CreateRequest.PaperPrescriptionData.CompensationCodeDisplay = selectedCompensation.Value;

            CreateRequest.PaperPrescriptionData.AagaSgasNumber = _view.AagaSgasNumber.Text;

            CreateRequest.PricePaid = new CurrencyDto() { Value = _view.CompensatedSum.Text.ToDecimal() };
            CreateRequest.PriceCompensated = new CurrencyDto() { Value = _view.PrepaymentCompensatedSum.Text.ToDecimal() };



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

            _diseaseCodeAutoCompleteCollection.AddRange(Session.TLK10AMClassifiers.Select(c => $"{c.Code}: {c.Title}").ToArray());
            _view.DiseaseCode.AutoCompleteCustomSource = _diseaseCodeAutoCompleteCollection;

            base.Init();
        }

        public override void Validate()
        {
            //if (string.IsNullOrWhiteSpace(_view.DoctorCode.Text))
            //    throw new RecipeException("Popierinio recepto duomenys -> 'KVP gydytojo kodas' privalo būti nurodytas!");

            if (string.IsNullOrWhiteSpace(_view.DiseaseCode.Text))
                throw new RecipeException("Popierinio recepto duomenys -> 'Ligos kodas' privalo būti nurodytas!");

            if (!_diseaseCodeAutoCompleteCollection.Contains(_view.DiseaseCode.Text))
                throw new RecipeException("Popierinio recepto duomenys -> 'Ligos kodas' privalo būti iš klasifikatoriaus!");

            if (string.IsNullOrWhiteSpace(_view.AagaSgasNumber.Text))
                throw new RecipeException("Popierinio recepto duomenys -> 'AAGA arba ISAS kortelės Nr.' privalo būti nurodytą!");

            if (string.IsNullOrWhiteSpace(_view.CompensatedSum.Text))
                throw new RecipeException("Išdavimo informacija -> 'Kompensuojama suma' privalo būti nurodytą!");

            if (string.IsNullOrWhiteSpace(_view.PrepaymentCompensatedSum.Text))
                throw new RecipeException("Išdavimo informacija -> 'Priemokos komp. suma' privalo būti nurodytą!");

            base.Validate();
        }
        #endregion
    }
}
