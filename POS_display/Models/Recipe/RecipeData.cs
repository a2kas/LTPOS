using Dapper.ColumnMapper;
using System;

namespace POS_display.Models.Recipe
{
	public class NotCompensatedRecipeData
	{
		[ColumnMapping("id")]
		public decimal Id { get; set; }

		[ColumnMapping("doses")]
		public decimal Doses { get; set; }

		[ColumnMapping("qty_day")]
		public decimal QtyDay { get; set; }

		[ColumnMapping("count_day")]
		public decimal CountDay { get; set; }

		[ColumnMapping("valid_from")]
		public DateTime ValidFrom { get; set; }

		[ColumnMapping("till_date")]
		public DateTime TillDate { get; set; }
	}
}
