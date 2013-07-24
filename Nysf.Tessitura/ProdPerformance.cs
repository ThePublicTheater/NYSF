using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Nysf.Tessitura
{
	[Serializable]
	public class ProdPerformance
	{
		public int Id { get; private set; }
		public DateTime StartTime { get; private set; }
		public string VenueName { get; private set; }
		public int AvailableSeatCount { get; private set; }
		public int VenueId { get; private set; }
		public int ProdTypeId { get; private set; }

		public ProdPerformance(int id, DateTime startTime, string venueName, int seats,
			int venueId, int prodTypeId)
		{
			Id = id;
			StartTime = startTime;
			VenueName = venueName;
			AvailableSeatCount = seats;
			VenueId = venueId;
			ProdTypeId = prodTypeId;
		}
	}
}