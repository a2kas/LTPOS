using System;
using System.Windows.Forms;

namespace POS_display.Views.PersonalPharmacist
{
    public interface IPersonalPharmacistView
    {
        TextBox ClientPersonalCode { get; set; }
        DateTime DueDate { get; set; }
        TextBox DoctorID { get; set; }
        TextBox DoctorName { get; set; }
        TextBox DoctorSurename { get; set; }
        ComboBox Hospital { get; set; }
        Button Apply { get; set; }
        Button Cancel { get; set; }

    }
}
