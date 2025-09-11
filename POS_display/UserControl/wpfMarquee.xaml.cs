using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace POS_display
{
    /// <summary>
    /// Interaction logic for wpfMarquee.xaml
    /// </summary>
    public partial class wpfMarquee : UserControl
    {
        public wpfMarquee()
        {
            InitializeComponent();
        }

        void Window_Loaded(object sender, RoutedEventArgs e)
        {
            DoubleAnimation doubleAnimation = new DoubleAnimation();
            doubleAnimation.From = -this.ActualWidth;
            doubleAnimation.To = this.ActualWidth;
            doubleAnimation.RepeatBehavior = RepeatBehavior.Forever;
            doubleAnimation.Duration = new Duration(TimeSpan.Parse("0:0:20"));
            Dispatcher.Invoke(
                            new Action(
                                delegate ()
                                {
                                    tbmarquee.BeginAnimation(Canvas.RightProperty, doubleAnimation);
                                }
                            ), null);
        }
    }
}
