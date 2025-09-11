using POS_display.Models.Recipe;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace POS_display.Repository.Recipe
{
    public interface IRecipeRepository
    {
        Task<List<TlkRestriction>> GetPriceRestrictions(string tlkId);

        Task<long> CreateVaccination(long posh_id,
                                     long posd_id,
                                     long order_id,
                                     long prescription_composition_id,
                                     string prescription_composition_ref,
                                     long dispensation_composition_id,
                                     string dispensation_composition_ref,
                                     long patient_id);

        Task<string> GetNpakId7(decimal productid);

        Task SetAdditionalInstructions(long id, string additionalinstructions);

        Task SetERecipeDiseaseCode(long id, string diseaseCode);

        Task<bool> HasUnsignedUkraineRefugeeRecipes(long userid);

        Task<bool> ExistDiseaseCodeInRefugeeDiseaseCodes(string diseasecode);

        Task<List<VaccineDays>> GetVaccineDays();

        DateTime? GetMedicationCompensationDateByNpakid7(string npakid7);

        Task SetAccumulatedSurchargeData(long id, bool surchargeEligible, decimal surchargeAmount, decimal missingSurchargeAmount, DateTime validto);

        Task UpdateRecipeCompensationData(decimal recipeId, decimal prepaymentCompensation, bool isFirstPrescription, bool hasLowIncome, bool isPrepaymentCompensation);

        Task<NewRecipeData> GetNewRecipeData(decimal posDetailId, DateTime checkDate, string barcode, string compPercent);

        Task<NotCompensatedRecipeData> GetNotCompensatedRecipeData(decimal id);

        Task<decimal> CreateNotCompensatedRecipe(decimal posHeaderId, decimal posDetailId, DateTime validFrom, decimal doses, decimal qtyDay, decimal countDay, DateTime tillDate);

        Task DeleteNotCompensatedRecipe(decimal posHeaderId, decimal posDetailId);

        Task<List<decimal>> GetRecipeNoByCompositionIds(List<decimal> compositionIds);

        Task SetPartialDispenseGroupId(decimal recipeid, string group_id);

        Task<string> GetPartialDispenseGroupIdByPosHeaderId(decimal posHeaderId);

        Task<RecipeEditModel> GetRecipeEditDataByCompositionId(decimal compositionId);

        Task UpdateRecipeByEditData(RecipeEditModel updateModel);

        Task<bool> UpdatePosDetailByRecipe(PosDetailUpdateByRecipeModel dataModel);

        Task<long> CreateErecipe(CreateErecipeModel dataModel);

        Task<long> CreateRecipe(CreateRecipeModel dataModel);

        Task UpdateRecipe(CreateRecipeModel dataModel);

        Task UpdateERecipe(UpdateErecipeModel dataModel);

        Task<CompensationModel> GetCompensationByCode(string code);
    }
}
