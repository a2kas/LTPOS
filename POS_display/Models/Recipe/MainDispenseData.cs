using System;
using System.ComponentModel;

namespace POS_display.Models.Recipe
{
    public class MainDispenseData
    {
        public string CompositionId { get; set; }
        public string ProprietaryName { get; set; }
        [Browsable(false)]
        public string DueDate { get; set; }
        public string QuantityValue { get; set; }

        private DateTime _dateDueDate;

        public DateTime DateDueDate
        {
            get
            {
                if (!DateTime.TryParse(DueDate, out _dateDueDate))
                {
                    _dateDueDate = DateTime.MinValue;
                }
                return _dateDueDate;
            }
        }
    }
}
