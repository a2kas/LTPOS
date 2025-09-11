using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using POS_display.Presenters.Erecipe.Dispense;
using System.Windows.Forms;

namespace POS_display.Views.Erecipe.Dispense
{
    public partial class DispensesView : FormBase, IDispensesView, IBaseView
    {
        #region Members
        private IDispensesInfoPresenter _dispensesInfoPresenter;
        #endregion

        #region Constructor
        public DispensesView()
        {
            InitializeComponent();
            _dispensesInfoPresenter = new DispensesInfoPresenter(this, Program.ServiceProvider.GetRequiredService<IMapper>());
        }
        #endregion

        #region Properties
        public DataGridView Dispenses
        {
            get => dgvDispenses;
            set => dgvDispenses = value;
        }

        public string DispensesInfo
        {
            get => lblDispenseInfo.Text;
            set => lblDispenseInfo.Text = value;
        }
        public string FormHeaderText
        {
            get => Text;
            set => Text = value;
        }
        #endregion

        #region Public methods
        public void SetData(Items.eRecipe.Recipe eRecipeItem)
        {
            ExecuteWithWait(() => _dispensesInfoPresenter.SetData(eRecipeItem));
        }
        #endregion
    }
}
