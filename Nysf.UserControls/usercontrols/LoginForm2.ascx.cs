using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Nysf.Tessitura;

namespace Nysf.UserControls
{
	public partial class LoginForm2 : GenericControl
	{
		#region Properties

		public bool RedirectOnSuccess { get; set; }
		public string RedirectUrl { get; set; }
		public string Copy_Blurb_LoginFailed { get; set; }

		public string Copy_Blurb_Prompt
		{
			get { return Blurb_Prompt.Text; }

			set { Blurb_Prompt.Text = value; }
		}

		public string Copy_Label_DefaultSubmit
		{
			get { return Label_DefaultSubmit.Text; }

			set { Label_DefaultSubmit.Text = value; }
		}

		public string Copy_Blurb_Legend
		{
			get { return Blurb_Legend.Text; }

			set { Blurb_Legend.Text = value; }
		}

		public string Copy_Label_Email
		{
			get { return Label_Email.Text; }

			set { Label_Email.Text = value; }
		}

		public string Copy_Blurb_EnterEmail
		{
			get { return Blurb_EnterEmail.Text; }

			set { Blurb_EnterEmail.Text = value; }
		}

		public string Copy_Blurb_FixEmail
		{
			get { return Blurb_FixEmail.Text; }

			set { Blurb_FixEmail.Text = value; }
		}

		public string Copy_Label_Password
		{
			get { return Label_Password.Text; }

			set { Label_Password.Text = value; }
		}

		public string Copy_Blurb_EnterPassword
		{
			get { return Blurb_EnterPassword.Text; }

			set { Blurb_EnterPassword.Text = value; }
		}

		public string Copy_Label_SubmitButton
		{
			get { return Label_SubmitButton.Text; }

			set { Label_SubmitButton.Text = value; }
		}

		#endregion

		#region Methods

		public LoginForm2()
		{
			SetDefaults();
		}

		protected void SetDefaults()
		{
			RedirectOnSuccess = true;
			RedirectUrl = "/";
			Copy_Blurb_LoginFailed = "Sorry. That username and password do not match.";
		}

		protected void Page_Load(object sender, EventArgs e)
		{
		}

		protected void Page_PreRender(object sender, EventArgs e)
		{
			Input_Email.Focus();
		}

		protected void Do_SignIn(object sender, EventArgs e)
		{
			if (Page.IsValid)
			{
				InsertValidationErrorMessage(Copy_Blurb_LoginFailed);
			}
		}

		#endregion
	}
}