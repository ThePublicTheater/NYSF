using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Nysf.Tessitura;
using System.Data;

namespace Nysf.Elmah
{
	public partial class WebForm1 : System.Web.UI.Page
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			WebClient.Login("bryandrenner@gmail.com", "Tr@v3lpu");
			DataSet results = WebClient.RawClient.GetPaymentMethod(Session[
					WebClient.TessSessionKeySessionKey].ToString());
		}
	}
}