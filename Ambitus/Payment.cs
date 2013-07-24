namespace Ambitus
{
	public class Payment
	{
		public int? Id { get; private set; }
		public decimal? Amount { get; private set; }
		public string GiftCertificateId { get; private set; }
		public string Description { get; private set; }

		public Payment(int? id, decimal? amount, string giftCertificateId, string description)
		{
			Id = id;
			Amount = amount;
			GiftCertificateId = giftCertificateId;
			Description = description;
		}
	}
}
