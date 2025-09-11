using System;
using System.ComponentModel;

namespace POS_display.Models.KAS
{
    public class ReturningItem : INotifyPropertyChanged
    {
        private long _posDetailId;
        private string _name;
        private decimal _qty;
        private decimal _discount;
        private decimal _compensationSum;
        private decimal _vat5Value;
        private decimal _vat21Value;
        private decimal _insuranceSum;
        private decimal _priceWithVAT;
        private decimal _sumWithVAT;
        private decimal _totalSum;
        private decimal _prepaymentCompensation;

        [Browsable(false)]
        public long PosDetailId
        {
            get
            {
                return _posDetailId;
            }
            set
            {
                _posDetailId = value;
            }
        }

        public string Name 
        {
            get 
            { 
                return _name; 
            }
            set
            {
                _name = value;
                NotifyPropertyChanged(nameof(Name));
            }
        }

        public decimal Qty 
        {
            get 
            {
                return _qty;
            }
            set
            {
                _qty = value;
                NotifyPropertyChanged(nameof(Qty));
            }
        }

        public decimal PriceWithVAT
        {
            get
            {
                return _priceWithVAT;
            }
            set
            {
                _priceWithVAT = value;
                NotifyPropertyChanged(nameof(PriceWithVAT));
            }
        }

        [Browsable(false)]
        public decimal Discount
        {
            get
            {
                return _discount;
            }
            set
            {
                _discount = value;
                NotifyPropertyChanged(nameof(Discount));
            }
        }

        public decimal DiscountSum
        {
            get
            {
                return Math.Round(((_priceWithVAT * _discount)/100) * _qty, 2);
            }
        }

        public decimal SumWithVAT
        {
            get
            {
                return Math.Round(_sumWithVAT,2);
            }
            set
            {
                _sumWithVAT = value;
                NotifyPropertyChanged(nameof(SumWithVAT));
            }
        }

        [Browsable(false)]
        public decimal CompensationSum 
        {
            get
            {
                return _compensationSum;
            }
            set
            {
                _compensationSum = value;
                NotifyPropertyChanged(nameof(CompensationSum));
            }
        }

        [Browsable(false)]
        public decimal PrepaymentCompensation
        {
            get
            {
                return _prepaymentCompensation;
            }
            set
            {
                _prepaymentCompensation = value;
                NotifyPropertyChanged(nameof(PrepaymentCompensation));
            }
        }

        public decimal InsuranceSum
        {
            get
            {
                return _insuranceSum;
            }
            set
            {
                _insuranceSum = value;
                NotifyPropertyChanged(nameof(InsuranceSum));
            }
        }

        public decimal VAT21Value
        {
            get
            {
                return _vat21Value;
            }
            set 
            {
                _vat21Value = value;
                NotifyPropertyChanged(nameof(VAT21Value));
            }
        }

        public decimal VAT5Value
        {
            get
            {
                return _vat5Value;
            }
            set 
            {
                _vat5Value = value;
                NotifyPropertyChanged(nameof(VAT5Value));
            }
        }

        public decimal TotalSum 
		{
            get 
            {
                return Math.Round(_totalSum,2);
            }
            set
            {
                _totalSum = value;
            }
        }

        public decimal TotalCompensation
        {
            get
            {
                return _compensationSum + _prepaymentCompensation;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(String propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
