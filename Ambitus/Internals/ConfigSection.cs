using System.Configuration;

namespace Ambitus.Internals
{
	public class ConfigSection : ConfigurationSection
	{
		#region Instance members

		[ConfigurationProperty("connection")]
		public ConnectionElement Connection
		{
			get
			{
				return (ConnectionElement)this["connection"];
			}
			set
			{
				this["connection"] = value;
			}
		}

		[ConfigurationProperty("defaults")]
		public DefaultsElement Defaults
		{
			get
			{
				return (DefaultsElement)this["defaults"];
			}
			set
			{
				this["defaults"] = value;
			}
		}

		[ConfigurationProperty("caching")]
		public CachingElement Caching
		{
			get
			{
				return (CachingElement)this["caching"];
			}
			set
			{
				this["caching"] = value;
			}
		}

		[ConfigurationProperty("session")]
		public SessionElement Session
		{
			get
			{
				return (SessionElement)this["session"];
			}
			set
			{
				this["session"] = value;
			}
		}

		#endregion

		#region Static members

		public static ConfigSection Settings
		{
			get { return (ConfigSection)ConfigurationManager.GetSection("ambitus"); }
		}

		#endregion
	}
}
