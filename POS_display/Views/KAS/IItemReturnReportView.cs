using System.Threading.Tasks;
using System.Windows.Forms;

namespace POS_display.Views.KAS
{
    public interface IItemReturnReportView
    {
        Label PharmacyNo { get; set; }

        Button PrintButton { get; set; }

        Button CloseButton { get; set; }

        TextBox Address { get; set; }

        TextBox CashDeskNr { get; set; }

        TextBox ChequeNr { get; set; }

        TextBox InsuranceCompany { get; set; }

        TextBox Cashier { get; set; }

        TextBox Buyer { get; set; }

        TextBox ReturnSum { get; set; }

        TextBox Date { get; set; }

        DataGridView ReturningItem { get; set; }

        Task Init();

        Task<decimal> GetRoundingValue();

        Task SendItemsReturnToCashRegister();
    }
}
