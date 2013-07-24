namespace Ambitus
{
	public class ZonePriceDefault
	{
		public int? PerfId { get; private set; }
		public int? PackageId { get; private set; }
		public int? ZoneId { get; private set; }
		public int? PriceTypeId { get; private set; }
		public decimal? Price { get; private set; }
		public decimal? BasePrice { get; private set; }
		public string ZoneName { get; private set; }
		public bool? Available { get; private set; }
		public int? AvailableCount { get; private set; }

		public ZonePriceDefault(int? perfId, int? packageId, int? zoneId, int? priceTypeId,
				decimal? price, decimal? basePrice, string zoneName, bool? available,
				int? availableCount)
		{
			PerfId = perfId;
			PackageId = packageId;
			ZoneId = zoneId;
			PriceTypeId = priceTypeId;
			Price = price;
			BasePrice = basePrice;
			ZoneName = zoneName;
			Available = available;
			AvailableCount = availableCount;
		}
	}
}
