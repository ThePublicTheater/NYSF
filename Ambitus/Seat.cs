namespace Ambitus
{
	public class Seat
	{
		public int? SectionId { get; private set; }
		public string RowNum { get; private set; }
		public string SeatNum { get; private set; }
		public short? StatusId { get; private set; }
		public int? Id { get; private set; }
		public int? ZoneId { get; private set; }
		public int? AllocationId { get; private set; }
		public int? SeatTypeId { get; private set; }
		public int? LogicalRowNum { get; private set; }
		public int? LogicalSeatNum { get; private set; }
		public short? ScreenX { get; private set; }
		public short? ScreenY { get; private set; }
		public bool? IsSeat { get; private set; }
		public bool? IsOnAisle { get; private set; }
		public bool? IsAccessibleWithoutStairs { get; private set; }
		public char? DisplayChar { get; private set; }
		public int? ScreenId { get; private set; }

		public Seat(int? _SectionId, string _RowNum, string _SeatNum, short? _StatusId,
				int? _Id, int? _ZoneId, int? _AllocationId, int? _SeatTypeId, int? _LogicalRowNum,
				int? _LogicalSeatNum, short? _ScreenX, short? _ScreenY, bool? _IsSeat,
				bool? _IsOnAisle, bool? _IsAccessibleWithoutStairs, char? _DisplayChar,
				int? _ScreenId)
		{
			SectionId = _SectionId;
			RowNum = _RowNum;
			SeatNum = _SeatNum;
			StatusId = _StatusId;
			Id = _Id;
			ZoneId = _ZoneId;
			AllocationId = _AllocationId;
			SeatTypeId = _SeatTypeId;
			LogicalRowNum = _LogicalRowNum;
			LogicalSeatNum = _LogicalSeatNum;
			ScreenX = _ScreenX;
			ScreenY = _ScreenY;
			IsSeat = _IsSeat;
			IsOnAisle = _IsOnAisle;
			IsAccessibleWithoutStairs = _IsAccessibleWithoutStairs;
			DisplayChar = _DisplayChar;
			ScreenId = _ScreenId;
		}
	}
}
