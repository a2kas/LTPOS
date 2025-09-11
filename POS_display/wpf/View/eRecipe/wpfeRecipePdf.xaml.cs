using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace POS_display.wpf.View
{
    /// <summary>
    /// Interaction logic for eRecipePdf.xaml
    /// </summary>
    public partial class wpfeRecipePdf : UserControl
    {
        private const string DateFormat = "yyyy-MM-dd HH:mm";
        public wpfeRecipePdf(Items.eRecipe.Recipe inDataContext)
        {
            InitializeComponent();
            //set DataContext
            try
            {
                if (inDataContext == null || inDataContext.eRecipe_RecipeNumber == 0)
                {
                    grRecipe.Visibility = Visibility.Hidden;
                    return;
                }
                grRecipe.DataContext = inDataContext;
                //tbInfo
                tbInfo.Inlines.Add(Line("Pacientas:"));
                tbInfo.Inlines.Add(new LineBreak());
                tbInfo.Inlines.Add(Line(inDataContext.Patient?.TextName?.FirstOrDefault() ?? inDataContext.eRecipe.PatientName, true));
                tbInfo.Inlines.Add(Line(", Sveikatos istorijos Nr.:"));
                tbInfo.Inlines.Add(Line(inDataContext.Patient?.ESI ?? inDataContext.eRecipe.PatientESI));
                tbInfo.Inlines.Add(Line(", Gim. d.: "));
                DateTime PatientBirthDate = helpers.getXMLDateOnly(inDataContext.Patient?.BirthDate ?? inDataContext.eRecipe.PatientBirthDate);
                int age = DateTime.Now.Year - PatientBirthDate.Year;
                if (DateTime.Now.Month < PatientBirthDate.Month || (DateTime.Now.Month == PatientBirthDate.Month && DateTime.Now.Day < PatientBirthDate.Day))//not had bday this year yet
                    age--;
                tbInfo.Inlines.Add(Line(PatientBirthDate.ToShortDateString() + ", "));
                tbInfo.Inlines.Add(Line(age.ToString() + "m., "));
                tbInfo.Inlines.Add(Line(inDataContext.Patient?.Gender ?? inDataContext.eRecipe.PatientGender));
                tbInfo.Inlines.Add(new LineBreak());
                tbInfo.Inlines.Add(Line("AAGA/SGAS Nr.: "));
                tbInfo.Inlines.Add(Line(inDataContext.eRecipe.AagaSgasNumber));
                tbInfo.Inlines.Add(new LineBreak());
                tbInfo.Inlines.Add(Line("Sveikatos priežiūros specialistas:"));
                tbInfo.Inlines.Add(new LineBreak());
                tbInfo.Inlines.Add(Line(inDataContext.eRecipe.AuthorName, true));
                tbInfo.Inlines.Add(Line(", Spaudo Nr.: "));
                tbInfo.Inlines.Add(Line(inDataContext.eRecipe.AuthorStampCode + ", "));
                tbInfo.Inlines.Add(Line(inDataContext.eRecipe.AuthorQualification));
                tbInfo.Inlines.Add(new LineBreak());
                tbInfo.Inlines.Add(Line("Sveikatos priežiūros įstaiga:"));
                tbInfo.Inlines.Add(new LineBreak());
                tbInfo.Inlines.Add(Line(inDataContext.eRecipe.OrganizationName, true));
                tbInfo.Inlines.Add(Line(", JAR: "));
                tbInfo.Inlines.Add(Line(inDataContext.eRecipe.OrganizationJAR));
                tbInfo.Inlines.Add(Line(", Sveidra ID: "));
                tbInfo.Inlines.Add(Line(inDataContext.eRecipe.OrganizationSVEIDRAID));
                //Recipe data
                txtWrittenDate.Text = inDataContext.eRecipe_DateWritten.ToString("yyyy-MM-dd hh:mm");
                string txt = "";
                if (inDataContext.eRecipe.CompensationTag.ToLower() == "true")
                    txt += (txt != "" ? " | " : "") + "Kompensuojamasis";
                else
                    txtCompensationCode.Text = "nekompensuojama";
                if (inDataContext.eRecipe.ControlledTypeName != "")
                    txt += (txt != "" ? " | " : "") + inDataContext.eRecipe.ControlledTypeName;
                if (inDataContext.eRecipe.ExtemporaneousDescription != "")
                    txt += (txt != "" ? " | " : "") + "Ekstemporalusis";
                if (inDataContext.eRecipe.PrescriptionTagsNominalTag?.ToLower() == "true")
                    txt += (txt != "" ? " | " : "") + "Vardinis";
                if (txt != "")
                {
                    tbRecipeHeader.Inlines.Add(Line(txt));
                    tbRecipeHeader.Inlines.Add(new LineBreak());
                }
                tbRecipeHeader.Inlines.Add(Line("Galioja nuo "));
                tbRecipeHeader.Inlines.Add(Line(inDataContext.eRecipe.ValidFrom, true));
                tbRecipeHeader.Inlines.Add(Line(" iki "));
                tbRecipeHeader.Inlines.Add(Line(inDataContext.eRecipe.ValidTo, true));
                tbRecipeHeader.Inlines.Add(Line(" (" + ((inDataContext.eRecipe_ValidTo - inDataContext.eRecipe_ValidFrom).Days + 1).ToString() + " d.)", true));

                tbMark.Text = inDataContext.eRecipe.UrgencyTag;
                if (inDataContext.eRecipe.PrescriptionTagsFirstPrescribingTag.ToLower() == "true")
                    tbMark.Text += "Pirmas paskyrimas\n";
                if (inDataContext.eRecipe.PrescriptionTagsLongTag.ToLower() == "true")
                    tbMark.Text += "Tęstinis/Ilgalaikis gydymas | išdavimai: " + inDataContext.eRecipe.NumberOfRepeatsAllowed + " | išduota: " + inDataContext.DispenseCount;
                if (inDataContext.eRecipe.PrescriptionTagsNominalTag?.ToLower() == "true" && inDataContext.eRecipe.PrescriptionTagsNominalConfirmTag?.ToLower() == "true")
                    tbMark.Text += "Patvirtinu, jog yra paciento sutikimas vartoti vardinius vaistus ir yra užpildytas vardinio vaisto skyrimo pareiškimas\nPareiškimas galioja iki: " + inDataContext.eRecipe.PrescriptionTagsNominalDeclarationValid;
                if (inDataContext.eRecipe.PrescriptionTagsGkkTag.ToLower() == "true")
                    tbMark.Text += "GKK sprendimas\n";
                if (inDataContext.eRecipe.PrescriptionTagsSpecialTag.ToLower() == "true")
                    tbMark.Text += "Ypatingas paskyrimas\n";
                if (inDataContext.eRecipe.PrescriptionTagsSpecialistDecisionTag.ToLower() == "true")
                    tbMark.Text += "Specialisto sprendimu\n";
                if (inDataContext.eRecipe.PrescriptionTagsLabelingExemptionTag.ToLower() == "true")
                    tbMark.Text += "Ženklinimo išimtis\n";
                if (inDataContext.eRecipe.PrescriptionTagsByDemandTag.ToLower() == "true")
                    tbMark.Text += "Esant poreikiui\n";
                if (inDataContext.eRecipe.ExtemporaneousDescription != "")
                {
                    tbRp.Inlines.Add(Line("Sudėtis (veikliosios ir pagalbinės medžiagos): "));
                    if (inDataContext.eRecipe.ExtemporaneousIngredientContainedMedicationActiveSubstances != "")
                    {
                        tbRp.Inlines.Add(new LineBreak());
                        tbRp.Inlines.Add(Line(inDataContext.eRecipe.ExtemporaneousIngredientContainedMedicationActiveSubstances, true));
                        tbRp.Inlines.Add(new LineBreak());
                        tbRp.Inlines.Add(Line(inDataContext.eRecipe.ExtemporaneousIngredientContainedMedicationStrength, true));
                        tbRp.Inlines.Add(new LineBreak());
                        tbRp.Inlines.Add(Line(inDataContext.eRecipe.ExtemporaneousIngredientContainedMedicationPharmaceuticalForm, true));
                        tbRp.Inlines.Add(new LineBreak());
                        tbRp.Inlines.Add(Line(inDataContext.eRecipe.ExtemporaneousIngredientAmountNumeratorValue + " ", true));
                        tbRp.Inlines.Add(Line(inDataContext.eRecipe.ExtemporaneousIngredientAmountNumeratorCode, true));
                        tbRp.Inlines.Add(Line(" / "));
                        tbRp.Inlines.Add(Line(inDataContext.eRecipe.ExtemporaneousIngredientAmountDenominatorValue + " ", true));
                        tbRp.Inlines.Add(Line(inDataContext.eRecipe.ExtemporaneousIngredientAmountDenominatorCode, true));
                    }
                    tbRp.Inlines.Add(new LineBreak());
                    tbRp.Inlines.Add(new LineBreak());
                    if (inDataContext.eRecipe.ExtemporaneousDescription != "")
                    {
                        tbRp.Inlines.Add(Line(inDataContext.eRecipe.ExtemporaneousDescription, true));
                        tbRp.Inlines.Add(new LineBreak());
                    }
                    if (inDataContext.eRecipe.ExtemporaneousVolumeWeightQuantityValue != "" || inDataContext.eRecipe.ExtemporaneousVolumeWeightQuantityUnits != "")
                    {
                        tbRp.Inlines.Add(Line("Gaminama iki: "));
                        tbRp.Inlines.Add(Line(inDataContext.eRecipe.ExtemporaneousVolumeWeightQuantityValue + " ", true));
                        tbRp.Inlines.Add(Line(inDataContext.eRecipe.ExtemporaneousVolumeWeightQuantityUnits, true));
                        tbRp.Inlines.Add(new LineBreak());
                    }
                    if (inDataContext.eRecipe.PharmaceuticalForm != "")
                    {
                        tbRp.Inlines.Add(Line("Gaminama farmacinė forma: "));
                        tbRp.Inlines.Add(Line(inDataContext.eRecipe.PharmaceuticalForm, true));
                        tbRp.Inlines.Add(new LineBreak());
                    }
                    if (inDataContext.eRecipe.ExtemporaneousMethod != "")
                    {
                        tbRp.Inlines.Add(Line("Gaminimo metodas: "));
                        tbRp.Inlines.Add(Line(inDataContext.eRecipe.ExtemporaneousMethod, true));
                        tbRp.Inlines.Add(new LineBreak());
                    }
                    tbRp.Inlines.Add(Line(inDataContext.eRecipe.QuantityCode == "vnt." ? "Kiekis: " : "Dozuočių skaičius: "));
                    tbRp.Inlines.Add(Line(inDataContext.eRecipe.QuantityValue + " ", true));
                    tbRp.Inlines.Add(Line(inDataContext.eRecipe.QuantityCode, true));
                }
                else
                {
                    if (inDataContext.eRecipe.GenericName != "" || inDataContext.eRecipe.AtcCode != "" || inDataContext.eRecipe.AtcName != "")
                    {
                        tbRp.Inlines.Add(Line(inDataContext.eRecipe.GenericName + " ", true));
                        tbRp.Inlines.Add(Line(inDataContext.eRecipe.AtcCode + " " + inDataContext.eRecipe.AtcName));
                        tbRp.Inlines.Add(new LineBreak());
                    }
                    if (inDataContext.eRecipe.ProprietaryName != "")
                    {
                        tbRp.Inlines.Add(Line(inDataContext.eRecipe.ProprietaryName, true));
                        tbRp.Inlines.Add(new LineBreak());
                    }
                    if (inDataContext.eRecipe.Strength != "")
                    {
                        tbRp.Inlines.Add(Line(inDataContext.eRecipe.Strength, true));
                        tbRp.Inlines.Add(new LineBreak());
                    }
                    if (inDataContext.eRecipe.PharmaceuticalForm != "")
                    {
                        tbRp.Inlines.Add(Line(inDataContext.eRecipe.PharmaceuticalForm, true));
                        tbRp.Inlines.Add(new LineBreak());
                    }
                    tbRp.Inlines.Add(Line(inDataContext.eRecipe.QuantityCode == "vnt." ? "Kiekis: " : "Dozuočių skaičius: "));
                    tbRp.Inlines.Add(Line(inDataContext.eRecipe.QuantityValue + " ", true));
                    tbRp.Inlines.Add(Line(inDataContext.eRecipe.QuantityCode, true));
                    tbRp.Inlines.Add(Line(inDataContext.eRecipe.DispensePackageName, true));
                }

                //Footer
                if (inDataContext.eRecipe.RouteName != "")
                    tbDS.Inlines.Add(Line(inDataContext.eRecipe.RouteName + ", "));
                if (inDataContext.eRecipe.DoseQuantityValue != "" && inDataContext.eRecipe.DoseQuantityCode != "")
                    tbDS.Inlines.Add(Line(inDataContext.eRecipe.DoseQuantityValue + " " + inDataContext.eRecipe.DoseQuantityCode));
                if (inDataContext.eRecipe.ScheduleFrequency != "")
                {
                    var units_str = Session.Params.Where(x => x.system == "ERECIPE" && x.par == inDataContext.eRecipe.ScheduleUnits.ToUpper()).DefaultIfEmpty(new Items.Params() { system = "ERECIPE", par = inDataContext.eRecipe.ScheduleUnits, value = inDataContext.eRecipe.ScheduleUnits }).First();
                    tbDS.Inlines.Add(Line(" x " + inDataContext.eRecipe.ScheduleFrequency + " kart. per " + (units_str.value != "" ? units_str.value : inDataContext.eRecipe.ScheduleUnits)));
                }
                if (inDataContext.eRecipe.TimeQuantity != "")
                    tbDS.Inlines.Add(Line(" kas " + inDataContext.eRecipe.TimeQuantity + " val."));
                if (inDataContext.eRecipe.UsageTagsMorning?.ToLower() == "true")
                    tbDS.Inlines.Add(Line(", ryte"));
                if (inDataContext.eRecipe.UsageTagsNoon?.ToLower() == "true")
                    tbDS.Inlines.Add(Line(", per pietus"));
                if (inDataContext.eRecipe.UsageTagsEvening?.ToLower() == "true")
                    tbDS.Inlines.Add(Line(", vakare"));
                if (inDataContext.eRecipe.UsageTagsBeforeMeal?.ToLower() == "true")
                    tbDS.Inlines.Add(Line(", prieš valgį"));
                if (inDataContext.eRecipe.UsageTagsDuringMeal?.ToLower() == "true")
                    tbDS.Inlines.Add(Line(", valgio metu"));
                if (inDataContext.eRecipe.UsageTagsAfterMeal?.ToLower() == "true")
                    tbDS.Inlines.Add(Line(", po valgio"));
                if (inDataContext.eRecipe.UsageTagsIndependMeal?.ToLower() == "true")
                    tbDS.Inlines.Add(Line(", nepriklausomai nuo valgymo"));
                if (inDataContext.eRecipe.UsageTagsAsNeeded?.ToLower() == "true")
                    tbDS.Inlines.Add(Line(", pagal poreikį"));
                if (inDataContext.eRecipe.UsageTagsBeforeSleep?.ToLower() == "true")
                    tbDS.Inlines.Add(Line(", prieš miegą"));
                if (tbDS.Inlines.Count > 0)
                    tbDS.Inlines.Add(new LineBreak());
                tbDS.Inlines.Add(Line(inDataContext.eRecipe.ExpectedSupplyDurationValue + " "));
                tbDS.Inlines.Add(Line(inDataContext.eRecipe.ExpectedSupplyDurationCode));
                tbLeftDispense.Text = inDataContext.QtyLeftDispense.ToString();
                tbDispensed.Text += $" {inDataContext.DispensedQty.ToString()}";
                tbDurationOfUse.Text += $" {inDataContext.DurationOfUseSum}";
                tbUntilDate.Text += $" {inDataContext.LastDispenseDueDateValue}";
                //Footer
                tbPatientInfo.Inlines.Add(Line(" " + (inDataContext.eRecipe.AdditionalInstructions == "" ? "-" : inDataContext.eRecipe.AdditionalInstructions)));
                tbPharmacistInfo.Inlines.Add(Line(" " + (inDataContext.eRecipe.DispenserNote == "" ? "-" : inDataContext.eRecipe.DispenserNote)));
                if (string.IsNullOrEmpty(inDataContext.eRecipe.LockWho))
                    tbRecipeLockInfo.Visibility = Visibility.Hidden;
                else
                {
                    var lockWhen = string.IsNullOrEmpty(inDataContext.eRecipe.LockWhen) ? "-" :
                        Convert.ToDateTime(inDataContext.eRecipe.LockWhen).ToString(DateFormat);
                    tbRecipeLockInfo.Inlines.Add(Line(" " + lockWhen));
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
