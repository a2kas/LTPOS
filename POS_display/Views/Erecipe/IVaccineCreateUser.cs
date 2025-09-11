using TamroUtilities.HL7.Models;

namespace POS_display.Views.ERecipe
{
    public interface IVaccineCreateUser
    {
        string InfectiousDisease { set; get; }
        string InfectiousDiseaseCode { get; }
        string Medicine { set; get; }
        string DoseNumber { set; get; }
        string Notes { set; get; }
        string NPAKID7 { set; get; }
        PatientDto Patient { set; get; }
        LowIncomeDto LowIncome { set; get; }
        Models.Barcode BarcodeModel { get; }
        void RaiseCloseEvent(object data, Enumerator.VaccineOrderEvent voe);
    }
}
