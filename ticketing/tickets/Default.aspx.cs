using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace tickets
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object s, EventArgs e)
        {
            System.Net.ServicePointManager.ServerCertificateValidationCallback = ((sender, certificate, chain, sslPolicyErrors) => true);

        }
    }

}