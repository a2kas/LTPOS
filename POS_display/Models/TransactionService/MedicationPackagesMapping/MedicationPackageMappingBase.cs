using System;

namespace POS_display.Models.TransactionService.MedicationPackagesMapping
{
    public abstract class MedicationPackageMappingBase
    {
        public Guid MedicationPackageMappingId { get; set; }
        public Guid MedicationPackageId { get; set; }
        public string Company { get; set; }
        public long ItemCode { get; set; }
        public byte[] RowVersion { get; set; }
    }
}
