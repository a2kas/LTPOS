using POS_display.Models.TransactionService.MedicationPackagesMapping;
using System;
using System.Collections.Generic;

namespace POS_display.Models.TransactionService.Medication
{
    public class MedicationPackage
    {
        public Guid MedicationPackageId { get; set; }
        public Guid? MedicationProductId { get; set; }
        public long ContainedMedicationId { get; set; }
        public long MedicationId { get; set; }
        public string MedicationName { get; set; }
        public long Npakid { get; set; }
        public long Npakid7 { get; set; }
        public long VaprisId { get; set; }
        public decimal? CompensationCost { get; set; }
        public decimal? RetailPrice { get; set; }
        public string PackageRegistrationNumber { get; set; }
        public string Numero { get; set; }
        public string MedicationStatus { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public string SupplyStatus { get; set; }
        public ICollection<MedicationPackageMapping> MedicationPackageMappings { get; set; }
    }
}
