using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Ambitus
{
	public class StateProvinceCollection : IEnumerable<StateProvince>
	{
		List<StateProvince> states;

		public StateProvince this[int i]
		{
			get
			{
				return states[i];
			}
		}

		public int Count
		{
			get
			{
				return states.Count;
			}
		}

		public StateProvinceCollection(DataTable tessResults)
		{
			states = (from rows in tessResults.AsEnumerable()
						 select new StateProvince(
								name: rows.Field<string>("description"),
								abbrev: rows.Field<string>("id"),
								countryId: rows.Field<int?>("country"))).ToList<StateProvince>();
		}

		private StateProvinceCollection(List<StateProvince> subset)
		{
			states = subset;
		}

		public StateProvinceCollection GetByCountryId(int id)
		{
			List<StateProvince> subset = (from item in states
										 where item.CountryId == id
										 select item).ToList<StateProvince>();
			if (subset.Count == 0)
			{
				return null;
			}
			return new StateProvinceCollection(subset);
		}

		public IEnumerator<StateProvince> GetEnumerator()
		{
			return states.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
}
