using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Ambitus
{
	public class ContributionRecordCollection
			: Internals.ApiResultInterpreter, IEnumerable<ContributionRecord>
	{
		List<ContributionRecord> records;

		public ContributionRecord this[int i]
		{
			get
			{
				return records[i];
			}
		}

		public int Count
		{
			get
			{
				return records.Count;
			}
		}

		public ContributionRecordCollection(DataTable tessResults)
		{
			records = (from row in tessResults.AsEnumerable()
						  select new ContributionRecord(
							    _customer_no: row.Field<int?>("customer_no"),
								_cont_date: row.Field<DateTime?>("cont_date"),
								_recd_amt: row.Field<decimal?>("recd_amt"),
								_cont_amt: row.Field<decimal?>("cont_amt"),
								_type: row.Field<string>("type"),
								_campaign: row.Field<string>("campaign"),
								_appeal: row.Field<string>("appeal"),
								_media_type: row.Field<string>("media_type"),
								_source_name: row.Field<string>("source_name"),
								_fund_no: row.Field<string>("fund_no"),
								_ref_no: row.Field<int?>("ref_no"),
								_billing_type: row.Field<string>("billing_type"),
								_credit_ind: row.Field<string>("credit_ind"),
								_n1n2_ind: row.Field<string>("n1n2_ind"),
								_creditee_name: row.Field<string>("creditee_name"),
								_credited_amt: row.Field<decimal?>("credited_amt"),
								_solicitor: row.Field<string>("solicitor"),
								_cont_designation: row.Field<string>("cont_designation")))
						.ToList<ContributionRecord>();
		}

		public IEnumerator<ContributionRecord> GetEnumerator()
		{
			return records.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
}
