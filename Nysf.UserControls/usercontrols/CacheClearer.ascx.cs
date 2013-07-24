using System;
using Nysf.Tessitura;

namespace Nysf.UserControls
{
	public partial class CacheClearer : GenericControl
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			WebClient.ClearPerfsCache();
		}
	}
}