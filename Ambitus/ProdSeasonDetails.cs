using System;
using System.Data;
using System.Linq;

namespace Ambitus
{
	public class ProdSeasonDetails : Internals.ApiResultInterpreter
	{
		public int? InventoryId { get; private set; }
		public string Name { get; private set; }
		public string TitleType { get; private set; }
		public string ShortName { get; private set; }
		public string Text1 { get; private set; }
		public string Text2 { get; private set; }
		public string Text3 { get; private set; }
		public string Text4 { get; private set; }
		public DateTime? PerfDate { get; private set; }
		public int? Id { get; private set; }
		public int? ProductionId { get; private set; }
		public int? TitleId { get; private set; }
		public int? Duration { get; private set; }
		public int? LanguageId { get; private set; }
		public decimal? Cost { get; private set; }
		public string Synopsis { get; private set; }
		public short? ActsCount { get; private set; }
		public string CreatedLocation { get; private set; }
		public string CreatedBy { get; private set; }
		public DateTime? CreatedDate { get; private set; }
		public short? EraId { get; private set; }
		public short? OriginalLanguageId { get; private set; }
		public string Libretto { get; private set; }
		public string Author { get; private set; }
		public string OriginalSynopsis { get; private set; }
		public string LanguageName { get; private set; }
		public short? ComposerId { get; private set; }
		public string ComposerFirstName { get; private set; }
		public string ComposerMiddleName { get; private set; }
		public string ComposerLastName { get; private set; }
		public string ComposerBio { get; private set; }
		public bool? Active { get; private set; }
		public string VenueName { get; private set; }
		public CreditCollection Credits { get; private set; }
		public PerfCollection Performances { get; private set; }

		public ProdSeasonDetails(DataSet tessResults, bool searchedByProdSeasonId)
		{
			var results = (from tRow in tessResults.Tables["Title"].AsEnumerable()
						  join pRow in tessResults.Tables["Production"].AsEnumerable()
						  on true equals true
						  select new
								  {
										InventoryId = tRow.Field<int?>("inv_no"),
										Name = tRow.Field<string>("description"),
										TitleType= tRow.Field<string>("type"),
										ShortName = tRow.Field<string>("short_name"),
										Text1 = tRow.Field<string>("text1"),
										Text2 = tRow.Field<string>("text2"),
										Text3 = tRow.Field<string>("text3"),
										Text4 = tRow.Field<string>("text4"),
										PerfDate = searchedByProdSeasonId ?
												null : tRow.Field<DateTime?>("perf_dt"),
										Id = pRow.Field<int?>("prod_season_no"),
										ProdId = pRow.Field<int?>("prod_no"),
										TitleId = pRow.Field<int?>("title_no"),
										Duration = pRow.Field<int?>("duration"),
										LanguageId = pRow.Field<int?>("lang"),
										Cost = pRow.Field<decimal?>("cost"),
										Synopsis = pRow.Field<string>("synop"),
										ActsCount = pRow.Field<short?>("num_acts"),
										CreatedLocation = pRow.Field<string>("create_loc"),
										CreatedBy = pRow.Field<string>("created_by"),
										CreatedDate = pRow.Field<DateTime?>("create_dt"),
										EraId = pRow.Field<short?>("era"),
										OriginalLanguageId = pRow.Field<short?>("orig_lang"),
										Libretto = pRow.Field<string>("lib"),
										Author = pRow.Field<string>("author"),
										OriginalSynopsis = pRow.Field<string>("orig_synop"),
										LanguageName = pRow.Field<string>("LangDesc"),
										ComposerId = pRow.Field<short?>("composerID"),
										ComposerFirstName = pRow.Field<string>("ComposerFN"),
										ComposerMiddleName = pRow.Field<string>("ComposerMN"),
										ComposerLastName = pRow.Field<string>("ComposerLN"),
										ComposerBio = pRow.Field<string>("bio"),
										Inactive = pRow.Field<string>("inactive"),
										VenueName = pRow.Field<string>("facility_desc")
								  }).Single();
			InventoryId = results.InventoryId;
			Name = results.Name;
			TitleType = results.TitleType;
			ShortName = results.ShortName;
			Text1 = results.Text1;
			Text2 = results.Text2;
			Text3 = results.Text3;
			Text4 = results.Text4;
			PerfDate = results.PerfDate;
			Id = results.Id;
			ProductionId = results.ProdId;
			TitleId = results.TitleId;
			Duration = results.Duration;
			LanguageId = results.LanguageId;
			Cost = results.Cost;
			Synopsis = results.Synopsis;
			ActsCount = results.ActsCount;
			CreatedLocation = results.CreatedLocation;
			CreatedBy = results.CreatedBy;
			CreatedDate = results.CreatedDate;
			EraId = results.EraId;
			OriginalLanguageId = results.OriginalLanguageId;
			Libretto = results.Libretto;
			Author = results.Author;
			OriginalSynopsis = results.OriginalSynopsis;
			LanguageName = results.LanguageName;
			ComposerId = results.ComposerId;
			ComposerFirstName = results.ComposerFirstName;
			ComposerMiddleName = results.ComposerMiddleName;
			ComposerLastName = results.ComposerLastName;
			ComposerBio = results.ComposerBio;
			Active = Invert(ToBool(results.Inactive));
			VenueName = results.VenueName;
			DataTableCollection tables = tessResults.Tables;
			if (tables["Performance"].Rows.Count != 0)
			{
				Performances = new PerfCollection(tables["Performance"]);
			}
			if (tables["Credits"].Rows.Count != 0)
			{
				Credits = new CreditCollection(tables["Credits"]);
			}
		}
	}
}