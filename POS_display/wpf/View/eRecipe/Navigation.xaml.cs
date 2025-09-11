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
    /// Interaction logic for Navigation.xaml
    /// </summary>
    public partial class Navigation : UserControl
    {
        public Navigation()
        {
            InitializeComponent();
            PageCount = 0;
            PageIndex = 1;
        }

        #region DependencyProperties
        public static readonly DependencyProperty NavigationClickProperty =
        DependencyProperty.Register(
            "NavigationClick",
            typeof(ICommand),
            typeof(Navigation),
            new UIPropertyMetadata(null));
        public ICommand NavigationClick
        {
            get { return (ICommand)GetValue(NavigationClickProperty); }
            set { SetValue(NavigationClickProperty, value); }
        }

        private static readonly DependencyProperty PageIndexValue =
        DependencyProperty.Register(
        "PageIndex", typeof(int),
        typeof(Navigation),
            new FrameworkPropertyMetadata()
            {
                PropertyChangedCallback = OnPageIndexChanged,
                BindsTwoWayByDefault = true
            }
        );
        public int PageIndex
        {
            get
            {
                return (int)GetValue(PageIndexValue);
            }
            set
            {
                SetValue(PageIndexValue, value);
                changeLabel();
            }
        }

        private static void OnPageIndexChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Navigation source = d as Navigation;
            if (e.OldValue != e.NewValue)
                source.PageIndex = (int)e.NewValue;
        }

        private static readonly DependencyProperty PageCountValue =
    DependencyProperty.Register(
    "PageCount", typeof(int),
    typeof(Navigation),
        new FrameworkPropertyMetadata()
        {
            PropertyChangedCallback = OnPageCountChanged,
            BindsTwoWayByDefault = true
        }
    );

        public int PageCount
        {
            get
            {
                return (int)GetValue(PageCountValue);
            }
            set
            {
                SetValue(PageCountValue, value);
                changeLabel();
            }
        }

        private static void OnPageCountChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Navigation source = d as Navigation;
            if (e.OldValue != e.NewValue)
                source.PageCount = (int)e.NewValue;
        }

        private static readonly DependencyProperty IsBusyValue =
   DependencyProperty.Register(
   "IsBusy", typeof(bool),
   typeof(Navigation)
   );
        public bool IsBusy 
        {
            get
            {
                return (bool)GetValue(IsBusyValue);
            }
            set
            {
                SetValue(IsBusyValue, value);
            }
        }
        #endregion

        private void changeLabel()
        {
            if (PageIndex <= 1)
                btnPrevious.IsEnabled = false;
            else
                btnPrevious.IsEnabled = true;
            if (PageIndex >= PageCount)
                btnNext.IsEnabled = false;
            else
                btnNext.IsEnabled = true;
            lblPageIndex.Content = $"{PageIndex} / {PageCount}";
        }

        private void btnPrevious_Click(object sender, RoutedEventArgs e)
        {
            if (IsBusy)
                return;
            PageIndex--;
            NavigationClick?.Execute(sender);
        }

        private void btnNext_Click(object sender, RoutedEventArgs e)
        {
            if (IsBusy)
                return;
            PageIndex++;
            NavigationClick?.Execute(sender);
        }

        private void lblPageIndex_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (IsBusy)
                return;
            ProgressDialog dlg = new ProgressDialog("", "Įveskite puslapio numerį");
            dlg.ShowDialog();
            if (dlg.DialogResult == System.Windows.Forms.DialogResult.OK)
            {
                int new_page = dlg.Result.ToInt();
                if (new_page > 0 && new_page <= PageCount)
                {
                    PageIndex = new_page;
                    NavigationClick?.Execute(sender);
                }
                else
                    helpers.alert(Enumerator.alert.warning, "Tokio puslapio nėra.");
            }
            dlg.Dispose();
            dlg = null;
        }
    }
}
