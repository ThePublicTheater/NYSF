using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Ambitus
{
	public class ZonePriceDefaultCollection
			: Internals.ApiResultInterpreter, IEnumerable<ZonePriceDefault>
	{
		List<ZonePriceDefault> defaults;

		public ZonePriceDefault this[int i]
		{
			get
			{
				return defaults[i];
			}
		}

		public int Count
		{
			get
			{
				return defaults.Count;
			}
		}

		public ZonePriceDefaultCollection(DataTable tessResults /* "Price" */)
		{
			defaults = (from row in tessResults.AsEnumerable()
						select new ZonePriceDefault(
							 perfId: row.Field<int?>("perf_no"),
							 packageId: row.Field<int?>("pkg_no"),
							 zoneId: row.Field<int?>("zone_no"),
							 priceTypeId: row.Field<int?>("price_type"),
							 price: row.Field<decimal?>("price"),
							 basePrice: row.Field<decimal?>("base_price"),
							 zoneName: row.Field<string>("description"),
							 available: ToBool(row.Field<string>("available")),
							 availableCount: row.Field<int?>("avail_count")))
						.ToList<ZonePriceDefault>();
		}

		public IEnumerator<ZonePriceDefault> GetEnumerator()
		{
			return defaults.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
}
