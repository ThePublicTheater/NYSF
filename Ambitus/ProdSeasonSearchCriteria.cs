using System;
namespace Ambitus
{
	public class ProdSeasonSearchCriteria
	{
		public DateTime? StartDateTime { get; set; }
		public DateTime? EndDateTime { get; set; }
		public short? VenueId { get; set; }
		public short? ModeOfSaleId { get; set; }
		public int? BusinessUnit { get; set; }
		public string[] Keywords { get; set; }
		public bool? MatchAllKeywords { get; set; }
		public string ArtistLastName { get; set; }
		public string ArtistFirstName { get; set; }
		public string ArtistMiddleName { get; set; }
		public string FullTextSearchCriteria { get; set; }
		public FullTextSearchSyntaxType? FullTextSearchSyntaxType { get; set; }
		public ContentTypeIdsParam ContentTypeIds { get; set; }
		public int[] Ids { get; set; }
		public int[] PerfIds { get; set; }
		public short[] SeasonIds { get; set; }
	}
}
