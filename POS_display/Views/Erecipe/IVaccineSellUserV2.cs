using TamroUtilities.HL7.Models;
using System;

namespace POS_display.Views.ERecipe
{
    public interface IVaccineSellUserV2
    {
        MedicationDto Medication { get; set; }
        string SerialNumber { set; get; }
        DateTime VaccinationDate { set; get; }
        string VaccinatedDose { set; get; }
        DateTime VaccineExpiryDate { set; get; }
        int VaccinationTypeindex { set; get; }
        object VaccinationType { set; get; }
        PatientDto Patient { set; get; }
        LowIncomeDto LowIncome { set; get; }
        void RaiseCloseEvent(object data, Enumerator.VaccineOrderEvent voe);
        string InfectiousDiseaseCode { get; }
        string InfectiousDisease { get; set; }
    }
}
