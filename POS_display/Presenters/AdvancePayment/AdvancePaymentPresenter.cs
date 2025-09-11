using CKas.Contracts.PresentCards;
using POS_display.Exceptions;
using POS_display.Repository.Pos;
using POS_display.Views.AdvancePayment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Tamroutilities.Client;

namespace POS_display.Presenters.AdvancePayment
{
    public class AdvancePaymentPresenter : IAdvancePaymentPresenter
    {
        #region Members
        private readonly IAdvancePaymentView _view;
        private readonly IPosRepository _posRepository;
        private readonly ITamroClient _tamroClient;
        private decimal _advancePaymentId;
        private List<string> _cardCache;
        #endregion

        #region Constructor
        public AdvancePaymentPresenter(IAdvancePaymentView view, IPosRepository posRepository, ITamroClient tamroClient)
        {
            _view = view ?? throw new ArgumentNullException();
            _posRepository = posRepository ?? throw new ArgumentNullException();
            _tamroClient = tamroClient ?? throw new ArgumentNullException();
            _cardCache = new List<string>();
        }
        #endregion

        #region Properties
        public List<string> CardCache 
        {
            get { return _cardCache; } 
            set { _cardCache = value; }
        }
        #endregion

        #region Public methods
        public async Task Init()
        {
            Dictionary<string, string> types = new Dictionary<string, string>
            {
                {"PRESENTCARD", "Dovanų kuponas"},
            };

            _view.AdvancePaymentType.DataSource = new BindingSource(types, null);
            _view.AdvancePaymentType.DisplayMember = "Value";
            _view.AdvancePaymentType.ValueMember = "Key";
            _view.AdvancePaymentType.SelectedIndex = 0;

            EnableControls();

            await Task.FromResult(0);
        }

        public async Task Confirm()
        {
            if (_cardCache.Contains(_view.OrderNumber.Text))
                return;

            if (PresentCardAleardyExistInBasket())
                throw new PresentCardException("Šis dovanų kuponas jau yra pirkinių krepšelyje");

            List<string> cardNumbers = new List<string>() { _view.OrderNumber.Text };
            var presentCards = await _tamroClient.GetAsync<List<PresentCardViewModel>>
                (string.Format(Session.CKasV1GetPresentCard, helpers.BuildQueryString("CardNumbers=", cardNumbers)));
            if (presentCards is null || presentCards.Count == 0)
                throw new PresentCardException("Tokio kodo nėra!");

            var presentCard = presentCards.First();

            if (presentCard.Status == PresentCardStatus.Expired)
                throw new PresentCardException("Dovanų kortelė nebegalioja!");

            if (presentCard.Status == PresentCardStatus.Issued)
                throw new PresentCardException("Dovanų kortelė jau yra išduota ir negali būti parduota");

            if (presentCard.Status == PresentCardStatus.Sold)
                throw new PresentCardException("Dovanų kortelė jau yra panaudota!");

            _view.AdvanceSum.Text = presentCard.Amount.ToString();

            _advancePaymentId = await _posRepository.CreateAdvancePayment(
                _view.PosHeader.Id,
                _view.SelectedAdvancePaymentType,
                _view.OrderNumber.Text,
                presentCard.Amount,
                presentCard.Id);

            if (_advancePaymentId <= 0)
                throw new PresentCardException("Klaida atliekant avansinį mokėjimą");

            _cardCache.Add(_view.OrderNumber.Text);          
            EnableControls();
        }

        public void EnableControls() 
        {
            _view.AdvanceSum.ReadOnly = _view.SelectedAdvancePaymentType == "PRESENTCARD";
        }

        public bool PresentCardAleardyExistInBasket() 
        {
            return _view.PosHeader?.PosdItems?.Any(e => e.Type == "ADVANCEPAYMENT" && e.barcodename == _view.OrderNumber.Text && e.PresentCardId != 0) ?? false;
        }
        #endregion
    }
}
