using POS_display.Repository.Recipe;
using POS_display.wpf;
using System;
using System.Linq;
using System.Windows.Forms;

namespace POS_display
{
    public partial class ucRecipeEditBase : UserControl
    {
        private readonly IRecipeRepository _recipeRepository;

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == (Keys.Enter))
            {
                SendKeys.Send("{TAB}");
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }
        //input variables
        internal Items.posd in_posd;
        internal string in_PickedUpByRef;
        internal Items.eRecipe.Recipe in_erecipe;
        internal bool formWaiting = false;
        //output variables
        public string out_form_action = "";
        public ucRecipeEditBase()
        {
            InitializeComponent();
            _recipeRepository = new RecipeRepository();
        }

        internal void form_wait(bool wait)
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

        internal void CloseWindow(DialogResult result)
        {
            switch (result)
            {
                case DialogResult.OK:
                    out_form_action = "recipe_saved";
                    break;
                case DialogResult.Cancel:
                    out_form_action = "recipe_closed";
                    break;
            }
            if (in_erecipe != null)
                this.Parent.Controls.Remove(this);
            else
                this.ParentForm.DialogResult = result;
        }

        internal virtual void tbNumber_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (this.formWaiting == true)
            {
                e.Handled = true;
                return;
            }
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
                e.Handled = true;
        }

        internal virtual void tbString_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (this.formWaiting == true)
            {
                e.Handled = true;
                return;
            }
        }

        internal virtual void tbMoney_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (this.formWaiting == true)
            {
                e.Handled = true;
                return;
            }
            helpers.tb_KeyPress(sender, e);
        }

        internal virtual void tbQty_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (this.formWaiting == true)
            {
                e.Handled = true;
                return;
            }
            TextBox tb = (sender as TextBox);
            if (e.KeyChar == '.')
                e.KeyChar = ',';
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != ',' && e.KeyChar != '-')
                e.Handled = true;
            if ((e.KeyChar == ',') && (tb.Text.IndexOf(',') > -1))
                e.Handled = true;
            if ((e.KeyChar == ',') && (tb.Text.Length == 0))
            {
                tb.Text = "0,";
                tb.Select(tb.Text.Length, tb.Text.Length);
                e.Handled = true;
            }
        }

        public void CheckValues(IRecipeEdit view)
        {
            var compensationDate = _recipeRepository.GetMedicationCompensationDateByNpakid7(in_erecipe?.Medication?.NPAKID7?.ToString());
            if (compensationDate != null && in_erecipe.eRecipe_CompensationTag)
            {
                helpers.alert(Enumerator.alert.warning,
                    $"Vaistas bus kompensuojamas TIK jei gydymas pradėtas iki {compensationDate.Value.ToString("yyyy-MM-dd")}\nPASITIKRINKITE!!!");
            }
            if (helpers.betweenday(view.ValidFrom, view.SalesDate) > view.RecipeValid - 1 && Session.getParam("ERECIPE", "V2") == "0")
                throw new ArgumentException("Receptas negalioja!!!");
            if (helpers.betweenday2(view.SalesDate.Date, view.ValidFrom.Date) > 0)
                throw new ArgumentException("Receptas negalioja, per anksti!!!");
            if (helpers.betweenday2(view.RecipeDate.Date, view.ValidFrom.Date) > 13 && view.Ext == 0)
                throw new ArgumentException("Receptas negalioja, gydytojo klaida?");
            if (view.Ext == 1 && helpers.betweenday(view.PastTillDate, DateTime.Now) >= 5 && view.PastTillDate > DateTime.Now)
                throw new ArgumentException("Vaistas nebaigtas vartoti! Per anksti bandoma išduoti vaistą, kurio praėjusio išdavimo užtenka dar daugiau nei 5 dienoms");
        }

        protected string ResolveStatusByDispensedQuantity(decimal quantity)
        {
            var recipeQty = in_erecipe.eRecipe.QuantityValue.ToDecimal();
            var totalDispensedQty = in_erecipe.DispensedQty + quantity;
            var remainingPercentage = 100 - ((totalDispensedQty * 100) / recipeQty);

            if (remainingPercentage > 10)
            {
                return "active";
            }

            bool isMPP = string.IsNullOrEmpty(in_erecipe.Medication.MedicationGroup);

            if (isMPP)
            {
                return (quantity > recipeQty) ?  string.Empty : "completed";
            }

            return (Math.Abs(remainingPercentage) < 15) ? "completed" : string.Empty;
        }
    }
}
