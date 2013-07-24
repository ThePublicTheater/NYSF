using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Ambitus
{
	public class SyosZonePriceOptionCollection : IEnumerable<SyosZonePriceOption>
	{
		private List<SyosZonePriceOption> options;

		public SyosZonePriceOption this[int i]
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

		public SyosZonePriceOptionCollection(DataTable zonePricesTable)
		{
			options = (from row in zonePricesTable.AsEnumerable()
					 select new SyosZonePriceOption(
						 _Id: row.Field<int?>("Id"),
						_Price: row.Field<decimal?>("Price"),
						_BasePrice: row.Field<decimal?>("BasePrice"),
						_AvailCount: row.Field<int?>("AvailCount"),
						_Zone: row.Field<int?>("Zone")))
					.ToList<SyosZonePriceOption>();
		}

		public IEnumerator<SyosZonePriceOption> GetEnumerator()
		{
			return options.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
}
