using POS_display.Repository.Recipe;
using POS_display.Views.PrescriptionCheck;
using System;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace POS_display.Presenters.PrescriptionCheck
{
	public class PrescriptionCheckPresenter : BasePresenter, IPrescriptionCheckPresenter
    {
        #region Members
        private readonly IPrescriptionCheckView _view;
		private readonly IRecipeRepository _recipeRepository;
		#endregion

		#region Constructor
		public PrescriptionCheckPresenter(IPrescriptionCheckView view, IRecipeRepository recipeRepository)
        {
			_view = view ?? throw new ArgumentNullException();
			_recipeRepository = recipeRepository ?? throw new ArgumentNullException();
		}
        #endregion

        #region Public methods
		public async Task Init()
		{
			var validFrom = DateTime.Now;
			var quantity = _view.PosDModel.qty;
			var doses = Math.Round(_view.PosDModel.RetailProductRatio * quantity);
			var qtyDay = 1m;
			var countDay = doses;

			if (_view.PosDModel.NotCompensatedRecipeId != 0) 
			{
				var recipeData = await _recipeRepository.GetNotCompensatedRecipeData(_view.PosDModel.NotCompensatedRecipeId);
				doses = Math.Round(recipeData.Doses,2);
				qtyDay = Math.Round(recipeData.QtyDay,2);
				countDay = Math.Round(recipeData.CountDay,2);
				validFrom = recipeData.ValidFrom;
			}

			_view.ValidFromValue.Value = validFrom;
			SetValidFromDisplay(_view.ValidFromValue);
			_view.Doses.Text = doses.ToString();
			_view.CountDay.Text = countDay.ToString();
			_view.QtyDay.Text = qtyDay.ToString();
			ValueChangesCalculation();
		}

		public async Task<decimal> Save() 
		{
			decimal.TryParse(_view.Doses.Text, out decimal doses);
			decimal.TryParse(_view.CountDay.Text, out decimal countDay);
			decimal.TryParse(_view.QtyDay.Text, out decimal qtyDay);
			return await _recipeRepository.CreateNotCompensatedRecipe(_view.PosDModel.hid, 
				_view.PosDModel.id, 
				_view.ValidFromValue.Value,
				doses,
				qtyDay,
				countDay,
				_view.TillDateValue.Value);
		}

		public void ValueChangesCalculation(object sender = null) 
        {
			var salesDate = DateTime.Now;
			decimal.TryParse(_view.Doses.Text, out decimal doses);
			decimal.TryParse(_view.CountDay.Text, out decimal countDay);
			decimal.TryParse(_view.QtyDay.Text, out decimal qtyDay);

			TextBox tb = sender as TextBox;
			if (tb?.Name == "tbDoses" || tb?.Name == "tbQtyDay")
			{
				_view.CountDay.Text = ((int)Math.Floor(doses / qtyDay)).ToString();
				decimal.TryParse(_view.CountDay.Text, out countDay);
			}
			else if (tb?.Name == "tbCountDay") 
			{
				_view.QtyDay.Text = (Math.Truncate(1000 * doses / countDay) / 1000).ToString();
			}
			_view.TillDateValue.Value = salesDate.AddDays((double)countDay + helpers.get_interval(salesDate, _view.ValidFromValue.Value) - 1);
			SetTillDateDisplay(_view.TillDateValue);
		}

		public void SetTillDateDisplay(object sender)
		{
			DateTimePicker dtp = sender as DateTimePicker;
			_view.TillDate.Text = dtp.Value.ToString("yyyy-MM-dd");
		}

		public void SetValidFromDisplay(object sender) 
		{
			DateTimePicker dtp = sender as DateTimePicker;
			_view.ValidFrom.Text = dtp.Value.ToString("yyyy-MM-dd");
		}

		public void QuantityKeyPress(object sender, KeyPressEventArgs e)
		{
			TextBox tb = (sender as TextBox);
			if (e.KeyChar == '.')
				e.KeyChar = ',';
			if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != ',' && e.KeyChar != '-')
				e.Handled = true;
			if ((e.KeyChar == ',') && (tb.Text.IndexOf(',') > -1))
				e.Handled = true;
			if ((e.KeyChar == ',') && (tb.Text.Length == 0))
			{
				tb.Text = "0,";
				tb.Select(tb.Text.Length, tb.Text.Length);
				e.Handled = true;
			}
		}

		public void NumberKeyPress(object sender, KeyPressEventArgs e) 
		{
			if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
				e.Handled = true;
		}

		#endregion
	}
}
