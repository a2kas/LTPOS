using POS_display.Items;
using POS_display.SR_InsuranceIF;
using POS_display.Utils.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.ServiceModel;
using System.Threading.Tasks;

namespace POS_display.Utils.Insurance
{
    public class IF : InsuranceBase
    {
        private readonly HWSSoapClient scInsurance;
        private readonly Guid HSP_GUID;
        private readonly string HSP_UNIT = "BVL";
        private const string InsuranceCompanyCode = "IFF";

        public IF(Items.Insurance insuranceItem)
        {
            var company = Session.Develop == true ? "TEST" : Session.SystemData.ecode;
            HSP_GUID = Session.getParam("IF_ID", company).ToGuid();
            HSP_UNIT = Session.getParam("IF_UNIT", company);

            scInsurance = new HWSSoapClient(Session.Develop == true ? "IF_TEST" : "IF");

            var user = Session.getParam("IF_USERNAME", company);
            var pass = Session.getParam("IF_PASSWORD", company);

            scInsurance.ClientCredentials.UserName.UserName = user;
            scInsurance.ClientCredentials.UserName.Password = pass;
        }

        public override string AuthoriseInsuranceCard(Items.Insurance insuranceItem)
        {
            if (insuranceItem == null)
                throw new Exception("Neatpažinta draudimo kortelė!");

            var request = new Request
            {
                hsp_guid = HSP_GUID,
                unit = HSP_UNIT,
                card_number = getCardNo(insuranceItem.CardNoLong),
                personal_code = getPersonalDigits(insuranceItem.CardNoLong)
            };

            var response = scInsurance.Request(request);
            if (response.error > RequestError.Ok)
                throw new Exception(response.error + ": " + response.error_description_lt);

            return response.request_guid.ToString();
        }

        public override async Task<Items.Insurance> CalcInsuranceCompensation(Items.posh poshItem) 
        {
            Items.Insurance result = poshItem.InsuranceItem;
            try
            {
                DateTime serviceDate = DateTime.Now;
                List<Service> services = new List<Service>();

                var request = new Request
                {
                    hsp_guid = HSP_GUID,
                    unit = HSP_UNIT,
                    card_number = getCardNo(poshItem.InsuranceItem.CardNoLong),
                    personal_code = getPersonalDigits(poshItem.InsuranceItem.CardNoLong)
                };

                foreach (var posDetail in poshItem.PosdItems.Where(e => e.status_insurance != 13))
                {
                    var hasReceipt = posDetail.recipeid > 0 || posDetail.erecipe_no > 0 || posDetail.status_insurance == 12 ? true : false;
                    var code = GetInsuranceMapping(posDetail);
                    services.Add(new Service()
                    {
                        id = helpers.getIntID(posDetail.id),
                        code = hasReceipt ? $"{code}R" : code,
                        price = posDetail.pricediscounted,
                        count = (byte)posDetail.qty,
                        service_date = serviceDate
                    });
                }

                request.services = services.ToArray();
                var response = scInsurance.Request(request);
                if (response.error > RequestError.Ok)
                    throw new Exception(response.error + ": " + response.error_description_lt);

                var payableServices = response.services.Where(e => e.error == ServiceError.Paid_Fully || e.error == ServiceError.Paid_Partly).ToList();
                result.Receipt = payableServices;
                result.CardSessionId = response.request_guid.ToString();

                foreach (var el in payableServices)
                {
                    var hasReceipt = el.code.EndsWith("R") ? 1 : 0;
                    if (poshItem.PosdItems.Where(pd => pd.id == helpers.getDecimalID(el.id)).First().status_insurance == 0)
                        await DB.cheque.CreateChequeTrans(helpers.getDecimalID(el.id), el.payable_amount, result.Company, response.request_guid.ToString(), result.CardNoLong, null, hasReceipt == 1 ? 12 : 11, "");
                    else
                        await DB.cheque.UpdateChequeTrans(helpers.getDecimalID(el.id), el.payable_amount, result.Company, response.request_guid.ToString(), result.CardNoLong, hasReceipt == 1 ? 12 : 11, "");
                }
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
                await DB.cheque.ClearInsuranceData(poshItem.Id);
                result = null;
            }

            return result;
        }

        public override async Task<bool> CancelInsuranceCompensation(Items.posh poshItem)
        {
            bool success = false;
            try
            {
                var serviceCancelations = GetCancelableServicesBySessionId(poshItem.InsuranceItem.CardSessionId);

                var cancelationRequest = new Cancelation
                {
                    hsp_guid = HSP_GUID,
                    unit = HSP_UNIT,
                    services = serviceCancelations
                };

                var response = scInsurance.Cancel(cancelationRequest);

                if (response.error > CancelationError.Ok)
                    throw new Exception(response.error + ": " + response.error_description_lt);

                var errors = response.services
                 .Where(s => s.error > ServiceCancelationError.Ok)
                 .Select(s => $"{s.error} - {s.error_description_lt}")
                 .ToList();

                if (errors.Any())
                {
                    throw new InvalidOperationException($"Service errors occurred:\n{string.Join("\n", errors)}");
                }

                success = await DB.cheque.CancelChequeTrans(poshItem.Id);
            }
            catch (EndpointNotFoundException ex)
            {
                Serilogger.GetLogger().Error(ex, ex.Message);
                helpers.alert(Enumerator.alert.error, "Nepavyksta prisijungti prie " + poshItem.InsuranceItem.CompanyString + " serverio.\nDraudimo kortelių aptarnavimas laikinai negalimas. Bandykite vėliau");
            }
            catch (Exception ex)
            {
                Serilogger.GetLogger().Error(ex, ex.Message);
            }
            return success;
        }

        public override async Task<bool> VoidInsuranceCompensation(Items.posh poshItem)
        {
            bool success = false;
            try
            {
                var serviceCancelations = GetCancelableServicesBySessionId(poshItem.InsuranceItem.CardSessionId);

                var cancelationRequest = new Cancelation
                {
                    hsp_guid = HSP_GUID,
                    unit = HSP_UNIT,
                    services = serviceCancelations
                };

                var response = scInsurance.Cancel(cancelationRequest);

                if (response.error > CancelationError.Ok)
                    throw new Exception(response.error + ": " + response.error_description_lt);

                var errors = response.services
                 .Where(s => s.error > ServiceCancelationError.Ok)
                 .Select(s => $"{s.error} - {s.error_description_lt}")
                 .ToList();

                if (errors.Any())
                {
                    throw new InvalidOperationException($"Service errors occurred:\n{string.Join("\n", errors)}");
                }

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

        public override async Task<string> ConfirmInsuranceCompensation(Items.posh poshItem)
        {
            string cardSessionId;
            try
            {
                var serviceConfirmations = (poshItem.InsuranceItem.Receipt as List<ServiceAnswer>)
                    .Where(answer => answer.reserve_guid != null)
                    .Select(answer => new ServiceConfirmation { reserve_guid = answer.reserve_guid })
                    .ToArray();

                var confirmation = new Confirmation
                {
                    hsp_guid = HSP_GUID,
                    unit = HSP_UNIT,
                    services = serviceConfirmations
                };

                var response = scInsurance.Confirm(confirmation);
                if (response.error > ConfirmationError.Ok)
                    throw new Exception(response.error + ": " + response.error_description_lt);

                var errors = response.services
                     .Where(s => s.error > ServiceConfirmationError.Ok)
                     .Select(s => $"{s.error} - {s.error_description_lt}")
                     .ToList();

                if (errors.Any())
                {
                    throw new InvalidOperationException($"Service errors occurred:\n{string.Join("\n", errors)}");
                }

                cardSessionId = poshItem.InsuranceItem.CardSessionId;
                await DB.cheque.ConfirmChequeTrans(poshItem.Id, cardSessionId);
            }
            catch (EndpointNotFoundException ex)
            {
                Serilogger.GetLogger().Error(ex, ex.Message);
                cardSessionId = "";
                helpers.alert(Enumerator.alert.error, "Nepavyksta prisijungti prie " + poshItem.InsuranceItem.CompanyString + " serverio.\nDraudimo kortelių aptarnavimas laikinai negalimas. Bandykite vėliau");
            }
            catch (Exception ex)
            {
                Serilogger.GetLogger().Error(ex, ex.Message);
                cardSessionId = "";
                helpers.alert(Enumerator.alert.error, ex.Message);
            }
            return cardSessionId;
        }

        public override List<ComboBox<decimal>> GetCardBalance(Items.Insurance insuranceItem)
        {
            return new List<ComboBox<decimal>>();
        }

        public override string getCardNo(string cardNoLong)
        {
            return cardNoLong.Substring(0, cardNoLong.Length - 11);
        }

        public override string getPersonalDigits(string cardNoLong)
        {
            return cardNoLong.Substring(cardNoLong.Length - 11);
        }

        private string GetInsuranceMapping(Items.posd posDetail)
        {
            using (var scTamroWS = new SR_TamroWS.TamroWSSoapClient())
            {
                var atc = scTamroWS.GetInsuranceMaping(InsuranceCompanyCode, posDetail.productid, posDetail.barcodeid);
                return atc.Length >= 3 ? atc.Substring(0,3) : string.Empty;
            }
        }

        private ServiceCancelation[] GetCancelableServicesBySessionId(string sessionId) 
        {
            var serviceCancelations = new List<ServiceCancelation>();

            try
            {
                var response = scInsurance.RequestInformation(new RequestInfo
                {
                    hsp_guid = HSP_GUID,
                    unit = HSP_UNIT,
                    request_guid = Guid.Parse(sessionId)
                });

                foreach (var service in response.services) 
                {
                    if (service.service_status != ReserveStatus.ActiveCancelable)
                        continue;

                    serviceCancelations.Add(new ServiceCancelation
                    {
                        reserve_guid = service.reserve_guid,
                        confirmation_guid = service.confirmation_guid
                    });
                }

                return serviceCancelations.ToArray();
            }
            catch (Exception ex)
            {
                return serviceCancelations.ToArray();
            }
        }
    }
}
