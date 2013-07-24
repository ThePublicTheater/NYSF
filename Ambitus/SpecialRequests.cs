namespace Ambitus
{
	public class SpecialRequests
	{
		public int? MinContiguousSeats { get; set; }
		public int? NumOfWheelChairSeats { get; set; }
		public bool? EnsureNoStairs { get; set; }
		public AisleSeatPref? AislePref { get; set; }
		public decimal? StartingPrice { get; set; }
		public decimal? EndingPrice { get; set; }
		public string StartingRow { get; set; }
		public string EndingRow { get; set; }
		public string StartingSeat { get; set; }
		public string EndingSeat { get; set; }
		public bool? AllowLeaveSingleSeats { get; set; }
	}
}