using System.Collections.Generic;

namespace POS_display.Models.NBO
{
    public class NBORecommendationSession
    {
        private bool _needRefresh = false;
        public string SessionId { get; set; }
        public string CustomerId { get; set; }
        public string CustomerIdType { get; set; }
        public Dictionary<long, string> RecommendationsID { get; set; }

        public NBORecommendationSession() 
        {
            ResetSession();
        }

        public void ResetSession()
        {
            SessionId = $"s{helpers.RandomString("0123456789", 10)}";
            CustomerId = string.Empty;
            CustomerIdType = "temporary POS";
            RecommendationsID = new Dictionary<long, string>();
            _needRefresh = true;
        }

        public void AssignSessionValues(string cardNo)
        {
            if (!string.IsNullOrEmpty(cardNo))
            {
                CustomerId = cardNo;
                CustomerIdType = "CardNo";
            }
        }

        public void InitRefresh() 
        {
            _needRefresh = true;
        }

        public void Refresh()
        {
            _needRefresh = false;
        }

        public bool NeedRefresh()
        {
            return _needRefresh;
        }
    }
}
