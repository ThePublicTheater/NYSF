using System;

namespace Nysf.Web.UserControls
{
	public partial class SessionTransferHandler : Nysf.Web.UserControls.UserControl
	{
		protected void Page_Init(object sender, EventArgs e)
		{
			NysfSession session = BrowserUtility.GetSession();
			if (session != null)
			{
				session.Revive();
			}
		}
	}
}