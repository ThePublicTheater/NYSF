using System;
using System.Web.Configuration;
using System.Web.UI;
using Nysf.Types;
using Nysf.Tessitura;

namespace Nysf.UserControls
{
    /// <summary>
    ///     A form that allows new constituents to register.
    /// </summary>
    public partial class RegistrationWizard : GenericControl
    {
        #region Properties

        public string FirstTimePromptText
        {
            get { return FirstTimePrompt.Text; }
            set { FirstTimePrompt.Text = value; }
        }

        public string FirstTimeAnswerNoText
        {
            get { return FirstTimeAnswer.Items.FindByValue("n").Text; }
            set { FirstTimeAnswer.Items.FindByValue("n").Text = value; }
        }

        public string FirstTimeAnswerYesText
        {
            get { return FirstTimeAnswer.Items.FindByValue("y").Text; }
            set { FirstTimeAnswer.Items.FindByValue("y").Text = value; }
        }

        public string FirstTimeAnswerErrorMessage
        {
            get { return FirstTimeAnswerValidator.ErrorMessage; }
            set { FirstTimeAnswerValidator.ErrorMessage = value; }
        }

        public string NewAccountSurnameLabelText
        {
            get { return NewAccountSurnameLabel.Text; }
            set { NewAccountSurnameLabel.Text = value; }
        }

        public string NewAccountSurnameErrorMessage
        {
            get { return NewAccountSurnameValidator.ErrorMessage; }
            set { NewAccountSurnameValidator.ErrorMessage = value; }
        }

        public string NewAccountGivenNameLabelText
        {
            get { return NewAccountGivenNameLabel.Text; }
            set { NewAccountGivenNameLabel.Text = value; }
        }

        public string NewAccountGivenNameErrorMessage
        {
            get { return NewAccountGivenNameValidator.ErrorMessage; }
            set { NewAccountGivenNameValidator.ErrorMessage = value; }
        }

        public string NewAccountEmailLabelText
        {
            get { return NewAccountEmailLabel.Text; }
            set { NewAccountEmailLabel.Text = value; }
        }

        public string NewAccountEmailRequiredErrorMessage
        {
            get { return NewAccountEmailRequiredValidator.ErrorMessage; }
            set { NewAccountEmailRequiredValidator.ErrorMessage = value; }
        }

        public string NewAccountEmailExpressionErrorMessage
        {
            get { return NewAccountEmailExpressionValidator.ErrorMessage; }
            set { NewAccountEmailExpressionValidator.ErrorMessage = value; }
        }

        public string ConfirmDetailsPromptText
        {
            get { return ConfirmDetailsPrompt.Text; }
            set { ConfirmDetailsPrompt.Text = value; }
        }

        public string ConfirmDetailsFullNameLabelText
        {
            get { return ConfirmDetailsFullNameLabel.Text; }
            set { ConfirmDetailsFullNameLabel.Text = value; }
        }

        public string ConfirmDetailsEmailLabelText
        {
            get { return ConfirmDetailsEmailLabel.Text; }
            set { ConfirmDetailsEmailLabel.Text = value; }
        }

        public string ConfirmDetailsFurtherInstructionsText
        {
            get { return ConfirmDetailsFurtherInstructions.Text; }
            set { ConfirmDetailsFurtherInstructions.Text = value; }
        }

        public string CheckEmailPromptText
        {
            get { return CheckEmailPrompt.Text; }
            set { CheckEmailPrompt.Text = value; }
        }

        public int EmailTemplateNumber { get; set; }

        public string RegisteringOrg { get; set; }

        #endregion

        #region Methods

        // TODO: force inclusion of EmailTemplateNumber attribute

        public RegistrationWizard()
        {
            if (!String.IsNullOrWhiteSpace(RegisteringOrg)
                && RegisteringOrg.ToLower() != "pt"
                && RegisteringOrg.ToLower() != "jp"
                && RegisteringOrg.ToLower() != "sitp")
            {
                throw new ArgumentException(
                    "The RegistrationWizard's RegisteringOrg must be \"pt\", \"jp\", or \"sitp\".");
            }
        }

		protected void Page_Load(object sender, EventArgs e)
		{
			if (!Page.IsPostBack)
			{
				string email = Request.QueryString["email"];
				if (!String.IsNullOrWhiteSpace(email))
				{
					NewWebAccountWizard.ActiveViewIndex = 1;
					NewAccountEmailTextBox.Text = email;
					NewAccountPreviousButton.Visible = false;
				}
			}
		}

        /// <summary>
        ///     Set the control for initial focus and the default button for the
        ///     form.
        /// </summary>
        protected void Page_PreRender(object sender, EventArgs e)
        {
            switch (NewWebAccountWizard.ActiveViewIndex)
            {
                case 0:
                    Page.SetFocus(FirstTimeAnswer);
                    Page.Form.DefaultButton = FirstTimeNextButton.UniqueID;
                    break;
                case 1:
                    Page.SetFocus(NewAccountGivenNameTextBox);
                    Page.Form.DefaultButton = NewAccountNextButton.UniqueID;
                    break;
                case 2:
                    Page.SetFocus(ConfirmDetailsNextButton);
                    Page.Form.DefaultButton = ConfirmDetailsNextButton.UniqueID;
                    break;
            }
        }

        /// <summary>
        ///   Switch to the input fields for basic login information if the
        ///   constituent is new.
        /// </summary>
        protected void FirstTimeNextButton_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                if (FirstTimeAnswer.SelectedValue == "n")
                    NewWebAccountWizard.ActiveViewIndex = 1;
                else
                    Response.Redirect(Utility.GetFullHrefFromSubpath(
                        WebConfigurationManager.AppSettings[
                            "nysf_UserControls_AccountLookupPageUrl"]));
            }
        }

        /// <summary>
        ///     Go back to the initial question view.
        /// </summary>
        protected void NewAccountPreviousButton_Click(object sender,
            EventArgs e)
        {
            NewWebAccountWizard.ActiveViewIndex = 0;
        }

        /// <summary>
        ///     If inputs are valid, go to the confirmation view.
        /// </summary>
        protected void NewAccountNextButton_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
                NewWebAccountWizard.ActiveViewIndex = 2;
        }

        /// <summary>
        ///     Go back to the login information input view.
        /// </summary>
        protected void ConfirmDetailsPreviousButton_Click(object sender,
            EventArgs e)
        {
            NewWebAccountWizard.ActiveViewIndex = 1;
        }

        /// <summary>
        ///     Register the constituent, send the registration confirmation
        ///     email, and notify the user to check email.
        /// </summary>
        protected void ConfirmDetailsNextButton_Click(object sender,
            EventArgs e)
        {
            if (Page.IsValid)
            {
                // Register the constituent

                if (String.IsNullOrWhiteSpace(RegisteringOrg))
                {
                    RegisteringOrg = WebConfigurationManager.AppSettings[
                        "nysf_UserControls_DefaultOrganization"];
                }
                Organization sourceOrg;
                switch (RegisteringOrg.ToLower())
                {
                    case "pt":
                        sourceOrg = Organization.PublicTheater;
                        break;
                    case "jp":
                        sourceOrg = Organization.JoesPub;
                        break;
                    case "sitp":
                        sourceOrg = Organization.ShakespeareInThePark;
                        break;
                    default:
                        sourceOrg = Organization.Other;
                        break;
                }

                bool registrationWasSuccessful =
                    WebClient.RegisterNewConstituent(
                        NewAccountEmailTextBox.Text.Trim(),
                        NewAccountGivenNameTextBox.Text.Trim(),
                        NewAccountSurnameTextBox.Text.Trim(),
                        EmailTemplateNumber,
                        sourceOrg);

                if (registrationWasSuccessful)
                {
                    NewWebAccountWizard.ActiveViewIndex = 3;
                }
                else
                {
                    // Redirect to login information input view if the email is
                    // already used

                    NewWebAccountWizard.ActiveViewIndex = 1;
                    EmailAlreadyAssociatedErrorPanel.Visible = true;
                    LookupLink.NavigateUrl = Utility.GetFullHrefFromSubpath(
                        WebConfigurationManager.AppSettings[
                            "nysf_UserControls_AccountLookupPageUrl"]);
                }
            }
        }

        #endregion
    }
}