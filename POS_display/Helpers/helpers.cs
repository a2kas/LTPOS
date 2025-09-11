using Microsoft.Win32;
using Newtonsoft.Json;
using POS_display.Properties;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Windows.Data;
using System.Windows.Forms;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace POS_display
{
    public class helpers
    {
        private static Random random = new Random();
        public static string GetMD5(string pass)
        {
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            md5.ComputeHash(ASCIIEncoding.ASCII.GetBytes(pass));
            byte[] result = md5.Hash;
            StringBuilder passMD5 = new StringBuilder();
            for (int i = 0; i < result.Length; i++)
            {
                passMD5.Append(result[i].ToString("x2"));
            }
            string strr = passMD5.ToString();
            return passMD5.ToString();
        }

        public static string MakeHash(string str)
        {
            byte[] bytes = Encoding.Unicode.GetBytes(str);
            byte[] buffer2 = new SHA1CryptoServiceProvider().ComputeHash(bytes);
            StringBuilder builder = new StringBuilder(buffer2.Length);
            for (int i = 0; i <= (buffer2.Length - 1); i++)
            {
                string str2 = buffer2[i].ToString("X");
                if (str2.Length == 1)
                {
                    builder.Append('0');
                }
                builder.Append(str2);
            }
            return builder.ToString();
        }

        public static DateTime getXMLDateOnly(string date)
        {
            if (date.IndexOf('T') > 0)
                date = date.Substring(0, date.IndexOf('T'));
            return Convert.ToDateTime(date).Date;
        }

        public static string doFormat(string inValue)
        {
            string sValue = inValue.ToString();
            if (sValue.IndexOf(",") == -1)
                sValue = sValue + ",00";
            int ttt = sValue.Length - 1 - sValue.IndexOf(",");
            if ((sValue.IndexOf(",") != -1) && (ttt == 1))
                sValue = sValue + "0";
            return sValue;
        }

        public static Point middleScreen(Form parent, Form dialog)
        {
            dialog.TopMost = false;
            dialog.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            dialog.BringToFront();
            int X = parent.Location.X + parent.Width / 2 - dialog.Width / 2;
            int Y = parent.Location.Y + parent.Height / 2 - dialog.Height / 2;
            Point XY = new Point(X, Y);

            return XY;
        }

        public static Point middleScreen2(Form dialog, bool topMost)//for 2nd display
        {
            dialog.TopMost = topMost;
            dialog.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            dialog.BringToFront();
            int X = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width / 2 - dialog.Width / 2;
            int Y = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height / 2 - dialog.Height / 2;
            Point XY = new Point(X, Y);

            return XY;
        }

        public static void tb_KeyPress(object sender, KeyPressEventArgs e)
        {
            TextBox _TextBox = (sender as TextBox);
            if (e.KeyChar == '.')
                e.KeyChar = ',';
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != ',' && e.KeyChar != '-')
                e.Handled = true;
            if (_TextBox.SelectionStart != 0 && e.KeyChar == '-')
                e.Handled = true;
            if ((e.KeyChar == ',') && (_TextBox.Text.IndexOf(',') > -1))
                e.Handled = true;
            if ((e.KeyChar == ',') && (_TextBox.Text.Length == 0))
            {
                _TextBox.Text = "0,";
                _TextBox.Select(_TextBox.Text.Length, _TextBox.Text.Length);
                e.Handled = true;
            }
            if ((e.KeyChar == ',') && _TextBox.Text.Length - _TextBox.SelectionStart > 2)
                e.Handled = true;
            if (!char.IsControl(e.KeyChar) && (_TextBox.Text.IndexOf(',') > -1) && (_TextBox.Text.Length - _TextBox.Text.IndexOf(',')) > 2 && _TextBox.SelectionStart > _TextBox.Text.IndexOf(','))
                e.Handled = true;
        }

        public static void CopyRowCell(DataGridView gv)
        {
            CopyRowCell(gv, gv?.CurrentRow?.Index ?? -1, gv?.CurrentCell?.ColumnIndex ?? -1);
        }

        public static void CopyRowCell(DataGridView gv, int selected_row, int selected_column)
        {
            try
            {
                System.Windows.Forms.Clipboard.SetText(gv.Rows[selected_row].Cells[selected_column].Value.ToString());
            }
            catch (Exception) { }
        }

        public static bool alert(Items.Error err, bool confirm = false)
        {
            return alert((Enumerator.alert)Enum.Parse(typeof(Enumerator.alert), err.type, true), err.description, confirm);
        }

        public static bool alert(Enumerator.alert alert_type, string message, bool confirm = false)
        {
            bool res = true;
            if (message == "")
                return res;
            if (message.Length > Settings.Default.max_error_text_length)
                message = message.Substring(0, Settings.Default.max_error_text_length) + "...";
            StackTrace stackTrace = new StackTrace();
            var Text = Assembly.GetEntryAssembly().GetName().Name + " - " + stackTrace.GetFrame(1).GetMethod().Name;
            alert dlg = new alert(alert_type, message, Text, confirm);
            dlg.Location = middleScreen2(dlg, true);
            dlg.BringToFront();
            dlg.ShowDialog();
            res = dlg.DialogResult == DialogResult.Yes;//set dialog result

            dlg.Dispose();
            dlg = null;

            return res;
        }

        public static int get_interval(DateTime date_from, DateTime date_to)
        {
            int interval = date_to.Date.Subtract(date_from.Date).Days;
            if (interval > 0)
                return interval;
            else
                return 0;
        }

        public static int betweenday(DateTime date_from, DateTime date_to)
        {
            int interval = date_to.Date.Subtract(date_from.Date).Days;
            return Math.Abs(interval);
        }

        public static int betweenday2(DateTime date_from, DateTime date_to)
        {
            int interval = date_to.Date.Subtract(date_from.Date).Days;
            return interval;
        }

        public static DateTime format_date(string datestr)
        {
            DateTime date;
            int yyyy = 0;
            int MM = 0;
            int dd = 0;
            if (DateTime.TryParse(datestr, out date))
                return date;
            else
            {
                datestr = Regex.Replace(datestr, "[^0-9]+", "", RegexOptions.Compiled);
                if (datestr.Length > 0 && datestr.Length <= 2)
                {
                    yyyy = DateTime.Now.Year;
                    MM = DateTime.Now.Month;
                    dd = int.Parse(datestr);
                }
                if (datestr.Length > 2 && datestr.Length <= 4)
                {
                    yyyy = DateTime.Now.Year;
                    MM = int.Parse(datestr.Substring(0, datestr.Length - 2));
                    dd = int.Parse(datestr.Substring(datestr.Length - 2, 2));
                }
                if (datestr.Length > 4 && datestr.Length <= 7)
                {
                    yyyy = 2000 + int.Parse(datestr.Substring(0, datestr.Length - 4));
                    MM = int.Parse(datestr.Substring(datestr.Length - 4, 2));
                    dd = int.Parse(datestr.Substring(datestr.Length - 2, 2));
                }
                if (datestr.Length > 7 && datestr.Length <= 8)
                {
                    yyyy = int.Parse(datestr.Substring(0, datestr.Length - 4));
                    MM = int.Parse(datestr.Substring(datestr.Length - 4, 2));
                    dd = int.Parse(datestr.Substring(datestr.Length - 2, 2));
                }

                if (yyyy > 0 && yyyy <= 3000 && MM > 0 && MM <= 12 && dd > 0 && dd <= DateTime.DaysInMonth(yyyy, MM))
                {
                    date = new DateTime(yyyy, MM, dd);
                    return date;
                }
                else
                    return DateTime.Now;
            }
        }

        public static XmlNode ReadXML(string xml, string attribute, string namespace_prefix, string namespace_uri)
        {
            XmlDocument xdoc = new XmlDocument();
            xdoc.Load(new StringReader(xml));
            XmlNamespaceManager nsmgr = new XmlNamespaceManager(xdoc.NameTable);
            if (namespace_prefix != "")
                nsmgr.AddNamespace(namespace_prefix, namespace_uri);
            XmlNode xNodelst = xdoc.DocumentElement.SelectSingleNode(attribute, nsmgr);

            return xNodelst;
        }

        public static string toXMLDateTime(DateTime date)
        {
            return date.Date.ToString().Replace(" ", "T").Replace(".", "-");
        }

        public static string toXMLNumber(string txt)
        {
            return txt.Replace(",", ".");
        }

        public static T FromXElement<T>(XElement xElement)
        {
            try
            {
                var xmlSerializer = new XmlSerializer(typeof(T));
                return (T)xmlSerializer.Deserialize(xElement.CreateReader());
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deserializing XML: {ex.Message}");
                throw;
            }
        }

        public static string getTemporaryPath()
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @"\Elektroniniai receptai\";
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            return path;
        }

        public static void ConvertBase64toPDF(string base64BinaryStr, string file_name)
        {
            try
            {
                string file_path = getTemporaryPath() + file_name + ".pdf";
                FileStream stream = new FileStream(file_path, FileMode.Create);
                BinaryWriter writer = new BinaryWriter(stream);
                byte[] sPDFDecoded = Convert.FromBase64String(base64BinaryStr);
                writer.Write(sPDFDecoded, 0, sPDFDecoded.Length);
                writer.Close();
                Process.Start(file_path);
            }
            catch (Exception e)
            {
                helpers.alert(Enumerator.alert.error, "Nepavyksta išsaugoti failo");
                Console.WriteLine(e.Message);
                Console.ReadLine();
            }
        }

        public static void ConvertByteArrayToPDF(byte[] pdfDocument, string file_name)
        {
            try
            {
                string file_path = getTemporaryPath() + file_name + ".pdf";
                FileStream stream = new FileStream(file_path, FileMode.Create);
                BinaryWriter writer = new BinaryWriter(stream);
                writer.Write(pdfDocument, 0, pdfDocument.Length);
                writer.Close();
                Process.Start(file_path);
            }
            catch (Exception e)
            {
                helpers.alert(Enumerator.alert.error, "Nepavyksta išsaugoti failo");
                Console.WriteLine(e.Message);
                Console.ReadLine();
            }
        }

        public static string ConvertPDFtoBase64(string pdfPath)
        {
            try
            {
                byte[] pdfBytes = File.ReadAllBytes(pdfPath);
                string pdfBase64 = Convert.ToBase64String(pdfBytes);
                return pdfBase64;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.ReadLine();
                return "";
            }
        }

        public static void delete_file(string filename)
        {
            try
            {
                System.IO.File.Delete(filename);
            }
            catch (System.IO.IOException e)
            {
                Console.WriteLine(e.Message);
                return;
            }
        }

        public static int getIntID(decimal number)
        {
            return (int)(number - 10000000000);
        }

        public static decimal getDecimalID(int number)
        {
            return (decimal)(number + 10000000000);
        }

        public static DateTime getDateTimeXML(string date)
        {
            if (date == "")
                return new DateTime();
            return XmlConvert.ToDateTime(date, XmlDateTimeSerializationMode.Utc);
        }

        public static void logMessage(string Message, string path, string filename)
        {
            try
            {
                bool new_file = false;
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                if (!File.Exists(path + filename) && filename.Contains(".csv"))
                    new_file = true;
                using (StreamWriter w = File.AppendText(path + filename))
                {
                    if (new_file == true)
                        w.WriteLine("date;billId;posh_id;item_count;transaction_time");
                    w.WriteLine(DateTime.Now + ";" + Message);
                }
            }
            catch (Exception ex)
            {
                //helpers.alert(Enumerator.alert.error, ex.Message);
            }
        }

        public static List<php_config> ReadPHPConfigFile(string path)
        {
            char[] sep = new char[] { '[', ']', '=', ';' };
            List<php_config> result = new List<php_config>();
            try
            {
                result = File.ReadLines(path, Encoding.Default).Where(l => l.StartsWith("$DB_CONF[")).Select(ln => new php_config
                {
                    id = ln.Split(sep).ToList()[1].Replace("\"", "").ToDecimal(),
                    name = ln.Split(sep).ToList()[3].Replace("\"", ""),
                    value = ln.Split(sep).ToList()[5].Replace("\"", "")
                }).ToList();
            }
            catch (Exception ex)
            {
                //helpers.alert(Enumerator.alert.error, ex.Message);
            }
            return result;
        }

        public static string GetRegistryValue(string registryPath, string keyName, string defValue)
        {
            string rez = "";
            try
            {
                RegistryKey reg = Registry.CurrentUser;
                RegistryKey key = reg.OpenSubKey(registryPath);
                if (key == null)
                {
                    reg.CreateSubKey(registryPath);
                    key = reg.OpenSubKey(registryPath, true);
                }
                rez = key.GetValue(keyName, "-1").ToString();
                if (rez == "-1")
                    rez = SetRegistryValue(registryPath, keyName, defValue);
            }
            catch (Exception ex)
            {
                rez = "";
            }
            return rez;
        }

        public static void DeleteRegistryKey(string keyName)
        {
            DeleteRegistryKey(Settings.Default.registry_path, keyName);
        }

        public static void DeleteRegistryKey(string registryPath, string keyName)
        {
            try
            {
                using (RegistryKey key = Registry.CurrentUser.OpenSubKey(registryPath, true))
                {
                    if (key != null)
                    {
                        key.DeleteValue(keyName);
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }

        public static string GetRegistryValue(string keyName, string defValue)
        {
            return GetRegistryValue(Settings.Default.registry_path, keyName, defValue);
        }

        public static string SetRegistryValue(string Path, string keyName, string Value)
        {
            string rez = "";
            try
            {
                RegistryKey reg = Registry.CurrentUser;
                RegistryKey key = reg.OpenSubKey(Path);
                key = reg.OpenSubKey(Path, true);
                key.SetValue(keyName, Value);
                key = reg.OpenSubKey(Path);
                rez = key.GetValue(keyName).ToString();
            }
            catch (Exception)
            {
                rez = "";
            }
            return rez;
        }

        public static T GetRandomFromList<T>(List<T> list)
        {
            var rand = new Random();
            return list.DefaultIfEmpty().ElementAt(rand.Next(list.Count()));
        }

        public static System.Windows.Controls.ScrollViewer GetScrollbar(System.Windows.DependencyObject dep)
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(dep); i++)
            {
                var child = VisualTreeHelper.GetChild(dep, i);
                if (child != null && child is System.Windows.Controls.ScrollViewer)
                    return child as System.Windows.Controls.ScrollViewer;
                else
                {
                    System.Windows.Controls.ScrollViewer sub = GetScrollbar(child);
                    if (sub != null)
                        return sub;
                }
            }
            return null;
        }

        public static string GetLatinString(string msg)
        {
            Encoding iso = Encoding.GetEncoding("ISO-8859-1");
            Encoding utf8 = Encoding.UTF8;
            byte[] utfBytes = utf8.GetBytes(msg);
            byte[] isoBytes = Encoding.Convert(utf8, iso, utfBytes);
            return iso.GetString(isoBytes);
        }
        public static bool IsWindows7
        {
            get
            {
                return (Environment.OSVersion.Version.Major == 6 &
                    Environment.OSVersion.Version.Minor == 1);
            }
        }        

        public static string RandomString(string chars, int length)
        {
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public static byte[] ImageToByte(Image img)
        {
            ImageConverter converter = new ImageConverter();
            return (byte[])converter.ConvertTo(img, typeof(byte[]));
        }

        public static Stream StringToStream(string src)
        {
            byte[] byteArray = Encoding.UTF8.GetBytes(src);
            return new MemoryStream(byteArray);
        }

        public static string BuildQueryString(string separator, List<string> values)
        {
            StringBuilder stringBuilder = new StringBuilder();

            for (int i = 0; i < values.Count; i++)
            {
                stringBuilder.Append(separator);
                stringBuilder.Append(values[i].Trim());

               if (i < values.Count - 1)
               {
                    stringBuilder.Append("&");
               }
            }
            return stringBuilder.ToString();
        }

        public static List<string> GetDistinctList(List<string> values)
        {
            List<string> distinctList = new List<string>();
            for (int i = 0; i < values.Count; i++)
            {
                distinctList.Add(values[i].Trim());
            }
            return distinctList.Distinct().ToList();
        }

        public static T GetNextFromList<T>(List<T> list, ref int lastIndex)
        {
            if (list == null || list.Count == 0)
            {
                return default;
            }

            lastIndex = (lastIndex + 1) % list.Count;
            return list[lastIndex];
        }

        public static string ParseNumberFromString(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return string.Empty;
            }

            Regex regex = new Regex(@"\d+");

            MatchCollection matches = regex.Matches(input);

            if (matches.Count > 0)
            {
                string numberStr = string.Empty;

                foreach (Match match in matches)
                {
                    numberStr += match.Value;
                }

                return numberStr;
            }

            return string.Empty;
        }

        public static string DataGridViewToJson(DataGridView dgv)
        {
            DataTable dt = new DataTable();

            foreach (DataGridViewColumn column in dgv.Columns)
            {
                dt.Columns.Add(column.HeaderText);
            }

            foreach (DataGridViewRow row in dgv.Rows)
            {
                if (!row.IsNewRow)
                {
                    DataRow dr = dt.NewRow();
                    for (int i = 0; i < dgv.Columns.Count; i++)
                    {
                        dr[i] = row.Cells[i].Value ?? DBNull.Value;
                    }
                    dt.Rows.Add(dr);
                }
            }

            return JsonConvert.SerializeObject(dt, Newtonsoft.Json.Formatting.Indented);
        }

        public static DialogResult ShowInputDialogBox(ref string input, string prompt, string title = "Title", int width = 350, int height = 100)
        {
            Size size = new Size(width, height);
            Form inputBox = new Form();

            inputBox.FormBorderStyle = FormBorderStyle.FixedDialog;
            inputBox.ClientSize = size;
            inputBox.Text = title;

            Label label = new Label();
            label.Text = prompt;
            label.Location = new Point(5, 5);
            label.Width = size.Width - 10;
            inputBox.Controls.Add(label);

            TextBox textBox = new TextBox();
            textBox.Size = new Size(size.Width - 10, 23);
            textBox.Location = new Point(5, label.Location.Y + 30);
            textBox.Text = input;
            inputBox.Controls.Add(textBox);

            //Create an OK Button 
            Button okButton = new Button();
            okButton.DialogResult = DialogResult.OK;
            okButton.Name = "okButton";
            okButton.Size = new Size(75, 23);
            okButton.Text = "&OK";
            okButton.Location = new Point(size.Width - 80 - 80, size.Height - 30);
            inputBox.Controls.Add(okButton);

            //Create a Cancel Button
            Button cancelButton = new Button();
            cancelButton.DialogResult = DialogResult.Cancel;
            cancelButton.Name = "cancelButton";
            cancelButton.Size = new Size(75, 23);
            cancelButton.Text = "&Atšaukti";
            cancelButton.Location = new Point(size.Width - 80, size.Height - 30);
            inputBox.Controls.Add(cancelButton);

            inputBox.AcceptButton = okButton;
            inputBox.CancelButton = cancelButton;

            DialogResult result = inputBox.ShowDialog();
            input = textBox.Text;

            return result;
        }

        private static readonly int KeySize = 32;
        private static readonly byte[] FixedIV = Encoding.UTF8.GetBytes("1234567890abcdef");

        public static string GenerateSecretKey()
        {
            using (var rng = new RNGCryptoServiceProvider())
            {
                byte[] key = new byte[KeySize];
                rng.GetBytes(key);
                return Convert.ToBase64String(key);
            }
        }

        public static string HashSHA256(string input)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(input));
                return Convert.ToBase64String(hashBytes);
            }
        }
        public static string Encrypt(string plaintext, string base64Key)
        {
            byte[] key = Convert.FromBase64String(base64Key);
            byte[] plaintextBytes = Encoding.UTF8.GetBytes(plaintext);

            using (Aes aes = Aes.Create())
            {
                aes.Key = key;
                aes.IV = FixedIV;
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;

                using (var encryptor = aes.CreateEncryptor())
                {
                    byte[] encryptedBytes = encryptor.TransformFinalBlock(plaintextBytes, 0, plaintextBytes.Length);
                    return Convert.ToBase64String(encryptedBytes);
                }
            }
        }

        public static string Decrypt(string encryptedBase64, string base64Key)
        {
            byte[] key = Convert.FromBase64String(base64Key);
            byte[] encryptedBytes = Convert.FromBase64String(encryptedBase64);

            using (Aes aes = Aes.Create())
            {
                aes.Key = key;
                aes.IV = FixedIV;
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;

                using (var decryptor = aes.CreateDecryptor())
                {
                    byte[] decryptedBytes = decryptor.TransformFinalBlock(encryptedBytes, 0, encryptedBytes.Length);
                    return Encoding.UTF8.GetString(decryptedBytes);
                }
            }
        }
    }

    public class php_config
    {
        public decimal id { get; set; }
        public string name { get; set; }
        public string value { get; set; }
    }
}