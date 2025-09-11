using POS_display.Models.General;
using System;
using System.ComponentModel;
using System.Text.RegularExpressions;

namespace POS_display.Models.CRM
{
    public class CRMClientData : ClientData
    {
        private bool _isCardActive;
        private string _cardNumber;
        private string _birthDate;
        private decimal _actualPoints;
        private int _agreementMarketingCommunication;
        private bool _personalPharmacistClient;

        [Browsable(false)]
        public bool IsCardActive
        {
            get => _isCardActive;
            set => _isCardActive = value;
        }

        public string CardNumber
        {
            get => _cardNumber;
            set => _cardNumber = value;
        }

        public string BirthDate
        {
            get => _birthDate;
            set => _birthDate = value;
        }

        [Browsable(false)]
        new public string Comment
        {
            get => base.Comment;
            set => base.Comment = value;
        }

        [Browsable(false)]
        public decimal ActualPoints
        {
            get => _actualPoints;
            set => _actualPoints = value;
        }

        [Browsable(false)]
        public int AgreementMarketingCommunication
        {
            get => _agreementMarketingCommunication;
            set => _agreementMarketingCommunication = value;
        }

        [Browsable(false)]
        public bool PersonalPharmacistClient
        {
            get => _personalPharmacistClient;
            set => _personalPharmacistClient = value;
        }

        public override string Validate()
        {
            string message = string.Empty;

            if (!string.IsNullOrEmpty(Email) && !IsValidEmail(Email))
                message = "Klaidingas El.pašto adreso formatas!";

            if (!string.IsNullOrEmpty(Phone) && !IsCorrectPhoneFormat(Phone))
                message = "Nurodytas telefono numerio formatas privalo būti - 370xxxxxxxx (šalies kodas be {+} priekyje!)";

            DateTime birthdate = DateTime.MinValue;
            if (!string.IsNullOrEmpty(_birthDate) && !DateTime.TryParse(_birthDate, out birthdate))
            {
                message = "Nurodytas neteisingas gimimo dienos datos formatas. Privalo būti mėnesis/diena/metai pvz: 01/20/2000";
            }

            if (birthdate.Date > DateTime.Now.Date)            
                message = "Gimimo data negali būti didesnė už dabartinę datą";            

            return message;
        }

        private bool IsCorrectPhoneFormat(string number)
        {
            return Regex.Match(number, "([0-9]{11})").Success;
        }
    }
}
