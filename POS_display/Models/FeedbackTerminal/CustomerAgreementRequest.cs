namespace POS_display.Models.FeedbackTerminal
{
    public class CustomerAgreementRequest : BaseRequest
    {
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string PostIndex { get; set; }
    }
}
