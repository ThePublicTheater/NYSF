using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;
using Nysf.Tessitura;

namespace Nysf.UserControls.Demos
{
    public partial class AccountWidget : GenericControl
    {
        #region Properties

        public bool ShowGreeting { get; set; }

        public string PreNameGreetingText
        {
            get { return PreNameGreeting.Text; }
            set { PreNameGreeting.Text = value; }
        }

        public string PostNameGreetingText
        {
            get { return PostNameGreeting.Text; }
            set { PostNameGreeting.Text = value; }
        }

        public string LoginLinkText
        {
            get { return LoginLink.Text; }
            set { LoginLink.Text = value; }
        }

        public string CartLinkText
        {
            get { return CartLink.Text; }
            set { CartLink.Text = value;
            CartLink2.Text = value;
            }
        }

        public string PromoLinkText
        {
            get { return PromoLink.Text; }
            set { PromoLink.Text = value; }
        }

        public string ManageLinkText
        {
            get { return ManageLink.Text; }
            set { ManageLink.Text = value; }
        }

        public string LogoutLinkText
        {
            get { return LogoutLink.Text; }
            set { LogoutLink.Text = value; }
        }

        public bool ShowCartLink { get; set; }
/*        {
            get { return CartLinkLi.Visible; }
            set { CartLinkLi.Visible = value; }
        }*/

        public bool ShowPromoLink { get; set; }
/*        {
            get { return PromoLinkLi.Visible; }
            set { PromoLinkLi.Visible = value; }
        }*/

        #endregion

        public AccountWidget() : base()
        {
            ShowGreeting = true;
            ShowCartLink = true;
            //ShowPromoLink = true;
        }
        
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session[WebClient.TessSessionKeySessionKey] == null)
                throw new ApplicationException("The AccountWidget requires that a Tessitura "
                    + "session have been set by the SessionHandler control.");
            if (ShowCartLink && WebClient.CartHasItems())
            {
                CartLink.NavigateUrl = Utility.GetFullHrefFromSubpath(
                    WebConfigurationManager.AppSettings["nysf_UserControls_CartPageUrl"]);
                CartLink2.NavigateUrl = Utility.GetFullHrefFromSubpath(
                        WebConfigurationManager.AppSettings["nysf_UserControls_CartPageUrl"]);
            }
            else
            {
                CartLinkLi.Visible = false;
                CartLinkLi2.Visible = false;
            }
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            if (!WebClient.IsLoggedIn())
            {
                LoginLink.NavigateUrl = Utility.GetFullHrefFromSubpath(
                    WebConfigurationManager.AppSettings["nysf_UserControls_LoginPageUrl"]);
                RegisterLink.NavigateUrl = Utility.GetFullHrefFromSubpath(
                    WebConfigurationManager.AppSettings["nysf_UserControls_RegistrationPageUrl"]);
                AccountWidgetViews.ActiveViewIndex = 0;
            }
            else
            {
                if (ShowGreeting)
                {
                    NamesLiteral.Text = WebClient.BuildGreetingNames();
                }
                else
                    Greeting.Visible = false;
                
                
                if (ShowPromoLink)
                    PromoLink.NavigateUrl = Utility.GetFullHrefFromSubpath(
                        WebConfigurationManager.AppSettings[
                            "nysf_UserControls_PromoCodeInputPageUrl"]);
                else
                    PromoLinkLi.Visible = false;
                ManageLink.NavigateUrl = Utility.GetFullHrefFromSubpath(
                    WebConfigurationManager.AppSettings["nysf_UserControls_ManagePageUrl"]);
                LogoutLink.NavigateUrl = Utility.GetFullHrefFromSubpath(
                    WebConfigurationManager.AppSettings["nysf_UserControls_LogoutPageUrl"]);
                AccountWidgetViews.ActiveViewIndex = 1;
            }
        }
    }
}