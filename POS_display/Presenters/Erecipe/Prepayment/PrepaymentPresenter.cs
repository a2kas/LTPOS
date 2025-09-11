using POS_display.Views.Erecipe.Prepayment;
using System;
using System.Drawing;

namespace POS_display.Presenters.Erecipe.Prepayment
{
    public class PrepaymentPresenter : IPrepaymentPresenter
    {
        #region Members
        private IPrepaymentView _view;
        private Items.eRecipe.Recipe _eRecipeData;
        #endregion

        #region Constructor
        public PrepaymentPresenter(IPrepaymentView view)
        {
            _view = view ?? throw new ArgumentNullException();
        }
        #endregion

        #region Public methods
        public void SetData(Items.eRecipe.Recipe eRecipeData)
        {
            _eRecipeData = eRecipeData;
            var isEligibleReimbursementText = eRecipeData.IsEligibleReimbursement ? "TAIP" : "NE";

            if (eRecipeData?.AccumulatedSurcharge != null)
            {
                _view.Content.SelectionFont = new Font(_view.Content.Font, FontStyle.Bold);
                _view.Content.AppendText($"Sukaupta priemokų krepšelio suma:");
                _view.Content.SelectionFont = _view.Content.Font;
                _view.Content.AppendText($" {eRecipeData.AccumulatedSurcharge.SurchargeAmount} Eur\n");
                _view.Content.SelectionFont = new Font(_view.Content.Font, FontStyle.Bold);
                _view.Content.AppendText($"Trūkstama suma iki priemokų krepšelio padengimo:");
                _view.Content.SelectionFont = _view.Content.Font;
                _view.Content.AppendText($" {eRecipeData.AccumulatedSurcharge.MissingSurchargeAmount} Eur\n");
                _view.Content.SelectionFont = new Font(_view.Content.Font, FontStyle.Bold);
                _view.Content.AppendText($"Iki kada galioja priemokų sumos padengimas:");
                _view.Content.SelectionFont = _view.Content.Font;
                _view.Content.AppendText($" {eRecipeData.AccumulatedSurcharge.ValidTo:yyyy/MM/dd}\n");
                _view.Content.SelectionFont = new Font(_view.Content.Font, FontStyle.Bold);
            }

            if (eRecipeData?.HasLowIncome != null)
            {
                var hasLowIncomeText = eRecipeData.HasLowIncome.HasLowIncome ? "TAIP" : "NE";
                _view.Content.AppendText($"Pacientas gauna mažas pajamas:");
                _view.Content.SelectionFont = _view.Content.Font;
                _view.Content.AppendText($" {hasLowIncomeText}\n");
                _view.Content.SelectionFont = new Font(_view.Content.Font, FontStyle.Bold);
            }

            _view.Content.AppendText($"Priklauso priemokos kompensacija:");
            _view.Content.SelectionFont = _view.Content.Font;
            _view.Content.AppendText($" {isEligibleReimbursementText}");

        }
        #endregion
    }
}
