namespace Ambitus
{
	public class AccountInfoUpdate
	{
		public DeletableParam<string> Email { get; private set; }
		public string Phone { get; set; }
		public DeletableParam<string> StreetAddress { get; private set; }
		public DeletableParam<string> SubStreetAddress { get; private set; }
		public DeletableParam<string> City { get; private set; }
		public string State { get; set; }
		public DeletableParam<string> PostalCode { get; private set; }
		public int? CountryId { get; set; }
		public string Fax { get; set; }
		public DeletableParam<string> FirstName { get; private set; }
		public DeletableParam<string> MiddleName { get; private set; }
		public DeletableParam<string> LastName { get; private set; }
		public DeletableParam<string> Prefix { get; private set; }
		public DeletableParam<string> Suffix { get; private set; }
		public DeletableParam<string> BusinessTitle { get; private set; }
		public int? EmailRestrictionId { get; set; }
		public int? MailRestrictionId { get; set; }
		public int? PhoneRestrictionId { get; set; }
		public bool? AcceptsHtmlEmail { get; set; }
		public DeletableParam<string> Gender { get; private set; }
		public DeletableParam<string> Gender2 { get; private set; }
		public DeletableParam<string> FirstName2 { get; private set; }
		public DeletableParam<string> MiddleName2 { get; private set; }
		public DeletableParam<string> LastName2 { get; private set; }
		public DeletableParam<string> Prefix2 { get; private set; }
		public DeletableParam<string> Suffix2 { get; private set; }
		public int? OriginalSourceId { get; set; }
		public int? AddressTypeId { get; set; }
		public int? EmailAddressTypeId { get; set; }
		public int? ConstituentTypeId { get; set; }
		public int? NameStatusId { get; set; }
		public int? Name2StatusId { get; set; }
		public DeletableParam<string> SalutationLine1 { get; private set; }
		public DeletableParam<string> SalutationLine2 { get; private set; }
		public DeletableParam<string> LetterSalutation { get; private set; }
		public bool? AllowChangesToSalutationsAndBusinessTitle { get; set; }

		public AccountInfoUpdate()
		{
			Email = new DeletableParam<string>(Tess.EmptyStringLiteral);
			StreetAddress = new DeletableParam<string>(Tess.EmptyStringLiteral);
			SubStreetAddress = new DeletableParam<string>(Tess.EmptyStringLiteral);
			City = new DeletableParam<string>(Tess.EmptyStringLiteral);
			PostalCode = new DeletableParam<string>(" ");
			FirstName = new DeletableParam<string>(Tess.EmptyStringLiteral);
			MiddleName = new DeletableParam<string>(Tess.EmptyStringLiteral);
			LastName = new DeletableParam<string>(Tess.EmptyStringLiteral);
			Prefix = new DeletableParam<string>(Tess.EmptyStringLiteral);
			Suffix = new DeletableParam<string>(Tess.EmptyStringLiteral);
			Gender = new DeletableParam<string>(Tess.EmptyStringLiteral);
			Gender2 = new DeletableParam<string>(Tess.EmptyStringLiteral);
			FirstName2 = new DeletableParam<string>(Tess.EmptyStringLiteral);
			MiddleName2 = new DeletableParam<string>(Tess.EmptyStringLiteral);
			LastName2 = new DeletableParam<string>(Tess.EmptyStringLiteral);
			Prefix2 = new DeletableParam<string>(Tess.EmptyStringLiteral);
			Suffix2 = new DeletableParam<string>(Tess.EmptyStringLiteral);
			BusinessTitle = new DeletableParam<string>(Tess.EmptyStringLiteral);
			SalutationLine1 = new DeletableParam<string>(Tess.EmptyStringLiteral);
			SalutationLine2 = new DeletableParam<string>(Tess.EmptyStringLiteral);
			LetterSalutation = new DeletableParam<string>(Tess.EmptyStringLiteral);
		}
	}
}
