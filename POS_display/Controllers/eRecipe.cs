using Newtonsoft.Json;
using POS_display.Models.Recipe;
using POS_display.Repository.Pos;
using POS_display.Utils.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;
using static POS_display.Enumerator;

namespace POS_display.Controllers
{
    internal static class eRecipe
    {
        internal static async Task<bool> PerformGroupDispenseSaving()
        {
            try
            {
                var results = await Session.eRecipeUtils.CreateRecipeDispenseMultiple(Session.GruopDispenseRequests);
                foreach (var result in results) 
                {
                    await DB.eRecipe.UpdateErecipeAsync(
                            result.ExternalId.ToDecimal(),
                            result.CompositionId.ToDecimal(),
                            result.CompositionRef,
                            "final",
                            result.MedicationDispenseId.ToDecimal(),
                            "completed",
                            1,
                            Session.PractitionerItem.Roles.First().Code == "6" || Session.PractitionerItem.Roles.First().Code == "7" ? 1 : 0,
                            "final",
                            string.Empty);
                }                                
                return true;
            }
            catch (Exception ex)
            {
                helpers.alert(Enumerator.alert.error, ex.Message);
                Serilogger.GetLogger().Error(ex, ex.Message);
                return false;
            }
        }

        internal static async Task<decimal> CreateGroupDipsenseEntry(Items.eRecipe.Recipe eRecipeItem, string PickedUpByRef, decimal posh_id, decimal posd_id, decimal productId, decimal recipe_id, DateTime TillDate, DateTime RecipeDate, DateTime SalesDate, decimal TotalSum, decimal PaySum, decimal CompSum, decimal GQty, decimal prepCompSum, int durationOfUse)
        {
            decimal erecipeId = await DB.eRecipe.CreateErecipeAsync(posh_id,
                                                posd_id,
                                                productId,
                                                Session.User.id,
                                                eRecipeItem.eRecipe_RecipeNumber,
                                                eRecipeItem.Encounter.EncounterId.ToDecimal(),
                                                recipe_id,
                                                RecipeDate,
                                                SalesDate,
                                                TillDate);

            if (erecipeId <= 0)
                throw new Exception("Nepavyksta išsaugoti elektroninio recepto duomenų bazėje!");

            Session.GruopDispenseRequests.Add(new GroupDispenseRequest()
            {
                eRecipeItem = eRecipeItem.DeepClone(),
                PickedUpByRef = PickedUpByRef,
                PoshId = posh_id,
                PosdId = posd_id,
                ProductId = productId,
                RecipeId = recipe_id,
                TillDate = TillDate,
                RecipeDate = RecipeDate,
                SalesDate = SalesDate,
                TotalSum = TotalSum,
                PaySum = PaySum,
                CompSum = CompSum,
                GQty = GQty,
                PrepCompSum = prepCompSum,
                DurationOfUse = durationOfUse,
                ConfirmDispense = Session.PractitionerItem.Roles.First().Code == "6" || Session.PractitionerItem.Roles.First().Code == "7" ? true : false,
                eRecipeId = erecipeId
            });

            await new PosRepository().UpdatePosMemo(Session.Devices.debtorid, POSMemoParamter.CreateMultipeDispense, JsonConvert.SerializeObject(Session.GruopDispenseRequests));
            return erecipeId;
        }

        internal static async Task<decimal> CreateERecipe(Items.eRecipe.Recipe eRecipeItem, string PickedUpByRef, decimal posh_id, decimal posd_id, decimal productId, decimal recipe_id, DateTime TillDate, DateTime RecipeDate, DateTime SalesDate, decimal TotalSum, decimal PaySum, decimal CompSum, decimal GQty, decimal prepCompSum, int durationOfUse)
        {
            decimal erecipeId;
            try
            {
                erecipeId = await DB.eRecipe.CreateErecipeAsync(posh_id,
                                                                posd_id,
                                                                productId,
                                                                Session.User.id,
                                                                eRecipeItem.eRecipe_RecipeNumber,
                                                                eRecipeItem.Encounter.EncounterId.ToDecimal(),
                                                                recipe_id,
                                                                RecipeDate,
                                                                SalesDate,
                                                                TillDate);

                if (erecipeId <= 0)
                    throw new Exception("Nepavyksta išsaugoti elektroninio recepto duomenų bazėje!");
                var confirmDispense = Session.PractitionerItem.Roles.First().Code == "6" || Session.PractitionerItem.Roles.First().Code == "7" ? true : false;
                bool isCheapest = Session.getParam("TLK", "CHEAPEST_OLD") == "1" ? 
                    await new PosRepository().IsCheapest(eRecipeItem?.Medication?.NPAKID7.ToString()) :
                    Session.TLKCheapests.Where(val => val.StartDate <= DateTime.Now && val.Npakid7 == eRecipeItem?.Medication?.NPAKID7.ToString())
                    .OrderByDescending(val => val.PriceListVersion)
                    .Select(val => val.IsCheapest)
                    .FirstOrDefault();

                var RecipeDispense = await Session.eRecipeUtils.CreateRecipeDispense(eRecipeItem,
                                                                                     PickedUpByRef,
                                                                                     TillDate,
                                                                                     TotalSum,
                                                                                     PaySum,
                                                                                     CompSum,
                                                                                     prepCompSum,
                                                                                     GQty,
                                                                                     DateTime.Now,
                                                                                     confirmDispense,
                                                                                     isCheapest,
                                                                                     durationOfUse);

                if (RecipeDispense?.CompositionId?.ToDecimal() > 0)
                {
                    var updated = await DB.eRecipe.UpdateErecipeAsync(
                        erecipeId, 
                        RecipeDispense.CompositionId.ToDecimal(),
                        RecipeDispense.CompositionRef,
                        "final", //todo
                        RecipeDispense.MedicationDispenseId.ToDecimal(),
                        "completed", //todo
                        1,
                        confirmDispense ? 1 : 0,
                        "final",
                        ""
                        );

                    if (!updated)
                        throw new Exception("Nepavyksta atnaujinti erecepto duomenų bazėje");
                }
                else
                    throw new Exception("Negautas elektroninio recepto išdavimo id. Mėginkite dar kartą.");
            }
            catch (Exception ex)
            {
                erecipeId = -1;
                helpers.alert(Enumerator.alert.error, ex.Message);
                if (eRecipeItem.RecipeDispense != null)
                {
                    await Session.eRecipeUtils.CancelRecipeDispense(eRecipeItem.RecipeDispense.CompositionId, "Panaikintas kvitas.", null);
                }
                await DB.eRecipe.MarkErecipeAsync(posd_id, eRecipeItem.eRecipe_RecipeNumber, recipe_id, ex.Message);
                Serilogger.GetLogger().Error(ex, ex.Message);
            }

            return erecipeId;
        }
    }
}
