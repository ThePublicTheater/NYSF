using System.Data;

namespace Ambitus
{
	public class ShippingOptions
	{
		public AddressOptionCollection Addresses { get; private set; }
		public ShippingMethodCollection Methods { get; private set; }

		public ShippingOptions(DataTable addressTable, DataTable methodTable)
		{
			if (addressTable.Rows.Count > 0)
			{
				Addresses = new AddressOptionCollection(addressTable);
			}
			if (methodTable.Rows.Count > 0)
			{
				Methods = new ShippingMethodCollection(methodTable);
			}
		}
	}
}
