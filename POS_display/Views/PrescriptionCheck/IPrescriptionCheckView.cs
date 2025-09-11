using System;
using System.Windows.Forms;

namespace POS_display.Views.PrescriptionCheck
{
    public interface IPrescriptionCheckView
    {
        TextBox Doses { get; set; }

        TextBox QtyDay { get; set; }

        TextBox CountDay { get; set; }

        TextBox TillDate { get; set; }

        TextBox ValidFrom { get; set; }

        DateTimePicker TillDateValue { get; set; }

        DateTimePicker ValidFromValue { get; set; }

        Items.posd PosDModel { get; set; }
    }
}
