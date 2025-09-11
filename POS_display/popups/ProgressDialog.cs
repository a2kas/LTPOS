using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace POS_display
{
    public partial class ProgressDialog : Form
    {
        private string _type;
        private bool allowEmpty;

        [DllImport("user32.dll")]
        private static extern IntPtr SendMessage(IntPtr hWnd, int Msg, int wParam, [MarshalAs(UnmanagedType.LPWStr)] string lParam);
        
        public ProgressDialog(string type, string text, System.Windows.Forms.Form form = null)
        {
            InitializeComponent();
            textBox1.Select();
            ColumnStyle pictureBox1Style = tableLayoutPanel1.ColumnStyles[0];
            if (form == null)
                form = Program.Display1;
            this.Location = helpers.middleScreen(form, this);
            _type = type;
            switch (type)
            {
                case "card":
                    this.UseWaitCursor = true;
                    this.lblProgress.Text = "Braukite kortelę";
                    this.tableLayoutPanel1.SetRowSpan(lblProgress, 2);
                    this.pictureBox1.Image = Properties.Resources.magstripe;
                    this.pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
                    this.textBox1.Visible = false;
                    this.FormBorderStyle = FormBorderStyle.None;
                    this.btnSave.Visible = false;
                    break;
                case "barcode":
                    this.lblProgress.Text = text;
                    this.pictureBox1.Image = Properties.Resources.barcode;
                    this.pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
                    SendMessage(textBox1.Handle, 0x1501, 1, "Barkodas");
                    this.Text = "Skenuokite barkodą";
                    break;
                case "SuspendRecipe":
                    this.lblProgress.Text = "Įrašykite sustabdymo priežastį:";
                    SendMessage(textBox1.Handle, 0x1501, 1, "Įrašykite sustabdymo priežastį");
                    pictureBox1Style.Width = 0;
                    this.pictureBox1.Image = null;
                    break;
                case "CancelRecipe":
                    this.lblProgress.Text = "Įrašykite atšaukimo priežastį:";
                    SendMessage(textBox1.Handle, 0x1501, 1, "Įrašykite atšaukimo priežastį");
                    pictureBox1Style.Width = 0;
                    this.pictureBox1.Image = null;
                    break;
                case "ReserveRecipe":
                    this.lblProgress.Text = "Numatoma pristatymo data:";
                    SendMessage(textBox1.Handle, 0x1501, 1, "Įrašykite numatomą pristatymo datą");
                    pictureBox1Style.Width = 0;
                    this.pictureBox1.Image = null;
                    break;
                case "dispense_warning":
                    this.lblProgress.Font = new Font("Serif", 8, FontStyle.Regular);
                    this.lblProgress.Text = text;
                    this.pictureBox1.Image = SystemIcons.Warning.ToBitmap();
                    this.pictureBox1.SizeMode = PictureBoxSizeMode.CenterImage;
                    SendMessage(textBox1.Handle, 0x1501, 1, "Pranešimo ignoravimo priežastis");
                    break;
                case "Admin":
                    pictureBox1Style.Width = 0;
                    this.pictureBox1.Image = null;
                    this.lblProgress.Text = text;
                    this.textBox1.UseSystemPasswordChar = true;
                    break;
                case "erecipe_first_prescription":
                    Dictionary<string, string> data = new Dictionary<string, string>();
                    data.Add("0", "Klientas atsisakė įsigyti kompensuojamąjį vaistinį preparatą.");
                    data.Add("1", "Sutrikęs pigiausio produkto tiekimas.");
                    data.Add("2", "Neįmanoma parduoti pigiausio produkto nepažeidžiant vidinės pakuotės.");
                    data.Add("3", "Klientas renkasi ne pigiausią kompensuojamą vaistinį preparatą.");
                    cb.DataSource = new BindingSource(data, null);
                    cb.DisplayMember = "Value";
                    cb.ValueMember = "Key";
                    this.lblProgress.Text = text;
                    break;
                case "donation":
                    this.pictureBox1.Image = Properties.Resources.donations.ToBitmap();
                    this.pictureBox1.SizeMode = PictureBoxSizeMode.CenterImage;
                    this.lblProgress.Text = text;
                    break;
                default:
                    pictureBox1Style.Width = 0;
                    this.pictureBox1.Image = null;
                    this.lblProgress.Text = text;
                    allowEmpty = true;
                    break;
            }
            int tmp = lblProgress.Text.Length / 30;
            this.Height += tmp * 5;
            this.Width += tmp * 100;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            var validationMessage = GetDataValidationNotification();
            if (!string.IsNullOrEmpty(validationMessage)) 
            {
                helpers.alert(Enumerator.alert.warning, validationMessage);
                return;
            }
            if (allowEmpty || Result != "")
                this.DialogResult = DialogResult.OK;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        #region Variables

        #region Private methods
        private string GetDataValidationNotification()
        {
            if (_type == "ReserveRecipe")
            {
                string format = "yyyy.MM.dd";
                if (!DateTime.TryParseExact(Result, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out _)) 
                {
                    return "Netinkamas datos formatas. Data privalo būti įvesta tokiu formatu: metai.mėnesis.diena pvz.: 2010.01.30";
                }
            }
            return string.Empty;
        }
        #endregion

        public string Result
        {
            get
            {
                if (this.tableLayoutPanel2.Controls.Contains(textBox1))
                    return textBox1.Text;
                else
                    return ((KeyValuePair<string, string>)cb.SelectedItem).Value;
            }
            set
            {
                if (this.tableLayoutPanel2.Controls.Contains(textBox1))
                    textBox1.Text = value;
                else
                    cb.SelectedValue = value;
            }
        }

        private ComboBox _cb;
        public ComboBox cb
        {
            get
            {
                if (_cb != null)
                    return _cb;
                _cb = new ComboBox();
                _cb.Anchor = textBox1.Anchor;
                _cb.Location = textBox1.Location;
                _cb.Size = textBox1.Size;
                _cb.TabIndex = textBox1.TabIndex;
                _cb.DropDownStyle = ComboBoxStyle.DropDownList;
                this.tableLayoutPanel2.Controls.Remove(this.textBox1);
                this.tableLayoutPanel2.Controls.Add(_cb, 0, 0);
                return _cb;
            }
        }
        #endregion
    }
}
