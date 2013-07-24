using System;
using Nysf.Tessitura;

namespace Nysf.UserControls
{
	public partial class AccountExistsCheck : System.Web.UI.UserControl
	{

		public string TrueRedirectUrl { get; set; }
		public string FalseRedirectUrl { get; set; }

		protected void Page_Init()
		{
			if (String.IsNullOrWhiteSpace(TrueRedirectUrl)
				|| String.IsNullOrWhiteSpace(FalseRedirectUrl))
			{
				throw new ApplicationException("The AccountExistsCheck user controls requires that "
					+ "TrueRedirectUrl and FalseRedirectUrl be set.");
			}
		}

		protected void CheckAddress(object sender, EventArgs e)
		{
			// TODO: accomodate redirectUrls that already have query strings
			if (WebClient.LoginExists(EmailInput.Text))
			{
				Response.Redirect(TrueRedirectUrl);
			}
			else
			{
				Response.Redirect(FalseRedirectUrl + "?email=" + EmailInput.Text);
			}
		}
	}
}