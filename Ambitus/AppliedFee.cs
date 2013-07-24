namespace Ambitus
{
	public class AppliedFee
	{
		public int? Id { get; private set; }
		public int? OrderId { get; private set; }
		public int? LineItemId { get; private set; }
		public int? SubLineItemId { get; private set; }
		public int? FeeId { get; private set; }
		public string FeeName { get; private set; }
		public decimal? Amount { get; private set; }
		public bool? HasOverriddenAmount { get; private set; }
		public decimal? OverriddenAmount { get; private set; }
		public bool? IsUserDefined { get; private set; }
		public string Frequency { get; private set; }
		public short? CategoryId { get; private set; }
		public string CategoryName { get; private set; }
		public short? DbStatus { get; private set; }

		public AppliedFee(int? _id, int? _order_no, int? _li_seq_no, int? _sli_no, int? _fee_no,
				string _fee_desc, decimal? _fee_amt, bool? _fee_override_ind,
				decimal? _fee_override_amt, bool? _user_def_ind, string _fee_frequency,
				short? _category, string _category_desc, short? _db_status)
		{
			Id = _id;
			OrderId = _order_no;
			LineItemId = _li_seq_no;
			SubLineItemId = _sli_no;
			FeeId = _fee_no;
			FeeName = _fee_desc;
			Amount = _fee_amt;
			HasOverriddenAmount = _fee_override_ind;
			OverriddenAmount = _fee_override_amt;
			IsUserDefined = _user_def_ind;
			Frequency = _fee_frequency;
			CategoryId = _category;
			CategoryName = _category_desc;
			DbStatus = _db_status;
		}
	}
}
