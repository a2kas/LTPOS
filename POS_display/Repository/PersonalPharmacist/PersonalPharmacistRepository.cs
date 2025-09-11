using System.Threading.Tasks;
using POS_display.Models.PersonalPharmacist;
using POS_display.Repository.PersonalPharmacist;
using Dapper;
using System.Collections.Generic;
using System.Linq;

namespace POS_display.Repository.PersonalPharmacist
{
    public class PersonalPharmacistRepository : BaseRepository, IPersonalPharmacistRepository
    {
        public async Task InsertPersonalPharmacistData(long posHeaderID, PersonalPharmacistData data)
        {
            using (var connection = DB_Base.GetConnection())
            {
                await connection.ExecuteAsync(PersonalPharmacistQueries.InsertPersonalPharmacistData, new
                {
                        poshid = posHeaderID,
                        clientpersonalcode = data.ClientPersonalCode.ToLong(),
                        hospitalname = data.HospitalName,
                        duedate = data.DueDate,
                        doctorid = data.DoctorID,
                        doctorname = data.DoctorName,
                        doctorsurename = data.DoctorSurename
                });
            }
        }

        public async Task<PersonalPharmacistData> GetPersonalPharmacistData(long posHeaderID)
        {
            using (var connection = DB_Base.GetConnection())
            {
                return await connection.QueryFirstOrDefaultAsync<PersonalPharmacistData>
                    (
                        PersonalPharmacistQueries.GetPersonalPharmacistData,
                        new { poshid = posHeaderID }
                    );
            }
        }

        public async Task DeletePersonalPharmacistByPoshID(long posHeaderID)
        {
            using (var connection = DB_Base.GetConnection())
            {
                await connection.ExecuteAsync(PersonalPharmacistQueries.DeletePersonalPharmacistData, new { poshid = posHeaderID });
            }
        }

        public async Task<List<string>> GetPersonalPharmacistHospitals()
        {
            using (var connection = DB_Base.GetConnection())
            {
                var result = await connection.QueryAsync<string>(PersonalPharmacistQueries.GetPersonalPharmacistHospitals);
                return result.ToList();
            }
        }
    }
}
