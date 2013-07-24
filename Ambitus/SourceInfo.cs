using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Ambitus
{
	public class SourceInfo : Internals.ApiResultInterpreter
	{
		public int? SourceId { get; private set; }
		public string PromoCode { get; private set; }
		public short? ModeOfSaleId { get; private set; }
		public bool? OverridesRank { get; private set; }
		public string SourceName { get; private set; }
		public int? AppealId { get; private set; }
		public string AppealDescription { get; private set; }
		public string Text1 { get; private set; }
		public string Text2 { get; private set; }
		public string Text3 { get; private set; }
		public string Text4 { get; private set; }
		public string Text5 { get; private set; }
		public string Text6 { get; private set; }
		public DateTime? Date { get; private set; }

		public SourceInfo(DataTable tessResults)
		{
			var results = (from rows in tessResults.AsEnumerable()
						   select new
						   {
							   SourceId = rows.Field<int?>("source_no"),
							   PromoCode = rows.Field<string>("promo_code"),
							   ModeOfSaleId = rows.Field<int?>("mos"),
							   OverridesRank = rows.Field<string>("override_rank_ind"),
							   SourceName = rows.Field<string>("source_name"),
							   AppealId = rows.Field<int?>("appeal_no"),
							   AppealDescription = rows.Field<string>("appeal_desc"),
							   Text1 = rows.Field<string>("text1"),
							   Text2 = rows.Field<string>("text2"),
							   Text3 = rows.Field<string>("text3"),
							   Text4 = rows.Field<string>("text4"),
							   Text5 = rows.Field<string>("text5"),
							   Text6 = rows.Field<string>("text6"),
							   Date = rows.Field<string>("promote_dt")
						   }).Single();
			SourceId = results.SourceId;
			PromoCode = results.PromoCode;
			ModeOfSaleId = (short?)results.ModeOfSaleId;
			OverridesRank = ToBool(results.OverridesRank);
			SourceName = results.SourceName;
			AppealId = results.AppealId;
			AppealDescription = results.AppealDescription;
			Text1 = results.Text1;
			Text2 = results.Text2;
			Text3 = results.Text3;
			Text4 = results.Text4;
			Text5 = results.Text5;
			Text6 = results.Text6;
			Date = ToDateTime(results.Date);
		}
	}
}
