using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Ambitus
{
	public class AddressOptionCollection
			: Internals.ApiResultInterpreter, IEnumerable<AddressOption>
	{
		List<AddressOption> addresses;

		public AddressOption this[int i]
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

		public AddressOptionCollection(DataTable tessResults)
		{
			addresses = (from row in tessResults.AsEnumerable()
						  select new AddressOption(
							    _address_no: row.Field<int?>("address_no"),
								_type_desc: row.Field<string>("type_desc"),
								_street1: row.Field<string>("street1"),
								_street2: row.Field<string>("street2"),
								_city: row.Field<string>("city"),
								_state: row.Field<string>("state"),
								_postal_code: row.Field<string>("postal_code"),
								_country: row.Field<string>("country"),
								_primary: ToBool(row.Field<string>("primary")),
								_address_type: row.Field<int?>("address_type"),
								_country_id: row.Field<int?>("country_id"),
								_months: row.Field<string>("months"),
								_start_dt: row.Field<DateTime?>("start_dt"),
								_end_dt: row.Field<DateTime?>("end_dt"),
								_alt_signor: row.Field<int?>("alt_signor"),
								_mail_purposes: row.Field<string>("mail_purposes"),
								_label: ToBool(row.Field<string>("label")),
								_geo_area: row.Field<int?>("geo_area"),
								_delivery_point: row.Field<string>("delivery_point")))
						.ToList<AddressOption>();
		}

		public AddressOption GetById(int id)
		{
			return (from p in addresses
					where p.Id == id
					select p).FirstOrDefault<AddressOption>();
		}

		public IEnumerator<AddressOption> GetEnumerator()
		{
			return addresses.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
}