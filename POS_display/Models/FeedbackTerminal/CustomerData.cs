using System;

namespace POS_display.Models.FeedbackTerminal
{
    public class CustomerData
    {
        public string CardNumber { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public string Gender { get; set; }

        public string City { get; set; }

        public string District { get; set; }

        public string Street { get; set; }

        public string DescNumber { get; set; }

        public string OrientNumber { get; set; }

        public DateTime DateOfBirth { get; set; }

        public string Zip { get; set; }

        public bool AgreementPersonalInfoProcessing { get; set; }

        public bool AgreementMarketingCommunication { get; set; }

        public string SignatureBase64 { get; set; }

    }
}
