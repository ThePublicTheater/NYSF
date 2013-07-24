using System;

namespace Ambitus
{
	public class AttributeOption
	{
		public string Id { get; protected set; }
		public string Description { get; protected set; }
		public bool? Active { get; protected set; }

		public AttributeOption(string id, string desc, bool? active)
		{
			Id = id;
			Description = desc;
			Active = active;
		}

		public override string ToString()
		{
			return Description ?? String.Empty;
		}
	}
}
