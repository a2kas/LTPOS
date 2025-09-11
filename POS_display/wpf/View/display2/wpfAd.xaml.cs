using System;
using System.IO;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace POS_display.wpf.View.display2
{
    /// <summary>
    /// Interaction logic for wpfAd.xaml
    /// </summary>
    public partial class wpfAd : UserControl
    {
        private int lastImageIndex = -1;
        private int ImageInterval;
        public wpfAd()
        {
            InitializeComponent();
            System.Windows.Threading.DispatcherTimer dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
            dispatcherTimer.Tick += dispatcherTimer_Tick;
            dispatcherTimer.Interval = new TimeSpan(0, 0, 1);
            dispatcherTimer.Start();
            LoadAnotherImage();
        }
        
        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            this.ImageInterval++;

            if (this.ImageInterval >= 8)
            {
                LoadAnotherImage();
                this.ImageInterval = 0;
            }
        }

        private void LoadAnotherImage()
        {
            try
            {
                var imageAds = Session.IsVerticalDisplay2 ? 
                    Session.ImagesVerticalAd :
                    Session.ImagesAd;

                if (imageAds == null || !imageAds.Any())
                    return;

                Dispatcher.BeginInvoke(
                    DispatcherPriority.Background, new Action(() =>
                {
                    var img = helpers.GetNextFromList(imageAds, ref lastImageIndex);
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
