using System.ComponentModel;
using System.Text.RegularExpressions;

namespace POS_display.Models.General
{
    public class ClientData
    {
        private string _id;
        private string _name;
        private string _surename;
        private string _phone;
        private string _email;
        private string _address;
        private string _city;
        private string _postCode;
        private string _country;
        private string _comment;

        [Browsable(false)]
        public string ID
        {
            get => _id;
            set => _id = value;
        }

        public string Name
        {
            get => _name;
            set => _name = value;
        }

        public string Surename
        {
            get => _surename;
            set => _surename = value;
        }

        public string Phone
        {
            get => _phone;
            set => _phone = value;
        }

        public string Email
        {
            get => _email;
            set => _email = value;
        }

        public string Address
        {
            get => _address;
            set => _address = value;
        }

        public string City
        {
            get => _city;
            set => _city = value;
        }

        public string PostCode
        {
            get => _postCode;
            set => _postCode = value;
        }

        public string Country
        {
            get => _country;
            set => _country = value;
        }

        public string Comment
        {
            get => _comment;
            set => _comment = value;
        }

        public virtual string Validate()
        {
            string message = string.Empty;
            if (string.IsNullOrEmpty(Name))
                message = "Vardas yra privalomas!";

            if (string.IsNullOrEmpty(Surename))
                message = "Pavardė yra privaloma!";

            if (string.IsNullOrEmpty(Phone))
                message = "Telefono numeris yra privalomas!";

            if (string.IsNullOrEmpty(Email))
                message = "El.paštas yra privalomas!";

            if (string.IsNullOrEmpty(Address))
                message = "Adresas yra privalomas!";

            if (string.IsNullOrEmpty(City))
                message = "Miestas yra privalomas!";

            if (string.IsNullOrEmpty(PostCode))
                message = "Pašto kodas yra privalomas!";

            if (string.IsNullOrEmpty(Country))
                message = "Šalis yra būtina!";

            if(!string.IsNullOrEmpty(Email) && !IsValidEmail(Email))
                message = "Klaidingas El.pašto adreso formatas!";

            if (!string.IsNullOrEmpty(Phone) && !IsPhoneNumber(Phone))
                message = "Klaidingas telefono numerio formatas!";

            return message;
        }

        public bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        public bool IsPhoneNumber(string number)
        {
            return Regex.Match(number, @"^\+?(\d[\d-. ]+)?(\([\d-. ]+\))?[\d-. ]+\d$").Success;
        }
    }
}
