using System;

namespace POS_display.Exceptions
{
    public class PdfSignatureException : Exception
    {
        public PdfSignatureException()
        {
        }

        public PdfSignatureException(string message)
            : base(message)
        {
        }

        public PdfSignatureException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}

