using TamroUtilities.HL7.Models;
using TamroUtilities.HL7.Models.Encounter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;

namespace POS_display.wpf.ViewModel
{
    public class wpfeRecipeListViewModel : BaseViewModel
    {
        public ICommand FindCommand { get; set; }
        public ICommand SellCommand { get; set; }
        public ICommand ReserveCommand { get; set; }
        public ICommand SuspendCommand { get; set; }
        public ICommand GetDispensePdfCommand { get; set; }
        public ICommand GetRecipePdfCommand { get; set; }
        public wpfeRecipeListViewModel()
        {
            FindCommand = new BaseCommand(Find);
            SellCommand = new BaseCommand(Sell);
            ReserveCommand = new BaseCommand(Reserve);
            SuspendCommand = new BaseCommand(Suspend);
            GetDispensePdfCommand = new BaseCommand(GetDispensePdf);
            GetRecipePdfCommand = new BaseCommand(GetRecipePdf);
        }

        #region Commands
        private async void Find(object obj)
        {
            var erecipe_item = new Items.eRecipe.Recipe();
            //nebeuzdarinejam encounterio
            /*if (EncounterItem.Id != "" && (PractitionerItem.RoleId == 6 || PractitionerItem.RoleId == 7))
                Session.eRecipeUtils.CloseEncounter(EncounterItem.Id, DB.eRecipe.getEncounterStatus(helpers.getDecimal(EncounterItem.Id)), CloseEncounter_cb);
            else*/
            erecipe_item.Patient = await Session.eRecipeUtils.GetPatient<PatientDto>(PersonalCode, PickedForUserId);
            erecipe_item.HasLowIncome = await Session.eRecipeUtils.GetLowIncome<LowIncomeDto>(eRecipeItem.Patient.PersonalCode);

            eRecipeItem = erecipe_item;//update view
            PickedForUserId = "";
            //Session.eRecipeUtils.GetRepresentedPersons(eRecipeItem.Patient.PatientId, GetRepresentedPersons_cb);
            if (Session.eRecipeEncounterList == null)
                Session.eRecipeEncounterList = new List<Items.eRecipe.Encounter>();
            var valid_encounter = Session.eRecipeEncounterList.FirstOrDefault(w => w.PatientId == erecipe_item.Patient.PatientId);
            if (valid_encounter == null)
            {
                eRecipeItem.Encounter = await Session.eRecipeUtils.CreateEncounter(erecipe_item.Patient.PatientId,
                    erecipe_item.Patient.PatientRef,
                    Session.PractitionerItem.PractitionerRef,
                    Session.PractitionerItem.OrganizationRef,
                    string.Empty,
                    null,
                    EncounterReason.Prescription);
            }
            else
                eRecipeItem.Encounter = valid_encounter.EncounterItem;
        }

        private void Sell(object obj)
        {
        }
        private void Reserve(object obj)
        {
        }
        private void Suspend(object obj)
        {
        }
        private void GetDispensePdf(object obj)
        {
        }
        private void GetRecipePdf(object obj)
        {
        }
        #endregion

        #region Variables
        private string _PersonalCode;
        public string PersonalCode
        {
            get { return _PersonalCode; }
            set { _PersonalCode = value; NotifyPropertyChanged("PersonalCode"); }
        }
        public string PickedForUserId { get; set; }
        private Items.eRecipe.Recipe _eRecipeItem;
        public Items.eRecipe.Recipe eRecipeItem
        {
            get
            {
                return _eRecipeItem;
            }

            set
            {
                _eRecipeItem = value;
                if (_eRecipeItem?.Patient != null)
                {
                    NotifyPropertyChanged("PatientName");
                    NotifyPropertyChanged("Age");
                    NotifyPropertyChanged("BirthDate");
                    NotifyPropertyChanged("Gender");
                    NotifyPropertyChanged("RelatedPersons");
                }
                //gbType.Enabled = true;
            }
        }

        public object RelatedPersons
        {
            get
            {
                var cmb = (from el in eRecipeItem.Patient.RelatedPersons
                           select new
                           {
                               Name = el.GivenName + " " + el.FamilyName,
                               Reference = el.Reference
                           })?.DefaultIfEmpty().ToList();
                cmb.Insert(0, new { Name = "Susiję asmenys:", Reference = "" });
                return cmb;
            }
        }

        public string Age
        {
            get
            {
                string Age;
                DateTime birth_date = helpers.getXMLDateOnly(_eRecipeItem.Patient.BirthDate);
                DateTime now = DateTime.Now;
                int age = now.Year - birth_date.Year;
                if (now.Month < birth_date.Month || (now.Month == birth_date.Month && now.Day < birth_date.Day))//not had bday this year yet
                    age--;
                Age = age.ToString();
                return Age;
            }
        }

        public string BirthDate
        {
            get
            {
                DateTime birth_date = helpers.getXMLDateOnly(_eRecipeItem.Patient.BirthDate);
                return birth_date.ToString("yyyy-MM-dd");
            }
        }
        
        private string _PatientName;
        public string PatientName
        {
            get
            {
                return _eRecipeItem.Patient.GivenName.First() + " " + _eRecipeItem.Patient?.FamilyName?.First();
            }
        }

        private AllergyIntoleranceDto _Allergies;
        public AllergyIntoleranceDto Allergies
        {
            get { return _Allergies; }
            set { _Allergies = value; NotifyPropertyChanged("Allergies"); }
        }
        public string Allergies_str
        {
            get
            {
                string res = "";
                foreach (var el in Allergies.Allergies)
                {
                    res += el.Code + " " + el.Name + " | " + el.Description + "\n";
                }
                return res;
            }
        }

        private RepresentedPersonsDto _RepresentedPersons;
        public RepresentedPersonsDto RepresentedPersons
        {
            get
            {
                return _RepresentedPersons;
            }
            set
            {
                _RepresentedPersons = value;
                NotifyPropertyChanged("RepresentedPersons"); 
                //if (_RepresentedPersons?.Persons != null)
                //{
                //    var RepresentedIems = (from el in _RepresentedPersons.Persons
                //                           select new
                //                           {
                //                               PersonalCode = el.PersonalCode,
                //                               PatientRef = el.PatientRef,
                //                               Name = el.GivenName?.First() + " " + el.FamilyName?.First()
                //                           }).ToList();
                //    RepresentedIems.Insert(0, new { PersonalCode = "", PatientRef = "", Name = "Įgaliojusieji asmenys:" });
                //    cmbRepresented.DataSource = RepresentedIems;
                //    cmbRepresented.DisplayMember = "Name";
                //    cmbRepresented.ValueMember = "PersonalCode";
                //}
            }
        }

        private RecipeListDto _RecipeList;
        public RecipeListDto RecipeList
        {
            get
            {
                return _RecipeList;
            }
            set
            {
                _RecipeList = value;
                NotifyPropertyChanged("RecipeList");
                //cbSelAll.Checked = false;
                //selectedRow = 0;
                //int count = int.Parse(_RecipeList?.Total ?? "0");
                //if (count < gridSize)
                //    count = gridSize;
                //RecipeListNavigation.PageCount = (int)Math.Ceiling((decimal)count / gridSize);
                //RecipeListNavigation.PageIndex = RecipeListNavigation.PageIndex;//refresh number
                //if (_RecipeList.RecipeList.Count > 0)
                //{
                //    form_wait(false);//reikia kad recepto info parodytu
                //    gvRecipeList.DataSource = _RecipeList.RecipeList.OrderBy(el => el.Status == "active" ? 1 : el.Status == "onhold" ? 2 : el.Status == "completed" ? 3 : 4).ToList();
                //    GridView_order();
                //    GridView_color();
                //    RecipeList_enableButtons();
                //}
                //else
                //    helpers.alert(Enumerator.alert.warning, "Pacientas neturi išrašytų el. receptų");
            }
        }
        #endregion
    }
}
