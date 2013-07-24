using Ambitus;
using System;
using System.Web.UI;

namespace Nysf.Web.UserControls
{
	public partial class RecoverForm : UserControl
	{
		public int EmailTemplateId { get; set; }
		public string Blurb_NoAccountFound { get; set; }
		public string Blurb_AccountNotRecoverable { get; set; }

		public RecoverForm()
		{
			SetDefaults();
		}

		protected void SetDefaults()
		{
			Blurb_NoAccountFound =
					"Sorry, but no account could be found using that email address. Please try a different address, or call the Box Office at 212-967-7555 for further assistance.";
			Blurb_AccountNotRecoverable =
					"An account was found with that email address, but it does not have a website login.  Please call the Box Office at 212-967-7555 for further assistance.";
		}

		protected void Page_PreRender(object sender, EventArgs e)
		{
			if (Views.ActiveViewIndex == 0)
			{
				Input_Email.Focus();
			}
		}

		protected void Do_Lookup(object sender, EventArgs e)
		{
			if (Page.IsValid)
			{
				NysfSession session = BrowserUtility.GetSession();
				SendCredentialsResult result = session.SendAccountRecoveryEmail(Input_Email.Text,
						EmailTemplateId);
				switch (result)
				{
					case SendCredentialsResult.Success:
						Views.ActiveViewIndex = 1;
						break;
					case SendCredentialsResult.NotFound:
						InsertValidationErrorMessage(Blurb_NoAccountFound);
						break;
					case SendCredentialsResult.NoActiveLogin:
						InsertValidationErrorMessage(Blurb_AccountNotRecoverable);
						break;
					default:
						throw new ApplicationException("Account lookup failed unexpectedly: "
								+ result.ToString());
				}
			}
		}
	}
}