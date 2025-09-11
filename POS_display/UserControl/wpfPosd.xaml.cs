using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Resources;
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
using System.Windows.Threading;

namespace POS_display
{
    /// <summary>
    /// Interaction logic for wpfPosd.xaml
    /// </summary>
    public partial class wpfPosd : UserControl
    {
        private int ImageInterval;
        public wpfPosd()
        {
            InitializeComponent();
            System.Windows.Threading.DispatcherTimer dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
            dispatcherTimer.Tick += dispatcherTimer_Tick;
            dispatcherTimer.Interval = new TimeSpan(0, 0, 2);
            dispatcherTimer.Start();
            LoadAnotherImage();
        }

        public void updateData(Items.posh PoshItem)
        {
            try
            {
                decimal chequesum = -1 * PoshItem.ChequeSum;
                decimal insurancesum = -1 * PoshItem.ChequeInsuranceSum;
                gvPosd.DataContext = (from el in PoshItem.PosdItems
                                      select new
                                      {
                                          barcodename = el.barcodename,
                                          qty = el.qty,
                                          price = el.price,
                                          discount_sum = el.discount_sum,
                                          sum = el.sum,
                                          cheque_sum = -1 * el.cheque_sum,
                                          cheque_sum_insurance = -1 * el.cheque_sum_insurance
                                      });

                lblTotalInfo.Content = "Viso mokėti\n";
                lblTotalValue.Content = PoshItem.TotalSum + PoshItem.ChequeSum + PoshItem.ChequeInsuranceSum + " EUR\n";

                lblInfo.Content = "";
                lblValue.Content = "";
                if (PoshItem.DiscountSum > 0 || PoshItem.RimiDiscAmount > 0 || chequesum > 0 || insurancesum > 0)
                    sep2.Visibility = Visibility.Visible;
                else
                    sep2.Visibility = Visibility.Hidden;
                if (PoshItem.DiscountSum > 0)
                {
                    lblInfo.Content += "Jūs sutaupote\n";
                    lblValue.Content += PoshItem.DiscountSum + " EUR\n";
                }
                if (PoshItem.RimiDiscAmount > 0)
                {
                    lblInfo.Content += "Mano RIMI pinigai\n";
                    lblValue.Content += PoshItem.RimiDiscAmount + " EUR\n";
                }
                if (chequesum > 0)
                {
                    lblInfo.Content += "GSK kompensuoja\n";
                    lblValue.Content += chequesum + " EUR\n";
                    gvPosd.Columns[5].Visibility = Visibility.Visible;//todo 5
                }
                else
                    gvPosd.Columns[5].Visibility = Visibility.Hidden;//todo 5

                if (insurancesum > 0)
                {
                    lblInfo.Content += "Draudimas kompensuoja\n";
                    lblValue.Content += insurancesum + " EUR\n";
                    gvPosd.Columns[6].Visibility = Visibility.Visible;
                }
                else
                    gvPosd.Columns[6].Visibility = Visibility.Hidden;//todo 6
                ResourceManager rm = Properties.Resources.ResourceManager;
                var bitmap = (Bitmap)rm.GetObject(PoshItem.LoyaltyCardType);
                if (bitmap != null)
                {
                    using (MemoryStream memory = new MemoryStream())
                    {
                        bitmap.Save(memory, ImageFormat.Png);
                        memory.Position = 0;
                        BitmapImage bitmapImage = new BitmapImage();
                        bitmapImage.BeginInit();
                        bitmapImage.StreamSource = memory;
                        bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                        bitmapImage.EndInit();
                        imgCard.Source = bitmapImage;
                    }
                }
                else
                    imgCard.Source = null;
                gvPosd.UpdateLayout();
            }
            catch (Exception ex)
            {
                helpers.alert(Enumerator.alert.display2, ex.Message);
            }
        }

        public bool isEmpty()
        {
            return gvPosd.DataContext == null;
        }

        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            this.ImageInterval++;

            if (this.ImageInterval >= 8)
            {
                this.LoadAnotherImage();
                this.ImageInterval = 0;
            }
        }

        private void LoadAnotherImage()
        {
            try
            {
                if (Session.ImagesSmall == null)
                    return;
                Dispatcher.BeginInvoke(
                    DispatcherPriority.Background, new Action(() =>
                    {
                        //string img = Session.FileUtils.GetRandomImageFromPath(Session.ImagePath, "Small_");
                        //if (!img.Equals(""))
                        var img = helpers.GetRandomFromList(Session.ImagesSmall);
                        if (img != null)
                        {
                            using (MemoryStream stream = new MemoryStream(img))
                            {
                                BitmapImage bitmap = new BitmapImage();
                                bitmap.BeginInit();
                                bitmap.CreateOptions = BitmapCreateOptions.PreservePixelFormat;
                                bitmap.CacheOption = BitmapCacheOption.OnLoad;
                                bitmap.UriSource = null;
                                bitmap.StreamSource = stream;
                                bitmap.EndInit();
                                imgAd.Source = bitmap;
                                imgAd.Stretch = Stretch.Fill;
                            }
                        }
                        else
                            imgAd.Source = null;
                    }));
            }
            catch (Exception ex)
            {
                helpers.alert(Enumerator.alert.display2, ex.Message);
            }
        }
    }
}
