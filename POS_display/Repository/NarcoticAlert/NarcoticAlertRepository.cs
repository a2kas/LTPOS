using System.Collections.Generic;
using System.Threading.Tasks;
using POS_display.Models.NarcoticAlert;
using Dapper;
using System.Linq;

namespace POS_display.Repository.NarcoticAlert
{
    public class NarcoticAlertRepository : BaseRepository, INarcoticAlertRepository
    {
        #region Members
        private static List<ATCCodifier> _atcCodifiers;
        #endregion

        #region Public methods
        public async Task Load()
        {
            _atcCodifiers = await GetATCCodifiers();
        }

        public async Task<List<ATCCodifier>> GetATCCodifiersByATC(string atc)
        {
            if (_atcCodifiers == null)
                await Load();

            List<ATCCodifier> atcCodifiers = new List<ATCCodifier>();
            foreach (ATCCodifier atcc in _atcCodifiers) 
            {
                foreach (var atcCode in atcc.ATCCodes) 
                {
                    if (atc.StartsWith(atcCode)) 
                    {
                        atcCodifiers.Add(atcc);
                    }
                }
            }
            return await Task.FromResult(atcCodifiers);
        }

        public async Task<List<ATCCodifier>> GetATCCodifiers()
        {
            if (_atcCodifiers != null)
                return _atcCodifiers;

            using (var connection = DB_Base.GetConnection())
            {
                var list = await connection.QueryAsync<ATCCodifier>(NarcoticAlertQueries.GetATCCodifiers);
                return list.ToList();
            }
        }
        #endregion
    }
}
