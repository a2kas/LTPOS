using POS_display.Models.Kvap;
using POS_display.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace POS_display
{
    public partial class check_recipe_kvr : Form
    {
        private bool formWaiting = false;
        private readonly decimal recipeNo;
        private readonly string doctorNo;
        private readonly DateTime salesDate;
        private readonly DateTime recipeDate;
        private readonly string personalCode;
        private readonly string npakId7;
        private readonly bool eRecipe;
        private List<validation> _validations;
        private bool _isSuccessfulCheck;


        public enum validation { KvrCheck, RecipeUnused }
        public List<RecipeError> RecipeErrors { get; set; }

        public bool IsSuccessfulCheck 
        {
            get { return _isSuccessfulCheck; }
        }

        public check_recipe_kvr(decimal recipe_no, string doctor_no, DateTime sales_date, DateTime recipe_date, bool e_recipe, List<validation> validations, string npak_id7 = null, string personal_code = null)
        {
            InitializeComponent();
            recipeNo = recipe_no;
            doctorNo = doctor_no;
            salesDate = sales_date;
            recipeDate = recipe_date;
            npakId7 = npak_id7;
            personalCode = personal_code;
            eRecipe = e_recipe;
            _validations = validations;
            RecipeErrors = new List<RecipeError>();
            _isSuccessfulCheck = false;
        }

        private void check_recipe_kvr_Load(object sender, EventArgs e)
        {
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            DB.POS.UpdateSession("Recepto duomenų patikrinimas pas TLK ", 2);
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
        }

        private void check_recipe_kvr_Closing(object sender, FormClosingEventArgs e)
        {
            if (this.formWaiting == true)
                e.Cancel = true;
        }

        private void form_wait(bool wait)
        {
            if (wait == true)
            {
                this.UseWaitCursor = true;
                this.Cursor = Cursors.WaitCursor;
                this.formWaiting = true;
            }
            else
            {
                this.UseWaitCursor = false;
                this.Cursor = Cursors.Default;
                this.formWaiting = false;
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private async void btnRefresh_Click(object sender, EventArgs e)
        {
            await ValidateRecipe();
        }

        public async Task ValidateRecipe()
        {
            if (formWaiting)
                return;
            try
            {
                form_wait(true);
                
                rtbRecipeNo.Text = "";
                rtbDoctorNo.Text = "";
                rtbError.Text = "";
                lblText1.Text = "Užklausa pateikta: " + DateTime.Now.ToString();
                lblText2.Text = "";
                if (_validations.Contains(validation.KvrCheck) && !eRecipe)
                {
                    var checkRecipe = await Session.KVAP.CheckRecipe(recipeNo.ToString(), doctorNo, salesDate.Date, recipeDate.Date);
                    RecipeErrors.AddRange(checkRecipe.RecipeErrors);
                }
                if (_validations.Contains(validation.RecipeUnused) && !string.IsNullOrWhiteSpace(npakId7) && !string.IsNullOrWhiteSpace(personalCode))
                {
                    var checkRecipe = await Session.KVAP.CheckIfRecipeUnused(npakId7, personalCode, DateTime.Now);
                    RecipeErrors.AddRange(checkRecipe.RecipeErrors);
                }
                rtbRecipeNo.Text = "\n\n\n\n\n\n" + recipeNo + "\n\n\n";
                rtbDoctorNo.Text = "\n\n\n\n\n\n" + doctorNo + "\n\n\n";
                foreach (var error in RecipeErrors)
                {
                    rtbError.Text += "\n" + error.CriticalMean;
                    rtbError.Text += "\n" + error.Name;
                    rtbError.Text += "\n" + error.Notes;
                    rtbError.Text += "\n";
                }
                lblText2.Text = "Atsakymas gautas: " + DateTime.Now.ToString();
                _isSuccessfulCheck = true;
            }
            catch (Exception ex)
            {
                helpers.alert(Enumerator.alert.error, ex.InnerException?.Message ?? ex.Message);
                lblText2.Text = "Atsakymas negautas!";
            }
            form_wait(false);
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            if (helpers.alert(Enumerator.alert.confirm, "Yra TLK klaidų, dėl kurių receptas gali būti nekompensuojamas.\nAr tikrai norite jas ignoruoti ir tęsti pardavimą?", true))
            {
                RecipeErrors.Clear();
                this.DialogResult = DialogResult.Cancel;
            }
        }
    }
}
