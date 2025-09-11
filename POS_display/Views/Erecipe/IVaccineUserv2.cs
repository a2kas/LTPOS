using TamroUtilities.HL7.Models;
using POS_display.Models.Recipe;
using System.Collections.Generic;

namespace POS_display.Views.ERecipe
{
    public interface IVaccineUserV2
    {
        string PersonalCode { get; set; }
        string PatientName { get; set; }
        string PatientSurname { get; set; }
        string PatientBirthDate { get; set; }
        bool IsActiveSellVaccine { get; set; }
        bool IsActiveCancelVaccine { get; set; }
        VaccinationEntry SelectedVaccineEntry { get; set; }
        List<VaccinationEntry> VaccineEntries { get; set; }
        PatientDto Patient { get; set; }
        LowIncomeDto LowIncome { get; set; }
        int PageIndex { get; }
        int Total { get; set; }
    }
}
