using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using POS_display.Items;
using POS_display.Utils.Logging;

namespace POS_display.Utils.Insurance.Gjensidige
{
    public class Gjensidige : InsuranceBase
    {
        private readonly GjensidigeService gjensidigeService;
        private List<string> saperionClaimsNrList = new List<string>();

        public Gjensidige(string baseAddress, string userName, string password, string applicationUserName)
        {
            gjensidigeService = new GjensidigeService(baseAddress, userName, password, applicationUserName);
        }

        public override string AuthoriseInsuranceCard(Items.Insurance InsuranceItem)
        {
            return gjensidigeService.GetHashString();
        }

        public override async Task<Items.Insurance> CalcInsuranceCompensation(Items.posh PoshItem)
        {
            var result = PoshItem.InsuranceItem;
            try
            {
                var t067ClaimsCheckRequest = new ExternalServices.Gjensidige.T067ClaimsCheckRequest();
                t067ClaimsCheckRequest.LoginParameters = gjensidigeService.GetLoginParameters();
                t067ClaimsCheckRequest.InsuredInfo = new ExternalServices.Gjensidige.InsuredInfo()
                {
                    FinId = PoshItem.InsuranceItem.CardNoLong.Substring(0, PoshItem.InsuranceItem.CardNoLong.Length - 4),
                    BirthDateInfo = PoshItem.InsuranceItem.CardNoLong.Remove(0, PoshItem.InsuranceItem.CardNoLong.Length - 4)
                };
                t067ClaimsCheckRequest.ItemsInfo = (from pd in PoshItem.PosdItems
                                                    where pd.status_insurance != 13
                                                    select new ExternalServices.Gjensidige.ItemInfo()
                                                    {
                                                        ItemGroup = pd.baltic_category_id.ToString(),
                                                        LastPrice = (int)(pd.sum * 100),
                                                        LineId = pd.id.ToString(),
                                                        RecipeTag = pd.recipeid > 0 || pd.erecipe_no > 0 || pd.status_insurance == 12,
                                                        CompensatedTag = pd.recipeid != 0,//todo ir kompensuojama suma didesne uz 0
                                                        UniqueCode = pd.productid.ToString(),//gal barkoda?
                                                        Code = pd.baltic_category_id.ToString(),
                                                        ItemName = pd.barcodename,
                                                        AtcCode = Task.WhenAll<string>(DB.recipe.getATCcode(pd.productid))?.Result[0]?.ToString() ?? "",//todo
                                                        FirstPrice = (int)(Math.Round(pd.qty * pd.price, 2) * 100),
                                                        Discount = (int)pd.discount
                                                    }).ToList();
                t067ClaimsCheckRequest.PartnerInfo = new ExternalServices.Gjensidige.PartnerInfo()
                {
                    PartnerCountry = "LT",
                    PartnerBranchCountry = "LT",
                    BranchAddress = Session.SystemData.address,
                    PartnerCode = Session.SystemData.ecode,
                    PosId = $"{Session.SystemData.prodcustid}_{Session.Devices.debtorname}"
                };
                t067ClaimsCheckRequest.OpDate = gjensidigeService.GetOperationDateTime();
                t067ClaimsCheckRequest.Guid = Guid.NewGuid();
                var t067ClaimsResponse = await gjensidigeService.CheckItems(t067ClaimsCheckRequest);
                result.Receipt = t067ClaimsResponse;
                foreach (var el in t067ClaimsResponse.ItemsPrices)
                {
                    decimal insurancePaySum = el.InsurerPaySum;
                    insurancePaySum /= 100;//nes jie saugo kaip int padauginta is 100
                    var requetItem = t067ClaimsCheckRequest.ItemsInfo.FirstOrDefault(f => f.LineId == el.LineId);
                    if (PoshItem.PosdItems.FirstOrDefault(f => f.id == el.LineId.ToDecimal()).status_insurance == 0)
                        await DB.cheque.CreateChequeTrans(el.LineId.ToDecimal(), insurancePaySum, result.Company, result.CardSessionId, result.CardNoLong, null, requetItem.RecipeTag ? 12 : 11, "");
                    else
                        await DB.cheque.UpdateChequeTrans(el.LineId.ToDecimal(), insurancePaySum, result.Company, result.CardSessionId, result.CardNoLong, requetItem.RecipeTag ? 12 : 11, "");
                }
                foreach (var el in PoshItem.PosdItems.Where(p => !t067ClaimsResponse.ItemsPrices.Any(i => i.LineId.ToDecimal() == p.id)))
                {
                    await DB.cheque.UpdateChequeTrans(el.id, 0, result.Company, result.CardSessionId, result.CardNoLong, el.recipeid > 0 ? 12 : 11, "");
                }

                if (t067ClaimsResponse.Status > 0)
                    throw new Exception(t067ClaimsResponse.Message);
            }
            catch (Exception ex)
            {
                helpers.alert(Enumerator.alert.error, ex.Message);
                await DB.cheque.ClearInsuranceData(PoshItem.Id);
                result = null;
            }

            return result;
        }

        public override async Task<bool> CancelInsuranceCompensation(Items.posh PoshItem)
        {
            ExternalServices.Gjensidige.PharmacyClaimCancelCaseRequest cancelClaimsRequest = new ExternalServices.Gjensidige.PharmacyClaimCancelCaseRequest(){ };
            cancelClaimsRequest.LoginParameters = gjensidigeService.GetLoginParameters();
            cancelClaimsRequest.PartnerInfo = new ExternalServices.Gjensidige.PartnerInfo()
            {
                PartnerCountry = "LT",
                PartnerBranchCountry = "LT",
                BranchAddress = Session.SystemData.address,
                PartnerCode = Session.SystemData.ecode,
                PosId = $"{Session.SystemData.prodcustid}_{Session.Devices.debtorname}",
                PartnerBranchCity = "TestCity"
            };
            bool somethingFailed = false;
            foreach (var claimNr in saperionClaimsNrList)
            {
                try
                {
                    cancelClaimsRequest.SaperionClaimNr = claimNr;
                    var cancelClaimsResponse = await gjensidigeService.CancelClaims(cancelClaimsRequest);
                    if (cancelClaimsResponse.Status != 0)
                    {
                        somethingFailed = true;
                    }
                }
                catch (Exception ex)
                {
                    somethingFailed = true;
                }
            }
            if (somethingFailed) helpers.alert(Enumerator.alert.error, "Draudimo kompensacijos atšaukimas nepavyko. Skambinkite į Gjensidige");
            return somethingFailed;
        }

        public override async Task<string> ConfirmInsuranceCompensation(Items.posh PoshItem)
        {
            var result = PoshItem.InsuranceItem;
            var t067ClaimsCheckRequest = new ExternalServices.Gjensidige.T067ClaimsCheckRequest();
            t067ClaimsCheckRequest.LoginParameters = gjensidigeService.GetLoginParameters();
            t067ClaimsCheckRequest.InsuredInfo = new ExternalServices.Gjensidige.InsuredInfo
            {
                FinId = PoshItem.InsuranceItem.CardNoLong.Substring(0, PoshItem.InsuranceItem.CardNoLong.Length - 4),
                BirthDateInfo = PoshItem.InsuranceItem.CardNoLong.Remove(0, PoshItem.InsuranceItem.CardNoLong.Length - 4)
            };

            t067ClaimsCheckRequest.ItemsInfo = (from pd in PoshItem.PosdItems
                                                where pd.status_insurance != 13
                                                select new ExternalServices.Gjensidige.ItemInfo()
                                                {
                                                    ItemGroup = pd.baltic_category_id.ToString(),
                                                    LastPrice = (int)(pd.sum * 100),
                                                    LineId = pd.id.ToString(),
                                                    RecipeTag = pd.recipeid > 0 || pd.erecipe_no > 0 || pd.status_insurance == 12,
                                                    CompensatedTag = pd.recipeid != 0,//todo ir kompensuojama suma didesne uz 0
                                                    UniqueCode = pd.productid.ToString(),//gal barkoda?
                                                    Code = pd.baltic_category_id.ToString(),
                                                    ItemName = pd.barcodename,
                                                    AtcCode = Task.WhenAll<string>(DB.recipe.getATCcode(pd.productid))?.Result[0]?.ToString() ?? "",//todo
                                                    FirstPrice = (int)(Math.Round(pd.qty * pd.price, 2) * 100),
                                                    Discount = (int)pd.discount
                                                }).ToList();
            t067ClaimsCheckRequest.PartnerInfo = new ExternalServices.Gjensidige.PartnerInfo()
            {
                PartnerCountry = "LT",
                PartnerBranchCountry = "LT",
                BranchAddress = Session.SystemData.address,
                PartnerCode = Session.SystemData.ecode,
                PosId = $"{Session.SystemData.prodcustid}_{Session.Devices.debtorname}"
            };
            t067ClaimsCheckRequest.OpDate = gjensidigeService.GetOperationDateTime();
            t067ClaimsCheckRequest.Guid = Guid.NewGuid();
            var t067ClaimsResponse = await gjensidigeService.RegisterClaim(t067ClaimsCheckRequest);
            string res = "";
            foreach (var i in t067ClaimsResponse.ClaimCaseItems)
            {
                saperionClaimsNrList.Add(i.SaperionClaimsNr);
                res += i.SaperionClaimsNr + ",";
            }
            if (res != "")
                await DB.cheque.ConfirmChequeTrans(PoshItem.Id, PoshItem.InsuranceItem.CardSessionId);
            return res;
        }

        public override List<ComboBox<decimal>> GetCardBalance(Items.Insurance insuranceItem)
        {
            return new List<ComboBox<decimal>>();
        }

        public override string getCardNo(string cardNoLong)
        {
            return cardNoLong.Substring(0, cardNoLong.Length - 4);
        }

        public override string getPersonalDigits(string cardNoLong)
        {
            return cardNoLong.Substring(cardNoLong.Length - 4);
        }

        public override async Task<bool> VoidInsuranceCompensation(Items.posh poshItem)
        {
            var cancelClaimsRequest = new ExternalServices.Gjensidige.PharmacyClaimCancelCaseRequest();
            cancelClaimsRequest.LoginParameters = gjensidigeService.GetLoginParameters();
            cancelClaimsRequest.PartnerInfo = new ExternalServices.Gjensidige.PartnerInfo()
            {
                PartnerCountry = "LT",
                PartnerBranchCountry = "LT",
                BranchAddress = Session.SystemData.address,
                PartnerCode = Session.SystemData.ecode,
                PosId = $"{Session.SystemData.prodcustid}_{Session.Devices.debtorname}"
            };
            var somethingFailed = false;
            foreach (var claimNr in saperionClaimsNrList)
            {
                try
                {
                    cancelClaimsRequest.SaperionClaimNr = claimNr;
                    var cancelClaimsResponse = await gjensidigeService.CancelClaims(cancelClaimsRequest);

                    if (cancelClaimsResponse.Status != 0)
                        somethingFailed = true;
                }
                catch (Exception e)
                {
                    somethingFailed = true;
                }
            }

            if (somethingFailed) 
                helpers.alert(Enumerator.alert.error, "Draudimo kompensacijos atšaukimas nepavyko. Skambinkite į gjensidige", true);

            return somethingFailed;
        }
    }
}