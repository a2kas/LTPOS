using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS_display.wpf.Model
{
    public class fmd : FMD.Model.Pack
    {
        #region Model
        public Items.posd posDetail;
        public string Barcode2D { get; set; } = "";
        public decimal id { get; set; }
        public string type { get; set; }
        public string referencenumber { get; set; }
        public bool deleted { get; set; }
        public string operationCode
        {
            set
            {
                Response.operationCode = value;
            }
        }
        public string state
        {
            get
            {
                return Response.state;
            }
            set
            {
                Response.state = value;
            }
        }
        public string information
        {
            set
            {
                Response.information = value;
            }
        }
        public string warning
        {
            get
            {
                return Response.warning;
            }
            set
            {
                Response.warning = value;
            }
        }
        public string alertId
        {
            get
            {
                return Response.alertId;
            }
            set
            {
                Response.alertId = value;
            }
        }
        public bool isvalidforsale
        {
            set
            {
                Response.Success = value;
            }
        }

        public FMD.Model.State state_enum
        {
            get
            {
                return Response.state.RemoveAllWhiteSpaces().Replace("-","").ToEnum<FMD.Model.State>();
            }
        }

        public string info
        {
            get
            {
                return Response.information ?? Response.warning ?? "";
            }
        }

        public FMD.Model.Pack Pack
        {
            get
            {
                return new FMD.Model.Pack()
                {
                    productCodeScheme = productCodeScheme,
                    productCode = productCode,
                    serialNumber = serialNumber,
                    batchId = batchId,
                    expiryDate = expiryDate
                };
            }
            set
            {
                productCodeScheme = value.productCodeScheme;
                productCode = value.productCode;
                serialNumber = value.serialNumber;
                batchId = value.batchId;
                expiryDate = value.expiryDate;
            }
        }

        private FMD.Model.SinglePackResponse _Response;
        public FMD.Model.SinglePackResponse Response
        {
            get
            {
                if (_Response == null)
                    _Response = new FMD.Model.SinglePackResponse();
                return _Response;
            }
            set
            {
                _Response = value;
            }
        }
        #endregion
        #region Business Logic
        public void Parse2Dbarcode()
        {
            if (Barcode2D != null)
            {
                var parsed = Session.BarcodeParser.Parse(Barcode2D);
                if (parsed?.Count > 0)
                {
                    productCodeScheme = parsed.FirstOrDefault(p => p.Key.Description == Parser2D.BarcodeElements.ProductCodeScheme).Value;
                    productCode = parsed.FirstOrDefault(p => p.Key.Description == Parser2D.BarcodeElements.ProductCode).Value;
                    serialNumber = parsed.FirstOrDefault(p => p.Key.Description == Parser2D.BarcodeElements.SerialNumber).Value;
                    batchId = parsed.FirstOrDefault(p => p.Key.Description == Parser2D.BarcodeElements.BatchId).Value;
                    expiryDate = parsed.FirstOrDefault(p => p.Key.Description == Parser2D.BarcodeElements.ExpiryDate).Value;
                }
            }
        }
        #endregion

        #region Operators
        //neveikia kai lygini su null
        //public static bool operator ==(fmd first, fmd second)
        //{
        //    if (first.productCodeScheme == second.productCodeScheme && first.productCode == second.productCode && first.serialNumber == second.serialNumber)
        //        return true;
        //    else
        //        return false;
        //}
        //public static bool operator !=(fmd first, fmd second)
        //{
        //    if (first.productCodeScheme != second.productCodeScheme || first.productCode != second.productCode)
        //        return true;
        //    else
        //        return false;
        //}
        #endregion
    }
}
