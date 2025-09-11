using POS_display.Items;
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
    public class Offline : InsuranceBase
    {
        public Offline(Items.Insurance insuranceItem)
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
                    Popups.display1_popups.Insurance dlg = new Popups.display1_popups.Insurance(PoshItem, "GJN");
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
                            await DB.cheque.CreateChequeTrans(el.id, xCompensatedValue, result.Company, result.CardSessionId, result.CardNoLong, null, status, "");
                        else
                            await DB.cheque.UpdateChequeTrans(el.id, xCompensatedValue, result.Company, result.CardSessionId, result.CardNoLong, status, "");
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
