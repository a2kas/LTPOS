namespace POS_display.wpf.Model.display2
{
    public class PricesUnComp : Prices
    {
        public new string ShortNameHeader
        {
            get
            {
                return "Vaisto pavadinimas";
            }
        }

        public new bool VKBPriceVisible
        {
            get
            {
                return false;
            }
        }

        public new string CompensatedPriceHeader
        {
            get
            {
                return "Mažmeninė kaina, EUR";
            }
        }
    }
}
