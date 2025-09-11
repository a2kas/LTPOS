using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS_display.Models
{
    public class display1
    {
        private decimal _Id = 0;
        private string _checkNo = "";
        private string _loyaltyCardNo = "";
        private string _loyaltyCardType = "";
        private decimal _loyaltyPointsSum = 0;
        private decimal _usedCardNo = 0;
        private decimal _rimiDiscAmount = 0;

        #region Operators
        public static bool operator ==(display1 first, display1 second)
        {
            if (first.Id == second.Id)
                return true;
            else
                return false;
        }

        public static bool operator !=(display1 first, display1 second)
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
            foreach (var sticker in Session.Stickers)
            {
                decimal sum = Math.Round(PosdItems.Where(od => od.info.StartsWith(sticker.prefix)).Sum(od => od.sum), 2);
                if (sum >= sticker.amount_from && sum < sticker.amount_to)
                {
                    Items.Error error = new Items.Error()
                    {
                        type = sticker.message_type,
                        description = sticker.message
                    };
                    if (!helpers.alert(error, sticker.message_type == "confirm"))
                        return false;
                }
            }
            return true;
        }
        #endregion

        public decimal Id
        {
            get { return _Id; }
            set { _Id = value; }
        }

        public string checkNo
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
                return Math.Round(PosdItems.Sum(od => od.sum), 2);
            }
        }

        public decimal ChequeSum
        {
            get
            {
                return Math.Round(PosdItems.Sum(od => od.cheque_sum), 2);
            }
        }

        public decimal ChequeInsuranceSum
        {
            get
            {
                return Math.Round(PosdItems.Sum(od => od.cheque_sum_insurance), 2);
            }
        }

        public decimal CompensatedSum
        {
            get
            {
                return Math.Round(PosdItems.Sum(od => od.compensationsum), 2);
            }
        }

        public decimal DiscountSum
        {
            get
            {
                return Math.Round(PosdItems.Where(pd => pd.recipeid == 0).Sum(pd => (pd.qty * pd.price) - pd.sum), 2);
            }
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
                return PosdItems.Count(pd => pd.erecipe_no > 0 && pd.erecipe_active != 1);
            }
        }
        public List<Items.posd> PosdItems { get; set; }
        public Items.Insurance InsuranceItem { get; set; }
    }
}
