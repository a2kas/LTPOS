using Hl7.Fhir.Model;
using System;
using System.Windows.Controls.Primitives;
using static Hl7.Fhir.Model.Composition;
using static Hl7.Fhir.Model.MedicationDispense;

namespace POS_display.Repository.Recipe
{
    public static class RecipeQueries
    {
        public static string GetPriceRestrictions =>  "SELECT * FROM tlk_kainos_ribojimai WHERE tlkid=@tlkid";
        public static string CreateVaccination => @"
                                    SELECT create_vaccination
                                    (@posh_id,
                                     @posd_id, 
                                     @order_id,
                                     @prescription_composition_id, 
                                     @prescription_composition_ref, 
                                     @dispensation_composition_id, 
                                     @dispensation_composition_ref,
                                     @patient_id
                                    )";


        public static string GetNpakId7 = @"
                                SELECT b.tlkid
                                FROM tlk_kainos_bind b
                                INNER JOIN tlk_kainos_d d on b.tlkid=d.tlkid 
                                WHERE b.productid = @productid 
                                AND now() BETWEEN r_price_start AND r_price_end AND d.percent=100;";

        public static string SetAdditionalInstructions => "UPDATE recipe SET additionalinstructions = @additionalinstructions WHERE id = @id";

        public static string SetERecipeDiseaseCode => "UPDATE erecipe SET disease_code = @disease_code WHERE id = @id";

        public static string SetAccumulatedSurchargeData => @"UPDATE recipe SET 
                                                                surcharge_eligible = @surcharge_eligible,
                                                                surcharge_amount = @surcharge_amount,
                                                                missing_surcharge_amount = @missing_surcharge_amount,
                                                                surcharge_valid_to = @surcharge_valid_to
                                                             WHERE id = @id";

        public static string CheckUnsignedUkraineRefugeeRecipes => @"
                                SELECT COUNT(er.id) <> 0 FROM erecipe er WHERE er.posd_id IN 
                                (
                                    SELECT ct.posd_id FROM
                                    cheque_trans ct WHERE ct.cheque_from = 'SAM' AND ct.status = 10) 
                                AND er.userid = @userid AND LOWER(er.documentstatus) = 'final' AND er.info = ''
                                AND er.documentdate <= now()::date + interval '1h'";

        public static string ExistDiseaseCodeInRefugeeDiseaseCodes => @"SELECT COUNT(id) <> 0 FROM refugee_diseasecode WHERE disease_code = @diseasecode";

        public static string GetVaccineDays => "SELECT product_id AS ProductId, " +
                                               "number_of_vaccine AS NumberOfVaccine," +
                                               " days AS Days FROM vaccine_days";

        public static string UpdateRecipeCompensationData => "UPDATE recipe SET" +
            " prepayment_compensation = @prepayment_compensation," +
            " is_first_prescription = @is_first_prescription," +
            " low_income_tag = @low_income_tag," +
            " is_prepayment_compensation = @is_prepayment_compensation WHERE id = @recipeId;";

        public static string NewRecipeDataWithPosDetail => @"SELECT
	                COALESCE((SELECT k.kompensation FROM tlk_kainos_d k WHERE sr.tlkid= k.tlkid AND k.percent= 50 AND
	                TRUNC(NOW()) BETWEEN k.r_price_start AND k.r_price_end), 0) AS c50,
	                COALESCE((SELECT k.kompensation FROM tlk_kainos_d k WHERE sr.tlkid = k.tlkid AND k.percent = 80 AND
	                TRUNC(NOW()) BETWEEN k.r_price_start AND k.r_price_end), 0) AS c80,
	                COALESCE((SELECT k.kompensation FROM tlk_kainos_d k WHERE sr.tlkid = k.tlkid AND k.percent = 90 AND
	                TRUNC(NOW()) BETWEEN k.r_price_start AND k.r_price_end), 0) AS c90,
	                COALESCE((SELECT k.kompensation FROM tlk_kainos_d k WHERE sr.tlkid = k.tlkid AND k.percent = 100 AND
	                TRUNC(NOW()) BETWEEN k.r_price_start AND k.r_price_end), 0) AS c100, sr.* 
                FROM search_recipeprice sr
                LEFT JOIN tlk_kainos_bind b ON sr.tlkid=b.tlkid
                WHERE sr.barcode = @barcode";

        public static string NewRecipeDataWithoutPosDetail => @"SELECT 
            (SELECT COALESCE(k.kompensation, 0) FROM tlk_kainos_d k WHERE sr.tlkid=k.tlkid AND k.percent=50 AND 
            TRUNC(@checkDate::date) BETWEEN k.r_price_start AND k.r_price_end) AS c50, 
            (SELECT COALESCE(k.kompensation, 0) FROM tlk_kainos_d k WHERE sr.tlkid=k.tlkid AND k.percent=80 AND 
            TRUNC(@checkDate::date) BETWEEN k.r_price_start AND k.r_price_end) AS c80, 
            (SELECT COALESCE(k.kompensation, 0) FROM tlk_kainos_d k WHERE sr.tlkid=k.tlkid AND k.percent=90 AND 
            TRUNC(@checkDate::date) BETWEEN k.r_price_start AND k.r_price_end) AS c90, 
            (SELECT COALESCE(k.kompensation, 0) FROM tlk_kainos_d k WHERE sr.tlkid=k.tlkid AND k.percent=100 AND 
            TRUNC(@checkDate::date) BETWEEN k.r_price_start AND k.r_price_end) AS c100, sr.* 
        FROM (
            SELECT 
                s.id AS productid, 
                t.vatsize, 
                b.barcode, 
                b.id AS barcodeid, 
                b.name AS productname, 
                up.ratio AS prodratio, 
                ub.ratio AS barratio, 
                COALESCE(tt.basic_price, 0) AS basicprice, 
                COALESCE(tt.retail_price, 0) AS salesprice, 
                COALESCE(tt.retail_price, 0) AS newsalesprice, 
                COALESCE(tlkii.tlkid, '') AS tlkid, 
                COALESCE(tlkii.tlkid, '') AS code2, 
                0 AS oldsalesprice, 
                0 AS oldbasicprice, 
                s.retailpr AS retailpr, 
                @checkDate::date AS pricedate,
                COALESCE(tt.padeng_priemoka, 0) AS padeng_priemoka 
            FROM 
                barcode b 
                INNER JOIN stock s ON s.id=b.productid 
                INNER JOIN taxes t ON t.id=s.salesvatid 
                INNER JOIN unit up ON up.id=s.unitid 
                INNER JOIN unit ub ON ub.id=b.unitid 
                LEFT JOIN (
                    SELECT * FROM (
                        SELECT bd.productid, h.tlkid, d.r_price_start 
                        FROM tlk_kainos_h h 
                        INNER JOIN tlk_kainos_bind bd ON bd.tlkid=h.tlkid AND TRUNC(@checkDate::date) < h.date_end 
                        LEFT JOIN tlk_kainos_d d ON d.tlkid=h.tlkid AND TRUNC(@checkDate::date) BETWEEN d.r_price_start AND d.r_price_end AND d.percent = @compPercent 
                    ) t 
                    WHERE (t.r_price_start IS NOT NULL OR (t.tlkid LIKE 'M%' AND LENGTH(t.tlkid)=4) OR t.tlkid LIKE 'G%')
                ) tlkii ON tlkii.productid=s.id 
                LEFT JOIN (
                    SELECT bd.productid, d.retail_price, d.basic_price, bd.tlkid, TRUNC(d.r_price_start) AS price_start, TRUNC(d.r_price_end) AS price_end, padeng_priemoka 
                    FROM tlk_kainos_bind bd
            INNER JOIN tlk_kainos_d d ON bd.tlkid=d.tlkid 
            WHERE d.percent=@compPercent AND TRUNC(@checkDate::date) BETWEEN TRUNC(d.r_price_start) AND TRUNC(d.r_price_end) 
            ) tt ON tt.productid=s.id 
        ) sr 
        WHERE sr.barcode = @barcode";

        public static string GetNotCompensatedRecipdById => @"SELECT 
                                                        id,
                                                        doses,
                                                        qty_day,
                                                        count_day,
                                                        valid_from,
                                                        till_date
                                                  FROM not_compensated_recipe
                                                  WHERE id = @id";

        public static string CreateNotCompensatedRecipe => "SELECT create_not_compensated_recipe(@db_code, @posh_id, @posd_id, @valid_from, @doses, @qty_day, @count_day, @till_date);";

        public static string DeleteNotCompensatedRecipe => "DELETE FROM not_compensated_recipe WHERE posh_id = @posh_id AND posd_id = @posd_id;";

        public static string GetRecipeNoByCompositionIds => "SELECT DISTINCT recipe_no FROM erecipe WHERE composition_id IN ({0})";

        public static string GetPartialDispeneGroupId => @"SELECT dispense_by_substances_group_id FROM erecipe WHERE recipe_no = @recipe_no AND active = 1 LIMIT 1";

        public static string SetPartialDispeneGroupId => @"UPDATE erecipe SET dispense_by_substances_group_id = @group_id WHERE id = @recipeid";

        public static string GetPartialDispeneGroupIdByHeaderId => @"SELECT dispense_by_substances_group_id FROM erecipe WHERE posh_id = @posHeaderId AND active = 1 LIMIT 1";

        public static string GetRecipeEditDataByCompositionId => @"SELECT er.id AS ErecipeId,
                                                                    r.id AS RecipeId,
                                                                    r.countday AS CountDay,
                                                                    r.till_date AS TillDate,
                                                                    r.totalsum AS TotalSum,
                                                                    r.compensationsum AS CompensationSum,
                                                                    r.gqty AS GQty,
                                                                    r.paysum AS PaySum,
                                                                    r.prepayment_compensation AS PrepaymentCompensationSum
                                                                FROM erecipe er LEFT JOIN recipe r ON r.id = er.recipe_id WHERE er.composition_id = @compositionId";


        public static string UpdateRecipeByEditData => @"UPDATE recipe 
                                               SET 
                                                   countday = @countday,
                                                   till_date = @till_date,
                                                   totalsum = @totalsum,
                                                   compensationsum = @compensationsum,
                                                   gqty = @gqty,
                                                   paysum = @paysum,
                                                   prepayment_compensation = @prepayment_compensation
                                               WHERE id = @RecipeId;";
        public static string UpdateERecipeByEditData => @"UPDATE recipe 
                                               SET 
                                                   till_date = @till_date
                                               WHERE id = @ErecipeId;";

        public static string UpdatePosDetailByRecipe => @"SELECT update_posd_recipe(@PosdId, @BarcodeId, @Qty, @NewPaySum, @NewHId, @PaySum)";

        public static string CreateErecipe => @"SELECT create_erecipe(@PoshId, @PosdId, @ProductId, @UserId, @RecipeNo, @EncounterId , @RecipeId, @RecipeDate::date, @SalesDate::date, @TillDate::date)";

        public static string CreateRecipe => @"SELECT create_recipe(@DbCode, @TlkId, @BarcodeId, @RecSer, @RecipeNo, @PersCode, @ClinicId, @DeseaseCode, @DoctorId, @RecipeDate, @SalesPrice, @BasicPrice, @CompensationId, @Qty, @TotalSum, @CompSum, @PaySum, @SalesDate, @GQty, @Water, @TaxoLaborum, @Ext, @CheckDate, @CheckNo, @QtyDay, @CountDay, @TillDate, @KvpDoctorNo, @AagaIsas, @ValidFrom, @ValidTill, @StoreId)";

        public static string UpdateRecipe => @"SELECT update_recipe(@DbCode, @tlkid, @barcodeid, @recser, @recipeno, @perscode, @clinicid, @deseasecode, @doctorid, @recipedate, @salesprice, @basicprice, @compensationid, @qty, @totalsum, @compsum, @paysum, @salesdate, @gqty, @water, @taxolaborum, @ext, @checkdate, @checkno, @qtyday, @countday, @tilldate, @kvpdoctorno, @aaga_isas, @valid_from, @valid_till, @store_id)";

        public static string UpdateErecipe => "SELECT update_erecipe(@Id, @CompositionId, @CompositionRef, @CompositionStatus, @MedicationDispenseId, @MedicationDispenseStatus, @Active, @Confirmed, @DocumentStatus, @Info);";

        public static string GetCompensationByCode => "SELECT Id, compcode AS Code, comppercent AS Percent, Name FROM compensation WHERE compcode = @code";
    }
}
