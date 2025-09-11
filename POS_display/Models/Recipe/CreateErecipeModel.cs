using System;

namespace POS_display.Models.Recipe
{
    public class CreateErecipeModel
    {
        public decimal PoshId { get; set; }

        public decimal PosdId { get; set; }

        public decimal ProductId { get; set; }

        public decimal UserId { get; set; }

        public decimal RecipeNo { get; set; }

        public decimal EncounterId { get; set; }

        public decimal RecipeId { get; set; }

        public DateTime RecipeDate { get; set; }

        public DateTime SalesDate { get; set; }

        public DateTime TillDate { get; set; }
    }
}
