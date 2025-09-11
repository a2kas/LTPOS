using System;
using System.Threading.Tasks;

namespace POS_display.Views.Erecipe.Dispense
{
    public interface IDispenseEditView
    {
        string SaleDate { get; set; }
        string MedicationValidUntil { get; set; }
        string DayCount { get; set; }
        string IssuedQuantity { get; set; }
        string SalePrice { get; set; }
        string ReimbursedAmount { get; set; }
        string AdditionalReimbursedAmount { get; set; }
        string PatientAmount { get; set; }
        string EditReason { get; set; }

        event EventHandler DayCountChanged;
        event EventHandler MedicationValidUntilChanged;
        event EventHandler SalePriceChanged;
        event EventHandler PatientAmountChanged;
        event EventHandler ReimbursedAmountChanged;

        Task Init(string compositionId);
    }
}