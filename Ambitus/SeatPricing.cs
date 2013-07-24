using System;
using System.Data;
using System.Linq;

namespace Ambitus
{
	public class SeatPricing : Internals.ApiResultInterpreter
	{
		public int? InventoryId { get; private set; }
		public string Name { get; private set; }
		public string Type { get; private set; }
		public string Text1 { get; private set; }
		public string Text2 { get; private set; }
		public string Text3 { get; private set; }
		public string Text4 { get; private set; }
		public DateTime? Date { get; private set; }
		public string ComposerFirstName { get; private set; }
		public string ComposerMiddleName { get; private set; }
		public string ComposerLastName { get; private set; }
		public string VenueName { get; private set; }
		public bool? OnSale { get; private set; }
		public string TypeName { get; private set; }
		public short? TypeId { get; private set; }
		public bool? Seat { get; private set; }
		public bool? Print { get; private set; }
		public int? ZmapId { get; private set; }
		public int? VenueId { get; private set; }
		public int? ProdSeasonId { get; private set; }
		public short? SeasonId { get; private set; }
		public string SeasonName { get; private set; }
		public DateTime? StartDate { get; private set; }
		public DateTime? EndDate { get; private set; }
		public short? StatusId { get; private set; }
		public string StatusName { get; private set; }
		public short? TimeSlotId { get; private set; }
		public string TimeSlotName { get; private set; }
		public bool? Ga { get; private set; }
		public ZonePriceDefaultCollection DefaultPricesPerZone { get; private set; }
		public PriceTypeCollection AvailablePriceTypes { get; private set; }
		public ZonePriceOptionCollection AllZonePriceCombos { get; private set; }
		public CreditCollection Credits { get; private set; }

		public SeatPricing(DataSet tessResults)
		{
			DataTableCollection tables = tessResults.Tables;
			if (tables["Price"].Rows.Count != 0)
			{
				DefaultPricesPerZone = new ZonePriceDefaultCollection(tables["Price"]);
			}
			if (tables["Credit"].Rows.Count != 0)
			{
				Credits = new CreditCollection(tables["Credit"]);
			}
			if (tables["PriceType"].Rows.Count != 0)
			{
				AvailablePriceTypes = new PriceTypeCollection(tables["PriceType"]);
			}
			if (tables["AllPrice"].Rows.Count != 0)
			{
				AllZonePriceCombos = new ZonePriceOptionCollection(tables["AllPrice"]);
			}
			var results = (from row in tessResults.Tables["Performance"].AsEnumerable()
						   select new
						   {
							   InventoryId = row.Field<int?>("inv_no"),
							   Name = row.Field<string>("description"),
							   Type = row.Field<string>("type"),
							   Text1 = row.Field<string>("text1"),
							   Text2 = row.Field<string>("text2"),
							   Text3 = row.Field<string>("text3"),
							   Text4 = row.Field<string>("text4"),
							   Date = row.Field<DateTime?>("perf_dt"),
							   ComposerFirstName = row.Field<string>("composerfn"),
							   ComposerMiddleName = row.Field<string>("composermn"),
							   ComposerLastName = row.Field<string>("composerln"),
							   VenueName = row.Field<string>("facility_desc"),
							   OnSale = ToBool(row.Field<string>("on_sale_ind")),
							   TypeName = row.Field<string>("performance_type"),
							   TypeId = row.Field<short?>("perf_type_id"),
							   Seat = ToBool(row.Field<string>("seat_ind")),
							   Print = ToBool(row.Field<string>("print_ind")),
							   ZmapId = row.Field<int?>("zmap_no"),
							   VenueId = row.Field<int?>("facility_no"),
							   ProdSeasonId = row.Field<int?>("prod_season_no"),
							   SeasonId = row.Field<short?>("season_no"),
							   SeasonName = row.Field<string>("season_desc"),
							   StartDate = row.Field<DateTime?>("start_dt"),
							   EndDate = row.Field<DateTime?>("end_dt"),
							   StatusId = row.Field<short?>("perf_status"),
							   StatusName = row.Field<string>("perf_status_desc"),
							   TimeSlotId = row.Field<short?>("time_slot"),
							   TimeSlotName = row.Field<string>("time_slot_desc"),
							   Ga = ToBool(row.Field<string>("ga_ind"))
						   }).Single();
			InventoryId = results.InventoryId;
			Name = results.Name;
			Type = results.Type;
			Text1 = results.Text1;
			Text2 = results.Text2;
			Text3 = results.Text3;
			Text4 = results.Text4;
			Date = results.Date;
			ComposerFirstName = results.ComposerFirstName;
			ComposerMiddleName = results.ComposerMiddleName;
			ComposerLastName = results.ComposerLastName;
			VenueName = results.VenueName;
			OnSale = results.OnSale;
			TypeName = results.TypeName;
			TypeId = results.TypeId;
			Seat = results.Seat;
			Print = results.Print;
			ZmapId = results.ZmapId;
			VenueId = results.VenueId;
			ProdSeasonId = results.ProdSeasonId;
			SeasonId = results.SeasonId;
			SeasonName = results.SeasonName;
			StartDate = results.StartDate;
			EndDate = results.EndDate;
			StatusId = results.StatusId;
			StatusName = results.StatusName;
			TimeSlotId = results.TimeSlotId;
			TimeSlotName = results.TimeSlotName;
			Ga = results.Ga;
		}
	}
}
