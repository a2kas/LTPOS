using System;

namespace POS_display.Models.Recipe
{
    public class RecipeEditModel
    {
        public decimal ErecipeId { get; set; }
        public decimal RecipeId { get; set; }
        public decimal  GQty { get; set; }
        public decimal CountDay { get; set; }
        public DateTime TillDate { get; set; }
        public decimal TotalSum { get; set; }
        public decimal CompensationSum { get; set; }
        public decimal PrepaymentCompensationSum { get; set; }
        public decimal PaySum { get; set; }
    }
}
