using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Nysf.Tessitura;

namespace Nysf.Apps.Utr
{
	public partial class Default : System.Web.UI.Page
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			if (!Page.IsPostBack)
				WebClient.Login("bryandrenner@gmail.com", "Tr@v3lpu");
		}
	}
}