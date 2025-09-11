using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;
using System.Linq;
using System;
using System.Data;

namespace POS_display.Repository.NBO
{
    public class NBORepository : BaseRepository, INBORepository
    {
        public async Task<Dictionary<long, string>> LoadNBORecommendationsByPosHeaderID(long posHeaderID)
        {
            using (var connection = DB_Base.GetConnection())
            {
                var result = await connection.QueryAsync<(long, string)>(NBOQueries.LoadBORecommendationsByPosHeaderID, new { posh_id = posHeaderID });
                return result.ToDictionary(x=>x.Item1, x => x.Item2);
            }
        }

        public async Task DeleteNBORecommendationByPosDetailID(long posDetailID)
        {
            using (var connection = DB_Base.GetConnection())
            {
                await connection.ExecuteAsync(NBOQueries.DeleteNBORecommendationByPosDetailID, new { posh_id = posDetailID });
            }
        }

        public async Task InsertNBORecommendation(long posHeaderID, long posDetailID, string recommendationID)
        {
            using (var connection = DB_Base.GetConnection())
            {
                await connection.QueryAsync(NBOQueries.InsertNBORecommendation, new
                {
                    posh_id = posHeaderID,
                    posd_id = posDetailID,
                    recommendation_id = recommendationID
                });
            }
        }

        public async Task DeleteNBORecommendationByPosHeaderID(long posHeaderID)
        {
            using (var connection = DB_Base.GetConnection())
            {
                await connection.ExecuteAsync(NBOQueries.DeleteNBORecommendationByPosHeaderID, new { posh_id = posHeaderID });
            }
        }
    }
}
