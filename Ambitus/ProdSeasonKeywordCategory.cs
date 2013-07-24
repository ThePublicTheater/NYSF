using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Ambitus
{
	public class ProdSeasonKeywordCategory
	{
		public string Name { get; private set; }
		public List<string> Keywords { get; private set; }

		public ProdSeasonKeywordCategory(EnumerableRowCollection<DataRow> tessResults,
				string catName, int? prodSeasonId)
		{
			Keywords = (from k in tessResults
						where k.Field<string>("category") == catName
							&& k.Field<int?>("prod_season_no") == prodSeasonId
						  select k.Field<string>("keyword"))
						.ToList<string>();
			Name = catName;
		}

		public override string ToString()
		{
			return Name ?? String.Empty;
		}
	}
}
