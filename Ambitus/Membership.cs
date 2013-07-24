using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ambitus
{
	public class Membership
	{
		public int? OrgId { get; private set; }
		public int? StatusId { get; private set; }
		public string LevelId { get; private set; }
		public DateTime? InitiationDate { get; private set; }
		public DateTime? ExpirationDate { get; private set; }
		public string StatusName { get; private set; }
		public string LevelName { get; private set; }
		public int? CategoryId { get; private set; }
		public string CategoryName { get; private set; }
		public string OrgName { get; private set; }
		public DateTime? InceptionDate { get; private set; }
		public int? BenProvider { get; private set; }
		public DateTime? LapseDate { get; private set; }
		public DateTime? RenewalDate { get; private set; }
		public bool? HasDeclined { get; private set; }
		public int? Id { get; private set; }

		public Membership(int? _memb_org_no, int? _current_status, string _memb_level,
				DateTime? _init_dt, DateTime? _expr_dt, string _current_status_desc,
				string _memb_level_desc, int? _memb_level_category, string _category_desc,
				string _memb_org_desc, DateTime? _inception_dt, int? _ben_provider,
				DateTime? _lapse_dt, DateTime? _renewal_dt, bool? _declined_ind,
				int? _cust_memb_no)
		{
			OrgId = _memb_org_no;
			StatusId = _current_status;
			LevelId = _memb_level;
			InitiationDate = _init_dt;
			ExpirationDate = _expr_dt;
			StatusName = _current_status_desc;
			LevelName = _memb_level_desc;
			CategoryId = _memb_level_category;
			CategoryName = _category_desc;
			OrgName = _memb_org_desc;
			InceptionDate = _inception_dt;
			BenProvider = _ben_provider;
			LapseDate = _lapse_dt;
			RenewalDate = _renewal_dt;
			HasDeclined = _declined_ind;
			Id = _cust_memb_no;
		}
	}
}
