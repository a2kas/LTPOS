using Microsoft.Win32;
using System;
using System.Net;

namespace POS_display
{
    class RegistryUtils
    {
        public static string GetValue(string Path, string keyName)
        {
            string rez = "";
            RegistryKey key = Registry.LocalMachine.OpenSubKey(Path);
            if (key == null)
            {
                RegistryUtils.CreateDefaultSettings();
                key = Registry.LocalMachine.OpenSubKey(Path);
            }
            try
            {
                rez = key.GetValue(keyName, "-------------").ToString();
            }
            catch (Exception ex)
            {
                rez = "";
            }
            return rez;
        }

        public static void CreateOnStart(string keyname, string def)
        {
            try
            {
                /*
                string path = @"SOFTWARE\Tamro\display2";
                RegistryKey key = Registry.LocalMachine.OpenSubKey(path, true);
                string rez = key.GetValue(keyname, "-1").ToString();
                if (rez.Equals("-1"))
                    key.SetValue(keyname, def);
                */
            }
            catch (Exception ex)
            {
            }
        }

        public static void CreateDefaultSettings()
        {
            try
            {
                String ipaddr = "127.0.0.1";
                IPAddress[] a = Dns.GetHostByName(Dns.GetHostName()).AddressList;
                for (int i = 0; i < a.Length; i++)
                {
                    Console.WriteLine("IpAddr[{0}]={1}", i, a[i]);
                    ipaddr = a[i].ToString();
                }


                string path = @"software\Tamro\display2";
                Registry.LocalMachine.CreateSubKey(path);
                RegistryKey key = Registry.LocalMachine.OpenSubKey(path, true);
                key.SetValue("host_name", ipaddr);
                String ipaddr1 = ipaddr.Substring(0, ipaddr.LastIndexOf(".")) + ".200";
                key.SetValue("server_host_data", "\\\\" + ipaddr1 + "\\data\\kas\\centras_devel\\receptai\\importas\\files\\_na");
                key.SetValue("server_data_path", "\\\\" + ipaddr1 + "\\data\\kas\\centras_devel\\receptai\\importas\\files\\_na\\");
                key.SetValue("server_images_path", "\\\\" + ipaddr1 + "\\data\\kas\\centras_devel\\img\\display2_images\\");

                RegistryKey rk = Registry.CurrentUser.OpenSubKey(@"Control Panel\Screen Saver.Slideshow", true);
                rk.SetValue("ImageDirectory", "\\\\" + ipaddr1 + "\\data\\www\\kas\\centras_devel\\img\\display2_images\\");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
    }
}
