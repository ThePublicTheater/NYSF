using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Ambitus
{
	public class VariableCollection : IEnumerable<Variable>
	{
		List<Variable> variables;

		public int Count
		{
			get
			{
				return variables.Count;
			}
		}

		public Variable this[int i]
		{
			get
			{
				return variables[i];
			}
		}

		public string this[string key]
		{
			get
			{
				return (from v in variables
						where v.Name == key
						select v.Value).FirstOrDefault<string>();
			}
		}

		public VariableCollection(DataTable tessResults)
		{
			variables = (from rows in tessResults.AsEnumerable()
						  select new Variable(
							  name: rows.Field<string>("Name"),
							  value: rows.Field<string>("Value"))).ToList<Variable>();
		}

		public IEnumerator<Variable> GetEnumerator()
		{
			return variables.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
}
