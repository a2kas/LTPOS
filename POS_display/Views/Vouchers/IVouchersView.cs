using POS_display.Models.Loyalty;
using System.Collections.Generic;
using System.Windows.Forms;

namespace POS_display.Views.Vouchers
{
    public interface IVouchersView
    {
        RadioButton All { get; set; }

        RadioButton Cheque { get; set; }

        CheckBox InstanceDiscount { get; set; }

        DataGridView VouchersGrid { get; set; }

        Label RecordsStatus { get; set; }

        Button LastPage { get; set; }

        Button FirstPage { get; set; }

        Button PreviousPage { get; set; }

        Button NextPage { get; set; }

        GroupBox ListType { get; set; }

        Button OK { get; set; }

        List<ManualVoucher> Vouchers { get; }

        Items.posh PosHeader { get; set; }
    }
}
