using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace POS_display.Items.eRecipe
{
    public class from_posd : Items.posd
    {
        public decimal prodratio { get; set; }
        public decimal barratio { get; set; }
    }
}
