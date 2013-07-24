namespace Ambitus
{
	public class SyosZonePriceOption
	{
		public int? Id { get; private set; }
		public decimal? Price { get; private set; }
		public decimal? BasePrice { get; private set; }
		public int? AvailableSeatCount { get; private set; }
		public int? ZoneId { get; private set; }

		public SyosZonePriceOption(int? _Id, decimal? _Price, decimal? _BasePrice,
				int? _AvailCount, int? _Zone)
		{
			Id = _Id;
			Price = _Price;
			BasePrice = _BasePrice;
			AvailableSeatCount = _AvailCount;
			ZoneId = _Zone;
		}
	}
}
