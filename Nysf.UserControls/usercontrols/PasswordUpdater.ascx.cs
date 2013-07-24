// TODO: if ultimately not needed, remove comment references to SessionHandler's "current page"

using System;
using System.Data;
using System.Web.Configuration;
using System.Web.UI;
using Nysf.Tessitura;

namespace Nysf.UserControls
{
    /// <summary>
    ///     A form that updates a user's password.
    /// </summary>
    public partial class PasswordUpdater : GenericControl
    {
        #region Properties

        public string OldPasswordLabelText
        {
            get { return OldPasswordLabel.Text; }
            set { OldPasswordLabel.Text = value; }
        }

        public string NewPasswordLabelText
        {
            get { return NewPasswordLabel.Text; }
            set { NewPasswordLabel.Text = value; }
        }

        public string ConfirmPasswordLabelText
        {
            get { return ConfirmPasswordLabel.Text; }
            set { ConfirmPasswordLabel.Text = value; }
        }

        public string OldPasswordRequiredErrorMessage
        {
            get { return OldPasswordRequiredValidator.ErrorMessage; }
            set { OldPasswordRequiredValidator.ErrorMessage = value; }
        }

        public string NewPasswordRequiredErrorMessage
        {
            get { return NewPasswordRequiredValidator.ErrorMessage; }
            set { NewPasswordRequiredValidator.ErrorMessage = value; }
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

        public string OldPasswordBadErrorMessage
        {
            get { return OldPasswordBadWarning.Text; }
            set { OldPasswordBadWarning.Text = value; }
        }

        public string SuccessMessage
        {
            get { return SuccessBlurb.Text; }
            set { SuccessBlurb.Text = value; }
        }

        #endregion

        #region Methods

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                // Check for Tessitura session key

                if (Session[WebClient.TessSessionKeySessionKey] == null)
                    throw new ApplicationException("There is no Tessitura " +
                        "session key associated with this browser session.");
                Utility.SetReferer(ViewState);
            }
        }

        /// <summary>
        ///     Update the user's login information.
        /// </summary>
        protected void SubmitButton_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                if (WebClient.SetNewPassword(NewPasswordTextBox.Text.Trim()))
                    PasswordWizard.ActiveViewIndex = 1;
                else
                    PasswordWizard.ActiveViewIndex = 2;
            }
        }

        /// <summary>
        ///     Redirect the user to the last page, or to the homepage.
        /// </summary>
        protected void ReturnToReferer(object sender, EventArgs e)
        {
            Utility.RedirectToReferer(ViewState);
        }

        protected void TryAgain(object sender, EventArgs e)
        {
            PasswordWizard.ActiveViewIndex = 0;
        }

        #endregion

    }
}