using POS_display.Items.eRecipe;
using POS_display.Items.Prices;
using System;
using System.Windows.Forms;

namespace POS_display.Views.Erecipe.PaperRecipe
{
    public partial class PaperRecipeSelectionView : FormBase, IPaperRecipeSelectionView
    {
        #region Members
        private GenericItem _genericItem;
        private Recipe _eRecipeItem;
        private Items.posd _posDetai;
        #endregion

        #region Constructor
        public PaperRecipeSelectionView()
        {
            InitializeComponent();

            cbRecipeForms.Items.Add("1 Formos nekompensuojamas receptas");
            cbRecipeForms.Items.Add("1 Formos - vienkartinis išdavimas be recepto");
            cbRecipeForms.Items.Add("2 Formos receptas (narkotinių vaistų)");
            cbRecipeForms.Items.Add("2 Formos, kai vaistinio preparato įsigijimas apmokamas iš valstybės biudžeto lėšų");
            cbRecipeForms.Items.Add("3 Formos kompensuojamas receptas");
            cbRecipeForms.Items.Add("3 Formos receptas (išimties atvejams)");
            cbRecipeForms.Items.Add("3 Formos, finansuojamos iš valstybės biudžeto");
            cbRecipeForms.Items.Add("3 Formos, vienkartinis išdavimas be galiojančio recepto");

            cbRecipeForms.SelectedIndex = 0;
        }
        #endregion

        #region Properties
        public ComboBox RecipeForms
        {
            get => cbRecipeForms;
            set => cbRecipeForms = value;
        }

        public Button Apply
        {
            get => btnApply;
            set => btnApply = value;
        }
        #endregion

        #region Public methods
        public void SetGenericItem(GenericItem genericItem)
        {
            _genericItem = genericItem;
            if (genericItem.NpakId7.ToString().StartsWith("9"))
            {
                cbRecipeForms.Items.Clear();
                cbRecipeForms.Items.Add("3 Formos kompensuojamas receptas");
                cbRecipeForms.Items.Add("3 Formos receptas (išimties atvejams)");
                cbRecipeForms.SelectedIndex = 0;
            }
        }

        public void SetERecipeItem(Recipe eRecipeItem)
        {
            _eRecipeItem = eRecipeItem;
        }

        public void SetPosDetail(Items.posd posDetail)
        {
            _posDetai = posDetail;
        }
        #endregion

        #region Private methods
        private void btnApply_Click(object sender, System.EventArgs e)
        {
            var selectRecipeFormIndex = cbRecipeForms.SelectedIndex;
            switch (selectRecipeFormIndex) 
            {
                case 0:
                    using (Form1NotCompensatedView form1NotCompensatedView = new Form1NotCompensatedView())
                    {
                        form1NotCompensatedView.MedicationItem = _genericItem;
                        form1NotCompensatedView.eRecipeItem = _eRecipeItem;
                        form1NotCompensatedView.PosDetail = _posDetai;
                        form1NotCompensatedView.ShowDialog();
                    }
                    break;
                case 1:
                    using (Form1OneTimeWithoutRecipeView form1OneTimeWithoutRecipeView = new Form1OneTimeWithoutRecipeView())
                    {
                        form1OneTimeWithoutRecipeView.MedicationItem = _genericItem;
                        form1OneTimeWithoutRecipeView.eRecipeItem = _eRecipeItem;
                        form1OneTimeWithoutRecipeView.PosDetail = _posDetai;
                        form1OneTimeWithoutRecipeView.ShowDialog();
                    }
                    break;
                case 2:
                    using (Form2NarcoticView form2NarcoticView = new Form2NarcoticView())
                    {
                        form2NarcoticView.MedicationItem = _genericItem;
                        form2NarcoticView.eRecipeItem = _eRecipeItem;
                        form2NarcoticView.PosDetail= _posDetai;
                        form2NarcoticView.ShowDialog();
                    }
                    break;
                case 3:
                    throw new NotImplementedException();
                case 4:
                case 7:
                    using (Form3CompensatedView form3CompensatedView = new Form3CompensatedView())
                    {
                        form3CompensatedView.MedicationItem = _genericItem;
                        form3CompensatedView.eRecipeItem = _eRecipeItem;
                        form3CompensatedView.PosDetail = _posDetai;
                        form3CompensatedView.ShowDialog();
                    }
                    break;
                case 5:
                    throw new NotImplementedException();
                case 6:
                    throw new NotImplementedException();
            }
        }
        #endregion
    }
}
