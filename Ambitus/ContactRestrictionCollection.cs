using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Ambitus
{
	public class ContactRestrictionCollection : Internals.ApiResultInterpreter,
			IEnumerable<ContactRestriction>
	{
		private List<ContactRestriction> restrictions;

		public ContactRestriction this[int i]
		{
			get
			{
				return restrictions[i];
			}
		}

		public int Count
		{
			get
			{
				return restrictions.Count;
			}
		}

		public ContactRestrictionCollection(DataTable tessResults)
		{
			restrictions = (from item in tessResults.AsEnumerable()
							select new ContactRestriction(
									id: item.Field<int?>("id"),
									desc: item.Field<string>("description"),
									type: ToContactRestrictionType(item.Field<string>("type"))))
							.ToList<ContactRestriction>();
		}

		private ContactRestrictionCollection(List<ContactRestriction> subset)
		{
			restrictions = subset;
		}

		public ContactRestrictionCollection GetByType(ContactRestrictionType type)
		{
			List<ContactRestriction> subset = (from item in restrictions
												where item.Type == type
												select item).ToList<ContactRestriction>();
			if (subset.Count == 0)
			{
				return null;
			}
			return new ContactRestrictionCollection(subset);
		}

		public string GetDescription(ContactRestrictionType type, int id)
		{
			return (from item in restrictions
					where item.Type == type && item.Id == id
					select item.Description).FirstOrDefault<string>();
		}

		public IEnumerator<ContactRestriction> GetEnumerator()
		{
			return restrictions.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
}
