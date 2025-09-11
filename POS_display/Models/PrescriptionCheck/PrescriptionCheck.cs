using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS_display.Models.PrescriptionCheck
{
    public class PrescriptionCheck
    {
        public string RecipeValid { get; set; }
        public Items.posd CurrentPosdRow { get; set; }
    }
}
