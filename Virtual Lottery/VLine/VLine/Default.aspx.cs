using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using VLine.Models;
using Nysf.Tessitura;
using Nysf.Tessitura.TessituraWebApi;

namespace VLine.WebForms
{
    public class Main : Page
    {
        protected Button btnLogOut;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!WebClient.IsLoggedIn())
            {
                Response.Redirect("/login?referer=/");
            }
        }

        protected void LogOut(object sender, EventArgs e)
        {
            WebClient.Logout();
            Response.Redirect("/login");
        }
    }
}