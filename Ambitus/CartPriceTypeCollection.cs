using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Ambitus
{
	public class CartPriceTypeCollection
			: Internals.ApiResultInterpreter, IEnumerable<CartPriceType>
	{
		List<CartPriceType> priceTypes;

		public CartPriceType this[int i]
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

		public CartPriceTypeCollection(DataTable tessResults)
		{
			priceTypes = (from row in tessResults.AsEnumerable()
						  select new CartPriceType(
								id: row.Field<short?>("price_type"),
								name: row.Field<string>("description"),
								shortName: row.Field<string>("short_desc"),
								categoryName: row.Field<string>("category"),
								isDefault: ToBool(row.Field<string>("def_price_type"))))
						.ToList<CartPriceType>();
		}

		public CartPriceType GetById(int id)
		{
			return (from p in priceTypes
					where p.Id == id
					select p).FirstOrDefault<CartPriceType>();
		}

		public IEnumerator<CartPriceType> GetEnumerator()
		{
			return priceTypes.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
}
