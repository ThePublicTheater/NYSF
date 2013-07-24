using System;

namespace Ambitus
{
	public class PaymentMethod
	{
		public int? Id { get; private set; }
		public string Name { get; private set; }
		public bool? Auth { get; private set; }
		public short? PaymentTypeId { get; private set; }
		public string PaymentTypeName { get; private set; }
		public string AccountType { get; private set; }
		public string CardPrefix { get; private set; }
		public bool? AddressVerification { get; private set; }
		public string MerchantId { get; private set; }
		public bool? UseCv2 { get; private set; }
		public string CardLength { get; private set; }
		public bool? Mod10 { get; private set; }

		public PaymentMethod(int? id, string name, bool? auth, short? paymentTypeId,
				string paymentTypeName, string accountType, string cardPrefix,
				bool? addressVerification, string merchantId, bool? useCv2, string cardLength,
				bool? mod10)
		{
			Id = id;
			Name = name;
			Auth = auth;
			PaymentTypeId = paymentTypeId;
			PaymentTypeName = paymentTypeName;
			AccountType = accountType;
			CardPrefix = cardPrefix;
			AddressVerification = addressVerification;
			MerchantId = merchantId;
			UseCv2 = useCv2;
			CardLength = cardLength;
			Mod10 = mod10;
		}

		public override string ToString()
		{
			return Name ?? String.Empty;
		}
	}
}
