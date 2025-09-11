namespace POS_display.Models.FeedbackTerminal
{
    public class FeedbackStartRequest : BaseRequest
    {
        public string ReceiptId { get; set; }
        public string GuestNumber { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool StopRequired { get; set; }
        public string Timeout { get; set; }
    }
}
