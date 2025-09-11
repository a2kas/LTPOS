using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace POS_display.Items.eRecipe
{
    public class ClientAllergy
    {
        private string _Id = "";
        public string Id
        {
            get { return _Id; }
            set { _Id = value.Substring(value.IndexOf("/") + 1); }
        }

        private string _Code = "";
        public string Code
        {
            get { return _Code; }
            set { _Code = value; }
        }

        private string _Name = "";
        public string Name
        {
            get { return _Name; }
            set { _Name = value; }
        }

        private string _Description;
        public string Description
        {
            get { return _Description; }
            set { _Description = value; }
        }
    }
}
