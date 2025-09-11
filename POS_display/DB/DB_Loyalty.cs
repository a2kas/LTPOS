using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS_display
{
    public class DB_Loyalty : DB_Base
    {
        public async Task<bool> createLoyaltyh(decimal posh_id, string type, string card_no, decimal active, decimal status, decimal accrue_points)
        {
            NpgsqlCommand cmd = new NpgsqlCommand();
            cmd.CommandText = "SELECT create_loyaltyh(@posh_id, @type, @card_no, @active, @status, @accrue_points)";
            cmd.Parameters.AddWithValue("@posh_id", posh_id);
            cmd.Parameters.AddWithValue("@type", type);
            cmd.Parameters.AddWithValue("@card_no", card_no);
            cmd.Parameters.AddWithValue("@active", active);
            cmd.Parameters.AddWithValue("@status", status);
            cmd.Parameters.AddWithValue("@accrue_points", accrue_points);

            return await DoSelectVoid(cmd);
        }

        public async Task<List<T>> GetLoyaltyh<T>(decimal posh_id)
        {
            NpgsqlCommand cmd = new NpgsqlCommand();
            cmd.CommandText = @"SELECT * FROM loyaltyh WHERE posh_id=@posh_id";
            cmd.Parameters.AddWithValue("@posh_id", posh_id);

            return await DoSelectList<T>(cmd);
        }

        public async Task<bool> setManualVouchers(decimal posh_id, string vouchers)
        {
            NpgsqlCommand cmd = new NpgsqlCommand();
            cmd.CommandText = "UPDATE loyaltyh SET manual_vouchers=@vouchers WHERE posh_id=@posh_id";
            cmd.Parameters.AddWithValue("@posh_id", posh_id);
            cmd.Parameters.AddWithValue("@vouchers", vouchers);

            return await DoSelectVoid(cmd);
        }

        public async Task<bool> setInstanceDiscount(decimal posh_id, decimal accrue_points)
        {
            NpgsqlCommand cmd = new NpgsqlCommand();
            cmd.CommandText = "UPDATE loyaltyh SET accrue_points=@accrue_points WHERE posh_id=@posh_id";
            cmd.Parameters.AddWithValue("@posh_id", posh_id);
            cmd.Parameters.AddWithValue("@accrue_points", accrue_points);

            return await DoSelectVoid(cmd);
        }

        public async Task<int> getCounter(decimal posh_id)
        {
            NpgsqlCommand cmd = new NpgsqlCommand();
            cmd.CommandText = "SELECT counter FROM loyaltyh WHERE posh_id=@posh_id";
            cmd.Parameters.AddWithValue("@posh_id", posh_id);

            return await DoSelectValue<int>(cmd);
        }

        public async Task<bool> changeLoyaltyhStatus(decimal posh_id, decimal active, decimal status)
        {
            NpgsqlCommand cmd = new NpgsqlCommand();
            cmd.CommandText = "UPDATE loyaltyh SET active=@active, status=@status, counter=counter+@status WHERE posh_id=@posh_id";
            cmd.Parameters.AddWithValue("@posh_id", posh_id);
            cmd.Parameters.AddWithValue("@active", active);
            cmd.Parameters.AddWithValue("@status", status);

            return await DoSelectVoid(cmd);
        }

    }
}
