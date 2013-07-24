using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Ambitus
{
	public class PriceTypeCollection
			: Internals.ApiResultInterpreter, IEnumerable<PriceType>
	{
		List<PriceType> priceTypes;

		public PriceType this[int i]
		{
			get
			{
				return priceTypes[i];
			}
		}

		public int Count
		{
			get
			{
				return priceTypes.Count;
			}
		}

		public PriceTypeCollection(DataTable tessResults /* "PriceType" */)
		{
			priceTypes = (from row in tessResults.AsEnumerable()
						select new PriceType(
							 id: row.Field<int?>("price_type"),
							 name: row.Field<string>("description"),
							 shortName: row.Field<string>("short_desc"),
							 category: row.Field<string>("category"),
							 isDefault: ToBool(row.Field<string>("def_price_type")),
							 isPromo: ToBool(row.Field<string>("promo")),
							 promoRank: row.Field<int?>("promo_rank"),
							 promoMax: row.Field<int?>("promo_max"),
							 promoText: row.Field<string>("promo_text")))
						.ToList<PriceType>();
		}

		public PriceType GetById(int id)
		{
			return (from p in priceTypes
					where p.Id == id
					select p).FirstOrDefault<PriceType>();
		}

		public IEnumerator<PriceType> GetEnumerator()
		{
			return priceTypes.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
}
