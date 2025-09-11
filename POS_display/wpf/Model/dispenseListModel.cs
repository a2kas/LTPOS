using TamroUtilities.HL7.Models;

namespace POS_display.wpf.Model
{
    public class dispenseListModel
    {
        public dispenseListModel(DispenseDto dto)
        {
            selected = false;
            Dispense = dto;
        }

        public DispenseDto Dispense { get; set; }
        public bool selected { get; set; }
        public string CompositionId
        {
            get
            {
                return Dispense.CompositionId;
            }
        }
        public string AuthorName
        {
            get
            {
                return Dispense.AuthorName;
            }
        }
        public string ProprietaryName
        {
            get
            {
                var name = "";
                if (Dispense.ProprietaryName != "")
                    name += Dispense.ProprietaryName + ", ";
                if (Dispense.Description != "")
                    name += Dispense.Description + ", ";
                if (Dispense.GenericName != "")
                    name += Dispense.GenericName + ", ";
                if (Dispense.Strength != "")
                    name += Dispense.Strength + ", ";
                if (Dispense.PharmaceuticalForm != "")
                    name += Dispense.PharmaceuticalForm + ", ";
                if (name.Contains(","))
                    return name.Substring(0, name.LastIndexOf(','));
                else return name;
            }
        }
        public string OrganizationName
        {
            get
            {
                return
                    Dispense.OrganizationName;
            }
        }

        public string DateWritten
        {
            get
            {
                return
                    Dispense.DateWritten;
            }
        }

        public string DateDueDate
        {
            get
            {
                return
                    Dispense.DueDate;
            }
        }

        public string QuantityValue
        {
            get
            {
                return Dispense.QuantityValue;
            }
        }
    }
}
