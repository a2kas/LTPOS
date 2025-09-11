using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using POS_display.Helpers;

namespace POS_display.Views.NarcoticAlert
{
    public interface INarcoticAlertView
    {
        Label Header { get; set; }
        RichTextBoxEx Notification { get; set; }
        DataGridView DrugMaterials { get; set; }
    }
}
