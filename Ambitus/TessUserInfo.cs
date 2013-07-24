using System.Data;
using System.Linq;

namespace Ambitus
{
	public class TessUserInfo : Internals.ApiResultInterpreter
	{
		public string Username { get; private set; }
		public string UgId { get; private set; }
		public int? UserControlGroupId { get; private set; }
		public int? ControlGroupId { get; private set; }
		public string ControlGroupName { get; private set; }
		public bool? Edit { get; private set; }

		public TessUserInfo(DataSet tessResults)
		{
			var userInfo = (from rows in tessResults.Tables["UserInfo"].AsEnumerable()
							select new
							{
								Username = rows.Field<string>("userid"),
								UgId = rows.Field<string>("ug_id"),
								UserControlGroupId = rows.Field<int?>("control_group")
							}).Single();
			var controlGroup = (from rows in tessResults.Tables["ControlGroup"].AsEnumerable()
								select new
								{
									ControlGroupId = rows.Field<int?>("id"),
									ControlGroupName = rows.Field<string>("description"),
									Edit = ToBool(rows.Field<string>("edit_ind"))
								}).Single();
			Username = userInfo.Username;
			UgId = userInfo.UgId;
			UserControlGroupId = userInfo.UserControlGroupId;
			ControlGroupId = controlGroup.ControlGroupId;
			ControlGroupName = controlGroup.ControlGroupName;
			Edit = controlGroup.Edit;
		}
	}
}
