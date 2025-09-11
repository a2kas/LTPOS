using Microsoft.Extensions.DependencyInjection;
using POS_display.Models.TransactionService.Medication;
using POS_display.Models.TransactionService.MedicationPackagesMapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tamroutilities.Client;

namespace POS_display.Utils.SAMAS
{
    public class SamasUtils : ISamasUtils
    {
        private readonly ITamroClient _tamroClient;
        private Dictionary<decimal, MedicationPackageMapping> _mappingCache;

        public SamasUtils()
        {
            _tamroClient = Program.ServiceProvider.GetRequiredService<ITamroClient>();
            _mappingCache = new Dictionary<decimal, MedicationPackageMapping>();
        }

        public async Task<decimal> GetNpakid7ByProductId(decimal productId)
        {
            var medicationMapping = await GetMedicationPackageMappingByProductId(productId);

            if (medicationMapping == null) return 0;

            var medicationPackage = await _tamroClient.GetAsync<MedicationPackage>
                (string.Format(Session.TransactionV1GetMedicationPackages, medicationMapping.MedicationPackageId));

            if (medicationPackage == null || medicationPackage.MedicationProductId == null)
                return 0;

            return medicationPackage.Npakid7;
        }

        public async Task<MedicationPackageMapping> GetMedicationPackageMappingByProductId(decimal productId) 
        {
            if (_mappingCache.ContainsKey(productId))
                return _mappingCache[productId];

            var medicationPackageMappings = await _tamroClient.GetAsync<List<MedicationPackageMapping>>
                (string.Format(Session.TransactionV1GetMedicationPackageMappingsByItemCode, productId, Session.ParentCompanyCode, false));

            if (medicationPackageMappings == null || medicationPackageMappings.Count == 0)
            {
                medicationPackageMappings = await _tamroClient.GetAsync<List<MedicationPackageMapping>>
                    (string.Format(Session.TransactionV1GetMedicationPackageMappingsByItemCode, productId, Session.ParentCompanyCode, true));
            }

            if (medicationPackageMappings == null || medicationPackageMappings.Count == 0)
                return null;

            _mappingCache.Add(productId, medicationPackageMappings.First());
            return medicationPackageMappings.First();
        }

        public async Task<MedicationPackageMapping> GetMedicationPackageMappingByMedicationPackageId(Guid medicationPackageId, bool deleted)
        {
            var medicationPackageMappings = await _tamroClient.GetAsync<List<MedicationPackageMapping>>
                (string.Format(Session.TransactionV1GetMedicationPackageMappingsByMedicationPackageId, 
                medicationPackageId.ToString().ToUpperInvariant(), Session.ParentCompanyCode, deleted));

            if (medicationPackageMappings == null || medicationPackageMappings.Count == 0)
                return null;

            var productId = medicationPackageMappings.First().ItemCode;
            if (!_mappingCache.ContainsKey(productId))
                _mappingCache.Add(productId, medicationPackageMappings.First());

            return medicationPackageMappings.First();
        }
    }
}
