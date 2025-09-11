using POS_display.Models.Recipe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;

namespace POS_display.wpf.View
{
    public partial class VaccineEntriesList : UserControl
    {
        private ViewModel.VaccineEntryListViewModel VM;
        public event EventHandler SelectionChanged_Event;
        public VaccineEntriesList(List<VaccinationEntry> vaccineEntries)
        {
            InitializeComponent();
            VM = new ViewModel.VaccineEntryListViewModel(vaccineEntries);
            DataContext = VM;
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
                var selected = dataGrid.SelectedItems.Cast<VaccinationEntry>().ToList();
                SelectionChanged_Event(selected, e);
            }
        }
    }
}
