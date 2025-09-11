using POS_display.Models.Loyalty;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace POS_display.Presenters.Vouchers
{
    public interface IVouchersPresenter
    {
        Items.posh PosHeader { get; set; }

        List<ManualVoucher> VouchersList { get; set; }

        Task Init(Items.posh posHeader);

        Task LoadData();

        void PreviousPageClick();

        void NextPageClick();

        void FirstPageClick();

        void LastPageClick();

        void SetCurrentCell(int currentRowIndex, int currentColumnIndex);

        void CopyRowCell();

        bool CheckSelectedVouchersAmount();

        void DetermineQuantityCellReadOnly(int rowIndex, int columnIndex);

        void DetermineQuantityCellValue(int rowIndex, int columnIndex);
    }
}
