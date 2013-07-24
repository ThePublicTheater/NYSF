using System;

namespace Ambitus
{
	public class Zone
	{
		public int? Id { get; private set; }
		public string Name { get; private set; }

		public Zone(int? id, string name)
		{
			Id = id;
			Name = name;
		}

		public override string ToString()
		{
			return Name ?? String.Empty;
		}
	}
}
