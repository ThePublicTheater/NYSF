using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Ambitus
{
	public class PriceTypeRestrictions : IEnumerable<int>
	{
		private List<int> priceTypeIds;
		private bool includeAll;

		public int this[int i]
		{
			get
			{
				return priceTypeIds[i];
			}
		}

		public int Count
		{
			get
			{
				if (priceTypeIds == null)
				{
					return 0;
				}
				return priceTypeIds.Count;
			}
		}

		public bool IncludeAllPriceTypes
		{
			get
			{
				return includeAll;
			}
		}

		public PriceTypeRestrictions()
			: this(null, true) { }

		public PriceTypeRestrictions(int priceTypeId)
			: this(new List<int> { priceTypeId }, false) { }

		public PriceTypeRestrictions(params int[] priceTypeIds)
			: this(priceTypeIds.ToList<int>(), false) { }

		public PriceTypeRestrictions(List<int> priceTypeIds)
			: this(priceTypeIds, false) { }

		private PriceTypeRestrictions(List<int> priceTypeIds, bool includeAllPriceTypes)
		{
			this.priceTypeIds = priceTypeIds;
			this.includeAll = includeAllPriceTypes;
		}

		public IEnumerator<int> GetEnumerator()
		{
			return priceTypeIds.AsEnumerable<int>().GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
}
