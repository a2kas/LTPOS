using POS_display.Models.Recipe;
using POS_display.Repository.Recipe;
using POS_display.Utils.EHealth;
using POS_display.Views.Erecipe.Dispense;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TamroUtilities.HL7.Models;
using TamroUtilities.HL7.Models.Dispense;

namespace POS_display.Presenters.Erecipe.Dispense
{
    public class DispenseEditPresenter : IDispenseEditPresenter
    {
        #region Members
        private readonly IDispenseEditView _view;
        private readonly IEHealthUtils _eHealthUtils;
        private readonly IRecipeRepository _recipeRepository;
        private RecipeEditModel _recipeEditModel;
        private DispenseDto _currentDispense;
        private const string DateFormat = "yyyy-MM-dd";
        private bool _isRecalculating = false;
        #endregion

        #region Constructor
        public DispenseEditPresenter(IDispenseEditView view, IEHealthUtils eHealthUtils, IRecipeRepository recipeRepository)
        {
            _view = view ?? throw new ArgumentNullException();
            _eHealthUtils = eHealthUtils ?? throw new ArgumentNullException();
            _recipeRepository = recipeRepository ?? throw new ArgumentNullException();
            SubscribeToViewEvents();
        }
        #endregion

        #region Public methods
        public async Task Init(string compositionId)
        {
            var dispenseList = await _eHealthUtils.GetDispenseList<DispenseListDto>(
                string.Empty,
                string.Empty,
                string.Empty,
                string.Empty,
                string.Empty,
                string.Empty,
                1,
                1,
                string.Empty,
                new List<string>() { compositionId });

            _currentDispense = dispenseList.DispenseList.FirstOrDefault();

            _view.MedicationValidUntil = _currentDispense.DueDate.ToDateTime().ToString("yyyy-MM-dd");
            _view.DayCount = !string.IsNullOrEmpty(_currentDispense.DurationOfUse) ? _currentDispense.DurationOfUse : "0";
            _view.SaleDate = _currentDispense.DateWritten.ToDateTime().ToString("yyyy-MM-dd"); ;
            _view.SalePrice = !string.IsNullOrEmpty(_currentDispense.PriceRetailValue) ? _currentDispense.PriceRetailValue : "0";
            _view.IssuedQuantity = !string.IsNullOrEmpty(_currentDispense.QuantityValue) ? _currentDispense.QuantityValue : "0";
            _view.PatientAmount = !string.IsNullOrEmpty(_currentDispense.PricePaidValue) ? _currentDispense.PricePaidValue : "0";
            _view.ReimbursedAmount = !string.IsNullOrEmpty(_currentDispense.PriceCompensatedValue) ? _currentDispense.PriceCompensatedValue : "0";

            RecalculateAdditionalReimbursedAmount();
        }

        public string Validate()
        {
            if (string.IsNullOrWhiteSpace(_view.EditReason))
                return "Būtina nurodyti redagavimo priežastį";

            if (decimal.TryParse(_view.AdditionalReimbursedAmount, out decimal additionalAmount) && additionalAmount < 0)
                return "Priemokos kompensuojama suma negali būti neigiama. Patikrinkite įvestas sumas.";

            if (decimal.TryParse(_view.SalePrice, out decimal salePrice) &&
                decimal.TryParse(_view.PatientAmount, out decimal patientAmount) &&
                decimal.TryParse(_view.ReimbursedAmount, out decimal reimbursedAmount))
            {
                if (salePrice < (patientAmount + reimbursedAmount))
                    return "Pardavimo kaina negali būti mažesnė nei paciento mokamos ir kompensuojamos sumų suma.";
            }

            return string.Empty;
        }

        public async Task Save()
        {
            await BindData();
            await _recipeRepository.UpdateRecipeByEditData(_recipeEditModel);
            var updateRequest = new UpdateDispenseRequest()
            {
                PractitionerId = Session.PractitionerItem.PractitionerId.ToLong(),
                PractitionerRef = Session.PractitionerItem.PractitionerRef,
                CompositionId = _currentDispense.CompositionId.ToLong(),
                UpdateReason = _view.EditReason,
                DueDate = _view.MedicationValidUntil.ToDateTime(),
                DurationOfUse = _view.DayCount.ToInt(),
                PricePaid = _view.PatientAmount.ToDecimal(),
                PriceRetail = _view.SalePrice.ToDecimal(),
                PriceCompensated = _view.ReimbursedAmount.ToDecimal(),
                Quantity = _view.IssuedQuantity.ToInt()
            };
            var response = _eHealthUtils.UpdateDispense(updateRequest);
        }
        #endregion

        #region Private methods
        private async Task BindData()
        {
            _recipeEditModel = await _recipeRepository.GetRecipeEditDataByCompositionId(_currentDispense.CompositionId.ToDecimal());
            _recipeEditModel.CountDay = _view.DayCount.ToDecimal();
            _recipeEditModel.TillDate = _view.MedicationValidUntil.ToDateTime();
            _recipeEditModel.TotalSum = _view.SalePrice.ToDecimal();
            _recipeEditModel.CompensationSum = _view.ReimbursedAmount.ToDecimal();
            _recipeEditModel.PrepaymentCompensationSum = _view.AdditionalReimbursedAmount.ToDecimal();
            _recipeEditModel.PaySum = _view.PatientAmount.ToDecimal();
        }

        private void SubscribeToViewEvents()
        {
            _view.DayCountChanged += OnDayCountChanged;
            _view.MedicationValidUntilChanged += OnMedicationValidUntilChanged;

            _view.SalePriceChanged += OnPriceChanged;
            _view.PatientAmountChanged += OnPriceChanged;
            _view.ReimbursedAmountChanged += OnPriceChanged;
        }

        private void OnDayCountChanged(object sender, EventArgs e)
        {
            if (_isRecalculating || _currentDispense == null) return;

            try
            {
                _isRecalculating = true;
                RecalculateFromDayCount();
            }
            finally
            {
                _isRecalculating = false;
            }
        }

        private void OnMedicationValidUntilChanged(object sender, EventArgs e)
        {
            if (_isRecalculating || _currentDispense == null) return;

            try
            {
                _isRecalculating = true;
                RecalculateFromMedicationValidUntil();
            }
            finally
            {
                _isRecalculating = false;
            }
        }

        private void OnPriceChanged(object sender, EventArgs e)
        {
            if (_isRecalculating || _currentDispense == null) return;

            try
            {
                _isRecalculating = true;
                RecalculateAdditionalReimbursedAmount();
            }
            finally
            {
                _isRecalculating = false;
            }
        }
        #endregion

        #region Private Recalculation Methods
        private void RecalculateFromDayCount()
        {
            if (!int.TryParse(_view.DayCount, out int dayCount) || dayCount <= 0)
                return;

            if (!DateTime.TryParseExact(_view.SaleDate, DateFormat, null, System.Globalization.DateTimeStyles.None, out DateTime saleDate))
                return;

            DateTime calculatedValidUntil = saleDate.AddDays(dayCount);
            _view.MedicationValidUntil = calculatedValidUntil.ToString(DateFormat);
        }

        private void RecalculateFromMedicationValidUntil()
        {
            if (!DateTime.TryParseExact(_view.MedicationValidUntil, DateFormat, null, System.Globalization.DateTimeStyles.None, out DateTime medicationValidUntil))
                return;

            if (!DateTime.TryParseExact(_view.SaleDate, DateFormat, null, System.Globalization.DateTimeStyles.None, out DateTime saleDate))
                return;

            int calculatedDayCount = (int)(medicationValidUntil - saleDate).TotalDays;
            if (calculatedDayCount > 0)
            {
                _view.DayCount = calculatedDayCount.ToString();
            }
        }

        private void RecalculateAdditionalReimbursedAmount()
        {

            if (!decimal.TryParse(_view.SalePrice, out decimal salePrice))
                salePrice = 0;

            if (!decimal.TryParse(_view.PatientAmount, out decimal patientAmount))
                patientAmount = 0;

            if (!decimal.TryParse(_view.ReimbursedAmount, out decimal reimbursedAmount))
                reimbursedAmount = 0;

            decimal additionalReimbursedAmount = salePrice - (patientAmount + reimbursedAmount);

            _view.AdditionalReimbursedAmount = additionalReimbursedAmount.ToString("F2");
        }
        #endregion
    }
}