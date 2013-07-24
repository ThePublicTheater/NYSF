using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Ambitus
{
	public class ProgramListingCollection
			: Internals.ApiResultInterpreter, IEnumerable<ProgramListing>
	{
		List<ProgramListing> listings;

		public ProgramListing this[int i]
		{
			get
			{
				return listings[i];
			}
		}

		public int Count
		{
			get
			{
				return listings.Count;
			}
		}

		public ProgramListingCollection(DataTable tessResults)
		{
			listings = (from row in tessResults.AsEnumerable()
						  select new ProgramListing(
							    _customer_no: row.Field<int?>("customer_no"),
								_program_type: row.Field<int?>("program_type"),
								_program_type_desc: row.Field<string>("program_type_desc"),
								_cust_pname: row.Field<string>("cust_pname"),
								_sort_name: row.Field<string>("sort_name"),
								_donation_level: row.Field<int?>("donation_level"),
								_donation_level_desc: row.Field<string>("donation_level_desc")))
						.ToList<ProgramListing>();
		}

		public IEnumerator<ProgramListing> GetEnumerator()
		{
			return listings.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
}
