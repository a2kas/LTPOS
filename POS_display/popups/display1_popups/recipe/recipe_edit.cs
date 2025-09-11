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
    public partial class recipe_edit : Form
    {
        private UserControl _ucRecipeEdit;
        private Items.posd in_posd;

        public recipe_edit(Items.posd posd_item)
        {
            InitializeComponent();
            in_posd = posd_item;
        }

        private void recipe_edit_Load(object sender, EventArgs e)
        {
            _ucRecipeEdit = new ucRecipeEdit(null, in_posd);
            _ucRecipeEdit.Dock = DockStyle.Fill;
            Controls.Add(_ucRecipeEdit);
            _ucRecipeEdit.BringToFront();
        }

        private void ucRecipeEdit_ControlRemoved(object sender, ControlEventArgs e)
        {
            ucRecipeEdit uc = e.Control as ucRecipeEdit;
            if (uc.out_form_action == "recipe_saved")
                this.DialogResult = DialogResult.OK;
            else
                this.DialogResult = DialogResult.Cancel;
        }
    }
}
