using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Ambitus
{
	public class FeeCategoryCollection
			: Internals.ApiResultInterpreter, IEnumerable<FeeCategory>
	{
		List<FeeCategory> categories;

		public FeeCategory this[int i]
		{
			get
			{
				return categories[i];
			}
		}

		public int Count
		{
			get
			{
				return categories.Count;
			}
		}

		public FeeCategoryCollection(DataTable tessResults)
		{
			categories = (from row in tessResults.AsEnumerable()
						  select new FeeCategory(
							  id: row.Field<short?>("category"),
							  name: row.Field<string>("category_desc"),
							  total: row.Field<decimal?>("total_fees")))
						.ToList<FeeCategory>();
		}

		public FeeCategory GetById(int id)
		{
			return (from p in categories
					where p.Id == id
					select p).FirstOrDefault<FeeCategory>();
		}

		public IEnumerator<FeeCategory> GetEnumerator()
		{
			return categories.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
}