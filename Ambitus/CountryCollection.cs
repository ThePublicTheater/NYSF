using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Ambitus
{
	public class CountryCollection : Internals.ApiResultInterpreter, IEnumerable<Country>
	{
		List<Country> countries;

		public Country this[int i]
		{
			get
			{
				return countries[i];
			}
		}

		public int Count
		{
			get
			{
				return countries.Count;
			}
		}

		public CountryCollection(DataTable tessResults)
		{
			countries = (from rows in tessResults.AsEnumerable()
						 select new Country(
								id: rows.Field<int?>("id"),
								name: rows.Field<string>("description"),
								abbrev: rows.Field<string>("short_desc"),
								stateUsePolicy:
										ToStateProvUsePolicy(rows.Field<string>("use_state_field")))
						).ToList<Country>();
		}

		public Country GetById(int id)
		{
			return (from c in countries
					where c.Id == id
					select c).FirstOrDefault<Country>();
		}

		public IEnumerator<Country> GetEnumerator()
		{
			return countries.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
}
