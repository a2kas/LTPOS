namespace POS_display.Models.Loyalty
{
    public class ManualVoucher
    {
        public int? Selected { get; set; }
        public int Qty { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public int RewardPriority { get; set; }
        public int MaxCount { get; set; }
    }
}
