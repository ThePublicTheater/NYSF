using System;

namespace Ambitus
{
	public class SyosPriceType
	{
		public int? Id { get; private set; }
		public string Name { get; private set; }
		public string ShortName { get; private set; }
		public bool? IsPromo { get; private set; }
		public int? MaxTicketsForPromo { get; private set; }

		public SyosPriceType(int? _Id, string _Description, string _ShortDescription,
				bool? _Promotion, int? _PromoMaxQuantity)
		{
			Id = _Id;
			Name = _Description;
			ShortName = _ShortDescription;
			IsPromo = _Promotion;
			MaxTicketsForPromo = _PromoMaxQuantity;
		}

		public override string ToString()
		{
			return Name ?? String.Empty;
		}
	}
}
