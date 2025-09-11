using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text;
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
    /// Interaction logic for wpfPrices.xaml
    /// </summary>
    public partial class wpfPrices : UserControl
    {
        private System.Windows.Threading.DispatcherTimer TimerMinShow;
        public int MinShowTimerCounter = 0;
        public int MinShowTime = 16;//because 1s is called immediately
        public int LeftTime = 0;
        public bool popup_opened = false;

        public bool hideNonModified;
        private int ScroolIndex = 0;
        public int MaxLinesOnScreen = 15;

        private prices_form_wpf pfForm;
        public wpfPrices()
        {
            InitializeComponent();
            TimerMinShow = new System.Windows.Threading.DispatcherTimer();
            TimerMinShow.Tick += TimerMinShow_Tick;
            TimerMinShow.Interval = new TimeSpan(0, 0, 1);
            gvPricesUncomp.Visibility = Visibility.Hidden;
            gvPricesComp.Visibility = Visibility.Hidden;
        }

        public void ScroolDG(int index, int FirstRowIndex)
        {
            this.ScroolIndex = this.ScroolIndex + index;
            if (this.ScroolIndex < 0)
                this.ScroolIndex = 1;
            this.ScroolIndex = FirstRowIndex;
            try
            {
                ScrollViewer scrollView = GetScrollbar(DG);
                if (scrollView != null)
                    scrollView.ScrollToVerticalOffset(ScroolIndex);
            }
            catch (Exception ex)
            {
                this.ScroolIndex = 0;
                helpers.alert(Enumerator.alert.display2, ex.Message);
            }
        }

        private static ScrollViewer GetScrollbar(DependencyObject dep)
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(dep); i++)
            {
                var child = VisualTreeHelper.GetChild(dep, i);
                if (child != null && child is ScrollViewer)
                    return child as ScrollViewer;
                else
                {
                    ScrollViewer sub = GetScrollbar(child);
                    if (sub != null)
                        return sub;
                }
            }
            return null;
        }

        public void setCurrentCell(int row_index)
        {
            if (row_index >= 0 && row_index < DG.Items.Count)
                DG.SelectedItem = DG.Items[row_index];
        }

        public void ShowInfoForPrice(int price, Items.Prices.Generics generics)
        {
            if (price == 1)//uncompensated
            {
                gvPricesUncomp.Visibility = Visibility.Visible;
                gvPricesComp.Visibility = Visibility.Hidden;
                gvPricesUncomp.DataContext = (from el in generics.ItemsAllList
                                     select new
                                     {
                                         shortname = el.shortname,
                                         RetailPrice = el.RetailPrice,
                                         CompensatedPrice = el.CompensatedPrice
                                     });
                lblCompPercent.Content = "";
                label2.Visibility = Visibility.Hidden;
                gvPricesUncomp.AlternationCount = gvPricesUncomp.Items.Count;
            }
            else//compensated
            {
                gvPricesUncomp.Visibility = Visibility.Hidden;
                gvPricesComp.Visibility = Visibility.Visible;
                gvPricesComp.DataContext = (from el in generics.ItemsAllList
                                     select new
                                     {
                                         shortname = el.shortname,
                                         RetailPrice = el.RetailPrice,
                                         VKBPrice = (price == 100 ? el.VKBPrice100 :
                                         price == 90 ? el.VKBPrice90 :
                                         price == 80 ? el.VKBPrice80 :
                                         price == 50 ? el.VKBPrice50 :
                                         0),
                                         Premium = (price == 100 ? el.Premium100 :
                                         price == 90 ? el.Premium90 :
                                         price == 80 ? el.Premium80 :
                                         price == 50 ? el.Premium50 :
                                         0),
                                         PrepComp = (price == 100 ? el.PrepComp100 :
                                         price == 90 ? el.PrepComp90 :
                                         price == 80 ? el.PrepComp80 :
                                         price == 50 ? el.PrepComp50 :
                                         0)
                                     });
                lblCompPercent.Content = price.ToString();
                label2.Visibility = Visibility.Visible;
                gvPricesComp.AlternationCount = gvPricesComp.Items.Count;
            }

            if (MinShowTimerCounter <= 0)
            {
                TimerMinShow_Tick(new object(), new EventArgs());
                TimerMinShow.Start();
            }
            lblName.Content = generics.ItemsAllList.Count > 0 ? generics.ItemsAllList.First().secatcname : lblName.Content;
            if (this.hideNonModified)
                this.lblModified.Content = "Taip";
            else
                this.lblModified.Content = "Ne";

            /*if (gvPrices.Rows.Count > 0)
                gvPrices.Rows[0].Selected = true;*/
        }

        public string ExecuteFromRemote(string npak_id, decimal dosage, int modified, bool comp, decimal productid, string barcode, int comp_percent)
        {
            string res = "";
            if (npak_id == "")
                return res;
            MinShowTimerCounter = 0;
            if (this.pfForm == null)
                this.pfForm = new prices_form_wpf(this);

            this.pfForm.PassInfo(npak_id, dosage, modified, comp, productid, barcode, comp_percent);
            this.pfForm.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.pfForm.Location = helpers.middleScreen2(pfForm, false);

            Items.Prices.GenericItem im = Session.Generics.ItemsAllList.Where(g => g.NpakId == npak_id && g.rekname == (comp ? "C" : "U")).DefaultIfEmpty(new Items.Prices.GenericItem() { secatcname = "" }).First();
            this.setEmptyValues(im.secatcname, "", dosage, modified);
            this.Focus();
            this.pfForm.ShowDialog();
            if (pfForm.DialogResult == System.Windows.Forms.DialogResult.OK)
                res = pfForm.barcode_res;
            bool open_recipe = false;
            if (pfForm.price_class == 50 || pfForm.price_class == 80 || pfForm.price_class == 90 || pfForm.price_class == 100)
                open_recipe = true;
            pfForm.Dispose();
            pfForm = null;

            /*if (open_recipe)
                Program.Display1.displayRecipeDialog();*///todo could be opened. what about erecipe?
            return res;
        }

        private void setEmptyValues(string name2, string comp, decimal dosage, int modified)
        {
            lblName.Content = name2;
            lblCompPercent.Content = comp;
            lblDosage.Content = dosage.ToString();
            gvPricesComp.DataContext = null;
            gvPricesUncomp.DataContext = null;
        }

        private void TimerMinShow_Tick(object sender, EventArgs e)
        {
            this.MinShowTimerCounter++;
            this.LeftTime = this.MinShowTime - this.MinShowTimerCounter;
            Program.Display2.parent_btnTimer(LeftTime);
            if (pfForm != null)
                pfForm.Timer = LeftTime;
            if (this.MinShowTimerCounter >= this.MinShowTime)
            {
                Session.FileUtils.DeleteMyFiles(Session.LocalIP);

                this.TimerMinShow.Stop();
                if (pfForm == null)
                    Program.Display2.changeScreen();
            }
        }

        private DataGrid DG
        {
            get
            {
                DataGrid dg = new DataGrid();
                if (gvPricesComp.Visibility == Visibility.Visible)
                    dg = gvPricesComp;
                if (gvPricesUncomp.Visibility == Visibility.Visible)
                    dg = gvPricesUncomp;
                return dg;
            }
        }
    }
}
