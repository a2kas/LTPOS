using TamroUtilities.HL7.Models;

namespace POS_display.Items.eRecipe
{
    public class Encounter
    {
        public string PatientId { get; set; }
        public EncounterDto EncounterItem { get; set; }
    }
}
