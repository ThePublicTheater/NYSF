using Ambitus;
using Nysf.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Nysf.Apps.Giving
{
	public partial class GoingPublicOrderForm : Nysf.Web.UserControls.UserControl
	{
		/*protected void Page_Load(object sender, EventArgs e)
		{
			if (!Page.IsPostBack)
			{
				string level = Request.QueryString["l"];
				switch (level)
				{
					case "1": Input_L1.Checked = true; break;
					case "2a": Input_L2A.Checked = true; break;
					case "2b": Input_L2B.Checked = true; break;
					case "3a": Input_L3A.Checked = true; break;
					case "3b": Input_L3B.Checked = true; break;
					case "4a": Input_L4A.Checked = true; break;
					case "4b": Input_L4B.Checked = true; break;
					case "5a": Input_L5A.Checked = true; break;
					case "5b": Input_L5B.Checked = true; break;
					case "6a": Input_L6A.Checked = true; break;
					case "6b": Input_L6B.Checked = true; break;
				}
			}
		}*/

		protected bool CheckInput()
		{
			// Check for separater in text field
			if (Input_AcknowlName.Text.Contains(ContributionCustomData.Separator)
					|| Input_CorpName.Text.Contains(ContributionCustomData.Separator))
			{
				InsertValidationErrorMessage("Sorry. The '" + ContributionCustomData.Separator +
							"' character cannot be used.");
				return false;
			}
			// Check for required sets
			if (!(Input_PublicTheater.Checked || Input_JoesPub.Checked))
			{
				InsertValidationErrorMessage("Please choose the recipient for your gift.");
				return false;
			}
			if (Input_DoAcknowl.Checked && String.IsNullOrWhiteSpace(Input_AcknowlName.Text))
			{
				InsertValidationErrorMessage(
						"Please specify how you would like to be acknowledged for this donation.");
				return false;
			}
			return true;
		}

		protected LevelOption GetLevelOption()
		{
			LevelOption choice = new LevelOption();
			choice.Amount = Decimal.Parse(Input_Amount.Text);
			if (Input_PublicTheater.Checked)
			{
				choice.Option = 'P';
				if (choice.Amount < 1000)
					choice.Level = "Supporter";
				else if (choice.Amount < 10000)
					choice.Level = "Friend";
				else if (choice.Amount < 25000)
					choice.Level = "Peer";
				else if (choice.Amount < 100000)
					choice.Level = "Contributor";
				else if (choice.Amount < 1000000)
					choice.Level = "Angel";
				else
					choice.Level = "Leader";
			}
			else if (Input_JoesPub.Checked)
			{
				choice.Option = 'J';
				if (choice.Amount < 1000)
					choice.Level = "Opening Act";
				else if (choice.Amount < 5000)
					choice.Level = "Rising Star";
				else if (choice.Amount < 10000)
					choice.Level = "Headliner";
				else if (choice.Amount < 15000)
					choice.Level = "Diva";
				else if (choice.Amount < 25000)
					choice.Level = "Rock Star";
				else
					choice.Level = "Icon";
			}
			return choice;
		}

		protected void Do_AddContributionToCart(object sender, EventArgs e)
		{
			if (Page.IsValid)
			{
				if (CheckInput())
				{
					LevelOption choice = GetLevelOption();
					string targetName;
					switch (choice.Option.Value)
					{
						case 'P': targetName = "Going Public Campaign"; break;
						case 'J': targetName = "Going Public for Joe's Pub"; break;
						default: throw new ApplicationException("The contribution's target "
							+ "organization value is not recognized.");
					}
					NysfSession session = BrowserUtility.GetSession();
					session.AddContribution(choice.Amount, 80, targetName , choice.Level,
							choice.Option, Input_Anon.Checked, Input_AcknowlName.Text.Trim(),
							0, false, Input_Matching.Checked, Input_CorpName.Text.Trim());
					Response.Redirect(ConfigSection.Settings.StandardPages.Cart);
				}
			}
		}

		protected struct LevelOption
		{
			public string Level;
			public char? Option;
			public decimal Amount;
		}
	}
}