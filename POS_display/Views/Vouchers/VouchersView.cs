using POS_display.Models.KAS;
using POS_display.Models.Loyalty;
using POS_display.Presenters.Vouchers;
using System;
using System.Collections.Generic;
using System.Windows.Forms;


namespace POS_display.Views.Vouchers
{
    public partial class VouchersView : FormBase, IVouchersView, IBaseView
    {
        #region Members
        private readonly VouchersPresenter _vouchersPresenter;
        #endregion

        #region Properties
        public RadioButton All
        {
            get { return rbAll; }
            set { rbAll = value; }
        }

        public RadioButton Cheque
        {
            get { return rbCheque; }
            set { rbCheque = value; }
        }

        public CheckBox InstanceDiscount
        {
            get { return cbInstanceDiscount; }
            set { cbInstanceDiscount = value; }
        }

        public DataGridView VouchersGrid
        {
            get { return gvVouchers; }
            set { gvVouchers = value; }
        }

        public Label RecordsStatus
        {
            get { return lblRecordsStatus; }
            set { lblRecordsStatus = value; }
        }

        public Button LastPage
        {
            get { return btnLast; }
            set { btnLast = value; }
        }

        public Button FirstPage
        {
            get { return btnFirst; }
            set { btnFirst = value; }
        }

        public Button PreviousPage
        {
            get { return btnPrevious; }
            set { btnPrevious = value; }
        }

        public Button NextPage
        {
            get { return btnNext; }
            set { btnNext = value; }
        }

        public GroupBox ListType
        {
            get { return gbListType; }
            set { gbListType = value; }
        }

        public Button OK
        {
            get { return btnOK; }
            set { btnOK = value; }
        }

        public List<ManualVoucher> Vouchers
        {
            get { return gvVouchers.DataSource as List<ManualVoucher>; }
        }
        public Items.posh PosHeader { get; set; }

        #endregion


        #region Constructor
        public VouchersView(Items.posh posHeader)
        {
            InitializeComponent();
            _vouchersPresenter = new VouchersPresenter(this);
            PosHeader = posHeader;
        }
        #endregion

        private async void VouchersView_Load(object sender, EventArgs e)
        {
            await ExecuteWithWaitAsync(async () => 
            {
                await _vouchersPresenter.UpdateSession("Vouceriai ", 2);
                await _vouchersPresenter.Init(PosHeader);
            }, false);
        }

        private void VouchersView_Closing(object sender, FormClosingEventArgs e)
        {
            ExecuteWithWait(() => cbInstanceDiscount.Enabled = true);
        }

        private void btnPrevious_Click(object sender, EventArgs e)
        {
            ExecuteWithWait(() => _vouchersPresenter.PreviousPageClick());
        }

        private void btnFirst_Click(object sender, EventArgs e)
        {
            ExecuteWithWait(() => _vouchersPresenter.FirstPageClick());
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            ExecuteWithWait(() => _vouchersPresenter.NextPageClick());
        }

        private void btnLast_Click(object sender, EventArgs e)
        {
            ExecuteWithWait(() => _vouchersPresenter.LastPageClick());
        }

        private void rb_CheckedChanged(object sender, EventArgs e)
        {
            ExecuteWithWait(() =>
            {
                RadioButton rb = sender as RadioButton;
                if (rb.Checked)
                    _vouchersPresenter.LoadData();
            });
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            ExecuteWithWait(() =>
            {
                Close();
                DialogResult = DialogResult.Cancel;
            });
        }

        private void gvVouchers_CurrentCellChanged(object sender, EventArgs e)
        {
            ExecuteWithWait(() =>
            {
                DataGridView dgv = sender as DataGridView;
                _vouchersPresenter.SetCurrentCell(
                    dgv.CurrentRow != null ? dgv.CurrentRow.Index : -1,
                    dgv.CurrentRow != null ? dgv.CurrentCell.ColumnIndex : -1);
            });
        }

        private void gvVouchers_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            ExecuteWithWait(() =>
            {
                DataGridView dgv = sender as DataGridView;
                if (e.ColumnIndex != -1 && e.RowIndex != -1 && e.Button == MouseButtons.Right)
                {
                    dgv.CurrentCell = dgv.Rows[e.RowIndex].Cells[e.ColumnIndex];
                    dgvContextMenu.Show(dgv, dgv.PointToClient(Cursor.Position));
                }
            });
        }

        private void gvVouchers_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            ExecuteWithWait(() => 
            {
                _vouchersPresenter.DetermineQuantityCellReadOnly(e.RowIndex, e.ColumnIndex);
                _vouchersPresenter.DetermineQuantityCellValue(e.RowIndex, e.ColumnIndex);
            });
        }

        private void CopyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ExecuteWithWait(() => _vouchersPresenter.CopyRowCell());
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            ExecuteWithWait(() =>
            {
                if(!_vouchersPresenter.CheckSelectedVouchersAmount())
                    helpers.alert(Enumerator.alert.error, "Nurodykite pasirinktų nuolaidų reikiamą kiekį");
                else
                    DialogResult = DialogResult.OK;
            });
        }
    }
}
