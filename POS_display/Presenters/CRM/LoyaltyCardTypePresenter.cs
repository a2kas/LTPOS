using POS_display.Utils;
using POS_display.Views.CRM;
using System;
using static POS_display.Enumerator;

namespace POS_display.Presenters.CRM
{
    public class LoyaltyCardTypePresenter: BasePresenter, ILoyaltyCardTypePresenter
    {
        #region Members
        private readonly ILoyaltyCardTypeView _view;
        private readonly FeedbackTerminal _feedbackTerminal;
        #endregion

        #region Constructor
        public LoyaltyCardTypePresenter(ILoyaltyCardTypeView view, FeedbackTerminal feedbackTerminal)
        {
            _view = view ?? throw new ArgumentNullException();
            _feedbackTerminal = feedbackTerminal ?? throw new ArgumentNullException();
        }
        #endregion

        #region Public methods
        public void InitUserCreation()
        {
            CRMLoyaltyCardType cardType = CRMLoyaltyCardType.None;

            if (_view.LoyaltyCard.Checked)
                cardType = CRMLoyaltyCardType.Simple;
            else if (_view.B2BCard.Checked)
                cardType = CRMLoyaltyCardType.B2B;

            _feedbackTerminal.CreateAccount(cardType);
        }
        #endregion
    }
}
