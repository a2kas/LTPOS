using Npgsql;
using System;
using System.Data;
using System.Threading.Tasks;

namespace POS_display
{
    public class DB_cheque : DB_Base
    {
        public void asyncCheque(decimal ID, dbDataTableCallback callback)
        {
            NpgsqlCommand cmd = new NpgsqlCommand();
            cmd.CommandText = "select (select round(qty*get_barcode_ratio((select barcodeid from posd where id=@ID)),2) from posd where id=@ID) as qty, "
                            + " *, "
                            + "case when cheque_sum*(select qty from posd where id=@ID)*get_barcode_ratio((select barcodeid from posd where id=@ID)) >= (select sum from posd where id=@ID) then round((select sum from posd where id=@ID),2) "
                            + "else round(get_barcode_ratio((select barcodeid from posd where id=@ID))*cheque_sum*(select qty from posd where id=@ID) ,2) "
                            + "end as cheq "
                            + "from cheque_params where productid=(select productid from posd where id=@ID) and trunc(now()) between valid_from and valid_till";
            cmd.Parameters.AddWithValue("@ID", ID);

            dbDataTableDelegate select = DoSelect;
            IAsyncResult result = select.BeginInvoke(cmd, callback, DataTableDelegateEnd_cb, null);
        }

        public async Task<bool> CreateChequeTrans(decimal ID, decimal amount, string from, string info, string card_no, string cheque_code, int status, string compensation_type)
        {
            NpgsqlCommand cmd = new NpgsqlCommand();
            cmd.CommandText = "SELECT create_cheque_trans(@ID, @AMOUNT, @FROM, @INFO, @CARDNO, @CHEQUE_CODE, @STATUS, @COMPENSATION_TYPE)";
            cmd.Parameters.AddWithValue("@ID", ID);
            cmd.Parameters.AddWithValue("@AMOUNT", amount);
            cmd.Parameters.AddWithValue("@FROM", from);
            cmd.Parameters.AddWithValue("@INFO", info);
            cmd.Parameters.AddWithValue("@CARDNO", card_no);
            cmd.Parameters.AddWithValue("@CHEQUE_CODE", cheque_code);
            cmd.Parameters.AddWithValue("@STATUS", status);
            cmd.Parameters.AddWithValue("@COMPENSATION_TYPE", compensation_type);

            return await DoSelectVoid(cmd);
        }

        public async Task<bool> UpdateChequeTrans(decimal posd_id, decimal amount, string from, string info, string card_no, int status, string compensation_type)
        {
            NpgsqlCommand cmd = new NpgsqlCommand();
            cmd.CommandText = "UPDATE cheque_trans " +
                "set cheque_sum=@AMOUNT, " +
                "info=@INFO, " +
                "card_no=@CARDNO, " +
                "status=@STATUS, " +
                "compensation_type=@COMPENSATION_TYPE, " +
                "price_full= (select d.pricediscounted from posd d where d.id = @posd_id)" +
                "WHERE posd_id=@posd_id AND cheque_from=@FROM";
            cmd.Parameters.AddWithValue("@posd_id", posd_id);
            cmd.Parameters.AddWithValue("@AMOUNT", amount);
            cmd.Parameters.AddWithValue("@FROM", from);
            cmd.Parameters.AddWithValue("@INFO", info);
            cmd.Parameters.AddWithValue("@CARDNO", card_no);
            cmd.Parameters.AddWithValue("@STATUS", status);
            cmd.Parameters.AddWithValue("@COMPENSATION_TYPE", compensation_type);

            return await DoSelectVoid(cmd);
        }

        public async Task<decimal> asyncChequeTrans(decimal ID)
        {
            NpgsqlCommand cmd = new NpgsqlCommand();
            cmd.CommandText = "SELECT card_no FROM cheque_trans where posd_id=@ID AND cheque_code IS NOT NULL";
            cmd.Parameters.AddWithValue("@ID", ID);

            return await DoSelectValue<decimal>(cmd);
        }

        public async Task<bool> ConfirmChequeTrans(decimal HID, string info)
        {
            NpgsqlCommand cmd = new NpgsqlCommand();
            cmd.CommandText = "UPDATE cheque_trans SET info=@INFO, status=10 WHERE posh_id=@HID AND cheque_code IS NULL AND status <> 13";
            cmd.Parameters.AddWithValue("@HID", HID);
            cmd.Parameters.AddWithValue("@INFO", info);

            return await DoSelectVoid(cmd);
        }

        public async Task<bool> CancelChequeTrans(decimal HID)
        {
            NpgsqlCommand cmd = new NpgsqlCommand();
            cmd.CommandText = "DELETE FROM cheque_trans WHERE posh_id=@HID AND cheque_code IS NULL";
            cmd.Parameters.AddWithValue("@HID", HID);

            return await DoSelectVoid(cmd);
        }

        public async Task<bool> VoidChequeTrans(decimal posh_id)
        {
            NpgsqlCommand cmd = new NpgsqlCommand();
            cmd.CommandText = "UPDATE cheque_trans SET status=15 WHERE posh_id=@posh_id AND cheque_code IS NULL";
            cmd.Parameters.AddWithValue("@posh_id", posh_id);

            return await DoSelectVoid(cmd);
        }

        public async Task<DataTable> GetInsuranceData(decimal posh_id)
        {
            NpgsqlCommand cmd = new NpgsqlCommand();
            cmd.CommandText = @"SELECT * FROM cheque_trans WHERE posh_id=@HID AND cheque_code IS NULL and status in (10, 11, 12) 
                                ORDER BY STATUS";
            cmd.Parameters.AddWithValue("@HID", posh_id);

            return await DoSelectDataTable(cmd);
        }

        public async Task<bool> ClearInsuranceData(decimal posh_id)
        {
            NpgsqlCommand cmd = new NpgsqlCommand();
            cmd.CommandText = "DELETE FROM cheque_trans WHERE posh_id=@HID AND cheque_code IS NULL";
            cmd.Parameters.AddWithValue("@HID", posh_id);

            return await DoSelectVoid(cmd);
        }

        public async Task<bool> ChangeInsuranceStatus(decimal posh_id, decimal posd_id, int status)
        {
            NpgsqlCommand cmd = new NpgsqlCommand();
            cmd.CommandText = "UPDATE cheque_trans SET status=@status WHERE posh_id=@posh_id AND posd_id=@posd_id AND cheque_code IS NULL";
            cmd.Parameters.AddWithValue("@posh_id", posh_id);
            cmd.Parameters.AddWithValue("@posd_id", posd_id);
            cmd.Parameters.AddWithValue("@status", status);

            return await DoSelectVoid(cmd);
        }
    }
}
