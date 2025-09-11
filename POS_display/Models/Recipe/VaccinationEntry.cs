using System;
using TamroUtilities.HL7.Models;

namespace POS_display.Models.Recipe
{
    [Serializable]
    public class VaccinationEntry
    {
        public long? OrderId { get; set; }
        public VaccinationDataDto Prescription { get; set; }
        public VaccinationDataDto Dispensation { get; set; }
        public string OrderStatus
        {
            get
            {
                if (Prescription != null && Dispensation == null)
                    return Prescription.OrderResponse?.Status ?? "Accepted";
                if (Dispensation != null)
                    return Dispensation.OrderResponse?.Status ?? "Accepted";
                return "Accepted";
            }            

        }
    }
}
