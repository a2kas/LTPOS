using System;

namespace POS_display.Models.Pos
{
    public class PosPayment
    {
        public decimal Id { get; set; }
        public decimal Hid { get; set; }
        public DateTime CreatedDate { get; set; }
        public PosPaymentType PaymentType { get; set; }
        public string PaymentTypeString => PaymentType.ToString();
        public string Code { get; set; }
        public decimal? PaymentId { get; set; }
        public decimal Amount { get; set; }
        public bool Deleted { get; set; }
    }
}
