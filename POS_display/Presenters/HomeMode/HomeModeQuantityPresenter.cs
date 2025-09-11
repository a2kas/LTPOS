using POS_display.Exceptions;
using POS_display.Models.HomeMode;
using POS_display.Repository.Price;
using POS_display.Views.HomeMode;
using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace POS_display.Presenters.HomeMode
{
    public class HomeModeQuantityPresenter : IHomeModeQuantityPresenter
    {
        #region Members
        private readonly IHomeModeQuantityView _view;
        private readonly IPriceRepository _priceRepository;
        private Items.Prices.GenericItem _selectedItem;
        #endregion

        #region Constructor
        public HomeModeQuantityPresenter(
            IHomeModeQuantityView view,
            IPriceRepository priceRepository,
            Items.Prices.GenericItem selectedItem)
        {
            _view = view ?? throw new ArgumentNullException();
            _priceRepository = priceRepository ?? throw new ArgumentNullException();
            _selectedItem = selectedItem ?? throw new ArgumentNullException();
        }
        #endregion

        #region Public methods
        public async Task<HomeModeQuantities> ResolveQuantities() 
        {
            HomeModeQuantities homeModeQuantities = new HomeModeQuantities
            {
                RealQuantity = await ResolvePlainRealQty(),
                HomeQuantity = await ResolvePlainHomeQty(),
                RealQuantityByRatio = await ResolveRealQtyByRatio(),
                HomeQuantityByRatio = await ResolveHomeQtyByRatio()
            };

            return homeModeQuantities;
        }

        public async Task Validate()
        {
            if (!int.TryParse(_view.HomeQuantity.Text, out int homeQty)) 
            {
                throw new HomeModeException("Kiekis į namus privalo būti sveikasis skaičius!");
            }

            if (homeQty == 0)
            {
                throw new HomeModeException("Į namus pristatomas kiekis negali būti 0.");
            }

            if (homeQty > _selectedItem.CurrentTamroQty) 
            {
                throw new HomeModeException($"Kiekis į namus privalo būti mažesnis už Tamro didmenos likutį.\n" +
                    $"[Tamro likutis: {_selectedItem.CurrentTamroQty}]");
            }

            string pattern = @"^D\d+$";
            if (!Regex.IsMatch(_view.RealQuantity.Text, pattern) && !int.TryParse(_view.RealQuantity.Text, out _))
            {
                throw new HomeModeException($"Blogas vaistinėje atiduodamo kiekio formatas!\n" +
                    $"Privalo būti D[skaičius] arba sveikasis skaičius!");
            }

            var realQty = await ResolveRealQtyByRatio();
            if (realQty < 0 || homeQty < 0)
            {
                throw new HomeModeException($"Kiekis negali būti mažesnis už 0!");
            }
            if (realQty > await GetPharmacyQty())
            {
                throw new HomeModeException("Nepakankamas prekės kiekis vaistinėje!");
            }
        }
        #endregion

        #region Private methods
        private async Task<decimal> ResolvePlainHomeQty()
        {
            decimal.TryParse(_view.HomeQuantity.Text, out decimal homeQuantity);
            return await Task.FromResult(homeQuantity);
        }

        private async Task<decimal> ResolvePlainRealQty()
        {
            var realQuantityStr = _view.RealQuantity.Text;
            decimal? ratio = await _priceRepository.GetProductRatio(_selectedItem.ProductId);
            return realQuantityStr.IndexOf("D") > -1 ?
                realQuantityStr.Replace("D", "").ToDecimal() / ratio ?? 1 :
                realQuantityStr.ToDecimal();
        }

        private async Task<decimal> ResolveHomeQtyByRatio()
        {
            decimal.TryParse(_view.HomeQuantity.Text, out decimal homeQuantity);
            decimal? ratio = await _priceRepository.GetProductRatio(_selectedItem.ProductId);
            return homeQuantity * ratio ?? 1;
        }

        private async Task<decimal> ResolveRealQtyByRatio()
        {
            var realQuantityStr = _view.RealQuantity.Text;
            decimal? ratio = await _priceRepository.GetProductRatio(_selectedItem.ProductId);
            return realQuantityStr.IndexOf("D") > -1 ?
                realQuantityStr.Replace("D", "").ToDecimal() :
                realQuantityStr.ToDecimal() * ratio ?? 1;
        }

        private async Task<decimal> GetPharmacyQty()
        {
            var qty = await _priceRepository.GetProductQty(_selectedItem.ProductId.ToLong()) ?? 0;
            var ratio = await _priceRepository.GetProductRatio(_selectedItem.ProductId) ?? 0;
            return qty * ratio;
        }
        #endregion
    }
}
