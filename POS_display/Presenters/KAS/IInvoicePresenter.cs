using POS_display.Models.KAS;
using POS_display.Models.Partner;
using System.Threading.Tasks;

namespace POS_display.Presenters.KAS
{
    public interface IInvoicePresenter
    {
        Task Init(PosHeader posHeader);
        Task SetPartnerDataByScala(ChequePresent chequePresent);
        Task<bool> CheckSFHeaderExist(string invoice_no);
        void EnableSaving();
        Task<Partner> LoadPartnerData(string ecode);
        void SetPartnerData(Partner partner);
    }
}
