using System;

namespace Ambitus
{
	public class ZonePriceOption
	{
		public int? ZoneId { get; private set; }
		public int? PriceTypeId { get; private set; }
		public decimal? Price { get; private set; }
		public decimal? BasePrice { get; private set; }
		public string ZoneName { get; private set; }
		public bool? Available { get; private set; }
		public int? AvailableCount { get; private set; }
		public int? ZoneDefaultPriceTypeId { get; private set; }
		public string PriceTypeName { get; private set; }
		public int? Rank { get; private set; }

		public ZonePriceOption(int? zoneId, int? priceTypeId, decimal? price, decimal? basePrice,
				string zoneName, bool? available, int? availableCount, int? zoneDefaultPriceTypeId,
				string priceTypeName, int? rank)
		{
			ZoneId = zoneId;
			PriceTypeId = priceTypeId;
			Price = price;
			BasePrice = basePrice;
			ZoneName = zoneName;
			Available = available;
			AvailableCount = availableCount;
			ZoneDefaultPriceTypeId = zoneDefaultPriceTypeId;
			PriceTypeName = priceTypeName;
			Rank = rank;
		}

		public override string ToString()
		{
			return (ZoneName ?? String.Empty) + ", " + (PriceTypeName ?? String.Empty)
				+ ", " + (Price.HasValue ? Price.Value.ToString("C") : String.Empty);
		}
	}
}
