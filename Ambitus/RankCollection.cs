using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Ambitus
{
	public class RankCollection
			: Internals.ApiResultInterpreter, IEnumerable<Rank>
	{
		List<Rank> ranks;

		public Rank this[int i]
		{
			get
			{
				return ranks[i];
			}
		}

		public int Count
		{
			get
			{
				return ranks.Count;
			}
		}

		public RankCollection(DataTable tessResults)
		{
			ranks = (from row in tessResults.AsEnumerable()
						  select new Rank(
							   _rank_type: row.Field<int?>("rank_type"),
							   _rank_type_desc: row.Field<string>("rank_type_desc"),
							   _rank: row.Field<short?>("rank")))
						.ToList<Rank>();
		}

		public IEnumerator<Rank> GetEnumerator()
		{
			return ranks.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
}
