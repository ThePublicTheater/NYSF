using System;

namespace Ambitus
{
	public class Interest
	{
		public int? ConstituentId { get; private set; }
		public short? KeywordId { get; private set; }
		public string KeywordName { get; private set; }
		public bool? Selected { get; private set; }
		public int? Weight { get; private set; }
		public string CategoryName { get; private set; }
		public DateTime? CreatedDateTime { get; private set; }
		public string CreatedByUser { get; private set; }
		public string CreatedLocation { get; private set; }
		public string LastUpdatedByUser { get; private set; }
		public DateTime? LastUpdatedDateTime { get; private set; }

		public Interest(int? _customer_no, short? _id, string _description, bool? _selected,
				int? _weight, string _category, DateTime? _create_dt, string _created_by,
				string _create_loc, string _last_updated_by, DateTime? _last_update_dt)
		{
			ConstituentId = _customer_no;
			KeywordId = _id;
			KeywordName = _description;
			Selected = _selected;
			Weight = _weight;
			CategoryName = _category;
			CreatedDateTime = _create_dt;
			CreatedByUser = _created_by;
			CreatedLocation = _create_loc;
			LastUpdatedByUser = _last_updated_by;
			LastUpdatedDateTime = _last_update_dt;
		}
	}
}
