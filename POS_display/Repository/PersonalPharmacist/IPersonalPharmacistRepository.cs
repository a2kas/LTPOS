using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using POS_display.Models.PersonalPharmacist;

namespace POS_display.Repository.PersonalPharmacist
{
    public interface IPersonalPharmacistRepository
    {
        Task InsertPersonalPharmacistData(long posHeaderID, PersonalPharmacistData data);
        Task<PersonalPharmacistData> GetPersonalPharmacistData(long posHeaderID);
        Task DeletePersonalPharmacistByPoshID(long posHeaderID);
        Task<List<string>> GetPersonalPharmacistHospitals();
    }
}
