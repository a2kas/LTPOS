using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS_display.Models.PersonalPharmacist
{
    public class PersonalPharmacistData
    {

        private string _pharmacyName;
        private string _clientPersonalCode;
        private string _hospitalName;
        private DateTime _dueDate;
        private string _doctorID;
        private string _doctorName;
        private string _doctorSurename;

        public string PharmacyName
        {
            get => _pharmacyName;
            set => _pharmacyName = value;
        }

        public string ClientPersonalCode
        {
            get => _clientPersonalCode;
            set => _clientPersonalCode = value;
        }

        public string HospitalName
        {
            get => _hospitalName;
            set => _hospitalName = value;
        }

        public DateTime DueDate
        {
            get => _dueDate;
            set => _dueDate = value;
        }

        public string DoctorID
        {
            get => _doctorID; 
            set => _doctorID = value;
        }

        public string DoctorName
        {
            get => _doctorName;
            set => _doctorName = value;
        }

        public string DoctorSurename
        {
            get => _doctorSurename;
            set => _doctorSurename = value;
        }
    }
}
