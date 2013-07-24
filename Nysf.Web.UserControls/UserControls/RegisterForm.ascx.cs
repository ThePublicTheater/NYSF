using Ambitus;
using Nysf.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Nysf.Web.UserControls
{
	public partial class RegisterForm : UserControl
	{
		public bool SendConfirmationEmail { get; set; }
		public int EmailTemplateId { get; set; }
		public string NewAccountAttributeValuePairs { get; set; }
		public string Copy_EmailUsed { get; set; }

		// TODO: make copy properties

		public RegisterForm()
		{
			SetDefaults();
		}

		protected void SetDefaults()
		{
			SendConfirmationEmail = false;
			EmailTemplateId = -1;
			NewAccountAttributeValuePairs = String.Empty;
			Copy_EmailUsed = "That email address has been used for another account.";
		}

		protected void CheckAttributeValuePairs()
		{
			// TODO: make this more eloquent (regex?)
			int length = NewAccountAttributeValuePairs.Length;
			if (length == 0)
			{
				return;
			}
			bool errorFound = false;
			if (length < 3)
			{
				errorFound = true;
			}
			else if (NewAccountAttributeValuePairs[length - 1] == '&'
					|| NewAccountAttributeValuePairs[length - 1] == '=')
			{
				errorFound = true;
			}
			else
			{
				char lastFoundControlChar = '&';
				bool foundNonControlChar = false;
				for (int cIndex = 0; cIndex < length; cIndex++)
				{
					char character = NewAccountAttributeValuePairs[cIndex];
					if (character == '&' || character == '=')
					{
						if (!foundNonControlChar || character == lastFoundControlChar)
						{
							errorFound = true;
							break;
						}
						lastFoundControlChar = character;
						foundNonControlChar = false;
					}
					else
					{
						foundNonControlChar = true;
					}
				}
			}
			if (errorFound)
			{
				throw new ApplicationException(
					"The RegisterForm's NewAccountAttributeValuePairs value is invalid.");
			}
		}

		protected void Page_Init(object sender, EventArgs e)
		{
			if (!Page.IsPostBack)
			{
				CheckAttributeValuePairs();
			}
		}

		protected void Page_PreRender(object sender, EventArgs e)
		{
			Input_FirstName.Focus();
		}

		protected void Do_Register(object sender, EventArgs e)
		{
			if (Page.IsValid)
			{
				Dictionary<string,string> attributeValuePairs = null;
				if (NewAccountAttributeValuePairs.Length > 2)
				{
					string[] pairs = NewAccountAttributeValuePairs.Split('&');
					attributeValuePairs = new Dictionary<string,string>();
					foreach (string pair in pairs)
					{
						string[] keyValue = pair.Split('=');
						attributeValuePairs.Add(keyValue[0],keyValue[1]);
					}
				}
				NysfSession session = BrowserUtility.GetSession();
				RegisterResult result = session.Register(Input_FirstName.Text, Input_LastName.Text,
						Input_Email.Text, Input_Password.Text, Input_EmailOptIn.Checked,
						attributeValuePairs, SendConfirmationEmail, EmailTemplateId);
				if (result == RegisterResult.Success)
				{
					BrowserUtility.RedirectToLastPage();
				}
				Input_EmailConfirm.Text = String.Empty;
				InsertValidationErrorMessage(Copy_EmailUsed);
			}
		}
	}
}