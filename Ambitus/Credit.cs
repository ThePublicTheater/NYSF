using System;

namespace Ambitus
{
	public class Credit
	{
		public string TypeName { get; private set; }
		public int? RoleId { get; private set; }
		public string RoleName { get; private set; }
		public string PersonName { get; private set; }
		public int? ArtistId { get; private set; }
		public int? Rank { get; private set; }

		public Credit(string typeName, int? roleId, string roleName, string personName,
				int? artistId, int? rank)
		{
			TypeName = typeName;
			RoleId = roleId;
			RoleName = roleName;
			PersonName = personName;
			ArtistId = artistId;
			Rank = rank;
		}

		public override string ToString()
		{
			return (PersonName ?? String.Empty) + ", " + (RoleName ?? String.Empty);
		}
	}
}
