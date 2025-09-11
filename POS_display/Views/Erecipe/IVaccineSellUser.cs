using TamroUtilities.HL7.Models;
using System;

namespace POS_display.Views.ERecipe
{
    public interface IVaccineSellUser
    {
        string VaccineFullName { set; get; }
        string SerialNumber { set; get; }
        DateTime VaccinationDate { set; get; }
        string VaccinatedDose { set; get; }
        DateTime VaccineExpiryDate { set; get; }
        int VaccinationTypeindex { set; get; }
        object VaccinationType { set; get; }
        PatientDto Patient { set; get; }
        void RaiseCloseEvent(object data, Enumerator.VaccineOrderEvent voe);
    }
}
