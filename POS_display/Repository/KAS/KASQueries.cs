using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS_display.Repository.KAS
{
    public static class KASQueries
    {
        public static string GetBENUMTransaction => "SELECT * FROM cheque_presents WHERE hid= @posh_id and buyer= @buyer and chnumber like 'BENUM%'";

        public static string GetSFHeader => @"SELECT check_id, buyer_id, check_no, documentdate, sask_nr, comment FROM sfh 
                                                 WHERE check_id = @check_id AND type = @type AND deleted = 0";
        public static string CheckSFH => @"SELECT sask_nr FROM sfh 
                                                WHERE sask_nr = @sask_nr AND deleted = 0 
                                                UNION SELECT documentno FROM saleh WHERE documentno = @sask_nr AND confirmed = 1";

        public static string FindSFNumber => @"SELECT get_sf_number(@startDate)";
    }
}
