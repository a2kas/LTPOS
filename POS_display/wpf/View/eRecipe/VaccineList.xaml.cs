using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace POS_display.wpf.View
{
    /// <summary>
    /// Interaction logic for VaccineList.xaml
    /// </summary>
    public partial class VaccineList : UserControl
    {
        private ViewModel.VaccineListViewModel VM;
        public event EventHandler SelectionChanged_Event;

        public VaccineList()
        {
            InitializeComponent();
            VM = new ViewModel.VaccineListViewModel()
            {
                PatientId = "",
                FilterPractitionerId = "",
                FilterOrganizationId = "",
                Status = "",
                DocStatus = "",
                GridSize = 0,
            };
            DataContext = VM;
        }
        public VaccineList(
            string patientId,
            string filterPractitionerId,
            string filterOrganizationId,
            string docType, 
            string status,
            string docstatus,
            int gridsize,
            DateTime dateFrom,
            DateTime dateTo)
        {
            InitializeComponent();
            VM = new ViewModel.VaccineListViewModel()
            {
                PatientId = patientId,
                FilterPractitionerId = filterPractitionerId,
                FilterOrganizationId = filterOrganizationId,
                Status = status,
                DocType = docType,
                DocStatus = docstatus,
                GridSize = gridsize,
                PageIndex = 1,
                DateFrom = dateFrom,
                DateTo = dateTo
            };
            DataContext = VM;
        }

        public async Task RefreshGrid()
        {
            await VM.RefreshCommand.ExecuteAsync();
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
            try
            {
                List<Model.VaccineListModel> current_rows = new List<Model.VaccineListModel>();
                current_rows.AddRange(dataGrid?.SelectedItems.Cast<Model.VaccineListModel>());
                if (current_rows.Count == 0)
                    current_rows.Add(VM.VaccineList.First());
                if (dataGrid?.CurrentCell.Column?.DisplayIndex != 0)
                {
                    foreach (var s in VM.VaccineList)
                        s.selected = false;
                }
                foreach (var s in current_rows)
                    s.selected = true;
                if (    SelectionChanged_Event != null && current_rows.Count() == 1)
                    SelectionChanged_Event(current_rows.First().VaccineOrder, e);
                ScrollViewer scrollView = helpers.GetScrollbar(dataGrid);
                var ScroolIndex = scrollView.VerticalOffset;
                dataGrid.Items?.Refresh();
                scrollView?.ScrollToVerticalOffset(ScroolIndex);
                dataGrid.Focus();
            }
            catch (Exception ex)
            {

            }
        }
        
        public void datagrid_selectAll(object sender, EventArgs e)
        {
            dataGrid.SelectAll();
            foreach (var s in VM.VaccineList)
                s.selected = true;
        }

        public void datagrid_unselectAll(object sender, EventArgs e)
        {
            foreach (var s in VM.VaccineList)
                s.selected = false;
            dataGrid.UnselectAll();
        }
    }
}
