using POS_display.Models;
using POS_display.Models.NBO;
using POS_display.Views.Display;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Security;
using System.Windows.Forms;
using TamroUtilities.HL7.Models;

namespace POS_display
{
    public partial class Display2View : FormBase, IDisplay2View
    {
        public int pricesFromTimer_Counter = -1;
        public int pricesFromTimer_ShowTime = 16;//because 1s is called immediately
        public int pricesFromTimer_LeftTime = 0;
        // For Windows Mobile, replace user32.dll with coredll.dll 
        [SuppressUnmanagedCodeSecurity]
        internal static class UnsafeNativeMethods
        {
            [DllImport("user32.dll")]
            public static extern bool RegisterHotKey(IntPtr hWnd, int id, int fsModifiers, int vlc);
        }
        private wpf.View.display2.wpfAd _wpfAd;
        private wpf.View.display2.wpfPosd _wpfPosd;
        private wpf.View.display2.wpfPosdVertical _wpfPosdVertical;
        public wpf.View.display2.PricesDisplay2 _wpfPrices;
        bool lockPrices = false;

        public Display2View()
        {
            InitializeComponent();

            ehDisplay2.BackgroundImage = Session.IsVerticalDisplay2 ? 
                Properties.Resources.Fonas_vertical :
                Properties.Resources.Fonas;
        }

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == 0x0312 && !lockPrices)
            {
                ShowPrices();
            }
            base.WndProc(ref m);
        }

        private void display2_Load(object sender, EventArgs e)
        {
            int width = Screen.PrimaryScreen.Bounds.Width;
            this.Location = new Point(width + 1, 0);
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            // Alt = 1, Ctrl = 2, Shift = 4, Win = 8
            UnsafeNativeMethods.RegisterHotKey(this.Handle, this.GetType().GetHashCode(), 8, (int)'V');
            UnsafeNativeMethods.RegisterHotKey(this.Handle, this.GetType().GetHashCode() + 1, 8, (int)'G');
            UnsafeNativeMethods.RegisterHotKey(this.Handle, this.GetType().GetHashCode() + 1, 8, (int)'N');
            pricesFromTimer.Interval = 1000;
            pricesFromTimer.Tick += pricesFromTimer_Tick;
            _wpfAd = new wpf.View.display2.wpfAd();
            _wpfPosd = new wpf.View.display2.wpfPosd();
            _wpfPosdVertical = new wpf.View.display2.wpfPosdVertical();
            _wpfPrices = new wpf.View.display2.PricesDisplay2();
            ChangeScreen();
            wpf.View.display2.wpfMarquee _wpfMarquee = new wpf.View.display2.wpfMarquee();
            ehMarquee.Child = _wpfMarquee;
        }
        public void ChangeScreen(System.Windows.Controls.UserControl wpf = null)
        {
            if (lockPrices)
                return;
            if (wpf == null)
            {
                if (pfForm != null)
                    return;
                if (PoshItem?.PosdItems?.Count > 0)
                {
                    if (Session.IsVerticalDisplay2)
                        wpf = _wpfPosdVertical;
                    else
                        wpf = _wpfPosd;
                }
                else
                    wpf = _wpfAd;
            }
            ehDisplay2.Child = wpf;
        }

        public void ShowPrices()
        {
            if (pricesFromTimer_LeftTime <= 0)
            {
                using (DrugPricesView dlg = new DrugPricesView())
                {
                    dlg.Location = helpers.middleScreen2(dlg, false);
                    dlg.LoadData();
                    dlg.ShowDialog();
                }
            }
        }

        public Items.Prices.GenericItem ExecuteFromRemote(RecipeDto recipeDTO, ref Display2Tag tags)
        {
            Items.Prices.GenericItem selectedItem = null;
            pricesFromTimer_Counter = -1;
            PricesVM = new wpf.ViewModel.display2.PricesViewModel(recipeDTO, ref tags);
            var wpf = new wpf.View.display2.PricesDisplay1();
            wpf.DataContext = PricesVM;
            if (Visible)
            {
                _wpfPrices.DataContext = PricesVM;
                ChangeScreen(_wpfPrices);
            }
            _wpfPrices = new wpf.View.display2.PricesDisplay2();
            using (pfForm = new Popups.wpf_dlg(wpf, "Kainos"))
            {
                pfForm.Location = helpers.middleScreen(Program.Display1, pfForm);
                pfForm.Activate();
                pfForm.BringToFront();
                pfForm.ShowDialog();
                if (pfForm.DialogResult == DialogResult.OK)
                    selectedItem = PricesVM.gvPricesSelectedRow;
                pfForm = null;
            }
            return selectedItem;
        }

        public Items.Prices.GenericItem ExecuteFromRemote(Models.Barcode model)
        {
            Items.Prices.GenericItem selectedItem = null;
            pricesFromTimer_Counter = -1;

            PricesVM = new wpf.ViewModel.display2.PricesViewModel(model);
            var wpf = new wpf.View.display2.PricesDisplay1
            {
                DataContext = PricesVM
            };

            if (Visible)
            {
                _wpfPrices.DataContext = PricesVM;
                ChangeScreen(_wpfPrices);
                StartTimer();
            }
            _wpfPrices = new wpf.View.display2.PricesDisplay2();
            using (pfForm = new POS_display.Popups.wpf_dlg(wpf, "Kainos"))
            {
                pfForm.Location = helpers.middleScreen(Program.Display1, pfForm);
                pfForm.Activate();
                pfForm.BringToFront();
                pfForm.ShowDialog();
                if (pfForm.DialogResult == DialogResult.OK)
                    selectedItem = PricesVM.gvPricesSelectedRow;
                pfForm = null;
            }
            return selectedItem;
        }

        public void StartTimer()
        {
            if (pricesFromTimer_Counter < 0)
            {
                pricesFromTimer_Counter = 0;
                pricesFromTimer.Start();
            }
        }

        private void pricesFromTimer_Tick(object sender, EventArgs e)
        {
            pricesFromTimer_Counter++;
            pricesFromTimer_LeftTime = this.pricesFromTimer_ShowTime - this.pricesFromTimer_Counter;
            parent_btnTimer(pricesFromTimer_LeftTime);
            if (pfForm != null)
                PricesVM.Timer = pricesFromTimer_LeftTime;
            if (this.pricesFromTimer_Counter >= this.pricesFromTimer_ShowTime)
            {
                Session.FileUtils.DeleteMyFiles(Session.LocalIP);

                pricesFromTimer.Stop();
                Program.Display2.ChangeScreen();
            }
        }

        public void parent_btnTimer(int time)
        {
            if (time > 0)
                lockPrices = true;
            else
                lockPrices = false;
            Program.Display1.display2_timer = time;
        }

        public void LoadRecommendations(List<NBORecommendation> recommendations)
        {
            if (Session.IsVerticalDisplay2)
            {
                _wpfPosdVertical?.HandleRecommendations(recommendations);
            }
            else 
            {
                _wpfPosd?.HandleRecommendations(recommendations);
            }
        }

        Items.posh _PoshItem = null;
        public Items.posh PoshItem
        {
            get { return _PoshItem; }
            set
            {
                _PoshItem = value;
                if (Session.IsVerticalDisplay2)
                {
                    if (_wpfPosdVertical != null)
                    {
                        _wpfPosdVertical.updateData(PoshItem);
                        ChangeScreen();
                    }
                } 
                else
                {
                    if (_wpfPosd != null)
                    {
                        _wpfPosd.updateData(PoshItem);
                        ChangeScreen();
                    }
                }

            }
        }

        public void UpdateData(Items.posh poshItem, bool isPaymentUpdate, decimal roundingValue)
        {
            if (Session.IsVerticalDisplay2)
            {
                if (_wpfPosdVertical != null)
                {
                    _wpfPosdVertical.updateData(PoshItem, isPaymentUpdate, roundingValue);
                    ChangeScreen();
                }
            }
            else
            {
                if (_wpfPosd != null)
                {
                    _wpfPosd.updateData(PoshItem, isPaymentUpdate, roundingValue);
                    ChangeScreen();
                }
            }
        }

        public POS_display.Popups.wpf_dlg pfForm { get; set; }
        public wpf.ViewModel.display2.PricesViewModel PricesVM { get; set; }
    }
}
