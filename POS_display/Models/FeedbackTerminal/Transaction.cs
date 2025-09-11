using System;

namespace POS_display.Models.FeedbackTerminal
{
    public class Transaction
    {
        public Guid TransactionId { get; set; }

        public ControllerName TransactionType { get; set; }

        public DateTime TimeStamp { get; private set; }

        public string FeedbackTerminalAddress { get; set; }

        public string SystemId { get; set; }

        public string NumberOfCashDesk { get; set; }

        public string UserName { get; set; }

        public string CustomerCardNumber { get; set; }

        public float CustomerAccruedPoints { get; set; }

        public CustomerData CustomerData { get; set; }

        public State TransactionState { get; set; }

        public string Info { get; set; }

        public string Employee { get; set; }

        public string MessageType { get; set; }

    }
}
