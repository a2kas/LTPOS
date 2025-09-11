using System;

namespace POS_display.Models.CRM
{
    public class CustomerRequestParams : CRMRequestParams
    {
        public string Email { get; set; }
        public string Phone { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime? BirthDate { get; set; }
    }
}
