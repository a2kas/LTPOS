using POS_display.Models.Pos;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace POS_display.Views
{
    public interface Ipayment
    {
        DataGridView PaymentViewGrid { get; }
        List<Models.payment> payment_list { get; set; }
        Items.posh PoshItem { get; set; }
        decimal PaySum { get; set; }
        decimal DebtorSum { get; set; }
        decimal RestSum { get; set; }
        string lastCheckNo { get; set; }
        event EventHandler<KeyEventArgs> Amount_KeyUp_Event;
        string Info { get; set; }
        Button CashButton { get; }
        Button CardButton { get; }
        bool IsRoundingEnabled { get; }
        CashPaymentRoundingResponse CashRoundingCalculation { get; set; }
    }
}
