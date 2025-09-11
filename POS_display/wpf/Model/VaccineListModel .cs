using TamroUtilities.HL7.Models;
using System;

namespace POS_display.wpf.Model
{
    public class VaccineListModel
    {
        public VaccineListModel(VaccinationDataDto dto)
        {
            selected = false;
            VaccineOrder = dto;
        }

        public VaccinationDataDto VaccineOrder { get; set; }
        public bool selected { get; set; }
        public string CompositionId
        {
            get
            {
                return VaccineOrder.CompositionId;
            }
        }
        public string DocType
        {
            get
            {
                if (VaccineOrder.CompositionType == "34108-1")                
                    return "Skyrimas";                
                else if (VaccineOrder.CompositionType == "11369-6")                
                    return "Išdavimas";                
                else                 
                    return "Nežinomas";
                
            }
        }
        public string Practitioner
        {
            get
            {
                string fullName = string.Empty;
                fullName += VaccineOrder.Practitioner.GivenName.Count != 0 ? VaccineOrder.Practitioner.GivenName[0] : string.Empty;
                fullName += fullName != string.Empty ? " " : "";
                fullName += VaccineOrder.Practitioner.FamilyName.Count != 0 ? VaccineOrder.Practitioner.FamilyName[0] : string.Empty;
                return fullName;
            }
        }
        public string DiseaseOrVaccineName
        {
            get
            {
                if (VaccineOrder.CompositionType == "34108-1")
                    return VaccineOrder.ImmunizationRecommendation.InfectiousDiseaseDisplay;
                else if (VaccineOrder.CompositionType == "11369-6")
                    return VaccineOrder.Immunization.VaccineName;
                else
                    return string.Empty;

            }
        }
        public DateTime Date
        {
            get
            {
                return VaccineOrder.DocumentCreated;
            }
        }
    }
}
