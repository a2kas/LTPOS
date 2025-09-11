using CommunityToolkit.HighPerformance.Buffers;
using Elasticsearch.Net;
using Hl7.Fhir.Rest;
using iTextSharp.text.xml.xmp;
using Microsoft.AspNetCore.Mvc;
using POS_display.SR_InsuranceERGO;
using POS_display.Utils.Logging;
using POS_display.wpf.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Threading.Tasks;

namespace POS_display.Utils.Insurance
{
    public class HDIS : InsuranceBase
    {
        private readonly Items.Insurance _insuranceItem;
        private string EndpointConfigurationName => _insuranceItem.Company + (Session.Develop ? "_TEST" : "");

        public HDIS(Items.Insurance insuranceItem)
        {
            _insuranceItem = insuranceItem;
        }

        public override string AuthoriseInsuranceCard(Items.Insurance InsuranceItem)
        {
            string card_session_id = "";
            try
            {
                if (InsuranceItem == null)
                    throw new Exception("Neatpažinta draudimo kortelė!");

                authentificateUserResponse authentificateUserResponse = new authentificateUserResponse();
                string ecodeUser = InsuranceItem.GetSettings(InsuranceItem.Company, Session.SystemData.ecode, "USER");
                string ecodePass = InsuranceItem.GetSettings(InsuranceItem.Company, Session.SystemData.ecode, "PASS");
                using (var client = new SR_InsuranceERGO.HDisWebServiceClient(EndpointConfigurationName))
                {
                    var authentificateUser = new authentificateUser
                    {
                        username = ecodeUser,
                        password = ecodePass,
                        countryCode = "LT"
                    };
                    authentificateUserResponse = client.authentificateUser(authentificateUser);
                    card_session_id = authentificateUserResponse.@return;
                }
                if (authentificateUserResponse.status < 0)
                    throw new Exception(authentificateUserResponse.status + ": " + authentificateUserResponse.statusText);
            }
            catch (EndpointNotFoundException ex)
            {
                Serilogger.GetLogger("ltpos_data").Error(ex, ex.Message);
                helpers.alert(Enumerator.alert.error, "Nepavyksta prisijungti prie " + InsuranceItem.CompanyString + " draudimo serverio.\nDraudimo kortelių aptarnavimas laikinai negalimas. Bandykite vėliau");
            }
            catch (Exception ex)
            {
                Serilogger.GetLogger("ltpos_data").Error(ex, ex.Message);
                helpers.alert(Enumerator.alert.error, ex.Message);
            }

            return card_session_id;
        }

        public override async Task<Items.Insurance> CalcInsuranceCompensation(Items.posh PoshItem)
        {
            Items.Insurance result = PoshItem.InsuranceItem;
            try
            {
                string code = result.Company;
                int i = 1;
                var filteredItems = PoshItem.PosdItems.Where(pd => pd.status_insurance != 13).ToList();
                var HMS = new List<SR_InsuranceERGO.healthMedService>();
                int rank = 0;
                foreach (var pd in filteredItems)
                {
                    var serviceCode = await GetInsuranceMapping(code, pd);

                    HMS.Add(new SR_InsuranceERGO.healthMedService()
                    {
                        serviceCode = serviceCode,
                        serviceCountSpecified = true,
                        serviceCount = (int)(
                            (Math.Round(pd.qty * pd.price, 2) == pd.sum 
                            && Math.Ceiling(pd.qty) - pd.qty == 0)
                            ? pd.qty
                            : 1
                        ),
                        sumSpecified = true,
                        sum = (double)(
                            (Math.Round(pd.qty * pd.price, 2) == pd.sum
                            && Math.Ceiling(pd.qty) - pd.qty == 0)
                            ? pd.pricediscounted + Math.Round(pd.cheque_sum / pd.qty, 2)
                            : pd.sum + pd.cheque_sum
                        ),
                        hasRecSpecified = true,
                        hasRec = pd.recipeid > 0 || pd.erecipe_no > 0 || pd.status_insurance == 12,
                        requestNo = pd.id.ToString(),
                        medName = pd.barcodename,
                        compByGovSpecified = true,
                        compByGov = pd.recipeid != 0
                    });

                    rank++;
                }
                SR_InsuranceERGO.healthMedService[] hmsArray = HMS.ToArray();
                Serilogger.GetLogger("ltpos_data").Information($"[HDIS][CalcInsuranceCompensation] Request: {HMS.ToJsonString()}");
                var res = new SR_InsuranceERGO.validateClaimsMedListResponse();
                using (var client = new SR_InsuranceERGO.HDisWebServiceClient(EndpointConfigurationName))
                {
                    var validateClaimsMedList = new validateClaimsMedList
                    {
                        sessionKey = result.CardSessionId,
                        cardNumber = getCardNo(result.CardNoLong),
                        insPersCode = getPersonalDigits(result.CardNoLong),
                        servicesInList = hmsArray,
                        medInstCode = Session.SystemData.ecode.ToString(),
                        eventDate = DateTime.Now.ToString(),
                        countryCode = "LT",
                        language = "LT",
                        pharmCode = Session.SystemData.kas_client_id.ToString()

                    };
                    res = client.validateClaimsMedList(validateClaimsMedList);

                    if (res.@return.status < 0)
                        throw new Exception(res.@return.status + ": " + res.@return.statusText);
                    result.Receipt = hmsArray;

                    SR_InsuranceERGO.servicesOutListParams[] emptyList = (from el in HMS
                                                                          select new SR_InsuranceERGO.servicesOutListParams()
                                                                          {
                                                                              requestNo = el.requestNo
                                                                          }).ToArray();
                    var joined = HMS.Join(res.@return.servicesOutList?.DefaultIfEmpty() ?? emptyList, left => left.requestNo, right => right.requestNo, (left, right) => new { HMS = left, res = right }).ToArray();
                    foreach (var el in joined)
                    {
                        if (!result.CardNoLong.Contains(string.IsNullOrEmpty(res.@return.cardNumber) ? string.Empty : res.@return.cardNumber))
                            result.CardNoLong = $"{res.@return.cardNumber}{result.CardNoLong}";
                        if (PoshItem.PosdItems.Where(pd => pd.id == el.HMS.requestNo.ToDecimal()).First().status_insurance == 0)
                            await DB.cheque.CreateChequeTrans(el.HMS.requestNo.ToDecimal(), (decimal)el.res.paymSum, result.Company, result.CardSessionId, result.CardNoLong, null, el.HMS.hasRec ? 12 : 11, "");
                        else
                            await DB.cheque.UpdateChequeTrans(el.HMS.requestNo.ToDecimal(), (decimal)el.res.paymSum, result.Company, result.CardSessionId, result.CardNoLong, el.HMS.hasRec ? 12 : 11, "");
                    }

                    Serilogger.GetLogger("ltpos_data").Information($"[HDIS][CalcInsuranceCompensation] Response: {res.ToJsonString()}");
                }
            }
            catch (EndpointNotFoundException ex)
            {
                Serilogger.GetLogger("ltpos_data").Error(ex, ex.Message);
                helpers.alert(Enumerator.alert.error, "Nepavyksta prisijungti prie " + result.CompanyString + " serverio.\nDraudimo kortelių aptarnavimas laikinai negalimas. Bandykite vėliau");
            }
            catch (Exception ex)
            {
                Serilogger.GetLogger("ltpos_data").Error(ex, ex.Message);
                helpers.alert(Enumerator.alert.error, ex.Message);
                await DB.cheque.ClearInsuranceData(PoshItem.Id);
                result = null;
            }

            return result;
        }

        public override async Task<string> ConfirmInsuranceCompensation(Items.posh PoshItem)
        {
            string cardSessionId = "";
            try
            {
                createClaimsMedListResponse res = new SR_InsuranceERGO.createClaimsMedListResponse();
                using (var client = new SR_InsuranceERGO.HDisWebServiceClient(EndpointConfigurationName))
                {
                    createClaimsMedList createClaimsMedList = new createClaimsMedList()
                    {
                        sessionKey = PoshItem.InsuranceItem.CardSessionId,
                        cardNumber = getCardNo(PoshItem.InsuranceItem.CardNoLong),
                        insPersCode = getPersonalDigits(PoshItem.InsuranceItem.CardNoLong),
                        servicesInList = (SR_InsuranceERGO.healthMedService[])PoshItem.InsuranceItem.Receipt,
                        medInstCode = Session.SystemData.ecode.ToString(),
                        eventDate = DateTime.Now.ToString(),
                        countryCode = "LT",
                        language = "LT",
                        pharmCode = Session.SystemData.kas_client_id.ToString()
                    };
                    
                    res = client.createClaimsMedList(createClaimsMedList);
                    Serilogger.GetLogger("ltpos_data").Information($"[HDIS][ConfirmInsuranceCompensation] Response: {res.ToJsonString()}");
                    if (res.@return.status < 0)
                        throw new Exception(res.@return.status + ": " + res.@return.statusText);
                    if (Math.Abs(PoshItem.ChequeInsuranceSum) != Math.Round((res.@return.servicesOutList?.Sum(s => s.paymSum).ToDecimal() ?? 0), 2))
                        throw new Exception("Draudimo kompensuojama suma nesutampa su apskaičiuota suma. Patikrinkite draudimo kompensuojamas prekes!");
                    foreach (var el in res.@return.servicesOutList?.ToList())
                        await DB.cheque.UpdateChequeTrans(el.requestNo.ToDecimal(), (decimal)el.paymSum, PoshItem.InsuranceItem.Company, PoshItem.InsuranceItem.CardSessionId + "/" + el.hidtID.ToString(), PoshItem.InsuranceItem.CardNoLong, 10, "");//reference find "/"
                    cardSessionId = PoshItem.InsuranceItem.CardSessionId;
                }
            }
            catch (EndpointNotFoundException ex)
            {
                Serilogger.GetLogger("ltpos_data").Error(ex, ex.Message);
                helpers.alert(Enumerator.alert.error, "Nepavyksta prisijungti prie " + PoshItem.InsuranceItem.CompanyString + " serverio.\nDraudimo kortelių aptarnavimas laikinai negalimas. Bandykite vėliau");
            }
            catch (Exception ex)
            {
                Serilogger.GetLogger("ltpos_data").Error(ex, ex.Message);
                helpers.alert(Enumerator.alert.error, ex.Message);
            }
            return cardSessionId;
        }

        public override async Task<bool> CancelInsuranceCompensation(Items.posh PoshItem)
        {
            return await DB.cheque.CancelChequeTrans(PoshItem.Id);
        }

        public override async Task<bool> VoidInsuranceCompensation(Items.posh poshItem)
        {
            bool success = false;
            try
            {
                var cardSessionIdSplitted = poshItem.InsuranceItem.CardSessionId.Split('/');//card_session_id / detId
                var ids = (from el in poshItem.PosdItems.Where(w => w.status_insurance == 10)
                           select new SR_InsuranceERGO.healthServiceDetId()
                           {
                               detId = el.InsuranceInfo.Split('/')?[1] ?? string.Empty,
                               requestNo = el.id.ToString()
                           }).Where(x => x.detId !=string.Empty).ToArray();
                cancelClaimsMedListResponse res = new SR_InsuranceERGO.cancelClaimsMedListResponse();
                using (var client = new SR_InsuranceERGO.HDisWebServiceClient(EndpointConfigurationName))
                {
                    cancelClaimsMedList cancelClaimsMedList = new cancelClaimsMedList
                    {
                        pharmCode = Session.SystemData.kas_client_id.ToString(),
                        detalizationIdsList = ids,
                        countryCode = "LT",
                        language = "LT",
                        sessionKey = cardSessionIdSplitted[0]
                    };
                    res = client.cancelClaimsMedList(cancelClaimsMedList);
                    Serilogger.GetLogger("ltpos_data").Information($"[HDIS][VoidInsuranceCompensation] Response: {res.ToJsonString()}");
                    if (res.@return.status < 0)
                        throw new Exception(res.@return.status + ": " + res.@return.statusText);
                    success = await DB.cheque.VoidChequeTrans(poshItem.Id);
                }
            }
            catch (EndpointNotFoundException ex)
            {
                Serilogger.GetLogger("ltpos_data").Error(ex, ex.Message);
                helpers.alert(Enumerator.alert.error, "Nepavyksta prisijungti prie draudimo serverio.\nDraudimo kortelių aptarnavimas laikinai negalimas. Bandykite vėliau");
            }
            catch (Exception ex)
            {
                Serilogger.GetLogger("ltpos_data").Error(ex, ex.Message);
                helpers.alert(Enumerator.alert.error, ex.Message);
            }
            return success;
        }

        public override List<Items.ComboBox<decimal>> GetCardBalance(Items.Insurance insuranceItem)
        {
            return new List<Items.ComboBox<decimal>>();
        }

        public override string getCardNo(string cardNoLong)
        {
            return cardNoLong.Substring(0, cardNoLong.Length - 11);
        }

        public override string getPersonalDigits(string cardNoLong)
        {
            return cardNoLong.Substring(cardNoLong.Length - 11);
        }
    }
}
