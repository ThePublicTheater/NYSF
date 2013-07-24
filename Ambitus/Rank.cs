namespace Ambitus
{
	public class Rank
	{
		public int? TypeId { get; private set; }
		public string TypeName { get; private set; }
		public short? Value { get; private set; }

		public Rank(int? _rank_type, string _rank_type_desc, short? _rank)
		{
			TypeId = _rank_type;
			TypeName = _rank_type_desc;
			Value = _rank;
		}
	}
}
