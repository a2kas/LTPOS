using TamroUtilities.HL7.Models;
using System;
using System.Linq;
using System.Windows.Controls;

namespace POS_display.wpf.View
{
    /// <summary>
    /// Interaction logic for DispenseList.xaml
    /// </summary>
    public partial class RecipeList : UserControl
    {
        private wpf.ViewModel.RecipeListViewModel VM;
        public event EventHandler SelectionChanged_Event;
        public RecipeList(RecipeListDto recipeListDto)
        {
            InitializeComponent();
            VM = new ViewModel.RecipeListViewModel(recipeListDto);
            this.DataContext = VM;
            dataGrid.SelectedIndex = 0;
        }

        public bool IsBusy
        {
            get
            {
                return VM.IsBusy;
            }
            set
            {
                VM.IsBusy = value;
            }
        }

        private void dataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dataGrid.SelectedItems.Count > 1)
                return;
            if (SelectionChanged_Event != null)
            {
                var selected = dataGrid.SelectedItems.Cast<RecipeDto>().ToList();
                SelectionChanged_Event(selected, e);
            }
        }
    }
}
