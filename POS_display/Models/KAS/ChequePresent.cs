
namespace POS_display.Models.KAS
{
    public class ChequePresent
    {
        public long Id { get; set; }
        public long HId { get; set; }
        public int Buyer { get; set; }
        public decimal TotalValue { get; set; }
        public decimal UsedValue { get; set; }
        public string CHNumber { get; set; }
    }
}
