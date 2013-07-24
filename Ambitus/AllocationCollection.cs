using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Ambitus
{
	public class AllocationCollection
			: Internals.ApiResultInterpreter, IEnumerable<Allocation>
	{
		List<Allocation> allocations;

		public Allocation this[int i]
		{
			get
			{
				return allocations[i];
			}
		}

		public int Count
		{
			get
			{
				return allocations.Count;
			}
		}

		public AllocationCollection(DataTable tessResults)
		{
			allocations = (from row in tessResults.AsEnumerable()
						  select new Allocation(
							   id: row.Field<int?>("ac_no"),
							   name: row.Field<string>("alloc_desc")))
						.ToList<Allocation>();
		}

		public Allocation GetById(int id)
		{
			return (from p in allocations
					where p.Id == id
					select p).FirstOrDefault<Allocation>();
		}

		public IEnumerator<Allocation> GetEnumerator()
		{
			return allocations.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
}
