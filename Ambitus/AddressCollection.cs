using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Ambitus
{
	public class AddressCollection
			: Internals.ApiResultInterpreter, IEnumerable<Address>
	{
		List<Address> addresses;

		public Address this[int i]
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

		public AddressCollection(DataTable tessResults /* "Addresses" */)
		{
			addresses = (from row in tessResults.AsEnumerable()
					   select new Address(
						   id: row.Field<int?>("address_no"),
						   constituentId: row.Field<int?>("customer_no"),
						   typeId: row.Field<int?>("address_type"),
						   typeName: row.Field<string>("address_type_desc"),
						   streetAddress: row.Field<string>("street1"),
						   subStreetAddress: row.Field<string>("street2"),
						   city: row.Field<string>("city"),
						   state: row.Field<string>("state"),
						   postalCode: row.Field<string>("postal_code"),
						   postalCodeFormat: row.Field<string>("postal_code_format"),
						   countryId: row.Field<int?>("country"),
						   countryLongName: row.Field<string>("country_long"),
						   countryShortName: row.Field<string>("country_short"),
						   startDate: row.Field<DateTime?>("start_dt"),
						   endDate: row.Field<DateTime?>("end_dt"),
						   months: row.Field<string>("months"),
						   isPrimary: ToBool(row.Field<string>("primary_ind")),
						   isActive: Invert(ToBool(row.Field<string>("inactive"))),
						   mailPurposes: row.Field<string>("mail_purposes"),
						   geoArea: row.Field<int?>("geo_area"))).ToList<Address>();
		}

		public Address GetPrimary()
		{
			return (from p in addresses
					where p.IsPrimary == true
					select p).Single<Address>();
		}

		public IEnumerator<Address> GetEnumerator()
		{
			return addresses.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
}
