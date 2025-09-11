using POS_display.Presenters.Erecipe.PaperRecipe;
using POS_display.Repository.Barcode;
using POS_display.Repository.Recipe;
using System.Windows.Forms;

namespace POS_display.Views.Erecipe.PaperRecipe
{
    public partial class Form3CompensatedView : PaperRecipeBaseView, IForm3CompensatedView
    {
        #region Constructor
        public Form3CompensatedView()
        {
            InitializeComponent();
        }
        #endregion

        #region Override
        public override IBasePaperRecipePresenter CreatePresenterInstance()
        {
            return new Form3CompensatedPresenter(this, Session.KVAP, Session.eRecipeUtils, new RecipeRepository());
        }
        #endregion    

        #region Properties
        public TextBox DiseaseCode
        {
            get => tbDiseaseCode;
            set => tbDiseaseCode = value;
        }

        public TextBox AagaSgasNumber
        {
            get => tbCardNo;
            set => tbCardNo = value;
        }

        public ComboBox CompensationCode
        {
            get => cbCompensationCode;
            set => cbCompensationCode = value;
        }

        public TextBox CompensatedSum
        {
            get => tbCompensatedSum;
            set => tbCompensatedSum = value;
        }

        public TextBox PrepaymentCompensatedSum
        {
            get => tbPrepaymentCompensatedSum;
            set => tbPrepaymentCompensatedSum = value;
        }
        #endregion
    }
}
