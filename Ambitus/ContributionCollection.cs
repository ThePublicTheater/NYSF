using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Ambitus
{
	public class ContributionCollection
			: Internals.ApiResultInterpreter, IEnumerable<Contribution>
	{
		List<Contribution> contributions;

		public Contribution this[int i]
		{
			get
			{
				return contributions[i];
			}
		}

		public int Count
		{
			get
			{
				return contributions.Count;
			}
		}

		public ContributionCollection(DataTable tessResults /* "Contribution" */)
		{
			contributions = (from row in tessResults.AsEnumerable()
						  select new Contribution(
								id: row.Field<int?>("ref_no"),
								orderId: row.Field<int?>("order_no"),
								amount: row.Field<decimal?>("contribution_amt"),
								fundId: row.Field<int?>("fund_no"),
								onAccountMethodId: row.Field<int?>("on_account_method"),
								fundName: row.Field<string>("fund_desc"),
								onAccountMethodName: row.Field<string>("on_account_desc"),
								action:
									ToContributionAction(row.Field<string>("renew_upgrade_ind")),
								decline: ToBool(row.Field<string>("decline_ind")),
								dbStatus: row.Field<short?>("db_status"),
								notes: row.Field<string>("notes"),
								custom0: row.Field<string>("custom_0"),
								custom1: row.Field<string>("custom_1"),
								custom2: row.Field<string>("custom_2"),
								custom3: row.Field<string>("custom_3"),
								custom4: row.Field<string>("custom_4"),
								custom5: row.Field<string>("custom_5"),
								custom6: row.Field<string>("custom_6"),
								custom7: row.Field<string>("custom_7"),
								custom8: row.Field<string>("custom_8"),
								custom9: row.Field<string>("custom_9")))
						.ToList<Contribution>();
		}

		public Contribution GetById(int id)
		{
			return (from c in contributions
					where c.Id == id
					select c).FirstOrDefault<Contribution>();
		}

		public IEnumerator<Contribution> GetEnumerator()
		{
			return contributions.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
}
