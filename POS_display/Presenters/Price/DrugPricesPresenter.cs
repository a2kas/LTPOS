using POS_display.Exceptions;
using POS_display.Models.TransactionService.Medication;
using POS_display.Models.TransactionService.MedicationPackagesMapping;
using POS_display.Repository.Barcode;
using POS_display.Views.DrugPrices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Tamroutilities.Client;

namespace POS_display.Presenters.Price
{
    public class DrugPricesPresenter : BasePresenter, IDrugPricesPresenter
    {
        #region Members
        private readonly IDrugPricesView _view;
        private readonly ITamroClient _tamroClient;
        private readonly IBarcodeRepository _barcodeRepository;
        #endregion

        #region Constructor
        public DrugPricesPresenter(IDrugPricesView view, ITamroClient tamroClient, IBarcodeRepository barcodeRepository)
        {
            _view = view ?? throw new ArgumentNullException(nameof(view));
            _tamroClient = tamroClient ?? throw new ArgumentNullException(nameof(tamroClient));
            _barcodeRepository = barcodeRepository ?? throw new ArgumentNullException(nameof(barcodeRepository));
        }
        #endregion

        #region Public methods
        public async Task<string> GetBarcodeByActiveSubstance(string activeSubstance)
        {
            var productId = await GetProductIdByActiveSubstance(activeSubstance);
            if (productId is null)
                throw new DrugPricesException($"Nesurasta nei viena prekė su '{activeSubstance}' aktyviaja medžiaga kuria prekiaujme");

            return await _barcodeRepository.FindBarcodeByProductId(productId.Value);
        }

        public async Task<string> GetBarcodeByGenericName(string genericName)
        {
            var productId = await GetProductIdByGenericName(genericName);
            if (productId is null)
                throw new DrugPricesException($"Nesurasta nei viena prekė su '{genericName}' firminiu pavadinimu kuria prekiaujme");

            return await _barcodeRepository.FindBarcodeByProductId(productId.Value);
        }

        public async Task<decimal?> GetProductIdByActiveSubstance(string activeSubstance)
        {
            if (string.IsNullOrWhiteSpace(activeSubstance))
            {
                throw new ArgumentException("Active substance cannot be null or whitespace.", nameof(activeSubstance));
            }

            try
            {
                var encodedActiveSubstance = HttpUtility.UrlEncode(activeSubstance);
                var medicationProducts = await _tamroClient.GetAsync<List<MedicationProduct>>
                    ($"/api/v1/medicationproducts?ActiveSubstance={encodedActiveSubstance}&Take=1000");

                if (medicationProducts == null || medicationProducts.Count == 0)
                {
                    return null;
                }

                var medicationProduct = medicationProducts.First();
                var medicationPackage = medicationProduct?.MedicationPackage?.FirstOrDefault(e => e.MedicationPackageMappings.Count != 0);
                var mapping = medicationPackage?.MedicationPackageMappings
                                .FirstOrDefault(e => e.Company == Session.ParentCompanyCode);

                return mapping?.ItemCode;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<decimal?> GetProductIdByGenericName(string genericName)
        {
            if (string.IsNullOrWhiteSpace(genericName))
            {
                throw new ArgumentException("Generic name cannot be null or whitespace.", nameof(genericName));
            }

            try
            {
                var encodedGenericName = HttpUtility.UrlEncode(genericName);
                var medicationPackages = await _tamroClient.GetAsync<List<MedicationPackage>>
                                        ($"/api/v1/medicationpackages?MedicationName={encodedGenericName}&Take=1000");

                if (medicationPackages == null || medicationPackages.Count == 0)
                {
                    return null;
                }

                string args = BuildQuery(medicationPackages.ConvertAll(e => e.MedicationPackageId));
                var medicationPackageMappings = await _tamroClient.GetAsync<List<MedicationPackageMapping>>
                    ($"/api/v1/medicationpackagesmappings?{args}" +
                    $"&Company={Session.ParentCompanyCode}&Take=1000");

                if (medicationPackageMappings == null || medicationPackageMappings.Count == 0)
                {
                    return null;
                }

                return medicationPackageMappings.First().ItemCode;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        #endregion

        #region Private methods
        private string BuildQuery(List<Guid> medicationPackageIDs)
        {
            if (medicationPackageIDs == null || medicationPackageIDs.Count == 0)
            {
                return string.Empty;
            }

            StringBuilder queryBuilder = new StringBuilder();
            foreach (var medicationPackageID in medicationPackageIDs)
            {
                queryBuilder.AppendFormat("MedicationPackageId={0}&", medicationPackageID);
            }

            if (queryBuilder.Length > 0)
            {
                queryBuilder.Length--;
            }

            return queryBuilder.ToString();
        }
        #endregion
    }
}
