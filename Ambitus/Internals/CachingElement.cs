using System;
using System.Configuration;

namespace Ambitus.Internals
{
	public class CachingElement : ConfigurationElement
	{
		[ConfigurationProperty("longCacheMinutes", IsRequired = true)]
		public int LongCacheMinutes
		{
			get
			{
				return (int)this["longCacheMinutes"];
			}
			set
			{
				this["longCacheMinutes"] = value;
			}
		}

		[ConfigurationProperty("shortCacheMinutes", IsRequired = true)]
		public int ShortCacheMinutes
		{
			get
			{
				return (int)this["shortCacheMinutes"];
			}
			set
			{
				this["shortCacheMinutes"] = value;
			}
		}
	}
}
