using System;
using System.Collections.Generic;

namespace POS_display.Models.TransactionService.Medication
{
    public class MedicationProduct
    {
        public Guid MedicationProductId { get; set; }
        public List<MedicationPackage> MedicationPackage { get; set; }
        public long MedicationId { get; set; }
        public string MedicationName { get; set; }
        public string MedicationGroup { get; set; }
        public string MedicationSubgroup { get; set; }
        public string ActiveSubstances { get; set; }
        public string Strength { get; set; }
        public string PharmaceuticalFormCode { get; set; }
        public string PharmaceuticalForm { get; set; }
        public string ATC { get; set; }
        public string ATCName { get; set; }
        public string PrescriptionMark { get; set; }
        public string MedicationStatus { get; set; }
        public string MarketingAuthorizationHolder { get; set; }
        public string DistributionBaseTypeCode { get; set; }
        public string DistributionBaseType { get; set; }
        public bool? LowTherapeuticIndex { get; set; }
        public DateTime? RegistrationDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
