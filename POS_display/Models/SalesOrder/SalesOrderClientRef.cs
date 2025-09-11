using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS_display.Models.SalesOrder
{
    public class SalesOrderClientRef
    {
        private decimal _hid;
        private decimal _externalhid;
        private decimal _poshid;
        private decimal _posdid;
        private decimal _clientid;
        private string _status;
        private string _trackingNumber;


        public decimal HID
        {
            get => _hid;
            set => _hid = value;
        }

        public decimal ExternalHID
        {
            get => _externalhid;
            set => _externalhid = value;
        }

        public decimal PoshID
        {
            get => _poshid;
            set => _poshid = value;
        }

        public decimal PosdID
        {
            get => _posdid;
            set => _posdid = value;
        }

        public decimal ClientID
        {
            get => _clientid;
            set => _clientid = value;
        }

        public string Status
        {
            get => _status;
            set => _status = value;
        }

        public string TrackingNumber
        {
            get => _trackingNumber;
            set => _trackingNumber = value;
        }
    }
}
