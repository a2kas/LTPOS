using System;

namespace POS_display.Exceptions
{
    public class PresentCardException : Exception
    {
        public PresentCardException()
        {
        }

        public PresentCardException(string message)
            : base(message)
        {
        }

        public PresentCardException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
