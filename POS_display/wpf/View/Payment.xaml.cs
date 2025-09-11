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
    /// Interaction logic for FMDposd.xaml
    /// </summary>
    public partial class Payment : UserControl
    {
        public Payment()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            //Keyboard.Focus(tb2Dcode);
        }

        private void tb2Dcode_KeyDown(object sender, KeyEventArgs e)
        {

        }
    }
}
