using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS_display.wpf.ViewModel.recipe
{
    public class RecipeEdit : BaseViewModel
    {
        public RecipeEdit()
        {
            model = new Model.recipe.RecipeEdit();
            using (var l = RecipeEdit_Loaded())
                l.Wait();
        }

        public RecipeEdit(Model.recipe.RecipeEdit _model)
        {
            model = _model;
            using (var l = RecipeEdit_Loaded())
                l.Wait();
        }

        public async Task RecipeEdit_Loaded()
        { 
            await ExecuteWithWaitAsync(async () =>
            {
                if (!Session.User.postname.StartsWith("Vaist") && !Session.User.postname.StartsWith("Farma") && !Session.User.postname.StartsWith("Ved") && !Session.User.postname.StartsWith("Vad"))
                    throw new Exception(Session.User.postname + " negali parduoti kompensuojamų receptų!");
                await DB.POS.UpdateSession("Receptai", 2);
                //if (model.RecipeId == 0)//new recipe
                //{
                //    model.StoreId = Session.SystemData.storeid;
                //    model.StoreName = Session.SystemData.storename;
                //    if (model.PosdId > 0)//from POS
                //    {
                //        var temp = await DB.recipe.getTLKPrice(model.ProductId, "100", DateTime.Now);
                //        if (temp?.Rows?.Count > 0)
                //        {
                //            maxmkaina = temp.Rows[0]["mmkaina"].ToDecimal();
                //            maxbkaina = temp.Rows[0]["mbkaina"].ToDecimal();
                //        }
                //        var t = DB.recipe.asyncNewRecipeData(model.PosdId, CheckDate.Date, model.Barcode, "100");
                //        if (t?.Rows?.Count > 0)
                //        {
                //            DateTime current_date = DateTime.Now;
                //            if (in_erecipe != null)//e receptas
                //            {
                //                if (in_erecipe.eRecipe_PrescriptionTagsLongTag && in_erecipe.DispenseCount > 0)//jei gyd. tęsti ir išdavimas ne pirmas
                //                    Ext = 1;
                //                model.RecipeNo = in_erecipe.eRecipe_RecipeNumber;
                //                model.KVPDoctorNo = "E";
                //                model.CompCode = in_erecipe.eRecipe_CompensationCode;
                //                model.RecipeDate = in_erecipe.eRecipe_DateWritten;
                //                if (model.Ext == 0)
                //                {
                //                    model.ValidFrom = in_erecipe.eRecipe_ValidFrom;
                //                    if (in_erecipe.eRecipe_PrescriptionTagsLongTag)//jei gydymui testi pirmas isdavimas
                //                        model.ValidTill = model.ValidFrom.AddDays(29);
                //                    else
                //                        model.ValidTill = in_erecipe.eRecipe_ValidTo;
                //                }
                //                else
                //                {
                //                    int counter = 1;
                //                    model.ValidFrom = in_erecipe.PastValidTo.AddDays(-4);
                //                    model.ValidTill = model.ValidFrom.AddDays(model.RecipeValidityPeriod);
                //                    while (current_date > model.ValidTill && model.ValidTill < in_erecipe.eRecipe_ValidTo)
                //                    {
                //                        model.ValidFrom = ValidFrom.AddDays(counter * in_erecipe.ValidationPeriod);
                //                        model.ValidTill = ValidFrom.AddDays(model.RecipeValidityPeriod);
                //                        counter++;
                //                    }
                //                    if (model.ValidTill > in_erecipe.eRecipe_ValidTo)
                //                        model.ValidTill = in_erecipe.eRecipe_ValidTo;
                //                    model.PastTillDate = in_erecipe.PastValidTo;
                //                }
                //                model.QtyDay = in_erecipe.eRecipe_DosePerDayQuantityValue;
                //                model.DeseaseCode = in_erecipe.eRecipe.ReasonCode;
                //                model.AAGA_ISAS = in_erecipe.eRecipe.AagaSgasNumber;
                //            }
                //            else
                //            {
                //                model.RecipeDate = current_date;
                //                model.ValidFrom = current_date;
                //                model.ValidTill = model.ValidFrom.AddDays(model.RecipeValidityPeriod);
                //            }
                //            model.SalesDate = current_date;
                //            model.CheckDate = current_date;
                //            model.RecipeValid = helpers.betweenday(model.ValidFrom.Date, model.ValidTill.Date) + 1;
                //            model.BasicPrice = t.Rows[0]["basicprice"].ToDecimal();
                //            model.SalesPrice = t.Rows[0]["newsalesprice"].ToDecimal();
                //            hd_compensation[0] = 0;
                //            hd_compensation[50] = t.Rows[0]["c50"].ToDecimal();
                //            hd_compensation[80] = t.Rows[0]["c80"].ToDecimal();
                //            hd_compensation[90] = t.Rows[0]["c90"].ToDecimal();
                //            hd_compensation[100] = t.Rows[0]["c100"].ToDecimal();
                //            tlkId = t.Rows[0]["code2"].ToString();
                //            hd_qty2 = t.Rows[0]["retailpr"].ToDecimal();
                //            model.Doses = Math.Round(hd_qty2 * Qty);
                //            hd_barratio = t.Rows[0]["barratio"].ToDecimal();
                //            hd_prodratio = t.Rows[0]["prodratio"].ToDecimal();
                //            if (hd_posdPriceDiscounted == 0 || (maxmkaina < hd_posdPriceDiscounted && maxmkaina != 0))
                //                model.SalesPrice = maxmkaina;
                //            else
                //                model.SalesPrice = hd_posdPriceDiscounted;
                //            if (hd_prodratio > 0)
                //                GQty = Qty * hd_barratio / hd_prodratio;
                //            await tbCompCode_Change();
                //            ChangedValuesEvent("NewRecipeData_cb", new EventArgs());
                //        }
                //        else
                //            throw new Exception("prekė yra nekompensuojama, tolimesni veiksmai negalimi!");
                //    }
                //    else//not from POS
                //    {
                //        //TODO
                //    }
                //}
            });
        }

        public Model.recipe.RecipeEdit model { get; set; }
    }
}
