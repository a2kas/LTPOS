using Dapper.ColumnMapper;
using System;

namespace POS_display.Models.Recipe
{
    public class NewRecipeData
    {
		[ColumnMapping("c50")]
		public decimal Comp50 { get; set; }

		[ColumnMapping("c80")]
		public decimal Comp80 { get; set; }

		[ColumnMapping("c90")]
		public decimal Comp90 { get; set; }

		[ColumnMapping("c100")]
		public decimal Comp100 { get; set; }

		[ColumnMapping("productid")]
		public decimal ProductId { get; set; }

		[ColumnMapping("barcodeid")]
		public decimal BarcodeId { get; set; }

		[ColumnMapping("productname")]
		public string ProductName { get; set; }

		[ColumnMapping("barcode")]
		public string Barcode { get; set; }

		[ColumnMapping("tlkid")]
		public decimal TlkId { get; set; }

		[ColumnMapping("code2")]
		public decimal Code2 { get; set; }

		[ColumnMapping("salesprice")]
		public decimal SalesPrice { get; set; }

		[ColumnMapping("newsalesprice")]
		public decimal newsalesprice { get; set; }

		[ColumnMapping("basicprice")]
		public decimal BasicPrice { get; set; }

		[ColumnMapping("oldsalesprice")]
		public decimal OldSalesPrice { get; set; }

		[ColumnMapping("pricedate")]
		public DateTime PriceDate { get; set; }

		[ColumnMapping("prodratio")]
		public decimal ProductRatio { get; set; }

		[ColumnMapping("barratio")]
		public decimal BarcodeRatio { get; set; }

		[ColumnMapping("vatsize")]
		public decimal VatSize { get; set; }

		[ColumnMapping("retailpr")]
		public decimal RetailProductRatio { get; set; }

		[ColumnMapping("padeng_priemoka")]
		public decimal PreCompensationSum { get; set; }
	}
}
