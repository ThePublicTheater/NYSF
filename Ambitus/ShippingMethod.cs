using System;

namespace Ambitus
{
	public class ShippingMethod
	{
		public int? Id { get; private set; }
		public string Name { get; private set; }
		public ShippingMethodRestriction? Restriction { get; private set; }
		public bool? IsDefault { get; private set; }

		public ShippingMethod(int? id, string name, ShippingMethodRestriction? restriction,
				bool? isDefault)
		{
			Id = id;
			Name = name;
			Restriction = restriction;
			IsDefault = isDefault;
		}

		public override string ToString()
		{
			return Name ?? String.Empty;
		}
	}
}