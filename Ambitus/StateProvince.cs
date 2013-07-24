using System;

namespace Ambitus
{
	public class StateProvince
	{
		public string Name { get; private set; }
		public string Abbreviation { get; private set; }
		public int? CountryId { get; private set; }

		public StateProvince(string name, string abbrev, int? countryId)
		{
			Name = name;
			Abbreviation = abbrev;
			CountryId = countryId;
		}

		public override string ToString()
		{
			return (Name ?? String.Empty) + ", " + (Abbreviation ?? String.Empty);
		}
	}
}
