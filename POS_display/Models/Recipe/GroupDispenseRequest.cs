using System;

namespace POS_display.Models.Recipe
{
    public class GroupDispenseRequest
    {
        public Items.eRecipe.Recipe eRecipeItem { get; set; }
        public string PickedUpByRef { get; set; }
        public decimal PoshId { get; set; }
        public decimal PosdId { get; set; }
        public decimal ProductId { get; set; }
        public decimal RecipeId { get; set; }
        public DateTime TillDate { get; set; }
        public DateTime RecipeDate { get; set; }
        public DateTime SalesDate { get; set; }
        public decimal TotalSum { get; set; }
        public decimal PaySum { get; set; }
        public decimal CompSum { get; set; }
        public decimal GQty { get; set; }
        public decimal PrepCompSum { get; set; }
        public int DurationOfUse { get; set; }
        public bool ConfirmDispense { get; set; }
        public decimal eRecipeId { get; set; }
    }
}
