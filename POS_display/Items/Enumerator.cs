namespace POS_display
{
    public static class Enumerator
    {
        public enum alert 
        { 
            error, 
            info, 
            warning, 
            display2, 
            confirm,
            donations
        }

        public enum display2forms 
        { 
            Posd, 
            Prices,
            Add 
        }

        public enum DrugType 
        { 
            NONE,
            NARC, 
            PSYC 
        }

        public enum VaccineOrderStatus 
        { 
            pending,
            accepted,
            complete,
            cancelled,
            aborted
        }

        public enum VaccineOrderEvent 
        {
            None,
            Closed,
            Created,
            CreatedToDispense,
            Aborted 
        }

        public enum ProductFlag
        {
            None          = 0,
            UA            = 1,  // If product has this flag then for that product can be applied Ukraine refugee insurance
            FmdException  = 2,  // If product has this flag then any FMD check is not applied for it
            NotReserveQty = 4   // Product can be sold even there is no stock
        }

        public enum VaccineDocFilterValue
        {
            Both          = 1,
            Prescription  = 2,
            Dispensation  = 3
        }

        public enum SalesOrderResult
        {
            UNKNOWN                 = 0,
            THERE_IS_NO_BARCODE     = 1,
            WRONG_KAS_CLIENT        = 2,
            INSUFFICIENT_QUANTITY   = 3,
            CREATE_STOCKH_ERROR     = 4,
            CREATE_STOCKD_ERROR     = 5,
            SAVE_CLIENT_ERROR       = 6
        }

        public enum CRMDataLoadState
        {
            None = 0,
            Loading = 1,
            Success = 2,
            Error = 3,
        }

        public enum CRMCustomerType
        {
            None = 0,
            LC = 1,
            nonLC = 2,
            RIMI = 3,
        }
        public enum ProductNote2
        {
            NONE = 0,
            NARC = 1,
            PSYC = 2,
            SALD = 3,
            VARD = 4
        }

        public enum SumType
        {
            Unknown = 0,
            Perecent = 1,
            Value = 2
        }

        public enum DiscountType 
        {
            Unknown = 0,
            Perecent = 1,
            Cumulative = 2,
            CumulativeWithVAT = 3
        }

        public enum EKJType
        {
            None = 0,
            ZReport = 1
        }

        public enum CRMLoyaltyCardType
        {
            None = 0,
            Simple = 1,
            B2B = 2
        }

        public enum CRMBillType
        {
            None = 0,
            AcceptPayment = 1,
            SendPurchase = 2,
            RecommendedBestReward = 3
        }

        public enum POSMemoParamter
        {
            HomeMode = 1,
            Wolt = 2,
            CreateMultipeDispense = 3
        }
        public enum FifoMode
        {
            NotReserveQty               = 0,
            ReserveQtyCalcPrices        = 1,
            ReserveQty                  = 2
        }

        public enum PosResponseStatus
        {
            Unknown = 0,
            OK = 1,
            NO = 2,
            ER = 3
        }

        public enum MppBarcodeStatus
        {
            Unknown = 0,
            IsInListInvalid = 1,
            IsInListValid = 2,
            ThereIsNoInList = 3
        }

        public enum RecipeFilterValue
        {
            None = 0,
            ActiveSubstance = 1
        }
    }
}
