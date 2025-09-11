using POS_display.Models.Loyalty;
using System.Collections.Generic;
using System.Threading.Tasks;
using static POS_display.Enumerator;

namespace POS_display.Repository.Loyalty
{
    public interface ILoyaltyRepository
    {
        Task ClearLoyaltyD(decimal posh_id, decimal posd_id);

        Task CreateLoyaltyH(decimal posh_id, string type, string card_no, decimal active, decimal status,
            decimal accrue_points);

        Task CreateLoyaltyD(decimal posh_id, decimal posd_id, string type, decimal sum_type, decimal sum,
            string description);

        Task DeleteLoyaltyDetailsByPosHeaderIdAndTypes(decimal posh_id, List<string> loyalty_types);

        Task DeleteLoyaltyDetailsByPosHeaderId(decimal posh_id);

        Task<List<LoyaltyDetail>> GetLoyaltyDetailsByPosHeaderId(decimal posh_id);

        Task CreateOrUpdateLoyaltyDetail(decimal posh_id, decimal posd_id, string type, SumType sum_type, decimal sum, string description);
    }
}
