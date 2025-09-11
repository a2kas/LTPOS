using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace POS_display.Items.KAS
{
    public class posd
    {
        public int selected { get; set; }
        public decimal id { get; set; }
        public decimal hid { get; set; }
        public decimal productid { get; set; }
        public decimal barcodeid { get; set; }
        public decimal qty { get; set; }
        public decimal qty_orig { get; set; }
        public decimal price { get; set; }
        public decimal priceincvat { get; set; }
        public decimal discount { get; set; }
        public decimal discount_sum { get; set; }
        public decimal pricediscounted { get; set; }
        public decimal vat { get; set; }
        public decimal vat_orig { get; set; }
        public decimal sum { get; set; }
        public decimal sumincvat { get; set; }
        public decimal sumincvat_orig { get; set; }
        public decimal recipeid { get; set; }
        public int recipe2 { get; set; }
        public string barcodename { get; set; }
        public string barcode { get; set; }
        public decimal vatsize { get; set; }
        public decimal f_cost_price { get; set; }
        public decimal serialid { get; set; }
        public string card_no { get; set; }
        public string info { get; set; }
        public string cheque_from { get; set; }
        public decimal cheque_sum { get; set; }
        public decimal compensationsum { get; set; }
        public decimal prepayment_compensation { get; set; }
        public bool fmd_required { get; set; }
        public decimal returnedqty { get; set; }
        public List<wpf.Model.fmd> fmd_model { get; set; } = new List<wpf.Model.fmd>();
        public bool fmd_is_valid_for_sale
        {
            get
            {
                return Math.Ceiling(qty) <= fmd_model?.Where(w => w.type == "decommission")?.Count();
            }
        }
        public string fmd_link
        {
            get
            {
                return fmd_required ? "FMD" : "";
            }
        }
    }
}
