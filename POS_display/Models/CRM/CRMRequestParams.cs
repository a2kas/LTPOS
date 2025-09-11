namespace POS_display.Models.CRM
{
    public class CRMRequestParams
    {
        public int Count { get; set; } = 100;
        public int Offset { get; set; }
        public string SortField { get; set; }
        public string SortDirection { get; set; }
        public bool IsValid { get; set; }
    }
}
