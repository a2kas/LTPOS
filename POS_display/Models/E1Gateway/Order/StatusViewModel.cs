namespace POS_display.Models.E1Gateway.Order
{
    public enum StatusViewModel
    {
        New = 10,
        CounterCreated = 15,
        Sending = 16,
        Sent = 20,
        Invoiced = 30,
        Error = -10,
        Consolidated = 40,
        Processing = 50,
    }
}
