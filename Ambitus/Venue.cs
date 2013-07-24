using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ambitus
{
	public class Venue
	{
		public int? Id { get; private set; }
		public string FacilityName { get; private set; }
		public string TheaterName { get; private set; }
		public string CombinedName { get; private set; }

		public Venue(int? id, string facilityName, string theaterName, string combinedName)
		{
			Id = id;
			FacilityName = facilityName;
			TheaterName = theaterName;
			CombinedName = combinedName;
		}

		public override string ToString()
		{
			return CombinedName ?? String.Empty;
		}
	}
}
