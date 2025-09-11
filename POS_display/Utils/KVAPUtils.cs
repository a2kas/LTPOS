using POS_display.Models.Kvap;
using POS_display.WR_KVAP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Services.Protocols;
using System.Xml;

namespace POS_display.Utils
{
    internal partial class KvapWebServiceOverriden : KvapWebService//override KvapWebService to add headers
    {
        protected override System.Net.WebRequest GetWebRequest(Uri uri)
        {
            System.Net.HttpWebRequest webRequest =
             (System.Net.HttpWebRequest)base.GetWebRequest(uri);
            webRequest.Headers.Add("SOAPAction", "");
            webRequest.Headers.Add("username", Session.Params.FirstOrDefault(p => p.system == "KVAP_USER" && p.par == Session.SystemData.ecode)?.value ?? "");
            webRequest.Headers.Add("password", Session.Params.FirstOrDefault(p => p.system == "KVAP_PASS" && p.par == Session.SystemData.ecode)?.value ?? "");
            webRequest.Accept = "text/xml";
            webRequest.Method = "POST";

            return webRequest;
        }
        protected override XmlWriter GetWriterForMessage(SoapClientMessage message, int bufferSize)
        {
            object xmlWriter = base.GetWriterForMessage(message, bufferSize);
            if (xmlWriter is XmlTextWriter)
                (xmlWriter as XmlTextWriter).Formatting = Formatting.Indented;
            return xmlWriter as XmlWriter;
        }
    }

    public interface IKVAP
    {
        VAISTININKAI GetPharmacists(string ecode, DateTime date_from, DateTime date_to);
        Task<Recipe> CheckIfRecipeUnused(string npakid, string personalCode, DateTime dateTime);
        Task<Recipe> CheckRecipe(string recipe_no, string gtpl, DateTime salesdate, DateTime recipedate);
        Task<Recipe> DeleteRecipe(string kvap_recipe_id);
        Task<Recipe> SendRecipe(submitKVRCreateKV_RECEPTAS submitKVRCreateModel);
        List<KV_RECEPTAIROW> GetPeriodRecipes(string ecode, string year, string month);
        SPSTPL_DETAILSROW GetSPSTPLDetails(string SPSTPL);
    }

    public class KVAPUtils : IKVAP
    {
        private readonly KvapWebServiceOverriden KVAP;

        public KVAPUtils(int timeout = -1)
        {
            KVAP = new KvapWebServiceOverriden();
            if (timeout > 0)
                KVAP.Timeout = timeout;
        }

        public SPSTPL_DETAILSROW GetSPSTPLDetails(string SPSTPL)
        {
            SOAP_REZULTATAS result = KVAP.getSPSTPLDetails(SPSTPL);

            if (result == null || result.UZKL_APD_REZ == null)
                throw new Exception("");
            if (result == null || result.UZKL_APD_REZ == null)
                throw new Exception("");
            var APDOROJIMO_KODAS = result.UZKL_APD_REZ.APDOROJIMO_KODAS.ToInt();
            if (APDOROJIMO_KODAS != 0)
                throw new Exception(result.UZKL_APD_REZ.APDOROJIMO_PRANESIMAS);

            if (!(result.REZULTATAI.Item is SPSTPL_DETAILS))
                return null;

            return (result.REZULTATAI.Item as SPSTPL_DETAILS)?.ROW?.FirstOrDefault();
        }

        public List<KV_RECEPTAIROW> GetPeriodRecipes(string ecode, string year, string month)
        {
            var data = new getKVRListKV_RECEPTAS()
            {
                PADALINYS_IST_ID = ecode,
                METAI = year,
                MENUO = month,
            };
           
            SOAP_REZULTATAS result = KVAP.getKVRList(new getKVRListKV_RECEPTAS[] { data });

            if (result == null || result.UZKL_APD_REZ == null)
                throw new Exception("");
            if (result == null || result.UZKL_APD_REZ == null)
                throw new Exception("");
            var APDOROJIMO_KODAS = result.UZKL_APD_REZ.APDOROJIMO_KODAS.ToInt();
            if (APDOROJIMO_KODAS != 0)
                throw new Exception(result.UZKL_APD_REZ.APDOROJIMO_PRANESIMAS);

            if (!(result.REZULTATAI.Item is KV_RECEPTAI))
                return null;

            return (result.REZULTATAI.Item as KV_RECEPTAI).ROW.ToList();
               
        }

        public VAISTININKAI GetPharmacists(string ecode, DateTime date_from, DateTime date_to)
        {
            var data = new getVaistininkasListGET_VAISTININKASLIST()
            {
                VAISTININKAS = new getVaistininkasListGET_VAISTININKASLISTVAISTININKAS()
                {
                    PADALINYS_IST_ID = ecode,
                    DATA_NUO = date_from,
                    DATA_IKI = date_to
                }
            };


            SOAP_REZULTATAS result = KVAP.getVaistininkasList(data);
            if(result == null || result.UZKL_APD_REZ == null)
                throw new Exception("");
            var APDOROJIMO_KODAS = result.UZKL_APD_REZ.APDOROJIMO_KODAS.ToInt();
            if (APDOROJIMO_KODAS != 0)
                throw new Exception(result.UZKL_APD_REZ.APDOROJIMO_PRANESIMAS);

            if (!(result.REZULTATAI.Item is VAISTININKAI))
                return null;

            return result.REZULTATAI.Item as VAISTININKAI;

        }

        public async Task<Recipe> CheckRecipe(string recipe_no, string gtpl, DateTime salesdate, DateTime recipedate)
        {
            return await Task.Factory.StartNew(() =>
            {
                var recipe = new submitKVRCheckKV_RECEPTAS[1]
                {
                    new submitKVRCheckKV_RECEPTAS()
                    {
                        NUMERIS = string.IsNullOrEmpty(recipe_no) ? "0" : recipe_no,
                        GTPL = string.IsNullOrEmpty(gtpl) ? "0" : gtpl,
                        VAISTO_ISDAVIMO_DATASpecified = true,
                        VAISTO_ISDAVIMO_DATA = salesdate,
                        RECEPTO_ISRASYMO_DATASpecified = true,
                        RECEPTO_ISRASYMO_DATA = recipedate
                    }
                };
                var errors = new List<RecipeError>();
                var submitKVRCheck = KVAP.submitKVRCheck(recipe);
                return GetResult(submitKVRCheck);
            });
        }

        public async Task<Recipe> CheckIfRecipeUnused(string npakid, string personalCode, DateTime dateTime)
        {
            return await Task.Factory.StartNew(() =>
            {
                var recipe = new submitKVRUpdateKV_RECEPTAS[1]
                {
                    new submitKVRUpdateKV_RECEPTAS()
                    {
                        ID = "1",//tikrinimo atveju visada 1
                        KVREC_TYPE = "T",//tikrinimo atveju "T"
                        VAISTO_ID = npakid,
                        VAISTO_ISDAVIMO_DATA = dateTime,
                        VAISTO_ISDAVIMO_DATASpecified = true,
                        ASMENS_KODAS = personalCode,
                        // Reikia nurodyti šias datas, kitaip grąžinama klaida iš serverio!
                        RECEPTO_ISRASYMO_DATA = DateTime.MaxValue,
                        GALIOJIMO_PRADZIA = DateTime.MaxValue,
                        GALIOJIMO_PABAIGA  = DateTime.MaxValue,
                        VAISTO_PAKANKA_IKI = DateTime.MaxValue
                    }
                };
                var errors = new List<RecipeError>();
                var submitKVRUpdate = KVAP.submitKVRUpdate(recipe);
                return GetResult(submitKVRUpdate);
            });
        }

        public async Task<Recipe> DeleteRecipe(string kvap_recipe_id)
        {
            return await Task.Factory.StartNew(() =>
            {
                var recipe = new submitKVRDeleteKV_RECEPTAS[1]
                {
                    new submitKVRDeleteKV_RECEPTAS()
                    {
                        ID = kvap_recipe_id
                    }
                };
                var errors = new List<RecipeError>();
                return GetResult(KVAP.submitKVRDelete(recipe));
            });
        }

        public async Task<Recipe> SendRecipe(submitKVRCreateKV_RECEPTAS submitKVRCreateModel)
        {
            return await Task.Factory.StartNew(() =>
            {
                submitKVRCreateModel.PADENG_PRIEMOKASpecified = submitKVRCreateModel.PADENG_PRIEMOKA > 0;
                var recipe = new submitKVRCreateKV_RECEPTAS[1]
                {
                    submitKVRCreateModel
                };
                var submitKVRCreate = KVAP.submitKVRCreate(recipe);
                return GetResult(submitKVRCreate);
            });
        }

        private Recipe GetResult(object input)
        {
            Recipe recipe = new Recipe();
            if (!(input is SOAP_REZULTATAS))
                return recipe;

            SOAP_REZULTATAS result = input as SOAP_REZULTATAS;
            if (result == null || result.UZKL_APD_REZ == null)
                throw new System.Exception("");
            var APDOROJIMO_KODAS = result.UZKL_APD_REZ.APDOROJIMO_KODAS.ToInt();
            if (APDOROJIMO_KODAS != 0)
                throw new System.Exception(result.UZKL_APD_REZ.APDOROJIMO_PRANESIMAS);

            if (result.REZULTATAI.Item is KV_RECEPTAI && (result.REZULTATAI.Item as KV_RECEPTAI).ROW != null)
            {
                foreach (KV_RECEPTAIROW row in (result.REZULTATAI.Item as KV_RECEPTAI).ROW)
                {
                    if (!string.IsNullOrEmpty(row.KLAIDOS_PRANESIMAS))
                        recipe.RecipeErrors.Add(new RecipeError { Notes = row.KLAIDOS_PRANESIMAS });

                    recipe.Id = row.RECEPTAS?.ID.ToDecimal() ?? 0;
                    recipe.Number = row.RECEPTAS?.NUMERIS ?? string.Empty;
                    recipe.Gtpl = row.RECEPTAS?.GTPL ?? string.Empty;
                    recipe.DrugId = row.RECEPTAS?.VAISTO_ID ?? string.Empty;
                    recipe.CompensationSum = row.RECEPTAS?.KOMPENSUOJAMA_SUMA ?? 0;
                    recipe.PrepaymentSum = row.RECEPTAS?.PACIENTO_PRIEMOKA ?? 0;
                    recipe.SaleSum = row.RECEPTAS?.PARDAVIMO_SUMA ?? 0;
                    recipe.Status = row.RECEPTAS?.STATUSAS ?? string.Empty;
                    recipe.PayedSum = row.RECEPTAS?.APMOKAMA_SUMA ?? 0;

                    if (row.KV_RECEPTO_KLAIDOS != null)
                    {
                        foreach (KV_RECEPTO_KLAIDOSROW kvRecipeErrorRow in row.KV_RECEPTO_KLAIDOS)
                        {
                            recipe.RecipeErrors.Add(new RecipeError
                            {
                                CriticalMean = kvRecipeErrorRow.KRITISKUMAS_MEAN,
                                Name = kvRecipeErrorRow.PAVADINIMAS,
                                Notes = kvRecipeErrorRow.PASTABOS
                            });
                        }
                    }
                }
            }

            return recipe;
        }
    }
}