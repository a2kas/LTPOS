namespace POS_display
{
    public abstract class DB : DB_MAIN
    {
        public DB()
        { }
    }
    public abstract class DB_MAIN
    {
        private static DB_POS _POS = null;
        private static DB_cheque _cheque = null;
        private static DB_recipe _recipe = null;
        private static DB_eRecipe _eRecipe = null;
        private static DB_Settings _Settings = null;
        private static DB_KAS _KAS = null;
        private static DB_Loyalty _Loyalty = null;
        private static DB_Pay _Pay = null;

        public DB_MAIN()
        { }

        public static DB_POS POS
        {
            get
            {
                if (null == _POS)
                    _POS = new DB_POS();
                return _POS;
            }
        }

        public static DB_cheque cheque
        {
            get
            {
                if (null == _cheque)
                    _cheque = new DB_cheque();
                return _cheque;
            }
        }

        public static DB_recipe recipe
        {
            get
            {
                if (null == _recipe)
                    _recipe = new DB_recipe();
                return _recipe;
            }
        }

        public static DB_eRecipe eRecipe
        {
            get
            {
                if (null == _eRecipe)
                    _eRecipe = new DB_eRecipe();
                return _eRecipe;
            }
        }

        public static DB_Settings Settings
        {
            get
            {
                if (null == _Settings)
                    _Settings = new DB_Settings();
                return _Settings;
            }
        }

        public static DB_KAS KAS
        {
            get
            {
                if (null == _KAS)
                    _KAS = new DB_KAS();
                return _KAS;
            }
        }

        public static DB_Loyalty Loyalty
        {
            get
            {
                if (null == _Loyalty)
                    _Loyalty = new DB_Loyalty();
                return _Loyalty;
            }
        }

        public static DB_Pay Pay
        {
            get
            {
                if (null == _Pay)
                    _Pay = new DB_Pay();
                return _Pay;
            }
        }
    }
}
