using POS_display.Models.CRM;
using POS_display.Models.Loyalty;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace POS_display.Items
{
    public class posh
    {
        #region Members
        private decimal _id = 0;
        private string _checkNo = "";
        private string _loyaltyCardNo = "";
        private string _loyaltyCardType = "";
        private decimal _loyaltyPointsSum = 0;
        private decimal _usedCardNo = 0;
        private decimal _rimiDiscAmount = 0;
        private decimal _discountSum = 0;
        private List<ManualVoucher> _manualVouchers = new List<ManualVoucher>();
        #endregion

        #region Operators
        public static bool operator == (posh first, posh second)
        {
            if (first.Id == second.Id)
                return true;
            else
                return false;
        }

        public static bool operator !=(posh first, posh second)
        {
            if (first.Id != second.Id)
                return true;
            else
                return false;
        }
        #endregion

        #region Functions
        public bool CountStickers()
        {
            foreach (var sticker in Session.Stickers ?? new List<Sticker>())
            {
                decimal sum = Math.Round(PosdItems.Where(od => od?.info?.StartsWith(sticker.prefix) ?? false).Sum(od => od.sum), 2);
                if (sum >= sticker.amount_from && sum < sticker.amount_to)
                {
                    Items.Error error = new Error()
                    {
                        type = sticker.message_type,
                        description = string.Format(sticker.message, Math.Round(sticker.amount_to - sum, 2))
                    };
                    if (!helpers.alert(error, sticker.message_type == "confirm"))
                        return false;
                }
            }
            return true;
        }

        public string BuildLogs() 
        {
            if (PosdItems == null || PosdItems.Count == 0)
                return $"PosHeader Id: {Id} is empty";

            string data = $"PosHeader Id: {Id}:{Environment.NewLine}";
            foreach (var posDetail in PosdItems) 
            {
                data += $"PosDetail Id: {posDetail.id}," +
                        $" Barcode: {posDetail.barcode}," +
                        $" ProductId: {posDetail.productid}," +
                        $" RecipeNo: {posDetail.recipeno}," +
                        $" RecipeId: {posDetail.recipeid}" +
                        $" Have Recipe: {posDetail.have_recipe}" +
                        $"{Environment.NewLine}";
            }
            return data;
        }
        #endregion

        #region Properties
        public decimal Id
        {
            get { return _id; }
            set { _id = value; }
        }

        public string CheckNo
        {
            get { return _checkNo; }
            set { _checkNo = value; }
        }

        public string LoyaltyCardType
        {
            get { return _loyaltyCardType; }
            set { _loyaltyCardType = value; }
        }

        public decimal TotalSum
        {
            get
            {
                return Math.Round(PosdItems?.Sum(od => od.sum) ?? 0, 2);
            }
        }

        public decimal ChequeSum
        {
            get
            {
                return Math.Round(PosdItems?.Sum(od => od.cheque_sum) ?? 0, 2);
            }
        }

        public decimal ChequeInsuranceSum
        {
            get
            {
                return Math.Round(PosdItems?.Sum(od => od.cheque_sum_insurance) ?? 0, 2);
            }
        }

        public decimal CompensatedSum
        {
            get 
            {
                return Math.Round(PosdItems?.Sum(od => od.compensationsum) ?? 0, 2);
            }
        }

        public decimal DiscountSum
        {
            get { return _discountSum == 0m ? 0m : Math.Round(PosdItems?.Where(pd => pd.compensationsum == 0)?.Sum(pd => (pd.qty * pd.price) - pd.sum) ?? 0, 2); }
            set { _discountSum = value; }
        }

        public decimal UsedCardNo
        {
            get { return _usedCardNo; }
            set { _usedCardNo = value; }
        }

        public decimal RimiDiscAmount
        {
            get { return _rimiDiscAmount; }
            set { _rimiDiscAmount = value; }
        }

        public decimal CountStrikedOut
        {
            get 
            {
                return PosdItems?.Count(pd => pd.erecipe_no > 0 && pd.erecipe_active != 1) ?? 0;
            }
        }
        public bool IsEOrder
        {
            get
            {
                return PosdItems?.Any(e => e.barcodename.Contains("B1P") || e.barcodename.Contains("T1P")) ?? false;
            }
        }

        public List<wpf.Model.fmd> fmd_models
        {
            get
            {
                return PosdItems?.Where(w => w.fmd_model.Count > 0)?.SelectMany(s => s.fmd_model)?.ToList() ?? new List<wpf.Model.fmd>();
            }
        }

        public decimal CountFMDfalsified
        {
            get
            {
                return fmd_models.Count(f => f.Response.Success == false);
            }
        }

        public List<Items.posd> PosdItems { get; set; }
        public Items.Insurance InsuranceItem { get; set; }
        public CRMData CRMItem { get; set; }
        public List<ManualVoucher> ManualVouchers
        {
            get
            {
                return _manualVouchers;
            }
            set
            {
                _manualVouchers = value;
            }
        }
        #endregion
    }
}
