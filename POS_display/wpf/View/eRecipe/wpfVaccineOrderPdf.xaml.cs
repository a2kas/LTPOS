using Qas.EHealth.Models;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;

namespace POS_display.wpf.View.eRecipe
{
    /// <summary>
    /// Interaction logic for wpfVaccineOrderPdf.xaml
    /// </summary>
    public partial class wpfVaccinationOrderPdf : UserControl
    {
        public wpfVaccinationOrderPdf(VaccineOrderDto orderDTO)
        {
            InitializeComponent();
            try
            {
                if (orderDTO == null || orderDTO.OrderId == null)
                {
                    grVaccinationOrder.Visibility = Visibility.Hidden;
                    return;
                }

                grVaccinationOrder.DataContext = orderDTO;
                // Created data
                txtCreatedDate.Text = orderDTO.Date.ToString("yyyy-MM-dd");

                tbInfo.Inlines.Add(Line("Pacientas:"));
                tbInfo.Inlines.Add(new LineBreak());
                tbInfo.Inlines.Add(Line(orderDTO.Patient?.TextName?.FirstOrDefault(), true));
                tbInfo.Inlines.Add(Line(", Sveikatos istorijos Nr.:"));
                tbInfo.Inlines.Add(Line(orderDTO.Patient?.ESI));
                tbInfo.Inlines.Add(Line(", Gim. d.: "));
                DateTime PatientBirthDate = helpers.getXMLDateOnly(orderDTO.Patient?.BirthDate);
                int age = DateTime.Now.Year - PatientBirthDate.Year;
                if (DateTime.Now.Month < PatientBirthDate.Month || (DateTime.Now.Month == PatientBirthDate.Month && DateTime.Now.Day < PatientBirthDate.Day))//not had bday this year yet
                    age--;
                tbInfo.Inlines.Add(Line(PatientBirthDate.ToShortDateString() + ", "));
                tbInfo.Inlines.Add(Line(age.ToString() + "m., "));
                tbInfo.Inlines.Add(Line(orderDTO.Patient?.Gender));
                tbInfo.Inlines.Add(new LineBreak());
                tbInfo.Inlines.Add(Line("Paskyrimą sukuręs specialistas:"));
                tbInfo.Inlines.Add(new LineBreak());
                tbInfo.Inlines.Add(Line($"{orderDTO.Practitioner.GivenName[0]} {orderDTO.Practitioner.FamilyName[0]}", true));
                tbInfo.Inlines.Add(Line(", Spaudo Nr.: "));
                tbInfo.Inlines.Add(Line(orderDTO.Practitioner.StampCode+ ", "));
                tbInfo.Inlines.Add(Line(String.Join(",",orderDTO.Practitioner.Qualification.Select(e=>e.QualificationDisplay).ToList())));
                tbInfo.Inlines.Add(new LineBreak());
                tbInfo.Inlines.Add(Line("Paskyrimą sukūrusio specialisto įstaiga:"));
                tbInfo.Inlines.Add(new LineBreak());
                tbInfo.Inlines.Add(Line(orderDTO.Practitioner.OrganizationName, true));
                tbInfo.Inlines.Add(Line(", JAR: "));
                tbInfo.Inlines.Add(Line(orderDTO.Practitioner.OrganizationJAR));
                tbInfo.Inlines.Add(Line(", Sveidra ID: "));
                tbInfo.Inlines.Add(Line(orderDTO.Practitioner.OrganizationSVEIDRAID));

                if (orderDTO.Immunization == null || string.IsNullOrEmpty(orderDTO.Immunization.ImmunizationId) || orderDTO.CompositionType == "34108-1") 
                {
                    for(int i = 11; i < grVaccinationOrder.RowDefinitions.Count; i++) 
                    {
                        grVaccinationOrder.RowDefinitions[i].Height = new GridLength(0);
                    }
                }
            }
            catch (Exception ex)
            {
                helpers.alert(Enumerator.alert.error, ex.Message);
            }
        }
        private Run Line(string txt, bool Bold = false)
        {
            Run r = new Run();
            r.Text = txt;
            if (Bold)
                r.FontWeight = FontWeights.Bold;

            return r;
        }

        public void form_wait(bool wait)
        {
            if (wait == true)
                this.Cursor = Cursors.Wait;
            else
                this.Cursor = Cursors.Arrow;
        }
    }
}
