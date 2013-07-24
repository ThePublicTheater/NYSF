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
	public partial class ActivationForm : UserControl
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			if (!Page.IsPostBack)
			{
				bool tempAuthenticated = false;
				NysfSession session = BrowserUtility.GetSession();
				string queryEmail = Request.QueryString["email"];
				string queryToken = Request.QueryString["token"];
				if (!(String.IsNullOrWhiteSpace(queryEmail)
						|| String.IsNullOrWhiteSpace(queryToken)))
				{
					tempAuthenticated = session.LoginWithToken(queryEmail, queryToken);
				}
				else if (session.IsTemporary.Value)
				{
					tempAuthenticated = true;
				}
				else
				{
					BrowserUtility.RedirectToLastPage();
				}
				if (tempAuthenticated)
				{
					DisplayUsername.Text = session.Username;
					Input_Email.Value = queryEmail;
				}
				else
				{
					Views.ActiveViewIndex = 1;
				}
			}
		}

		protected void Page_PreRender(object sender, EventArgs e)
		{
			Input_Password.Focus();
		}

		protected void Do_ResetPassword(object sender, EventArgs e)
		{
			if (Page.IsValid)
			{
				NysfSession session = BrowserUtility.GetSession();
				session.UpdatePassword(Input_Password.Text);
				BrowserUtility.RedirectToLastPage();
			}
		}
	}
}