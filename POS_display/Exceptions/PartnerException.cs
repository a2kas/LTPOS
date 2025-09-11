using System;

namespace POS_display.Exceptions
{
    public class PartnerException : Exception
    {
        public PartnerException()
        {
        }

        public PartnerException(string message)
            : base(message)
        {
        }

        public PartnerException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
