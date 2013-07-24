using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Ambitus
{
	public class PerfCollection : Internals.ApiResultInterpreter, IEnumerable<Performance>
	{
		private List<Performance> perfs;

		public Performance this[int i]
		{
			get
			{
				return perfs[i];
			}
		}

		public int Count
		{
			get
			{
				return perfs.Count;
			}
		}

		public PerfCollection(DataTable tessResults /* "Performance" */)
		{
			// TODO: Take into account "WebContent" table
			perfs = (from row in tessResults.AsEnumerable()
					 select new Performance(
							packageId: row.Field<int?>("pkg_no"),
							id: row.Field<int?>("perf_no"),
							packageCode: row.Field<string>("pkg_code"),
							code: row.Field<string>("perf_code"),
							date: row.Field<DateTime?>("perf_date"),
							grossAvailability: row.Field<int?>("gross_availbility"),
							availabilityByCustomer: row.Field<int?>("availbility_by_customer"),
							venueId: row.Field<int?>("facility_no"),
							metCriteria: ToBool(row.Field<string>("met_criteria_in")),
							timeSlotId: row.Field<int?>("time_slot"),
							name: row.Field<string>("description"),
							onSale: ToBool(row.Field<string>("on_sale_ind")),
							businessUnit: row.Field<int?>("bu"),
							prodSeasonId: row.Field<int?>("prod_season_no"),
							noName: ToBool(row.Field<string>("no_name")),
							zmapId: row.Field<int?>("zmap_no"),
							startDate: row.Field<DateTime?>("start_dt"),
							endDate: row.Field<DateTime?>("end_dt"),
							firstDate: row.Field<DateTime?>("first_dt"),
							lastDate: row.Field<DateTime?>("last_dt"),
							venueName: row.Field<string>("facility_desc"),
							weight: row.Field<int?>("weight"),
							superPackage: ToBool(row.Field<string>("super_pkg_ind")),
							fixedSeat: ToBool(row.Field<string>("fixed_seat_ind")),
							flex: ToBool(row.Field<string>("flex_ind")),
							prodTypeId: row.Field<int?>("prod_type"),
							prodTypeName: row.Field<string>("prod_type_desc"),
							seasonId: row.Field<short?>("season_no"),
							seasonName: row.Field<string>("season_desc"),
							statusId: row.Field<int?>("perf_status"),
							statusName: row.Field<string>("perf_status_desc"),
							relevance: row.Field<int?>("relevance"),
							premiereId: row.Field<short?>("premiere_id"),
							premiereName: row.Field<string>("premiere_desc"),
							timeSlotName: row.Field<string>("time_slot_desc")))
					.ToList<Performance>();
		}

		public Performance GetById(int id)
		{
			return (from item in perfs
					where item.Id == id
					select item).FirstOrDefault<Performance>();
		}

		public IEnumerator<Performance> GetEnumerator()
		{
			return perfs.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
}
