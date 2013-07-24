using System;
using System.Configuration;

namespace Ambitus.Internals
{
	public class DefaultsElement : ConfigurationElement
	{
		[ConfigurationProperty("loadDynamically", IsRequired = true)]
		public Boolean LoadDynamically
		{
			get
			{
				return (bool)this["loadDynamically"];
			}
			set
			{
				this["loadDynamically"] = value;
			}
		}

		[ConfigurationProperty("ip", DefaultValue = "0.0.0.0", IsRequired = true)]
		[RegexStringValidator(@"^\d{1,3}\.\d{1,3}\.\d{1,3}.\d{1,3}$")]
		public String Ip
		{
			get
			{
				return (string)this["ip"];
			}
			set
			{
				this["ip"] = value;
			}
		}

		[ConfigurationProperty("businessUnit", IsRequired = true)]
		public int BusinessUnit
		{
			get
			{
				return (int)this["businessUnit"];
			}
			set
			{
				this["businessUnit"] = value;
			}
		}

		[ConfigurationProperty("loginTypeId", IsRequired = true)]
		public int LoginTypeId
		{
			get
			{
				return (int)this["loginTypeId"];
			}
			set
			{
				this["loginTypeId"] = value;
			}
		}

		[ConfigurationProperty("credentialsEmailTemplateId", IsRequired = true)]
		public int CredentialsEmailTemplateId
		{
			get
			{
				return (int)this["credentialsEmailTemplateId"];
			}
			set
			{
				this["credentialsEmailTemplateId"] = value;
			}
		}

		[ConfigurationProperty("modeOfSaleId", IsRequired = true)]
		public short ModeOfSaleId
		{
			get
			{
				return (short)this["modeOfSaleId"];
			}
			set
			{
				this["modeOfSaleId"] = value;
			}
		}

		[ConfigurationProperty("organizationName", IsRequired = true)]
		public string OrganizationName
		{
			get
			{
				return (string)this["organizationName"];
			}
			set
			{
				this["organizationName"] = value;
			}
		}

		[ConfigurationProperty("emptyStringLiteral", IsRequired = true)]
		public string EmptyStringLiteral
		{
			get
			{
				return (string)this["emptyStringLiteral"];
			}
			set
			{
				this["emptyStringLiteral"] = value;
			}
		}
	}
}
