using System;

namespace Ambitus
{
	public class CartPriceType
	{
		public short? Id { get; private set; }
		public string Name { get; private set; }
		public string ShortName { get; private set; }
		public string CategoryName { get; private set; }
		public bool? IsDefault { get; private set; }

		public CartPriceType(short? id, string name, string shortName, string categoryName,
				bool? isDefault)
		{
			Id = id;
			Name = name;
			ShortName = shortName;
			CategoryName = categoryName;
			IsDefault = isDefault;
		}

		public override string ToString()
		{
			return Name ?? String.Empty;
		}
	}
}
