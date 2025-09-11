using POS_display.Presenters.CRM;
using System.Windows.Forms;

namespace POS_display.Views.CRM
{
    public partial class LoyaltyCardTypeView : FormBase, ILoyaltyCardTypeView
    {
        #region Members
        private readonly ILoyaltyCardTypePresenter _loyaltyCardTypePresenter;
        #endregion

        #region Constructor
        public LoyaltyCardTypeView()
        {
            InitializeComponent();
            _loyaltyCardTypePresenter = new LoyaltyCardTypePresenter(this, Session.FeedbackTerminal);
        }
        #endregion

        #region Properties
        public Button Create
        {
            get => btnCreate;
            set => btnCreate = value;
        }

        public RadioButton LoyaltyCard
        {
            get => rbLoyaltyCard;
            set => rbLoyaltyCard = value;
        }

        public RadioButton B2BCard
        {
            get => rbB2BCard;
            set => rbB2BCard = value;
        }
        #endregion

        #region Private methods
        private void btnCreate_Click(object sender, System.EventArgs e)
        {
            ExecuteWithWait(() =>
            {
                _loyaltyCardTypePresenter.InitUserCreation();
                DialogResult = DialogResult.OK;
            });
        }
        #endregion
    }
}
