using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Ambitus
{
	public class LineItemCollection
			: Internals.ApiResultInterpreter, IEnumerable<LineItem>
	{
		List<LineItem> lineItems;

		public LineItem this[int i]
		{
			get
			{
				return lineItems[i];
			}
		}

		public int Count
		{
			get
			{
				return lineItems.Count;
			}
		}

		public LineItemCollection(DataTable liTable, DataTable sliTable)
		{
			lineItems = (from row in liTable.AsEnumerable()
						  select new LineItem(
							    _li_seq_no: row.Field<int?>("li_seq_no"),
								_li_no: row.Field<int?>("li_no"),
								_order_no: row.Field<int?>("order_no"),
								_pkg_no: row.Field<int?>("pkg_no"),
								_perf_no: row.Field<int?>("perf_no"),
								_priority: row.Field<short?>("priority"),
								_zone_no: row.Field<short?>("zone_no"),
								_alt_upgrd_ind: row.Field<string>("alt_upgrd_ind"),
								_pkg_li_no: row.Field<int?>("pkg_li_no"),
								_primary_ind: ToBool(row.Field<string>("primary_ind")),
								_perf_code: row.Field<string>("perf_code"),
								_perf_dt: row.Field<DateTime?>("perf_dt"),
								_perf_desc: row.Field<string>("perf_desc"),
								_pkg_code: row.Field<string>("pkg_code"),
								_pkg_desc: row.Field<string>("pkg_desc"),
								_facility_desc: row.Field<string>("facility_desc"),
								_db_status: row.Field<short?>("db_status"),
								_notes: row.Field<string>("notes"),
								_zmap_no: row.Field<int?>("zmap_no"),
								_text1: row.Field<string>("text1"),
								_text2: row.Field<string>("text2"),
								_text3: row.Field<string>("text3"),
								_text4: row.Field<string>("text4"),
								_prod_type: row.Field<short?>("prod_type"),
								_prod_type_desc: row.Field<string>("prod_type_desc"),
								_category: row.Field<int?>("category"),
								_assoc_customer_no: row.Field<int?>("assoc_customer_no"),
								_season_no: row.Field<short?>("season_no"),
								_season_desc: row.Field<string>("season_desc"),
								_ga_ind: ToBool(row.Field<string>("ga_ind")),
								_facil_no: row.Field<int?>("facil_no"),
								sliTable: sliTable))
						.ToList<LineItem>();
		}

		public LineItem GetById(int id)
		{
			return (from p in lineItems
					where p.Id == id
					select p).FirstOrDefault<LineItem>();
		}

		public IEnumerator<LineItem> GetEnumerator()
		{
			return lineItems.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
}
