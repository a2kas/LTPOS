using System;

namespace POS_display.Exceptions
{
    public class HomeModeException : Exception
    {
        public HomeModeException()
        {
        }

        public HomeModeException(string message)
            : base(message)
        {
        }

        public HomeModeException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
