using POS_display.Models.NBO;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
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
using System.Windows.Threading;

namespace POS_display.wpf.View.display2
{
    /// <summary>
    /// Interaction logic for wpfRecommendations.xaml
    /// </summary>
    public partial class wpfRecommendation : UserControl
    {
        public wpfRecommendation()
        {
            InitializeComponent();
        }

        public void AppendRecommendationData(NBORecommendation rec)
        {
            Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() =>
            {
                try
                {
                    using (MemoryStream stream = new MemoryStream(rec.Picture))
                    {
                        BitmapImage bitmap = new BitmapImage();
                        bitmap.BeginInit();
                        bitmap.CreateOptions = BitmapCreateOptions.PreservePixelFormat;
                        bitmap.CacheOption = BitmapCacheOption.OnLoad;
                        bitmap.UriSource = null;
                        bitmap.StreamSource = stream;
                        bitmap.EndInit();

                        RecommendationImg.Source = bitmap;
                        RecommendationImg.VerticalAlignment = VerticalAlignment.Center;
                        RecommendationImg.HorizontalAlignment = HorizontalAlignment.Center;
                        RecommendationText.Content = CutOffText(rec.Name);
                        RecommendationInfo1.Text = rec.ResolveInfo1Field();
                        RecommendationInfo2.Text = rec.ResolveInfo2Field();
                        RecommendationInfo3.Text = rec.ResolveInfo3Field();
                        if (rec.Discount != 0m)
                            RecommendationInfo3.TextDecorations = TextDecorations.Strikethrough;
                        HeaderText.Content = rec.HeaderText;
                    }
                }
                catch (Exception ex)
                {
                    helpers.alert(Enumerator.alert.display2, ex.Message);
                }
            }));
        }

        private string CutOffText(string text) 
        {
            try
            {
                string completedText = string.Empty;
                int completedTextWidth = 0;

                Font font = new Font(RecommendationText.FontFamily.Source, Convert.ToSingle(RecommendationText.FontSize));
                var parts = text.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                int maxWidth = (int)(MainGrid.ColumnDefinitions[1].ActualWidth + MainGrid.ColumnDefinitions[2].ActualWidth);
                foreach (string part in parts)
                {
                    System.Drawing.Size textSize = System.Windows.Forms.TextRenderer.MeasureText($"{part} ", font);
                    completedTextWidth += textSize.Width;

                    if (completedTextWidth >= maxWidth)
                        break;

                    completedText += $"{part} ";

                }
                return completedText;
            }
            catch 
            {
                return text;
            }
        }
    }
}
