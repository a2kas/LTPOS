using Dapper;
using POS_display.Models.Partner;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace POS_display.Repository.Partners
{
    public class PartnerRepository: BaseRepository, IPartnerRepository
    {
        public async Task<List<Partner>> GetDebtors(PartnerFilterModel filter)
        {
            using (var connection = DB_Base.GetConnection())
            {
                return (await connection.QueryAsync<Partner>(PartnerQueries.SearchDebtor(filter), new { value = $"{filter.Value}%"})).ToList();
            }
        }

        public async Task<Partner> GetDebtorById(decimal id)
        {
            using (var connection = DB_Base.GetConnection())
            {
                return await connection.QueryFirstOrDefaultAsync<Partner>(PartnerQueries.GetDebtorById, new { id });
            }
        }

        public async Task<Partner> GetPartner(string ecode)
        {
            using (var connection = DB_Base.GetConnection())
            {
                return await connection.QueryFirstOrDefaultAsync<Partner>(PartnerQueries.GetPartner, new { ecode });
            }
        }

        public async Task<Partner> GetPartnerByScala(string ecode)
        {
            using (var connection = DB_Base.GetConnection())
            {
                return await connection.QueryFirstOrDefaultAsync<Partner>(PartnerQueries.GetPartnerByScala, new { ecode });
            }
        }

        public async Task<Partner> GetPartnerById(decimal id)
        {
            using (var connection = DB_Base.GetConnection())
            {
                return await connection.QueryFirstOrDefaultAsync<Partner>(PartnerQueries.GetPartnerById, new { id });
            }
        }

        public async Task<List<PartnerType>> GetPartnerByModule(string module) 
        {
            using (var connection = DB_Base.GetConnection())
            {
                return (await connection.QueryAsync<PartnerType>(PartnerQueries.GetPartnerTypesByModule, new { module })).ToList();
            }
        }

        public async Task InsertPartner(Partner partner)
        {
            using (var connection = DB_Base.GetConnection())
            {
                await connection.QueryAsync(PartnerQueries.InsertPartner, new
                {
                    id = partner.Id,
                    name = partner.Name,
                    type = partner.Type,
                    ecode = partner.ECode,
                    tcode = partner.TCode,
                    address = partner.Address,
                    postindex = partner.PostIndex,
                    email = partner.Email,
                    agent = partner.Agent,
                    phone = partner.Phone,
                    fax = partner.Fax,
                    credtypeid = partner.CredTypeId.ToLong(),
                    debtypeid = partner.DebTypeId.ToLong(),
                    balance = partner.Balance.ToDecimal(),
                    credit = partner.Credit.ToDecimal(),
                    descrip = partner.Descrip,
                    old_ecode = (partner.Id - 10000000000),
                    old_ecode_scala = (partner.Id - 10000000000),
                    city = partner.City
                });
            }
        }

        public async Task UpdatePartner(Partner partner)
        {
            using (var connection = DB_Base.GetConnection())
            {
                await connection.QueryAsync(PartnerQueries.UpdatePartner, new
                {
                    id = partner.Id,
                    name = partner.Name,
                    type = partner.Type,
                    ecode = partner.ECode,
                    tcode = partner.TCode,
                    address = partner.Address,
                    postindex = partner.PostIndex,
                    email = partner.Email,
                    agent = partner.Agent,
                    phone = partner.Phone,
                    fax = partner.Fax,
                    credtypeid = partner.CredTypeId.ToLong(),
                    debtypeid = partner.DebTypeId.ToLong(),
                    balance = partner.Balance.ToDecimal(),
                    credit = partner.Credit.ToDecimal(),
                    descrip = partner.Descrip,
                    city = partner.City
                });
            }
        }

        public async Task SetPartnerAgreement(decimal partnerId, string signature)
        {
            using (var connection = DB_Base.GetConnection())
            {
                await connection.QueryAsync(PartnerQueries.SetPartnerAgreement, new
                {
                    partnerId,
                    signature
                });
            }
        }

        public async Task<bool> HasPartnerAgreement(decimal partnerId)
        {
            using (var connection = DB_Base.GetConnection())
            {
                return await connection.QueryFirstAsync<bool>(PartnerQueries.HasPartnerAgreement, new
                {
                    partnerId
                });
            }
        }
    }
}
