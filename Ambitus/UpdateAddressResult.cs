using System;
using System.Data;
using System.Linq;

namespace Ambitus
{
	public class UpdateAddressResult : Internals.ApiResultInterpreter
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
		public string Phone1 { get; private set; }
		public string Phone2 { get; private set; }
		public string Fax { get; private set; }
		public string FormattedPhone1 { get; private set; }
		public string FormattedPhone2 { get; private set; }
		public string FormattedFax { get; private set; }

		public UpdateAddressResult(DataTable tessResults)
		{
			var result = (from row in tessResults.AsEnumerable()
						  select new
						  {
							  id = row.Field<int?>("address_no"),
							  constituentId = row.Field<int?>("customer_no"),
							  typeId = row.Field<int?>("address_type"),
							  typeName = row.Field<string>("address_type_desc"),
							  streetAddress = row.Field<string>("street1"),
							  subStreetAddress = row.Field<string>("street2"),
							  city = row.Field<string>("city"),
							  state = row.Field<string>("state"),
							  postalCode = row.Field<string>("postal_code"),
							  postalCodeFormat = row.Field<string>("postal_code_format"),
							  countryId = row.Field<int?>("country"),
							  countryLongName = row.Field<string>("country_long"),
							  countryShortName = row.Field<string>("country_short"),
							  startDate = row.Field<DateTime?>("start_dt"),
							  endDate = row.Field<DateTime?>("end_dt"),
							  months = row.Field<string>("months"),
							  isPrimary = ToBool(row.Field<string>("primary_ind")),
							  isActive = Invert(ToBool(row.Field<string>("inactive"))),
							  mailPurposes = row.Field<string>("mail_purposes"),
							  geoArea = row.Field<int?>("geo_area"),
							  phone1 = row.Field<string>("phone1"),
							  phone2 = row.Field<string>("phone2"),
							  fax = row.Field<string>("fax_phone"),
							  formattedPhone1 = row.Field<string>("phone1_format"),
							  formattedPhone2 = row.Field<string>("phone2_format"),
							  formattedFax = row.Field<string>("phone_fax_format")
						  }).Single();
			Id = result.id;
			ConstituentId = result.constituentId;
			TypeId = result.typeId;
			TypeName = result.typeName;
			StreetAddress = result.streetAddress;
			SubStreetAddress = result.subStreetAddress;
			City = result.city;
			State = result.state;
			PostalCode = result.postalCode;
			PostalCodeFormat = result.postalCodeFormat;
			CountryId = result.countryId;
			CountryLongName = result.countryLongName;
			CountryShortName = result.countryShortName;
			StartDate = result.startDate;
			EndDate = result.endDate;
			Months = result.months;
			IsPrimary = result.isPrimary;
			IsActive = result.isActive;
			MailPurposes = result.mailPurposes;
			GeoArea = result.geoArea;
			FormattedPhone1 = result.formattedPhone1;
			FormattedPhone2 = result.formattedPhone2;
			FormattedFax = result.formattedFax;
		}
	}
}
