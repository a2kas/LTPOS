using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS_display.Repository.NBO
{
    public static class NBOQueries
    {
        public static string LoadBORecommendationsByPosHeaderID => "SELECT posd_id, recommendation_id FROM nbo_recommendation WHERE @posh_id = posh_id";
        public static string InsertNBORecommendation => "INSERT INTO nbo_recommendation VALUES (@posh_id, @posd_id, @recommendation_id)";
        public static string DeleteNBORecommendationByPosHeaderID => "DELETE FROM nbo_recommendation WHERE posh_id = @posh_id";
        public static string DeleteNBORecommendationByPosDetailID => "DELETE FROM nbo_recommendation WHERE posd_id = @posd_id";
    }
}
