using System;
using System.Data;
using System.Linq;

namespace Ambitus
{
	public class ConstituentHeader : Internals.ApiResultInterpreter
	{
		public int? ConstituentId { get; private set; }
		public string FullName1 { get; private set; }
		public string FullName2 { get; private set; }
		public string ConstituenciesList { get; private set; }
		public string MembershipLevel { get; private set; }
		public string CurrentStatus { get; private set; }
		public DateTime? MembershipExpirationDate { get; private set; }
		public DateTime? CreatedDateTime { get; private set; }
		public decimal? Donation { get; private set; }
		public string RetrievedByUser { get; private set; }
		public int? RetrievedDuringBatchId { get; private set; }
		public decimal? OnAccount { get; private set; }
		public bool? Active { get; private set; }
		public int? RetrievedDuringBatchTypeId { get; private set; }

		public ConstituentHeader(DataTable tessResults)
		{
			var header = (from row in tessResults.AsEnumerable()
						  select new
						  {
							  customer_no = row.Field<int?>("customer_no"),
							  full_name1 = row.Field<string>("full_name1"),
							  full_name2 = row.Field<string>("full_name2"),
							  all_const = row.Field<string>("all_const"),
							  memb_level = row.Field<string>("memb_level"),
							  current_status = row.Field<string>("current_status"),
							  memb_expirtion = row.Field<DateTime?>("memb_expirtion"),
							  create_dt = row.Field<DateTime?>("create_dt"),
							  donation = row.Field<decimal?>("donation"),
							  userid = row.Field<string>("userid"),
							  batch_no = row.Field<int?>("batch_no"),
							  on_account = row.Field<decimal?>("on_account"),
							  inactive = Invert(ToBool(row.Field<string>("inactive"))),
							  batch_type = row.Field<int?>("batch_type")
						  }).Single();
			ConstituentId = header.customer_no;
			FullName1 = header.full_name1;
			FullName2 = header.full_name2;
			ConstituenciesList = header.all_const;
			MembershipLevel = header.memb_level;
			CurrentStatus = header.current_status;
			MembershipExpirationDate = header.memb_expirtion;
			CreatedDateTime = header.create_dt;
			Donation = header.donation;
			RetrievedByUser = header.userid;
			RetrievedDuringBatchId = header.batch_no;
			OnAccount = header.on_account;
			Active = header.inactive;
			RetrievedDuringBatchTypeId = header.batch_type;

		}
	}
}
