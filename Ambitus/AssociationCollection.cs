using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Ambitus
{
	public class AssociationCollection
			: Internals.ApiResultInterpreter, IEnumerable<Association>
	{
		List<Association> associations;

		public Association this[int i]
		{
			get
			{
				return associations[i];
			}
		}

		public int Count
		{
			get
			{
				return associations.Count;
			}
		}

		public AssociationCollection(DataTable tessResults)
		{
			associations = (from row in tessResults.AsEnumerable()
						  select new Association(
							_xref_no: row.Field<int?>("xref_no"),
							_xref_no_inv: row.Field<int?>("xref_no_inv"),
							_associate_no: row.Field<int?>("associate_no"),
							_category: row.Field<string>("category"),
							_xref_type: row.Field<int?>("xref_type"),
							_xref_type_desc: row.Field<string>("xref_type_desc"),
							_name: row.Field<string>("name"),
							_gender: row.Field<string>("gender"),
							_title: row.Field<string>("title"),
							_start_dt: row.Field<DateTime?>("start_dt"),
							_end_dt: row.Field<DateTime?>("end_dt"),
							_salary: row.Field<decimal?>("salary"),
							_notes: row.Field<string>("notes"),
							_n1n2_ind: row.Field<int?>("n1n2_ind"),
							_n1n2_ind_assoc: row.Field<int?>("n1n2_ind_assoc"),
							active: Invert(ToBool(row.Field<string>("inactive")))))
						.ToList<Association>();
		}

		public IEnumerator<Association> GetEnumerator()
		{
			return associations.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
}
