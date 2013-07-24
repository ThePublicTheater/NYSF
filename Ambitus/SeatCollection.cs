using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;

namespace Ambitus
{
	public class SeatCollection
			: Internals.ApiResultInterpreter, IEnumerable<Seat>
	{
		List<Seat> seats;

		public Seat this[int i]
		{
			get
			{
				return seats[i];
			}
		}

		public int Count
		{
			get
			{
				return seats.Count;
			}
		}

		public SeatCollection(DataTable tessResults /* "S" */)
		{
			seats = new List<Seat>();
			foreach (DataRow row in tessResults.Rows)
			{
				string[] data = row[0].ToString().Split(',');
				Seat seat = new Seat(
					_SectionId: String.IsNullOrWhiteSpace(data[0]) ?
							(int?)null : Int32.Parse(data[0]),
					_RowNum: String.IsNullOrWhiteSpace(data[1]) ? null : data[1],
					_SeatNum: String.IsNullOrWhiteSpace(data[2]) ? null : data[2],
					_StatusId: String.IsNullOrWhiteSpace(data[3]) ?
							(short?)null : Int16.Parse(data[3]),
					_Id: String.IsNullOrWhiteSpace(data[4]) ?
							(int?)null : Int32.Parse(data[4]),
					_ZoneId: String.IsNullOrWhiteSpace(data[5]) ?
							(int?)null : Int32.Parse(data[5]),
					_AllocationId: String.IsNullOrWhiteSpace(data[6]) ?
							(int?)null : Int32.Parse(data[6]),
					_SeatTypeId: String.IsNullOrWhiteSpace(data[7]) ?
							(int?)null : Int32.Parse(data[7]),
					_LogicalRowNum: String.IsNullOrWhiteSpace(data[8]) ?
							(int?)null : Int32.Parse(data[8]),
					_LogicalSeatNum: String.IsNullOrWhiteSpace(data[9]) ?
							(int?)null : Int32.Parse(data[9]),
					_ScreenX: String.IsNullOrWhiteSpace(data[10]) ?
							(short?)null : Int16.Parse(data[10]),
					_ScreenY: String.IsNullOrWhiteSpace(data[11]) ?
							(short?)null : Int16.Parse(data[11]),
					_IsSeat: String.IsNullOrWhiteSpace(data[12]) ?
							(bool?)null : (data[12] == "1" ? true : false),
					_IsOnAisle: String.IsNullOrWhiteSpace(data[13]) ?
							(bool?)null : ToBool(data[13]),
					_IsAccessibleWithoutStairs: String.IsNullOrWhiteSpace(data[14]) ?
							(bool?)null : ToBool(data[14]),
					_DisplayChar: String.IsNullOrWhiteSpace(data[15]) ?
							(char?)null : ToChar(data[15]),
					_ScreenId: String.IsNullOrWhiteSpace(data[16]) ?
							(int?)null : Int32.Parse(data[16]));
				seats.Add(seat);
			}
		}

		public IEnumerator<Seat> GetEnumerator()
		{
			return seats.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
}
