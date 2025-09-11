using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using POS_display.Views.Price;
using POS_display.Repository;
using POS_display.Repository.Price;
using POS_display.Repository.Pos;
using POS_display.Models.Price;

namespace POS_display.Presenters.Price
{
    public class PricePresenter : BasePresenter
    {
        #region Members
        private readonly IPriceView _view;
        private readonly IPriceRepository _priceRepository = null;
        #endregion

        #region Constructor
        public PricePresenter(IPriceView view, IPriceRepository priceRepository)
        {
            _view = view;
            _priceRepository = priceRepository;
        }
        #endregion

        #region Public methods
        public async Task<bool> ChangePosDPrice(PosDPrice posDPrice)
        {
            return await PosRepository.ChangePosDPrice(posDPrice);
        }
        public void EnableButtons()
        {
            if (_view.Price.Text.ToDecimal() > 0)
                _view.CalcButton.Enabled = true;
            else
                _view.CalcButton.Enabled = false;
        }
        #endregion
    }
}
