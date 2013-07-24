using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Nysf.Tessitura;

namespace Nysf.Apps.OldStyleTickets.usercontrols
{
    public partial class MembershipForm : System.Web.UI.UserControl
    {
		protected const string LastCheckoutTimeSessionKey
				= "Nysf_Apps_OldStyleTickets_MemCheckoutTime";
		protected const string HasCheckedOutSessionKey = "Nysf_Apps_OldStyleTickets_MemCheckedOut";

		// TODO: require email template number
		public int EmailTemplateNumber { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
			if (!Page.IsPostBack)
			{
				if (!WebClient.IsLoggedIn())
				{
					throw new ApplicationException("The MembershipForm requires an "
						+ "authenticated Tessitura session.");
				}
				Utility.SetReferer(ViewState);
			}
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            if (Views.GetActiveView() == VerificationView)
            {
				decimal contributionAmount = 0;
                if (!String.IsNullOrWhiteSpace(AdditionalDonationInput.Text))
                {
                    contributionAmount = Decimal.Parse(
                        AdditionalDonationInput.Text.Replace("$", "").Replace(",", ""));
                }
                int numOfMemberships = Int32.Parse(QuantityInput.SelectedValue);
				decimal subtotal =
					numOfMemberships * WebClient.MembershipPrice + contributionAmount;
				decimal handlingFee =
					numOfMemberships * WebClient.MembershipHandlingFee;
                CheckBoxIdBlurb.Text = AgreeInput.ClientID;
                NumOfMembershipsBlurb.Text = QuantityInput.SelectedValue;
                ContributionBlurb.Text = contributionAmount.ToString("C");
                SubtotalBlurb.Text = subtotal.ToString("C");
                HandlingChargesBlurb.Text = handlingFee.ToString("C");
                TotalBlurb.Text = (subtotal + handlingFee).ToString("C");
				Session.Remove(HasCheckedOutSessionKey);
            }
			else if (Views.GetActiveView() == CheckoutView)
			{
                if (NameField.Text.Trim().Length==0)
                {
                    // Build the UI

                    // Build the card type selector
                    Dictionary<int, string> cardTypes = WebClient.GetMembershipCreditCardTypes();
                    foreach (int key in cardTypes.Keys)
                    {
                        CardTypeField.Items.Add(new ListItem(cardTypes[key], key.ToString()));
                    }

                    // Build the years selector

                    for (int i = 0; i < 20; i++)
                    {
                        ExpYearField.Items.Add((DateTime.Now.Year + i).ToString());
                    }

                    // Build the Tess-dependend UI elements

                    DataSet countriesWithStates = WebClient.GetCountries(true);
                    DataSet countriesWithoutStates = WebClient.GetCountries(false);
                    CountryField.DataSource = countriesWithStates.Tables["Country"];
                    CountryField.DataValueField = "id";
                    CountryField.DataTextField = "description";
                    CountryField.AppendDataBoundItems = true;
                    CountryField.DataBind();
                    CountryField.DataSource = countriesWithoutStates.Tables["Country"];
                    CountryField.DataBind();

                    // Get the current account information

                    AccountInfo accountInfo = WebClient.GetAccountInformation();

                    // Fill out the UI with the current account information

                    AccountPerson person = accountInfo.People.Person1;
                    if (!String.IsNullOrEmpty(person.FirstName))
                        NameField.Text = person.FirstName;
                    if (!String.IsNullOrEmpty(person.MiddleName))
                        NameField.Text += " " + person.MiddleName;
                    if (!String.IsNullOrEmpty(person.LastName))
                        NameField.Text += " " + person.LastName;
                    NameBlurb.Text = NameField.Text;
                    if (!String.IsNullOrEmpty(accountInfo.Email))
                        EmailBlurb.Text = accountInfo.Email;
                    if (!String.IsNullOrEmpty(accountInfo.Phone))
                        PhoneField.Text = accountInfo.Phone;
                    if (accountInfo.CountryId != null)
                    {
                        CountryField.SelectedValue = accountInfo.CountryId.ToString();
                        ConfigureAddressFields();
                    }
                    if (!String.IsNullOrEmpty(accountInfo.Address))
                        AddressField.Text = accountInfo.Address;
                    if (!String.IsNullOrEmpty(accountInfo.SubAddress))
                        SubAddressField.Text = accountInfo.SubAddress;
                    if (!String.IsNullOrEmpty(accountInfo.City))
                        CityField.Text = accountInfo.City;
                    if (!String.IsNullOrEmpty(accountInfo.StateId))
                        StateField.SelectedValue = accountInfo.StateId;
                    if (!String.IsNullOrEmpty(accountInfo.PostalCode))
                        ZipField.Text = accountInfo.PostalCode;
                    if (accountInfo.AddressTypeId != null)
                    {
                        if (accountInfo.AddressTypeId != 3 && accountInfo.AddressTypeId != 2
                            && accountInfo.AddressTypeId != 13)
                        {
                            AddressTypeField.Items.Add(new ListItem(
                                accountInfo.AddressTypeId.ToString(), accountInfo.AddressTypeDesc));
                        }
                        AddressTypeField.SelectedValue = accountInfo.AddressTypeId.ToString();
                    }
                    BillingErrorMessage.Visible = false;
                }
			}
        }

        protected void ContinueToVerification(object sender, EventArgs e)
        {
            if (Page.IsValid)
                Views.SetActiveView(VerificationView);
        }

        protected void ContinueToCheckout(object sender, EventArgs e)
        {
			if (Page.IsValid)
			{
				GrandTotalBlurb.Text = TotalBlurb.Text;
				Views.SetActiveView(CheckoutView);
			}
        }

        protected void BackToSelection(object sender, EventArgs e)
        {
            Views.SetActiveView(SelectionView);
        }

        protected void VerifyAgreed(object source, ServerValidateEventArgs args)
        {
            args.IsValid = AgreeInput.Checked;
        }

		private void ConfigureAddressFields()
		{
			// Reset and empty StateField

			StateField.SelectedIndex = 0;
			for (int i = StateField.Items.Count - 1; i > 0; i--)
				StateField.Items.RemoveAt(i);

			// Fill StateField with states of the selected country

			if (CountryField.SelectedValue != "")
			{
				DataSet states = WebClient.GetStates(int.Parse(CountryField.SelectedValue));
				StateField.DataSource = states.Tables["StateProvince"];
				StateField.DataValueField = "id";
				StateField.DataTextField = "description";
				StateField.AppendDataBoundItems = true;
				StateField.DataBind();
			}

			// Show or hide the state field

			if (StateField.Items.Count > 1)
			{
				StateFieldGroup.Visible = true;
			}
			else
			{
				StateFieldGroup.Visible = false;
			}
		}

		protected void CountryChanged(object sender, EventArgs e)
		{
			ConfigureAddressFields();
		}

		protected void VerifyExpDateInFuture(object source, ServerValidateEventArgs args)
		{
			int inputYear =
				ExpYearField.SelectedValue == "" ? 0 : Int32.Parse(ExpYearField.SelectedValue);
			int inputMonth =
				ExpMonthField.SelectedValue == "" ? 0 : Int32.Parse(ExpMonthField.SelectedValue);
			if (inputYear == DateTime.Now.Year && inputMonth < DateTime.Now.Month)
				args.IsValid = false;
			else
				args.IsValid = true;
		}

		protected void ProcessPurchase(object sender, EventArgs e)
		{
			if (Page.IsValid)
			{
				bool? hasCheckedOut = (bool?)Session[HasCheckedOutSessionKey];
				DateTime? lastCheckoutTime = (DateTime?)Session[LastCheckoutTimeSessionKey];
				if (hasCheckedOut == true || (lastCheckoutTime.HasValue
						&& DateTime.Now.AddMinutes(-5) < lastCheckoutTime.Value))
				{
					HttpContext.Current.Response.Redirect("/CheckoutBlocked.aspx");
				}

				// Save address to primary address

				AccountInfo accountInfo = WebClient.GetAccountInformation();
				accountInfo.Address = AddressField.Text.Trim();
				accountInfo.SubAddress = SubAddressField.Text.Trim();
				accountInfo.City = CityField.Text.Trim();
				accountInfo.StateId = StateField.SelectedValue;
				accountInfo.PostalCode = ZipField.Text.Trim();
				accountInfo.AddressTypeId = Int32.Parse(AddressTypeField.SelectedValue);
				accountInfo.Phone = PhoneField.Text.Trim();
				WebClient.UpdateAccountInfo(accountInfo);

				CheckoutResult result = WebClient.PurchaseMemberships(
					Int32.Parse(QuantityInput.SelectedValue),
					Decimal.Parse(ContributionBlurb.Text.Substring(1)),
					AutoRenewalInput.Checked,
					NameField.Text.Trim(), CardNumField.Text.Trim(),
					Int32.Parse(CardTypeField.SelectedValue),
					Int32.Parse(ExpMonthField.SelectedValue),
					Int32.Parse(ExpYearField.SelectedValue),
					AuthCodeField.Text.Trim(),
					"Membership Email Confirmation",
					EmailTemplateNumber);
				if (result == CheckoutResult.Succeeded)
				{
					hasCheckedOut = true;
					lastCheckoutTime = DateTime.Now;
					HttpContext.Current.Session[HasCheckedOutSessionKey] = hasCheckedOut;
					HttpContext.Current.Session[LastCheckoutTimeSessionKey] = lastCheckoutTime;
					Response.Redirect("/CheckedOut.aspx");
				}
				else
				{
					BillingErrorMessage.Visible = true;
				}
			}
		}
	}
}