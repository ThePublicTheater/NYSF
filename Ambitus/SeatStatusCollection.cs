using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Ambitus
{
	public class SeatStatusCollection : Internals.ApiResultInterpreter, IEnumerable<SeatStatus>
	{
		private List<SeatStatus> statuses;

		public SeatStatus this[int i]
		{
			get
			{
				return statuses[i];
			}
		}

		public int Count
		{
			get
			{
				return statuses.Count;
			}
		}

		public SeatStatusCollection(DataTable tessResults /* "Status */)
		{
			statuses = (from item in tessResults.AsEnumerable()
							select new SeatStatus(
									id: item.Field<short?>("id"),
									code: item.Field<string>("status_code"),
									desc: item.Field<string>("description"),
									foreColor: item.Field<int?>("fore_color"),
									backColor: item.Field<int?>("back_color"),
									priority: item.Field<byte?>("status_priority"),
									legend: ToChar(item.Field<string>("status_legend"))))
							.ToList<SeatStatus>();
		}

		public SeatStatus GetById(int id)
		{
			return (from status in statuses
					where status.Id == id
					select status).FirstOrDefault<SeatStatus>();
		}

		public IEnumerator<SeatStatus> GetEnumerator()
		{
			return statuses.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
}
