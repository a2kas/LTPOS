using POS_display.Models.Partner;
using System.Collections.Generic;
using System.Windows.Forms;

namespace POS_display.Views.Partners
{
    public interface IPartnersView
    {
        Label RecordStatus { get; set; }

        Button NextPage { get; set; }

        Button LastPage { get; set; }

        Button FirstPage { get; set; }

        Button PreviousPage { get; set; }

        Button EditPartner { get; set; }

        Button NewPartner { get; set; }

        Button CloseButton { get; set; }

        Button FindButton { get; set; }

        PartnerViewData FocusedPartner { get; set; }

        PartnerEditConfig PartnerEditConfig { get; set; }

        List<PartnerViewData> CurrentPageData { get; set; }

        DataGridView PartnersGridView { get; set; }

        ComboBox FilterByValues { get; set; }

        TextBox FilterValue { get; set; }

        void SetFocusPartner(long partnerId);
    }
}
