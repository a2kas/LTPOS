using Cortex.Client.Model;
using POS_display.Models.Loyalty;
using System;
using System.Collections.Generic;

namespace POS_display.Models.CRM
{
    public class CRMData
    {
        public CRMClientData Account { get; set; }
        public List<ManualVoucher> ManualVouchers { get; set; }
        public PostPurchaseAcceptPayment200Response AcceptedPaymentResponse { get; set; }

        public bool Pensioner
        {
            get
            {
                string dateOfBirth = Account?.BirthDate;
                if (string.IsNullOrEmpty(dateOfBirth) || dateOfBirth.ToDateTime() == DateTime.MinValue)
                    return false;

                DateTime dob;
                if (DateTime.TryParse(dateOfBirth, out dob))
                {
                    DateTime now = DateTime.Now;
                    int age = now.Year - dob.Year;
                    if (now.Month < dob.Month || (now.Month == dob.Month && now.Day < dob.Day))
                        age--;
                    return age >= 60;
                }
                return false;
            }
        }

        public int AccruePoints { get; set; }
    }
}
