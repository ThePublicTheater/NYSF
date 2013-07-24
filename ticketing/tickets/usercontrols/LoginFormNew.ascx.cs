using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Nysf.Tessitura;
using Nysf;
using System.Web.Configuration;


namespace tickets.usercontrols
{
    public partial class LoginForm : GenericControl
    {
        #region Variables

        bool useNewLoginAlgorithm;

        #endregion

        #region Properties

        public string DefaultEmail
        {
            get { return EmailTextBox.Text; }
            set { EmailTextBox.Text = value; }
        }

        public string EmailAddressLabelText
        {
            get { return EmailAddressLabel.Text; }
            set { EmailAddressLabel.Text = value; }
        }

        public string PasswordLabelText
        {
            get { return PasswordLabel.Text; }
            set { PasswordLabel.Text = value; }
        }

        /*public string RememberLabelText
        {
            get { return RememberLabel.Text; }
            set { RememberLabel.Text = value; }
        }*/

        public string LoginFailureMessage
        {
            get { return FailBlurb.Text; }
            set { FailBlurb.Text = value; }
        }

        private string _ConfirmAddress;
        public string ConfirmURL
        {
            get { return _ConfirmAddress; }
            set { _ConfirmAddress = value; }
        }

        #endregion

        #region Methods

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (Session[WebClient.TessSessionKeySessionKey] == null)
                    throw new ApplicationException("The LoginForm requires that a Tessitura session have "
                        + "been set by the SessionHandler control.");

                // TODO: require secure connection

                // TODO: should this be placed in viewstate by SessionHandler automatically?
                Utility.SetReferer(ViewState);
               Session["referer"] = Utility.GetReferer(ViewState);
            }

            // Determine whether to use the new login algorithm (using email instead of username)

            try
            {
                useNewLoginAlgorithm = Convert.ToBoolean(
                    WebConfigurationManager.AppSettings["nysf_UserControls_UseNewLoginAlgorithm"]);
            }
            catch (InvalidCastException exception)
            {
                throw new ApplicationException("The value for web.config's <appSettings> key, "
                    + "\"nysf_UserControls_UseNewLoginAlgorithm\", must be either \"true\" or "
                    + "\"false\".", exception);
            }
            EmailAddressValidator.Enabled = useNewLoginAlgorithm;

            EmailTextBox.Focus(); // TODO: make sure this works

            // TODO: set form's default button
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            Page.SetFocus(EmailTextBox);
            Page.Form.DefaultButton = SubmitButton.UniqueID;
        }

        protected void SubmitButton_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                // Attempt to authenticate the current Tessitura session key

                bool loginWasSuccessful =
                    WebClient.Login(EmailTextBox.Text.Trim(), PasswordTextBox.Text);

                if (loginWasSuccessful)
                {
                    // Redirect to activation page if login is temporary

                    if (Session[WebClient.TempLoginSessionKey] != null &&
                        (bool)Session[WebClient.TempLoginSessionKey])
                    {
                        Utility.RedirectWithReferer(
                            WebConfigurationManager.AppSettings["nysf_UserControls_ActivationPageUrl"],
                            ViewState);

                    }

                    // Redirect to the designated current page or to the homepage

                   // Utility.RedirectToReferer(ViewState);
                    Response.Redirect(_ConfirmAddress);
                }
                else
                {
                    /*// Log back in as anonymous user

                    string anonymousUsername =
                        WebConfigurationManager.AppSettings["nysf_Tessitura_AnonymousUsername"];
                    string anonymousPassword =
                        WebConfigurationManager.AppSettings["anonymousPassword"];

                    if (!Tessitura.Login(anonymousUsername,
                            anonymousPassword))
                        throw new Exception(
                            "Unable to log into Tessitura with the anonymous " +
                            "username and password.");*/

                    // Show the login failure blurb

                    FailBlurb.Visible = true;
                }
            }
        }

        #endregion
    }
   
}