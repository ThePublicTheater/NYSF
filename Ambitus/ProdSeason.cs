using System;
using System.Data;

namespace Ambitus
{
	public class ProdSeason
	{
		public int? Id { get; private set; }
		public string Name { get; private set; }
		public short? SeasonId { get; private set; }
		public string SeasonName { get; private set; }
		public string Language { get; private set; }
		public string Composer { get; private set; }
		public string Author { get; private set; }
		public DateTime? FirstDate { get; private set; }
		public DateTime? LastDate { get; private set; }
		public string Synopsis { get; private set; }
		public short? PremiereId { get; private set; }
		public string PremiereName { get; private set; }
		public string PerfId { get; private set; }
		public bool? IsOnSale { get; private set; }
		public CreditCollection Credits { get; private set; }
		public ProdSeasonKeywordCatCollection KeywordCategories { get; private set; }

		public ProdSeason(int? _prod_season_no, string _prod_desc, short? _season_no,
				string _season_desc, string _language, string _composer, string _author,
				DateTime? _first_dt, DateTime? _last_dt, string _synop, short? _premiere_id,
				string _premiere_desc, string _perf_no, bool? _on_sale_ind,
				EnumerableRowCollection<DataRow> enumCredits,
				EnumerableRowCollection<DataRow> enumKeywords)
		{
			Id = _prod_season_no;
			Name = _prod_desc;
			SeasonId = _season_no;
			SeasonName = _season_desc;
			Language = _language;
			Composer = _composer;
			Author = _author;
			FirstDate = _first_dt;
			LastDate = _last_dt;
			Synopsis = _synop;
			PremiereId = _premiere_id;
			PremiereName = _premiere_desc;
			PerfId = _perf_no;
			IsOnSale = _on_sale_ind;
			Credits = new CreditCollection(enumCredits, "prod_season_no", Id);
			KeywordCategories =
					new ProdSeasonKeywordCatCollection(enumKeywords, Id);
		}

		public override string ToString()
		{
			return Name ?? String.Empty;
		}
	}

}
