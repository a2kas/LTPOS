using POS_display.Models.CRM;
using System.Collections.Generic;
using System.Windows.Forms;

namespace POS_display.Views.CRM
{
    public interface IClientSearchView
    {
        TextBox ClientName { get; set; }
        TextBox Surename { get; set; }
        TextBox Phone { get; set; }
        TextBox Email { get; set; }
        MaskedTextBox BirthDate { get; set; }
        List<CRMClientData> Clients { get; set; }
        CRMClientData FocusedClient { get; set; }
        Button ConfirmButton { get; set; }
        Label BirthDateNote { get; set; }
    }
}
