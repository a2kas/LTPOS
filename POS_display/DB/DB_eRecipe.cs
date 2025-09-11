using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace POS_display
{
    public class DB_eRecipe : DB_Base
    {
        public async Task<List<Items.posd>> GetPosdDataAsync(decimal posd_Id)
        {
            NpgsqlCommand cmd = new NpgsqlCommand();
            cmd.CommandText = @"SELECT pd.hid, pd.id, pd.productid, pd.qty, pd.barcodeid, pd.barcode, pd.barcodename, pd.recipeid, pd.sum, pd.price, pd.pricediscounted, s.retailpr as retail_price, qty*get_barcode_ratio(pd.barcodeid) as gqty, 
                                (SELECT b.tlkid 
				                FROM tlk_kainos_bind b
                                INNER JOIN tlk_kainos_d d ON b.tlkid=d.tlkid 
				                WHERE b.productid = pd.productid
                                AND now() BETWEEN r_price_start and r_price_end AND d.percent=100 LIMIT 1) AS npakid7, 
                                (SELECT code FROM atc WHERE id=s.atcid) AS atc, 
                                (SELECT ratio from unit WHERE id=s.unitid) AS prodratio, 
                                (SELECT ratio from unit WHERE id=(SELECT unitid FROM barcode WHERE productid=pd.productid LIMIT 1)) AS barratio 
                                FROM search_posd pd 
                                LEFT JOIN stock s ON s.id=pd.productid 
                                WHERE pd.id=@posd_id";
            cmd.Parameters.AddWithValue("@posd_ID", posd_Id);

            return await DoSelectList<Items.posd>(cmd);
        }

        public async Task<decimal> CreateErecipeAsync(decimal posh_id, decimal posd_id, decimal productId, decimal userId, decimal recipe_no, decimal encounterId, decimal recipe_id, DateTime recipedate, DateTime salesdate, DateTime till_date)
        {
            NpgsqlCommand cmd = new NpgsqlCommand();
            cmd.CommandText = @"SELECT create_erecipe(
                                    @posh_id, 
                                    @posd_id, 
                                    @productId, 
                                    @userId, 
                                    @recipe_no,
                                    @encounterId,
                                    @recipe_id,
                                    @recipedate,
                                    @salesdate,
                                    @till_date)";
            cmd.Parameters.AddWithValue("@posh_id", posh_id);
            cmd.Parameters.AddWithValue("@posd_id", posd_id);
            cmd.Parameters.AddWithValue("@productId", productId);
            cmd.Parameters.AddWithValue("@userId", userId);
            cmd.Parameters.AddWithValue("@recipe_no", recipe_no);
            cmd.Parameters.AddWithValue("@encounterId", encounterId);
            cmd.Parameters.AddWithValue("@recipe_id", recipe_id);
            cmd.Parameters.AddWithValue("@recipedate", recipedate);
            cmd.Parameters.AddWithValue("@salesdate", salesdate);
            cmd.Parameters.AddWithValue("@till_date", till_date);

            return await DoSelectValue<decimal>(cmd);
        }

        public async Task<bool> UpdateErecipeAsync(decimal id, decimal composition_id, string composition_ref, string compositionStatus, decimal medication_dispense_id, string medicationDispenseStatus, int active, int confirmed, string documentstatus, string info)
        {
            NpgsqlCommand cmd = new NpgsqlCommand
            {
                CommandText = @"SELECT update_erecipe(
                                    @id, 
                                    @composition_id, 
                                    @composition_ref, 
                                    @compositionstatus, 
                                    @medication_dispense_id, 
                                    @medicationdispensestatus,
                                    @active, 
                                    @confirmed,
                                    @documentstatus,
                                    @info
                                    );"
            };
            cmd.Parameters.AddWithValue("@id", id);
            cmd.Parameters.AddWithValue("@composition_id", composition_id);
            cmd.Parameters.AddWithValue("@composition_ref", composition_ref);
            cmd.Parameters.AddWithValue("@compositionstatus", compositionStatus);
            cmd.Parameters.AddWithValue("@medication_dispense_id", medication_dispense_id);
            cmd.Parameters.AddWithValue("@medicationdispensestatus", medicationDispenseStatus);
            cmd.Parameters.AddWithValue("@active", active);
            cmd.Parameters.AddWithValue("@confirmed", confirmed);
            cmd.Parameters.AddWithValue("@documentstatus", documentstatus);
            cmd.Parameters.AddWithValue("@info", info);

            return await DoSelectVoid(cmd);
        }

        public async Task<decimal> UpdateErecipeDocumentStatusAsync(decimal composition_id, string documentstatus)
        {
            NpgsqlCommand cmd = new NpgsqlCommand();
            cmd.CommandText = @"UPDATE erecipe 
                                SET documentstatus=@documentstatus
                                WHERE composition_id=@composition_id;";
            cmd.Parameters.AddWithValue("@documentstatus", documentstatus);
            cmd.Parameters.AddWithValue("@composition_id", composition_id);

            return await DoSelectValue<decimal>(cmd);
        }

        public async Task<decimal> UpdateErecipeConfirmedAsync(decimal composition_id, int confirmed)
        {
            NpgsqlCommand cmd = new NpgsqlCommand();
            cmd.CommandText = @"UPDATE erecipe 
                                SET confirmed=@confirmed
                                WHERE composition_id=@composition_id;";
            cmd.Parameters.AddWithValue("@confirmed", confirmed);
            cmd.Parameters.AddWithValue("@composition_id", composition_id);

            return await DoSelectValue<decimal>(cmd);
        }

        public async Task<decimal> GetErecipeAsync(decimal posd_id)
        {
            NpgsqlCommand cmd = new NpgsqlCommand();
            cmd.CommandText = "SELECT composition_id FROM erecipe WHERE posd_id=@posd_id and active=1";
            cmd.Parameters.AddWithValue("@posd_id", posd_id);

            return await DoSelectValue<decimal>(cmd);
        }

        public async Task<DataTable> GetErecipeByCompositionIdAsync(decimal composition_id)
        {
            NpgsqlCommand cmd = new NpgsqlCommand();
            cmd.CommandText = "SELECT posd_id, recipe_no, recipe_id FROM erecipe WHERE composition_id=@composition_id";
            cmd.Parameters.AddWithValue("@composition_id", composition_id);

            return await DoSelectDataTable(cmd);
        }

        public async Task<bool> MarkErecipeAsync(decimal posd_id, decimal recipe_no, decimal recipe_id, string info)
        {
            NpgsqlCommand cmd = new NpgsqlCommand();
            cmd.CommandText = @"UPDATE erecipe 
                                SET active=0, 
                                info=@info 
                                WHERE posd_id=@posd_id
                                AND recipe_no=@recipe_no
                                AND recipe_id=@recipe_id";
            cmd.Parameters.AddWithValue("@posd_id", posd_id);//pakaktu tik sitos salygos, taciau kai MP - posd_id=0 tada salyga recipe_id, bet kai nekompensuojamas recipe_id=0 todel belieka recipe_no + active=0
            cmd.Parameters.AddWithValue("@recipe_no", recipe_no);
            cmd.Parameters.AddWithValue("@recipe_id", recipe_id);
            cmd.Parameters.AddWithValue("@info", helpers.GetLatinString(info));

            return await DoSelectVoid(cmd);
        }

        public async Task<DataTable> GetDispenseListAsync(string status, string confirmed, string docStatus, decimal userId, DateTime dateFrom, DateTime dateTo, string note2)
        {
            NpgsqlCommand cmd = new NpgsqlCommand();
            cmd.CommandText = @"SELECT e.id,
                                    e.posh_id, 
                                    e.posd_id,
                                    e.productid,
                                    e.userid,
                                    e.recipe_no,
                                    e.status,
                                    e.composition_id,
                                    e.composition_ref,
                                    e.medication_dispense_id,
                                    e.encounter_id,
                                    e.active,
                                    e.info,
                                    e.recipe_id,
                                    e.recipedate,
                                    e.salesdate,
                                    e.till_date,
                                    e.confirmed,
                                    e.medicationDispenseStatus
                                    FROM erecipe e
                                    LEFT JOIN stock s on e.productid=s.id
                                WHERE 1=1
                                    AND salesdate BETWEEN @dateFrom AND @dateTo";
            if (!string.IsNullOrWhiteSpace(status))
            {
                var statusInt = status == "completed" ? 1 : 0;
                cmd.CommandText += " AND e.active = @active";
                cmd.Parameters.AddWithValue("@active", statusInt);
            }
            if (!string.IsNullOrWhiteSpace(confirmed))
            {
                var confirmedInt = confirmed == "true" ? 1 : 0;
                cmd.CommandText += " AND e.confirmed = @confirmed";
                cmd.Parameters.AddWithValue("@confirmed", confirmedInt);
            }
            if (!string.IsNullOrWhiteSpace(docStatus))
            {
                cmd.CommandText += " AND e.documentstatus = @docStatus";
                cmd.Parameters.AddWithValue("@docStatus", docStatus);
            }
            if (userId > 0)
            {
                cmd.CommandText += " AND e.userid = @userId";
                cmd.Parameters.AddWithValue("@userId", userId);
            }
            if (!string.IsNullOrWhiteSpace(note2))
            {
                cmd.CommandText += " AND s.note2 = @note2";
                cmd.Parameters.AddWithValue("@note2", note2);
            }
            cmd.CommandText += $" ORDER BY COALESCE(salesdate, '1990-01-01') DESC";
            cmd.Parameters.AddWithValue("@dateFrom", dateFrom.Date);
            cmd.Parameters.AddWithValue("@dateTo", dateTo.Date.AddDays(1).AddSeconds(-1));

            return await DoSelectDataTable(cmd);
        }
    }
}
