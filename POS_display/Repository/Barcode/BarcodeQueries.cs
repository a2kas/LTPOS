namespace POS_display.Repository.Barcode
{
    public static class BarcodeQueries
    {
        public static string GetBarcodeData =>
            @"SELECT b.id AS barcodeid, b.productid, s.gr4 FROM barcode b LEFT JOIN stock s ON s.id = b.productid  WHERE b.barcode = @barcode LIMIT 1";

        public static string ExistInBasisQuantity => "SELECT COUNT(productid) <> 0 FROM basis_quantity WHERE productid = @productid";

        public static string GetNpakId7ByProductId => "SELECT tlkid FROM tlk_kainos_bind WHERE productid = @productId";

        public static string FindBarcodeByProductId => "SELECT barcode FROM barcode WHERE productid = @productId LIMIT 1";
    }
}
