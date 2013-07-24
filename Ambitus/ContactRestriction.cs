using System;

namespace Ambitus
{
	public class ContactRestriction
	{
		public int? Id { get; private set; }
		public string Description { get; private set; }
		public ContactRestrictionType? Type { get; private set; }

		public ContactRestriction(int? id, string desc, ContactRestrictionType? type)
		{
			Id = id;
			Description = desc;
			Type = type;
		}

		public override string ToString()
		{
			return (Description ?? String.Empty) + ", " + Type.ToString();
		}
	}
}
