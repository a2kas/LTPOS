using POS_display.Models.FMD;
using System;
using System.Collections.Generic;
using System.Linq;
using static POS_display.Enumerator;
using FMD.Model;
using Newtonsoft.Json;

namespace POS_display.Items
{
    public class posd
    {
        private readonly List<FMDRestriction> _fmdRestrictions = new List<FMDRestriction>()
        {
            new FMDRestriction { State = State.Stolen },
            new FMDRestriction { State = State.Destroyed },
            new FMDRestriction { State = State.CheckedOut },
            new FMDRestriction { State = State.Recalled },
            new FMDRestriction { State = State.Exported },
            new FMDRestriction { State = State.Withdrawn },
            new FMDRestriction { State = State.Expired },
            new FMDRestriction { State = State.Sample, Info = "The pack was decommissioned as supplied at another location." },
            new FMDRestriction { State = State.Sample, Info = "The pack was decommissioned as sample at this location." },
            new FMDRestriction { State = State.FreeSample, Info = "The pack was decommissioned as sample at another location." },
            new FMDRestriction { State = State.FreeSample, Info = "The pack was decommissioned as sample at this location." },
            new FMDRestriction { State = State.Supplied, Info = "The pack was decommissioned as supplied at another location." }
        };

        public decimal id { get; set; }
        public string Type { get; set; }
        public string info { get; set; }
        public decimal productid { get; set; }
        public decimal barcodeid { get; set; }
        public decimal recipeid { get; set; }
        public decimal VaccinePrescriptionCompositionId { get; set; }
        public decimal VaccineDispensationCompositionId { get; set; }
        public decimal VaccinatedPatientId { get; set; }
        public decimal discount { get; set; }
        public decimal qty { get; set; }
        public decimal discount_sum { get; set; }
        public decimal hid { get; set; }
        public string barcode { get; set; }
        public string barcodename { get; set; }
        public string barcodename_info { get; set; }//grid rodo
        public string barcodename2 { get; set; }
        public decimal row_no { get; set; }
        public decimal gqty { get; set; }
        public decimal paysum { get; set; }
        public decimal compensationsum { get; set; }
        public string compcode { get; set; }
        public decimal salesprice { get; set; }
        public decimal recipeno { get; set; }
        public decimal basicprice { get; set; }
        public DateTime till_date { get; set; }
        public DateTime salesdate { get; set; }
        public decimal totalsum { get; set; }
        public decimal vatsize { get; set; }
        public decimal rqty { get; set; }
        public decimal RetailProductRatio { get; set; }
        public decimal comppercent { get; set; }
        public decimal price { get; set; }
        public decimal pricediscounted { get; set; }
        public decimal sum { get; set; }
        public decimal ecrtax { get; set; }
        public string endsum { get; set; }
        public string symbol { get; set; }//grid rodo
        public bool Saveable { get; set; }
        public decimal cheque_sum { get; set; }
        public decimal cheque_sum_insurance { get; set; }
        public int status_insurance { get; set; }
        public decimal retail_price { get; set; }
        public decimal erecipe_no { get; set; }
        public decimal erecipe_active { get; set; }
        public decimal tlk_status { get; set; }
        public string loyalty_type { get; set; }
        public int have_recipe { get; set; }
        public int apply_insurance { get; set; }
        public string InsuranceInfo { get; set; }
        public decimal baltic_category_id { get; set; }
        public string gr4 { get; set; }
        public string note2 { get; set; }
        public string atc { get; set; }
        public string npakid7 { get; set; }
        public bool fmd_required { get; set; }
        public string compensation_type { get; set; }
        public bool IsSalesOrderProduct { get; set; }
        public string eRecipeStatus { get; set; }
        public string eRecipeDiseaseCode { get; set; }
        public string RecipeDiseaseCode { get; set; }
        public decimal NotCompensatedRecipeId { get; set; }
        public DateTime? NotCompensatedTillDate { get; set; }
        public decimal PresentCardId { get; set; }
        public decimal OnHandQty { get; set; }
        public decimal BarcodeRatio { get; set; }
        public string DiscountType { get; set; }
        public string eRecipeDispenseBySubstancesGroupId { get; set; }
        public ProductFlag Flags { get; set; }
        [JsonIgnore]
        private List<wpf.Model.fmd> _fmd_model;
        [JsonIgnore]
        public List<wpf.Model.fmd> fmd_model
        {
            get
            {
                if (_fmd_model == null)
                    _fmd_model = new List<wpf.Model.fmd>();
                return _fmd_model;
            }
            set
            {
                _fmd_model = value;
            }
        }

        public bool IsValidToSellFMD
        {
            get
            {
                if (Flags.HasFlag(ProductFlag.FmdException))
                    return true;
                return fmd_model.Count >= Math.Abs(qty) && fmd_model.All(e => string.IsNullOrEmpty(e.alertId));
            }
        }

        public bool HasRestriction
        {
            get
            {
                foreach (var model in fmd_model)
                {
                    var restriction = _fmdRestrictions.FirstOrDefault(e => string.Equals(e
                        .State
                        .ToString()
                        .ToLowerInvariant(), model.state, StringComparison.InvariantCultureIgnoreCase));
                    if (restriction == null)
                        continue;
                    if (string.IsNullOrEmpty(restriction.Info) || restriction.Info.Equals(model.info, StringComparison.InvariantCultureIgnoreCase))
                        return true;
                }
                return false;
            }
        }

        public string fmd_link
        {
            get
            {
                return fmd_required ? "FMD" : "";
            }
        }

        public bool is_deposit
        {
            get
            {
                return barcode?.StartsWith("DEPOZITAS") ?? false;
            }
        }
    }
}
