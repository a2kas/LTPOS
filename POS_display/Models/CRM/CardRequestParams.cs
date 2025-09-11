
namespace POS_display.Models.CRM
{
    public class CardRequestParams : CRMRequestParams
    {
        public string CustomerId { get; set; }
        public string CardNumber { get; set; }
        public string CardTypeId { get; set; }
    }
}
