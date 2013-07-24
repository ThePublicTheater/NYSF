namespace Ambitus
{
	public class AddressUpdateClearingParams
	{
		public string Phone1 { get; set; }
		public string StreetAddress { get; set; }
		public string City { get; set; }
		public string PostalCode { get; set; }
		public string Phone2 { get; set; }
		public string Fax { get; set; }
		public string Months { get; set; }
		public bool? SetToInactive { get; private set; }
	}
}
