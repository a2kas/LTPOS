using System.Windows.Controls;
using System.Windows.Input;

namespace POS_display.wpf.View.display2
{
    /// <summary>
    /// Interaction logic for wpfPrices.xaml
    /// </summary>
    public partial class PricesDisplay1 : UserControl
    {
        public PricesDisplay1()
        {
            InitializeComponent();
            Loaded += (s, e) => Keyboard.Focus(this);
        }
    }
}
