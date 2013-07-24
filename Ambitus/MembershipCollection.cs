using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Ambitus
{
	public class MembershipCollection
			: Internals.ApiResultInterpreter, IEnumerable<Membership>
	{
		List<Membership> memberships;

		public Membership this[int i]
		{
			get
			{
				return memberships[i];
			}
		}

		public int Count
		{
			get
			{
				return memberships.Count;
			}
		}

		public MembershipCollection(DataTable tessResults)
		{
			memberships = (from row in tessResults.AsEnumerable()
						  select new Membership(
							    _memb_org_no: row.Field<int?>("memb_org_no"),
								_current_status: row.Field<int?>("current_status"),
								_memb_level: row.Field<string>("memb_level"),
								_init_dt: row.Field<DateTime?>("init_dt"),
								_expr_dt: row.Field<DateTime?>("expr_dt"),
								_current_status_desc: row.Field<string>("current_status_desc"),
								_memb_level_desc: row.Field<string>("memb_level_desc"),
								_memb_level_category: row.Field<int?>("memb_level_category"),
								_category_desc: row.Field<string>("category_desc"),
								_memb_org_desc: row.Field<string>("memb_org_desc"),
								_inception_dt: row.Field<DateTime?>("inception_dt"),
								_ben_provider: row.Field<int?>("ben_provider"),
								_lapse_dt: row.Field<DateTime?>("lapse_dt"),
								_renewal_dt: row.Field<DateTime?>("renewal_dt"),
								_declined_ind: ToBool(row.Field<string>("declined_ind")),
								_cust_memb_no: row.Field<int?>("cust_memb_no")))
						.ToList<Membership>();
		}

		public IEnumerator<Membership> GetEnumerator()
		{
			return memberships.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
}
