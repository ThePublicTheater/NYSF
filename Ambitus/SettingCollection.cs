using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Ambitus
{
	public class SettingCollection : IEnumerable<SettingParent>
	{
		private List<SettingParent> parents;

		public SettingParent this[int i]
		{
			get
			{
				return parents[i];
			}
		}

		public int Count
		{
			get
			{
				return parents.Count;
			}
		}

		public string this[string n]
		{
			get
			{
				foreach (SettingParent p in parents)
				{
					string value = p[n];
					if (value != null)
					{
						return value;
					}
				}
				return null;
			}
		}

		public int SettingsCount
		{
			get
			{
				int i = 0;
				foreach (SettingParent p in parents)
				{
					i += p.Count;
				}
				return i;
			}
		}

		public SettingCollection(DataTable tessResult)
		{
			var results = from row in tessResult.AsEnumerable()
						  select new
						  {
							  ParentName = row.Field<string>("parent_table"),
							  SettingName = row.Field<string>("field_name"),
							  SettingValue = row.Field<string>("default_value")
						  };

			string[] parentNames = (from item in results
										select item.ParentName)
										.Distinct<string>().ToArray<string>();

			parents = new List<SettingParent>();
			foreach (string parentName in parentNames)
			{
				List<Setting> newSettings = (from item in results
											 where parentName == item.ParentName
											 select new Setting(
													 name: item.SettingName,
													 value: item.SettingValue)).ToList<Setting>();
				parents.Add(new SettingParent(parentName, newSettings));
			}
		}

		public IEnumerator<SettingParent> GetEnumerator()
		{
			return parents.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
}
