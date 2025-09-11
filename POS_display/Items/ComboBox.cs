using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace POS_display.Items
{
    public class ComboBox<T>
    {
        public T ValueMember { get; set; } 
        public string DisplayMember { get; set; }
    }
}
