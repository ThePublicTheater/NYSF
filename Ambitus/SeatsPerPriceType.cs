namespace Ambitus
{
	public class SeatsPerPriceType
	{
		public int PriceTypeId { get; set; }
		public byte NumOfSeats { get; set; }

		public SeatsPerPriceType(int priceTypeId, byte numOfSeats)
		{
			PriceTypeId = priceTypeId;
			NumOfSeats = numOfSeats;
		}
	}
}
