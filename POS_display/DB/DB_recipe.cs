using Npgsql;
using System;
using System.Data;
using System.Threading.Tasks;

namespace POS_display
{
    public class DB_recipe : DB_Base
    {
        public async Task<DataTable> asyncSearchRecipe(decimal ID)
        {
            NpgsqlCommand cmd = new NpgsqlCommand();
            cmd.CommandText = @"select sr.*, pd.id as posd_id, pd.hid as posh_id from search_recipe sr 
                                left join posd pd on pd.recipeid = sr.id
                                where sr.id = @ID";
            cmd.Parameters.AddWithValue("@ID", ID);

            return await DoSelectDataTable(cmd);
        }

        public void asyncStore(string filter_key, string filter_value, dbDataTableCallback callback)
        {
            NpgsqlCommand cmd = new NpgsqlCommand();
            cmd.CommandText = "SELECT store.id, store.name, store.address, ledger.code, ledger.id AS ledgerid, ledger.name AS lname, DECODE(store.localstore,1,'Taip',0,'Ne',3,'Centras') AS local "
                            + "FROM store "
                            + "LEFT JOIN ledger ON store.ledgerid=ledger.id ";

            string[] filterKeys = filter_key.Split(':');
            for (int i = 0; i < filterKeys.Length; i++)
            {
                if (i == 0)
                    cmd.CommandText += "WHERE ";
                else
                    cmd.CommandText += "OR ";
                cmd.CommandText += string.Format("nls_lower({0},'NLS_SORT = LITHUANIAN') like nls_lower(@FILTER_VALUE,'NLS_SORT = LITHUANIAN') ", filterKeys[i]);
            }
            cmd.CommandText += " ORDER BY name";
            cmd.Parameters.AddWithValue("@FILTER_VALUE", filter_value + "%");

            dbDataTableDelegate select = DoSelect;
            IAsyncResult result = select.BeginInvoke(cmd, callback, DataTableDelegateEnd_cb, null);
        }

        public void asyncCompensation(string filter_key, string filter_value, dbDataTableCallback callback)
        {
            NpgsqlCommand cmd = new NpgsqlCommand();
            cmd.CommandText = "SELECT * FROM compensation ";

            string[] filterKeys = filter_key.Split(':');
            for (int i = 0; i < filterKeys.Length; i++)
            {
                if (i == 0)
                    cmd.CommandText += "WHERE ";
                else
                    cmd.CommandText += "OR ";
                cmd.CommandText += string.Format("nls_lower({0},'NLS_SORT = LITHUANIAN') like nls_lower(@FILTER_VALUE,'NLS_SORT = LITHUANIAN') ", filterKeys[i]);
            }
            cmd.CommandText += " ORDER BY name";
            cmd.Parameters.AddWithValue("@FILTER_VALUE", filter_value + "%");

            dbDataTableDelegate select = DoSelect;
            IAsyncResult result = select.BeginInvoke(cmd, callback, DataTableDelegateEnd_cb, null);
        }

        public void asyncDoctor(string filter_key, string filter_value, dbDataTableCallback callback)
        {
            NpgsqlCommand cmd = new NpgsqlCommand();
            cmd.CommandText = "SELECT * FROM doctor ";

            string[] filterKeys = filter_key.Split(':');
            for (int i = 0; i < filterKeys.Length; i++)
            {
                if (i == 0)
                    cmd.CommandText += "WHERE ";
                else
                    cmd.CommandText += "OR ";
                cmd.CommandText += string.Format("nls_lower({0},'NLS_SORT = LITHUANIAN') like nls_lower(@FILTER_VALUE,'NLS_SORT = LITHUANIAN') ", filterKeys[i]);
            }
            cmd.CommandText += " ORDER BY name";
            cmd.Parameters.AddWithValue("@FILTER_VALUE", filter_value + "%");

            dbDataTableDelegate select = DoSelect;
            IAsyncResult result = select.BeginInvoke(cmd, callback, DataTableDelegateEnd_cb, null);
        }

        public async Task<DataTable> asyncGetRecipeKVR(decimal recipeid)
        {
            NpgsqlCommand cmd = new NpgsqlCommand();
            if (Session.SystemData.kas_client_id == 1096 || Session.SystemData.kas_client_id == 1097 || Session.SystemData.kas_client_id == 1098 || Session.SystemData.kas_client_id == 1099 || Session.SystemData.kas_client_id == 1101 || Session.SystemData.kas_client_id == 1138 || Session.SystemData.kas_client_id == 1215 || Session.SystemData.kas_client_id == 1216)//MP
            {
                cmd.CommandText = @"select sr.*, round(sr.gqty,0) as qtyrep, round(sr.countday,0) as countdayrep,
					case when s.kas_client_id=0 or s.kas_client_id is null then (select cast(my_server as numeric) from recipe_parm limit 1)
					else s.kas_client_id end as kvr_pharmacist_id
					from search_recipe_erecipe sr
 					left join store s on s.id=sr.store_id
                    where sr.id=@recipeid and sr.tlk_status=2";
                cmd.Parameters.AddWithValue("@recipeid", recipeid);
            }
            else
            {
                cmd.CommandText = @"select sr.*, round(sr.gqty,0) as qtyrep, round(sr.countday,0) as countdayrep,
					case when usr.insidephone='' or usr.insidephone is null then (select my_server from recipe_parm limit 1)
					else usr.insidephone end as kvr_pharmacist_id
					from search_recipe_erecipe sr
					left join users usr on usr.login=sr.userlogin
					where sr.id=@recipeid and sr.tlk_status=2";
                cmd.Parameters.AddWithValue("@recipeid", recipeid);
            }

            return await DoSelectDataTable(cmd);
        }

        public async Task<bool> UpdateTLKStatus(decimal recipeid, int tlk_status)
        {
            NpgsqlCommand cmd = new NpgsqlCommand();
            cmd.CommandText = String.Format("UPDATE recipe SET tlk_status={0} WHERE id=@recipeid", tlk_status);
            cmd.Parameters.AddWithValue("@recipeid", recipeid);

            return await DoSelectVoid(cmd);
        }

        public DataTable getDoctorName(string doctor_code)
        {
            NpgsqlCommand cmd = new NpgsqlCommand();
            cmd.CommandText = "SELECT id, name FROM doctor WHERE code=@code";
            cmd.Parameters.AddWithValue("@code", doctor_code);

            return queryGetDataTable(cmd);
        }

        public void asyncHospital(string filter_key, string filter_value, dbDataTableCallback callback)
        {
            NpgsqlCommand cmd = new NpgsqlCommand();
            cmd.CommandText = "SELECT * FROM hospital ";

            string[] filterKeys = filter_key.Split(':');
            for (int i = 0; i < filterKeys.Length; i++)
            {
                if (i == 0)
                    cmd.CommandText += "WHERE ";
                else
                    cmd.CommandText += "OR ";
                cmd.CommandText += string.Format("nls_lower({0},'NLS_SORT = LITHUANIAN') like nls_lower(@FILTER_VALUE,'NLS_SORT = LITHUANIAN') ", filterKeys[i]);
            }
            cmd.CommandText += " ORDER BY name";
            cmd.Parameters.AddWithValue("@FILTER_VALUE", filter_value + "%");

            dbDataTableDelegate select = DoSelect;
            IAsyncResult result = select.BeginInvoke(cmd, callback, DataTableDelegateEnd_cb, null);
        }

        public DataTable getHospitalName(decimal clinic_code)
        {
            NpgsqlCommand cmd = new NpgsqlCommand();
            cmd.CommandText = "SELECT id, name FROM hospital WHERE code=@code";
            cmd.Parameters.AddWithValue("@code", clinic_code);

            return queryGetDataTable(cmd);
        }

        public async Task<DataTable> asyncNewRecipeData(decimal posd_id, DateTime checkdate, string barcode, string comppercent)
        {
            NpgsqlCommand cmd = new NpgsqlCommand();
            if (posd_id != -1)
            {
                cmd.CommandText = string.Format(@"select 
				                coalesce((select k.kompensation from tlk_kainos_d k where sr.tlkid=k.tlkid and k.percent=50 and 
				                trunc(now()) between k.r_price_start and k.r_price_end order by k.r_price_start desc limit 1), 0) as c50, 
				                coalesce((select k.kompensation from tlk_kainos_d k where sr.tlkid=k.tlkid and k.percent=80 and 
				                trunc(now()) between k.r_price_start and k.r_price_end order by k.r_price_start desc limit 1), 0) as c80, 
				                coalesce((select k.kompensation from tlk_kainos_d k where sr.tlkid=k.tlkid and k.percent=90 and 
				                trunc(now()) between k.r_price_start and k.r_price_end order by k.r_price_start desc limit 1), 0) as c90, 
				                coalesce((select k.kompensation from tlk_kainos_d k where sr.tlkid=k.tlkid and k.percent=100 and 
				                trunc(now()) between k.r_price_start and k.r_price_end order by k.r_price_start desc limit 1), 0) as c100,sr.* 
				                from search_recipeprice_v2 sr left join tlk_kainos_bind b on sr.tlkid=b.tlkid where sr.barcode='{0}'", barcode.Trim());
            }
            if (posd_id == 0)
            {
                cmd.CommandText = string.Format(@"select 
                                (select coalesce(k.kompensation,0) from tlk_kainos_d k where sr.tlkid=k.tlkid and k.percent=50 and 
                                trunc('{0}'::date) between k.r_price_start and k.r_price_end order by k.r_price_start desc limit 1) as c50, 
                                (select coalesce(k.kompensation,0) from tlk_kainos_d k where sr.tlkid=k.tlkid and k.percent=80 and 
                                trunc('{0}'::date) between k.r_price_start and k.r_price_end order by k.r_price_start desc limit 1) as c80, 
                                (select coalesce(k.kompensation,0) from tlk_kainos_d k where sr.tlkid=k.tlkid and k.percent=90 and 
                                trunc('{0}'::date) between k.r_price_start and k.r_price_end order by k.r_price_start desc limit 1) as c90, 
                                (select coalesce(k.kompensation,0) from tlk_kainos_d k where sr.tlkid=k.tlkid and k.percent=100 and 
                                trunc('{0}'::date) between k.r_price_start and k.r_price_end order by k.r_price_start desc limit 1) as c100,sr.* 
                                from ( 
                                select s.id as productid, t.vatsize, b.barcode, b.id as barcodeid, b.name as productname, up.ratio as prodratio, ub.ratio as barratio, 
                                coalesce(tt.basic_price,0) as basicprice, 
                                coalesce(tt.retail_price,0) as salesprice, 
                                coalesce(tt.retail_price,0) as newsalesprice, 
                                coalesce(tlkii.tlkid,'') as tlkid, 
                                coalesce(tlkii.tlkid,'') as code2, 
                                0 as oldsalesprice, 
                                0 as oldbasicprice, 
                                s.retailpr as retailpr, 
                                '{0}'::date as pricedate,
                                COALESCE(tt.padeng_priemoka , 0) AS padeng_priemoka,
                                s.note2 as note2
                                from barcode b 
                                inner join stock s on s.id=b.productid 
                                inner join taxes t on t.id=s.salesvatid 
                                inner join unit up on up.id=s.unitid 
                                inner join unit ub on ub.id=b.unitid 
                                left join (select * from ( 
                                select bd.productid, h.tlkid, d.r_price_start from tlk_kainos_h h inner join tlk_kainos_bind bd on bd.tlkid=h.tlkid and trunc('{0}'::date) < h.date_end 
                                left join tlk_kainos_d d on d.tlkid=h.tlkid and trunc('{0}'::date) between d.r_price_start and d.r_price_end and d.percent={2} 
                                )t where (t.r_price_start is not null or (t.tlkid like 'M%' and length(t.tlkid)=4) or  t.tlkid like 'G%') ) tlkii on tlkii.productid=s.id 
                                left join ( 
                                select bd.productid, d.retail_price, d.basic_price, bd.tlkid, trunc(d.r_price_start) as price_start, trunc(d.r_price_end) as price_end, padeng_priemoka from tlk_kainos_bind bd 
                                inner join tlk_kainos_d d on bd.tlkid=d.tlkid 
                                where d.percent={2} and trunc('{0}'::date) between trunc(d.r_price_start) and trunc(d.r_price_end) 
                                )tt on tt.productid=s.id 
                                ) sr where sr.barcode='{1}'", checkdate, barcode.Trim(), comppercent);
            }

            return await DoSelectDataTable(cmd);
        }

        public async Task<bool> asyncRepairRecipe(decimal recipe_id)
        {
            NpgsqlCommand cmd = new NpgsqlCommand();
            cmd.CommandText = "SELECT repair_recipe(@recipe_id)";
            cmd.Parameters.AddWithValue("@recipe_id", recipe_id);

            return await DoSelectVoid(cmd);
        }

        public async Task<bool> asyncCommitRecipe(decimal id, string tlkid, decimal barcodeid, string recser, decimal recipeno, string perscode, string clinicid, string deseasecode, string doctorid, DateTime recipedate, decimal salesprice, decimal basicprice, decimal compensationid, decimal qty, decimal totalsum, decimal compsum, decimal paysum, DateTime salesdate, decimal gqty, decimal water, decimal taxolaborum, int ext, DateTime checkdate, decimal checkno, decimal qtyday, decimal countday, DateTime tilldate, string kvpdoctorno, string aaga_isas, DateTime valid_from, DateTime valid_till)
        {
            NpgsqlCommand cmd = new NpgsqlCommand();
            cmd.CommandText = "SELECT commit_recipe(@id, @tlkid, @barcodeid, @recser, @recipeno, @perscode, @clinicid, @deseasecode, @doctorid, @recipedate, @salesprice, @basicprice, @compensationid, @qty, @totalsum, @compsum, @paysum, @salesdate, @gqty, @water, @taxolaborum, @ext, @checkdate, @checkno, @qtyday, @countday, @db_code, @tilldate, @kvpdoctorno, @aaga_isas, @valid_from, @valid_till)";
            cmd.Parameters.AddWithValue("@id", id);
            cmd.Parameters.AddWithValue("@tlkid", tlkid);
            cmd.Parameters.AddWithValue("@barcodeid", barcodeid);
            cmd.Parameters.AddWithValue("@recser", recser);
            cmd.Parameters.AddWithValue("@recipeno", recipeno);
            cmd.Parameters.AddWithValue("@perscode", perscode);
            cmd.Parameters.AddWithValue("@clinicid", clinicid);
            cmd.Parameters.AddWithValue("@deseasecode", deseasecode);
            cmd.Parameters.AddWithValue("@doctorid", doctorid);
            cmd.Parameters.AddWithValue("@recipedate", recipedate);
            cmd.Parameters.AddWithValue("@salesprice", salesprice);
            cmd.Parameters.AddWithValue("@basicprice", basicprice);
            cmd.Parameters.AddWithValue("@compensationid", compensationid);
            cmd.Parameters.AddWithValue("@qty", qty);
            cmd.Parameters.AddWithValue("@totalsum", totalsum);
            cmd.Parameters.AddWithValue("@compsum", compsum);
            cmd.Parameters.AddWithValue("@paysum", paysum);
            cmd.Parameters.AddWithValue("@salesdate", salesdate);
            cmd.Parameters.AddWithValue("@gqty", gqty);
            cmd.Parameters.AddWithValue("@water", water);
            cmd.Parameters.AddWithValue("@taxolaborum", taxolaborum);
            cmd.Parameters.AddWithValue("@ext", ext);
            cmd.Parameters.AddWithValue("@checkdate", checkdate);
            cmd.Parameters.AddWithValue("@checkno", checkno);
            cmd.Parameters.AddWithValue("@qtyday", qtyday);
            cmd.Parameters.AddWithValue("@countday", countday);
            cmd.Parameters.AddWithValue("@db_code", Session.SystemData.code);
            cmd.Parameters.AddWithValue("@tilldate", tilldate);
            cmd.Parameters.AddWithValue("@kvpdoctorno", kvpdoctorno);
            cmd.Parameters.AddWithValue("@aaga_isas", aaga_isas);
            cmd.Parameters.AddWithValue("@valid_from", valid_from);
            cmd.Parameters.AddWithValue("@valid_till", valid_till.Date);

            return await DoSelectVoid(cmd);
        }

        public async Task<DataTable> getTLKPrice(decimal product_id, string comppercent, DateTime checkdate)
        {
            NpgsqlCommand cmd = new NpgsqlCommand();
            cmd.CommandText = "SELECT coalesce(get_tlk_mkaina('-1', @product_id, @comppercent, @checkdate),0) as mmkaina, coalesce(get_tlk_bkaina('-1', @product_id, @comppercent, @checkdate),0) as mbkaina, 1";
            cmd.Parameters.AddWithValue("@product_id", product_id);
            cmd.Parameters.AddWithValue("@comppercent", comppercent);
            cmd.Parameters.AddWithValue("@checkdate", checkdate);

            return await DoSelectDataTable(cmd);
        }

        public async Task<string> getATCcode(decimal product_id)
        {
            NpgsqlCommand cmd = new NpgsqlCommand();
            cmd.CommandText = "SELECT a.code FROM atc a LEFT JOIN stock s ON s.atcid=a.id WHERE s.id=@product_id";
            cmd.Parameters.AddWithValue("@product_id", product_id);

            return await DoSelectValue<string>(cmd);
        }

        public async Task<DataTable> getCompPercent(decimal compCode)
        {
            NpgsqlCommand cmd = new NpgsqlCommand();
            cmd.CommandText = "SELECT id, comppercent FROM compensation WHERE compcode=@compcode";
            cmd.Parameters.AddWithValue("@compcode", compCode);

            return await DoSelectDataTable(cmd);
        }

        public async Task<DataTable> getCompCode(decimal compPercent)
        {
            NpgsqlCommand cmd = new NpgsqlCommand();
            cmd.CommandText = "SELECT id, compcode FROM compensation WHERE comppercent=@comppercent";
            cmd.Parameters.AddWithValue("@comppercent", compPercent);

            return await DoSelectDataTable(cmd);
        }

        public async Task<decimal> getCentrDebtorid()
        {
            NpgsqlCommand cmd = new NpgsqlCommand();
            cmd.CommandText = "SELECT COALESCE((SELECT p.id FROM systemdata s INNER JOIN partners p ON p.id=s.debtorid2 LIMIT 1), 0) AS centrdebtorid";

            return await DoSelectValue<decimal>(cmd);
        }

        public async Task<decimal> getDeseaseId(string desease_code, DateTime sales_date)
        {
            NpgsqlCommand cmd = new NpgsqlCommand();
            cmd.CommandText = "SELECT id FROM recipe_diseases WHERE code=@desease_code AND @sales_date BETWEEN startdate AND enddate";
            cmd.Parameters.AddWithValue("@desease_code", desease_code);
            cmd.Parameters.AddWithValue("@sales_date", sales_date);

            return await DoSelectValue<decimal>(cmd);
        }

        public void asyncLoadRecipeData(decimal recipe_id, dbDataTableCallback callback)
        {
            NpgsqlCommand cmd = new NpgsqlCommand
            {
                CommandText = @"SELECT CASE WHEN sr.ext=1 THEN '1' ELSE '' END AS ext_test, 
                                TRUNC(r.valid_till) AS valid_till, sr.*, r.store_id, store.name AS storename 
                                FROM search_recipe_v3 sr 
                                INNER JOIN recipe r ON r.id=sr.id 
                                LEFT JOIN store ON store.id=r.store_id 
                                WHERE sr.id=@recipe_id"
            };
            cmd.Parameters.AddWithValue("@recipe_id", recipe_id);

            dbDataTableDelegate select = DoSelect;
            IAsyncResult result = select.BeginInvoke(cmd, callback, DataTableDelegateEnd_cb, null);
        }

        public async Task<bool> asyncInsertKVAPrecipe(decimal rec_id, string rec_nr, string gtpl, string item_id, decimal comp_sum, decimal prep, decimal pay_sum, string rec_messages, string rec_status, decimal payed_sum)
        {
            NpgsqlCommand cmd = new NpgsqlCommand();
            cmd.CommandText = "INSERT INTO kvap_recipereport (datestamp, rec_id, rec_nr, gtpl, item_id, comp_sum, prep, pay_sum, rec_messages, rec_status, payed_sum) values(now(), @rec_id, @rec_nr, @gtpl, @item_id, @comp_sum, @prep, @pay_sum, @rec_messages, @rec_status, @payed_sum)";
            cmd.Parameters.AddWithValue("@rec_id", rec_id);
            cmd.Parameters.AddWithValue("@rec_nr", rec_nr);
            cmd.Parameters.AddWithValue("@gtpl", gtpl);
            cmd.Parameters.AddWithValue("@item_id", item_id);
            cmd.Parameters.AddWithValue("@comp_sum", comp_sum);
            cmd.Parameters.AddWithValue("@prep", prep);
            cmd.Parameters.AddWithValue("@pay_sum", pay_sum);
            cmd.Parameters.AddWithValue("@rec_messages", rec_messages);
            cmd.Parameters.AddWithValue("@rec_status", rec_status);
            cmd.Parameters.AddWithValue("@payed_sum", payed_sum);

            return await DoSelectVoid(cmd);
        }

        public async Task<decimal> asyncGetKVAPrecipe(decimal recipe_id)
        {
            NpgsqlCommand cmd = new NpgsqlCommand();
            cmd.CommandText = @"SELECT rec_id FROM kvap_recipereport 
                                WHERE rec_nr=(
                                SELECT CASE WHEN kvpdoctorno='E' 
                                THEN (SELECT composition_id FROM erecipe WHERE recipe_id=id) 
                                ELSE recipeno END
                                FROM recipe 
                                WHERE id=@recipe_id)";
            cmd.Parameters.AddWithValue("@recipe_id", recipe_id);

            return await DoSelectValue<decimal>(cmd);
        }

        public async Task<bool> asyncDeleteKVAPrecipe(decimal rec_id)
        {
            NpgsqlCommand cmd = new NpgsqlCommand();
            cmd.CommandText = "DELETE FROM kvap_recipereport WHERE rec_id=@rec_id";
            cmd.Parameters.AddWithValue("@rec_id", rec_id);

            return await DoSelectVoid(cmd);
        }

        public DataTable getRecipeCompPrice(decimal recipe_id)
        {
            NpgsqlCommand cmd = new NpgsqlCommand();
            cmd.CommandText = string.Format(@"select 
                            (select coalesce(k.kompensation,0) from tlk_kainos_d k where sr.tlkid=k.tlkid and k.percent=50 and 
                            (select trunc(salesdate) from search_recipe where id={0}) between k.r_price_start and k.r_price_end) as c50, 
                            (select coalesce(k.kompensation,0) from tlk_kainos_d k where sr.tlkid=k.tlkid and k.percent=80 and 
                            (select trunc(salesdate) from search_recipe where id={0}) between k.r_price_start and k.r_price_end) as c80, 
                            (select coalesce(k.kompensation,0) from tlk_kainos_d k where sr.tlkid=k.tlkid and k.percent=90 and 
                            (select trunc(salesdate) from search_recipe where id={0}) between k.r_price_start and k.r_price_end) as c90, 
                            (select coalesce(k.kompensation,0) from tlk_kainos_d k where sr.tlkid=k.tlkid and k.percent=100 and 
                            (select trunc(salesdate) from search_recipe where id={0}) between k.r_price_start and k.r_price_end) as c100 
                            from search_recipe sr where sr.id={0}", recipe_id);

            return queryGetDataTable(cmd);
        }

        public async Task<decimal> getProductIdFromBarcode(string barcode)
        {
            NpgsqlCommand cmd = new NpgsqlCommand();
            cmd.CommandText = @"SELECT productid FROM barcode WHERE barcode=@barcode LIMIT 1";
            cmd.Parameters.AddWithValue("@barcode", barcode);

            return await DoSelectValue<decimal>(cmd);
        }

        public DataTable getDataFromBarcode2(string barcode)
        {
            NpgsqlCommand cmd = new NpgsqlCommand();
            cmd.CommandText = @"SELECT b.id, b.productid, b.name, 
                                get_sales_price(b.productid, b.id) as price,
                                (SELECT ratio from unit WHERE id=(SELECT unitid FROM stock WHERE id=b.productid LIMIT 1)) AS prodratio,
                                (SELECT ratio from unit WHERE id=b.unitid) AS barratio,
                                a.code as atc_code,
                                s.note2
                                FROM barcode b
                                LEFT JOIN stock s on s.id=b.productid
                                LEFT JOIN atc a on a.id=s.atcid
                                WHERE b.barcode=@barcode LIMIT 1";
            cmd.Parameters.AddWithValue("@barcode", barcode);

            return queryGetDataTable(cmd);
        }

        public DataTable getInvalidRecipe(string recipe_no, string kvp_doctor_no)
        {
            NpgsqlCommand cmd = new NpgsqlCommand();
            cmd.CommandText = "SELECT * FROM invalid_recipe WHERE recipe_nr=@recipe_no OR doctor_nr=@kvp_doctor_no";
            cmd.Parameters.AddWithValue("@recipe_no", recipe_no.ToString());
            cmd.Parameters.AddWithValue("@kvp_doctor_no", kvp_doctor_no.ToString());

            return queryGetDataTable(cmd);
        }

        public async Task<decimal> asyncCreateRecipe(string tlkid, decimal barcodeid, string recser, decimal recipeno, string perscode, string clinicid, string deseasecode, string doctorid, DateTime recipedate, decimal salesprice, decimal basicprice, decimal compensationid, decimal qty, decimal totalsum, decimal compsum, decimal paysum, DateTime salesdate, decimal gqty, decimal water, decimal taxolaborum, int ext, DateTime checkdate, decimal checkno, decimal qtyday, decimal countday, DateTime tilldate, string kvpdoctorno, string aaga_isas, DateTime valid_from, DateTime valid_till, decimal store_id)
        {
            NpgsqlCommand cmd = new NpgsqlCommand();
            cmd.CommandText = "SELECT create_recipe(@db_code, @tlkid, @barcodeid, @recser, @recipeno, @perscode, @clinicid, @deseasecode, @doctorid, @recipedate, @salesprice, @basicprice, @compensationid, @qty, @totalsum, @compsum, @paysum, @salesdate, @gqty, @water, @taxolaborum, @ext, @checkdate, @checkno, @qtyday, @countday, @tilldate, @kvpdoctorno, @aaga_isas, @valid_from, @valid_till, @store_id)";
            cmd.Parameters.AddWithValue("@db_code", Session.SystemData.code);
            cmd.Parameters.AddWithValue("@tlkid", tlkid);
            cmd.Parameters.AddWithValue("@barcodeid", barcodeid);
            cmd.Parameters.AddWithValue("@recser", recser);
            cmd.Parameters.AddWithValue("@recipeno", recipeno);
            cmd.Parameters.AddWithValue("@perscode", perscode);
            cmd.Parameters.AddWithValue("@clinicid", clinicid);
            cmd.Parameters.AddWithValue("@deseasecode", deseasecode);
            cmd.Parameters.AddWithValue("@doctorid", doctorid);
            cmd.Parameters.AddWithValue("@recipedate", recipedate);
            cmd.Parameters.AddWithValue("@salesprice", salesprice);
            cmd.Parameters.AddWithValue("@basicprice", basicprice);
            cmd.Parameters.AddWithValue("@compensationid", compensationid);
            cmd.Parameters.AddWithValue("@qty", qty);
            cmd.Parameters.AddWithValue("@totalsum", totalsum);
            cmd.Parameters.AddWithValue("@compsum", compsum);
            cmd.Parameters.AddWithValue("@paysum", paysum);
            cmd.Parameters.AddWithValue("@salesdate", salesdate);
            cmd.Parameters.AddWithValue("@gqty", gqty);
            cmd.Parameters.AddWithValue("@water", water);
            cmd.Parameters.AddWithValue("@taxolaborum", taxolaborum);
            cmd.Parameters.AddWithValue("@ext", ext);
            cmd.Parameters.AddWithValue("@checkdate", checkdate);
            cmd.Parameters.AddWithValue("@checkno", checkno);
            cmd.Parameters.AddWithValue("@qtyday", qtyday);
            cmd.Parameters.AddWithValue("@countday", countday);
            cmd.Parameters.AddWithValue("@tilldate", tilldate);
            cmd.Parameters.AddWithValue("@kvpdoctorno", kvpdoctorno);
            cmd.Parameters.AddWithValue("@aaga_isas", aaga_isas);
            cmd.Parameters.AddWithValue("@valid_from", valid_from);
            cmd.Parameters.AddWithValue("@valid_till", valid_till.Date);
            cmd.Parameters.AddWithValue("@store_id", store_id);

            return await DoSelectValue<decimal>(cmd);
        }

        public async Task<bool> asyncUpdateRecipe(decimal id, string tlkid, decimal barcodeid, string recser, decimal recipeno, string perscode, string clinicid, string deseasecode, string doctorid, DateTime recipedate, decimal salesprice, decimal basicprice, decimal compensationid, decimal qty, decimal totalsum, decimal compsum, decimal paysum, DateTime salesdate, decimal gqty, decimal water, decimal taxolaborum, int ext, DateTime checkdate, decimal checkno, decimal qtyday, decimal countday, DateTime tilldate, string kvpdoctorno, string aaga_isas, DateTime valid_from, DateTime valid_till, decimal store_id)
        {
            NpgsqlCommand cmd = new NpgsqlCommand();
            cmd.CommandText = "SELECT update_recipe(@id, @tlkid, @barcodeid, @recser, @recipeno, @perscode, @clinicid, @deseasecode, @doctorid, @recipedate, @salesprice, @basicprice, @compensationid, @qty, @totalsum, @compsum, @paysum, @salesdate, @gqty, @water, @taxolaborum, @ext, @checkdate, @checkno, @qtyday, @countday, @tilldate, @kvpdoctorno, @aaga_isas, @valid_from, @valid_till, @store_id)";
            cmd.Parameters.AddWithValue("@id", id);
            cmd.Parameters.AddWithValue("@tlkid", tlkid);
            cmd.Parameters.AddWithValue("@barcodeid", barcodeid);
            cmd.Parameters.AddWithValue("@recser", recser);
            cmd.Parameters.AddWithValue("@recipeno", recipeno);
            cmd.Parameters.AddWithValue("@perscode", perscode);
            cmd.Parameters.AddWithValue("@clinicid", clinicid);
            cmd.Parameters.AddWithValue("@deseasecode", deseasecode);
            cmd.Parameters.AddWithValue("@doctorid", doctorid);
            cmd.Parameters.AddWithValue("@recipedate", recipedate);
            cmd.Parameters.AddWithValue("@salesprice", salesprice);
            cmd.Parameters.AddWithValue("@basicprice", basicprice);
            cmd.Parameters.AddWithValue("@compensationid", compensationid);
            cmd.Parameters.AddWithValue("@qty", qty);
            cmd.Parameters.AddWithValue("@totalsum", totalsum);
            cmd.Parameters.AddWithValue("@compsum", compsum);
            cmd.Parameters.AddWithValue("@paysum", paysum);
            cmd.Parameters.AddWithValue("@salesdate", salesdate);
            cmd.Parameters.AddWithValue("@gqty", gqty);
            cmd.Parameters.AddWithValue("@water", water);
            cmd.Parameters.AddWithValue("@taxolaborum", taxolaborum);
            cmd.Parameters.AddWithValue("@ext", ext);
            cmd.Parameters.AddWithValue("@checkdate", checkdate);
            cmd.Parameters.AddWithValue("@checkno", checkno);
            cmd.Parameters.AddWithValue("@qtyday", qtyday);
            cmd.Parameters.AddWithValue("@countday", countday);
            cmd.Parameters.AddWithValue("@tilldate", tilldate);
            cmd.Parameters.AddWithValue("@kvpdoctorno", kvpdoctorno);
            cmd.Parameters.AddWithValue("@aaga_isas", aaga_isas);
            cmd.Parameters.AddWithValue("@valid_from", valid_from);
            cmd.Parameters.AddWithValue("@valid_till", valid_till.Date);
            cmd.Parameters.AddWithValue("@store_id", store_id);

            return await DoSelectVoid(cmd);
        }

        public async Task<bool> asyncUpdatePosdRecipe(decimal posd_id, decimal barcodeid, decimal qty, decimal newpaysum, decimal newhid, decimal paysum)
        {
            NpgsqlCommand cmd = new NpgsqlCommand();
            cmd.CommandText = "SELECT update_posd_recipe(@posd_id, @barcodeid, @qty, @newpaysum, @newhid, @paysum)";
            cmd.Parameters.AddWithValue("@posd_id", posd_id);
            cmd.Parameters.AddWithValue("@barcodeid", barcodeid);
            cmd.Parameters.AddWithValue("@qty", qty);
            cmd.Parameters.AddWithValue("@newpaysum", newpaysum);
            cmd.Parameters.AddWithValue("@newhid", newhid);
            cmd.Parameters.AddWithValue("@paysum", paysum);

            return await DoSelectVoid(cmd);
        }
        [ObsoleteAttribute("Use same method from 'Recipe' repository", false)]
        public async Task<string> getNpakId7(decimal productid)
        {
            NpgsqlCommand cmd = new NpgsqlCommand();
            cmd.CommandText = @"SELECT b.tlkid 
				                FROM tlk_kainos_bind b
				                INNER JOIN tlk_kainos_d d on b.tlkid=d.tlkid
				                WHERE b.productid = @productid
				                AND now() BETWEEN r_price_start AND r_price_end
				                AND d.percent=100;";
            cmd.Parameters.AddWithValue("@productid", productid);

            return await DoSelectValue<string>(cmd);
        }

        public async Task<DataTable> getRecipeByPosdId(decimal posd_id)
        {
            NpgsqlCommand cmd = new NpgsqlCommand();
            cmd.CommandText = @"SELECT r.id as recipe_id, r.recipeno as recipe_no 
                                FROM posd pd
                                LEFT JOIN recipe r on r.id=pd.recipeid
                                WHERE pd.id=@posd_id;";
            cmd.Parameters.AddWithValue("@posd_id", posd_id);

            return await DoSelectDataTable(cmd);
        }
    }
}

