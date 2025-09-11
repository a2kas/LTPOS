using static POS_display.Enumerator;

namespace POS_display.Models.Pos
{
    public class CashPaymentRoundingResponse
    {
        public PosResponseStatus ResponseStatus { get; set; }
        public bool IsValid { get; set; }
        public bool IsPaid { get; set; }
        public decimal CashAmountToFinish { get; set; }
        public decimal CreditAmountToFinish { get; set; }
        public decimal RoundingValue { get; set; }

        public override string ToString()
        {
            return $"{ResponseStatus},{IsValid},{IsPaid},{CashAmountToFinish},{CreditAmountToFinish},{RoundingValue}";
        }
    }
}
