using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Ambitus
{
	public class ZonePriceOptionCollection
			: Internals.ApiResultInterpreter, IEnumerable<ZonePriceOption>
	{
		List<ZonePriceOption> options;

		public ZonePriceOption this[int i]
		{
			get
			{
				return options[i];
			}
		}

		public int Count
		{
			get
			{
				return options.Count;
			}
		}

		public ZonePriceOptionCollection(DataTable tessResults /* "AllPrice" */)
		{
			options = (from row in tessResults.AsEnumerable()
						  select new ZonePriceOption(
							   zoneId: row.Field<int?>("zone_no"),
							   priceTypeId: row.Field<int?>("price_type"),
							   price: row.Field<decimal?>("price"),
							   basePrice: row.Field<decimal?>("base_price"),
							   zoneName: row.Field<string>("description"),
							   available: ToBool(row.Field<string>("available")),
							   availableCount: row.Field<int?>("avail_count"),
							   zoneDefaultPriceTypeId: row.Field<int?>("def_price_type"),
							   priceTypeName: row.Field<string>("price_type_desc"),
							   rank: row.Field<int?>("rank")))
						.ToList<ZonePriceOption>();
		}

		private ZonePriceOptionCollection(List<ZonePriceOption> subset)
		{
			options = subset;
		}

		public ZonePriceOptionCollection GetAvailableZonePriceOptions(
				PriceTypeCollection priceTypes)
		{
			List<ZonePriceOption> subset = (from z in options
											join p in priceTypes
											on z.PriceTypeId equals p.Id
											select z).ToList<ZonePriceOption>();
			if (subset.Count == 0)
			{
				return null;
			}
			return new ZonePriceOptionCollection(subset);
		}

		public int[] GetZoneIds()
		{
			int[] results = (from z in options
							 select z.ZoneId.Value).ToArray<int>();
			if (results.Length == 0)
			{
				return null;
			}
			return results;
		}

		public IEnumerator<ZonePriceOption> GetEnumerator()
		{
			return options.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
}
