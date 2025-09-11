using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace POS_display.Utils
{
    public class FileUtils
    {
        public void DeleteMyFiles(string myip)
        {
            try
            {
                var di = new DirectoryInfo(Session.Path);
                var rgFiles = di.GetFiles("*" + myip + "*");
                foreach (var t in rgFiles)
                {
                    File.Delete(Session.Path + "\\" + t.Name);
                }
            }
            catch
            {
            }
        }

        public void GetAllImg()
        {
            try
            {
                Session.ImagesAd = new List<byte[]>();
                Session.ImagesVerticalAd = new List<byte[]>();
                Session.ImagesPOS = new List<byte[]>();
                Session.ImagesAnimation = new List<byte[]>();
                var files_dir = Directory.GetFiles(Session.ImagePath, "*", SearchOption.TopDirectoryOnly);

                foreach (var f in files_dir)
                {
                    var path = f.Split('\\');
                    if (path.Last().StartsWith("ad_"))
                        Session.ImagesAd.Add(File.ReadAllBytes(f));
                    else if (path.Last().StartsWith("vertical_ad_"))
                        Session.ImagesVerticalAd.Add(File.ReadAllBytes(f));
                    else if (path.Last().StartsWith("POS_"))
                        Session.ImagesPOS.Add(File.ReadAllBytes(f));
                    else if (path.Last().StartsWith("Animation_"))
                        Session.ImagesAnimation.Add(File.ReadAllBytes(f));
                }
            }
            catch (Exception ex)
            {
                helpers.alert(Enumerator.alert.error, ex.Message);
            }
        }

    }
}
