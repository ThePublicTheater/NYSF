using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Ambitus
{
	public class VenueCollection : IEnumerable<Venue>
	{
		private List<Venue> venues;

		public Venue this[int i]
		{
			get
			{
				return venues[i];
			}
		}

		public int Count
		{
			get
			{
				return venues.Count;
			}
		}

		public VenueCollection(DataTable tessResults)
		{
			venues = (from row in tessResults.AsEnumerable()
					  select new Venue(
							id: row.Field<int?>("facil_no"),
							facilityName: row.Field<string>("facilityDesc"),
							theaterName: row.Field<string>("theaterDesc"),
							combinedName: row.Field<string>("CombineDesc"))).ToList<Venue>();
		}

		public Venue GetById(int id)
		{
			return (from item in venues
					where item.Id == id
					select item).FirstOrDefault<Venue>();
		}

		public IEnumerator<Venue> GetEnumerator()
		{
			return venues.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
}
