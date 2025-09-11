using System;

namespace POS_display.Exceptions
{
    public class PrescriptionProductInBasketException : Exception
    {
        public PrescriptionProductInBasketException()
        {
        }

        public PrescriptionProductInBasketException(string message)
            : base(message)
        {
        }

        public PrescriptionProductInBasketException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
