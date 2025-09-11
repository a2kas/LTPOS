using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS_display.Models.SalesOrder
{
    public class TransferResult
    {
        private decimal _internalHid;
        private decimal _externalHid;
        private decimal _clientId;

        public decimal InternalID
        {
            get => _internalHid;
            set => _internalHid = value;
        }

        public decimal ExternalID
        {
            get => _externalHid;
            set => _externalHid = value;
        }

        public decimal ClientId
        {
            get => _clientId;
            set => _clientId = value;
        }
    }
}
