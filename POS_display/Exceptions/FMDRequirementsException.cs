using System;

namespace POS_display.Exceptions
{
    public class FMDRequirementsException : Exception
    {
        public FMDRequirementsException()
        {
        }

        public FMDRequirementsException(string message)
            : base(message)
        {
        }

        public FMDRequirementsException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
