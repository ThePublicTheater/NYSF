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
    public class Index : Page
    {
        protected TextBox txtUname;
        protected TextBox txtPassword;
        protected Label lblErrorMessage;
        protected Button btnLogin;

        protected void Page_Load(object sender, EventArgs e)
        {
            
        }

        protected void SignIn(object sender, EventArgs e)
        {
            if ((String.IsNullOrEmpty(txtUname.Text) && String.IsNullOrEmpty(txtPassword.Text)) || (String.IsNullOrEmpty(txtUname.Text) || String.IsNullOrEmpty(txtPassword.Text)))
            {
                Response.Redirect("/login");
            }
            else
            {
                if (WebClient.Login(txtUname.Text, txtPassword.Text))
                    Response.Redirect("/");
            }

            Response.Redirect("/login");
        }
    }
}