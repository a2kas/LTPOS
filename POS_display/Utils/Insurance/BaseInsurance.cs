using POS_display.Models.TransactionService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Threading.Tasks;

namespace POS_display.Utils.Insurance
{
    public abstract class InsuranceBase
    {
        public abstract string AuthoriseInsuranceCard(Items.Insurance insuranceItem);
        public abstract Task<Items.Insurance> CalcInsuranceCompensation(Items.posh poshItem);
        public abstract Task<string> ConfirmInsuranceCompensation(Items.posh poshItem);
        public abstract Task<bool> CancelInsuranceCompensation(Items.posh poshItem);
        public abstract Task<bool> VoidInsuranceCompensation(Items.posh poshItem);
        public abstract List<Items.ComboBox<decimal>> GetCardBalance(Items.Insurance insuranceItem);
        public abstract string getCardNo(string cardNoLong);
        public abstract string getPersonalDigits(string cardNoLong);

        public Func<T> TryCatchWrapper<T>(Func<T> functionToExcute) where T : class
        {
            Func<T> tryBlockWrapper = () =>
            {
                try
                {
                    return functionToExcute();
                }
                catch (EndpointNotFoundException ex)
                {
                    helpers.alert(Enumerator.alert.error, "Nepavyksta prisijungti prie draudimo serverio.\nDraudimo kortelių aptarnavimas laikinai negalimas. Bandykite vėliau");
                    return null;
                }
                catch (Exception ex)
                {
                    helpers.alert(Enumerator.alert.error, ex.Message);
                    return null;
                }
            };

            return tryBlockWrapper;
        }

        public Func<T> ExecuteServiceRequest<T>(Func<T> functionToExcute) where T : class
        {
            return TryCatchWrapper(() =>
            {
                return functionToExcute();
            });
        }

        protected async Task<string> GetInsuranceMapping(string companyCode, Items.posd posDetail)
        {
            if (companyCode == "IFF")
            {
                using (var scTamroWS = new SR_TamroWS.TamroWSSoapClient())
                {
                    return scTamroWS.GetInsuranceMaping(companyCode, posDetail.productid, posDetail.barcodeid);
                }
            }
            else
            {
                var results = await Session.TamroGateway.GetAsync<List<BalticCategoryTreeID>>($"/api/v1/balticcategorytree?Level4Id={posDetail.baltic_category_id}");
                var balticCategoryTreeID = results.FirstOrDefault();
                if (balticCategoryTreeID != null)
                {
                    List<string> args = new List<string> { "BENU", "K", "MP", "VP", "X01", "Ž", "-" };
                    if ((balticCategoryTreeID.Level1Id == 1 || balticCategoryTreeID.Level1Id == 2) &&
                        !args.Any(arg => posDetail.atc.StartsWith(arg, StringComparison.InvariantCultureIgnoreCase)))
                        return posDetail.atc;
                    else
                        return posDetail.baltic_category_id.ToString();
                }
                return string.Empty;
            }
        }
    }
}
