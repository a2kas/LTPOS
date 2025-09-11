namespace POS_display.Models.FeedbackTerminal
{
    public class FeedbackStopRequest : BaseRequest
    {
        public string ReceiptId { get; set; }
        public string Timeout { get; set; }
    }
}