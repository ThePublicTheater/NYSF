using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Ambitus
{
	public class InterestCollection
			: Internals.ApiResultInterpreter, IEnumerable<Interest>
	{
		List<Interest> interests;

		public Interest this[int i]
		{
			get
			{
				return interests[i];
			}
		}

		public int Count
		{
			get
			{
				return interests.Count;
			}
		}

		public InterestCollection(DataTable tessResults)
		{
			interests = (from row in tessResults.AsEnumerable()
						  select new Interest(
							    _customer_no: row.Field<int?>("customer_no"),
								_id: row.Field<short?>("id"),
								_description: row.Field<string>("description"),
								_selected: ToBool(row.Field<string>("selected")),
								_weight: row.Field<int?>("weight"),
								_category: row.Field<string>("category"),
								_create_dt: row.Field<DateTime?>("create_dt"),
								_created_by: row.Field<string>("created_by"),
								_create_loc: row.Field<string>("create_loc"),
								_last_updated_by: row.Field<string>("last_updated_by"),
								_last_update_dt: row.Field<DateTime?>("last_update_dt")))
						.ToList<Interest>();
		}

		public IEnumerator<Interest> GetEnumerator()
		{
			return interests.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
}
