using System;

namespace Ambitus
{
	public class ContributionRecord
	{
		public int? ConstituentId { get; private set; }
		public DateTime? CreatedDateTime { get; private set; }
		public decimal? ReceivedAmount { get; private set; }
		public decimal? Amount { get; private set; }
		public string TypeName { get; private set; }
		public string CampaignName { get; private set; }
		public string AppealName { get; private set; }
		public string MediaTypeName { get; private set; }
		public string SourceName { get; private set; }
		public string FundName { get; private set; }
		public int? Id { get; private set; }
		public string BillingTypeName { get; private set; }
		public string CreditInd { get; private set; }
		public string N1N2 { get; private set; }
		public string CrediteeName { get; private set; }
		public decimal? CreditedAmount { get; private set; }
		public string SolicitorUser { get; private set; }
		public string Designation { get; private set; }

		public ContributionRecord(int? _customer_no, DateTime? _cont_date, decimal? _recd_amt,
				decimal? _cont_amt, string _type, string _campaign, string _appeal,
				string _media_type, string _source_name, string _fund_no, int? _ref_no,
				string _billing_type, string _credit_ind, string _n1n2_ind, string _creditee_name,
				decimal? _credited_amt, string _solicitor, string _cont_designation)
		{
			ConstituentId = _customer_no;
			CreatedDateTime = _cont_date;
			ReceivedAmount = _recd_amt;
			Amount = _cont_amt;
			TypeName = _type;
			CampaignName = _campaign;
			AppealName = _appeal;
			MediaTypeName = _media_type;
			SourceName = _source_name;
			FundName = _fund_no;
			Id = _ref_no;
			BillingTypeName = _billing_type;
			CreditInd = _credit_ind;
			N1N2 = _n1n2_ind;
			CrediteeName = _creditee_name;
			CreditedAmount = _credited_amt;
			SolicitorUser = _solicitor;
			Designation = _cont_designation;
		}
	}
}
