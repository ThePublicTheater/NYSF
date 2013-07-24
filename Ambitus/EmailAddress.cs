using System;
using System.Data;
using System.Linq;

namespace Ambitus
{
	public class EmailAddress : Internals.ApiResultInterpreter
	{
		public int? Id { get; private set; }
		public string Address { get; private set; }
		public int? TypeId { get; private set; }
		public string TypeName { get; private set; }
		public bool? EmailInd { get; private set; }
		public DateTime? StartDate { get; private set; }
		public DateTime? EndDate { get; private set; }
		public int? N1N2 { get; private set; }
		public string Months { get; private set; }
		public bool? IsPrimary { get; private set; }
		public bool? Active { get; private set; }
		public string Purposes { get; private set; }
		public bool? AcceptsHtmlFormat { get; private set; }
		public bool? MarketInd { get; private set; }
		public bool? Inactive1 { get; private set; }

		public EmailAddress(int? id, string address, int? typeId, string typeName, bool? emailInd,
			DateTime? startDate, DateTime? endDate, int? n1N2, string months, bool? isPrimary,
			bool? active, string purposes, bool? acceptsHtmlFormat, bool? marketInd,
			bool? inactive1)
		{
			Id = id;
			Address = address;
			TypeId = typeId;
			TypeName = typeName;
			EmailInd = emailInd;
			StartDate = startDate;
			EndDate = endDate;
			N1N2 = n1N2;
			Months = months;
			IsPrimary = isPrimary;
			Active = active;
			Purposes = purposes;
			AcceptsHtmlFormat = acceptsHtmlFormat;
			MarketInd = marketInd;
			Inactive1 = inactive1;
		}

		public EmailAddress(DataTable tessResult)
		{
			var result = (from row in tessResult.AsEnumerable()
						  select new
						  {
							  id = row.Field<int?>("eaddress_no"),
							  address = row.Field<string>("address"),
							  typeId = row.Field<int?>("eaddress_type"),
							  typeName = row.Field<string>("address_type_desc"),
							  emailInd = ToBool(row.Field<string>("email_ind")),
							  startDate = row.Field<DateTime?>("start_dt"),
							  endDate = row.Field<DateTime?>("end_dt"),
							  n1N2 = row.Field<int?>("n1n2_ind"),
							  months = row.Field<string>("months"),
							  isPrimary = ToBool(row.Field<string>("primary_ind")),
							  active = Invert(ToBool(row.Field<string>("inactive"))),
							  purposes = row.Field<string>("mail_purposes"),
							  acceptsHtmlFormat = ToBool(row.Field<string>("html_ind")),
							  marketInd = ToBool(row.Field<string>("market_ind")),
							  inactive1 = ToBool(row.Field<string>("inactive1"))
						  }).Single();
			Id = result.id;
			Address = result.address;
			TypeId = result.typeId;
			TypeName = result.typeName;
			EmailInd = result.emailInd;
			StartDate = result.startDate;
			EndDate = result.endDate;
			N1N2 = result.n1N2;
			Months = result.months;
			IsPrimary = result.isPrimary;
			Active = result.active;
			Purposes = result.purposes;
			AcceptsHtmlFormat = result.acceptsHtmlFormat;
			MarketInd = result.marketInd;
			Inactive1 = result.inactive1;
		}

		public override string ToString()
		{
			return Address ?? String.Empty;
		}
	}

}
