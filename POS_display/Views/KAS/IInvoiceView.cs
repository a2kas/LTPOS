using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace POS_display.Views.KAS
{
    public interface IInvoiceView
    {
        long CreditorId { get; set; }

        TextBox CheckNo { get; set; }

        TextBox CheckDate { get; set; }

        TextBox DocumentDate { get; set; }

        TextBox DocumentNo { get; set; }

        TextBox DebtorEcode { get; set; }

        Button SelDebtor { get; set; }

        TextBox DebtorName { get; set; }

        Button Save { get; set; }

    }
}
