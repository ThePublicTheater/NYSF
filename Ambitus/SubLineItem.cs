namespace Ambitus
{
	public class SubLineItem
	{
		public int? sli_no { get; private set; }
		public decimal? due_amt { get; private set; }
		public decimal? paid_amt { get; private set; }
		public short? price_type { get; private set; }
		public int? seat_no { get; private set; }
		public short? comp_code { get; private set; }
		public short? sli_status { get; private set; }
		public int? perf_no { get; private set; }
		public int? pkg_no { get; private set; }
		public int? zone_no { get; private set; }
		public int? ret_parent_sli_no { get; private set; }
		public int? order_no { get; private set; }
		public string seat_row { get; private set; }
		public string seat_num { get; private set; }
		public string zone_desc { get; private set; }
		public string section_desc { get; private set; }
		public string section_short_desc { get; private set; }
		public string section_print_desc { get; private set; }
		public string section_add_text { get; private set; }
		public string section_add_text2 { get; private set; }
		public short? db_status { get; private set; }
		public int? prod_season_no { get; private set; }
		public int? facility_no { get; private set; }
		public short? seat_type { get; private set; }
		public string seat_type_desc { get; private set; }

		public SubLineItem(int? _sli_no, decimal? _due_amt, decimal? _paid_amt,
				short? _price_type, int? _seat_no, short? _comp_code, short? _sli_status,
				int? _perf_no, int? _pkg_no, int? _zone_no, int? _ret_parent_sli_no, int? _order_no,
				string _seat_row, string _seat_num, string _zone_desc, string _section_desc,
				string _section_short_desc, string _section_print_desc, string _section_add_text,
				string _section_add_text2, short? _db_status, int? _prod_season_no,
				int? _facility_no, short? _seat_type, string _seat_type_desc)
		{
			sli_no = _sli_no;
			due_amt = _due_amt;
			paid_amt = _paid_amt;
			price_type = _price_type;
			seat_no = _seat_no;
			comp_code = _comp_code;
			sli_status = _sli_status;
			perf_no = _perf_no;
			pkg_no = _pkg_no;
			zone_no = _zone_no;
			ret_parent_sli_no = _ret_parent_sli_no;
			order_no = _order_no;
			seat_row = _seat_row;
			seat_num = _seat_num;
			zone_desc = _zone_desc;
			section_desc = _section_desc;
			section_short_desc = _section_short_desc;
			section_print_desc = _section_print_desc;
			section_add_text = _section_add_text;
			section_add_text2 = _section_add_text2;
			db_status = _db_status;
			prod_season_no = _prod_season_no;
			facility_no = _facility_no;
			seat_type = _seat_type;
			seat_type_desc = _seat_type_desc;
		}
	}
}
