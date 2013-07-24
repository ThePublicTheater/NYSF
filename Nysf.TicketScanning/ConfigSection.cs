using System.Configuration;

namespace Nysf.TicketScanning
{
	public class ConfigSection : ConfigurationSection
	{
		public static ConfigSection Settings
		{
			get { return (ConfigSection)ConfigurationManager.GetSection("nysf_TicketScanning"); }
		}

		[ConfigurationProperty("connectionString", IsRequired = true)]
		public string ConnectionString
		{
			get { return (string)this["connectionString"]; }
		}

		[ConfigurationProperty("cacheMins", IsRequired = true)]
		public int CacheMins
		{
			get { return (int)this["cacheMins"]; }
		}
	}
}