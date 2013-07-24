using System;

namespace Ambitus
{
	public class AddressCreationParams
	{
		public string Phone1 { get; set; }
		public string Phone2 { get; set; }
		public string SubStreetAddress { get; set; }
		public string PostalCode { get; set; }
		public string Fax { get; set; }
		public int? AddressTypeId { get; set; }
		public DateTime? StartDate { get; set; }
		public DateTime? EndDate { get; set; }
		public string Months { get; set; }
		public int[] MailPurposeIds { get; set; }
	}
}
