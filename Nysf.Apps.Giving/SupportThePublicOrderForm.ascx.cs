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
    public partial class SupportThePublicOrderForm : Nysf.Web.UserControls.UserControl
    {
        protected const string ProgNickName = "Support";
        protected decimal Num;

        protected void Page_Load(object sender, EventArgs e)
        {

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
            if (String.IsNullOrWhiteSpace(Input_Contrib.Text))
            {
                InsertValidationErrorMessage(
                    "Please enter a contribution amount.");
                return false;
            }
            else
            {
                List<char> testerarray = Input_Contrib.Text.ToList();
                testerarray.RemoveAt(0);
                
                bool IsNum = decimal.TryParse(testerarray.ToString(), out Num);
                if (!IsNum)
                {
                    InsertValidationErrorMessage(
                        "Please enter a number for contribution amount.");
                    return false;
                }

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
            choice.Amount = Num;
            choice.CorpMatch = Input_Matching.Checked;
            if (choice.CorpMatch)
            {
                choice.CorpName = Input_CorpName.Text;
            }
            choice.TaxDeduct = Input_DeclineBenefits.Checked;
            choice.AnonDonation = Input_Anon.Checked;
            if (!choice.AnonDonation && Input_DoAcknowl.Checked)
            {
                choice.AckName = Input_AcknowlName.Text;
            }
            return choice;
        }

        protected void Do_AddContributionToCart(object sender, EventArgs e)
        {
            /*
            if (Page.IsValid)
            {
                if (CheckInput())
                {
                    LevelOption choice = GetLevelOption();
                    NysfSession session = BrowserUtility.GetSession();
                    session.AddContribution(choice.Amount, 15, ProgNickName, null,
                            null, choice.AnonDonation, choice.AckName.Trim(),
                            0, choice.TaxDeduct, choice.CorpMatch, choice.CorpName.Trim());
                    Response.Redirect(ConfigSection.Settings.StandardPages.Cart);
                }
            }
            */
        }

        protected struct LevelOption
        {
            public decimal Amount;
            public string CorpName;
            public string AckName;
            public bool CorpMatch;
            public bool TaxDeduct;
            public bool AnonDonation;
        }

        protected void TextBox1_TextChanged(object sender, EventArgs e)
        {
            List<char> convert = Input_Contrib.Text.ToList();
            if (convert[0] != '$')
            {
                List<char> converted = new List<char>();
                converted.Add('$');
                converted.AddRange(convert);
                Input_Contrib.Text = converted.ToString();
            }            
        }
    }
}