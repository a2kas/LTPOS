using POS_display.Models.Discount;
using POS_display.Repository.Discount;
using POS_display.Repository.Loyalty;
using POS_display.Views.Discount;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using static POS_display.Enumerator;

namespace POS_display.Presenters.Discount
{
    public class DiscountPresenter: BasePresenter
    {
        #region Members
        private readonly IDiscountView _view;
        private readonly IDiscountRepository _discountRepository;
        private readonly ILoyaltyRepository _loyaltyRepository;
        private const string _manualDiscountType = "P";
        #endregion

        #region Constructor
        public DiscountPresenter(IDiscountView view,
            IDiscountRepository discountRepository,
            ILoyaltyRepository loyaltyRepository)
        {
            _view = view;
            _discountRepository = discountRepository;
            _loyaltyRepository = loyaltyRepository;
        }
        #endregion

        #region Public methods
        public async Task<bool> ApplyManualDiscount(decimal HID, decimal ID, decimal type1, decimal type2, decimal discount_sum, string discount_type) 
        {
            try
            {
                SumType sumType = type2 == 1 ? SumType.Perecent : SumType.Value;
                await _loyaltyRepository.CreateOrUpdateLoyaltyDetail(HID, ID, _manualDiscountType, sumType, discount_sum, discount_type);
                return await _discountRepository.CreateDiscount(HID, ID, type1, type2, discount_sum, discount_type);
            }
            catch
            {
                return false;
            }
        }
        public async Task LoadDiscountCategories() 
        {
            _view.DiscountCategories = await _discountRepository.GetDiscountCategories();
        }
        public async Task LoadDiscountTypes1()
        {
            _view.DiscountTypes1 = await _discountRepository.GetDiscountTypes1();
        }
        public async Task LoadDiscountTypes2()
        {
            decimal hid = _view.SelectedDiscountCategory.Id;
            string perfix = _view.SelectedDiscountCategory.Perfix;

            _view.DiscountTypes2 = await _discountRepository.GetDiscountTypes2(hid, perfix); 

            if (perfix.Equals("LAI") || perfix.Equals("LIA") || perfix.Equals("SPK") ||
                perfix.Equals("SPD") || perfix.StartsWith("W"))
            {
                _view.CardNoTextBox.Enabled = true;
                _view.CalcButton.Enabled = false;
            }
            else
            {
                _view.CardNoTextBox.Enabled = false;
                _view.CalcButton.Enabled = true;
            }
        }
        public async Task LoadDiscounts()
        {
            List<DiscountD> discounts = await _discountRepository.GetDiscounts(_view.SelectedDiscountType2.Type, _view.SelectedDiscountCategory.Perfix);

            bool handInput = false;
            Models.Discount.DiscountType discountCategory = _view.DiscountTypes2.FirstOrDefault(val => val.Type.Equals("4"));
            if (discountCategory != null)
            {
                discountCategory.Type = "1";
                handInput = true;
            }

            _view.DiscountSum.Items.Clear();
            if (discounts.Count > 0)
            {
                foreach (DiscountD discount in discounts)
                {
                    if (discount.Value > 0 && !handInput)
                    {
                        _view.DiscountSum.Items.Add(discount.Value);
                        _view.DiscountSum.DropDownStyle = ComboBoxStyle.DropDownList;
                        _view.DiscountSum.SelectedIndex = 0;
                        _view.DiscountSum.Select();
                    }
                    else
                    {
                        _view.DiscountSum.DropDownStyle = ComboBoxStyle.DropDown;
                        _view.DiscountSum.Select();
                    }
                }
            }
            else
            {
                _view.DiscountSum.DropDownStyle = ComboBoxStyle.DropDown;
                _view.DiscountSum.Select();
            }
        }
        public async Task<bool> CalculateDiscount(decimal poshId, decimal posdId) 
        {
            decimal type1 = _view.SelectedDiscountType1.Type.ToDecimal();
            decimal discount;
            if (_view.DiscountSum.DropDownStyle == ComboBoxStyle.DropDown)
                discount = _view.DiscountSum.Text.ToDecimal();
            else
                discount = _view.DiscountSum.SelectedItem.ToDecimal();
            string cardNo = _view.CardNoTextBox.Text;
            if (_view.CardNoTextBox.Enabled == true)
            {
                if (cardNo.Length == 0)
                {
                    _view.CardNoTextBox.Select();
                    throw new Exception("Nenurodytas kortelės numeris!");
                }
            }
            if (discount >= 0 && (discount <= 100 || !_view.SelectedDiscountType2.Type.Equals("1")))
            {
                if (await ApplyManualDiscount(poshId, posdId, type1, _view.SelectedDiscountType2.Type.ToDecimal(), discount, _view.SelectedDiscountCategory.Perfix + cardNo))
                    return true;
                else
                    throw new Exception("Nuolaidos pritaikyti nepavyko!");                
            }
            else
            {
                _view.DiscountSum.Select();
                throw new Exception("Procentinė nuolaida privalo būti intervale nuo 0 iki 100!");
            }
        }
        public void EnableButtons(object sender)
        {
            if (sender == null) return;
            if (sender is TextBox)
            {
                _view.CalcButton.Enabled = true;
            }
            else if (sender is ComboBox) 
            {
                if (_view.DiscountSum.Text.ToDecimal() > 0)
                    _view.CalcButton.Enabled = true;
                else
                    _view.CalcButton.Enabled = false;
            }
        }
        #endregion
    }
}
