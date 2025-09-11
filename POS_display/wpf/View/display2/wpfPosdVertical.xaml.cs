using POS_display.Models.NBO;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Resources;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace POS_display.wpf.View.display2
{
    /// <summary>
    /// Interaction logic for wpfPosd.xaml
    /// </summary>
    public partial class wpfPosdVertical : UserControl
    {
        private int ImageInterval;
        public wpfPosdVertical()
        {
            InitializeComponent();
        }

        public void updateData(Items.posh PoshItem, bool isPaymentUpdate = false, decimal roundingValue = 0.0m)
        {
            try
            {
                decimal chequesum = -1 * PoshItem.ChequeSum;
                decimal insurancesum = -1 * PoshItem.ChequeInsuranceSum;
                gvPosd.DataContext = (from el in PoshItem?.PosdItems ?? new List<Items.posd>()
                                      select new
                                      {
                                          el.barcodename,
                                          el.qty,
                                          el.price,
                                          el.discount_sum,
                                          el.sum,
                                          cheque_sum = -1 * el.cheque_sum,
                                          cheque_sum_insurance = -1 * el.cheque_sum_insurance
                                      });

                var totalSum = PoshItem.TotalSum + PoshItem.ChequeSum + PoshItem.ChequeInsuranceSum;
                lblTotalInfo.Content = "Viso mokėti\n";
                lblTotalValue.Content = totalSum + " EUR\n";

                lblInfo.Content = "";
                lblValue.Content = "";

                if (Session.IsRoundingEnabled && isPaymentUpdate)
                {
                    lblInfo.Content += "Apvalinimo suma\n";
                    lblValue.Content += roundingValue + " EUR\n";
                    lblInfo.Content += "Suma mokant grynais\n";
                    lblValue.Content += totalSum + roundingValue + " EUR\n";
                }

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

        public void HandleRecommendations(List<NBORecommendation> recommendations)
        {
            Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() =>
            {
                HandleControlsVisiblityByRecsAmount(recommendations == null ? 0 : recommendations.Count);
                if (recommendations != null && recommendations.Count > 0)
                    wpfRecommendation2.AppendRecommendationData(recommendations[0]);
                if (recommendations != null && recommendations.Count > 1)
                    wpfRecommendation1.AppendRecommendationData(recommendations[1]);
                if (recommendations != null && recommendations.Count > 2)
                    wpfRecommendation3.AppendRecommendationData(recommendations[2]);
                if (recommendations != null && recommendations.Count > 3)
                    wpfRecommendation4.AppendRecommendationData(recommendations[3]);
            }));
        }

        private void HandleControlsVisiblityByRecsAmount(int amount)
        {
            Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() =>
            {
                if (amount == 0)
                {
                    wpfRecommendationBorder1.Visibility = Visibility.Hidden;
                    wpfRecommendationBorder2.Visibility = Visibility.Hidden;
                    wpfRecommendationBorder3.Visibility = Visibility.Hidden;
                    wpfRecommendationBorder4.Visibility = Visibility.Hidden;
                }
                else if (amount == 1)
                {
                    wpfRecommendationBorder1.Visibility = Visibility.Visible;
                    wpfRecommendationBorder2.Visibility = Visibility.Hidden;
                    wpfRecommendationBorder3.Visibility = Visibility.Hidden;
                    wpfRecommendationBorder4.Visibility = Visibility.Hidden;
                }
                else if (amount == 2)
                {
                    wpfRecommendationBorder1.Visibility = Visibility.Visible;
                    wpfRecommendationBorder2.Visibility = Visibility.Visible;
                    wpfRecommendationBorder3.Visibility = Visibility.Hidden;
                    wpfRecommendationBorder4.Visibility = Visibility.Hidden;
                }
                else if (amount == 3)
                {
                    wpfRecommendationBorder1.Visibility = Visibility.Visible;
                    wpfRecommendationBorder2.Visibility = Visibility.Visible;
                    wpfRecommendationBorder3.Visibility = Visibility.Visible;
                    wpfRecommendationBorder4.Visibility = Visibility.Hidden;
                }
                else
                {
                    wpfRecommendationBorder1.Visibility = Visibility.Visible;
                    wpfRecommendationBorder2.Visibility = Visibility.Visible; 
                    wpfRecommendationBorder3.Visibility = Visibility.Visible;
                    wpfRecommendationBorder4.Visibility = Visibility.Visible;
                }
            }));
        }
    }
}
