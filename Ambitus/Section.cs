using System;

namespace Ambitus
{
	public class Section
	{
		public int? Id { get; private set; }
		public string Name { get; private set; }

		public Section(int? id, string name)
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
