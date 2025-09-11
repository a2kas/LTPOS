namespace POS_display.Models.Recipe
{
    public class UpdateErecipeModel
    {
        public decimal Id { get; set; }

        public decimal CompositionId { get; set; }

        public string CompositionRef { get; set; }

        public string CompositionStatus { get; set; }

        public decimal MedicationDispenseId { get; set; }

        public string MedicationDispenseStatus { get; set; }

        public decimal Active { get; set; }

        public int Confirmed { get; set; }

        public string DocumentStatus { get; set; }

        public string Info { get; set; }
    }
}