using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Ambitus
{
	public class CreditCollection : IEnumerable<Credit>
	{
		private List<Credit> credits;

		public Credit this[int i]
		{
			get
			{
				return credits[i];
			}
		}

		public int Count
		{
			get
			{
				return credits.Count;
			}
		}

		public CreditCollection(DataTable tessResults)
		{
			credits = (from item in tessResults.AsEnumerable()
					   select new Credit(
							typeName: item.Field<string>("credit_type"),
							roleId: item.Field<int?>("role_no"),
							roleName: item.Field<string>("role_desc"),
							personName: item.Field<string>("full_name"),
							artistId: item.Field<int?>("artist_no"),
							rank: item.Field<int?>("rank"))).ToList<Credit>();
		}

		public CreditCollection(EnumerableRowCollection<DataRow> enumCredits,
				string criteriaFieldName, int? criteriaFieldValue)
		{
			credits = (from c in enumCredits
						   where c.Field<int?>(criteriaFieldName) == criteriaFieldValue
					   select new Credit(
							typeName: c.Field<string>("credit_type"),
							roleId: c.Field<int?>("role_no"),
							roleName: c.Field<string>("role_desc"),
							personName: c.Field<string>("full_name"),
							artistId: c.Field<int?>("artist_no"),
							rank: c.Field<int?>("rank"))).ToList<Credit>();
		}

		public IEnumerator<Credit> GetEnumerator()
		{
			return credits.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
}
