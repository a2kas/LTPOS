using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS_display.wpf.Model.recipe
{
    public class RecipeEdit : ViewModel.BaseViewModel
    {
        private Items.posd _in_posd;
        public Items.posd in_posd
        {
            get
            {
                return _in_posd;
            }
            set
            {
                _in_posd = value;
                PoshId = _in_posd.hid;
                PosdId = _in_posd.id;
                ProductId = _in_posd.productid;
                StoreName = Session.SystemData.storename;
                BarcodeId = _in_posd.barcodeid;
                Barcode = _in_posd.barcode;
                BarcodeName = _in_posd.barcodename;
                Qty = _in_posd.qty;
                RecipeId = _in_posd.recipeid;
                PosdPriceDiscounted = _in_posd.pricediscounted;
            }
        }

        public decimal PoshId { get; set; }
        public decimal PosdId { get; set; }
        public decimal ProductId { get; set; }
        public decimal RecipeId { get; set; }
        public decimal PosdPriceDiscounted { get; set; }
        public decimal StoreId { get; set; }
        public decimal BarcodeId { get; set; }

        private string _StoreName;
        public string StoreName
        {
            get
            {
                return _StoreName;
            }
            set
            {
                _StoreName = value;
                NotifyPropertyChanged("StoreName");
            }
        }

        private string _Barcode;
        public string Barcode
        {
            get
            {
                return _Barcode;
            }
            set
            {
                _Barcode = value;
                NotifyPropertyChanged("Barcode");
                NotifyPropertyChanged("Barcode_Enabled");
            }
        }

        public bool Barcode_Enabled
        {
            get
            {
                return string.IsNullOrWhiteSpace(Barcode);
            }
        }

        private string _BarcodeName;
        public string BarcodeName
        {
            get
            {
                return _BarcodeName;
            }
            set
            {
                _BarcodeName = value;
                NotifyPropertyChanged("BarcodeName");
            }
        }

        public bool BarcodeName_Enabled
        {
            get
            {
                return string.IsNullOrWhiteSpace(Barcode);
            }
        }

        private decimal _RecipeNo;
        public decimal RecipeNo
        {
            get
            {
                return _RecipeNo;
            }
            set
            {
                _RecipeNo = value;
                NotifyPropertyChanged("RecipeNo");
            }
        }

        private string _KVPDoctorNo;
        public string KVPDoctorNo
        {
            get
            {
                return _KVPDoctorNo;
            }
            set
            {
                _KVPDoctorNo = value;
                NotifyPropertyChanged("KVPDoctorNo");
                NotifyPropertyChanged("E_Recipe");
            }
        }

        private decimal _CompCode;
        public decimal CompCode
        {
            get
            {
                return _CompCode;
            }
            set
            {
                _CompCode = value;
                NotifyPropertyChanged("CompCode");
            }
        }

        private string _CompPercent;
        public string CompPercent
        {
            get
            {
                return _CompPercent;
            }
            set
            {
                _CompPercent = value;
                NotifyPropertyChanged("CompPercent");
            }
        }

        private DateTime _RecipeDate;
        public DateTime RecipeDate
        {
            get
            {
                return _RecipeDate;
            }
            set
            {
                _RecipeDate = value;
                NotifyPropertyChanged("RecipeDate");
            }
        }

        private DateTime _ValidFrom;
        public DateTime ValidFrom
        {
            get
            {
                return _ValidFrom;
            }
            set
            {
                _ValidFrom = value;
                NotifyPropertyChanged("ValidFrom");
            }
        }

        private decimal _Doses;
        public decimal Doses
        {
            get
            {
                return _Doses;
            }
            set
            {
                _Doses = value;
                NotifyPropertyChanged("Doses");
            }
        }

        private decimal _QtyDay;
        public decimal QtyDay
        {
            get
            {
                return _QtyDay;
            }
            set
            {
                _QtyDay = value;
                NotifyPropertyChanged("QtyDay");
            }
        }

        private int _CountDay;
        public int CountDay
        {
            get
            {
                return _CountDay;
            }
            set
            {
                _CountDay = value;
                NotifyPropertyChanged("CountDay");
            }
        }

        private DateTime _TillDate;
        public DateTime TillDate
        {
            get
            {
                return _TillDate;
            }
            set
            {
                _TillDate = value;
                NotifyPropertyChanged("TillDate");
            }
        }

        private DateTime _ValidTill;
        public DateTime ValidTill
        {
            get
            {
                return _ValidTill;
            }
            set
            {
                _ValidTill = value;
                NotifyPropertyChanged("ValidTill");
            }
        }

        private DateTime _PastTillDate;
        public DateTime PastTillDate
        {
            get
            {
                return _PastTillDate;
            }
            set
            {
                _PastTillDate = value;
                NotifyPropertyChanged("PastTillDate");
            }
        }

        private DateTime _ContValidFrom;
        public DateTime ContValidFrom
        {
            get
            {
                return _ContValidFrom;
            }
            set
            {
                _ContValidFrom = value;
                NotifyPropertyChanged("ContValidFrom");
            }
        }

        private DateTime _ContValidTill;
        public DateTime ContValidTill
        {
            get
            {
                return _ContValidTill;
            }
            set
            {
                _ContValidTill = value;
                NotifyPropertyChanged("ContValidTill");
            }
        }

        private string _DeseaseCode;
        public string DeseaseCode
        {
            get
            {
                return _DeseaseCode;
            }
            set
            {
                _DeseaseCode = value;
                NotifyPropertyChanged("DeseaseCode");
            }
        }

        private string _AAGA_ISAS;
        public string AAGA_ISAS
        {
            get
            {
                return _AAGA_ISAS;
            }
            set
            {
                _AAGA_ISAS = value;
                NotifyPropertyChanged("AAGA_ISAS");
            }
        }

        private bool _PrescriptionTagsLongTag;
        public bool PrescriptionTagsLongTag
        {
            get
            {
                return _PrescriptionTagsLongTag;
            }
            set
            {
                _PrescriptionTagsLongTag = value;
                NotifyPropertyChanged("PrescriptionTagsLongTag");
            }
        }

        private string _Water;
        public string Water
        {
            get
            {
                return _Water;
            }
            set
            {
                _Water = value;
                NotifyPropertyChanged("Water");
            }
        }

        private string _DoctorCode;
        public string DoctorCode
        {
            get
            {
                return _DoctorCode;
            }
            set
            {
                _DoctorCode = value;
                NotifyPropertyChanged("DoctorCode");
            }
        }

        private string _DoctorName;
        public string DoctorName
        {
            get
            {
                return _DoctorName;
            }
            set
            {
                _DoctorName = value;
                NotifyPropertyChanged("DoctorName");
            }
        }

        private decimal _Taxolaborum;
        public decimal Taxolaborum
        {
            get
            {
                return _Taxolaborum;
            }
            set
            {
                _Taxolaborum = value;
                NotifyPropertyChanged("Taxolaborum");
            }
        }

        private decimal _ClinicCode;
        public decimal ClinicCode
        {
            get
            {
                return _ClinicCode;
            }
            set
            {
                _ClinicCode = value;
                NotifyPropertyChanged("ClinicCode");
            }
        }

        private string _ClinicName;
        public string ClinicName
        {
            get
            {
                return _ClinicName;
            }
            set
            {
                _ClinicName = value;
                NotifyPropertyChanged("ClinicName");
            }
        }

        private int _Ext;
        public int Ext
        {
            get
            {
                return _Ext;
            }
            set
            {
                _Ext = value;
                NotifyPropertyChanged("Ext");
            }
        }

        private decimal _Qty;
        public decimal Qty
        {
            get
            {
                return _Qty;
            }
            set
            {
                _Qty = value;
                NotifyPropertyChanged("Qty");
            }
        }

        private decimal _GQty;
        public decimal GQty
        {
            get
            {
                return _GQty;
            }
            set
            {
                _GQty = value;
                NotifyPropertyChanged("GQty");
            }
        }

        private DateTime _SalesDate;
        public DateTime SalesDate
        {
            get
            {
                return _SalesDate;
            }
            set
            {
                _SalesDate = value;
                NotifyPropertyChanged("SalesDate");
            }
        }

        private DateTime _CheckDate;
        public DateTime CheckDate
        {
            get
            {
                return _CheckDate;
            }
            set
            {
                _CheckDate = value;
                NotifyPropertyChanged("CheckDate");
            }
        }

        private decimal _SalesPrice;
        public decimal SalesPrice
        {
            get
            {
                return _SalesPrice;
            }
            set
            {
                _SalesPrice = value;
                NotifyPropertyChanged("SalesPrice");
            }
        }

        private decimal _BasicPrice;
        public decimal BasicPrice
        {
            get
            {
                return _BasicPrice;
            }
            set
            {
                _BasicPrice = value;
                NotifyPropertyChanged("BasicPrice");
            }
        }

        private string _tlkId;
        public string tlkId
        {
            get
            {
                return _tlkId;
            }
            set
            {
                _tlkId = value;
                NotifyPropertyChanged("tlkId");
            }
        }

        private decimal _CheckNo;
        public decimal CheckNo
        {
            get
            {
                return _CheckNo;
            }
            set
            {
                _CheckNo = value;
                NotifyPropertyChanged("CheckNo");
            }
        }

        private decimal _CompSum;
        public decimal CompSum
        {
            get
            {
                return _CompSum;
            }
            set
            {
                _CompSum = value;
                NotifyPropertyChanged("CompSum");
            }
        }

        private decimal _PaySum;
        public decimal PaySum
        {
            get
            {
                return _PaySum;
            }
            set
            {
                _PaySum = value;
                NotifyPropertyChanged("PaySum");
            }
        }

        private decimal _TotalSum;
        public decimal TotalSum
        {
            get
            {
                return _TotalSum;
            }
            set
            {
                _TotalSum = value;
                NotifyPropertyChanged("TotalSum");
            }
        }

        private decimal _RecipeValid;
        public decimal RecipeValid
        {
            get
            {
                return _RecipeValid;
            }
            set
            {
                _RecipeValid = value;
                NotifyPropertyChanged("RecipeValid");
            }
        }

        public bool E_Recipe
        {
            get
            {
                return KVPDoctorNo.ToUpper() == "E";
            }
        }

        public int RecipeValidityPeriod
        {
            get
            {
                int period = 30;//default
                if (Ext == 1)
                {
                    //TODO
                    //if (E_Recipe)
                    //     period = in_erecipe.ValidationPeriod;
                    //else
                    //    period = 35;
                }
                return period - 1;//include this day
            }
        }
    }
}
