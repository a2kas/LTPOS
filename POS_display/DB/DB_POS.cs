using System;
using Npgsql;
using System.Data;
using System.Collections.Generic;
using System.Threading.Tasks;
using POS_display.Items;

namespace POS_display
{
    public class DB_POS : DB_Base
    {
        //Async calls
        [ObsoleteAttribute("Use same method from 'POS' repository", false)]
        public void asyncAutocomplete(string table_name, string column_name, dbDataTableCallback callback)
        {
            NpgsqlCommand cmd = new NpgsqlCommand();
            cmd.CommandText = string.Format("SELECT DISTINCT {0} FROM {1}", column_name, table_name);

            dbDataTableDelegate select = DoSelect;
            IAsyncResult result = select.BeginInvoke(cmd, callback, DataTableDelegateEnd_cb, null);
        }

        public async Task<decimal> AsyncDeletePosd(decimal posh_id, decimal posd_id)
        {
            NpgsqlCommand cmd = new NpgsqlCommand();
            cmd.CommandText = "SELECT delete_posd(@db_code,@ID,@HID,@FIFOMODE)";
            cmd.Parameters.AddWithValue("@db_code", Session.SystemData.code);
            cmd.Parameters.AddWithValue("@ID", posd_id);
            cmd.Parameters.AddWithValue("@HID", posh_id);
            cmd.Parameters.AddWithValue("@FIFOMODE", Session.FifoMode);

            return await DoSelectValue<decimal>(cmd);
        }

        public async Task<decimal> AsyncCancelPosh(decimal posh_id)
        {
            NpgsqlCommand cmd = new NpgsqlCommand();
            cmd.CommandText = "SELECT cancel_posh(@db_code,@FIFOMODE,@HID)";
            cmd.Parameters.AddWithValue("@db_code", Session.SystemData.code);
            cmd.Parameters.AddWithValue("@FIFOMODE", Session.FifoMode);
            cmd.Parameters.AddWithValue("@HID", posh_id);

            return await DoSelectValue<decimal>(cmd);
        }

        public T Login<T>(string username, string password)
        {
            NpgsqlCommand cmd = new NpgsqlCommand();
            cmd.CommandText = "SELECT id, name || ' ' || SUBSTRING(COALESCE(surname,''),1,1) || '.' as DisplayName, insidephone as StampId, locked, postname as PostName, personalfile as Stamp, expire_date AS ExpireDate FROM search_users_v2 WHERE login=@un and password=@pass";
            cmd.Parameters.AddWithValue("@un", username);
            cmd.Parameters.AddWithValue("@pass", helpers.GetMD5(password));

            return queryGetClass<T>(cmd);
        }

        public T getRecipeParm<T>()
        {
            NpgsqlCommand cmd = new NpgsqlCommand();
            cmd.CommandText = "SELECT print_on_save, timeout, check_, tlk_id, my_protocol, send_to_tlk FROM recipe_parm LIMIT 1";

            return queryGetClass<T>(cmd);
        }

        public T getSearchSystemdata<T>()
        {
            NpgsqlCommand cmd = new NpgsqlCommand();
            cmd.CommandText = @"SELECT ss.name, ss.ecode, ss.address, ss.code, ss.storeid, ss.storename, ss.kas_client_id, ss.currencyid, ss.currencycode , s.prodcustid, ss.phone, s.internalemail AS InternalEmail, pharmacy_no AS PharmacyNo,
                                s.prodis_custno AS ProdisCustNo
                                FROM search_systemdata ss
                                INNER JOIN systemdata s ON ss.id=s.id";

            return queryGetClass<T>(cmd);
        }

        public T getSearchDevices<T>()
        {
            NpgsqlCommand cmd = new NpgsqlCommand();
            cmd.CommandText = "SELECT debtorid, realtime, fiscal, negativeqty as fifomode, debtorname, deviceno, cash_only, header4, weightmask, pricemask, drvname as postype, programfile, errorfile FROM search_devices WHERE computername=@Host";
            cmd.Parameters.AddWithValue("@Host", Session.LocalIP);

            return queryGetClass<T>(cmd);
        }

        public void asyncSearchStockinfo5(string filter_key, string filter_value, string compensated, dbDataTableCallback callback)
        {
            string[] filterKeys = filter_key.Split(':');
            NpgsqlCommand cmd = new NpgsqlCommand();
            cmd.CommandText = @"select si.*,get_sales_price(si.productid, si.id, - 1, - 1, 1, 0) AS salesprice, product_info.info, vatsize, 
                            get_comp_sales_price(si.productid, si.id, 100)  as pk1, 
                            get_comp_sales_price(si.productid, si.id, 90)  as pk2, 
                            get_comp_sales_price(si.productid, si.id, 80)  as pk3, 
                            get_comp_sales_price(si.productid, si.id, 50)  as pk4,
                            pl.oficina_location as oficina,
                            pl.oficina_2_location as oficina_2,
                            pl.stock_location as stock,
                            0 as tamro_qty
                            from search_stockinfo5  si
                            left join product_location pl on pl.productid = si.productid
                            left join product_info on si.productid=product_info.productid 
                            left join taxes on si.salesvatid=taxes.id ";

            for (int i = 0; i < filterKeys.Length; i++)
            {
                if (i == 0)
                    cmd.CommandText += "where ";
                else
                    cmd.CommandText += "or ";
                cmd.CommandText += string.Format("{0} like @FILTER_VALUE ", filterKeys[i]);
            }
            cmd.CommandText += string.Format("{0} order by rus_prior", compensated);
            cmd.Parameters.AddWithValue("@FILTER_VALUE", filter_value + "%");

            dbDataTableDelegate select = DoSelect;
            IAsyncResult result = select.BeginInvoke(cmd, callback, DataTableDelegateEnd_cb, null);
        }

        public void asyncSearchService(string filter_key, string filter_value, dbDataTableCallback callback)
        {
            string[] filterKeys = filter_key.Split(':');
            NpgsqlCommand cmd = new NpgsqlCommand();
            cmd.CommandText = "select id, name, typename from search_service ";

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

        public async Task<bool> asyncCreatePrepTrans(decimal ID, decimal creditor)
        {
            NpgsqlCommand cmd = new NpgsqlCommand();
            cmd.CommandText = "select create_prep_trans(@ID, @CREDITOR)";
            cmd.Parameters.AddWithValue("@ID", ID);
            cmd.Parameters.AddWithValue("@CREDITOR", creditor);

            return await DoSelectVoid(cmd);
        }

        public async Task<bool> asyncChangePosdPrice(decimal ID, decimal price_new)
        {
            NpgsqlCommand cmd = new NpgsqlCommand();
            cmd.CommandText = "SELECT change_posd_price(@ID, @PRICE_NEW) > 0";
            cmd.Parameters.AddWithValue("@ID", ID);
            cmd.Parameters.AddWithValue("@PRICE_NEW", price_new);

            return await DoSelectValue<bool>(cmd);
        }

        public async Task<decimal> asyncECRReports(decimal cmb_report, decimal edt_change)
        {
            NpgsqlCommand cmd = new NpgsqlCommand();
            cmd.CommandText = "SELECT ecr_reports(@db_code, @cmb_report, @debtorid, @db_userid, @edt_change)";
            cmd.Parameters.AddWithValue("@db_code", Session.SystemData.code);
            cmd.Parameters.AddWithValue("@cmb_report", cmb_report);
            cmd.Parameters.AddWithValue("@debtorid", Session.Devices.debtorid);
            cmd.Parameters.AddWithValue("@db_userid", Session.User.id);
            cmd.Parameters.AddWithValue("@edt_change", edt_change);

            return await DoSelectValue<decimal>(cmd);
        }
        //Sync calls
        public async Task<bool> UpdateSession(string action, decimal f_mode)
        {
            NpgsqlCommand cmd = new NpgsqlCommand();
            cmd.CommandText = "SELECT update_session(@db_code, @db_userid, @Host, @Action, @Fmode)";
            cmd.Parameters.AddWithValue("@db_code", Session.SystemData.code);
            cmd.Parameters.AddWithValue("@db_userid", Session.User.id);
            cmd.Parameters.AddWithValue("@Host", Session.LocalIP);
            cmd.Parameters.AddWithValue("@Action", action);
            cmd.Parameters.AddWithValue("@Fmode", f_mode);

            return await DoSelectVoid(cmd);
        }

        public async Task<bool> SaveStamp(decimal user_id, string stamp)
        {
            NpgsqlCommand cmd = new NpgsqlCommand();
            cmd.CommandText = "UPDATE users SET personalfile=@stamp WHERE id=@user_id";
            cmd.Parameters.AddWithValue("@user_id", user_id);
            cmd.Parameters.AddWithValue("@stamp", stamp);

            return await DoSelectVoid(cmd);
        }

        [ObsoleteAttribute("Use GetPosDetails method from 'POS' repository", false)]
        public async Task<List<T>> GetPosd<T>(decimal posh_id)
        {
            NpgsqlCommand cmd = new NpgsqlCommand();
            cmd.CommandText = @"SELECT * FROM (
            select 
            d.id, 
            CAST('POSD' AS CHARACTER VARYING) as type,
            pi.info, 
            d.productid, 
            d.barcodeid, 
            d.recipeid, 
            d.discount, 
            d.qty, 
            round(d.qty*d.price*d.discount/100,2) as discount_sum, 
            d.hid, 
            b.barcode as barcode, 
            coalesce(b.name, s.name)|| '('||coalesce(prr.name,'')||')' as barcodename, 
            coalesce(b.name, s.name)|| '('||coalesce(prr.name,'')||')' || coalesce('\n     ' || pi.info, '') as barcodename_info, 
            s.name2 as barcodename2, 
            r.row_no, 
            r.gqty, 
            r.paysum, 
            ROUND(COALESCE(r.compensationsum, 0) + COALESCE(r.prepayment_compensation, 0), 2) AS compensationsum, 
            c.compcode, 
            r.salesprice, 
            r.recipeno, 
            r.basicprice, 
            case when er.recipe_no<>0 then er.till_date else r.till_date end as till_date, 
            r.salesdate, 
            r.totalsum, 
            d.vatproc as vatsize, 
            r.qty as rqty,
            s.retailpr as RetailProductRatio,
            c.comppercent, 
            d.price, 
            d.pricediscounted, 
            d.sum,
            v.prescription_composition_id AS VaccinePrescriptionCompositionId,
            v.dispensation_composition_id AS VaccineDispensationCompositionId,
            v.patient_id AS VaccinatedPatientId,
            case when d.recipeid=0 then t.ecrtax else t.ecrtax2 end as ecrtax, 
            --case when d.recipeid=0 then 0 else -1 end as recipe2, 
            case when d.recipeid=0 then ' ' else to_char(d.sum,'99990.99') end as endsum, 
            --round((select abs(ff.costsum/ff.qty)*get_barcode_ratio(d.barcodeid) from fifotrans ff where ff.outmodul='K' and outtransdid=d.id limit 1),2) as cost_price, 
            /*to_char(round((1::numeric - CASE WHEN d.recipeid = 0::numeric THEN round((d.price * d.qty -  d.pricediscounted * d.qty) / 
            CASE WHEN (d.price * d.qty) = 0::numeric THEN 1.00::numeric 
            ELSE d.price * d.qty END, 4) 
            ELSE round((d.price * d.qty + COALESCE(( SELECT  r.compensationsum FROM recipe r WHERE r.id = d.recipeid), 0::numeric) - d.pricediscounted  * d.qty) / 
            (CASE WHEN (d.price * d.qty) = 0::numeric THEN 1.00::numeric 
            ELSE d.price * d.qty 
            END + COALESCE(( SELECT r.compensationsum 
            FROM recipe r 
            WHERE r.id = d.recipeid), 0::numeric)), 4) 
            END) * 100::numeric,0),'999.99') AS discount_total, */
            (select rv.symbol from regvst rv where trim(rv.regnum)=trim(s.note1) limit 1) as symbol,
            CASE WHEN (nca.atc IS NOT NULL AND s.gr4 like any (array['Rx%'])) THEN true ELSE false END AS Saveable,
            coalesce(round(-tr.cheque_sum,2),0) as cheque_sum, 
            case when tr2.status in (10, 11, 12) then coalesce(round(-tr2.cheque_sum,2),0.00) else 0.00 end as cheque_sum_insurance, 
            coalesce(tr2.status, 0) as status_insurance,
            /*case when 
            round(coalesce((select dd.basic_price from tlk_kainos_bind bd inner 
            join tlk_kainos_d dd on dd.tlkid=bd.tlkid where trunc(now()) between 
            r_price_start and r_price_end and dd.percent=100 and 
            bd.productid=d.productid limit 1),0.00),2)=0 then ' ' 
            else 
            to_char(round(coalesce((select dd.basic_price from tlk_kainos_bind bd inner 
            join tlk_kainos_d dd on dd.tlkid=bd.tlkid where trunc(now()) between 
            r_price_start and r_price_end and dd.percent=100 and 
            bd.productid=d.productid limit 1),0.00),2),'FM9990.99') end 
            as basic_price, */
            (select retail_price as rp from tlk_kainos_d k where k.tlkid=r.tlkid 
            and trunc(now()) between k.r_price_start and k.r_price_end  and percent=(select comppercent from compensation where id=r.compensationid) 
            order by k.r_price_start limit 1) as retail_price, 
            er.recipe_no as erecipe_no, 
            er.active as erecipe_active,
            er.status as eRecipeStatus,
            er.disease_code AS eRecipeDiseaseCode,
            r.deseasecode AS RecipeDiseaseCode,
            r.tlk_status,
            ncr.id AS NotCompensatedRecipeId,
            ncr.till_date AS NotCompensatedTillDate,
            ld.loyalty_type,
            case when coalesce(tr2.status, 0) = 12 then 1 else 0 end as have_recipe,
            case when coalesce(tr2.status, 0) <> 13 then 1 else 0 end as apply_insurance,
            tr2.info as InsuranceInfo,
            s.bcat_id as baltic_category_id,
            coalesce(s.gr4,'') as gr4,
            s.note2,
            pf.flag AS Flags,
            a.code as atc,
            d.discounttype as DiscountType,
            --fmd_required
            coalesce(
            case when s.gr4 like any (array['Rx%', 'VAC'])
            and s.atcid not in (select id from atc
            where code like 'O%'
            or code like 'V10%'
            or code like 'B05BA%'
            or code like 'B05BB%'
            or code like 'B05BC%'
            or code like 'B05X%'
            or code like 'V07AB%'
            or code like 'V08%'
            or code like 'V04CL%'
            or code like 'V01AA%')
            then true end,
            case when s.gr4 like 'OTC%' 
            and s.name2 like 'OMEPRAZOL%' 
            and (s.name like '%20%MG%' or s.name like '%40%MG%')--RN
            then true end) as fmd_required,
            tr2.compensation_type,
            CASE WHEN sop.productid IS NULL THEN false ELSE true END AS IsSalesOrderProduct,
            0 as PresentCardId,
            q.qty AS OnHandQty,
            u.ratio AS BarcodeRatio
            --fmd_required
            from posd d 
            inner join stock s on s.id=d.productid 
            left join producer prr on prr.id=s.manufid 
            left join barcode b on b.id=d.barcodeid 
            inner join taxes t on t.vatsize=d.vatproc and t.modul='S' 
            left join recipe r on r.id=d.recipeid 
            left join cheque_trans tr on tr.posd_id=d.id and tr.posh_id = d.hid and tr.cheque_code is not null
            left join cheque_trans tr2 on tr2.posd_id=d.id and tr2.posh_id = d.hid and tr2.cheque_code is null
            left join compensation c on c.id=r.compensationid 
            left join product_info pi on s.id=pi.productid and (select tamro_owned from systemdata limit 1)=1 
            left join erecipe er on er.posd_id=d.id and er.posh_id = d.hid
            left join loyaltyd ld on ld.posd_id=d.id and ld.posh_id = d.hid and ld.loyalty_type = 'P'
            left join atc a on a.id=s.atcid
            left join vaccination v on v.posd_id=d.id
            left join product_flag pf on s.id = pf.productid
            left join sales_order_product sop on sop.productid = d.productid
            left join not_compensated_atc nca on nca.atc = a.code
            left join not_compensated_recipe ncr on ncr.posh_id = d.hid and ncr.posd_id = d.id
            left join unit u on u.id = b.unitid
            left join quantity q on q.productid = s.id and q.storeid = 10000000101
            where d.hid=@HID
            order by d.id DESC
            )f
            UNION ALL 
            SELECT 
            advancepaymentid as id, 
            CAST('ADVANCEPAYMENT' AS CHARACTER VARYING) as type,
            '' as info,
            1 as productid,
            1 as barcodeid,
            0 as recipeid,
            0.00 as discount,
            1.000 as qty,
            0.00 as discount_sum,
            poshid as hid,
            '0' as barcode,
            info AS barcodename,
            CASE WHEN type='ESHOP' THEN 'E-parduotuvės užsakymas nr. ' || info
            WHEN type='PRESENTCARD' THEN 'Dovanų kuponas nr. ' || info
            ELSE info END AS barcodename_info,
            '' as barcodename2,
            null as row_no,
            null as gqty,
            null as paysum,
            null as compensationsum,
            null as compcode,
            null as salesprice,
            null as recipeno,
            null as basicprice,
            null as till_date,
            null as salesdate,
            null as totalsum,
            0.00 as vatsize,
            null as rqty,
            1 as RetailProductRatio,
            null as comppercent,
            sum as price,
            sum as pricediscounted,
            sum as sum,
            0 AS VaccinePrescriptionCompositionId,
            0 AS VaccineDispensationCompositionId,
            0 AS VaccinatedPatientId,
            0 as ecrtax,
            null as endsum,
            null as symbol,
            false as Saveable,
            0 as cheque_sum,
            0.00 as cheque_sum_insurance,
            0 as status_insurance,
            null as retail_price,
            null as erecipe_no,
            null as erecipe_active,
            null as eRecipeStatus,
            null AS eRecipeDiseaseCode,
            null AS RecipeDiseaseCode,
            null as tlk_status,
            0 AS NotCompensatedRecipeId,
            null AS NotCompensatedRecipeTillDate,
            '' as loyalty_type,
            0 as have_recipe,
            0 as apply_insurance,
            null as insuranceinfo,
            0 as baltic_category_id,
            '' as gr4,
            '' as note2,
            0 AS Flags,
            '' as atc,
            '' as DiscountType,
            null as fmd_required,
            '' as compensation_type,
            false as IsSalesOrderProduct,
            present_card_id as PresentCardId,
            0 as OnHandQty,
            1 as BarcodeRatio
            FROM advancepayment
            WHERE poshid=@HID";
            cmd.Parameters.AddWithValue("@HID", posh_id);

            return await DoSelectList<T>(cmd);
        }

        public DataTable getPaymentType()
        {
            NpgsqlCommand cmd = new NpgsqlCommand();
            cmd.CommandText = @"select stock.id, stock.name from stock, type where stock.typeid=type.id and type.modul='K' and stock.id='63833' 
                                union 
                                select 0 as id, 'Grynais' as name";

            return queryGetDataTable(cmd);
        }

        public async Task<List<T>> getPayment<T>()
        {
            NpgsqlCommand cmd = new NpgsqlCommand();
            cmd.CommandText = "SELECT * FROM payment";

            return await DoSelectList<T>(cmd);
        }

        public async Task<DataTable> GetPosh()
        {
            NpgsqlCommand cmd = new NpgsqlCommand();
            cmd.CommandText = @"SELECT ph.id, ph.checkno, ph.DiscountSum FROM posh ph
                                WHERE ph.status='N' AND ph.type='K' AND ph.debtorid=@DEBTORID";
            cmd.Parameters.AddWithValue("@DEBTORID", Session.Devices.debtorid);

            return await DoSelectDataTable(cmd);
        }

        public async Task<decimal> CreatePosh()
        {
            NpgsqlCommand cmd = new NpgsqlCommand();
            cmd.CommandText = "SELECT create_posh(@db_code, @DEBTORID, @DOCUMENTDATE, 'K', 'N', @USERID, @DEVICENO)";
            cmd.Parameters.AddWithValue("@db_code", Session.SystemData.code);
            cmd.Parameters.AddWithValue("@DEBTORID", Session.Devices.debtorid);
            cmd.Parameters.AddWithValue("@DOCUMENTDATE", DateTime.Now.ToString());
            cmd.Parameters.AddWithValue("@USERID", Session.User.id);
            cmd.Parameters.AddWithValue("@DEVICENO", Session.Devices.deviceno);

            return await DoSelectValue<decimal>(cmd);
        }

        public async Task<bool> update_posh_date(decimal posh_id)
        {
            NpgsqlCommand cmd = new NpgsqlCommand();
            cmd.CommandText = "update posh set documentdate=CURRENT_TIMESTAMP, userid=@db_userid where id=@HID";
            cmd.Parameters.AddWithValue("@db_userid", Session.User.id);
            cmd.Parameters.AddWithValue("@HID", posh_id);

            return await DoSelectVoid(cmd);
        }
        public async Task<decimal> create_posd_service(decimal posh_id, string barcode, decimal mode, decimal sum)
        {
            NpgsqlCommand cmd = new NpgsqlCommand();
            cmd.CommandText = "SELECT create_posd_pos(@db_code,@HID,@DEBTORID,@STOREID,@edt_barcode,@MODE,@PRICEMASK,@PRICECODE,@WEIGHTMASK,@WEIGHTCODE,@PRICEFIFO,@WEIGHTFIFO,@FIFOMODE)";
            cmd.Parameters.AddWithValue("@db_code", Session.SystemData.code);
            cmd.Parameters.AddWithValue("@HID", posh_id);
            cmd.Parameters.AddWithValue("@DEBTORID", Session.Devices.debtorid);
            cmd.Parameters.AddWithValue("@STOREID", Session.SystemData.storeid);
            cmd.Parameters.AddWithValue("@edt_barcode", barcode);
            cmd.Parameters.AddWithValue("@MODE", mode);
            cmd.Parameters.AddWithValue("@PRICEMASK", "");
            cmd.Parameters.AddWithValue("@PRICECODE", 0);
            cmd.Parameters.AddWithValue("@WEIGHTMASK", "");
            cmd.Parameters.AddWithValue("@WEIGHTCODE", 0);
            cmd.Parameters.AddWithValue("@PRICEFIFO", 0);
            if (mode == 1)
                cmd.Parameters.AddWithValue("@WEIGHTFIFO", 0);
            else
                cmd.Parameters.AddWithValue("@WEIGHTFIFO", 1);
            cmd.Parameters.AddWithValue("@FIFOMODE", sum);

            return await DoSelectValue<decimal>(cmd);
        }

        public async Task<decimal> CreatePosdPos(decimal posh_id, string barcode, decimal mode, decimal sum)
        {
            NpgsqlCommand cmd = new NpgsqlCommand();
            cmd.CommandText = "SELECT create_posd_pos(@db_code,@HID,@DEBTORID,@STOREID,@edt_barcode,@MODE,@PRICEMASK,@PRICECODE,@WEIGHTMASK,@WEIGHTCODE,@PRICEFIFO,@WEIGHTFIFO,@FIFOMODE)";
            cmd.Parameters.AddWithValue("@db_code", Session.SystemData.code);
            cmd.Parameters.AddWithValue("@HID", posh_id);
            cmd.Parameters.AddWithValue("@DEBTORID", Session.Devices.debtorid);
            cmd.Parameters.AddWithValue("@STOREID", Session.SystemData.storeid);
            cmd.Parameters.AddWithValue("@edt_barcode", barcode);
            cmd.Parameters.AddWithValue("@MODE", mode);
            if (mode == 0 || mode == 6)
            {
                string[] temp = Session.Devices.pricemask.Split('*');
                cmd.Parameters.AddWithValue("@PRICEMASK", temp[0]);
                if (temp.Length > 1)
                    cmd.Parameters.AddWithValue("@PRICECODE", temp[1]);
                else
                    cmd.Parameters.AddWithValue("@PRICECODE", 0);
                if (temp.Length > 2)
                    cmd.Parameters.AddWithValue("@PRICEFIFO", temp[2]);
                else
                    cmd.Parameters.AddWithValue("@PRICEFIFO", 0);
                temp = Session.Devices.weightmask.Split('*');
                cmd.Parameters.AddWithValue("@WEIGHTMASK", temp[0]);
                if (temp.Length > 1)
                    cmd.Parameters.AddWithValue("@WEIGHTCODE", temp[1]);    
                else
                    cmd.Parameters.AddWithValue("@WEIGHTCODE", 0);
                if (temp.Length > 2)
                    cmd.Parameters.AddWithValue("@WEIGHTFIFO", temp[2]);
                else
                    cmd.Parameters.AddWithValue("@WEIGHTFIFO", 0);
                cmd.Parameters.AddWithValue("@FIFOMODE", Session.FifoMode);
            }
            if (mode == 1)
            {
                cmd.Parameters.AddWithValue("@PRICEMASK", "");
                cmd.Parameters.AddWithValue("@PRICECODE", 0);
                cmd.Parameters.AddWithValue("@WEIGHTMASK", "");
                cmd.Parameters.AddWithValue("@WEIGHTCODE", 0);
                cmd.Parameters.AddWithValue("@PRICEFIFO", 0);
                cmd.Parameters.AddWithValue("@WEIGHTFIFO", 0);
                cmd.Parameters.AddWithValue("@FIFOMODE", sum);
            }
            if (mode == 2 || mode == 3 || mode == 4 || mode == 5)
            {
                cmd.Parameters.AddWithValue("@PRICEMASK", "");
                cmd.Parameters.AddWithValue("@PRICECODE", 0);
                cmd.Parameters.AddWithValue("@WEIGHTMASK", "");
                cmd.Parameters.AddWithValue("@WEIGHTCODE", 0);
                cmd.Parameters.AddWithValue("@PRICEFIFO", 0);
                cmd.Parameters.AddWithValue("@WEIGHTFIFO", 1);
                cmd.Parameters.AddWithValue("@FIFOMODE", sum);
            }

            return await DoSelectValue<decimal>(cmd);
        }

        public async Task<decimal> CalcRimiDisc(decimal posh_id)
        {
            NpgsqlCommand cmd = new NpgsqlCommand();
            cmd.CommandText = "SELECT COALESCE(calc_discount_rimi(@HID), 0)";
            cmd.Parameters.AddWithValue("@HID", posh_id);

            return await DoSelectValue<decimal>(cmd);
        }

        public async Task<string> GetRimiCardNo(decimal posh_id)
        {
            NpgsqlCommand cmd = new NpgsqlCommand();
            cmd.CommandText = "SELECT COALESCE(cardno, '') FROM disc_log WHERE hid=@HID";
            cmd.Parameters.AddWithValue("@HID", posh_id);

            return await DoSelectValue<string>(cmd);
        }

        public async Task<int> GetEmployeePos(decimal posh_id)
        {
            NpgsqlCommand cmd = new NpgsqlCommand();
            cmd.CommandText = "SELECT CAST(COUNT(id) AS INT) AS count FROM employee_pos WHERE hid=@HID";
            cmd.Parameters.AddWithValue("@HID", posh_id);

            return await DoSelectValue<int>(cmd);
        }

        public async Task<int> GetDoctorPos(decimal posh_id)
        {
            NpgsqlCommand cmd = new NpgsqlCommand();
            cmd.CommandText = "SELECT CAST(COUNT(id) AS INT) AS count FROM posd WHERE hid=@HID AND (discounttype LIKE 'VVG%' OR discounttype LIKE 'RIM%')";
            cmd.Parameters.AddWithValue("@HID", posh_id);

            return await DoSelectValue<int>(cmd);
        }

        public async Task<string> getNextSalehNo(string doc_no)
        {
            NpgsqlCommand cmd = new NpgsqlCommand();
            cmd.CommandText = string.Format("SELECT CONCAT(CAST('{0}' AS CHARACTER VARYING), CAST(COUNT(1) + 1 AS CHARACTER VARYING)) AS num FROM saleh WHERE documentno LIKE '{0}%'", doc_no);

            return await DoSelectValue<string>(cmd);
        }

        public List<T> getPOSErrors<T>()
        {
            NpgsqlCommand cmd = new NpgsqlCommand();
            cmd.CommandText = @"SELECT 
                                system,
                                code,
                                CASE WHEN description = '' THEN description_orig ELSE description END AS description, 
                                type 
                                FROM pos_errors ";

            return queryGetList<T>(cmd);
        }

        public async Task<string> getGr4(decimal product_id)
        {
            NpgsqlCommand cmd = new NpgsqlCommand();
            cmd.CommandText = @"select coalesce(gr4, '') as gr4 from stock where id = @product_id";
            cmd.Parameters.AddWithValue("@product_id", product_id);

            return await DoSelectValue<string>(cmd);
        }

        public async Task<decimal> getBarcodeRatio(decimal productid)
        {
            NpgsqlCommand cmd = new NpgsqlCommand();
            cmd.CommandText = "SELECT get_barcode_ratio(get_barcodeid(@productid))";
            cmd.Parameters.AddWithValue("@productid", productid);

            return await DoSelectValue<decimal>(cmd);
        }

        public async Task<List<T>> getStickers<T>()
        {
            NpgsqlCommand cmd = new NpgsqlCommand();
            cmd.CommandText = @"SELECT h.id, name, prefix, amount_from, amount_to, message_type, message FROM stickersh h
                                LEFT JOIN stickersd d ON h.id=d.hid
                                WHERE NOW() BETWEEN valid_from AND valid_to";

            return await DoSelectList<T>(cmd);
        }

        public async Task<List<T>> GetFMDitem<T>(decimal posh_id, decimal posd_id)
        {
            NpgsqlCommand cmd = new NpgsqlCommand
            {
                CommandText = @"SELECT * FROM fmd_trans 
                                WHERE poshid=@posh_id
                                AND posdid=@posd_id
                                AND deleted=false;"
            };
            cmd.Parameters.AddWithValue("@posh_id", posh_id);
            cmd.Parameters.AddWithValue("@posd_id", posd_id);

            return await DoSelectList<T>(cmd);
        }

        public async Task<decimal> deleteFMDitem(decimal id)
        {
            NpgsqlCommand cmd = new NpgsqlCommand
            {
                CommandText = @"SELECT delete_fmd_trans(@id);"
            };
            cmd.Parameters.AddWithValue("@id", id);

            return await DoSelectValue<decimal>(cmd);
        }
        
        public async Task<decimal> CreateFMDtrans(wpf.Model.fmd model)
        {
            NpgsqlCommand cmd = new NpgsqlCommand
            {
                CommandText = "SELECT create_fmd_trans(@posh_id, @posd_id, @documentdate, @debtorid, @userid, @product_code_scheme, @product_code, @serial_number, @batch_id, @expiry_date, @type, @operation_code, @state, @information, @warning, @alert_id, @isvalidforsale, @referencenumber, @deleted);"
            };
            cmd.Parameters.AddWithValue("@posh_id", model.posDetail?.hid ?? 0);
            cmd.Parameters.AddWithValue("@posd_id", model.posDetail?.id ?? 0);
            cmd.Parameters.AddWithValue("@documentdate", DateTime.Now);
            cmd.Parameters.AddWithValue("@debtorid", Session.Devices.debtorid);
            cmd.Parameters.AddWithValue("@userid", Session.User.id);
            cmd.Parameters.AddWithValue("@product_code_scheme", model.productCodeScheme);
            cmd.Parameters.AddWithValue("@product_code", model.productCode);
            cmd.Parameters.AddWithValue("@serial_number", model.serialNumber);
            cmd.Parameters.AddWithValue("@batch_id", model.batchId);
            cmd.Parameters.AddWithValue("@expiry_date", model.expiryDate);
            cmd.Parameters.AddWithValue("@type", model.type);
            cmd.Parameters.AddWithValue("@operation_code", model.Response.operationCode);
            cmd.Parameters.AddWithValue("@state", model.Response.state);
            cmd.Parameters.AddWithValue("@information", model.Response.information);
            cmd.Parameters.AddWithValue("@warning", model.Response.warning);
            cmd.Parameters.AddWithValue("@alert_id", model.Response.alertId);
            cmd.Parameters.AddWithValue("@isvalidforsale", model.Response.Success);
            cmd.Parameters.AddWithValue("@referencenumber", model.referencenumber);
            cmd.Parameters.AddWithValue("@deleted", model.deleted);

            return await DoSelectValue<decimal>(cmd);
        }

        [ObsoleteAttribute("Use same method from 'POS' repository", false)]
        public async Task<decimal> DeleteAdvancePayment(decimal advancePaymentId)
        {
            NpgsqlCommand cmd = new NpgsqlCommand
            {
                CommandText = @"SELECT delete_advancepayment(@advancePaymentId);"
            };
            cmd.Parameters.AddWithValue("@advancePaymentId", advancePaymentId);

            return await DoSelectValue<decimal>(cmd);
        }

        [ObsoleteAttribute("Use same method from 'POS' repository", false)]
        public async Task<decimal> CreateAdvancePayment(decimal poshId, string advancePaymentType, string orderNumber, decimal price)
        {
            NpgsqlCommand cmd = new NpgsqlCommand
            {
                CommandText = @"SELECT create_advancepayment(@id, @advancePaymentType, @orderNumber, @price);"
            };
            cmd.Parameters.AddWithValue("@id", poshId);
            cmd.Parameters.AddWithValue("@advancePaymentType", advancePaymentType);
            cmd.Parameters.AddWithValue("@orderNumber", orderNumber);
            cmd.Parameters.AddWithValue("@price", price);

            return await DoSelectValue<decimal>(cmd);
        }

        [ObsoleteAttribute("Use same method from 'POS' repository", false)]
        public async Task<decimal> CreateAdvancePaymentDuplicate(decimal poshId,decimal advancepaymentid)
        {
            NpgsqlCommand cmd = new NpgsqlCommand
            {
                CommandText = @"SELECT create_advancepayment_duplicate(@poshId, @advancepaymentid);"
            };
            cmd.Parameters.AddWithValue("@poshId", poshId);
            cmd.Parameters.AddWithValue("@advancepaymentid", advancepaymentid);

            return await DoSelectValue<decimal>(cmd);
        }

        public async Task<decimal> GetPrepTransCreditor(decimal posh_id)
        {
            var cmd = new NpgsqlCommand();
            cmd.CommandText = "SELECT creditor_id FROM prep_trans where posh_id =@posh_id";
            cmd.Parameters.AddWithValue("@posh_id", posh_id);

            return await DoSelectValue<decimal>(cmd);
        }
    }
}
