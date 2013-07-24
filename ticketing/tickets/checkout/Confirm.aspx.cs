using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Nysf.Tessitura;
using Nysf.Types;
using Facebook;
namespace tickets.checkout
{
    public partial class Confirmed : System.Web.UI.Page
    {
        public string REDIRECT_URL;
        protected void Page_Load(object sender, EventArgs e)
        {
            //REDIRECT_URL = (Session["referer"]?? "/default.aspx").ToString() ;
            if (Request["accessToken"] != null && !WebClient.IsLoggedIn())
            {
                Session["facebookAccess"] = Request["accessToken"];
                var accessToken = Session["facebookAccess"].ToString();
                var client = new FacebookClient(accessToken);
                dynamic result = client.Get("me", new { fields = "first_name,last_name,email,id,username" });
                string firstName = result.first_name;
                string lastName = result.last_name;
                string id = result.id;
                string email=result.email;
                string userName = result.username;

               
                if(!WebClient.Login(email, id))
                {
                    if (WebClient.RegisterNewConstituentFacebook(email, firstName, lastName, -1, Organization.PublicTheater, id))                
                        WebClient.Login(email, id);
                }
                if (!WebClient.IsLoggedIn())
                {
                    if (!WebClient.Login(userName + "@facebook.com", id))
                    {
                        if (WebClient.RegisterNewConstituentFacebook(userName + "@facebook.com", firstName, lastName, -1, Organization.PublicTheater, id))
                            WebClient.Login(userName + "@facebook.com", id);
                    }
                }
                //if (!WebClient.IsLoggedIn())
                //{
                //    message.Text = "Sorry, we could not log you in with facebook. Please log in using your Public Theater account or create a new one.";
                //    message.ForeColor = System.Drawing.Color.Red;
                //}
                
            }
            
        }
    }
}