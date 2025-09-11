using POS_display.wpf.ViewModel.display2;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace POS_display.wpf.View.display2
{
    /// <summary>
    /// Interaction logic for wpfPrices.xaml
    /// </summary>
    public partial class Prices: UserControl
    {
        public Prices()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty IsQtyVisibleProperty = 
            DependencyProperty.Register(
      "IsQtyVisibleProperty", 
      typeof(Visibility), 
      typeof(Prices), 
      new PropertyMetadata(Visibility.Visible, new PropertyChangedCallback(OnVisibilityChanged)));
        public Visibility IsQtyVisible
        {
            get
            {
                return (Visibility)this.GetValue(IsQtyVisibleProperty);
            }
            set
            {
                this.SetValue(IsQtyVisibleProperty, value);
            }
        }
        private static void OnVisibilityChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((Prices)d).QtyColumn.Visibility = (Visibility)e.NewValue;
            ((Prices)d).GIColumn.Visibility = (Visibility)e.NewValue;
            ((Prices)d).NonTamroButton.Visibility = (Visibility)e.NewValue;
            ((Prices)d).ActiveSubstanceSearch.Visibility = (Visibility)e.NewValue == Visibility.Visible ? Visibility.Visible : Visibility.Hidden;
            ((Prices)d).PharmaceuticalFormLabel.Visibility = (Visibility)e.NewValue == Visibility.Visible ? Visibility.Hidden : Visibility.Visible;
            ((Prices)d).StrengthValueLabel.Visibility = (Visibility)e.NewValue == Visibility.Visible ? Visibility.Hidden : Visibility.Visible;
            ((Prices)d).PharmaceuticalForm.Visibility = (Visibility)e.NewValue == Visibility.Visible ? Visibility.Visible : Visibility.Hidden;
            ((Prices)d).Strength.Visibility = (Visibility)e.NewValue == Visibility.Visible ? Visibility.Visible : Visibility.Hidden;
            ((Prices)d).Oficina.Visibility = (Visibility)e.NewValue;
            ((Prices)d).Oficina_2.Visibility = (Visibility)e.NewValue;
            ((Prices)d).Stock.Visibility = (Visibility)e.NewValue;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            Keyboard.Focus(DG);
        }

        private void OpenDialogButton_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new ActiveSubstanceSelection();
            if (dialog.ShowDialog() == true)
            {
                var viewModel = (PricesViewModel)this.DataContext;
                viewModel.GenericName = dialog.SelectedValue;
            }
        }
    }
}
