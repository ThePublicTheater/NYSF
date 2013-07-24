using System.Data;
using System.Linq;

namespace Ambitus
{
	public class LoginInfo
	{
		public int? ConstituentId { get; private set; }
		public short? ModeOfSaleId { get; private set; }
		public short? OriginalModeOfSaleId { get; private set; }
		public int? SourceId { get; private set; }
		public bool? IsPermanent { get; private set; }

		public LoginInfo(DataTable tessResults)
		{
			var results = (from rows in tessResults.AsEnumerable()
						 select new
						 {
							 ConstituentId = rows.Field<int?>("customer_no"),
							 ModeOfSaleId = rows.Field<int?>("MOS"),
							 OriginalModeOfSaleId = rows.Field<int?>("OriginalMOS"),
							 SourceId = rows.Field<int?>("promotion_code"),
							 IsPermanent = rows.Field<string>("Status")
						 }).Single();
			ConstituentId = results.ConstituentId;
			ModeOfSaleId = (short?)results.ModeOfSaleId;
			OriginalModeOfSaleId = (short?)results.OriginalModeOfSaleId;
			SourceId = results.SourceId;
			IsPermanent = results.IsPermanent == "P";
		}
	}
}
