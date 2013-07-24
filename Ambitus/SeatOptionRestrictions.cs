namespace Ambitus
{
	public class SeatOptionRestrictions
	{
		public int[] ZoneIds { get; set; }
		public short[] SectionIds { get; set; }
		public short[] ScreenIds { get; set; }
		public bool? Summarize { get; set; }
		public bool? CalculatePackageAllocDetails { get; set; }
		public PriceTypeRestrictions RequiredPriceTypes { get; set; }
		public bool? GetNonSeats { get; set; }
	}
}
