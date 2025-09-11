using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace POS_display.wpf.View
{
    /// <summary>
    /// Interaction logic for DispenseList.xaml
    /// </summary>
    public partial class DispenseList : UserControl
    {
        private wpf.ViewModel.DispenseListViewModel VM;
        public event EventHandler SelectionChanged_Event;

        public DispenseList()
        {
            InitializeComponent();
            VM = new wpf.ViewModel.DispenseListViewModel()
            {
                PatientId = "",
                FilterPractitionerId = "",
                FilterOrganizationId = "",
                Status = "",
                Confirmed = "",
                DocStatus = "",
                GridSize = 0,
                FromDB = false
            };
            this.DataContext = VM;
        }
        public DispenseList(string patientid, string filterPractitionerId, string filterOrganizationId, string status, string confirmed, string docstatus, int gridsize, bool fromDB, DateTime dateFrom, DateTime dateTo, string note2)
        {
            InitializeComponent();
            VM = new wpf.ViewModel.DispenseListViewModel()
            {
                PatientId = patientid,
                FilterPractitionerId = filterPractitionerId,
                FilterOrganizationId = filterOrganizationId,
                Status = status,
                Confirmed = confirmed,
                DocStatus = docstatus,
                GridSize = gridsize,
                PageIndex = 1,
                FromDB = fromDB,
                DateFrom = dateFrom,
                DateTo = dateTo,
                Note2 = note2
        };
            this.DataContext = VM;
        }

        public async Task refreshGrid()
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
                List<wpf.Model.dispenseListModel> current_rows = new List<wpf.Model.dispenseListModel>();
                current_rows.AddRange(dataGrid?.SelectedItems.Cast< wpf.Model.dispenseListModel>());
                if (current_rows.Count == 0)
                    current_rows.Add(VM.DispenseList.First());
                if (dataGrid?.CurrentCell.Column?.DisplayIndex != 0)
                {
                    foreach (var s in VM.DispenseList)
                        s.selected = false;
                }
                foreach (var s in current_rows)
                    s.selected = true;
                if (    SelectionChanged_Event != null && current_rows.Count() == 1)
                    SelectionChanged_Event(current_rows.First().Dispense, e);
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
            foreach (var s in VM.DispenseList)
                s.selected = true;
        }
        public void datagrid_unselectAll(object sender, EventArgs e)
        {
            foreach (var s in VM.DispenseList)
                s.selected = false;
            dataGrid.UnselectAll();
        }
    }
}
