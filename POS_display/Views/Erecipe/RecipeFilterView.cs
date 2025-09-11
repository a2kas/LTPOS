using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace POS_display.Views.Erecipe
{
    public partial class RecipeFilterView : FormBase, IRecipeFilterView
    {
        private AutoCompleteStringCollection _asDataActiveSubtance = new AutoCompleteStringCollection();
        private Dictionary<Enumerator.RecipeFilterValue, string> _filterValues = new Dictionary<Enumerator.RecipeFilterValue, string>();

        public RecipeFilterView(Dictionary<Enumerator.RecipeFilterValue, string> filterValues)
        {
            InitializeComponent();
            _filterValues = filterValues ?? new Dictionary<Enumerator.RecipeFilterValue, string>();
        }

        #region Properties
        public Dictionary<Enumerator.RecipeFilterValue, string> FilterValues
        {
            get { return _filterValues; }
        }
        #endregion


        #region Public methods
        public void Init()
        {
            try
            {
                _asDataActiveSubtance.AddRange(Session.ActiveSubstances.ToArray());
                tbActiveSubtance.AutoCompleteCustomSource = _asDataActiveSubtance;
                foreach (var item in _filterValues)
                {
                    switch (item.Key)
                    {
                        case Enumerator.RecipeFilterValue.ActiveSubstance:
                            tbActiveSubtance.Text = item.Value;
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                helpers.alert(Enumerator.alert.warning, ex.Message);
            }
        }
        #endregion

        #region Private methods
        private void btnApply_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void tbActiveSubtance_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter)
                return;

            if (!Session.ActiveSubstances.Contains(tbActiveSubtance.Text))
                return;

            if (_filterValues.ContainsKey(Enumerator.RecipeFilterValue.ActiveSubstance))
            {
                _filterValues[Enumerator.RecipeFilterValue.ActiveSubstance] = tbActiveSubtance.Text;
            }
            else 
            {
                _filterValues.Add(Enumerator.RecipeFilterValue.ActiveSubstance, tbActiveSubtance.Text);
            }
        }
        #endregion
    }
}
