using POS_display.Items.eRecipe;
using POS_display.Items.Prices;
using System.Windows.Forms;

namespace POS_display.Views.Erecipe.PaperRecipe
{
    public interface IPaperRecipeBaseView
    {
        bool IsFieldValueChanging { get; set; }

        DateTimePicker CreationDate { get; set; }
        DateTimePicker ExpiryStart { get; set; }
        TextBox ValidityPeriod { get; set; }
        DateTimePicker ExpiryEnd { get; set; }
        TextBox DoctorCode { get; set; }
        Label DoctorCodeLabel { get; set; }
        TextBox Stamp { get; set; }
        Label StampLabel { get; set; }
        TextBox SveidraID { get; set; }
        Label SveidraIDLabel { get; set; }
        TextBox InfoForSpecialist { get; set; }
        CheckBox SpecialPrescription { get; set; }
        CheckBox SpecialistDecision { get; set; }
        CheckBox LabelingExemption { get; set; }
        CheckBox GKDecision { get; set; }
        CheckBox CertainName { get; set; }
        CheckBox BrandName { get; set; }
        ComboBox NominalConfirm { get; set; }
        TextBox Medication { get; set; }
        TextBox PrescriptionDosesAmount { get; set; }
        ComboBox PharmaceuticalForm { get; set; }
        ComboBox Route { get; set; }
        TextBox OneTimeDose { get; set; }
        ComboBox PharmaceuticalFormMeasureUnit { get; set; }
        TextBox TimesPer { get; set; }
        ComboBox TimesPerSelection { get; set; }
        CheckBox InMorning { get; set; }
        CheckBox DuringLunch { get; set; }
        CheckBox InEvening { get; set; }
        CheckBox AsNeeded { get; set; }
        TextBox DailyDose { get; set; }
        CheckBox BeforeEating { get; set; }
        CheckBox AfterEating { get; set; }
        CheckBox DuringEating { get; set; }
        CheckBox RegardlessEating { get; set; }
        CheckBox BeforeSleeping { get; set; }
        DateTimePicker SalesDate { get; set; }
        DateTimePicker NominalDeclarationValid { get; set; }
        TextBox DispensedMedication { get; set; }
        TextBox DipsenseDosesAmount { get; set; }
        TextBox AmountPerDay { get; set; }
        TextBox AmountOfDays { get; set; }
        DateTimePicker EnoughUntil { get; set; }
        TextBox Price { get; set; }
        Button NextFirst { get; set; }
        Button NextSecond { get; set; }
        GenericItem MedicationItem { get; set; }
        Recipe eRecipeItem { get; set; }  
        Items.posd PosDetail { get; set; }

    }
}
