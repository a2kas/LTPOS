using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS_display.Repository.NBO
{
    public interface INBORepository
    {
        Task<Dictionary<long, string>> LoadNBORecommendationsByPosHeaderID(long posHeaderID);
        Task InsertNBORecommendation(long posHeaderID, long posDetailID, string recommendationID);
        Task DeleteNBORecommendationByPosHeaderID(long posHeaderID);
        Task DeleteNBORecommendationByPosDetailID(long posDetailID);
    }
}
