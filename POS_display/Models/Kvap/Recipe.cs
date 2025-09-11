using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS_display.Models.Kvap
{
    public class Recipe
    {
        public decimal Id { get; set; }
        public string Number { get; set; }
        public string Gtpl { get; set; }
        public string DrugId { get; set; }
        public decimal CompensationSum { get; set; }
        public decimal PrepaymentSum { get; set; }
        public decimal SaleSum { get; set; }
        public string Status { get; set; }
        public decimal PayedSum { get; set; }
        public List<RecipeError> RecipeErrors { get; set; } = new List<RecipeError>();
    }
}
