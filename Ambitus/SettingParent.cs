using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Ambitus
{
	public class SettingParent : IEnumerable<Setting>
	{
		private List<Setting> settings;

		public string Name { get; private set; }

		public Setting this[int i]
		{
			get
			{
				return settings[i];
			}
		}

		public int Count
		{
			get
			{
				return settings.Count;
			}
		}

		public SettingParent(string name, List<Setting> settings)
		{
			Name = name;
			this.settings = settings;
		}

		public string this[string n]
		{
			get
			{
				return (from s in settings
						where s.Name == n
						select s.Value).FirstOrDefault<string>();
			}
		}

		public IEnumerator<Setting> GetEnumerator()
		{
			return settings.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
}
