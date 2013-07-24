using System;
using System.Configuration;

namespace Ambitus.Internals
{
	public class SessionElement : ConfigurationElement
	{
		[ConfigurationProperty("enablePasswordEncryption", IsRequired = true)]
		public bool EnablePasswordEncryption
		{
			get
			{
				return (bool)this["enablePasswordEncryption"];
			}
			set
			{
				this["enablePasswordEncryption"] = value;
			}
		}

		[ConfigurationProperty("passwordSalt", IsRequired = true)]
		public string PasswordSalt
		{
			get
			{
				return (string)this["passwordSalt"];
			}
			set
			{
				this["passwordSalt"] = value;
			}
		}

		[ConfigurationProperty("useSessionCache", IsRequired = true)]
		public bool UseSessionCache
		{
			get
			{
				return (bool)this["useSessionCache"];
			}
			set
			{
				this["useSessionCache"] = value;
			}
		}

		[ConfigurationProperty("seatServerClockOffsetSeconds", IsRequired = true)]
		public int SeatServerClockOffsetSeconds
		{
			get
			{
				return (int)this["seatServerClockOffsetSeconds"];
			}
			set
			{
				this["seatServerClockOffsetSeconds"] = value;
			}
		}

		[ConfigurationProperty("cartExpirationSeconds", IsRequired = true)]
		public int CartExpirationSeconds
		{
			get
			{
				return (int)this["cartExpirationSeconds"];
			}
			set
			{
				this["cartExpirationSeconds"] = value;
			}
		}

		[ConfigurationProperty("debugCheckSession", IsRequired = true)]
		public bool DebugCheckSession
		{
			get
			{
				return (bool)this["debugCheckSession"];
			}
			set
			{
				this["debugCheckSession"] = value;
			}
		}

	}
}
