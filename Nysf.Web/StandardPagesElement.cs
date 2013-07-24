using System;
using System.Configuration;

namespace Nysf.Web
{
	public class StandardPagesElement : ConfigurationElement
	{
		[ConfigurationProperty("timeout", IsRequired = true)]
		public String Timeout
		{
			get
			{
				return (string)this["timeout"];
			}
			set
			{
				this["timeout"] = value;
			}
		}

		[ConfigurationProperty("activate", IsRequired = true)]
		public String Activate
		{
			get
			{
				return (string)this["activate"];
			}
			set
			{
				this["activate"] = value;
			}
		}

		[ConfigurationProperty("expired", IsRequired = true)]
		public String Expired
		{
			get
			{
				return (string)this["expired"];
			}
			set
			{
				this["expired"] = value;
			}
		}

		[ConfigurationProperty("login", IsRequired = true)]
		public String Login
		{
			get
			{
				return (string)this["login"];
			}
			set
			{
				this["login"] = value;
			}
		}

		[ConfigurationProperty("enterPromo", IsRequired = true)]
		public String EnterPromo
		{
			get
			{
				return (string)this["enterPromo"];
			}
			set
			{
				this["enterPromo"] = value;
			}
		}

		[ConfigurationProperty("register", IsRequired = true)]
		public String Register
		{
			get
			{
				return (string)this["register"];
			}
			set
			{
				this["register"] = value;
			}
		}

		[ConfigurationProperty("cart", IsRequired = true)]
		public String Cart
		{
			get
			{
				return (string)this["cart"];
			}
			set
			{
				this["cart"] = value;
			}
		}

		[ConfigurationProperty("checkout", IsRequired = true)]
		public String Checkout
		{
			get
			{
				return (string)this["checkout"];
			}
			set
			{
				this["checkout"] = value;
			}
		}

		[ConfigurationProperty("account", IsRequired = true)]
		public String Account
		{
			get
			{
				return (string)this["account"];
			}
			set
			{
				this["account"] = value;
			}
		}

		[ConfigurationProperty("logout", IsRequired = true)]
		public String Logout
		{
			get
			{
				return (string)this["logout"];
			}
			set
			{
				this["logout"] = value;
			}
		}

		[ConfigurationProperty("postCheckout", IsRequired = true)]
		public String PostCheckout
		{
			get
			{
				return (string)this["postCheckout"];
			}
			set
			{
				this["postCheckout"] = value;
			}
		}
	}
}
