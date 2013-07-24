using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Ambitus
{
	public class PhoneNumberCollection
			: Internals.ApiResultInterpreter, IEnumerable<PhoneNumber>
	{
		List<PhoneNumber> numbers;

		public PhoneNumber this[int i]
		{
			get
			{
				return numbers[i];
			}
		}

		public int Count
		{
			get
			{
				return numbers.Count;
			}
		}

		public PhoneNumberCollection(DataTable tessResults)
		{
			numbers = (from row in tessResults.AsEnumerable()
						  select new PhoneNumber(
							    _phone_no: row.Field<int?>("phone_no"),
								_address_no: row.Field<int?>("address_no"),
								_phone: row.Field<string>("phone"),
								_type: row.Field<int?>("type"),
								_type_desc: row.Field<string>("type_desc"),
								_day_ind: row.Field<string>("day_ind"),
								_tele_ind: ToBool(row.Field<string>("tele_ind")),
								_primary_adr: ToBool(row.Field<string>("primary_adr"))))
						.ToList<PhoneNumber>();
		}

		public IEnumerator<PhoneNumber> GetEnumerator()
		{
			return numbers.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
}
