using POS_display.Models.Recipe;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TamroUtilities.HL7.Models;
using TamroUtilities.HL7.Models.Dispense;
using TamroUtilities.HL7.Models.Encounter;
using static POS_display.Utils.EHealth.EHealthUtils;

namespace POS_display.Utils.EHealth
{
    public interface IEHealthUtils
    {
        CancellationTokenSource cts { get; set; }

        bool Init();

        T GetPractitioner<T>(string StampCode, string personalCode = "");

        T GetOrganization<T>(string SVEIDRAID);

        Task<T> GetPatient<T>(string SearchTxt, string PatientId);

        Task<T> GetLowIncome<T>(string searchTxt);

        Task<T> GetAccumulatedSurcharge<T>(string personalCode, string DIK, string validDate);

        Task<T> GetAllergies<T>(string PatientId);

        Task<T> GetRepresentedPersons<T>(string PatientId);

        Task<EncounterDto> CreateEncounter(string PatientId, string PatientRef, string PractitionerRef, string OrganizationRef, string patientEsi, bool? isPatientInsured, EncounterReason reason);

        Task<T> GetRecipeList<T>(string PatientId, int PageSize, int Page, string Status);

        Task<T> GetDispenseList<T>(string FilterPatientId, string FilterPractitionerId, string FilterOrganizationId, string Status, string StatusConfirmed, string DocStatus, int PageSize, int Page, string FilterMedicationPrescriptionId, List<string> compositionIds);

        DispenseListDto UpdateDispense(UpdateDispenseRequest updateDispenseRequest);

        Task<T> GetRecipe<T>(string RecipeNumber, bool IncludePatient);

        Task<T> LockRecipe<T>(string MedicationPrescriptionId, string PractitionerRef, string OrganizationRef, MedicationPrescriptionStatus? Status = MedicationPrescriptionStatus.OnHold);

        Task<T> UnlockRecipe<T>(string MedicationPrescriptionId);

        T GetMedicationList<T>(string parameter, string value, string PageSize = "", string Page = "");

        Task<RecipeDispenseDto> CreateRecipeDispense(Items.eRecipe.Recipe eRecipeItem, string PickedUpByRef, DateTime DispenseDueDate, decimal PriceRetail, decimal PricePaid, decimal PriceCompensated, decimal prepCompSum, decimal Quantity, DateTime DispenseDate, bool confirmDispense, bool isCheapest, int durationOfUse);

        Task<List<RecipeDispenseDto>> CreateRecipeDispenseMultiple(List<GroupDispenseRequest> groupDispenseRequests);

        Task GetDispensePdf(string CompositionId, string proc_name = "GetSignedDispensePdf", string pdf_name = "SignedPdfBase64Encoded");

        void GetSignedDispensePdf(string CompositionId, SOAPCallback callback);

        void GetUnsignedDispensePdf(string CompositionId, SOAPCallback callback);

        void ConfirmSignedDispensePdf(string CompositionId, string SignedPdfBase64Encoded, SOAPCallback callback);

        Task<bool> SignDispense(string CompositionId);

        Task<bool> SignVaccine(string CompositionId);

        Task<bool> CancelRecipeDispense(string CompositionId, string StatusReason, List<Items.eRecipe.Issue> issues);

        Task<T> ReserveRecipe<T>(string MedicationPrescriptionId, string PractitionerRef, string StatusReason, string deliveryDate);

        Task<T> CancelRecipeReservation<T>(string MedicationPrescriptionId, string PractitionerRef, string StatusReason);

        void SuspendRecipe(string MedicationPrescriptionId, string PractitionerRef, string StatusReason, SOAPCallback callback);

        void CancelRecipeSuspension(string MedicationPrescriptionId, string PractitionerRef, string StatusReason, SOAPCallback callback);

        Task<bool> ConfirmRecipeDispense(string MedicationDispenseId, string PractitionerRef, bool ConfirmDispense);

        List<string> GetClassifier(string ClassifierName, string DateFrom);

        Task<T> GetVaccinationData<T>(
           string patientId,
           string practitionerId,
           string organizationId,
           int docType,
           string status,
           string docStatus,
           int pageSize,
           int page,
           string dtFrom,
           string dtTo,
           string orderIds);

        Task<T> CheckESI<T>(string esiNr);

        Task<T> CancelVaccinationPrescription<T>(string compositionId, string reason);

        Task<T> CancelVaccinationDispensation<T>(string compositionId, string reason);

        Task<T> MakeVaccinationPrescription<T>(
            string patientRef,
            string encounterRef,
            string infectiousDiseaseCode,
            string infectiousDiseaseDisplay,
            string medicationName,
            string medicationNPAKID,
            int doseNumber,
            string notes);

        Task<T> MakeVaccinationDispensation<T>(
            string patientRef,
            string medicationRef,
            string encounterRef,
            string orderRef,
            string orderResponseId,
            string infectiousDiseaseCode,
            string infectiousDiseaseDisplay,
            string medicationName,
            string medicationNPAKID,
            string routeCode,
            string routeDisplay,
            string vaccineSerie,
            int doseNumber,
            DateTime vaccinationDate,
            DateTime medicineExpiryDate);

        Task<T> MakeVaccinationDispensationV2<T>(
            string patientRef,
            string medicationRef,
            string encounterRef,
            string orderRef,
            string infectiousDiseaseCode,
            string infectiousDiseaseDisplay,
            string medicationName,
            string medicationNPAKID,
            string routeCode,
            string routeDisplay,
            string vaccineSerie,
            int doseNumber,
            DateTime medicineExpiryDate);

        T GetImmunization<T>(string patientId, string practitionerId, string dateFrom, string dateTo, int pageSize = 100, int page = 1);

        Task<T> CreatePaperRecipeDispense<T>(CreateDispenseRequest request);

        Task<T> ChangeRecipeStatus<T>(string MedicationPrescriptionId, MedicationPrescriptionStatus status, string StatusReason);

        Task RetryLockRecipe(string compositionId, bool locking, string dispenseBySubstancesGroupId);

        void SetRequestorId(string practitionerId);

    }
}
