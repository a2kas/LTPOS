using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS_display.Repository.PersonalPharmacist
{
    public static class PersonalPharmacistQueries
    {
        public static string InsertPersonalPharmacistData => @"INSERT INTO PersonalPharmacistData VALUES (
                                                                @poshid, 
                                                                @clientpersonalcode,
                                                                @hospitalname, 
                                                                @duedate,
                                                                @doctorid, 
                                                                @doctorname, 
                                                                @doctorsurename)";
        public static string GetPersonalPharmacistData => "SELECT * FROM PersonalPharmacistData WHERE poshid = @poshid";
        public static string DeletePersonalPharmacistData => "DELETE FROM PersonalPharmacistData WHERE poshid = @poshid";
        public static string GetPersonalPharmacistHospitals => "SELECT * FROM PersonalPharmacistHospital";
    }
}
