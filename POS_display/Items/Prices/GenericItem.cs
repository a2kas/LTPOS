using POS_display.Models;
using System;

namespace POS_display.Items.Prices
{
    public class GenericItem
    {
        public string NpakId { get; set; }
        public string ShortName { get; set; }
        public string SecondATCName { get; set; }
        public string CompensationName { get; set; }
        public decimal Premium100 { get; set; }
        public decimal Premium90 { get; set; }
        public decimal Premium80 { get; set; }
        public decimal Premium50 { get; set; }
        public decimal MaxUnitPrice { get; set; }
        public decimal CompensatedPrice { get; set; }
        public string SecondATCNameId { get; set; }
        public decimal RetailPrice { get; set; }
        public int Ratio { get; set; }
        public int Modified { get; set; } 
        public decimal ProductId { get; set; }
        public decimal NpakId7 { get; set; }
        public string FormOfUse { get; set; }
        public decimal GIChainProc { get; set; }
        public byte IsCheapest { get; set; }
        public int RxPriority { get; set; }
        //not from generics
        public string ItemAtcCode { get; set; }
        public decimal VKBPrice100 { get; set; }
        public decimal VKBPrice90 { get; set; }
        public decimal VKBPrice80 { get; set; }
        public decimal VKBPrice50 { get; set; }
        public decimal PrepComp100 { get; set; }
        public decimal PrepComp90 { get; set; }
        public decimal PrepComp80 { get; set; }
        public decimal PrepComp50 { get; set; }
        public decimal CurrentBalanceQty { get; set; }
        public decimal CurrentTamroQty { get; set; }
        public decimal Qty { get; set; }
        public bool IsSelected { get; set; }
        public bool IsMaxGIChain { get; set; } = false;

        public string OficinaLocation { get; set; }
        public string Oficina2Location { get; set; }
        public string StockLocation { get; set; }

        public bool NotHasItemCard { get; set; }
        public decimal BarcodeRatio { get; set; }

        public string SupplyStatus { get; set; }

        public bool LowTherapeuticIndex { get; set; }

        #region ReadOnly varible

        public Barcode BarcodeModel;
        public decimal qtyVKBPrice100
        {
            get
            {
                return Math.Round(VKBPrice100 * Qty, 2);
            }
        }
        public decimal qtyVKBPrice90
        {
            get
            {
                return Math.Round(VKBPrice90 * Qty, 2);
            }
        }
        public decimal qtyVKBPrice80
        {
            get
            {
                return Math.Round(VKBPrice80 * Qty, 2);
            }
        }
        public decimal qtyVKBPrice50
        {
            get
            {
                return Math.Round(VKBPrice50 * Qty, 2);
            }
        }
        public decimal qtyVKBPrice { get; set; }
        #endregion
    }
}
