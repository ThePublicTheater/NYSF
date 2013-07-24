namespace Ambitus
{
	public class Country
	{
		public int? Id { get; private set; }
		public string Name { get; private set; }
		public string Abbreviation { get; private set; }
		public StateProvUsePolicy? StateUsePolicy { get; private set; }

		public Country(int? id, string name, string abbrev, StateProvUsePolicy? stateUsePolicy)
		{
			Id = id;
			Name = name;
			Abbreviation = abbrev;
			StateUsePolicy = stateUsePolicy;
		}
	}
}
