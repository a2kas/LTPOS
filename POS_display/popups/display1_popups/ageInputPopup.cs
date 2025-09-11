using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using POS_display.Models;
using POS_display.Presenters;

namespace POS_display
{
    public partial class ageInputPopup : Form
    {
        public bool isOver75 { get; set; }
        public ageInputPopup()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            var dateTimeNow = DateTime.Now;
            if (dateTimeNow.Year - dateTimePicker.Value.Year >= 125)
            {
                helpers.alert(Enumerator.alert.warning, "Bloga gimimo data.");
                return;
            }
            isOver75 = (dateTimeNow.Year - dateTimePicker.Value.Year > 75 ||
                        dateTimeNow.Year - dateTimePicker.Value.Year == 75 && dateTimePicker.Value.Month < dateTimeNow.Month ||
                        dateTimeNow.Year - dateTimePicker.Value.Year == 75 && dateTimePicker.Value.Month == dateTimeNow.Month && dateTimePicker.Value.Day <= dateTimeNow.Day);

            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
