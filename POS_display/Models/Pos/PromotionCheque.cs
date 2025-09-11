namespace POS_display.Models.Pos
{
    public class PromotionCheque
    {
        private decimal _cost;

        public decimal HeaderId { get; set; }
        public decimal LineNo { get; set; }
        public string Line { get; set; }
        public decimal Style { get; set; }
        public bool NoLC { get; set; }
        public bool Rimi { get; set; }
        public bool Benu { get; set; }
        public decimal Cost 
        {
            get { return _cost; }
            set { _cost = value; } 
        }

        public bool IsFitByPOSHeader(Items.posh posHeader)
        {
            if (posHeader.TotalSum < _cost)
                return false;

            if (NoLC && posHeader.LoyaltyCardType == "NOLC")
                return true;

            if (Rimi && posHeader.LoyaltyCardType == "RIMI")
                return true;

            if (Benu && posHeader.LoyaltyCardType == "BENU")
                return true;

            return false;
        }
    }
}
