using System;

namespace Ambitus
{
	public class Allocation
	{
		public int? Id { get; private set; }
		public string Name { get; private set; }

		public Allocation(int? id, string name)
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
