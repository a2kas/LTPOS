using POS_display.Helpers;
using POS_display.Presenters.Erecipe.Prepayment;
using System.Windows.Forms;

namespace POS_display.Views.Erecipe.Prepayment
{
    public partial class PrepaymentView : FormBase, IPrepaymentView, IBaseView
    {
        #region Members
        private IPrepaymentPresenter _prepaymentPresenter;
        #endregion

        #region Contstructor
        public PrepaymentView()
        {
            InitializeComponent();
            _prepaymentPresenter = new PrepaymentPresenter(this);
        }
        #endregion

        #region Properties
        public Button CloseButton
        {
            get => btnClose;
            set => btnClose = value;
        }

        public RichTextBoxEx Content 
        {
            get => rtbContent;
            set => rtbContent = value;
        }
        #endregion

        #region Public methods
        public void SetData(Items.eRecipe.Recipe eRecipeData) 
        {
            ExecuteWithWait(() => _prepaymentPresenter.SetData(eRecipeData));
        }
        #endregion

        #region Private methods
        private void btnClose_Click(object sender, System.EventArgs e)
        {
            Close();
        }
        #endregion
    }
}
