using POS_display.Models.HomeMode;
using System.Collections.Generic;

namespace POS_display.Models
{
    public class Barcode
    {
        public decimal PosdId { get; set; }
        public decimal Mode { get; set; }
        public string EAN { get; set; }
        public string BarcodeStr { get; set; }
        public string SerialNumber { get; set; }
        public string QtyStr { get; set; }
        public decimal ProductId { get; set; }
        public decimal Dosage { get; set; }
        public string Gr4 { get; set; }
        public bool IsInPrices
        {
            get
            {
                var lst = new List<string>
                {
                    { "Rx" },
                    { "Rx-N" },
                    { "RxK" },
                    { "RxK-N" },
                    { "MEDPK" }
                };
                return lst.Contains(Gr4);
            }
        }

        public decimal BarcodeID { get; set; }
        public bool FirstPrescription { get; set; } = false;
        public bool CheapestPrescription { get; set; }
        public string FirstPrescriptionReason { get; set; }
        public int CompPercent { get; set; } = 0;
        public decimal NpakId7 { get; set; }
        public bool LowIncomeTag { get; set; }
        public bool IsSalesOrderProduct { get; set; }
        public bool HasPriceChange { get; set; }
        public wpf.Model.fmd FmdModel { get; set; }
        public decimal Qty => decimal.TryParse(QtyStr, out var qty) ? qty : 1;
        public decimal ProductIdBySecondScreen { get; set; }
        public bool EnteredByMainFieldInDisplay1 { get; set; }
        public string MppBarcode { get; set; } = string.Empty;

        public HomeModeQuantities HomeModeQuantities { get; set; } = new HomeModeQuantities();

        public Items.Prices.GenericItem SelectedItem  { get; set; }
    }
}
