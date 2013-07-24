using System;

namespace Nysf.Web.UserControls
{
	public partial class LogoutHandler : System.Web.UI.UserControl
	{
		protected void Page_Init(object sender, EventArgs e)
		{
			BrowserUtility.LogoutAndClearSession();
		}
	}
}