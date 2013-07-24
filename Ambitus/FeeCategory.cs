using System;

namespace Ambitus
{
	public class FeeCategory
	{
		public short? Id { get; private set; }
		public string Name { get; private set; }
		public decimal? Total { get; private set; }

		public FeeCategory(short? id, string name, decimal? total)
		{
			Id = id;
			Name = name;
			Total = total;
		}

		public override string ToString()
		{
			return Name ?? String.Empty;
		}
	}
}
