using System;
using System.Collections.Generic;

namespace POS_display.Items.Prices
{
    public class Generics : ICloneable
    {
        private List<GenericItem> _it;

        public List<GenericItem> ItemsAllList
        {
            get { return _it; }
            set { this._it = value; }
        }

        public Generics() { }

        #region ICloneable Members
        public object Clone()
        {
            return this.MemberwiseClone();
        }

        #endregion
    }
}
