using System;

namespace POS_display.Models.Recipe
{
    public class CreateRecipeModel
    {
        public decimal DbCode { get; set; }

        public string TlkId { get; set; }

        public decimal BarcodeId { get; set; }

        public string RecSer { get; set; }

        public decimal RecipeNo { get; set; }

        public string PersCode { get; set; }

        public string ClinicId { get; set; }

        public string DeseaseCode { get; set; }

        public string DoctorId { get; set; }

        public DateTime RecipeDate { get; set; }

        public decimal SalesPrice { get; set; }

        public decimal BasicPrice { get; set; }

        public decimal CompensationId { get; set; }

        public decimal Qty { get; set; }

        public decimal TotalSum { get; set; }

        public decimal CompSum { get; set; }

        public decimal PaySum { get; set; }

        public DateTime SalesDate { get; set; }

        public decimal GQty { get; set; }

        public decimal Water { get; set; }

        public decimal TaxoLaborum { get; set; }

        public int Ext { get; set; }

        public DateTime CheckDate { get; set; }

        public decimal CheckNo { get; set; }

        public decimal QtyDay { get; set; }

        public decimal CountDay { get; set; }

        public DateTime TillDate { get; set; }

        public string KvpDoctorNo { get; set; }

        public string AagaIsas { get; set; }

        public DateTime ValidFrom { get; set; }

        public DateTime ValidTill { get; set; }

        public decimal StoreId { get; set; }
    }
}
