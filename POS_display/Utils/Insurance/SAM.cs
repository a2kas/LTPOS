using POS_display.Repository.Recipe;
using POS_display.Utils.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using static POS_display.Enumerator;

namespace POS_display.Utils.Insurance
{
    public class SAM : InsuranceBase
    {
        private readonly RecipeRepository _recipeRepository;

        public SAM(Items.Insurance insuranceItem)
        {
            insuranceItem.PartnerId = "";
            _recipeRepository = new RecipeRepository();
        }

        public override string AuthoriseInsuranceCard(Items.Insurance InsuranceItem)
        {
            return string.Empty;
        }

        public override async Task<Items.Insurance> CalcInsuranceCompensation(Items.posh PoshItem)
        {
            Items.Insurance result = PoshItem.InsuranceItem;
            Serilogger.GetLogger("ltpos_data").Information($"[CalcInsuranceCompensation]: {PoshItem.BuildLogs()}");
            try
            {
                decimal totalCompensated = 0;
                foreach (var posDetail in PoshItem.PosdItems)
                {
                    decimal compensatedSum = 0;
                    bool hasRecipe = posDetail.recipeid > 0 || posDetail.erecipe_no > 0 || posDetail.status_insurance == 12;
                    
                    var supportedDisease = await _recipeRepository.ExistDiseaseCodeInRefugeeDiseaseCodes(posDetail.erecipe_no > 0 ? 
                        posDetail.eRecipeDiseaseCode :
                        posDetail.RecipeDiseaseCode);

                    if ((posDetail.gr4.StartsWith("rx", StringComparison.InvariantCultureIgnoreCase) ||
                        posDetail.Flags.HasFlag(ProductFlag.UA)) && hasRecipe && supportedDisease)
                    {
                        if (posDetail.erecipe_no > 0)
                        {
                            // if recipe is as erecipe then it must be "nekompensuojamas"
                            compensatedSum = posDetail.eRecipeStatus == "nekompensuojamas" ? posDetail.sum : 0;
                        }
                        else
                            compensatedSum = posDetail.compcode == "4" ? posDetail.sum : 0;
                    }

                    if (posDetail.status_insurance == 0)
                        await DB.cheque.CreateChequeTrans(posDetail.id, compensatedSum, result.Company, result.CardSessionId, result.CardNoLong, null, hasRecipe ? 12 : 11, "");
                    else
                        await DB.cheque.UpdateChequeTrans(posDetail.id, compensatedSum, result.Company, result.CardSessionId, result.CardNoLong, hasRecipe ? 12 : 11, "");
                    totalCompensated += compensatedSum;
                }

                if (totalCompensated > 0)
                    result.CardSessionId = totalCompensated.ToString();
            }
            catch (Exception ex)
            {
                Serilogger.GetLogger().Error(ex, ex.Message);
                helpers.alert(Enumerator.alert.error, ex.Message);
                await DB.cheque.ClearInsuranceData(PoshItem.Id);
                result = null;
            }

            return result;
        }

        public override async Task<string> ConfirmInsuranceCompensation(Items.posh PoshItem)
        {
            await DB.cheque.ConfirmChequeTrans(PoshItem.Id, PoshItem.InsuranceItem.CardSessionId);
            return PoshItem.InsuranceItem.CardSessionId;
        }

        public override async Task<bool> CancelInsuranceCompensation(Items.posh PoshItem)
        {
            return await DB.cheque.CancelChequeTrans(PoshItem.Id);
        }

        public override async Task<bool> VoidInsuranceCompensation(Items.posh poshItem)
        {
            return await DB.cheque.VoidChequeTrans(poshItem.Id);
        }

        public override List<Items.ComboBox<decimal>> GetCardBalance(Items.Insurance insuranceItem)
        {
            return new List<Items.ComboBox<decimal>>();
        }

        public override string getCardNo(string cardNoLong)
        {
            return cardNoLong.Substring(0, cardNoLong.Length - 4);
        }

        public override string getPersonalDigits(string cardNoLong)
        {
            return cardNoLong.Substring(cardNoLong.Length - 4);
        }
    }
}
