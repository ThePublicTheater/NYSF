using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Ambitus
{
	public class ProdSeasonKeywordCatCollection
			: Internals.ApiResultInterpreter, IEnumerable<ProdSeasonKeywordCategory>
	{
		private List<ProdSeasonKeywordCategory> categories;

		public ProdSeasonKeywordCategory this[int i]
		{
			get
			{
				return categories[i];
			}
		}

		public ProdSeasonKeywordCategory this[string name]
		{
			get
			{
				return (from k in categories
						where k.Name == name
						select k).FirstOrDefault<ProdSeasonKeywordCategory>();
			}
		}

		public int Count
		{
			get
			{
				return categories.Count;
			}
		}

		public ProdSeasonKeywordCatCollection(EnumerableRowCollection<DataRow> tessResults,
				int? prodSeasonId)
		{
			List<string> catNames = (from k in tessResults
									 where k.Field<int?>("prod_season_no") == prodSeasonId
									 select k.Field<string>("category"))
								.Distinct<string>().ToList<string>();
			categories = (from n in catNames
						  select new ProdSeasonKeywordCategory(tessResults, n, prodSeasonId))
						  .ToList<ProdSeasonKeywordCategory>();
		}

		public IEnumerator<ProdSeasonKeywordCategory> GetEnumerator()
		{
			return categories.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
}
