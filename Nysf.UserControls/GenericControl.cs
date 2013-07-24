using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Configuration;
using System.Web.UI.WebControls;

namespace Nysf.UserControls
{
    public abstract class GenericControl : System.Web.UI.UserControl
    {
        protected const string TargetSmallScreenUserAgentSessionKey =
            "nysf_UserControls_TargetSmallScreenUserAgent";

        static GenericControl()
        {
            // Check for required entries in the config file

            string[] requiredSettingKeys =
            {
                "nysf_UserControls_LoginPageUrl",
                "nysf_UserControls_RegistrationPageUrl",
                "nysf_UserControls_AccountLookupPageUrl",
                "nysf_UserControls_ActivationPageUrl",
                "nysf_UserControls_CartPageUrl",
                "nysf_UserControls_PromoCodeInputPageUrl",
                "nysf_UserControls_ManagePageUrl",
                "nysf_UserControls_ChangePasswordPageUrl",
                "nysf_UserControls_UpdateAccountInfoPageUrl",
                "nysf_UserControls_LogoutPageUrl",
                "nysf_UserControls_CheckOutPageUrl",
                "nysf_UserControls_DefaultOrganization",
                "nysf_UserControls_DefaultStartOfUrlToStylesheets",
                "nysf_UserControls_RecaptchaPublicKey",
                "nysf_UserControls_RecaptchaPrivateKey",
                "nysf_UserControls_UseNewLoginAlgorithm",
                "nysf_UserControls_EnableRedirectsToHttps",
                "nysf_UserControls_CommunicationsPageUrl"
            };
            Utility.VerifyAppSettings(requiredSettingKeys);
        }

        public GenericControl() : base() { }

		protected void InsertValidationErrorMessage(string message)
		{
			CustomValidator tempValidator = new CustomValidator();
			tempValidator.IsValid = false;
			tempValidator.Display = ValidatorDisplay.None;
			tempValidator.ErrorMessage = message;
			Page.Form.Controls.Add(tempValidator);
		}
    }
}