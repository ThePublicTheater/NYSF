using System;

namespace Ambitus
{
	public class Address
	{
		public int? Id { get; private set; }
		public int? ConstituentId { get; private set; }
		public int? TypeId { get; private set; }
		public string TypeName { get; private set; }
		public string StreetAddress { get; private set; }
		public string SubStreetAddress { get; private set; }
		public string City { get; private set; }
		public string State { get; private set; }
		public string PostalCode { get; private set; }
		public string PostalCodeFormat { get; private set; }
		public int? CountryId { get; private set; }
		public string CountryLongName { get; private set; }
		public string CountryShortName { get; private set; }
		public DateTime? StartDate { get; private set; }
		public DateTime? EndDate { get; private set; }
		public string Months { get; private set; }
		public bool? IsPrimary { get; private set; }
		public bool? IsActive { get; private set; }
		public string MailPurposes { get; private set; }
		public int? GeoArea { get; private set; }

		public Address(int? id, int? constituentId, int? typeId, string typeName,
				string streetAddress, string subStreetAddress, string city, string state,
				string postalCode, string postalCodeFormat, int? countryId, string countryLongName,
				string countryShortName, DateTime? startDate, DateTime? endDate, string months,
				bool? isPrimary, bool? isActive, string mailPurposes, int? geoArea)
		{
			Id = id;
			ConstituentId = constituentId;
			TypeId = typeId;
			TypeName = typeName;
			StreetAddress = streetAddress;
			SubStreetAddress = subStreetAddress;
			City = city;
			State = state;
			PostalCode = postalCode;
			PostalCodeFormat = postalCodeFormat;
			CountryId = countryId;
			CountryLongName = countryLongName;
			CountryShortName = countryShortName;
			StartDate = startDate;
			EndDate = endDate;
			Months = months;
			IsPrimary = isPrimary;
			IsActive = isActive;
			MailPurposes = mailPurposes;
			GeoArea = geoArea;
		}

		public override string ToString()
		{
			return StreetAddress ?? String.Empty;
		}
	}

}
