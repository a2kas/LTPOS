using POS_display.Models.Loyalty;
using POS_display.Utils.Logging;
using POS_display.Views.Vouchers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace POS_display.Presenters.Vouchers
{
    public class VouchersPresenter : BasePresenter, IVouchersPresenter
    {
        #region Members
        private readonly IVouchersView _view;
        private int _currentPageIndex;
        private int _pageSize = 10;
        private int _currentRowIndex = -1;
        private int _currentColumnIndex = -1;
        private int _lastPageIndex;
        #endregion

        #region Constructor
        public VouchersPresenter(IVouchersView view)
        {
            _view = view ?? throw new ArgumentNullException();
        }
        #endregion

        #region Properties
        public Items.posh PosHeader { get; set; }
        public List<ManualVoucher> VouchersList { get; set; } = new List<ManualVoucher>();
        #endregion

        #region Public methods
        public async System.Threading.Tasks.Task Init(Items.posh posHeader)
        {
            PosHeader = posHeader;
            _view.InstanceDiscount.Enabled = posHeader.CRMItem.Pensioner;
            _view.InstanceDiscount.Checked = posHeader.CRMItem.AccruePoints == 0;
            await LoadData();
        }

        public async System.Threading.Tasks.Task LoadData()
        {
            try
            {
                List<ManualVoucher> SelectedVouchers = new List<ManualVoucher>();
                foreach (var el in PosHeader.CRMItem.ManualVouchers.GroupBy(val => val.Code))
                {
                    SelectedVouchers.Add(new ManualVoucher()
                    {
                        Selected = 1,
                        Code = el.First().Code,
                        Qty = el.Count()
                    });
                }

                string listType = _view.All.Checked ? "F" : "C";
                var recommendedRewardsList = await Session.CRMRestUtils.RecommendedBestRewards(PosHeader, listType);
                VouchersList = (from el in recommendedRewardsList.Data.RecommendedBestRewards
                                from p in SelectedVouchers
                                .Where(map => map.Code == el.Code)
                                .DefaultIfEmpty(new ManualVoucher() { Selected = 0, Code = "", Name = "", RewardPriority = 0, Qty = 0, MaxCount = 0 })
                                select new ManualVoucher
                                {
                                    Selected = p.Selected,
                                    Code = el.Code ?? p.Code,
                                    Name = el.Name ?? p.Name,
                                    RewardPriority = (int)el.RewardPriority,
                                    MaxCount = el.MaxCount,
                                    Qty = p.Selected == 1 ? p.Qty : el.MaxCount
                                }).ToList();

                _currentPageIndex = VouchersList.Count != 0 ? 1 : 0;
                SetCurrentPageData();
            }
            catch (Exception ex)
            {
                Serilogger.GetLogger().Error(ex, ex.Message);
                helpers.alert(Enumerator.alert.error, ex.Message);
            }
        }

        private void SetCurrentPageData() 
        {
            int startIndex = (_currentPageIndex - 1) * _pageSize;
            int endIndex = (_currentPageIndex - 1) * _pageSize + _pageSize;
            if (endIndex > VouchersList.Count)
                endIndex = VouchersList.Count;

            var VouchersListInPage = VouchersList.Skip(startIndex).Take(_pageSize).ToList();
            _view.VouchersGrid.DataSource = VouchersListInPage?.Count() > 0 ? VouchersListInPage : new List<ManualVoucher>();
            _lastPageIndex = (int)Math.Ceiling(Convert.ToDecimal(VouchersList.Count) / _pageSize);

            if (VouchersListInPage != null && VouchersListInPage.Count() > 0)
            {
                _view.RecordsStatus.Text = _currentPageIndex + " / " + _lastPageIndex;
                foreach (DataGridViewRow dr in _view.VouchersGrid.Rows)
                {
                    _view.VouchersGrid.Rows[dr.Index].Cells["colQty"].ReadOnly = _view.VouchersGrid.Rows[dr.Index].Cells["colSelected"].Value.ToDecimal() != 1;
                }
            }
            else
            {
                _currentPageIndex = 0;
                _view.VouchersGrid.DataSource = new List<ManualVoucher>();
                _view.RecordsStatus.Text = _currentPageIndex + " / " + _currentPageIndex;
            }
            EnableNavigation();
        }

        public void PreviousPageClick()
        {
            EnableControls(false);

            _currentPageIndex--;
            _view.NextPage.Enabled = true;
            _view.LastPage.Enabled = true;

            SetCurrentPageData();
            EnableControls(true);
        }

        public void NextPageClick()
        {
            EnableControls(false);

            _currentPageIndex++;
            _view.FirstPage.Enabled = true;
            _view.PreviousPage.Enabled = true;

            SetCurrentPageData();
            EnableControls(true);
        }

        public void FirstPageClick()
        {
            EnableControls(false);

            _currentPageIndex = 1;
            _view.FirstPage.Enabled = true;
            _view.PreviousPage.Enabled = true;
            _view.NextPage.Enabled = false;
            _view.LastPage.Enabled = false;

            SetCurrentPageData();
            EnableControls(true);
        }

        public void LastPageClick()
        {
            EnableControls(false);

            _currentPageIndex = (int)Math.Ceiling(Convert.ToDecimal(VouchersList.Count()) / _pageSize);
            _view.FirstPage.Enabled = true;
            _view.PreviousPage.Enabled = true;
            _view.NextPage.Enabled = false;
            _view.LastPage.Enabled = false;

            SetCurrentPageData();
            EnableControls(true);
        }

        public void SetCurrentCell(int currentRowIndex, int currentColumnIndex)
        {
            _currentRowIndex = currentRowIndex;
            _currentColumnIndex = currentColumnIndex;
        }

        public void CopyRowCell()
        {
            helpers.CopyRowCell(_view.VouchersGrid, _currentRowIndex, _currentColumnIndex);
        }

        public bool CheckSelectedVouchersAmount()
        {
            foreach (DataGridViewRow dr in _view.VouchersGrid.Rows)
            {
                ManualVoucher voucher = (ManualVoucher)dr.DataBoundItem;
                if (voucher.Selected == 1 && voucher.Qty <= 0)           
                    return false;
            }
            return true;
        }

        public void DetermineQuantityCellReadOnly(int rowIndex, int columnIndex) 
        {
            if (rowIndex > -1 && columnIndex == _view.VouchersGrid.Rows[rowIndex].Cells["colSelected"].ColumnIndex)
            {
                _view.VouchersGrid.Rows[rowIndex].Cells["colQty"].ReadOnly = _view.VouchersGrid.Rows[rowIndex].Cells["colSelected"].Value.ToDecimal() == 0;
            }
        }

        public void DetermineQuantityCellValue(int rowIndex, int columnIndex)
        {
            if (rowIndex > -1 && columnIndex == _view.VouchersGrid.Rows[rowIndex].Cells["colQty"].ColumnIndex)
            {
                int.TryParse(_view.VouchersGrid.Rows[rowIndex].Cells["colQty"].Value.ToString(), out int qty);
                int.TryParse(_view.VouchersGrid.Rows[rowIndex].Cells["colMaxCount"].Value.ToString(), out int maxQty);
                if (qty > maxQty)
                    _view.VouchersGrid.Rows[rowIndex].Cells["colQty"].Value = maxQty;
            }
        }
        #endregion

        #region Private methods
        private void EnableNavigation()
        {
            if (_currentPageIndex >= _lastPageIndex)
            {
                _view.NextPage.Enabled = false;
                _view.LastPage.Enabled = false;
            }
            else
            {
                _view.NextPage.Enabled = true;
                _view.LastPage.Enabled = true;
            }

            if (_currentPageIndex <= 1)
            {
                _view.PreviousPage.Enabled = false;
                _view.FirstPage.Enabled = false;
            }
            else
            {
                _view.PreviousPage.Enabled = true;
                _view.FirstPage.Enabled = true;
            }
        }

        private void EnableControls(bool apply) 
        {
            _view.ListType.Enabled = apply;
            _view.OK.Enabled = apply;
        }
        #endregion
    }
}
