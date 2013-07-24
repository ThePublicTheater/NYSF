using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;
using Nysf.Tessitura;
using Nysf.Types;

namespace Nysf.UserControls
{
    public partial class MailingOptInHandler : System.Web.UI.UserControl
    {
		public string RegisteringOrg { get; set; }

		public string PromptText
		{
			get { return Prompt.InnerHtml; }
			set { Prompt.InnerHtml = value; }
		}

		public string ButtonText
		{
			get { return JoinButton.Text; }
			set { JoinButton.Text = value; }
		}

        protected void Page_Load(object sender, EventArgs e)
        {
			if (!Page.IsPostBack)
			{
				if (!String.IsNullOrWhiteSpace(RegisteringOrg)
					&& RegisteringOrg.ToLower() != "pt"
					&& RegisteringOrg.ToLower() != "jp"
					&& RegisteringOrg.ToLower() != "sitp")
				{
					throw new ArgumentException(
						"The MailingOptInHandler's RegisteringOrg must be \"pt\", \"jp\", or "
						+ "\"sitp\".");
				}

			}
        }

		protected void DoJoin(object sender, EventArgs e)
		{
			if (String.IsNullOrWhiteSpace(RegisteringOrg))
			{
				RegisteringOrg = WebConfigurationManager.AppSettings[
					"nysf_UserControls_DefaultOrganization"];
			}
			string orgAttName = null;
			switch (RegisteringOrg.ToLower())
			{
				case "pt":
					orgAttName = "The Public Theater";
					break;
				case "jp":
					orgAttName = "Joe’s Pub";
					break;
				case "sitp":
					orgAttName = "Shakespeare in the Park";
					break;
			}
			Nysf.Tessitura.AttributeCollection sysAtts = WebClient.GetAttributes();
			Nysf.Tessitura.Attribute att = sysAtts.GetAttributeByName(
				"cp_Em_" + orgAttName);
			WebClient.AddAttribute(att.Id, 1, true);
			OptInViews.SetActiveView(DoneView);
			OrgTitleBlurb.Text = orgAttName;
		}
    }
}