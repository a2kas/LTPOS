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
using POS_display.Items;

namespace POS_display.Utils
{
    public abstract class InsuranceBase
    {
        public abstract string AuthoriseInsuranceCard(Items.Insurance InsuranceItem);
        public abstract Task<Items.Insurance> CalcInsuranceCompensation(Items.posh PoshItem);
        public abstract Task<string> ConfirmInsuranceCompensation(Items.posh PoshItem);
        public abstract Task<bool> CancelInsuranceCompensation(Items.posh PoshItem);
        public abstract Task<bool> VoidInsuranceCompensation(decimal posh_id, Items.Insurance insuranceItem);
        public abstract List<Items.ComboBox<decimal>> GetCardBalance(Items.Insurance insuranceItem);
        public abstract string getCardNo(string cardNoLong);
        public abstract string getPersonalDigits(string cardNoLong);
        public abstract void Close();

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
    }

    public class InsuranceEPS : InsuranceBase
    {
        public SR_InsuranceEPS.HealthInsuranceECRCompensationServiceSoapClient scInsurance;

        public InsuranceEPS(Items.Insurance insuranceItem)
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
                using (var scTamroWS = new SR_TamroWS.TamroWSSoapClient())
                {
                    currentReceipt.purchasedServices = (from pd in PoshItem.PosdItems
                                                        where pd.status_insurance != 13
                                                        let Rank = i++
                                                        select new SR_InsuranceEPS.POSpurchasedService()
                                                        {
                                                            recId = helpers.getIntID(pd.id),
                                                            lCode = pd.productid.ToString(),
                                                            lName = pd.barcodename,
                                                            lPrice = pd.price,
                                                            lVATRate = pd.vatsize,
                                                            lItemQuantity = pd.qty,
                                                            lTotal = pd.sum + pd.cheque_sum,
                                                            lWasStateCompensation = pd.recipeid == 0 ? (byte)0 : (byte)1,
                                                            lStateCmpsnValue = pd.compensationsum,
                                                            ATCCode = scTamroWS.GetInsuranceMaping(code, pd.productid, pd.barcodeid),
                                                            lHasReceipt = pd.recipeid > 0 || pd.erecipe_no > 0 || pd.status_insurance == 12 ? (byte)1 : (byte)0
                                                        }).ToArray();
                }
                errCode = scInsurance.POSCalculateCompensation(result.PartnerId, Session.SystemData.kas_client_id.ToString(), result.CardSessionId, ref currentReceipt, out errMsg);
                result.Receipt = currentReceipt;
                foreach (var el in currentReceipt.purchasedServices)
                {
                    if (PoshItem.PosdItems.Where(pd => pd.id == helpers.getDecimalID(el.recId)).First().status_insurance == 0)
                        await DB.cheque.CreateChequeTrans(helpers.getDecimalID(el.recId), el.xCompensatedValue, result.Company, result.CardSessionId, result.CardNoLong, null, el.lHasReceipt == 1 ? 12 : 11);
                    else
                        await DB.cheque.UpdateChequeTrans(helpers.getDecimalID(el.recId), el.xCompensatedValue, result.Company, result.CardSessionId, result.CardNoLong, el.lHasReceipt == 1 ? 12 : 11);
                }
                if (errCode > 0)
                    throw new Exception(errCode + ": " + errMsg.Substring(errMsg.IndexOf("message:") + 8));
            }
            catch (EndpointNotFoundException ex)
            {
                helpers.alert(Enumerator.alert.error, "Nepavyksta prisijungti prie " + result.CompanyString + " serverio.\nDraudimo kortelių aptarnavimas laikinai negalimas. Bandykite vėliau");
            }
            catch (Exception ex)
            {
                helpers.alert(Enumerator.alert.error, ex.Message);
                await DB.cheque.ClearInsuranceData(PoshItem.Id);
                result = null;
            }

            return result;
        }

        public override async Task<string> ConfirmInsuranceCompensation(Items.posh PoshItem)
        {
            string card_session_id = "";
            try
            {
                int errCode = 0;
                string errMsg = "";
                card_session_id = scInsurance.POSConfirmCompensation(PoshItem.InsuranceItem.PartnerId, Session.SystemData.kas_client_id.ToString(), PoshItem.InsuranceItem.CardSessionId, (SR_InsuranceEPS.POSpurchaseReceipt)PoshItem.InsuranceItem.Receipt, out errCode, out errMsg);
                if (errCode > 0)
                    throw new Exception(errCode + ": " + errMsg.Substring(errMsg.IndexOf("message:") + 8));
                await DB.cheque.ConfirmChequeTrans(PoshItem.Id, card_session_id);
            }
            catch (EndpointNotFoundException ex)
            {
                helpers.alert(Enumerator.alert.error, "Nepavyksta prisijungti prie " + PoshItem.InsuranceItem.CompanyString + " serverio.\nDraudimo kortelių aptarnavimas laikinai negalimas. Bandykite vėliau");
            }
            catch (Exception ex)
            {
                helpers.alert(Enumerator.alert.error, ex.Message);
            }
            return card_session_id;
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
                helpers.alert(Enumerator.alert.error, "Nepavyksta prisijungti prie " + PoshItem.InsuranceItem.CompanyString + " serverio.\nDraudimo kortelių aptarnavimas laikinai negalimas. Bandykite vėliau");
            }
            catch (Exception ex)
            {
                //helpers.alert(Enumerator.alert.error, ex.Message);
            }
            return success;
        }

        public override async Task<bool> VoidInsuranceCompensation(decimal posh_id, Items.Insurance insuranceItem)
        {
            bool success = false;
            try
            {
                int errCode = 0;
                string errMsg = "";
                scInsurance = new SR_InsuranceEPS.HealthInsuranceECRCompensationServiceSoapClient(insuranceItem.Company + (Session.Develop == true ? "_TEST" : ""));
                errCode = scInsurance.POSVoidCompensation(insuranceItem.GetSettings(insuranceItem.Company, Session.SystemData.ecode, "ID"), Session.SystemData.kas_client_id.ToString(), "", insuranceItem.CardSessionId, out errMsg);
                if (errCode > 0)
                    throw new Exception(errCode + ": " + errMsg.Substring(errMsg.IndexOf("message:") + 8));
                success = await DB.cheque.VoidChequeTrans(posh_id);
            }
            catch (EndpointNotFoundException ex)
            {
                helpers.alert(Enumerator.alert.error, "Nepavyksta prisijungti prie draudimo serverio.\nDraudimo kortelių aptarnavimas laikinai negalimas. Bandykite vėliau");
            }
            catch (Exception ex)
            {
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
                helpers.alert(Enumerator.alert.error, "Nepavyksta prisijungti prie " + insuranceItem.CompanyString + " serverio.\nDraudimo kortelių aptarnavimas laikinai negalimas. Bandykite vėliau");
            }
            catch (Exception ex)
            {
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

        public override void Close()
        {
            scInsurance.Close();
        }
    }

    public class InsuranceHDIS : InsuranceBase
    {
        public SR_InsuranceERGO.HDisWebServiceClient scInsurance;

        public InsuranceHDIS(Items.Insurance insuranceItem)
        {
            scInsurance = new SR_InsuranceERGO.HDisWebServiceClient(insuranceItem.Company + (Session.Develop == true ? "_TEST" : ""));
        }

        public override string AuthoriseInsuranceCard(Items.Insurance InsuranceItem)
        {
            string card_session_id = "";
            try
            {
                if (InsuranceItem == null)
                    throw new Exception("Neatpažinta draudimo kortelė!");
                int password_days = 0;
                int status = -1;
                string statusText = "";
                card_session_id = scInsurance.authentificateUser(InsuranceItem.GetSettings(InsuranceItem.Company, Session.SystemData.ecode, "USER"), InsuranceItem.GetSettings(InsuranceItem.Company, Session.SystemData.ecode, "PASS"), "LT", out password_days, out status, out statusText);
                if (status < 0)
                    throw new Exception(status + ": " + statusText);
            }
            catch (EndpointNotFoundException ex)
            {
                helpers.alert(Enumerator.alert.error, "Nepavyksta prisijungti prie " + InsuranceItem.CompanyString + " draudimo serverio.\nDraudimo kortelių aptarnavimas laikinai negalimas. Bandykite vėliau");
            }
            catch (Exception ex)
            {
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
                SR_InsuranceERGO.healthMedService[] HMS = (from pd in PoshItem.PosdItems
                                                           where pd.status_insurance != 13
                                                           let Rank = i++
                                                           select new SR_InsuranceERGO.healthMedService()
                                                           {
                                                               serviceCode = pd.baltic_category_id.ToString(),
                                                               serviceCountSpecified = true,
                                                               serviceCount = (int)(
                                                                   (Math.Round(pd.qty * pd.price, 2) == pd.sum  //if qty*price equal to sum
                                                                   && Math.Ceiling(pd.qty) - pd.qty == 0) //and qty is integer
                                                                   ? pd.qty //then send qty
                                                                   : 1 //else send 1
                                                                   ),
                                                               sumSpecified = true,
                                                               sum = (double)(
                                                                    (Math.Round(pd.qty * pd.price, 2) == pd.sum //if qty*price equal to sum
                                                                    && Math.Ceiling(pd.qty) - pd.qty == 0) //and qty is integer
                                                                    ? pd.pricediscounted + Math.Round(pd.cheque_sum / pd.qty, 2) //send price for single package
                                                                    : pd.sum + pd.cheque_sum //else send total sum
                                                                    ),
                                                               hasRecSpecified = true,
                                                               hasRec = pd.recipeid > 0 || pd.erecipe_no > 0 || pd.status_insurance == 12 ? true : false,
                                                               requestNo = pd.id.ToString(),
                                                               medName = pd.barcodename,
                                                               compByGovSpecified = true,
                                                               compByGov = pd.recipeid == 0 ? false : true,
                                                               receiptDateSpecified = true,
                                                               receiptDate = DateTime.Now
                                                           }).ToArray();
                var res = scInsurance.validateClaimsMedList(
                    result.CardSessionId,
                    Session.SystemData.ecode,
                    getPersonalDigits(result.CardNoLong),
                    getCardNo(result.CardNoLong),
                    DateTime.Now,
                    HMS,
                    "LT",
                    "LT",
                    Session.SystemData.kas_client_id.ToString());
                result.Receipt = HMS;
                SR_InsuranceERGO.servicesOutListParams[] emptyList = (from el in HMS
                                                                      select new SR_InsuranceERGO.servicesOutListParams()
                                                                      {
                                                                          requestNo = el.requestNo
                                                                      }).ToArray();
                var joined = HMS.Join(res.servicesOutList?.DefaultIfEmpty() ?? emptyList, left => left.requestNo, right => right.requestNo, (left, right) => new { HMS = left, res = right }).ToArray();
                foreach (var el in joined)
                {
                    var CardNo = result.CardNoLong;
                    if (!CardNo.Contains(res.cardNumber))
                        CardNo = $"{res.cardNumber}{CardNo}";
                    if (PoshItem.PosdItems.Where(pd => pd.id == el.HMS.requestNo.ToDecimal()).First().status_insurance == 0)
                        await DB.cheque.CreateChequeTrans(el.HMS.requestNo.ToDecimal(), (decimal)el.res.paymSum, result.Company, result.CardSessionId, CardNo, null, el.HMS.hasRec ? 12 : 11);
                    else
                        await DB.cheque.UpdateChequeTrans(el.HMS.requestNo.ToDecimal(), (decimal)el.res.paymSum, result.Company, result.CardSessionId, CardNo, el.HMS.hasRec ? 12 : 11);
                }
                if (res.status < 0)
                    throw new Exception(res.status + ": " + res.statusText);
                //if (res.servicesOutList == null || res.servicesOutList.Where(s => s.status == 1).Count() == 0)
                //    throw new Exception(res.explanation);
            }
            catch (EndpointNotFoundException ex)
            {
                helpers.alert(Enumerator.alert.error, "Nepavyksta prisijungti prie " + result.CompanyString + " serverio.\nDraudimo kortelių aptarnavimas laikinai negalimas. Bandykite vėliau");
            }
            catch (Exception ex)
            {
                helpers.alert(Enumerator.alert.error, ex.Message);
                await DB.cheque.ClearInsuranceData(PoshItem.Id);
                result = null;
            }

            return result;
        }

        public override async Task<string> ConfirmInsuranceCompensation(Items.posh PoshItem)
        {
            try
            {
                var res = scInsurance.createClaimsMedList(
                    PoshItem.InsuranceItem.CardSessionId,
                    Session.SystemData.ecode,
                    getPersonalDigits(PoshItem.InsuranceItem.CardNoLong),
                    getCardNo(PoshItem.InsuranceItem.CardNoLong),
                    DateTime.Now,
                    (SR_InsuranceERGO.healthMedService[])PoshItem.InsuranceItem.Receipt,
                    "LT",
                    "LT",
                    Session.SystemData.kas_client_id.ToString());
                if (res.status < 0)
                    throw new Exception(res.status + ": " + res.statusText);
                if (res?.servicesOutList?.Count() > 0)
                {
                    int count = 0;
                    foreach (var el in res.servicesOutList)
                        await DB.cheque.UpdateChequeTrans(el.requestNo.ToDecimal(), (decimal)el.paymSum, PoshItem.InsuranceItem.Company, PoshItem.InsuranceItem.CardSessionId + "/" + el.hidtID.ToString(), PoshItem.InsuranceItem.CardNoLong, 10);//reference find "/"
                }
            }
            catch (EndpointNotFoundException ex)
            {
                helpers.alert(Enumerator.alert.error, "Nepavyksta prisijungti prie " + PoshItem.InsuranceItem.CompanyString + " serverio.\nDraudimo kortelių aptarnavimas laikinai negalimas. Bandykite vėliau");
            }
            catch (Exception ex)
            {
                helpers.alert(Enumerator.alert.error, ex.Message);
            }
            return PoshItem.InsuranceItem.CardSessionId;
        }

        public override async Task<bool> CancelInsuranceCompensation(Items.posh PoshItem)
        {
            return await DB.cheque.CancelChequeTrans(PoshItem.Id);
        }

        public override async Task<bool> VoidInsuranceCompensation(decimal posh_id, Items.Insurance insuranceItem)
        {
            bool success = false;
            try
            {
                insuranceItem.CardSessionId = ((DataTable)insuranceItem.Receipt).Rows[0]["info"].ToString().Split('/')?[0];//reference find "/"
                var ids = (from el in ((DataTable)insuranceItem.Receipt).AsEnumerable()
                           select new SR_InsuranceERGO.healthServiceDetId()
                           {
                               detIdSpecified = true,
                               detId = Convert.ToInt64(el["info"].ToString().Split('/')?[1]),//card_session_id / detId
                               requestNo = el["posd_id"].ToString()
                           }).Where(x => x.detId > 0).ToArray();
                var res = scInsurance.cancelClaimsMedList(
                    insuranceItem.CardSessionId,
                    ids,
                    "LT",
                    "LT",
                    Session.SystemData.kas_client_id.ToString());
                if (res.status < 0)
                    throw new Exception(res.status + ": " + res.statusText);
                success = await DB.cheque.VoidChequeTrans(posh_id);
            }
            catch (EndpointNotFoundException ex)
            {
                helpers.alert(Enumerator.alert.error, "Nepavyksta prisijungti prie draudimo serverio.\nDraudimo kortelių aptarnavimas laikinai negalimas. Bandykite vėliau");
            }
            catch (Exception ex)
            {
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

        public override void Close()
        {
            scInsurance.Close();
        }
    }

    public class InsuranceOFFLINE : InsuranceBase
    {
        public InsuranceOFFLINE(Items.Insurance insuranceItem)
        {
            insuranceItem.PartnerId = "";
        }

        public override string AuthoriseInsuranceCard(Items.Insurance InsuranceItem)
        {
            return "";
        }

        public override async Task<Items.Insurance> CalcInsuranceCompensation(Items.posh PoshItem)
        {
            Items.Insurance result = PoshItem.InsuranceItem;
            try
            {
                DataTable dt_insurance = await DB.cheque.GetInsuranceData(PoshItem.Id);
                decimal insuranceSum = 0;
                if (dt_insurance.Rows.Count > 0)
                    insuranceSum = dt_insurance.Rows[0]["info"].ToDecimal();

                if (insuranceSum == 0)//single sum input
                {
                    var posd_ext = (from el in PoshItem.PosdItems
                                    select new
                                    {
                                        apply_insurance = el.apply_insurance,
                                        id = el.id,
                                        sum = el.sum,
                                        status_insurance = el.status_insurance,
                                        have_recipe = el.have_recipe,
                                        gr4 = el.gr4,
                                        baltic_category_id = el.baltic_category_id,
                                        atc = Task.WhenAll<string>(DB.recipe.getATCcode(el.productid))?.Result[0]?.ToString() ?? ""
                                    }).ToList();
                    string[] gr4_medicines = new string[8]
                        {
                                "OTC",
                                "OTC-N",
                                "Rx",
                                "RxK",
                                "RxK-N",
                                "RxK-V",
                                "Rx-N",
                                "Rx-V"
                        };
                    string[] gr4_vitamins = new string[1]
                        {
                                "MP"
                        };
                    Insurance dlg = new Insurance(PoshItem, "GJN");
                    dlg.Location = helpers.middleScreen(Program.Display1, dlg);
                    dlg.MedicinesSum = posd_ext.Where(pd => pd.apply_insurance == 1 && gr4_medicines.Contains(pd.gr4)).Sum(pd => pd.sum);//A. Rx || B.OTC
                    dlg.VitaminsSum = posd_ext.Where(pd => pd.apply_insurance == 1 && gr4_vitamins.Contains(pd.gr4)).Sum(pd => pd.sum);//C.7  Vitamins/minerals
                    dlg.OthersSum = dlg.MedicinesSum + dlg.VitaminsSum;
                    Program.Display1.Invoke(new Action(() =>
                    {
                        dlg.ShowDialog();
                    }));
                    if (dlg.DialogResult == System.Windows.Forms.DialogResult.OK)
                    {
                        insuranceSum = dlg.InsuranceSum;
                        result.CardSessionId = insuranceSum.ToString();
                    }
                    dlg.Dispose();
                    dlg = null;
                    decimal total_sum = posd_ext.Where(pd => pd.apply_insurance == 1 && (gr4_medicines.Contains(pd.gr4) || gr4_vitamins.Contains(pd.gr4))).Sum(pd => pd.sum);
                    int count = posd_ext.Where(pd => pd.apply_insurance == 1 && (gr4_medicines.Contains(pd.gr4) || gr4_vitamins.Contains(pd.gr4))).Count();
                    decimal remain = insuranceSum;
                    int i = 0;

                    foreach (var el in PoshItem.PosdItems)
                    {
                        decimal xCompensatedValue = 0;
                        int status = 0;
                        if (posd_ext.Where(p => p.id == el.id).Count() > 0 //if exist in posd_ext
                            && el.apply_insurance == 1
                            && (gr4_medicines.Contains(el.gr4) || gr4_vitamins.Contains(el.gr4)))
                        {
                            i++;
                            xCompensatedValue = Math.Round(insuranceSum * el.sum / total_sum, 2, MidpointRounding.AwayFromZero);
                            if (i == count)
                                xCompensatedValue = remain;
                            remain -= xCompensatedValue;
                            status = el.have_recipe == 1 ? 12 : 11;
                        }
                        else
                        {
                            status = 13;
                        }
                        if (el.status_insurance == 0)
                            await DB.cheque.CreateChequeTrans(el.id, xCompensatedValue, result.Company, result.CardSessionId, result.CardNoLong, null, status);
                        else
                            await DB.cheque.UpdateChequeTrans(el.id, xCompensatedValue, result.Company, result.CardSessionId, result.CardNoLong, status);
                    }
                }
            }
            catch (EndpointNotFoundException ex)
            {
                helpers.alert(Enumerator.alert.error, "Nepavyksta prisijungti prie " + result.CompanyString + " serverio.\nDraudimo kortelių aptarnavimas laikinai negalimas. Bandykite vėliau");
            }
            catch (Exception ex)
            {
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

        public override async Task<bool> VoidInsuranceCompensation(decimal posh_id, Items.Insurance insuranceItem)
        {
            return await DB.cheque.VoidChequeTrans(posh_id);
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

        public override void Close() { }
    }

    public class InsuranceGjensidige : InsuranceBase
    {
        private readonly ExternalServices.Gjensidige.GjensidigeService gjensidigeService;

        public InsuranceGjensidige(string baseAddress, string userName, string password)
        {
            gjensidigeService = new ExternalServices.Gjensidige.GjensidigeService(baseAddress, userName, password);
        }

        public override string AuthoriseInsuranceCard(Items.Insurance InsuranceItem)
        {
            return gjensidigeService.GetHashString();
        }

        public override async Task<Items.Insurance> CalcInsuranceCompensation(Items.posh PoshItem)
        {
            SR_TamroWS.TamroWSSoapClient scTamroWS = new SR_TamroWS.TamroWSSoapClient();
            Items.Insurance result = PoshItem.InsuranceItem;
            ExternalServices.Gjensidige.T067ClaimsCheckRequest t067ClaimsCheckRequest = new ExternalServices.Gjensidige.T067ClaimsCheckRequest()
            {
                LoginParameters = gjensidigeService.GetLoginParameters(),
                InsuredInfo = new ExternalServices.Gjensidige.InsuredInfo()
                {
                    BirthDateInfo = getPersonalDigits(PoshItem.InsuranceItem.CardNoLong).ToInt(),
                    FinId = (Int64)getCardNo(PoshItem.InsuranceItem.CardNoLong).ToDecimal()
                },
                ItemsInfo = (from pd in PoshItem.PosdItems
                             where pd.status_insurance != 13
                             select new ExternalServices.Gjensidige.ItemInfo()
                             {
                                 ItemGroup = scTamroWS.GetBalticCategory(pd.productid)?.cn3 ?? "",
                                 LastPrice = (int)(pd.sum * 100),
                                 LineId = pd.id.ToString(),
                                 RecipeTag = pd.recipeid > 0 || pd.erecipe_no > 0 || pd.status_insurance == 12 ? true : false,
                                 CompensatedTag = pd.recipeid == 0 ? false : true,//todo ir kompensuojama suma didesne uz 0
                                 UniqueCode = pd.productid.ToString(),//gal barkoda?
                                 Code = pd.baltic_category_id.ToString(),
                                 ItemName = pd.barcodename,
                                 AtcCode = Task.WhenAll<string>(DB.recipe.getATCcode(pd.productid))?.Result[0]?.ToString() ?? "",//todo
                                 FirstPrice = (int)(Math.Round(pd.qty * pd.price, 2) * 100),
                                 Discount = (int)pd.discount
                             }).ToList(),
                PartnerInfo = new ExternalServices.Gjensidige.PartnerInfo()
                {
                    PartnerCountry = "LT",
                    PartnerBranchCountry = "LT",
                    BranchAddress = Session.SystemData.address,
                    PartnerCode = Session.SystemData.ecode,
                    PosId = $"{Session.SystemData.prodcustid}_{Session.Devices.debtorname}"
                },
                OpDate = gjensidigeService.GetOperationDateTime(),
            };
            scTamroWS.Close();
            var t067ClaimsResponse = await gjensidigeService.CheckItems(t067ClaimsCheckRequest);
            result.Receipt = t067ClaimsResponse;
            foreach (var el in t067ClaimsResponse.ItemsPrices)
            {
                decimal insurancePaySum = el.InsurerPaySum;
                insurancePaySum /= 100;//nes jie saugo kaip int padauginta is 100
                var requetItem = t067ClaimsCheckRequest.ItemsInfo.FirstOrDefault(f => f.LineId == el.LineId);
                if (PoshItem.PosdItems.FirstOrDefault(f => f.id == el.LineId.ToDecimal()).status_insurance == 0)
                    await DB.cheque.CreateChequeTrans(el.LineId.ToDecimal(), insurancePaySum, result.Company, result.CardSessionId, result.CardNoLong, null, requetItem.RecipeTag ? 12 : 11);
                else
                    await DB.cheque.UpdateChequeTrans(el.LineId.ToDecimal(), insurancePaySum, result.Company, result.CardSessionId, result.CardNoLong, requetItem.RecipeTag ? 12 : 11);
            }
            if (t067ClaimsResponse.Status > 0)
                throw new Exception(t067ClaimsResponse.Message);

            return result;
        }

        public override Task<bool> CancelInsuranceCompensation(Items.posh PoshItem)
        {
            throw new NotImplementedException();
        }

        public override void Close()
        {
        }

        public override async Task<string> ConfirmInsuranceCompensation(Items.posh PoshItem)
        {
            var t067ClaimsResponse = (ExternalServices.Gjensidige.T067ClaimsResponse)PoshItem.InsuranceItem.Receipt;
            ExternalServices.Gjensidige.T067ClaimsRegisterRequest t067ClaimsRegisterRequest = new ExternalServices.Gjensidige.T067ClaimsRegisterRequest()
            {
                CheckId = t067ClaimsResponse.CheckId,
                LoginParameters = gjensidigeService.GetLoginParameters(),
                OpDate = gjensidigeService.GetOperationDateTime()
            };
            var t067ClaimsRegisterResponse = await gjensidigeService.Register(t067ClaimsRegisterRequest);
            if (t067ClaimsRegisterResponse.Status > 0)
                throw new Exception(t067ClaimsRegisterResponse.Message);
            return PoshItem.InsuranceItem.CardSessionId;
        }

        public override List<ComboBox<decimal>> GetCardBalance(Items.Insurance insuranceItem)
        {
            throw new NotImplementedException();
        }

        public override string getCardNo(string cardNoLong)
        {
            return cardNoLong.Substring(0, cardNoLong.Length - 4);
        }

        public override string getPersonalDigits(string cardNoLong)
        {
            return cardNoLong.Substring(cardNoLong.Length - 4); 
        }

        public override Task<bool> VoidInsuranceCompensation(decimal posh_id, Items.Insurance insuranceItem)
        {
            throw new NotImplementedException();
        }
    }
}
