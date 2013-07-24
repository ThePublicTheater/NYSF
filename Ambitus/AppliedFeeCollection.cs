using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Ambitus
{
	public class AppliedFeeCollection
			: Internals.ApiResultInterpreter, IEnumerable<AppliedFee>
	{
		List<AppliedFee> fees;

		public AppliedFee this[int i]
		{
			get
			{
				return fees[i];
			}
		}

		public int Count
		{
			get
			{
				return fees.Count;
			}
		}

		public AppliedFeeCollection(DataTable tessResults)
		{
			fees = (from row in tessResults.AsEnumerable()
						  select new AppliedFee(
							    _id: row.Field<int?>("id"),
								_order_no: row.Field<int?>("order_no"),
								_li_seq_no: row.Field<int?>("li_seq_no"),
								_sli_no: row.Field<int?>("sli_no"),
								_fee_no: row.Field<int?>("fee_no"),
								_fee_desc: row.Field<string>("fee_desc"),
								_fee_amt: row.Field<decimal?>("fee_amt"),
								_fee_override_ind: ToBool(row.Field<string>("fee_override_ind")),
								_fee_override_amt: row.Field<decimal?>("fee_override_amt"),
								_user_def_ind: ToBool(row.Field<string>("user_def_ind")),
								_fee_frequency: row.Field<string>("fee_frequency"),
								_category: row.Field<short?>("category"),
								_category_desc: row.Field<string>("category_desc"),
								_db_status: row.Field<short?>("db_status")))
						.ToList<AppliedFee>();
		}

		public AppliedFee GetById(int id)
		{
			return (from p in fees
					where p.Id == id
					select p).FirstOrDefault<AppliedFee>();
		}

		public IEnumerator<AppliedFee> GetEnumerator()
		{
			return fees.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
}
