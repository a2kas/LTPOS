using POS_display.Models.Partner;

namespace POS_display.Repository.Partners
{
    public static class PartnerQueries
    {
        public static string SearchDebtor(PartnerFilterModel search)
        {
            var searchByArgs = search.SearchByKey.Split(new char[] {':'});
            var query = @"SELECT
                            id,
                            name,
                            type,
                            ecode,
                            tcode,
                            address,
                            agent,
                            old_ecode_scala AS Scala,
                            debtortypename,
                            descrip,
                            email,
                            phone,
                            city,
                            postindex
                        FROM search_debtor_v2 ";

            for (int i = 0; i < searchByArgs.Length; i++)
            {
                if (i == 0)
                    query += "WHERE ";
                else
                    query += "OR ";
                query += string.Format("nls_lower({0},'NLS_SORT = LITHUANIAN') LIKE nls_lower(@value,'NLS_SORT = LITHUANIAN') ", searchByArgs[i]);
            }
            query += " ORDER BY name";
            return query;
        }

        public static string GetDebtorById => "SELECT * FROM search_debtor_v2 WHERE id = @id";

        public static string GetPartner => "SELECT id, ecode, name FROM partners WHERE ecode = @ecode";

        public static string GetPartnerByScala => "SELECT id, ecode, name FROM partners WHERE old_ecode_scala = @ecode";

        public static string GetPartnerById => "SELECT * FROM partners WHERE id = @id";

        public static string GetPartnerTypesByModule => "SELECT * FROM search_type WHERE modul = @module";

        public static string InsertPartner => "INSERT INTO partners VALUES (@id, @name, @type, @ecode, @tcode, @address, @postindex, @email, @agent, @phone," +
            " @fax, @credtypeid, @debtypeid, @balance, @credit, 0, @descrip, @old_ecode, @old_ecode_scala, @city)";

        public static string UpdatePartner => "UPDATE partners SET name = @name, type = @type, ecode = @ecode, tcode = @tcode, address = @address," +
            " postindex = @postindex, email = @email, agent = @agent, phone = @phone, fax = @fax," +
            " credtypeid = @credtypeid, debtypeid = @debtypeid, balance = @balance, credit = @credit, descrip = @descrip, city = @city WHERE id = @id";

        public static string SetPartnerAgreement => "SELECT set_partner_agreement(@partnerId, @signature);";

        public static string HasPartnerAgreement => "SELECT COUNT(partner_id) > 0 FROM partner_agreement WHERE partner_id = @partnerId";
    }
}
