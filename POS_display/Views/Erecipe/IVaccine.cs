using System.Collections.Generic;
using eRecipeWS.DTO;

namespace POS_display.Views.ERecipe
{
    public interface IVaccine
    {
        string PersonalCode { get; set; }
        string PatientName { get; set; }
        string PatientSurname { get; set; }
        string PatientBirthDate { get; set; }
        bool IsActiveSellVaccine { get; set; }
        bool IsActiveCreateVaccine { get; set; }
        bool IsActiveCancelVaccine { get; set; }
        VaccineOrderDto SelectedOrderItem { get; set; }
        VaccineOrderListDto VaccineOrderList { get; set; }
        PatientDto Patient { get; set; }
        int PageIndex { get; }
    }
}
