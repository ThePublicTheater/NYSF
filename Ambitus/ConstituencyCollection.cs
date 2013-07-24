using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Ambitus
{
	public class ConstituencyCollection
			: Internals.ApiResultInterpreter, IEnumerable<Constituency>
	{
		List<Constituency> constituencies;

		public Constituency this[int i]
		{
			get
			{
				return constituencies[i];
			}
		}

		public int Count
		{
			get
			{
				return constituencies.Count;
			}
		}

		public ConstituencyCollection(DataTable tessResults)
		{
			constituencies = (from row in tessResults.AsEnumerable()
						  select new Constituency(
							    _constituency: row.Field<string>("constituency"),
								_created_by: row.Field<string>("created_by"),
								_create_dt: row.Field<DateTime?>("create_dt"),
								_n1n2_ind: row.Field<byte?>("n1n2_ind"),
								_id: row.Field<int?>("id"),
								_short_desc: row.Field<string>("short_desc")))
						.ToList<Constituency>();
		}

		public IEnumerator<Constituency> GetEnumerator()
		{
			return constituencies.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
}
