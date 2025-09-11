using Npgsql;
using System;
using System.Threading.Tasks;

namespace POS_display
{
    public class DB_Pay : DB_Base
    {
        public async Task<bool> update_posh(string LAST_CHECK_NO, decimal HIDASCH)
        {
            NpgsqlCommand cmd = new NpgsqlCommand();
            cmd.CommandText = "update posh set deviceno=@DEVICE_NO, checkno=@LAST_CHECK_NO, status=@STATUS,documentdate=CURRENT_TIMESTAMP, userid=@db_userid where id=@HIDASCH";
            cmd.Parameters.AddWithValue("@DEVICE_NO", Session.Devices.deviceno);
            cmd.Parameters.AddWithValue("@LAST_CHECK_NO", LAST_CHECK_NO);
            if (Session.FifoMode == (decimal)Enumerator.FifoMode.NotReserveQty)
                cmd.Parameters.AddWithValue("@STATUS", "A");
            else
                cmd.Parameters.AddWithValue("@STATUS", "R");
            cmd.Parameters.AddWithValue("@db_userid", Session.User.id);
            cmd.Parameters.AddWithValue("@HIDASCH", HIDASCH);

            return await DoSelectVoid(cmd);
        }

        public async Task<bool> delete_posd(decimal HIDASCH)
        {
            NpgsqlCommand cmd = new NpgsqlCommand();
            cmd.CommandText = "delete from posd where barcodeid=0 and qty=0 and sum=0 and hid=@HIDASCH";
            cmd.Parameters.AddWithValue("@HIDASCH", HIDASCH);

            return await DoSelectVoid(cmd);
        }

        public async Task<bool> update_recipe_checkno(string LAST_CHECK_NO, decimal HIDASCH)
        {
            NpgsqlCommand cmd = new NpgsqlCommand();
            cmd.CommandText = "update recipe set checkno=@LAST_CHECK_NO where id in (select recipeid from posd where hid=@HIDASCH)";
            cmd.Parameters.AddWithValue("@LAST_CHECK_NO", LAST_CHECK_NO);
            cmd.Parameters.AddWithValue("@HIDASCH", HIDASCH);

            return await DoSelectVoid(cmd);
        }

        public async Task<bool> update_recipe_date(string LAST_CHECK_NO, decimal HIDASCH)
        {
            NpgsqlCommand cmd = new NpgsqlCommand();
            cmd.CommandText = "update recipe set salesdate=CURRENT_TIMESTAMP, checkdate=CURRENT_TIMESTAMP where id in (select recipeid from posd where hid=@HIDASCH)";
            cmd.Parameters.AddWithValue("@LAST_CHECK_NO", LAST_CHECK_NO);
            cmd.Parameters.AddWithValue("@HIDASCH", HIDASCH);

            return await DoSelectVoid(cmd);
        }

        public async Task<bool> insert_cheque_presents(decimal HIDASCH, string CHBUYER, decimal CHFULLAMNT, decimal CHAMOUNT, string CHCARD)
        {
            NpgsqlCommand cmd = new NpgsqlCommand();
            cmd.CommandText = "insert into cheque_presents (hid, buyer, totalvalue, usedvalue, chnumber) values (@HIDASCH, @CHBUYER, @CHFULLAMNT, @CHAMOUNT, @CHCARD)";
            cmd.Parameters.AddWithValue("@HIDASCH", HIDASCH);
            cmd.Parameters.AddWithValue("@CHBUYER", CHBUYER);
            cmd.Parameters.AddWithValue("@CHFULLAMNT", CHFULLAMNT);
            cmd.Parameters.AddWithValue("@CHAMOUNT", CHAMOUNT);
            cmd.Parameters.AddWithValue("@CHCARD", CHCARD);

            return await DoSelectVoid(cmd);
        }

        [ObsoleteAttribute("Use same method from 'Payment' repository", false)]
        internal async Task<bool> UpdatePoshAdvancepayment(decimal id, string type, decimal totalsum)
        {
            var cmd = new NpgsqlCommand();
            cmd.CommandText = @"UPDATE posh 
                                    SET
                                        type = @type,
                                        status = 'T',
                                        totalsum = @totalsum,
                                        deviceno='',
                                        checkno=''
                                    WHERE id = @id;";
            cmd.Parameters.AddWithValue("@id", id);
            cmd.Parameters.AddWithValue("@type", type);
            cmd.Parameters.AddWithValue("@totalsum", totalsum);

            return await DoSelectVoid(cmd);
        }

        [ObsoleteAttribute("Use same method from 'Payment' repository", false)]

        internal async Task<bool> UpdateAdvancepayment(decimal advancePaymentId, decimal paymentId, decimal paidSum)
        {
            var cmd = new NpgsqlCommand();
            cmd.CommandText = @"SELECT update_advancepayment(@advancepaymentid, @paymentId, @paidSum);";
            cmd.Parameters.AddWithValue("@advancepaymentid", advancePaymentId);
            cmd.Parameters.AddWithValue("@paymentId", paymentId);
            cmd.Parameters.AddWithValue("@paidSum", paidSum);

            return await DoSelectVoid(cmd);
        }
    }
}