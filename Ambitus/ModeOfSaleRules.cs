using System;
using System.Data;
using System.Linq;

namespace Ambitus
{
	public class ModeOfSaleRules : Internals.ApiResultInterpreter
	{
		// TODO: figure out better names, expected values for these fields:
		public string Name { get; private set; }
		public bool? GeneralPublic { get; private set; }
		public string Category { get; private set; }
		public bool? SubLineItemAutoDelete { get; private set; }
		public bool? MustBePaid { get; private set; }
		public bool? MustBeSeated { get; private set; }
		public bool? MustBeTicketed { get; private set; }
		public HoldUntilMethod? HoldUntilMethod { get; private set; }
		public DateTime? HoldUntilDate { get; private set; }
		public int? HoldUntilDays { get; private set; }
		public int? DefaultHeaderFormat { get; private set; }
		public bool? ClearSourceId { get; private set; }
		public int? RequireHaboWithinDays { get; private set; }
		public bool? RequireHaboForForeignAddresses { get; private set; }
		public string EditDate { get; private set; }
		public string StartPackageOrPerf { get; private set; }
		public bool? ClearSourceOnReload { get; private set; }
		public bool? EditSourceOnReload { get; private set; }
		public bool? CategoryRequired { get; private set; }
		public bool? SubsSummaryRequired { get; private set; }
		public int? DefaultAckFormat { get; private set; }
		public int? DefaultShippingMethod { get; private set; }
		public bool? AllowUnseatedPaid { get; private set; }

		public ModeOfSaleRules(DataTable tessResults)
		{
			var result = (from row in tessResults.AsEnumerable()
						 select new
						 {
							 Description = row.Field<string>("description"),
							 GeneralPublic = row.Field<string>("general_public_ind"),
							 Category = row.Field<string>("category"),
							 SubLineItemAutoDelete = row.Field<string>("sli_auto_delete_ind"),
							 MustBePaid = row.Field<string>("must_be_paid_ind"),
							 MustBeSeated = row.Field<string>("must_be_seated_ind"),
							 MustBeTicketed = row.Field<string>("must_be_ticketed_ind"),
							 HoldUntilMethod = row.Field<string>("hold_until_method"),
							 HoldUntilDate = row.Field<DateTime?>("hold_until_date"),
							 HoldUntilDays = row.Field<int?>("hold_until_days"),
							 DefaultHeaderFormat = row.Field<int?>("default_header_format"),
							 ClearSourceNo = row.Field<string>("clear_source_no_ind"),
							 HaboDays = row.Field<int?>("habo_days"),
							 HaboForeign = row.Field<string>("habo_foreign"),
							 EditDate = row.Field<string>("edit_date"),
							 StartPackageOrPerf = row.Field<string>("start_pkg_or_perf"),
							 ClearSourceOnReload = row.Field<string>("clear_source_on_reload"),
							 EditSourceOnReload = row.Field<string>("edit_source_on_reload"),
							 CategoryRequired = row.Field<string>("category_required"),
							 SubsSummaryRequired = row.Field<string>("subs_summary_required"),
							 DefaultAckFormat = row.Field<int?>("default_ack_format"),
							 DefaultShipMethod = row.Field<int?>("default_ship_method"),
							 AllowUnseatedPaidInd = row.Field<string>("allow_unseated_paid_ind")
						 }).Single();
			Name = result.Description;
			GeneralPublic = ToBool(result.GeneralPublic);
			Category = result.Category;
			SubLineItemAutoDelete = ToBool(result.SubLineItemAutoDelete);
			MustBePaid = ToBool(result.MustBePaid);
			MustBeSeated = ToBool(result.MustBeSeated);
			MustBeTicketed = ToBool(result.MustBeTicketed);
			HoldUntilMethod = ToHoldUntilMethod(result.HoldUntilMethod);
			HoldUntilDate = result.HoldUntilDate;
			HoldUntilDays = result.HoldUntilDays;
			DefaultHeaderFormat = result.DefaultHeaderFormat;
			ClearSourceId = ToBool(result.ClearSourceNo);
			RequireHaboWithinDays = result.HaboDays;
			RequireHaboForForeignAddresses = ToBool(result.HaboForeign);
			EditDate = result.EditDate;
			StartPackageOrPerf = result.StartPackageOrPerf;
			ClearSourceOnReload = ToBool(result.ClearSourceOnReload);
			EditSourceOnReload = ToBool(result.EditSourceOnReload);
			CategoryRequired = ToBool(result.CategoryRequired);
			SubsSummaryRequired = ToBool(result.SubsSummaryRequired);
			DefaultAckFormat = result.DefaultAckFormat;
			DefaultShippingMethod = result.DefaultShipMethod;
			AllowUnseatedPaid = ToBool(result.AllowUnseatedPaidInd);
		}
	}
}
