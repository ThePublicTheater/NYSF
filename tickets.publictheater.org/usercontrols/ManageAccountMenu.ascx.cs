using Nysf.Tessitura;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Nysf.UserControls
{
    public partial class ManageAccountMenu : GenericControl
    {

        #region Properties

        public bool ShowChangePasswordLink
        {
            get { return ChangePasswordLink.Visible; }
            set { ChangePasswordLink.Visible = value; }
        }

        public bool ShowUpdateAccountInfoLink
        {
            get { return UpdateLink.Visible; }
            set { UpdateLink.Visible = value; }
        }

        public bool ShowCommunicationsLink
        {
            get { return CommunicationsLink.Visible; }
            set { CommunicationsLink.Visible = value; }
        }

        public string ChangePasswordLinkText
        {
            get { return ChangePasswordLink.Text; }
            set { ChangePasswordLink.Text = value; }
        }

        public string UpdateAccountInfoLinkText
        {
            get { return UpdateLink.Text; }
            set { UpdateLink.Text = value; }
        }

        public string CommunicationsLinkText
        {
            get { return CommunicationsLink.Text; }
            set { CommunicationsLink.Text = value; }
        }

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            Utility.SetReferer(ViewState);
            if (!WebClient.IsLoggedIn())
            {
                throw new ApplicationException("The ManageAccountMenu requires an "
                    + "authenticated Tessitura session.");
            }
            if (ShowChangePasswordLink)
            {
                string link = Utility.GetFullHrefWithReferer(
                    WebConfigurationManager.AppSettings[
                        "nysf_UserControls_ChangePasswordPageUrl"], ViewState);
                ChangePasswordLink.NavigateUrl = link;
            }
            if (ShowUpdateAccountInfoLink)
            {
                string link = Utility.GetFullHrefWithReferer(
                    WebConfigurationManager.AppSettings[
                        "nysf_UserControls_UpdateAccountInfoPageUrl"], ViewState);
                UpdateLink.NavigateUrl = link;
            }
            if (ShowCommunicationsLink)
            {
                string link = Utility.GetFullHrefWithReferer(
                    WebConfigurationManager.AppSettings[
                        "nysf_UserControls_CommunicationsPageUrl"], ViewState);
                CommunicationsLink.NavigateUrl = link;
            }
        }
    }
}