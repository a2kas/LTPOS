using Microsoft.Extensions.DependencyInjection;
using POS_display.Presenters;
using POS_display.Presenters.Price;
using POS_display.Repository.Barcode;
using POS_display.Views.DrugPrices;
using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using Tamroutilities.Client;

namespace POS_display
{
    public partial class DrugPricesView : FormBase, IDrugPricesView
    {
        #region Members
        private AutoCompleteStringCollection _asDataActiveSubtance = new AutoCompleteStringCollection();
        private AutoCompleteStringCollection _asDataMedicationName = new AutoCompleteStringCollection();
        private readonly IDrugPricesPresenter _drugPricesPresenter;
        #endregion

        #region Properties
        public string SearchBarcode
        {
            get
            {
                return tbSearchBarcode.Text;
            }
            set
            {
                tbSearchBarcode.Text = value;
            }
        }
        #endregion

        #region Constructor
        public DrugPricesView()
        {
            _drugPricesPresenter = new DrugPricesPresenter(this, 
                Program.ServiceProvider.GetRequiredService<ITamroClient>(),
                new BarcodeRepository());
            InitializeComponent();
        }
        #endregion

        #region Public methods
        public void LoadData()
        {
            try
            {
                _asDataActiveSubtance.AddRange(Session.ActiveSubstances.ToArray());
                _asDataMedicationName.AddRange(Session.MedicationNames.ToArray());

                tbSearchActiveSubstance.AutoCompleteCustomSource = _asDataActiveSubtance;
                tbSearchMedicationName.AutoCompleteCustomSource= _asDataMedicationName;
            }
            catch (Exception ex)
            {
                helpers.alert(Enumerator.alert.warning, ex.Message);
            }
        }
        #endregion

        #region Actions
        private void SearchFormView_Activated(object sender, EventArgs e)
        {
            tbQty.Select(0, tbQty.Text.Length);
        }

        private void SearchFormView_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                Close();
            }
        }

        private async void tbSearchActiveSubstance_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter) return;
            await ExecuteWithWaitAsync(async () =>
            {
                string barcode = await _drugPricesPresenter.GetBarcodeByActiveSubstance(tbSearchActiveSubstance.Text);
                await SubmitBarcode(barcode, tbQty.Value);
                DialogResult = DialogResult.OK;
            });
        }

        private async void tbSearchMedicationName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter) return;
            await ExecuteWithWaitAsync(async () =>
            {
                string barcode = await _drugPricesPresenter.GetBarcodeByGenericName(tbSearchMedicationName.Text);
                await SubmitBarcode(barcode, tbQty.Value);
                DialogResult = DialogResult.OK;
            });
        }

        public async void tbSearchBarcode_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter) return;
            await ExecuteWithWaitAsync(async () =>
            {
                await SubmitBarcode(tbSearchBarcode.Text, tbQty.Value);
                DialogResult = DialogResult.OK;
            });
        }
        private async Task SubmitBarcode(string barcode, decimal dosage) 
        {
            var barcodeModel = new Models.Barcode() { BarcodeStr = barcode };
            var barcodePresnter = new BarcodePresenter(null, barcodeModel);
            await barcodePresnter.GetDataFromBarcode();
            barcodeModel.Dosage = dosage;
            Program.Display2.ExecuteFromRemote(barcodeModel);
        }

        private void tbQty_Enter(object sender, EventArgs e)
        {
            NumericUpDown tb = sender as NumericUpDown;
            tb.Select(0, tb.Text.Length);
        }

        private void btnSelBarcode_Click(object sender, EventArgs e)
        {
            using (stock_info dlg = new stock_info())
            {
                dlg.Location = helpers.middleScreen(this, dlg);
                dlg.caller = "select_barcode";
                dlg.ShowDialog();
                if (!dlg.selBarcode.Equals(""))
                {
                    SearchBarcode = dlg.selBarcode;
                    tbSearchBarcode.Select();
                    tbSearchBarcode_KeyDown(new object(), new KeyEventArgs(Keys.Enter));
                }
            }
        }
        #endregion
    }
}
