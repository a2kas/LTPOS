using System.Collections.Generic;

namespace POS_display.Models.NBO
{
    public class NBORecommendationResponse
    {
        public string Description { get; set; }
        public string Id { get; set; }
        public List<NBORecommendation> Recommendations { get; set; }
    }
}
