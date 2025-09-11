using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Data;

namespace POS_display.wpf
{
    /// <summary>
    /// Interaction logic for ActiveSubstanceSelection.xaml
    /// </summary>
    public partial class ActiveSubstanceSelection : Window
    {
        private ObservableCollection<string> _items;
        private ICollectionView _filteredItems;
        public ActiveSubstanceSelection()
        {
            InitializeComponent();

            _items = new ObservableCollection<string>(Session.ActiveSubstances.Except(new List<string> { "Nenurodyta", "" }).OrderBy(e => e));
            _filteredItems = CollectionViewSource.GetDefaultView(_items);
            SelectionListBox.ItemsSource = _filteredItems;
        }

        public string SelectedValue { get; private set; }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            SetSelectedItem();
        }

        private void SelectionListBox_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            SetSelectedItem();
        }

        private void SearchTextBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            _filteredItems.Filter = item =>
            {
                if (item is string str)
                {
                    return str.ToLower().Contains(SearchTextBox.Text.ToLower());
                }
                return false;
            };

            _filteredItems.Refresh();
        }

        private void SetSelectedItem()
        {
            if (SelectionListBox.SelectedItem != null && !string.IsNullOrWhiteSpace(SelectionListBox.SelectedItem.ToString()))
            {
                SelectedValue = SelectionListBox.SelectedItem.ToString();
                DialogResult = true;
            }
            else
            {
                MessageBox.Show("Pasirinkite veikliają medžiagą");
            }
        }
    }
}
