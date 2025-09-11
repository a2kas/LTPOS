namespace POS_display.Models.Recipe
{
    public class PosDetailUpdateByRecipeModel
    {
        public decimal PosdId { get; set; }

        public decimal BarcodeId { get; set; }

        public decimal Qty { get; set; }

        public decimal NewPaySum { get; set; }

        public decimal NewHId { get; set; }

        public decimal PaySum { get; set; }
    }
}
