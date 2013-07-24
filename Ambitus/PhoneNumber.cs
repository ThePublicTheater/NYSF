namespace Ambitus
{
	public class PhoneNumber
	{
		public int? Id { get; private set; }
		public int? AddressId { get; private set; }
		public string Number { get; private set; }
		public int? TypeId { get; private set; }
		public string TypeName { get; private set; }
		public string DayInd { get; private set; }
		public bool? AcceptsTelemarketing { get; private set; }
		public bool? IsPrimary { get; private set; }

		public PhoneNumber(int? _phone_no, int? _address_no, string _phone, int? _type,
				string _type_desc, string _day_ind, bool? _tele_ind, bool? _primary_adr)
		{
			Id = _phone_no;
			AddressId = _address_no;
			Number = _phone;
			TypeId = _type;
			TypeName = _type_desc;
			DayInd = _day_ind;
			AcceptsTelemarketing = _tele_ind;
			IsPrimary = _primary_adr;
		}
	}
}
