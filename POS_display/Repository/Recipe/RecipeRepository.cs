using Dapper;
using POS_display.Models.Recipe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace POS_display.Repository.Recipe
{
    public class RecipeRepository : BaseRepository, IRecipeRepository
    {
        private DB_Base DbBase => new DB_Base();

        private Dictionary<string, DateTime> _medidcationCompensations = new Dictionary<string, DateTime>() 
        {
           /* { "1061810", new DateTime(2022,07,05) },
            { "1031774", new DateTime(2022,07,05) },
            { "1076929", new DateTime(2022,07,05) },
            { "1050601", new DateTime(2021,04,01) },
            { "1076377", new DateTime(2021,04,01) },
            { "1027998", new DateTime(2021,04,01) },
            { "1028004", new DateTime(2021,04,01) },
            { "1005666", new DateTime(2021,04,01) },
            { "1000778", new DateTime(2021,04,01) },
            { "1000776", new DateTime(2021,04,01) },
            { "1000779", new DateTime(2021,04,01) },
            { "1000777", new DateTime(2021,07,01) },
            { "1091526", new DateTime(2021,04,01) },
            { "1007116", new DateTime(2023,01,02) },
            { "1089034", new DateTime(2023,01,02) },
            { "1089035", new DateTime(2023,01,02) },
            { "1004632", new DateTime(2022,07,01) },
            { "1002344", new DateTime(2022,07,01) },
            { "1003706", new DateTime(2022,01,01) },
            { "1003271", new DateTime(2021,04,01) },
            { "1088444", new DateTime(2021,04,01) },
            { "1003272", new DateTime(2021,04,01) },
            { "1082311", new DateTime(2021,04,01) },
            { "1081746", new DateTime(2021,04,01) },
            { "1011033", new DateTime(2022,07,01) },
            { "1030073", new DateTime(2022,07,01) },
            { "1080180", new DateTime(2021,04,01) },
            { "1093898", new DateTime(2021,04,01) },
            { "1033465", new DateTime(2023,01,02) },
            { "1033458", new DateTime(2023,01,02) },
            { "1061941", new DateTime(2023,01,02) },
            { "1003150", new DateTime(2021,04,01) },
            { "1067101", new DateTime(2022,07,01) },
            { "1073277", new DateTime(2022,07,01) },
            { "1086335", new DateTime(2021,04,01) },
            { "1087896", new DateTime(2021,04,01) },
            { "1027935", new DateTime(2021,04,01) }*/
        };


        public async Task<List<TlkRestriction>> GetPriceRestrictions(string tlkId)
        {
            List<TlkRestriction> priceRestrictions;
            using (var connection = DbBase.GetConnection())
            {
                var result = await connection.QueryAsync<TlkRestriction>(RecipeQueries.GetPriceRestrictions, new { tlkId });
                priceRestrictions = result.ToList();
            }

            return priceRestrictions;
        }

        public async Task<long> CreateVaccination(long posh_id,
                                                  long posd_id,
                                                  long order_id,
                                                  long prescription_composition_id,
                                                  string prescription_composition_ref,
                                                  long dispensation_composition_id,
                                                  string dispensation_composition_ref,
                                                  long patient_id)
        {
            using (var connection = DbBase.GetConnection())
            {
                return await connection.ExecuteAsync(RecipeQueries.CreateVaccination,
                    new
                    {
                        posh_id,
                        posd_id,
                        order_id,
                        prescription_composition_id,
                        prescription_composition_ref,
                        dispensation_composition_id,
                        dispensation_composition_ref,
                        patient_id
                    });
            }
        }

        public async Task<string> GetNpakId7(decimal productid) 
        {
            using (var connection = DbBase.GetConnection())
            {
                return await connection.QueryFirstAsync<string>(RecipeQueries.GetNpakId7, new {productid = productid});
            }
        }

        public async Task SetAdditionalInstructions(long id, string additionalinstructions)
        {
            using (var connection = DbBase.GetConnection())
            {
                await connection.ExecuteAsync(RecipeQueries.SetAdditionalInstructions,
                    new
                    {
                        id,
                        additionalinstructions
                    });
            }
        }

        public async Task SetERecipeDiseaseCode(long id, string diseaseCode)
        {
            using (var connection = DbBase.GetConnection())
            {
                await connection.ExecuteAsync(RecipeQueries.SetERecipeDiseaseCode,
                    new
                    {
                        id,
                        disease_code = diseaseCode
                    });
            }
        }

        public async Task<bool> HasUnsignedUkraineRefugeeRecipes(long userid)
        {
            using (var connection = DB_Base.GetConnection())
            {
                return await connection.QueryFirstOrDefaultAsync<bool>(RecipeQueries.CheckUnsignedUkraineRefugeeRecipes, new { userid });
            }
        }

        public async Task<bool> ExistDiseaseCodeInRefugeeDiseaseCodes(string diseasecode)
        {
            using (var connection = DB_Base.GetConnection())
            {
                return await connection.QueryFirstAsync<bool>(RecipeQueries.ExistDiseaseCodeInRefugeeDiseaseCodes, new { diseasecode });
            }
        }

        public async Task<List<VaccineDays>> GetVaccineDays()
        {
            using (var connection = DB_Base.GetConnection())
            {
                var list = await connection.QueryAsync<VaccineDays>(RecipeQueries.GetVaccineDays);
                return list.ToList();
            }
        }

        public DateTime? GetMedicationCompensationDateByNpakid7(string npakid7)
        {
            if (!string.IsNullOrWhiteSpace(npakid7) && _medidcationCompensations.ContainsKey(npakid7)) 
            {
                return _medidcationCompensations[npakid7];
            }
            return null;
        }

        public async Task SetAccumulatedSurchargeData(long id, bool surchargeEligible, decimal surchargeAmount, decimal missingSurchargeAmount, DateTime validto)
        {
            using (var connection = DbBase.GetConnection())
            {
                await connection.ExecuteAsync(RecipeQueries.SetAccumulatedSurchargeData,
                    new
                    {
                        id,
                        surcharge_eligible = surchargeEligible ? 1 : 0,
                        surcharge_amount = surchargeAmount,
                        missing_surcharge_amount = missingSurchargeAmount,
                        surcharge_valid_to = validto == DateTime.MinValue ? (object)DBNull.Value : validto
                    });
            }
        }

        public async Task UpdateRecipeCompensationData(decimal recipeId, decimal prepaymentCompensation, bool isFirstPrescription, bool hasLowIncome, bool isPrepaymentCompensation)
        {
            using (var connection = DbBase.GetConnection())
            {
                await connection.ExecuteAsync(RecipeQueries.UpdateRecipeCompensationData,
                    new
                    {
                        recipeId,
                        prepayment_compensation = prepaymentCompensation,
                        is_first_prescription  = isFirstPrescription ? 1 : 0,
                        is_prepayment_compensation = isPrepaymentCompensation ? 1 : 0,
                        low_income_tag = hasLowIncome ? 1 : 0
                    });
            }
        }

		public async Task<NewRecipeData> GetNewRecipeData(decimal posDetailId, DateTime checkDate, string barcode, string compPercent)
		{
			using (var connection = DbBase.GetConnection())
			{
				return await connection.QueryFirstOrDefaultAsync<NewRecipeData>(posDetailId == 0 ? 
                    RecipeQueries.NewRecipeDataWithoutPosDetail :
                    RecipeQueries.NewRecipeDataWithPosDetail,
					new
					{
						barcode,
						checkDate,
						compPercent
					});
			}
		}

		public async Task<NotCompensatedRecipeData> GetNotCompensatedRecipeData(decimal id)
		{
			using (var connection = DbBase.GetConnection())
			{
				return await connection.QueryFirstOrDefaultAsync<NotCompensatedRecipeData>(RecipeQueries.GetNotCompensatedRecipdById, new { id });
			}
		}

		public async Task<decimal> CreateNotCompensatedRecipe(decimal posHeaderId, decimal posDetailId, DateTime validFrom, decimal doses, decimal qtyDay, decimal countDay, DateTime tillDate)
		{
			using (var connection = DbBase.GetConnection())
			{
				return await connection.QueryFirstAsync<decimal>(RecipeQueries.CreateNotCompensatedRecipe, 
                    new 
                    {
						db_code = Session.SystemData.code,
                        posh_id = posHeaderId,
                        posd_id = posDetailId,
                        valid_from = validFrom.ToString("yyyy.MM.dd"),
                        doses, 
                        qty_day = qtyDay,
                        count_day = countDay,
                        till_date = tillDate.ToString("yyyy.MM.dd")
					});
			}
		}

		public async Task DeleteNotCompensatedRecipe(decimal posHeaderId, decimal posDetailId)
		{
			using (var connection = DbBase.GetConnection())
			{
				await connection.ExecuteAsync(RecipeQueries.DeleteNotCompensatedRecipe,
					new
					{
						posh_id = posHeaderId,
						posd_id = posDetailId
					});
			}
		}

        public async Task<List<decimal>> GetRecipeNoByCompositionIds(List<decimal> compositionIds)
        {
            using (var connection = DbBase.GetConnection())
            {
                string args = string.Join(", ", compositionIds.Select(n => $"{n}"));
                return (await connection.QueryAsync<decimal>(string.Format(RecipeQueries.GetRecipeNoByCompositionIds, args))).ToList();
            }
        }

        public async Task SetPartialDispenseGroupId(decimal recipeid, string group_id)
        {
            using (var connection = DB_Base.GetConnection())
            {
                await connection.ExecuteAsync(RecipeQueries.SetPartialDispeneGroupId, new { recipeid, group_id });
            }
        }

        public async Task<string> GetPartialDispenseGroupIdByPosHeaderId(decimal posHeaderId)
        {
            using (var connection = DB_Base.GetConnection())
            {
                return await connection.QueryFirstAsync<string>(RecipeQueries.GetPartialDispeneGroupIdByHeaderId, new { posHeaderId });
            }
        }

        public async Task<RecipeEditModel> GetRecipeEditDataByCompositionId(decimal compositionId)
        {
            using (var connection = DB_Base.GetConnection())
            {
                return await connection.QueryFirstOrDefaultAsync<RecipeEditModel>(RecipeQueries.GetRecipeEditDataByCompositionId, new { compositionId });
            }
        }

        public async Task UpdateRecipeByEditData(RecipeEditModel updateModel)
        {
            using (var connection = DB_Base.GetConnection())
            {
                await connection.ExecuteAsync(RecipeQueries.UpdateRecipeByEditData,
                    new
                    {
                        updateModel.ErecipeId,
                        updateModel.RecipeId,
                        countday = updateModel.CountDay,
                        till_date = updateModel.TillDate,
                        totalsum = updateModel.TotalSum,
                        compensationsum = updateModel.CompensationSum,
                        gqty = updateModel.GQty,
                        paysum = updateModel.PaySum,
                        prepayment_compensation = updateModel.PrepaymentCompensationSum
                    }
                );
                await connection.ExecuteAsync(RecipeQueries.UpdateERecipeByEditData,
                    new
                    {
                        updateModel.ErecipeId,
                        till_date = updateModel.TillDate
                    }
                );
            }
        }

        public async Task<bool> UpdatePosDetailByRecipe(PosDetailUpdateByRecipeModel dataModel)
        {
            using (var connection = DbBase.GetConnection())
            {
                return await connection.QueryFirstAsync<bool>(RecipeQueries.UpdatePosDetailByRecipe, new 
                { 
                    dataModel.PosdId,
                    dataModel.BarcodeId,
                    dataModel.Qty,
                    dataModel.NewPaySum,
                    dataModel.NewHId,
                    dataModel.PaySum,
                });
            }
        }

        public async Task<long> CreateErecipe(CreateErecipeModel dataModel)
        {
            using (var connection = DbBase.GetConnection())
            {
                return await connection.QueryFirstAsync<long>(RecipeQueries.CreateErecipe, new 
                {
                    dataModel.PoshId,
                    dataModel.PosdId,
                    dataModel.ProductId,
                    dataModel.UserId,
                    dataModel.RecipeNo,
                    dataModel.EncounterId,
                    dataModel.RecipeId,
                    dataModel.RecipeDate,
                    dataModel.SalesDate,
                    dataModel.TillDate
                });
            }
        }

        public async Task<long> CreateRecipe(CreateRecipeModel dataModel)
        {
            using (var connection = DbBase.GetConnection())
            {
                return await connection.QueryFirstAsync<long>(RecipeQueries.CreateRecipe, new 
                {
                    dataModel.DbCode,
                    dataModel.TlkId,
                    dataModel.BarcodeId,
                    dataModel.RecSer,
                    dataModel.RecipeNo,
                    dataModel.PersCode,
                    dataModel.ClinicId,
                    dataModel.DeseaseCode,
                    dataModel.DoctorId,
                    dataModel.RecipeDate,
                    dataModel.SalesPrice,
                    dataModel.BasicPrice,
                    dataModel.CompensationId,
                    dataModel.Qty,
                    dataModel.TotalSum,
                    dataModel.CompSum,
                    dataModel.PaySum,
                    dataModel.SalesDate,
                    dataModel.GQty,
                    dataModel.Water,
                    dataModel.TaxoLaborum,
                    dataModel.Ext,
                    dataModel.CheckDate,
                    dataModel.CheckNo,
                    dataModel.QtyDay,
                    dataModel.CountDay,
                    dataModel.TillDate,
                    dataModel.KvpDoctorNo,
                    dataModel.AagaIsas,
                    dataModel.ValidFrom,
                    dataModel.ValidTill,
                    dataModel.StoreId
                });
            }
        }

        public async Task UpdateRecipe(CreateRecipeModel dataModel)
        {
            using (var connection = DbBase.GetConnection())
            {
                await connection.ExecuteAsync(RecipeQueries.UpdateRecipe, new
                {
                    dataModel.DbCode,
                    dataModel.TlkId,
                    dataModel.BarcodeId,
                    dataModel.RecSer,
                    dataModel.RecipeNo,
                    dataModel.PersCode,
                    dataModel.ClinicId,
                    dataModel.DeseaseCode,
                    dataModel.DoctorId,
                    dataModel.RecipeDate,
                    dataModel.SalesPrice,
                    dataModel.BasicPrice,
                    dataModel.CompensationId,
                    dataModel.Qty,
                    dataModel.TotalSum,
                    dataModel.CompSum,
                    dataModel.PaySum,
                    dataModel.SalesDate,
                    dataModel.GQty,
                    dataModel.Water,
                    dataModel.TaxoLaborum,
                    dataModel.Ext,
                    dataModel.CheckDate,
                    dataModel.CheckNo,
                    dataModel.QtyDay,
                    dataModel.CountDay,
                    dataModel.TillDate,
                    dataModel.KvpDoctorNo,
                    dataModel.AagaIsas,
                    dataModel.ValidFrom,
                    dataModel.ValidTill,
                    dataModel.StoreId
                });
            }
        }

        public async Task UpdateERecipe(UpdateErecipeModel dataModel)
        {
            using (var connection = DbBase.GetConnection())
            {
                await connection.ExecuteAsync(RecipeQueries.UpdateErecipe, new 
                { 
                    dataModel.Id,
                    dataModel.CompositionId,
                    dataModel.CompositionRef,
                    dataModel.CompositionStatus,
                    dataModel.MedicationDispenseId,
                    dataModel.MedicationDispenseStatus,
                    dataModel.Active,
                    dataModel.Confirmed,
                    dataModel.DocumentStatus,
                    dataModel.Info
                });
            }
        }

        public async Task<CompensationModel> GetCompensationByCode(string code)
        {
            using (var connection = DbBase.GetConnection())
            {
                return await connection.QueryFirstAsync<CompensationModel>(RecipeQueries.GetCompensationByCode, new { code });
            }
        }
    }
}
