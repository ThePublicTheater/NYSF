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
	public partial class PartnersOrderForm : Nysf.Web.UserControls.UserControl
	{
		protected const string ProgNickName = "Partners";

		protected void Page_Load(object sender, EventArgs e)
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
		}

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
			if (!(Input_L1.Checked || Input_L2A.Checked || Input_L2B.Checked || Input_L3A.Checked
					|| Input_L3B.Checked || Input_L4A.Checked || Input_L4B.Checked
					|| Input_L5A.Checked || Input_L5B.Checked || Input_L6A.Checked
					|| Input_L6B.Checked))
			{
				InsertValidationErrorMessage("Please choose a giving level / amount.");
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
			if (Input_L1.Checked)
			{
				choice.Level = "Summer Partner";
				choice.Amount = 1000;
			}
			else if (Input_L2A.Checked)
			{
				choice.Level = "Shiva Partner";
				choice.Amount = 2000;
				choice.Option = 'A';
			}
			else if (Input_L2B.Checked)
			{
				choice.Level = "Shiva Partner";
				choice.Amount = 2000;
				choice.Option = 'B';
			}
			else if (Input_L3A.Checked)
			{
				choice.Level = "Martinson Partner";
				choice.Amount = 4000;
				choice.Option = 'A';
			}
			else if (Input_L3B.Checked)
			{
				choice.Level = "Martinson Partner";
				choice.Amount = 4000;
				choice.Option = 'B';
			}
			else if (Input_L4A.Checked)
			{
				choice.Level = "Anspacher Partner";
				choice.Amount = 6000;
				choice.Option = 'A';
			}
			else if (Input_L4B.Checked)
			{
				choice.Level = "Anspacher Partner";
				choice.Amount = 6000;
				choice.Option = 'B';
			}
			else if (Input_L5A.Checked)
			{
				choice.Level = "Newman Partner";
				choice.Amount = 10000;
				choice.Option = 'A';
			}
			else if (Input_L5B.Checked)
			{
				choice.Level = "Newman Partner";
				choice.Amount = 10000;
				choice.Option = 'B';
			}
			else if (Input_L6A.Checked)
			{
				choice.Level = "LuEsther Partner";
				choice.Amount = 15000;
				choice.Option = 'A';
			}
			else if (Input_L6B.Checked)
			{
				choice.Level = "LuEsther Partner";
				choice.Amount = 15000;
				choice.Option = 'B';
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
							choice.Option, Input_Anon.Checked, Input_AcknowlName.Text.Trim(),
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