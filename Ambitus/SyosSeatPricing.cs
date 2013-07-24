using System;
using System.Data;
using System.Linq;

namespace Ambitus
{
	public class SyosSeatPricing
	{
		public int? PerfId { get; private set; }
		public string PerfName { get; private set; }
		public DateTime? PerfDateTime { get; private set; }
		public string ZoneMapId { get; private set; }
		public string VenueName { get; private set; }
		public bool? IsPromo { get; private set; }
		public bool? IsGenerallyAvailable { get; private set; }
		public string TimeSlotName { get; private set; }
		public ZoneCollection Zones { get; private set; }
		public SyosPriceTypeCollection PriceTypes { get; private set; }
		public SyosZonePriceOptionCollection ZonePriceTypeCombos { get; private set; }

		public SyosSeatPricing(DataTable perfTable, DataTable priceTypesTable,
				DataTable zonesTable, DataTable zonePriceTypesTable)
		{
			var results = (from row in perfTable.AsEnumerable()
						   select new
						   {
							   Id = row.Field<int?>("Id"),
							   Title = row.Field<string>("Title"),
							   PerformanceDate = row.Field<DateTime?>("PerformanceDate"),
							   ZoneMap = row.Field<string>("ZoneMap"),
							   Venue = row.Field<string>("Venue"),
							   PromoInd = row.Field<bool?>("PromoInd"),
							   GeneralAvailability = row.Field<bool?>("GeneralAvailability"),
							   TimeSlotDescription = row.Field<string>("TimeSlotDescription")
						   }).Single();
			PerfId = results.Id;
			PerfName = results.Title;
			PerfDateTime = results.PerformanceDate;
			ZoneMapId = results.ZoneMap;
			VenueName = results.Venue;
			IsPromo = results.PromoInd;
			IsGenerallyAvailable = results.GeneralAvailability;
			TimeSlotName = results.TimeSlotDescription;
			Zones = new ZoneCollection(zonesTable);
			PriceTypes = new SyosPriceTypeCollection(priceTypesTable);
			ZonePriceTypeCombos = new SyosZonePriceOptionCollection(zonePriceTypesTable);
		}
	}
}
