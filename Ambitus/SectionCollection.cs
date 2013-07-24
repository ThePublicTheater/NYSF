using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Ambitus
{
	public class SectionCollection
			: Internals.ApiResultInterpreter, IEnumerable<Section>
	{
		List<Section> sections;

		public Section this[int i]
		{
			get
			{
				return sections[i];
			}
		}

		public int Count
		{
			get
			{
				return sections.Count;
			}
		}

		public SectionCollection(DataTable tessResults)
		{
			sections = (from row in tessResults.AsEnumerable()
						  select new Section(
							   id: row.Field<int?>("section"),
							   name: row.Field<string>("section_desc")))
						.ToList<Section>();
		}

		public Section GetById(int id)
		{
			return (from p in sections
					where p.Id == id
					select p).FirstOrDefault<Section>();
		}

		public IEnumerator<Section> GetEnumerator()
		{
			return sections.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
}