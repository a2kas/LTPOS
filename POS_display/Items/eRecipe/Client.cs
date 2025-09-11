using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace POS_display.Items.eRecipe
{
    public class Client
    {
        private string _Id = "";
        public string Id
        {
            get { return _Id; }
            set { _Id = value; }
        }

        private string _GivenName = "";
        public string GivenName
        {
            get { return _GivenName; }
            set { _GivenName = value; }
        }

        private string _FamilyName = "";
        public string FamilyName
        {
            get { return _FamilyName; }
            set { _FamilyName = value; }
        }

        private string _RelatedPerson;
        public string RelatedPerson
        {
            get { return _RelatedPerson; }
            set { _RelatedPerson = value; }
        }
    }
}
