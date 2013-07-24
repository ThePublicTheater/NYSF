using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Ambitus
{
	public class Cart : Internals.ApiResultInterpreter
	{
		public string SessionKey { get; private set; }
		public int? OrderId { get; private set; }
		public int? AppealId { get; private set; }
		public int? SourceId { get; private set; }
		public int? ConstituentId { get; private set; }
		public string Solicitor { get; private set; }
		public short? ModeOfSaleId { get; private set; }
		public DateTime? Date { get; private set; }
		public int? BatchId { get; private set; }
		public short? Class { get; private set; }
		public int? AddressId { get; private set; }
		public DateTime? HoldUntilDate { get; private set; }
		public int? TransactionId { get; private set; }
		public bool? HoldAtBoxOffice { get; private set; }
		public string Notes { get; private set; }
		public short? BusinessUnit { get; private set; }
		public int? ShippingMethodId { get; private set; }
		public string ShippingMethodName { get; private set; }
		public decimal? OrderTotal { get; private set; }
		public decimal? OrderValue { get; private set; }
		public string Notes1 { get; private set; }
		public short? DbStatus { get; private set; }
		public decimal? AmountToCharge { get; private set; }
		public DateTime? FirstSeatAddedTime { get; private set; }
		public decimal? AmountPaidToDate { get; private set; }
		public decimal? AmountPaidNow { get; private set; }
		public decimal? BalanceToCharge { get; private set; }
		public decimal? Subtotal { get; private set; }
		public decimal? HandlingCharges { get; private set; }
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
		public ContributionCollection Contributions { get; private set; }
		public LineItemCollection LineItems { get; private set; }
		public AppliedFeeCollection AppliedFees { get; private set; }
		public FeeCategoryCollection FeeCategories { get; private set; }
		public CartPriceTypeCollection PriceTypes { get; private set; }
		public PaymentCollection Payments { get; private set; }
		public PackageLineItemCollection PackageLineItems { get; private set; }

		public Cart(DataSet tessResults)
		{
			DataTableCollection tables = tessResults.Tables;
			if (tables["Contribution"].Rows.Count > 0)
			{
				Contributions = new ContributionCollection(tables["Contribution"]);
			}
			if (tables["LineItem"].Rows.Count > 0)
			{
				LineItems = new LineItemCollection(tables["LineItem"], tables["SubLineItem"]);
			}
			if (tables["SubLineItemFee"].Rows.Count > 0)
			{
				AppliedFees = new AppliedFeeCollection(tables["SubLineItemFee"]);
			}
			if (tables["Fee"].Rows.Count > 0)
			{
				FeeCategories = new FeeCategoryCollection(tables["Fee"]);
			}
			if (tables["PriceType"].Rows.Count > 0)
			{
				PriceTypes = new CartPriceTypeCollection(tables["PriceType"]);
			}
			if (tables["Payment"].Rows.Count > 0)
			{
				Payments = new PaymentCollection(tables["Payment"]);
			}
			if (tables["PackageLineItem"].Rows.Count > 0)
			{
				PackageLineItems =
					new PackageLineItemCollection(tables["PackageLineItem"], tables["PackageSubLineItem"]);
			}
			var result = (from o in tessResults.Tables["Order"].AsEnumerable()
						  select new
						  {
							    SessionKey = o.Field<string>("sessionkey"),
								OrderId = o.Field<int?>("order_no"),
								AppealId = o.Field<int?>("appeal_no"),
								SourceId = o.Field<int?>("source_no"),
								ConstituentId = o.Field<int?>("customer_no"),
								Solicitor = o.Field<string>("solicitor"),
								ModeOfSaleId = o.Field<short?>("MOS"),
								Date = o.Field<DateTime?>("order_dt"),
								BatchId = o.Field<int?>("batch_no"),
								Class = o.Field<short?>("class"),
								AddressId = o.Field<int?>("address_no"),
								HoldUntilDate = o.Field<DateTime?>("hold_until_dt"),
								TransactionId = o.Field<int?>("transaction_no"),
								HoldAtBoxOffice = o.Field<string>("habo_ind"),
								Notes = o.Field<string>("notes"),
								BusinessUnit = o.Field<short?>("bu"),
								ShippingMethodId = o.Field<int?>("shipping_method"),
								ShippingMethodName = o.Field<string>("shipping_method_desc"),
								OrderTotal = o.Field<decimal?>("order_total"),
								OrderValue = o.Field<decimal?>("order_value"),
								Notes1 = o.Field<string>("notes1"),
								DbStatus = o.Field<short?>("db_status"),
								AmountToCharge = o.Field<decimal?>("amt_to_charge"),
								FirstSeatAddedTime = o.Field<DateTime?>("first_seat_added_dt"),
								AmountPaidToDate = o.Field<decimal?>("amt_paid_to_dt"),
								AmountPaidNow = o.Field<decimal?>("amt_paid_now"),
								BalanceToCharge = o.Field<decimal?>("balance_to_charge"),
								Subtotal = o.Field<decimal?>("SubTotal"),
								HandlingCharges = o.Field<decimal?>("HandlingCharges"),
								Custom1 = o.Field<string>("custom_1"),
								Custom2 = o.Field<string>("custom_2"),
								Custom3 = o.Field<string>("custom_3"),
								Custom4 = o.Field<string>("custom_4"),
								Custom5 = o.Field<string>("custom_5"),
								Custom6 = o.Field<string>("custom_6"),
								Custom7 = o.Field<string>("custom_7"),
								Custom8 = o.Field<string>("custom_8"),
								Custom9 = o.Field<string>("custom_9"),
								Custom0 = o.Field<string>("custom_0")
						  }).Single();
			SessionKey = result.SessionKey;
			OrderId = result.OrderId;
			AppealId = result.AppealId;
			SourceId = result.SourceId;
			ConstituentId = result.ConstituentId;
			Solicitor = result.Solicitor;
			ModeOfSaleId = result.ModeOfSaleId;
			Date = result.Date;
			BatchId = result.BatchId;
			Class = result.Class;
			AddressId = result.AddressId;
			HoldUntilDate = result.HoldUntilDate;
			TransactionId = result.TransactionId;
			HoldAtBoxOffice = ToBool(result.HoldAtBoxOffice);
			Notes = result.Notes;
			BusinessUnit = result.BusinessUnit;
			ShippingMethodId = result.ShippingMethodId;
			ShippingMethodName = result.ShippingMethodName;
			OrderTotal = result.OrderTotal;
			OrderValue = result.OrderValue;
			Notes1 = result.Notes1;
			DbStatus = result.DbStatus;
			AmountToCharge = result.AmountToCharge;
			FirstSeatAddedTime = result.FirstSeatAddedTime;
			AmountPaidToDate = result.AmountPaidToDate;
			AmountPaidNow = result.AmountPaidNow;
			BalanceToCharge = result.BalanceToCharge;
			Subtotal = result.Subtotal;
			HandlingCharges = result.HandlingCharges;
			Custom0 = result.Custom0;
			Custom1 = result.Custom1;
			Custom2 = result.Custom2;
			Custom3 = result.Custom3;
			Custom4 = result.Custom4;
			Custom5 = result.Custom5;
			Custom6 = result.Custom6;
			Custom7 = result.Custom7;
			Custom8 = result.Custom8;
			Custom9 = result.Custom9;
		}
	}
}
