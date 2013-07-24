using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Ambitus
{
	public class SeatTypeCollection
			: Internals.ApiResultInterpreter, IEnumerable<SeatType>
	{
		List<SeatType> seatTypes;

		public SeatType this[int i]
		{
			get
			{
				return seatTypes[i];
			}
		}

		public int Count
		{
			get
			{
				return seatTypes.Count;
			}
		}

		public SeatTypeCollection(DataTable tessResults)
		{
			seatTypes = (from row in tessResults.AsEnumerable()
						  select new SeatType(
							   id: row.Field<int?>("seat_type"),
							   name: row.Field<string>("seat_type_desc")))
						.ToList<SeatType>();
		}

		public SeatType GetById(int id)
		{
			return (from p in seatTypes
					where p.Id == id
					select p).FirstOrDefault<SeatType>();
		}

		public IEnumerator<SeatType> GetEnumerator()
		{
			return seatTypes.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
}