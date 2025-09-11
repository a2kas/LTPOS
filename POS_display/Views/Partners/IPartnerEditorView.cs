using POS_display.Models.Partner;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace POS_display.Views.Partners
{
    public interface IPartnerEditorView
    {
        long Id { get; set; }

        TextBox PartnerName { get; set; }

        ComboBox BuyerType { get; set; }

        ComboBox SupplierType { get; set; }

        TextBox Fax { get; set; }

        TextBox Phone { get; set; }

        TextBox PostIndex { get; set; }

        TextBox Email { get; set; } 

        TextBox CountryCode { get; set; }

        TextBox VatCode { get; set; }

        ComboBox Type { get; set; }

        TextBox CompanyCode { get; set; }

        TextBox Comment { get; set; }

        TextBox Address { get; set; }

        TextBox City { get; set; }

        PartnerEditConfig PartnerEditConfig { get; set; }

        Button Save { get; set; }

        Task Init();

        Task LoadData(decimal partnerId = 0m);

    }
}
