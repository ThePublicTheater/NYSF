using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Nysf.Tessitura
{
	public class SyosSeatRequest
	{
		public int SeatId { get; private set; }
		public int ZoneId { get; private set; }
		public int PriceTypeId { get; private set; }

		public SyosSeatRequest(int zoneId, int seatId, int priceTypeId)
		{
			SeatId = seatId;
			ZoneId = zoneId;
			PriceTypeId = priceTypeId;
		}
	}
}