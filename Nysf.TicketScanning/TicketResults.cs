using System;

namespace Nysf.TicketScanning
{
	public class TicketResults
	{
		public string Message { get; set; }
		public string Row { get; set; }
		public string Seat { get; set; }
		public string PartyName { get; set; }
		public int PartyCount { get; set; }
		public DateTime OrderDate { get; set; }
		public bool PartyHasMembership { get; set; }
		public bool PartyHasPartnership { get; set; }
		public string PerfTitle { get; set; }
		public DateTime PerfDate { get; set; }
	}
}