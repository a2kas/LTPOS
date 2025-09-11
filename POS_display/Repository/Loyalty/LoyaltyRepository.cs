using Dapper;
using POS_display.Models.Loyalty;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static POS_display.Enumerator;

namespace POS_display.Repository.Loyalty
{
    public class LoyaltyRepository : BaseRepository, ILoyaltyRepository
    {
        public async Task ClearLoyaltyD(decimal posh_id, decimal posd_id)
        {
            using (var connection = DB_Base.GetConnection())
            {
                await connection.ExecuteAsync(LoyaltyQueries.DeleteLoyaltyD, new {posh_id, posd_id});
            }
        }

        public async Task CreateLoyaltyH(decimal posh_id, string type, string card_no, decimal active, decimal status, decimal accrue_points)
        {
            using (var connection = DB_Base.GetConnection())
            {
                await connection.ExecuteAsync(LoyaltyQueries.CreateLoyaltyH,
                    new
                    {
                        posh_id,
                        type,
                        card_no,
                        active,
                        status,
                        accrue_points
                    });
            }
        }

        public async Task CreateLoyaltyD(decimal posh_id, decimal posd_id, string type, decimal sum_type, decimal sum, string description)
        {
            using (var connection = DB_Base.GetConnection())
            {
                await connection.ExecuteAsync(LoyaltyQueries.CreateLoyaltyD, 
                    new
                    {
                        posh_id,
                        posd_id,
                        type,
                        sum_type,
                        sum,
                        description
                    });
            }
        }

        public async Task DeleteLoyaltyDetailsByPosHeaderIdAndTypes(decimal posh_id, List<string> loyalty_types)
        {
            string types = string.Join(", ", loyalty_types.Select(n => $"'{n}'"));
            using (var connection = DB_Base.GetConnection())
            {
                await connection.ExecuteAsync(string.Format(LoyaltyQueries.DeleteLoyaltyDetailsByPosHeaderIdAndTypes, posh_id, types));
            }
        }

        public async Task DeleteLoyaltyDetailsByPosHeaderId(decimal posh_id)
        {
            using (var connection = DB_Base.GetConnection())
            {
                await connection.ExecuteAsync(LoyaltyQueries.DeleteLoyaltyDetailsByPosHeaderId, new { posh_id });
            }
        }

        public async Task<List<LoyaltyDetail>> GetLoyaltyDetailsByPosHeaderId(decimal posh_id)
        {
            using (var connection = DB_Base.GetConnection())
            {
                return (await connection.QueryAsync<LoyaltyDetail>(LoyaltyQueries.GetLoyaltyDetailsByPosHeaderId, new { posh_id })).ToList();
            }
        }

        public async Task CreateOrUpdateLoyaltyDetail(decimal posh_id, decimal posd_id, string type, SumType sum_type, decimal sum, string description)
        {
            using (var connection = DB_Base.GetConnection())
            {
                await connection.ExecuteAsync(LoyaltyQueries.CreateOrUpdateLoyaltyDetail,
                    new
                    {
                        posh_id,
                        posd_id,
                        type,
                        sum_type = (decimal)sum_type,
                        sum,
                        description
                    });
            }
        }
    }
}
