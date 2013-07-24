using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Ambitus
{
	public class ZoneCollection : IEnumerable<Zone>
	{
		private List<Zone> zones;

		public Zone this[int i]
		{
			get
			{
				return zones[i];
			}
		}

		public int Count
		{
			get
			{
				return zones.Count;
			}
		}

		public ZoneCollection(DataTable zonesTable)
		{
			zones = (from z in zonesTable.AsEnumerable()
					   select new Zone(
						   id: z.Field<int?>("Zone"),
						   name: z.Field<string>("Description"))).ToList<Zone>();
		}

		public IEnumerator<Zone> GetEnumerator()
		{
			return zones.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
}
