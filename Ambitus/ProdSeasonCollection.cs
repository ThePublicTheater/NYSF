using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Ambitus
{
	public class ProdSeasonCollection
			: Internals.ApiResultInterpreter, IEnumerable<ProdSeason>
	{
		private List<ProdSeason> prodSeasons;

		public ProdSeason this[int i]
		{
			get
			{
				return prodSeasons[i];
			}
		}

		public int Count
		{
			get
			{
				return prodSeasons.Count;
			}
		}

		public ProdSeasonCollection(DataTable prodSeasonTable, DataTable creditsTable,
				DataTable keywordsTable)
		{
			EnumerableRowCollection<DataRow> enumCredits = creditsTable.AsEnumerable();
			EnumerableRowCollection<DataRow> enumKeywords = keywordsTable.AsEnumerable();
			prodSeasons = (from row in prodSeasonTable.AsEnumerable()
						  select new ProdSeason(
							_prod_season_no: row.Field<int?>("prod_season_no"),
							_prod_desc: row.Field<string>("prod_desc"),
							_season_no: row.Field<short?>("season_no"),
							_season_desc: row.Field<string>("season_desc"),
							_language: row.Field<string>("language"),
							_composer: row.Field<string>("composer"),
							_author: row.Field<string>("author"),
							_first_dt: row.Field<DateTime?>("first_dt"),
							_last_dt: row.Field<DateTime?>("last_dt"),
							_synop: row.Field<string>("synop"),
							_premiere_id: row.Field<short?>("premiere_id"),
							_premiere_desc: row.Field<string>("premiere_desc"),
							_perf_no: row.Field<string>("perf_no"),
							_on_sale_ind: ToBool(row.Field<string>("on_sale_ind")),
							enumCredits: enumCredits,
							enumKeywords: enumKeywords))
						.ToList<ProdSeason>();
		}

		public ProdSeason GetById(int id)
		{
			return (from p in prodSeasons
					where p.Id == id
					select p).FirstOrDefault<ProdSeason>();
		}

		public IEnumerator<ProdSeason> GetEnumerator()
		{
			return prodSeasons.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
}
