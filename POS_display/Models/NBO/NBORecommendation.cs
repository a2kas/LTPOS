using System;
using System.ComponentModel;

namespace POS_display.Models.NBO
{
    public class NBORecommendation
    {
        [Browsable(false)]
        public string CN0 { get; set; }

        public string Name { get; set; }

        [Browsable(false)]
        public string RetailNo { get; set; }

        public decimal Discount { get; set; }

        public decimal Price { get; set; }

        [Browsable(false)]
        public decimal OldPrice { get; set; }

        [Browsable(false)]
        public bool OutOfStock { get; set; }

        [Browsable(false)]
        public bool HasPicture { get { return Picture != null && Picture.Length != 0; } }

        [Browsable(false)]
        public byte[] Picture { get; set; }

        [Browsable(false)]
        public string ProductGr4 { get; set; }

        [Browsable(false)]
        public string HeaderText { get; set; }

        public string ResolveInfo1Field()
        {
            return Discount == 0m ? $"{Math.Round(Price,2)} €" : $"{Math.Round(Discount, 0)} %";
        }

        public string ResolveInfo2Field()
        {
            return Discount != 0m ? $"{Math.Round(Price, 2)} €" : string.Empty;
        }

        public string ResolveInfo3Field()
        {
            return Discount != 0m ? $"{Math.Round(OldPrice, 2)} €" : string.Empty;
        }
    }
}
