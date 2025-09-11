namespace TransactionService.Models.TransModel
{
    public class BecomeTransactionAmount
    {
        public long BecomeTransactionId { get; internal set; }
        public string ClientOrderNo { get; set; }
        public decimal OrderAmount { get; set; }
        public decimal PaidAmount { get; set; }
        public decimal LeftAmount { get; set; }
        public int Status { get; set; }
    }
}
