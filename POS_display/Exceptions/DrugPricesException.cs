using System;

namespace POS_display.Exceptions
{
    public class DrugPricesException : Exception
    {
        public DrugPricesException()
        {
        }

        public DrugPricesException(string message)
            : base(message)
        {
        }

        public DrugPricesException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
