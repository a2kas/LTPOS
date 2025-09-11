using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using TamroUtilities.HL7.Models;
using TamroUtilities.HL7.Models.AccumulatedSurcharge;

namespace POS_display.Items.eRecipe
{
    public class Recipe
    {
        public PatientDto Patient { get; set; }
        public LowIncomeDto HasLowIncome { get; set; }
        public AccumulatedSurchargeDto AccumulatedSurcharge { get; set; }
        public EncounterDto Encounter { get; set; }
        public MedicationDto Medication { get; set; }
        public List<Items.eRecipe.Issue> DispenseWarnings { get; set; }
        public RecipeDispenseDto RecipeDispense { get; set; }
        public RecipeDto eRecipe { get; set; }
        public DispenseListDto DispenseList { get; set; } = new DispenseListDto();
        public string PrescriptionStatus { get; set; } = string.Empty;
        public string MppBarcode { get; set; } = string.Empty;
        public string Diagnosis
        {
            get
            {
                if (!string.IsNullOrEmpty(eRecipe?.ConditionCode))
                {
                    return $"{eRecipe?.ConditionCode} {eRecipe?.ConditionDisplay}";
                }
                else
                {
                    return $"{eRecipe?.ReasonCode} {eRecipe?.ReasonName}";
                }
            }
        }

        public string DiagnosisCode
        {
            get
            {
                if (!string.IsNullOrEmpty(eRecipe?.ConditionCode))
                {
                    return eRecipe?.ConditionCode;
                }
                else
                {
                    return eRecipe?.ReasonCode;
                }
            }
        }

        public string AdditionalInstructionsForPatient { get; set; }
        public bool IsDispenseBySubtances { get; set; }
        public string DispenseBySubstancesGroupId { get; set; }
        public bool IsOverLimitQty { get; set; }
        public bool IsEligibleReimbursement
        {
            get { return HasLowIncome?.HasLowIncome == true || (AccumulatedSurcharge?.SurchargeEligible == true && eRecipe?.Type != "mpp"); }
        }
        public bool AllowSurchargeEligible
        {
            get { return AccumulatedSurcharge?.SurchargeEligible == true && eRecipe?.Type != "mpp"; }
        }
        public bool IsValidToDispenseWithoutValidPrescription()
        {
            return string.Equals(eRecipe.Status, "completed", StringComparison.OrdinalIgnoreCase);
        }
        #region RecipeDto_converted
        public decimal eRecipe_RecipeNumber
        {
            get
            {
                return (eRecipe?.RecipeNumber ?? "").ToDecimal();
            }
            set
            {
                eRecipe.RecipeNumber = value.ToString();
            }
        }

        public bool eRecipe_CompensationTag
        {
            get
            {
                return eRecipe.CompensationTag.ToBool();
            }
            set
            {
                eRecipe.CompensationTag = value.ToString();
            }
        }

        public int eRecipe_NumberOfRepeatsAllowed
        {
            get
            {
                return eRecipe.NumberOfRepeatsAllowed.ToInt();
            }
            set
            {
                eRecipe.NumberOfRepeatsAllowed = value.ToString();
            }
        }

        public bool eRecipe_PrescriptionTagsLongTag
        {
            get
            {
                return eRecipe?.PrescriptionTagsLongTag?.ToBool() ?? false;
            }
            set
            {
                eRecipe.PrescriptionTagsLongTag = value.ToString();
            }
        }

        public bool eRecipe_PrescriptionTagsNominalTag
        {
            get
            {
                return eRecipe?.PrescriptionTagsNominalTag?.ToBool() ?? false;
            }
        }


        public bool eRecipe_PrescriptionTagsByDemandTag
        {
            get
            {
                return eRecipe?.PrescriptionTagsByDemandTag?.ToBool() ?? false;
            }
        }


        public bool eRecipe_PrescriptionTagsLabelingExemptionTag
        {
            get
            {
                return eRecipe?.PrescriptionTagsLabelingExemptionTag?.ToBool() ?? false;
            }
        }

        public DateTime eRecipe_DateWritten
        {
            get
            {
                return Convert.ToDateTime(eRecipe.DateWritten);
            }
            set
            {
                eRecipe.DateWritten = value.ToString();
            }
        }

        public DateTime eRecipe_ValidFrom
        {
            get
            {
                return Convert.ToDateTime(eRecipe.ValidFrom);
            }
            set
            {
                eRecipe.ValidFrom = value.ToString();
            }
        }

        public DateTime eRecipe_ValidTo
        {
            get
            {
                return Convert.ToDateTime(eRecipe.ValidTo);
            }
            set
            {
                eRecipe.ValidTo = value.ToString();
            }
        }

        public decimal eRecipe_DosePerDayQuantityValue
        {
            get
            {
                return eRecipe.DosePerDayQuantityValue.ToDecimal();
            }
            set
            {
                eRecipe.DosePerDayQuantityValue = value.ToString();
            }
        }

        public decimal eRecipe_CompensationCode
        {
            get
            {
                return eRecipe.CompensationCode.ToDecimal();
            }
            set
            {
                eRecipe.CompensationCode = value.ToString();
            }
        }

        public DateTime LatestDueDate 
        {
            get { return DispenseList?.DispenseList?.OrderByDescending(e => e.DateWritten?.ToDateTime())?.FirstOrDefault()?.DueDate?.ToDateTime() ?? DateTime.Now; }
        }

        public DateTime EarliestDueDate
        {
            get { return DispenseList?.DispenseList?.Min(e => e.DueDate?.ToDateTime()) ?? DateTime.Now; }
        }

        #endregion

        private int _DispenseCount = 0;
        private DateTime _PastValitTo;
        private decimal _CompositionId = 0;
        private decimal _DispensedQty = 0;
        public int DispenseCount
        {
            get { return _DispenseCount; }
            set { _DispenseCount = value; }
        }
        public DateTime PastValidTo
        {
            get { return _PastValitTo; }
            set { _PastValitTo = value; }
        }

        public int ValidationPeriod { get; set; }

        public decimal CompositionId
        {
            get { return _CompositionId; }
            set { _CompositionId = value; }
        }
        public decimal DispensedQty
        {
            get { return _DispensedQty; }
            set { _DispensedQty = value; }
        }

        public decimal DurationOfUseSum
        {
            get
            {
                return DispenseList?.DispenseList?.Sum(e =>
                    decimal.TryParse(e.DurationOfUse, out decimal result) ? result : 0m) ?? 0m;
            }
        }

        public string LastDispenseDueDateValue
        {
            get
            {
                if (DispenseList?.DispenseList == null || DispenseList.DispenseList.Count == 0)
                    return "Nėra";
                return DispenseList?.DispenseList.OrderByDescending(e => e.DateWritten).First().DueDate.Substring(0,10);
            }
        }

        public decimal QtyLeftDispense
        {
            get 
            {
                decimal qtyLeftDispense = (eRecipe?.QuantityValue?.ToDecimal() ?? 0) - _DispensedQty;
                return qtyLeftDispense < 0 ? 0 : qtyLeftDispense;
            }
        }

        public Recipe DeepClone()
        {
            var json = JsonConvert.SerializeObject(this);
            return JsonConvert.DeserializeObject<Recipe>(json);
        }
    }
}
