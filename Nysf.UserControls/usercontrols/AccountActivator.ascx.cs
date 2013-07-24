// TODO: if ultimately not needed, remove comment references to SessionHandler's "current page"

using System;
using System.Data;
using System.Web.Configuration;
using System.Web.UI;
using Nysf.Tessitura;

namespace Nysf.UserControls
{
    /// <summary>
    ///     A form that handles the password creation and activation of
    ///     temporary (new and forgotten-password) accounts.
    /// </summary>
    /// <remarks>
    ///     Ideally, when placed on a page, this control receives a user's email
    ///     address and temporary password from the query string.  It will also
    ///     facilitate changing the password for a user who is already
    ///     authenticated, ignoring the query string.
    ///     
    ///     When placed on a page with the SessionHandler, the SessionHandler
    ///     must not make the page current, and it must allow temporary users.
    ///     
    ///     Pages redirecting to this control that don't wish to be made the "referer" must set
    ///     their own referer by query string.
    /// </remarks>
    public partial class AccountActivator : GenericControl
    {
        #region Properties

        public string PasswordLabelText
        {
            get { return PasswordLabel.Text; }
            set { PasswordLabel.Text = value; }
        }

        public string ConfirmPasswordLabelText
        {
            get { return ConfirmPasswordLabel.Text; }
            set { ConfirmPasswordLabel.Text = value; }
        }

        public string PasswordRequiredErrorMessage
        {
            get { return PasswordRequiredValidator.ErrorMessage; }
            set { PasswordRequiredValidator.ErrorMessage = value; }
        }

        public string PasswordConfirmedErrorMessage
        {
            get { return PasswordConfirmedValidator.ErrorMessage; }
            set { PasswordConfirmedValidator.ErrorMessage = value; }
        }

        public string PasswordMismatchErrorMessage
        {
            get { return PasswordsMatchValidator.ErrorMessage; }
            set { PasswordsMatchValidator.ErrorMessage = value; }
        }

        public string SubmitButtonText
        {
            get { return SubmitButton.Text; }
            set { SubmitButton.Text = value; }
        }

        public string ContinueButtonText
        {
            get { return ContinueButton.Text; }
            set { ContinueButton.Text = value; }
        }

        public string AuthenticationErrorMessage
        {
            get { return ErrorMessage.Text; }
            set { ErrorMessage.Text = value; }
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Verify authenticity of the user who requires activation.
        /// </summary>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (!String.IsNullOrWhiteSpace(Request.QueryString["onactivate"]))
                {
                    if( RedirectUrl.Value.Length>0)
                        RedirectUrl.Value = Request.QueryString["onactivate"] + RedirectUrl.Value;
                    else
                        RedirectUrl.Value = Request.QueryString["onactivate"];
                }

                // Check for Tessitura session key

                if (Session[WebClient.TessSessionKeySessionKey] == null)
                    throw new ApplicationException("There is no Tessitura " +
                        "session key associated with this browser session.");

                Utility.SetReferer(ViewState);

                bool authenticationWasSuccessful = false;

                // Determine the username to activate

                string targetUsername = "";
                if (Request.QueryString["ident"] != null &&
                    Request.QueryString["ident"] != "")
                    targetUsername = Request.QueryString["ident"];
                else
                    if (WebClient.IsLoggedIn())
                    {
                        targetUsername = Session[WebClient.UsernameSessionKey].ToString();
                    }

                // Authenticate the target username if necessary

                if (targetUsername != "")
                {
                    if (Session[WebClient.UsernameSessionKey] == null
                        || targetUsername != Session[WebClient.UsernameSessionKey].ToString())
                    {
                        if (String.IsNullOrWhiteSpace(Request.QueryString["token"]))
                            throw new ApplicationException("A password token " +
                                "is required to activate the user.");
                        string targetPassword = Request.QueryString["token"];
                        authenticationWasSuccessful =
                            WebClient.Login(targetUsername,
                                targetPassword);
                    }
                    else
                    {
                        authenticationWasSuccessful = true;
                    }
                }

                // Display error if authentication failed

                if (!authenticationWasSuccessful)
                {
                    PasswordCreationWizard.ActiveViewIndex = 2;
                }
            }
        }

        /// <summary>
        ///     Update the user's login information.
        /// </summary>
        protected void SubmitButton_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                WebClient.SetNewPassword(PasswordTextBox.Text.Trim());
				if (!String.IsNullOrWhiteSpace(RedirectUrl.Value))
					Response.Redirect(RedirectUrl.Value);
				else
					PasswordCreationWizard.ActiveViewIndex = 1;
            }
        }

        /// <summary>
        ///     Redirect the user to the last page, or to the homepage.
        /// </summary>
        protected void ContinueButton_Click(object sender, EventArgs e)
        {
            Utility.RedirectToReferer(ViewState);
        }

        #endregion
    }
}