using System;

namespace Ambitus
{
	public class SeatType
	{
		public int? Id { get; private set; }
		public string Name { get; private set; }

		public SeatType(int? id, string name)
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
