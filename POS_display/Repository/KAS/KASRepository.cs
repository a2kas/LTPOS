using Dapper;
using POS_display.Models.KAS;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace POS_display.Repository.KAS
{
    public class KASRepository : BaseRepository, IKASRepository
    {
        public async Task<ChequePresent> GetBENUMTransaction(long posh_id, int buyer)
        {
            using (var connection = DB_Base.GetConnection())
            {
                return await connection.QueryFirstOrDefaultAsync<ChequePresent>(KASQueries.GetBENUMTransaction, new { posh_id, buyer });
            }
        }

        public async Task<long> GetSFNumber(DateTime startDate)
        {
            using (var connection = DB_Base.GetConnection())
            {
                return await connection.QueryFirstAsync<long>(KASQueries.FindSFNumber, new { startDate });
            }
        }

        public async Task<SFHeader> GetSFHeader(long check_id, string type)
        {
            using (var connection = DB_Base.GetConnection())
            {
                return await connection.QueryFirstOrDefaultAsync<SFHeader>(KASQueries.GetSFHeader, new { check_id, type });
            }
        }

        public async Task<bool> CheckSFHeader(string invoice_no)
        {
            using (var connection = DB_Base.GetConnection())
            {
                var result = await connection.QueryAsync<string>(KASQueries.CheckSFH, new { sask_nr = invoice_no });
                return result.ToList().Count > 0;
            }
        }
    }
}
