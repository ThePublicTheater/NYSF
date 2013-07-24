namespace Ambitus
{
	public class ProgramListing
	{
		public int? ConstituentId { get; private set; }
		public int? TypeId { get; private set; }
		public string TypeName { get; private set; }
		public string PrintedName { get; private set; }
		public string SortByName { get; private set; }
		public int? DonationLevelId { get; private set; }
		public string DonationLevelName { get; private set; }

		public ProgramListing(int? _customer_no, int? _program_type, string _program_type_desc,
				string _cust_pname, string _sort_name, int? _donation_level,
				string _donation_level_desc)
		{
			ConstituentId = _customer_no;
			TypeId = _program_type;
			TypeName = _program_type_desc;
			PrintedName = _cust_pname;
			SortByName = _sort_name;
			DonationLevelId = _donation_level;
			DonationLevelName = _donation_level_desc;
		}
	}
}
