using System.Collections;
using System.Collections.Generic;

namespace Ambitus
{
	public class AttributeOptionCollection : IEnumerable<AttributeOption>
	{
		private List<AttributeOption> options;

		public AttributeOption this[int i]
		{
			get
			{
				return options[i];
			}
		}

		public int Count
		{
			get
			{
				return options.Count;
			}
		}

		public AttributeOptionCollection(List<AttributeOption> options)
		{
			this.options = options;
		}

		public AttributeOption GetOption(string id)
		{
			foreach (AttributeOption o in options)
			{
				if (o.Id == id)
				{
					return o;
				}
			}
			return null;
		}

		public IEnumerator<AttributeOption> GetEnumerator()
		{
			return options.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
}
