namespace POS_display.wpf.Model.display2
{
    public class PricesComp : Prices
    {

        public new string ShortNameHeader
        {
            get
            {
                return "Vaisto pavadinimas / Medicinos pagalbos priemonės pavadinimas";
            }
        }

        public new string VKBPriceHeader
        {
            get
            {
                return "Valstybės kompensuojamoji dalis, EUR";
            }
        }

        public new bool VKBPriceVisible
        {
            get
            {
                return true;
            }
        }

        public new string CompensatedPriceHeader
        {
            get
            {
                return "Priemoka, EUR";
            }
        }
    }
}
