using POS_display.WR_KVAP;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;

namespace POS_display
{
    public partial class send_recipe_kvr : Form
    {
        private bool formWaiting = false;

        private DialogResult dialog_result = DialogResult.Cancel;
        //input variables
        private decimal recipeId = 0;

        public send_recipe_kvr(decimal recipe_id)
        {
            recipeId = recipe_id;
            InitializeComponent();
        }

        private void send_recipe_kvr_Load(object sender, EventArgs e)
        {
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            DB.POS.UpdateSession("Recepto duomenų patikrinimas pas TLK ", 2);
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            btnRefresh_Click(null, null);
        }

        private void send_recipe_kvr_Closing(object sender, FormClosingEventArgs e)
        {
            if (this.formWaiting == true)
                e.Cancel = true;
            this.DialogResult = dialog_result;
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
            this.Close();
        }

        private async void btnRefresh_Click(object sender, EventArgs e)
        {
            try
            {
                if (formWaiting == true || recipeId <= 0)
                    return;
                form_wait(true);
                tbRecipeNo.Text = "";
                tbStatus.Text = "";
                rtbError.Text = "";
                lblText1.Text = "Užklausa pateikta: " + DateTime.Now.ToString();
                lblText2.Text = "";
                await DB.recipe.UpdateTLKStatus(recipeId, 2);
                var dt = await DB.recipe.asyncGetRecipeKVR(recipeId);
                if (dt.Rows.Count <= 0)
                    throw new System.Exception("");
                var submitKVRCreateModel = new submitKVRCreateKV_RECEPTAS
                {
                    PADALINYS_IST_ID = Session.RecipeParm.tlk_id,
                    VAISTININKO_SPAUDO_ID = dt.Rows[0]["kvr_pharmacist_id"].ToString(),
                    NUMERIS = dt.Rows[0]["kvpdoctorno"].ToString() == "E" ? helpers.toXMLNumber(dt.Rows[0]["composition_id"].ToString()) : helpers.toXMLNumber(dt.Rows[0]["recipeno"].ToString()),
                    KVREC_TYPE = dt.Rows[0]["kvpdoctorno"].ToString() == "E" ? "E" : "K",
                    GTPL = dt.Rows[0]["kvpdoctorno"].ToString() == "E" ? "" : dt.Rows[0]["kvpdoctorno"].ToString(),
                    VAISTO_ID = dt.Rows[0]["tlkid"].ToString(),
                    VAISTO_KIEKIS = dt.Rows[0]["qtyrep"].ToDecimal(),
                    PAROS_DOZE = dt.Rows[0]["qtyday"].ToDecimal(),
                    KOMP_KODAS = dt.Rows[0]["compensationcode"].ToString(),
                    LIG_KODAS = dt.Rows[0]["deseasecode"].ToString(),
                    RECEPTO_ISRASYMO_DATA = dt.Rows[0]["recipedate"].ToDateTime(),
                    GALIOJIMO_PRADZIA = dt.Rows[0]["valid_from"].ToDateTime(),
                    GALIOJIMO_PABAIGA = dt.Rows[0]["valid_till"].ToDateTime(),
                    VAISTO_ISDAVIMO_DATA = dt.Rows[0]["salesdate"].ToDateTime(),
                    VAISTO_PAKANKA_IKI = DateTime.Parse(dt.Rows[0]["till_date"].ToString()),
                    VARTOJIMO_DIENU_SKAICIUS = dt.Rows[0]["countdayrep"].ToDecimal(),
                    PARDAVIMO_SUMA = dt.Rows[0]["totalsum"].ToDecimal(),
                    KOMPENSUOJAMA_SUMA = dt.Rows[0]["compensationsum"].ToDecimal(),
                    PACIENTO_PRIEMOKA = dt.Rows[0]["paysum"].ToDecimal(),
                    AAGA_ISAS_NUMERIS = dt.Rows[0]["aaga_isas"].ToString(),
                    TESTINIS_GYDYMAS = dt.Rows[0]["ext"].ToDecimal() == 1 ? "<TESTINIS_GYDYMAS>T</TESTINIS_GYDYMAS>" : "",
                    IRASO_PINIGU_SUMU_VALIUTA = "EUR",
                    PADENG_PRIEMOKA = dt.Rows[0]["prepayment_compensation"].ToDecimal(),
                    PPP_LENGVATA = helpers.toXMLNumber(dt.Rows[0]["is_prepayment_compensation"].ToString())
                };
                var recipe = await Session.KVAP.SendRecipe(submitKVRCreateModel);
                if (recipe == null)
                    throw new System.Exception("Klaida!");


                lblText2.Text = "Atsakymas gautas: " + DateTime.Now.ToString();
                tbRecipeNo.Text = submitKVRCreateModel.NUMERIS;
                string errorsAggregated = "";
                foreach (var error in recipe.RecipeErrors)
                {
                    errorsAggregated += $"{error.CriticalMean} - {error.Name} - {error.Notes}\n";
                    rtbError.Text += error.Name + "\n\n";
                    rtbError.Text += error.Notes + "\n";
                    rtbError.Text += "-------------------------------------------------------------------------------------------------\n";
                }

                if (recipe.Id > 0)//jei gaunam rec id is tlk
                {
                    await DB.recipe.UpdateTLKStatus(recipeId, 1);
                    await DB.recipe.asyncInsertKVAPrecipe(recipe.Id, recipe.Number, recipe.Gtpl, recipe.DrugId,
                                                    recipe.CompensationSum, recipe.PrepaymentSum, recipe.SaleSum,
                                                    errorsAggregated, recipe.Status, recipe.PayedSum);
                    tbStatus.Text = "Receptas sėkmingai priduotas į TLK!\n";
                    dialog_result = DialogResult.OK;
                }
                else
                {
                    tbStatus.Text = "Recepto priduoti į TLK nepavyko!";
                    await DB.recipe.UpdateTLKStatus(recipeId, 0);
                }
                if (dialog_result == DialogResult.OK)
                    btnRefresh.Enabled = false;
                form_wait(false);
            }
            catch (System.Exception ex)
            {
                await DB.recipe.UpdateTLKStatus(recipeId, 0);
                form_wait(false);
                lblText2.Text = "Atsakymas negautas!";
                rtbError.Text = ex.Message;
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (formWaiting == true || recipeId <= 0)
                return;
            //todo
        }
    }
}
