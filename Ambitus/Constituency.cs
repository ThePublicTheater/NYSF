using System;

namespace Ambitus
{
	public class Constituency
	{
		public string TypeName { get; private set; }
		public string CreatedByUser { get; private set; }
		public DateTime? CreatedDateTime { get; private set; }
		public byte? N1N2 { get; private set; }
		public int? TypeId { get; private set; }
		public string TypeShortName { get; private set; }

		public Constituency(string _constituency, string _created_by, DateTime? _create_dt,
				byte? _n1n2_ind, int? _id, string _short_desc)
		{
			TypeName = _constituency;
			CreatedByUser = _created_by;
			CreatedDateTime = _create_dt;
			N1N2 = _n1n2_ind;
			TypeId = _id;
			TypeShortName = _short_desc;
		}
	}
}
