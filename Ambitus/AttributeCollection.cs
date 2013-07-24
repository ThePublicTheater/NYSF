using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Ambitus
{
	public class AttributeCollection : Internals.ApiResultInterpreter, IEnumerable<Attribute>
	{
		private List<Attribute> attributes;

		public Attribute this[int i]
		{
			get
			{
				return attributes[i];
			}
		}

		public Attribute this[string name]
		{
			get
			{
				return (from att in attributes
						where att.Description == name
						select att).SingleOrDefault<Attribute>();
			}
		}

		public int Count
		{
			get
			{
				return attributes.Count;
			}
		}

		public AttributeCollection(DataSet tessResults)
		{
			EnumerableRowCollection<DataRow> attributeRows =
					tessResults.Tables["AttributesKeyword"].AsEnumerable();
			EnumerableRowCollection<DataRow> optionRows =
					tessResults.Tables["AttributesOption"].AsEnumerable();
			int?[] ids = (from row in attributeRows
						select row.Field<int?>("keyword_no")).ToArray<int?>();
			attributes = new List<Attribute>();
			foreach (int? id in ids)
			{
				Attribute newAtt = (from row in attributeRows
									where row.Field<int?>("keyword_no") == id
									select new Attribute(
											id: id,
											desc: row.Field<string>("description"),
											dataType:
												ToAttributeDataType(row.Field<int?>("data_type")),
											editMask: row.Field<string>("edit_mask"),
											allowMultiple:
													ToBool(row.Field<string>("multiple_value")),
											options: ExtractOptions(optionRows, id)))
									.Single<Attribute>();
				attributes.Add(newAtt);
			}
		}

		private AttributeOptionCollection ExtractOptions(
				EnumerableRowCollection<DataRow> optionRows, int? id)
		{
			AttributeOptionCollection options = new AttributeOptionCollection(
					(from row in optionRows
					where row.Field<int>("keyword_no") == id
					select new AttributeOption(
							id: row.Field<string>("id_value"),
							desc: row.Field<string>("desc_value"),
							active: Invert(ToBool(row.Field<string>("inactive")))))
					.ToList<AttributeOption>());
			if (options.Count == 0)
			{
				return null;
			}
			return options;
		}

		public Attribute GetById(int id)
		{
			return (from att in attributes
					where att.Id == id
					select att).FirstOrDefault<Attribute>();
		}

		public IEnumerator<Attribute> GetEnumerator()
		{
			return attributes.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
}
