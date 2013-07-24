using System.Configuration;

namespace Nysf.Web
{
	public class ConfigSection : ConfigurationSection
	{
		#region Instance members

		[ConfigurationProperty("browserSessionKey", IsRequired = true)]
		public string BrowserSessionKey
		{
			get
			{
				return (string)this["browserSessionKey"];
			}
			set
			{
				this["browserSessionKey"] = value;
			}
		}

		[ConfigurationProperty("sourceIdQueryStringKey", IsRequired = true)]
		public string SourceIdQueryStringKey
		{
			get
			{
				return (string)this["sourceIdQueryStringKey"];
			}
			set
			{
				this["sourceIdQueryStringKey"] = value;
			}
		}

		[ConfigurationProperty("debugEnableSsl", IsRequired = true)]
		public bool DebugEnableSsl
		{
			get
			{
				return (bool)this["debugEnableSsl"];
			}
			set
			{
				this["debugEnableSsl"] = value;
			}
		}

		[ConfigurationProperty("anonymousUsername", IsRequired = true)]
		public string AnonymousUsername
		{
			get
			{
				return (string)this["anonymousUsername"];
			}
			set
			{
				this["anonymousUsername"] = value;
			}
		}

		[ConfigurationProperty("anonymousPassword", IsRequired = true)]
		public string AnonymousPassword
		{
			get
			{
				return (string)this["anonymousPassword"];
			}
			set
			{
				this["anonymousPassword"] = value;
			}
		}

		[ConfigurationProperty("standardPages")]
		public StandardPagesElement StandardPages
		{
			get
			{
				return (StandardPagesElement)this["standardPages"];
			}
			set
			{
				this["standardPages"] = value;
			}
		}

		#endregion

		#region Static members

		public static ConfigSection Settings
		{
			get { return (ConfigSection)ConfigurationManager.GetSection("nysf.web"); }
		}

		#endregion
	}
}
