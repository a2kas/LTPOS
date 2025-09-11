using POS_display.Models;
using System.Collections.Generic;
using System.Windows.Forms;
using POS_display.Models.General;
using System.Threading.Tasks;

namespace POS_display.Views.SalesOrder
{
    public interface ISalesOrderView
    {
        TextBox ClientName { get; set; }
        TextBox Surename { get; set; }
        TextBox Phone { get; set; }
        TextBox Email { get; set; }
        TextBox Address { get; set; }
        TextBox City { get; set; }
        TextBox PostCode { get; set; }
        ComboBox Country { get; set; }
        TextBox Comment { get; set; }
        List<ClientData> Clients { get; set; }
        Barcode  BarcodeModel { get; set; }
        Items.posh PoshItem { get; set; }
        ClientData FocusedClient { get; set; }
        Button EditClientData { get; set; }
        Button PrintAgreement { get; set; }
        Button SendToTerminal { get; set; }
        string CustomerSignature { get; set; }
        decimal QtyToTransfer { get; set; }
        Task UpdateCurrentSalesOrderPosDetail(long posDetailId);
    }
}
