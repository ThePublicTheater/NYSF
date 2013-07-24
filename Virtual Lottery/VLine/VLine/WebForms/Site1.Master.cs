using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Nysf.Tessitura;

namespace VLine.WebForms
{
    public partial class Site1 : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Page.Title="Shakespeare in the Park Virtual Ticketing";
        }
        protected void Page_Prerender(object sender, EventArgs e)
        {
            if (WebClient.IsLoggedIn())
            {
                AccountInfo account = WebClient.GetAccountInformation();
                UsernameLabel.Text = account.People.Person1.FirstName + " " + account.People.Person1.LastName+"!";
                noUser.Visible = false;
            }
            else
            {
                isUser.Visible = false;
                noUser.Visible = true;
            }
        }

        protected void LinkButton1_Click(object sender, EventArgs e)
        {
            WebClient.Logout();
            Response.Redirect("/login");
        }
    }
}