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
	public partial class FellowsOrderForm : Nysf.Web.UserControls.UserControl
	{
		protected const string ProgNickName = "Fellows";

		protected void Page_Load(object sender, EventArgs e)
		{
			if (!Page.IsPostBack)
			{
				string level = Request.QueryString["l"];
				switch (level)
				{
					//case "2": Input_L2.Checked = true; break;
					case "3": Input_L3.Checked = true; break;
				//	case "4": Input_L4.Checked = true; break;
					case "5": Input_L5.Checked = true; break;
				}
			}
		}

		protected bool CheckInput()
		{
			// Check for separater in text field
			if (Input_CorpName.Text.Contains(ContributionCustomData.Separator))
			{
				InsertValidationErrorMessage("Sorry. The '" + ContributionCustomData.Separator +
							"' character cannot be used.");
				return false;
			}
			// Check for required sets
			if (!(Input_L3.Checked || Input_L5.Checked))
			{
				InsertValidationErrorMessage("Please choose a giving level / amount.");
				return false;
			}
			return true;
		}

		protected LevelOption GetLevelOption()
		{
			LevelOption choice = new LevelOption();
            //if (Input_L2.Checked)
            //{
            //    choice.Level = "Friend";
            //    choice.Amount = 200;
            //}
			if (Input_L3.Checked)
			{
				choice.Level = "Champion";
				choice.Amount = 400;
			}
            //else if (Input_L4.Checked)
            //{
            //    choice.Level = "Emissary";
            //    choice.Amount = 750;
            //}
			else if (Input_L5.Checked)
			{
				choice.Level = "Partner";
				choice.Amount = 1000;
			}
			else
			{
				throw new ApplicationException(
						"The chosen level hasn't been accounted for in GetLevelOption().");
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
					NysfSession session = BrowserUtility.GetSession();
					session.AddContribution(choice.Amount, 15, ProgNickName, choice.Level,
							choice.Option, false, String.Empty,
							0, Input_DeclineBenefits.Checked,
							Input_Matching.Checked, Input_CorpName.Text.Trim());
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