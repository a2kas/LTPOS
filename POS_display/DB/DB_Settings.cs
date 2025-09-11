using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS_display
{
    public class DB_Settings : DB_Base
    {
        // Recipe Settings //
        public void asyncRecipeParams(dbDataTableCallback callback)
        {
            NpgsqlCommand cmd = new NpgsqlCommand();
            cmd.CommandText = "select *, (select send_to_tlk from recipe_parm) as send_to_tlk from search_recipe_parm";

            dbDataTableDelegate select = DoSelect;
            IAsyncResult result = select.BeginInvoke(cmd, callback, DataTableDelegateEnd_cb, null);
        }

        public void asyncSearchledger(string filter_key, string filter_value, dbDataTableCallback callback)
        {
            string[] filterKeys = filter_key.Split(':');
            NpgsqlCommand cmd = new NpgsqlCommand();
            cmd.CommandText = "select id,code,name,type,decode(blocked,0,'Ne','Taip') as blocked,abs(restamount) as restamount from ledger ";

            for (int i = 0; i < filterKeys.Length; i++)
            {
                if (i == 0)
                    cmd.CommandText += "where ";
                else
                    cmd.CommandText += "or ";
                cmd.CommandText += string.Format("nls_lower({0},'NLS_SORT = LITHUANIAN') like nls_lower(@FILTER_VALUE,'NLS_SORT = LITHUANIAN') ", filterKeys[i]);
            }
            cmd.CommandText += " order by name";
            cmd.Parameters.AddWithValue("@FILTER_VALUE", filter_value + "%");

            dbDataTableDelegate select = DoSelect;
            IAsyncResult result = select.BeginInvoke(cmd, callback, DataTableDelegateEnd_cb, null);
        }

        public async Task<bool> asyncUpdateRecipeParams(decimal account_id, decimal offset_account_id, string txt_my_os, string txt_my_email, string txt_my_server, string txt_my_protocol, string txt_my_login, string txt_my_pasword, string txt_tlk_email, string txt_tlk_id, int cmb_commit_from_pos, int cmb_save_on_exit, int cmb_check, int edt_timeout, int cmb_send)
        {
            NpgsqlCommand cmd = new NpgsqlCommand();
            cmd.CommandText = "select update_recipe_parm(@account_id, @offset_account_id, @txt_my_os, @txt_my_email, @txt_my_server, @txt_my_protocol, @txt_my_login, @txt_my_pasword, @txt_tlk_email, @txt_tlk_id, @cmb_commit_from_pos,@cmb_save_on_exit, @cmb_check, @edt_timeout, @cmb_send)";
            cmd.Parameters.AddWithValue("@account_id", account_id);
            cmd.Parameters.AddWithValue("@offset_account_id", offset_account_id);
            cmd.Parameters.AddWithValue("@txt_my_os", txt_my_os);
            cmd.Parameters.AddWithValue("@txt_my_email", txt_my_email);
            cmd.Parameters.AddWithValue("@txt_my_server", txt_my_server);
            cmd.Parameters.AddWithValue("@txt_my_protocol", txt_my_protocol);
            cmd.Parameters.AddWithValue("@txt_my_login", txt_my_login);
            cmd.Parameters.AddWithValue("@txt_my_pasword", txt_my_pasword);
            cmd.Parameters.AddWithValue("@txt_tlk_email", txt_tlk_email);
            cmd.Parameters.AddWithValue("@txt_tlk_id", txt_tlk_id);
            cmd.Parameters.AddWithValue("@cmb_commit_from_pos", cmb_commit_from_pos);
            cmd.Parameters.AddWithValue("@cmb_save_on_exit", cmb_save_on_exit);
            cmd.Parameters.AddWithValue("@cmb_check", cmb_check);
            cmd.Parameters.AddWithValue("@edt_timeout", edt_timeout);
            cmd.Parameters.AddWithValue("@cmb_send", cmb_send);

            return await DoSelectVoid(cmd);
        }

        public List<T> getParams<T>()
        {
            NpgsqlCommand cmd = new NpgsqlCommand();
            cmd.CommandText = "SELECT UPPER(system) as system, UPPER(par) as par, value FROM params";

            return queryGetList<T>(cmd);
        }

        public async Task<string> updateParams(string system, string par, string value)
        {
            NpgsqlCommand cmd = new NpgsqlCommand();
            cmd.CommandText = "UPDATE params SET value=UPPER(@value) WHERE UPPER(system)=@system AND UPPER(par)=@par";
            cmd.Parameters.AddWithValue("@system", system);
            cmd.Parameters.AddWithValue("@par", par);
            cmd.Parameters.AddWithValue("@value", value);

            return await DoSelectValue<string>(cmd);
        }
    }
}
