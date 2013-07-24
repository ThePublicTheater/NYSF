using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Ambitus
{
	public class SyosPriceTypeCollection : IEnumerable<SyosPriceType>
	{
		private List<SyosPriceType> priceTypes;

		public SyosPriceType this[int i]
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

		public SyosPriceTypeCollection(DataTable priceTypesTable)
		{
			priceTypes = (from row in priceTypesTable.AsEnumerable()
					 select new SyosPriceType(
						_Id: row.Field<int?>("Id"),
						_Description: row.Field<string>("Description"),
						_ShortDescription: row.Field<string>("ShortDescription"),
						_Promotion: row.Field<bool?>("Promotion"),
						_PromoMaxQuantity: row.Field<int?>("PromoMaxQuantity")))
					.ToList<SyosPriceType>();
		}

		public SyosPriceType GetById(int id)
		{
			return (from p in priceTypes where p.Id == id select p).FirstOrDefault<SyosPriceType>();
		}

		public IEnumerator<SyosPriceType> GetEnumerator()
		{
			return priceTypes.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
}
