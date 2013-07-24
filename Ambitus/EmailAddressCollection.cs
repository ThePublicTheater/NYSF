using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Ambitus
{
	public class EmailAddressCollection
			: Internals.ApiResultInterpreter, IEnumerable<EmailAddress>
	{
		List<EmailAddress> addresses;

		public EmailAddress this[int i]
		{
			get
			{
				return addresses[i];
			}
		}

		public int Count
		{
			get
			{
				return addresses.Count;
			}
		}

		public EmailAddressCollection(DataTable tessResults)
		{
			addresses = (from row in tessResults.AsEnumerable()
						  select new EmailAddress(
								id: row.Field<int?>("eaddress_no"),
								address: row.Field<string>("address"),
								typeId: row.Field<int?>("eaddress_type"),
								typeName: row.Field<string>("address_type_desc"),
								emailInd: ToBool(row.Field<string>("email_ind")),
								startDate: row.Field<DateTime?>("start_dt"),
								endDate: row.Field<DateTime?>("end_dt"),
								n1N2: row.Field<int?>("n1n2_ind"),
								months: row.Field<string>("months"),
								isPrimary: ToBool(row.Field<string>("primary_ind")),
								active: Invert(ToBool(row.Field<string>("inactive"))),
								purposes: row.Field<string>("mail_purposes"),
								acceptsHtmlFormat: ToBool(row.Field<string>("html_ind")),
								marketInd: ToBool(row.Field<string>("market_ind")),
								inactive1: ToBool(row.Field<string>("inactive1"))))
						.ToList<EmailAddress>();
		}

		public EmailAddress GetPrimary()
		{
			return (from c in addresses
					where c.IsPrimary.HasValue && c.IsPrimary.Value
					select c).SingleOrDefault<EmailAddress>();
		}

		public IEnumerator<EmailAddress> GetEnumerator()
		{
			return addresses.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
}
