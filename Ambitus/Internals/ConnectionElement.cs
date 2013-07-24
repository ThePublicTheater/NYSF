using System;
using System.Configuration;

namespace Ambitus.Internals
{
	public class ConnectionElement : ConfigurationElement
	{
		[ConfigurationProperty("allowUnsecure", IsRequired = true)]
		public Boolean AllowUnsecure
		{
			get
			{
				return (bool)this["allowUnsecure"];
			}
			set
			{
				this["allowUnsecure"] = value;
			}
		}

		[ConfigurationProperty("webApiUrl", IsRequired = true, DefaultValue = "http://localhost")]
		[RegexStringValidator(@"(http|https)://[\w\-_]+(\.[\w\-_]+)?([\w\-\.,@?^=%&amp;:/~\+#]*[\w\-\@?^=%&amp;/~\+#])?")]
		public string WebApiUrl
		{
			get
			{
				return (string)this["webApiUrl"];
			}
			set
			{
				this["webApiUrl"] = value;
			}
		}
	}
}
