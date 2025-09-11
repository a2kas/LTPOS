using System;

namespace POS_display.Exceptions
{
    public class CRMException : Exception
    {
        public CRMException()
        {
        }

        public CRMException(string message)
            : base(message)
        {
        }

        public CRMException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
