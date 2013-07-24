using System;
using System.Data;

namespace Ambitus
{
	public class PackageLineItem
	{
		public int? Id { get; private set; }
		public int? Number { get; private set; }
		public int? OrderId { get; private set; }
		public int? PackageId { get; private set; }
		public int? SuperPackageId { get; private set; }
		public int? PerfId { get; private set; }
		public short? Priority { get; private set; }
		public short? ZoneId { get; private set; }
		public string AltUpgradeInd { get; private set; }
		public int? PackageLineItemId { get; private set; }
		public bool? IsPrimary { get; private set; }
		public string PerfCode { get; private set; }
		public DateTime? PerfDateTime { get; private set; }
		public string PerfName { get; private set; }
		public string PackageCode { get; private set; }
		public string PackageName { get; private set; }
		public string FacilityName { get; private set; }
		public short? DbStatus { get; private set; }
		public string Notes { get; private set; }
		public int? ZoneMapId { get; private set; }
		public string Text1 { get; private set; }
		public string Text2 { get; private set; }
		public string Text3 { get; private set; }
		public string Text4 { get; private set; }
		public int? PerfGroupId { get; private set; }
		public short? ProdTypeId { get; private set; }
		public string ProdTypeName { get; private set; }
		public bool? HasFixedSeats { get; private set; }
		public bool? IsSuperPackage { get; private set; }
		public bool? IsFlexPackage { get; private set; }
		public int? Category { get; private set; }
		public int? AssociatedCustomerId { get; private set; }
		public short? SeasonId { get; private set; }
		public string SeasonName { get; private set; }
		public bool? IsGeneralAdmission { get; private set; }
		public int? FacilityId { get; private set; }
		public SubLineItemCollection SubLineItems { get; private set; }

		public PackageLineItem(int? _li_seq_no, int? _li_no, int? _order_no, int? _pkg_no,
				int? _super_pkg_no, int? _perf_no, short? _priority, short? _zone_no,
				string _alt_upgrd_ind, int? _pkg_li_no, bool? _primary_ind, string _perf_code,
				DateTime? _perf_dt, string _perf_desc, string _pkg_code, string _pkg_desc,
				string _facility_desc, short? _db_status, string _notes, int? _zmap_no,
				string _text1, string _text2, string _text3, string _text4, int? _perf_group_no,
				short? _prod_type, string _prod_type_desc, bool? _fixed_seat_ind,
				bool? _super_pkg_ind, bool? _flex_ind, int? _category, int? _assoc_customer_no,
				short? _season_no, string _season_desc, bool? _ga_ind, int? _facil_no,
				DataTable sliTable)
		{
			Id = _li_seq_no;
			Number = _li_no;
			OrderId = _order_no;
			PackageId = _pkg_no;
			SuperPackageId = _super_pkg_no;
			PerfId = _perf_no;
			Priority = _priority;
			ZoneId = _zone_no;
			AltUpgradeInd = _alt_upgrd_ind;
			PackageLineItemId = _pkg_li_no;
			IsPrimary = _primary_ind;
			PerfCode = _perf_code;
			PerfDateTime = _perf_dt;
			PerfName = _perf_desc;
			PackageCode = _pkg_code;
			PackageName = _pkg_desc;
			FacilityName = _facility_desc;
			DbStatus = _db_status;
			Notes = _notes;
			ZoneMapId = _zmap_no;
			Text1 = _text1;
			Text2 = _text2;
			Text3 = _text3;
			Text4 = _text4;
			PerfGroupId = _perf_group_no;
			ProdTypeId = _prod_type;
			ProdTypeName = _prod_type_desc;
			HasFixedSeats = _fixed_seat_ind;
			IsSuperPackage = _super_pkg_ind;
			IsFlexPackage = _flex_ind;
			Category = _category;
			AssociatedCustomerId = _assoc_customer_no;
			SeasonId = _season_no;
			SeasonName = _season_desc;
			IsGeneralAdmission = _ga_ind;
			FacilityId = _facil_no;
			if (sliTable.Rows.Count > 0)
			{
				SubLineItems = new SubLineItemCollection(sliTable, Id);
			}
		}
	}
}
