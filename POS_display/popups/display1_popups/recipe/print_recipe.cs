using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace POS_display
{
    public partial class print_recipe : Form
    {
        private bool formWaiting = false;
        private string caller = "";
        //input variables
        public decimal recipeId = 0;

        public print_recipe()
        {
            InitializeComponent();
        }

        private async void print_recipe_Load(object sender, EventArgs e)
        {
            form_wait(true);
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            DB.POS.UpdateSession("Recepto spausdinimas ", 2);
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed

            var recipe_data = await DB.recipe.asyncSearchRecipe(recipeId);
            if (recipe_data?.Rows?.Count > 0)
            {
                decimal qty = 0;
                qty = recipe_data.Rows[0]["qty"].ToDecimal();
                richTextBox1.Text += "\n";
                richTextBox1.Text += "Recepto Nr.: \t\t" + recipe_data.Rows[0]["row_no"].ToString() + "\n";
                richTextBox1.Text += "\n";
                richTextBox1.Text += "10. " + recipe_data.Rows[0]["productname"].ToString() + "\n";
                richTextBox1.Text += "11. Pakanka iki:   \t" + recipe_data.Rows[0]["till_date"].ToString().Substring(0, 10) + "\n";
                decimal sales_price = Math.Round(recipe_data.Rows[0]["salesprice"].ToDecimal() * qty, 2);
                richTextBox1.Text += "12. Vaisto kaina   \t= " + sales_price.ToString() + "\n";
                decimal basic_price = basic_price = Math.Round(recipe_data.Rows[0]["basicprice"].ToDecimal() * qty, 2);
                richTextBox1.Text += "       Bazinė kaina  \t= " + basic_price.ToString() + "\n";
                richTextBox1.Text += "13. Pacientas sum. \t= " + recipe_data.Rows[0]["paysum"].ToString() + "\n";
                richTextBox1.Text += "14. Kompensuojama \t= " + recipe_data.Rows[0]["compensationsum"].ToString() + " (" + recipe_data.Rows[0]["comppercent"].ToString() + "%)" + "\n";
                richTextBox1.Text += "\n";
                richTextBox1.Text += "Išfasavimas \t\t" + qty.ToString() + " (" + recipe_data.Rows[0]["gqty"].ToString() + ")" + "\n";
                richTextBox1.Text += "\n";
                richTextBox1.Text += Session.SystemData.name + "\n";
                richTextBox1.Text += "Įmonės kodas: \t\t" + Session.SystemData.ecode + "\n";
                richTextBox1.Text += "Vaistai išduoti: \t\t" + recipe_data.Rows[0]["salesdate"].ToString().Substring(0, 10) + "\n";
                richTextBox1.Text += "Vaistus išdavė: \t\t" + Session.User.postname + " " + Session.User.DisplayName + "\n";
                richTextBox1.Text += "\n";
                richTextBox1.Text += "Kasos apar. numeris: \t" + Session.Devices.deviceno + "\n";
                richTextBox1.Text += "Kasos čekio data: \t" + recipe_data.Rows[0]["checkdate"].ToString().Substring(0, 10) + "\n";
                richTextBox1.Text += "Kasos čekio numeris: \t" + recipe_data.Rows[0]["checkno"].ToString() + "\n";
            }
            form_wait(false);
        }

        private void print_recipe_Closing(object sender, FormClosingEventArgs e)
        {
            if (this.formWaiting == true)
                e.Cancel = true;
            if (caller == "btnPrint")
            {
                caller = "";
                e.Cancel = true;
            }
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
            this.Close();
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            showPrint();
        }

        private void showPrint()
        {
            caller = "btnPrint";
            PrintDialog pd = new PrintDialog();
            pd.Document = printDocument1;
            DialogResult result = pd.ShowDialog();
            if (result == DialogResult.OK)
                printDocument1.Print();
            pd.Dispose();
            pd = null;
        }

        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            e.Graphics.DrawString(richTextBox1.Text, richTextBox1.Font, Brushes.Black, 100, 20);
            e.Graphics.PageUnit = GraphicsUnit.Inch; 
        }
    }
}
