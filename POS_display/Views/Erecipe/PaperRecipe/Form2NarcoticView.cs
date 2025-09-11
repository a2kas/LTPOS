using POS_display.Presenters.Erecipe.PaperRecipe;
using POS_display.Repository.Barcode;
using POS_display.Repository.Recipe;
using System.Windows.Forms;

namespace POS_display.Views.Erecipe.PaperRecipe
{
    public partial class Form2NarcoticView : PaperRecipeBaseView, IForm2NarcoticView
    {
        #region Constructor
        public Form2NarcoticView()
        {
            InitializeComponent();
        }
        #endregion

        #region Override
        public override IBasePaperRecipePresenter CreatePresenterInstance()
        {
            return new Form2NarcoticPresenter(this, Session.KVAP, Session.eRecipeUtils, new RecipeRepository());
        }
        #endregion

        #region Properties
        public TextBox RecipeSerial
        {
            get => tbRecipeSerial;
            set => tbRecipeSerial = value;
        }

        public TextBox RecipeNumber
        {
            get => tbRecipeNumber;
            set => tbRecipeNumber = value;
        }

        public ComboBox CompensationCode
        {
            get => cbCompensationCode;
            set => cbCompensationCode = value;
        }
        #endregion
    }
}
