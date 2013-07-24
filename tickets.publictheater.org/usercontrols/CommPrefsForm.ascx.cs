using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Nysf.Tessitura;

namespace Nysf.UserControls
{
    public partial class CommPrefsForm : GenericControl
    {
		public bool ShowEmailSubPrefs { get; set; }

		public CommPrefsForm()
		{
			ShowEmailSubPrefs = true;
		}

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                Utility.SetReferer(ViewState);
				if (ShowEmailSubPrefs)
				{
					Nysf.Tessitura.AttributeCollection sysAtts = WebClient.GetAttributes();
					ConstituentAttributeCollection acctAtts = WebClient.GetConstituentAttributes();
					/*
						Disabled organizational subpreferences:
					 
					Dictionary<Nysf.Tessitura.Attribute, bool> ptAtts =
						new Dictionary<Nysf.Tessitura.Attribute, bool>();
					Dictionary<Nysf.Tessitura.Attribute, bool> jpAtts =
						new Dictionary<Nysf.Tessitura.Attribute, bool>();
					Dictionary<Nysf.Tessitura.Attribute, bool> sitpAtts =
						new Dictionary<Nysf.Tessitura.Attribute, bool>();
					foreach (Nysf.Tessitura.Attribute att in sysAtts)
					{
						if (att.Name.Contains("cp_Em_PT_"))
							ptAtts.Add(att, acctAtts.HasAttributeValuePair(att.Id, 1));
						else if (att.Name.Contains("cp_Em_JP_"))
							jpAtts.Add(att, acctAtts.HasAttributeValuePair(att.Id, 1));
						else if (att.Name.Contains("cp_Em_SP_"))
							sitpAtts.Add(att, acctAtts.HasAttributeValuePair(att.Id, 1));
					}
					AddAttributesToCheckBoxList(ptAtts, PtSubCheckBoxes);
					AddAttributesToCheckBoxList(jpAtts, JpSubCheckBoxes);
					AddAttributesToCheckBoxList(sitpAtts, SitpSubCheckBoxes);*/
					Nysf.Tessitura.Attribute PtOptInAtt =
						sysAtts.GetAttributeByName("cp_Em_The Public Theater");
					Nysf.Tessitura.Attribute JpOptInAtt =
						sysAtts.GetAttributeByName("cp_Em_Joe’s Pub");
					Nysf.Tessitura.Attribute SitpOptInAtt =
						sysAtts.GetAttributeByName("cp_Em_Shakespeare in the Park");
					PtCheckBox.Checked = acctAtts.HasAttributeValuePair(PtOptInAtt.Id, 1);
					JpCheckBox.Checked = acctAtts.HasAttributeValuePair(JpOptInAtt.Id, 1);
					SitpCheckBox.Checked = acctAtts.HasAttributeValuePair(SitpOptInAtt.Id, 1);
				}
                AccountInfo acctInfo = WebClient.GetAccountInformation();
                WantPhoneField.Checked = acctInfo.WantsPhone ?? true;
                WantMailField.Checked = acctInfo.WantsMail ?? true;
                WantEmailField.Checked = acctInfo.WantsEmail ?? true;
            }
			if (ShowEmailSubPrefs)
			{
				if (WantEmailField.Checked)
				{
					EmailTopicsFieldset.Visible = true;
				}
				else
				{
					PtCheckBox.Checked = false;
					JpCheckBox.Checked = false;
					SitpCheckBox.Checked = false;
					EmailTopicsFieldset.Visible = false;
				}
				if (PtCheckBox.Checked)
				{
					PtSubCheckBoxes.Visible = true;
				}
				else
				{
					foreach (ListItem item in PtSubCheckBoxes.Items)
					{
						item.Selected = false;
					}
					PtSubCheckBoxes.Visible = false;
				}
				if (JpCheckBox.Checked)
				{
					JpSubCheckBoxes.Visible = true;
				}
				else
				{
					foreach (ListItem item in JpSubCheckBoxes.Items)
					{
						item.Selected = false;
					}
					JpSubCheckBoxes.Visible = false;
				}
				if (SitpCheckBox.Checked)
				{
					SitpSubCheckBoxes.Visible = true;
				}
				else
				{
					foreach (ListItem item in SitpSubCheckBoxes.Items)
					{
						item.Selected = false;
					}
					SitpSubCheckBoxes.Visible = false;
				}
			}
			else
			{
				EmailTopicsFieldset.Visible = false;
				WantEmailField.AutoPostBack = false;
			}
            SuccessMessage.Visible = false;
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            Page.Form.DefaultButton = SubmitButton.UniqueID;
        }

        private void AddAttributesToCheckBoxList(Dictionary<Nysf.Tessitura.Attribute,bool> atts,
            CheckBoxList checkBoxes)
        {
            foreach (Nysf.Tessitura.Attribute att in atts.Keys)
            {
                string text = att.Name.Split('_')[3];
                string value = att.Id.ToString();
                bool selected = atts[att];
                ListItem newItem = new ListItem(text, value);
                newItem.Selected = selected;
                checkBoxes.Items.Add(newItem);
            }
        }

        protected void GoBack(object sender, EventArgs e)
        {
            Utility.RedirectToReferer(ViewState);
        }

        protected void Submit(object sender, EventArgs e)
        {
            AccountInfo acctInfo = WebClient.GetAccountInformation();
            acctInfo.WantsPhone = WantPhoneField.Checked;
            acctInfo.WantsMail = WantMailField.Checked;
            acctInfo.WantsEmail = WantEmailField.Checked;
            WebClient.UpdateAccountInfo(acctInfo);
			if (ShowEmailSubPrefs)
			{
				Nysf.Tessitura.AttributeCollection sysAtts = WebClient.GetAttributes();
				Dictionary<int, Object> attsToUpdate = new Dictionary<int, Object>();
				attsToUpdate.Add(sysAtts.GetAttributeByName("cp_Em_The Public Theater").Id,
					PtCheckBox.Checked ? 1 : 0);
				attsToUpdate.Add(sysAtts.GetAttributeByName("cp_Em_Joe’s Pub").Id,
					JpCheckBox.Checked ? 1 : 0);
				attsToUpdate.Add(sysAtts.GetAttributeByName("cp_Em_Shakespeare in the Park").Id,
					SitpCheckBox.Checked ? 1 : 0);
				foreach (ListItem item in PtSubCheckBoxes.Items)
				{
					attsToUpdate.Add(Int32.Parse(item.Value), item.Selected ? 1 : 0);
				}
				foreach (ListItem item in JpSubCheckBoxes.Items)
				{
					attsToUpdate.Add(Int32.Parse(item.Value), item.Selected ? 1 : 0);
				}
				foreach (ListItem item in SitpSubCheckBoxes.Items)
				{
					attsToUpdate.Add(Int32.Parse(item.Value), item.Selected ? 1 : 0);
				}
				ConstituentAttributeCollection acctAtts = WebClient.GetConstituentAttributes();
				foreach (int attId in attsToUpdate.Keys)
				{
					WebClient.AddAttribute(attId, attsToUpdate[attId], true);
				}
				if (attsToUpdate.Count > 0)
				{
					SuccessMessage.Visible = true;
				}
			}
			else
				SuccessMessage.Visible = true;
        }
    }
}