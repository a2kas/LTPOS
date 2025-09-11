namespace POS_display.Models.CRM
{
    public class CampaignItemsList
    {
        public string CampaignProductId { get; set; }
        public string LocalItemCode { get; set; }
        public decimal Value { get; set; }
        public string ValueTypeId { get; set; }
        public string ValidFrom { get; set; }
        public string ValidTo { get; set; }
        public string[] DisplayIn { get; set; }
        public int State { get; set; }
        public Enumerator.CRMCustomerType ClientType { get; set; }
    }
}
