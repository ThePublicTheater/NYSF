namespace Ambitus
{
	public class Contribution
	{
		public int? Id { get; private set; }
		public int? OrderId { get; private set; }
		public decimal? Amount { get; private set; }
		public int? FundId { get; private set; }
		public int? OnAccountMethodId { get; private set; }
		public string FundName { get; private set; }
		public string OnAccountMethodName { get; private set; }
		public ContributionAction? Action { get; private set; }
		public bool? Decline { get; private set; }
		public short? DbStatus { get; private set; }
		public string Notes { get; private set; }
		public string Custom0 { get; private set; }
		public string Custom1 { get; private set; }
		public string Custom2 { get; private set; }
		public string Custom3 { get; private set; }
		public string Custom4 { get; private set; }
		public string Custom5 { get; private set; }
		public string Custom6 { get; private set; }
		public string Custom7 { get; private set; }
		public string Custom8 { get; private set; }
		public string Custom9 { get; private set; }

		public Contribution(int? id, int? orderId, decimal? amount, int? fundId,
				int? onAccountMethodId, string fundName, string onAccountMethodName,
				ContributionAction? action, bool? decline, short? dbStatus, string notes,
				string custom0, string custom1, string custom2, string custom3, string custom4,
				string custom5, string custom6, string custom7, string custom8, string custom9)
		{
			Id = id;
			OrderId = orderId;
			Amount = amount;
			FundId = fundId;
			OnAccountMethodId = onAccountMethodId;
			FundName = fundName;
			OnAccountMethodName = onAccountMethodName;
			ContributionAction? Action = action;
			Decline = decline;
			DbStatus = dbStatus;
			Notes = notes;
			Custom0 = custom0;
			Custom1 = custom1;
			Custom2 = custom2;
			Custom3 = custom3;
			Custom4 = custom4;
			Custom5 = custom5;
			Custom6 = custom6;
			Custom7 = custom7;
			Custom8 = custom8;
			Custom9 = custom9;
		}
	}
}
