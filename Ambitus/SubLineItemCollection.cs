using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Ambitus
{
	public class SubLineItemCollection
			: Internals.ApiResultInterpreter, IEnumerable<SubLineItem>
	{
		List<SubLineItem> subLineItems;

		public SubLineItem this[int i]
		{
			get
			{
				return subLineItems[i];
			}
		}

		public int Count
		{
			get
			{
				return subLineItems.Count;
			}
		}

		public SubLineItemCollection(DataTable tessResults, int? lineItemId)
		{
			subLineItems = (from row in tessResults.AsEnumerable()
							where row.Field<int?>("li_seq_no") == lineItemId
						  select new SubLineItem(
							    _sli_no: row.Field<int?>("sli_no"),
								_due_amt: row.Field<decimal?>("due_amt"),
								_paid_amt: row.Field<decimal?>("paid_amt"),
								_price_type: row.Field<short?>("price_type"),
								_seat_no: row.Field<int?>("seat_no"),
								_comp_code: row.Field<short?>("comp_code"),
								_sli_status: row.Field<short?>("sli_status"),
								_perf_no: row.Field<int?>("perf_no"),
								_pkg_no: row.Field<int?>("pkg_no"),
								_zone_no: row.Field<int?>("zone_no"),
								_ret_parent_sli_no: row.Field<int?>("ret_parent_sli_no"),
								_order_no: row.Field<int?>("order_no"),
								_seat_row: row.Field<string>("seat_row"),
								_seat_num: row.Field<string>("seat_num"),
								_zone_desc: row.Field<string>("zone_desc"),
								_section_desc: row.Field<string>("section_desc"),
								_section_short_desc: row.Field<string>("section_short_desc"),
								_section_print_desc: row.Field<string>("section_print_desc"),
								_section_add_text: row.Field<string>("section_add_text"),
								_section_add_text2: row.Field<string>("section_add_text2"),
								_db_status: row.Field<short?>("db_status"),
								_prod_season_no: row.Field<int?>("prod_season_no"),
								_facility_no: row.Field<int?>("facility_no"),
								_seat_type: row.Field<short?>("seat_type"),
								_seat_type_desc: row.Field<string>("seat_type_desc")))
						.ToList<SubLineItem>();
		}

		public SubLineItem GetById(int id)
		{
			return (from p in subLineItems
					where p.sli_no == id
					select p).FirstOrDefault<SubLineItem>();
		}

		public IEnumerator<SubLineItem> GetEnumerator()
		{
			return subLineItems.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
}
