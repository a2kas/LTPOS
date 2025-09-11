using POS_display.Items;
using POS_display.Models.TransactionService;
using POS_display.Utils.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI;

namespace POS_display.Utils.Insurance
{
    public class EPS : InsuranceBase
    {
        public SR_InsuranceEPS.HealthInsuranceECRCompensationServiceSoapClient scInsurance;

        public EPS(Items.Insurance insuranceItem)
        {
            scInsurance = new SR_InsuranceEPS.HealthInsuranceECRCompensationServiceSoapClient(insuranceItem.Company + (Session.Develop == true ? "_TEST" : ""));
        }

        public override string AuthoriseInsuranceCard(Items.Insurance InsuranceItem)
        {
            var result = ExecuteServiceRequest(() =>
            {
                string card_session_id = "";
                if (InsuranceItem == null)
                    throw new Exception("Neatpažinta draudimo kortelė!");
                int errCode = 0;
                string errMsg = "";
                card_session_id = scInsurance.POSAuthorizeCard(InsuranceItem.PartnerId, Session.SystemData.kas_client_id.ToString(), helpers.MakeHash(InsuranceItem.CardNoLong), out errCode, out errMsg);
                if (errCode > 0)
                    throw new Exception(errCode + ": " + errMsg.Substring(errMsg.IndexOf("message:") + 8));
                return card_session_id;
            });

            return result.Invoke();
        }

        public override async Task<Items.Insurance> CalcInsuranceCompensation(Items.posh PoshItem)
        {
            Items.Insurance result = PoshItem.InsuranceItem;
            try
            {
                int errCode;
                string errMsg;
                SR_InsuranceEPS.POSpurchaseReceipt currentReceipt = new SR_InsuranceEPS.POSpurchaseReceipt();

                string code = result.Company;
                int i = 1;

                List<SR_InsuranceEPS.POSpurchasedService> purchasedServices = new List<SR_InsuranceEPS.POSpurchasedService>();
                foreach (var posDetail in PoshItem.PosdItems.Where(e=>e.status_insurance != 13)) 
                {
                    var atc = await GetInsuranceMapping(code, posDetail);
                    purchasedServices.Add(new SR_InsuranceEPS.POSpurchasedService()
                    {
                        recId = helpers.getIntID(posDetail.id),
                        lCode = posDetail.productid.ToString(),
                        lName = posDetail.barcodename,
                        lPrice = posDetail.price,
                        lVATRate = posDetail.vatsize,
                        lItemQuantity = posDetail.qty,
                        lTotal = posDetail.sum + posDetail.cheque_sum,
                        lWasStateCompensation = posDetail.recipeid == 0 ? (byte)0 : (byte)1,
                        lStateCmpsnValue = posDetail.compensationsum,
                        ATCCode = atc,
                        lHasReceipt = posDetail.recipeid > 0 || posDetail.erecipe_no > 0 || posDetail.status_insurance == 12 ? (byte)1 : (byte)0
                    });
                }
                currentReceipt.purchasedServices = purchasedServices.ToArray();
                errCode = scInsurance.POSCalculateCompensation(result.PartnerId, Session.SystemData.kas_client_id.ToString(), result.CardSessionId, ref currentReceipt, out errMsg);
                result.Receipt = currentReceipt;
                foreach (var el in currentReceipt.purchasedServices)
                {
                    if (PoshItem.PosdItems.Where(pd => pd.id == helpers.getDecimalID(el.recId)).First().status_insurance == 0)
                        await DB.cheque.CreateChequeTrans(helpers.getDecimalID(el.recId), el.xCompensatedValue, result.Company, result.CardSessionId, result.CardNoLong, null, el.lHasReceipt == 1 ? 12 : 11, el.xLimitName);
                    else
                        await DB.cheque.UpdateChequeTrans(helpers.getDecimalID(el.recId), el.xCompensatedValue, result.Company, result.CardSessionId, result.CardNoLong, el.lHasReceipt == 1 ? 12 : 11, el.xLimitName);
                }
                if (errCode > 0)
                    throw new Exception(errCode + ": " + errMsg.Substring(errMsg.IndexOf("message:") + 8));

                Serilogger.GetLogger("ltpos_data").Information($"[CalcInsuranceCompensation] - Insurance company: {code}," +
                    $" Sent purchased services: {purchasedServices.ToJsonString()}, " +
                    $" Got purchased services: {currentReceipt?.purchasedServices?.ToJsonString()}, " +
                    $" POS Details: {PoshItem.BuildLogs()}");
            }
            catch (EndpointNotFoundException ex)
            {
                Serilogger.GetLogger().Error(ex, ex.Message);
                helpers.alert(Enumerator.alert.error, "Nepavyksta prisijungti prie " + result.CompanyString + " serverio.\nDraudimo kortelių aptarnavimas laikinai negalimas. Bandykite vėliau");
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
            string cardSessionId;
            try
            {
                int errCode = 0;
                string errMsg = "";
                cardSessionId = scInsurance.POSConfirmCompensation(PoshItem.InsuranceItem.PartnerId, Session.SystemData.kas_client_id.ToString(), PoshItem.InsuranceItem.CardSessionId, (SR_InsuranceEPS.POSpurchaseReceipt)PoshItem.InsuranceItem.Receipt, out errCode, out errMsg);
                if (errCode > 0)
                    throw new Exception(errCode + ": " + errMsg.Substring(errMsg.IndexOf("message:") + 8));
                await DB.cheque.ConfirmChequeTrans(PoshItem.Id, cardSessionId);
            }
            catch (EndpointNotFoundException ex)
            {
                Serilogger.GetLogger().Error(ex, ex.Message);
                cardSessionId = "";
                helpers.alert(Enumerator.alert.error, "Nepavyksta prisijungti prie " + PoshItem.InsuranceItem.CompanyString + " serverio.\nDraudimo kortelių aptarnavimas laikinai negalimas. Bandykite vėliau");
            }
            catch (Exception ex)
            {
                Serilogger.GetLogger().Error(ex, ex.Message);
                cardSessionId = "";
                helpers.alert(Enumerator.alert.error, ex.Message);
            }
            return cardSessionId;
        }

        public override async Task<bool> CancelInsuranceCompensation(Items.posh PoshItem)
        {
            bool success = false;
            try
            {
                int errCode = 0;
                string errMsg = "";
                errCode = scInsurance.POSCancelCompensation(PoshItem.InsuranceItem.PartnerId, Session.SystemData.kas_client_id.ToString(), PoshItem.InsuranceItem.CardSessionId, out errMsg);
                if (errCode > 0)
                    throw new Exception(errCode + ": " + errMsg.Substring(errMsg.IndexOf("message:") + 8));
                success = await DB.cheque.CancelChequeTrans(PoshItem.Id);
            }
            catch (EndpointNotFoundException ex)
            {
                Serilogger.GetLogger().Error(ex, ex.Message);
                helpers.alert(Enumerator.alert.error, "Nepavyksta prisijungti prie " + PoshItem.InsuranceItem.CompanyString + " serverio.\nDraudimo kortelių aptarnavimas laikinai negalimas. Bandykite vėliau");
            }
            catch (Exception ex)
            {
                Serilogger.GetLogger().Error(ex, ex.Message);
                //helpers.alert(Enumerator.alert.error, ex.Message);
            }
            return success;
        }

        public override async Task<bool> VoidInsuranceCompensation(Items.posh poshItem)
        {
            bool success = false;
            try
            {
                int errCode = 0;
                string errMsg = "";
                scInsurance = new SR_InsuranceEPS.HealthInsuranceECRCompensationServiceSoapClient(poshItem.InsuranceItem.Company + (Session.Develop == true ? "_TEST" : ""));
                errCode = scInsurance.POSVoidCompensation(poshItem.InsuranceItem.GetSettings(poshItem.InsuranceItem.Company, Session.SystemData.ecode, "ID"), Session.SystemData.kas_client_id.ToString(), "", poshItem.InsuranceItem.CardSessionId, out errMsg);
                if (errCode > 0)
                    throw new Exception(errCode + ": " + errMsg.Substring(errMsg.IndexOf("message:") + 8));
                success = await DB.cheque.VoidChequeTrans(poshItem.Id);
            }
            catch (EndpointNotFoundException ex)
            {
                Serilogger.GetLogger().Error(ex, ex.Message);
                helpers.alert(Enumerator.alert.error, "Nepavyksta prisijungti prie draudimo serverio.\nDraudimo kortelių aptarnavimas laikinai negalimas. Bandykite vėliau");
            }
            catch (Exception ex)
            {
                Serilogger.GetLogger().Error(ex, ex.Message);
                helpers.alert(Enumerator.alert.error, ex.Message);
            }
            return success;
        }

        public override List<Items.ComboBox<decimal>> GetCardBalance(Items.Insurance insuranceItem)
        {
            List<Items.ComboBox<decimal>> result = new List<Items.ComboBox<decimal>>();
            try
            {
                if (insuranceItem == null)
                    throw new Exception("");
                int errCode;
                string errMsg;
                result = (from el in
                          scInsurance.POSGetCardBalance(insuranceItem.PartnerId, Session.SystemData.kas_client_id.ToString(), helpers.MakeHash(insuranceItem.CardNoLong), out errCode, out errMsg)
                          select new Items.ComboBox<decimal>()
                          {
                              DisplayMember = el.xLimitName,
                              ValueMember = el.xLimitBalance
                          }).ToList();
                if (errCode > 0)
                    throw new Exception(errCode + ": " + errMsg.Substring(errMsg.IndexOf("message:") + 8));
            }
            catch (EndpointNotFoundException ex)
            {
                Serilogger.GetLogger().Error(ex, ex.Message);
                helpers.alert(Enumerator.alert.error, "Nepavyksta prisijungti prie " + insuranceItem.CompanyString + " serverio.\nDraudimo kortelių aptarnavimas laikinai negalimas. Bandykite vėliau");
            }
            catch (Exception ex)
            {
                Serilogger.GetLogger().Error(ex, ex.Message);
                helpers.alert(Enumerator.alert.error, ex.Message);
            }

            return result;
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
