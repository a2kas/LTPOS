using POS_display.Models.TransactionService.MedicationPackagesMapping;
using System;
using System.Threading.Tasks;

namespace POS_display.Utils.SAMAS
{
    public interface ISamasUtils
    {
        Task<decimal> GetNpakid7ByProductId(decimal productId);
        Task<MedicationPackageMapping> GetMedicationPackageMappingByProductId(decimal productId);
        Task<MedicationPackageMapping> GetMedicationPackageMappingByMedicationPackageId(Guid medicationPackageId, bool deleted);
    }
}
