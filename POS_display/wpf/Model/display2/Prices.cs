namespace POS_display.wpf.Model.display2
{
    public class Prices
    { 
        public virtual string VKBPriceHeader { get; }
        public virtual bool VKBPriceVisible { get; }
        public virtual bool GIVisible { get; set; } = true;
        public virtual string CompensatedPriceHeader { get; }
        public int Percentage { get; set; }
        public string GenericName { get; set; }
        public string lblName { get; set; } = "Veiklioji medžiaga";
        public string lblAmount { get; set; } = "Perkamas doz. kiekis";
        public string lblStrength { get; set; } = "Stiprumas";
        public string lblCompensationPercent { get; set; } = "Kompensacijos procentas";
        public string lblFormOfUse { get; set; } = "Farmacinė forma";
        public virtual bool TamroQtyVisible { get; set; } = Session.HomeMode;
        public virtual bool PharmacyQtyVisible { get; set; } = !Session.HomeMode;
    }
}
