using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using POS_display.Models.Partner;
using POS_display.Presenters.Partners;
using POS_display.Repository.Partners;
using POS_display.Repository.Pos;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace POS_display.Views.Partners
{
    public partial class PartnersView : FormBase, IPartnersView, IBaseView
    {
        #region Members
        private readonly PartnersPresenter _partnersPresenter;
        private PartnerViewData _focusedPartner;
        private int _currentRow = 0;
        private int _currentColumn = 0;
        private long _partnerToFocusId;
        private PartnerEditConfig _partnerEditConfig;
        #endregion

        #region Constructor
        public PartnersView()
        {
            InitializeComponent();
            var mapper = Program.ServiceProvider.GetRequiredService<IMapper>();
            _partnersPresenter = new PartnersPresenter(this, new PosRepository(), new PartnerRepository(), mapper);
        }
        #endregion

        #region Properties
        public Label RecordStatus 
        {
            get => lblRecordsStatus;
            set => lblRecordsStatus = value;
        }

        public Button NextPage
        {
            get => btnNext;
            set => btnNext = value;
        }

        public Button LastPage
        {
            get => btnLast;
            set => btnLast = value;
        }

        public Button FirstPage
        {
            get => btnFirst;
            set => btnFirst = value;
        }

        public Button PreviousPage
        {
            get => btnPrevious;
            set => btnPrevious = value;
        }

        public Button NewPartner
        {
            get => btnNew;
            set => btnNew = value;
        }

        public Button EditPartner
        {
            get => btnEdit;
            set => btnEdit = value;
        }

        public Button CloseButton
        {
            get => btnClose;
            set => btnClose = value;
        }

        public Button FindButton
        {
            get => btnFind;
            set => btnFind = value;
        }

        public ComboBox FilterByValues
        {
            get => cbFilter;
            set => cbFilter = value;
        }

        public TextBox FilterValue
        {
            get => tbFilter;
            set => tbFilter = value;
        }

        public List<PartnerViewData> CurrentPageData
        {
            get => partnersDataGridView.DataSource as List<PartnerViewData>;
            set => partnersDataGridView.DataSource = value;
        }

        public PartnerViewData FocusedPartner
        {
            get => _focusedPartner;
            set => _focusedPartner = value;
        }

        public DataGridView PartnersGridView
        {
            get => partnersDataGridView;
            set => partnersDataGridView = value;
        }

        public PartnerEditConfig PartnerEditConfig
        {
            get => _partnerEditConfig;
            set => _partnerEditConfig = value;
        }

        public void SetFocusPartner(long partnerId) 
        {
            _partnerToFocusId = partnerId;            
        }
        #endregion

        #region Actions
        private async void PartnersView_Load(object sender, EventArgs e)
        {
            await ExecuteWithWaitAsync(async () => await _partnersPresenter.Init());
        }

        private void btnPrevious_Click(object sender, EventArgs e)
        {
            ExecuteWithWait(() => _partnersPresenter.SetPreviousPage());
        }

        private void btnFirst_Click(object sender, EventArgs e)
        {
            ExecuteWithWait(() => _partnersPresenter.SetFirstPage());
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            ExecuteWithWait(() => _partnersPresenter.SetNextPage());
        }

        private void btnLast_Click(object sender, EventArgs e)
        {
            ExecuteWithWait(() => _partnersPresenter.SetLastPage());
        }

        private async void btnFind_Click(object sender, EventArgs e)
        {
            await ExecuteWithWaitAsync(async () => await _partnersPresenter.LoadPartners());            
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private async void btnEdit_Click(object sender, EventArgs e)
        {
            using (PartnerEditorView partnerEditorView = new PartnerEditorView())
            {
                partnerEditorView.PartnerEditConfig = _partnerEditConfig;
                await partnerEditorView.Init();
                var focusedPartner = _partnersPresenter.GetFocusedPartner();
                await partnerEditorView.LoadData(focusedPartner.Id);
                if (partnerEditorView.ShowDialog() == DialogResult.OK)
                {
                    ExecuteWithWait(() => _partnersPresenter.ClearFilter());
                    await ExecuteWithWaitAsync(async () => await _partnersPresenter.LoadPartners());
                    ExecuteWithWait(() => _partnersPresenter.LoadFilterAutoCompleteData());
                    ExecuteWithWait(() => _partnersPresenter.FocusPartner(partnerEditorView.Id));
                    helpers.alert(Enumerator.alert.info, $"Partnerio '{partnerEditorView.PartnerName.Text}' duomenys sėkmingai atnaujinti.");
                }
            }
        }

        private async void btnNew_Click(object sender, EventArgs e)
        {
            using (PartnerEditorView partnerEditorView = new PartnerEditorView())
            {
                partnerEditorView.PartnerEditConfig = _partnerEditConfig;
                await partnerEditorView.Init();
                if (partnerEditorView.ShowDialog() == DialogResult.OK)
                {
                    ExecuteWithWait(() => _partnersPresenter.ClearFilter());
                    await ExecuteWithWaitAsync(async () => await _partnersPresenter.LoadPartners());
                    ExecuteWithWait(() => _partnersPresenter.LoadFilterAutoCompleteData());
                    ExecuteWithWait(() => _partnersPresenter.FocusPartner(partnerEditorView.Id));
                    helpers.alert(Enumerator.alert.info, $"Partneris '{partnerEditorView.PartnerName.Text}' sėkmingai sukurtas.");
                }
            }
        }

        private async void PartnersView_Shown(object sender, EventArgs e)
        {
            Application.DoEvents();
            await ExecuteWithWaitAsync(async () => await _partnersPresenter.LoadPartners());
            ExecuteWithWait(() => _partnersPresenter.LoadFilterAutoCompleteData());
            if (_partnerToFocusId > 0)
                ExecuteWithWait(() => _partnersPresenter.FocusPartner(_partnerToFocusId));
        }

        private void cbFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            ExecuteWithWait(() => _partnersPresenter.SetFilterAutoCompleteAvailability());
        }

        private void partnersDataGridView_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            DataGridView dgv = sender as DataGridView;
            if (e.ColumnIndex != -1 && e.RowIndex != -1 && e.Button == MouseButtons.Right)
            {
                dgv.CurrentCell = dgv.Rows[e.RowIndex].Cells[e.ColumnIndex];
                dgvContextMenu.Show(dgv, dgv.PointToClient(Cursor.Position));
            }

            _currentColumn = e.ColumnIndex;
            _currentRow = e.RowIndex;
        }

        private void partnersDataGridView_DoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            DataGridView dgv = sender as DataGridView;
            _focusedPartner = (dgv.Rows[e.RowIndex].DataBoundItem as PartnerViewData);
            DialogResult = DialogResult.OK;
            Close();
        }

        private void kopijuotiToolStripMenuItem_Click(object sender, EventArgs e)
        {
            helpers.CopyRowCell(partnersDataGridView, _currentRow, _currentColumn);
        }
        #endregion

    }
}
