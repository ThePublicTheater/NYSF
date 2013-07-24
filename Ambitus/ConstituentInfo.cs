using System.Data;

namespace Ambitus
{
	public class ConstituentInfo
	{
		public AddressCollection Addresses { get; private set; }
		public ConstituentAttributeCollection Attributes { get; private set; }
		public EmailAddressCollection EmailAddresses { get; private set; }
		public AssociationCollection Associations { get; private set; }
		public ConstituencyCollection Constituencies { get; private set; }
		public ConstituentHeader Header { get; private set; }
		public ContributionRecordCollection Contributions { get; private set; }
		public InterestCollection Interests { get; private set; }
		public MembershipCollection Memberships { get; private set; }
		public PhoneNumberCollection PhoneNumbers { get; private set; }
		public ProgramListingCollection ProgramListings { get; private set; }
		public RankCollection Rankings { get; private set; }

		public ConstituentInfo(DataSet tessResults)
		{
			DataTableCollection tables = tessResults.Tables;
			if (tables.Contains("Addresses") && tables["Addresses"].Rows.Count > 0)
			{
				Addresses = new AddressCollection(tables["Addresses"]);
			}
			if (tables.Contains("ConstituentAttribute")
					&& tables["ConstituentAttribute"].Rows.Count > 0)
			{
				Attributes = new ConstituentAttributeCollection(tables["ConstituentAttribute"]);
			}
			if (tables.Contains("EmailAddresses") && tables["EmailAddresses"].Rows.Count > 0)
			{
				EmailAddresses = new EmailAddressCollection(tables["EmailAddresses"]);
			}
			if (tables.Contains("Associations") && tables["Associations"].Rows.Count > 0)
			{
				Associations = new AssociationCollection(tables["Associations"]);
			}
			if (tables.Contains("Constituency") && tables["Constituency"].Rows.Count > 0)
			{
				Constituencies = new ConstituencyCollection(tables["Constituency"]);
			}
			if (tables.Contains("ConstituentHeader") && tables["ConstituentHeader"].Rows.Count > 0)
			{
				Header = new ConstituentHeader(tables["ConstituentHeader"]);
			}
			if (tables.Contains("Contribution") && tables["Contribution"].Rows.Count > 0)
			{
				Contributions = new ContributionRecordCollection(tables["Contribution"]);
			}
			if (tables.Contains("Interests") && tables["Interests"].Rows.Count > 0)
			{
				Interests = new InterestCollection(tables["Interests"]);
			}
			if (tables.Contains("Memberships") && tables["Memberships"].Rows.Count > 0)
			{
				Memberships = new MembershipCollection(tables["Memberships"]);
			}
			if (tables.Contains("Phones") && tables["Phones"].Rows.Count > 0)
			{
				PhoneNumbers = new PhoneNumberCollection(tables["Phones"]);
			}
			if (tables.Contains("ProgramListings") && tables["ProgramListings"].Rows.Count > 0)
			{
				ProgramListings = new ProgramListingCollection(tables["ProgramListings"]);
			}
			if (tables.Contains("Rankings") && tables["Rankings"].Rows.Count > 0)
			{
				Rankings = new RankCollection(tables["Rankings"]);
			}
		}
	}
}
