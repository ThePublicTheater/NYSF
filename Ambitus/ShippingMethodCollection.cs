using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Ambitus
{
	public class ShippingMethodCollection
			: Internals.ApiResultInterpreter, IEnumerable<ShippingMethod>
	{
		List<ShippingMethod> methods;

		public ShippingMethod this[int i]
		{
			get
			{
				return methods[i];
			}
		}

		public int Count
		{
			get
			{
				return methods.Count;
			}
		}

		public ShippingMethodCollection(DataTable tessResults)
		{
			methods = (from row in tessResults.AsEnumerable()
					   select new ShippingMethod(
							id: row.Field<int?>("id"),
							name: row.Field<string>("description"),
							restriction:
									ToShippingMethodRestriction(row.Field<string>("restricted")),
							isDefault: ToBool(row.Field<string>("default"))))
						.ToList<ShippingMethod>();
		}

		public ShippingMethod GetById(int id)
		{
			return (from p in methods
					where p.Id == id
					select p).FirstOrDefault<ShippingMethod>();
		}

		public IEnumerator<ShippingMethod> GetEnumerator()
		{
			return methods.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
}
