using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Ambitus
{
	public class ConstituentAttributeCollection : Internals.ApiResultInterpreter,
			IEnumerable<ConstituentAttribute>
	{
		private List<ConstituentAttribute> attributes;

		public ConstituentAttribute this[int i]
		{
			get
			{
				return attributes[i];
			}
		}

		public ConstituentAttribute this[string name]
		{
			get
			{
				return (from att in attributes
						where att.Name == name
						select att).SingleOrDefault<ConstituentAttribute>();
			}
		}

		public int Count
		{
			get
			{
				return attributes.Count;
			}
		}

		public ConstituentAttributeCollection(DataTable tessResults)
		{
			attributes = (from row in tessResults.AsEnumerable()
						  select new ConstituentAttribute(
								id: row.Field<int?>("keyword_no"),
								name: row.Field<string>("attribute"),
								value: row.Field<string>("attribute_value"),
								dataType: ToAttributeDataType(row.Field<string>("data_type")),
								n1n2: row.Field<byte?>("n1n2_ind"))).ToList<ConstituentAttribute>();
		}

		public IEnumerator<ConstituentAttribute> GetEnumerator()
		{
			return attributes.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
}
