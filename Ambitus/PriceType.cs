using System;

namespace Ambitus
{
	public class PriceType
	{
		public int? Id { get; private set; }
		public string Name { get; private set; }
		public string ShortName { get; private set; }
		public string Category { get; private set; }
		public bool? IsDefault { get; private set; }
		public bool? IsPromo { get; private set; }
		public int? PromoRank { get; private set; }
		public int? PromoMax { get; private set; }
		public string PromoText { get; private set; }

		public PriceType(int? id, string name, string shortName, string category, bool? isDefault,
				bool? isPromo, int? promoRank, int? promoMax, string promoText)
		{
			Id = id;
			Name = name;
			ShortName = shortName;
			Category = category;
			IsDefault = isDefault;
			IsPromo = isPromo;
			PromoRank = promoRank;
			PromoMax = promoMax;
			PromoText = promoText;
		}

		public override string ToString()
		{
			return Name ?? String.Empty;
		}
	}
}
