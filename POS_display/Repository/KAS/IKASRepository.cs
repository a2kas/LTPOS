using POS_display.Models.KAS;
using System;
using System.Threading.Tasks;

namespace POS_display.Repository.KAS
{
    public interface IKASRepository
    {
        Task<ChequePresent> GetBENUMTransaction(long posh_id, int buyer);
        Task<long> GetSFNumber(DateTime startDate);
        Task<SFHeader> GetSFHeader(long check_id, string type);
        Task<bool> CheckSFHeader(string invoice_no);
    }
}
