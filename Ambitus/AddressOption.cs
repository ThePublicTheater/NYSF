using System;

namespace Ambitus
{
	public class AddressOption
	{
		public int? Id { get; private set; }
		public string TypeName { get; private set; }
		public string StreetAddress { get; private set; }
		public string SubStreetAddress { get; private set; }
		public string City { get; private set; }
		public string State { get; private set; }
		public string PostalCode { get; private set; }
		public string CountryName { get; private set; }
		public bool? IsPrimary { get; private set; }
		public int? TypeId { get; private set; }
		public int? CountryId { get; private set; }
		public string Months { get; private set; }
		public DateTime? StartDate { get; private set; }
		public DateTime? EndDate { get; private set; }
		public int? alt_signor { get; private set; }
		public string MailPurposes { get; private set; }
		public bool? IsLabel { get; private set; }
		public int? GeoArea { get; private set; }
		public string DeliveryPoint { get; private set; }

		public AddressOption(int? _address_no, string _type_desc, string _street1, string _street2,
				string _city, string _state, string _postal_code, string _country, bool? _primary,
				int? _address_type, int? _country_id, string _months, DateTime? _start_dt,
				DateTime? _end_dt, int? _alt_signor, string _mail_purposes, bool? _label,
				int? _geo_area, string _delivery_point)
		{
			Id = _address_no;
			TypeName = _type_desc;
			StreetAddress = _street1;
			SubStreetAddress = _street2;
			City = _city;
			State = _state;
			PostalCode = _postal_code;
			CountryName = _country;
			IsPrimary = _primary;
			TypeId = _address_type;
			CountryId = _country_id;
			Months = _months;
			StartDate = _start_dt;
			EndDate = _end_dt;
			alt_signor = _alt_signor;
			MailPurposes = _mail_purposes;
			IsLabel = _label;
			GeoArea = _geo_area;
			DeliveryPoint = _delivery_point;
		}
	}
}
