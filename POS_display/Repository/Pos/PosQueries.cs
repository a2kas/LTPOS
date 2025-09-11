using System.Web;
using System.Windows.Controls.Primitives;

namespace POS_display.Repository.Pos
{
    public static class PosQueries
    {
        public static string AsyncAutoComplete => @"SELECT @columnName FROM @tableName WHERE @columnName IS NOT NULL AND TRIM(@columnName) <> ''";

        public static string CreatePosPayment =>
            @"INSERT INTO pospayment 
            (hid, paymentid, code, paymenttype, amount)
            VALUES
            (@Hid, @PaymentId, @Code, @PaymentTypeString, @Amount);";

        public static string GetPosPayment =>
            @"SELECT * FROM pospayment WHERE hid=@Hid AND deleted = @deleted;";

        public static string VoidPosPayment =>
            @"UPDATE pospayment SET deleted = true WHERE hid = @Hid AND id = @Id AND deleted = false;";

        public static string UpdateSession =>            
            @"SELECT update_session(@db_code, @db_userid, @Host, @Action, @Fmode)";

        public static string ChangePosdPrice => "SELECT change_posd_price(@id, @price_new) > 0";

        public static string AsyncECRReports => "SELECT ecr_reports(@db_code, @cmb_report, @debtorid, @db_userid, @edt_change)";

        public static string GetProductGr4 => "SELECT coalesce(gr4, '') AS gr4 FROM stock WHERE id = @productId";

        public static string GetFMDItemByPosIds => "SELECT* FROM fmd_trans WHERE poshid = @posh_id AND posdid = @posd_id AND deleted = false AND documentdate > NOW() - INTERVAL '6 month';";

        public static string GetFMDItem => "SELECT * FROM fmd_trans WHERE id = @id AND deleted = false AND documentdate > NOW() - INTERVAL '6 month';";

        public static string DeleteFMDItem => "SELECT * FROM fmd_trans WHERE id = @id AND deleted = false;";

        public static string CreateFMDItem => @"SELECT create_fmd_trans(@posh_id,
             @posd_id, @documentdate, @debtorid, @userid, @product_code_scheme, @product_code,
             @serial_number, @batch_id, @expiry_date, @type, @operation_code, @state,
             @information, @warning, @alert_id, @isvalidforsale, @referencenumber, @deleted);";

        public static string UpdateFMDItem => @"SELECT update_fmd_trans(@id, @operation_code, @state,
              @information, @warning, @alert_id, @isvalidforsale, @referencenumber, @deleted);";

        public static string CreateSaledFromPOS => @"SELECT create_saledfrompos(@db_code, @debtor_id, @date);";

        public static string CheckIfSuppliedFMDItemExist => @"SELECT COUNT(id) <> 0 FROM fmd_trans WHERE type = 'supply' AND
                                                            state = 'Supplied' AND productid = @product_id AND productcode = @product_code AND
                                                            batchid = @batch_id AND serialnumber = @serial_number AND alertid IS NULL AND documentdate > NOW() - INTERVAL '6 month';";

        public static string IsCompensated => @"SELECT is_compensated(@product_id);";

        public static string IsCheapest => @"SELECT COALESCE((SELECT cheapest FROM tlk_kainos_d WHERE tlkid = @npkaid7 AND cheapest = 1 AND NOW() >= r_price_start AND NOW() < r_price_end GROUP BY tlkid, r_price_start, r_price_end, cheapest),0);";

        public static string InsertPosZ => @"INSERT INTO pos_z (date,
                                                              znr,
                                                              pos,
                                                              cash,
                                                              credit,
                                                              card,
                                                              cheque,
                                                              vata,
                                                              vatb,
                                                              vatc,
                                                              vatd,
                                                              vate,
                                                              vatf,
                                                              totala,
                                                              totalb,
                                                              totalc,
                                                              totald,
                                                              totale,
                                                              totalf,
                                                              packa,
                                                              packb,
                                                              packc,
                                                              packd,
                                                              packe,
                                                              packf,
                                                              gpack,
                                                              gcred,
                                                              gcash,
                                                              discqty,
                                                              overqty,
                                                              discsum,
                                                              oversum,
                                                              cashin,
                                                              cashout,
                                                              pay1,
                                                              pay2,
                                                              pay3,
                                                              pay4,
                                                              advancein1,
                                                              advanceout1,
                                                              advancein2,
                                                              advanceout2,
                                                              advancein3,
                                                              advanceout3,
                                                              cashrest,
                                                              created,
                                                              a_tax_percent,
                                                              a_total,
                                                              a_total_tax,
                                                              a_total_w_tax,
                                                              c_tax_percent,
                                                              c_total,
                                                              c_total_tax,
                                                              c_total_w_tax,
                                                              total_income_w_tax,
                                                              b_tax_percent,
                                                              b_total,
                                                              b_total_tax,
                                                              b_total_w_tax
                                                              ) VALUES (
                                                              @date,
                                                              @znr,
                                                              @pos,
                                                              @cash,
                                                              @credit,
                                                              @card,
                                                              @cheque,
                                                              @vata,
                                                              @vatb,
                                                              @vatc,
                                                              @vatd,
                                                              @vate,
                                                              @vatf,
                                                              @totala,
                                                              @totalb,
                                                              @totalc,
                                                              @totald,
                                                              @totale,
                                                              @totalf,
                                                              @packa,
                                                              @packb,
                                                              @packc,
                                                              @packd,
                                                              @packe,
                                                              @packf,
                                                              @gpack,
                                                              @gcred,
                                                              @gcash,
                                                              @discqty,
                                                              @overqty,
                                                              @discsum,
                                                              @oversum,
                                                              @cashin,
                                                              @cashout,
                                                              @pay1,
                                                              @pay2,
                                                              @pay3,
                                                              @pay4,
                                                              @advancein1,
                                                              @advanceout1,
                                                              @advancein2,
                                                              @advanceout2,
                                                              @advancein3,
                                                              @advanceout3,
                                                              @cashrest,
                                                              @created,
                                                              @ataxpercent,
                                                              @atotal,
                                                              @atotaltax,
                                                              @atotalwtax,
                                                              @ctaxpercent,
                                                              @ctotal,
                                                              @ctotaltax,
                                                              @ctotalwtax,
                                                              @totalincomewtax,
                                                              @btaxpercent,
                                                              @btotal,
                                                              @btotaltax,
                                                              @btotalwtax);";


        public static string GetPharmaceuticalForms => @"SELECT CASE WHEN pharmaceutical_form = '' THEN 'Nenurodyta' ELSE pharmaceutical_form END FROM pharmaceutical_form";

        public static string GetPromotionCheques => @"SELECT 
                                                        d.hid AS HeaderId,
                                                        d.line_no AS LineNo,
                                                        d.line AS Line,
                                                        d.style AS Style,
                                                        h.nolc AS NoLC,
                                                        h.rimi AS Rimi,
                                                        h.benu AS Benu,
                                                        h.cost as Cost
                                                    FROM prom_cheque_h h
                                                    LEFT JOIN prom_cheque_d d on h.id = d.hid
                                                    WHERE DATE(NOW()) BETWEEN valid_from AND valid_to AND enabled = 1
                                                    ORDER BY line_no";

        public static string GetTLKPricesByNpakid7List => @"SELECT tlkid AS Npakid7, kompensation AS Compensation, percent, retail_price AS RetailPrice, cheapest AS IsCheapest FROM tlk_kainos_d WHERE tlkid IN ({0}) AND NOW() >= r_price_start AND NOW() <= r_price_end";

        public static string GetTLKPricesByProductIDs => @"SELECT bind.productid, d.tlkid AS Npakid7, d.kompensation AS Compensation, d.percent, d.retail_price AS RetailPrice, d.cheapest AS IsCheapest FROM 
                                                        tlk_kainos_d d
                                                        LEFT JOIN tlk_kainos_bind bind ON bind.tlkid = d.tlkid WHERE bind.productid IN ({0}) 
                                                        AND NOW() >= r_price_start AND NOW() <= r_price_end";

        public static string GetGenericItemsData => @"SELECT productid, vatsize, priority FROM search_genericitemdata WHERE productid IN ({0})";

        public static string GetUserPrioritesRatioInPeriod => @"SELECT get_user_priorities_ratio_in_period(@userId, @from, @to);";

        public static string GetProductsLocations => @"SELECT productid,
                                                              oficina_location AS Oficina,
                                                              oficina_2_location AS Oficina2,
                                                              stock_location AS Stock
                                                     FROM product_location";

        public static string CreateAdvancePayment => @"SELECT create_advancepayment(@poshid, @advancePaymentType, @orderNumber, @price, @presentCardId);";

        public static string DeleteAdvancePayment => @"SELECT delete_advancepayment(@advancePaymentId);";

        public static string CreateAdvancePaymentDuplicate => @"SELECT create_advancepayment_duplicate(@poshId, @advancepaymentid);";

        public static string CreateSaleDetail => @"SELECT create_saled(@db_code, @hid, @debtorid, @productid, @barcodeid,
                                        @storeid, @serialid, @qty, @price, @discount, @pricediscounted, 
                                        @sum, @vatamount, @costprice, @currencyid, @fifoid, @objectid, @jobhid, @vatproc, @returned_posd_id)";

        public static string CreateSaleHeader => "SELECT create_saleh(@db_code)";

        public static string GetNextSaleHeaderNo => "SELECT CONCAT(CAST('@doc_no' AS CHARACTER VARYING), CAST(COUNT(1) + 1 AS CHARACTER VARYING)) AS num FROM saleh WHERE documentno LIKE '@doc_noPrefix'";

        public static string ReturnSaleHeader => "SELECT return_saleh(@id, @db_code)";

        public static string SetSaleHeaderTransId => "UPDATE saleh SET transid = @transId WHERE id = @id";

        public static string UpdateSaleHeader => "SELECT update_saleh(@id, @debtorid, @type, @documentno, @documentdate, @currencyid, @currate, @payedfor, @corect0, @corect5, @corect18, @corectdis, @corect19, @corect21)";

        public static string GetLastEKJEntryByType => "SELECT * FROM ekj WHERE type = @ekjType ORDER BY date DESC LIMIT 1";

        public static string GetZReportEntryByEkjId => "SELECT * FROM z_report WHERE ekj_id = @ekj_id";

        public static string GetDeviceSetttingValueByKey => "SELECT * FROM settings WHERE key = @key";

        public static string CalculateRimiDiscount => "SELECT calculate_discount_rimi(@posHeaderId)";

        public static string HasChequePresentCard => "SELECT COUNT(id) <> 0 FROM posd WHERE hid = @posHeaderId AND productid = 10000100596";

        public static string InsertPosMemo => "INSERT INTO pos_memo VALUES (@debtorId, @paramKey, @value)";

        public static string UpdatePosMemo => "UPDATE pos_memo SET value = @value WHERE debtor_id = @debtorId AND parameter = @paramKey";

        public static string GetPosMemoValue => "SELECT value FROM pos_memo WHERE debtor_id = @debtorId AND parameter = @paramKey";

        public static string GetProductFalgs => @"SELECT productid, flag FROM product_flag";

        public static string GetExclusiveProducts => @"SELECT productid, npakid7 FROM exclusive_product";

        public static string GetCheapests => @"SELECT kainyno_versija AS PriceListVersion, pradzia AS StartDate, tlkid AS Npakid7, pigiausias AS IsCheapest FROM tlk_vaistai_pigiausi";

        public static string GetLackOfSales => @"SELECT barcode, name, qty, qty_left AS QtyLeft, qty_lack AS QtyLack FROM report_lackofsale";

        public static string WriteDBLog => @"SELECT generate_log(@log)";

        public static string GetBarcodeByProductId => @"SELECT barcode FROM barcode WHERE LENGTH(barcode) > 6 AND barcode ~ '^[0-9].*[0-9]$' AND productid = @productId LIMIT 1;";

        public static string GetPosd => @"SELECT * FROM (
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
            er.dispense_by_substances_group_id AS eRecipeDispenseBySubstancesGroupId,
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
            '' as eRecipeDispenseBySubstancesGroupId, 
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
    }
}
