using POS_display.Models.Partner;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace POS_display.Repository.Partners
{
    public interface IPartnerRepository
    {
        Task<List<Partner>> GetDebtors(PartnerFilterModel filter);
        Task<Partner> GetPartner(string ecode);
        Task<Partner> GetPartnerByScala(string ecode);
        Task<Partner> GetPartnerById(decimal id);
        Task<Partner> GetDebtorById(decimal id);
        Task<List<PartnerType>> GetPartnerByModule(string module);
        Task InsertPartner(Partner partner);
        Task UpdatePartner(Partner partner);
        Task SetPartnerAgreement(decimal partnerId, string signature);
        Task<bool> HasPartnerAgreement(decimal partnerId);
    }
}
