using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS_display
{
    public class DB_KAS : DB_Base
    {
        public async Task<List<T>> asyncSearchPosh<T>(string filter_key, string filter_value)
        {
            string[] filterKeys = filter_key.Split(':');
            NpgsqlCommand cmd = new NpgsqlCommand();
            cmd.CommandText = @"SELECT p.id,
                                p.type,
                                p.status,
                                p.totalsum,
                                p.vat,
                                p.documentdate, 
                                p.documentdate2, 
                                p.debtorname,
                                p.sumincvat,
                                p.deviceno,
                                p.checkno,
                                case when d.id IS NULL THEN 'N' ELSE 'T' END as sf 
                                FROM search_posh_new p 
                                LEFT JOIN (select * from sfh where deleted =0) d on p.id=d.check_id ";

            for (int i = 0; i < filterKeys.Length; i++)
            {
                if (i == 0)
                    cmd.CommandText += "where ";
                else
                    cmd.CommandText += "or ";
                cmd.CommandText += string.Format("lower({0}) like lower(@FILTER_VALUE) ", filterKeys[i]);
            }

            if (Session.Develop)
                cmd.CommandText += " ORDER BY p.documentdate DESC";
            else
                cmd.CommandText += " AND debtorid=@debtorid ORDER BY p.documentdate DESC";
            cmd.Parameters.AddWithValue("@FILTER_VALUE", filter_value + "%");
            cmd.Parameters.AddWithValue("@debtorid", Session.Devices.debtorid);

            return await DoSelectList<T>(cmd);
        }

        public async Task<List<T>> asyncSearchPosd<T>(decimal HID, int selected)
        {
            NpgsqlCommand cmd = new NpgsqlCommand();
            cmd.CommandText = @"SELECT 
                                case when pd.barcodeid<>0 then @selected else 0 end as selected,
                                pd.id, 
                                pd.hid, 
                                pd.productid,
                                pd.barcodeid,
                                pd.qty,
                                pd.qty as qty_orig,
                                pd.price,
                                pd.priceincvat,
                                pd.discount,
                                pd.discount_sum,
                                pd.pricediscounted,
                                pd.sumincvat - ROUND(pd.sumincvat/(100+pd.vatsize)*100, 2) as vat,
                                pd.sumincvat - ROUND(pd.sumincvat/(100+pd.vatsize)*100, 2) as vat_orig,
                                pd.sum,
                                pd.sumincvat,
                                pd.sumincvat as sumincvat_orig,
                                pd.recipeid,
                                pd.recipe2,
                                pd.barcodename,
                                pd.barcode,
                                pd.vatsize,
                                (select costprice from fifotrans where posdid=pd.id order by id limit 1)*get_barcode_ratio(pd.barcodeid) as f_cost_price, 
                                (select serialid from fifotrans where posdid=pd.id order by id limit 1) as serialid, 
                                ch.card_no, 
                                ch.info, 
                                ch.cheque_from, 
                                ch.cheque_sum,
                                r.compensationsum,
                                r.prepayment_compensation,
                                    COALESCE((
                                    SELECT SUM(qty) 
                                    FROM saled 
                                    WHERE returned_posd_id = pd.id 
                                    AND productid = pd.productid 
                                    AND barcodeid = pd.barcodeid
                                ), 0) AS returnedqty,
                                (SELECT COUNT(1)>0 FROM fmd_trans where poshid=pd.hid AND posdid=pd.id) as fmd_required
                                FROM search_posd_new pd
				                LEFT JOIN cheque_trans ch ON ch.posh_id=pd.hid AND ch.posd_id=pd.id AND ch.cheque_code IS NULL
                                LEFT JOIN recipe r ON r.id = pd.recipeid
                                WHERE pd.hid=@HID";
            cmd.Parameters.AddWithValue("@HID", HID);
            cmd.Parameters.AddWithValue("@selected", selected);

            return await DoSelectList<T>(cmd);
        }

        public DataTable getBENUMtransaction(decimal posh_id, decimal buyer)
        {
            NpgsqlCommand cmd = new NpgsqlCommand();
            cmd.CommandText = "SELECT * FROM cheque_presents WHERE hid=@posh_id and buyer=@buyer and chnumber like 'BENUM%'";
            cmd.Parameters.AddWithValue("@posh_id", posh_id);
            cmd.Parameters.AddWithValue("@buyer", buyer);

            return queryGetDataTable(cmd);
        }

        public DataTable getSFH(decimal check_id, string type)
        {
            NpgsqlCommand cmd = new NpgsqlCommand();
            cmd.CommandText = "select check_id, buyer_id, check_no, documentdate, sask_nr, comment from sfh where check_id=@check_id and type=@type and deleted=0";
            cmd.Parameters.AddWithValue("@check_id", check_id);
            cmd.Parameters.AddWithValue("@type", type);

            return queryGetDataTable(cmd);
        }
    }
}
