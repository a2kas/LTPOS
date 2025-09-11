namespace POS_display.Models.FeedbackTerminal
{
    public class BaseRequest
    {
        public string FeedbackTerminalAddress { get; set; }
        public string SystemId { get; set; }
        public string NumberOfCashDesk { get; set; }
        public string Employee { get; set; }
    }
}
