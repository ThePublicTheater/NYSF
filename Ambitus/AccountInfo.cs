using System.Data;
using System.Linq;

namespace Ambitus
{
	public class AccountInfo : Internals.ApiResultInterpreter
	{
		public int? ConstituentId { get; private set; }
		public string Prefix { get; private set; }
		public string FirstName { get; private set; }
		public string MiddleName { get; private set; }
		public string LastName { get; private set; }
		public string Suffix { get; private set; }
		public char? Gender { get; private set; }
		public string SalutationLine1 { get; private set; }
		public string SalutationLine2 { get; private set; }
		public string LetterSalutation { get; private set; }
		public string BusinessTitle { get; private set; }
		public string NameStatus { get; private set; }
		public string Prefix2 { get; private set; }
		public string FirstName2 { get; private set; }
		public string MiddleName2 { get; private set; }
		public string LastName2 { get; private set; }
		public string Suffix2 { get; private set; }
		public char? Gender2 { get; private set; }
		public string Name2Status { get; private set; }
		public string StreetAddress { get; private set; }
		public string SubStreetAddress { get; private set; }
		public string City { get; private set; }
		public string State { get; private set; }
		public string PostalCode { get; private set; }
		public int? CountryId { get; private set; }
		public string CountryName { get; private set; }
		public string Phone { get; private set; }
		public string Phone2 { get; private set; }
		public string Fax { get; private set; }
		public string Email { get; private set; }
		public bool? AcceptsHtmlEmail { get; private set; }
		public string OriginalSource { get; private set; }
		public bool? UseAvs { get; private set; }
		public int? MailRestrictionId { get; private set; }
		public string MailRestriction { get; private set; }
		public int? PhoneRestrictionId { get; private set; }
		public string PhoneRestriction { get; private set; }
		public int? EmailRestrictionId { get; private set; }
		public string EmailRestriction { get; private set; }
		public bool? EmailActive { get; private set; }

		public AccountInfo(DataTable tessResults)
		{
			var results = (from rows in tessResults.AsEnumerable()
						   select new
						   {
							   ConstituentId = rows.Field<int?>("customer_no"),
							   Prefix = rows.Field<string>("prefix"),
							   FirstName = rows.Field<string>("fname"),
							   MiddleName = rows.Field<string>("mname"),
							   LastName = rows.Field<string>("lname"),
							   Suffix = rows.Field<string>("suffix"),
							   Gender = rows.Field<string>("gender_1"),
							   SalutationLine1 = rows.Field<string>("esal1_desc"),
							   SalutationLine2 = rows.Field<string>("esal2_desc"),
							   LetterSalutation = rows.Field<string>("lsal_desc"),
							   BusinessTitle = rows.Field<string>("business_title"),
							   NameStatus = rows.Field<string>("name_status"),
							   Prefix2 = rows.Field<string>("prefix2"),
							   FirstName2 = rows.Field<string>("fname2"),
							   MiddleName2 = rows.Field<string>("mname2"),
							   LastName2 = rows.Field<string>("lname2"),
							   Suffix2 = rows.Field<string>("suffix2"),
							   Gender2 = rows.Field<string>("gender_2"),
							   Name2Status = rows.Field<string>("name2_status"),
							   StreetAddress = rows.Field<string>("street1"),
							   SubStreetAddress = rows.Field<string>("street2"),
							   City = rows.Field<string>("city"),
							   State = rows.Field<string>("state"),
							   PostalCode = rows.Field<string>("postal_code"),
							   CountryId = rows.Field<int?>("country"),
							   CountryName = rows.Field<string>("country_name"),
							   Phone = rows.Field<string>("phone"),
							   Phone2 = rows.Field<string>("phone2"),
							   Fax = rows.Field<string>("fax_phone"),
							   Email = rows.Field<string>("email"),
							   AcceptsHtmlEmail = rows.Field<string>("html_ind"),
							   OriginalSource = rows.Field<string>("original_source"),
							   UseAvs = rows.Field<string>("use_avs"),
							   MailRestrictionId = rows.Field<int?>("mail_ind_id"),
							   MailRestriction = rows.Field<string>("mail_ind"),
							   PhoneRestrictionId = rows.Field<int?>("phone_ind_id"),
							   PhoneRestriction = rows.Field<string>("phone_ind"),
							   EmailRestrictionId = rows.Field<int?>("emarket_ind_id"),
							   EmailRestriction = rows.Field<string>("emarket_ind"),
							   EmailInactive = rows.Field<string>("email_inactive")
						   }).Single();
			ConstituentId = results.ConstituentId;
			Prefix = results.Prefix;
			FirstName = results.FirstName;
			MiddleName = results.MiddleName;
			LastName = results.LastName;
			Suffix = results.Suffix;
			Gender = ToChar(results.Gender);
			SalutationLine1 = results.SalutationLine1;
			BusinessTitle = results.BusinessTitle;
			NameStatus = results.NameStatus;
			Prefix2 = results.Prefix2;
			FirstName2 = results.FirstName2;
			MiddleName2 = results.MiddleName2;
			LastName2 = results.LastName2;
			Suffix2 = results.Suffix2;
			Gender2 = ToChar(results.Gender2);
			SalutationLine2 = results.SalutationLine2;
			Name2Status = results.Name2Status;
			StreetAddress = results.StreetAddress;
			SubStreetAddress = results.SubStreetAddress;
			City = results.City;
			State = results.State;
			PostalCode = results.PostalCode;
			CountryId = results.CountryId;
			CountryName = results.CountryName;
			Phone = results.Phone;
			Phone2 = results.Phone2;
			Fax = results.Fax;
			LetterSalutation = results.LetterSalutation;
			AcceptsHtmlEmail = ToBool(results.AcceptsHtmlEmail);
			OriginalSource = results.OriginalSource;
			UseAvs = ToBool(results.UseAvs);
			MailRestrictionId = results.MailRestrictionId;
			MailRestriction = results.MailRestriction;
			PhoneRestrictionId = results.PhoneRestrictionId;
			PhoneRestriction = results.PhoneRestriction;
			EmailRestrictionId = results.EmailRestrictionId;
			EmailRestriction = results.EmailRestriction;
			EmailActive = Invert(ToBool(results.EmailInactive));
			Email = results.Email;
		}
	}
}
