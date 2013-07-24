using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Nysf.Tessitura;
using Nysf.Types;
using System.Data;

namespace Nysf.Apps.OldStyleTickets
{
    public partial class CheckoutWizard : GenericControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (!WebClient.IsLoggedIn())
                {
                    Utility.SetReferer(ViewState);
                    Cart cart = WebClient.GetCart();
                    if (cart == null || !cart.HasItems)
                        CheckoutViews.SetActiveView(EmptyCartView);
                    else
                    {
                        // Build the UI

                        GrandTotalBlurb.Text = cart.Total.ToString("C");
                        OrderNumber.Value = cart.Id.ToString();
                        AmountDue.Value = cart.Total.ToString();

                        List<Organization> orgs = new List<Organization>();
                        foreach (CartSeatGroupItem seatGroup in cart.SeatGroups)
                        {
                            if (!orgs.Contains(seatGroup.Performance.Organization))
                                orgs.Add(seatGroup.Performance.Organization);
                        }
                        StringBuilder orgsText = new StringBuilder();
                        if (orgs.Count > 0)
                            orgsText.Append(orgs[0].ToString());
                        for (int c = 1; c < orgs.Count; c++)
                        {
                            orgsText.Append("," + orgs[c].ToString());
                        }
                        CartOrgs.Value = orgsText.ToString();

                        // Build the card type selector
                        Dictionary<int, string> cardTypes = WebClient.GetCreditCardTypes();
                        foreach (int key in cardTypes.Keys)
                        {
                            CardTypeField.Items.Add(new ListItem(cardTypes[key], key.ToString()));
                        }

                        // Build the years selector

                        for (int i = 0; i < 20; i++)
                        {
                            ExpYearField.Items.Add((DateTime.Now.Year + i).ToString());
                        }

                        // Determine if mail delivery option should be available

                        bool foundPerfSoon = false;
                        foreach (CartSeatGroupItem seatGroup in cart.SeatGroups)
                        {
                            if (seatGroup.Performance.StartTime < DateTime.Now.AddDays(14))
                            {
                                foundPerfSoon = true;
                                break;
                            }
                        }
                        if (!foundPerfSoon)
                            ShippingMethodField.Items.Add(new ListItem("Mail", "1"));

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


                        // Fill out the UI with the current account information

                            EmailBlurb.Visible = false;
                            NameBlurb.Visible = false;
                        CheckoutViews.SetActiveView(BillingEntryView);
                    }

                }
                else
                {
                    Utility.SetReferer(ViewState);
                    Cart cart = WebClient.GetCart();
                    if (cart == null || !cart.HasItems)
                        CheckoutViews.SetActiveView(EmptyCartView);
                    else
                    {
                        // Build the UI

                        GrandTotalBlurb.Text = cart.Total.ToString("C");
                        OrderNumber.Value = cart.Id.ToString();
                        AmountDue.Value = cart.Total.ToString();

                        List<Organization> orgs = new List<Organization>();
                        foreach (CartSeatGroupItem seatGroup in cart.SeatGroups)
                        {
                            if (!orgs.Contains(seatGroup.Performance.Organization))
                                orgs.Add(seatGroup.Performance.Organization);
                        }
                        StringBuilder orgsText = new StringBuilder();
                        if (orgs.Count > 0)
                            orgsText.Append(orgs[0].ToString());
                        for (int c = 1; c < orgs.Count; c++)
                        {
                            orgsText.Append("," + orgs[c].ToString());
                        }
                        CartOrgs.Value = orgsText.ToString();

                        // Build the card type selector
                        Dictionary<int, string> cardTypes = WebClient.GetCreditCardTypes();
                        foreach (int key in cardTypes.Keys)
                        {
                            CardTypeField.Items.Add(new ListItem(cardTypes[key], key.ToString()));
                        }

                        // Build the years selector

                        for (int i = 0; i < 20; i++)
                        {
                            ExpYearField.Items.Add((DateTime.Now.Year + i).ToString());
                        }

                        // Determine if mail delivery option should be available

                        bool foundPerfSoon = false;
                        foreach (CartSeatGroupItem seatGroup in cart.SeatGroups)
                        {
                            if (seatGroup.Performance.StartTime < DateTime.Now.AddDays(14))
                            {
                                foundPerfSoon = true;
                                break;
                            }
                        }
                        if (!foundPerfSoon)
                            ShippingMethodField.Items.Add(new ListItem("Mail", "1"));

                        // Build the Tess-dependend UI elements

                       // DataSet countriesWithStates = WebClient.GetCountries(true);
                       // DataSet countriesWithoutStates = WebClient.GetCountries(false);
                      //  CountryField.DataSource = countriesWithStates.Tables["Country"];
                        DataSet countries = WebClient.GetCountries();
                        CountryField.DataSource = countries.Tables["Country"];
                        CountryField.DataValueField = "id";
                        CountryField.DataTextField = "description";
                        CountryField.AppendDataBoundItems = true;
                        CountryField.DataBind();
                        CountryField.SelectedValue = "1";

                      //  CountryField.DataSource = countriesWithoutStates.Tables["Country"];
                       // CountryField.DataBind();

                        // Get the current account information

                        AccountInfo accountInfo = WebClient.GetAccountInformation();

                        // Fill out the UI with the current account information
                        EmailBlurb.Visible = false;
                        NameBlurb.Visible = false;

                        AccountPerson person = accountInfo.People.Person1;
                        if (!String.IsNullOrEmpty(person.FirstName))
                            NameField.Text = person.FirstName;
                        if (!String.IsNullOrEmpty(person.MiddleName))
                            NameField.Text += " " + person.MiddleName;
                        if (!String.IsNullOrEmpty(person.LastName))
                            NameField.Text += " " + person.LastName;
                        TextBox111.Text = NameField.Text;
                        if (!String.IsNullOrEmpty(accountInfo.Email))
                            TextBox122.Text = accountInfo.Email;
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
                        CheckoutViews.SetActiveView(BillingEntryView);
                    }
                }
            }
			BillingErrorMessage.Visible = false;
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

            // Show or hide the address fields, "mail" shipping option based on whether states were
            // found

            if (StateField.Items.Count > 1)
            {
                if (ShippingMethodField.Items.Count == 3)
                    ShippingMethodField.Items[2].Enabled = true;
                StateFieldGroup.Visible = true;
            }
            else
            {
                if (ShippingMethodField.Items.Count == 3)
                {
                    ShippingMethodField.SelectedItem.Selected = false;
                    ShippingMethodField.Items[0].Selected = true;
                    ShippingMethodField.Items[2].Enabled = false;
                }
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

        protected void ProcessReservation(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
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

                CheckoutResult result = WebClient.Checkout(Int32.Parse(OrderNumber.Value),
                    Decimal.Parse(AmountDue.Value), Int32.Parse(ShippingMethodField.SelectedValue),
                    NameField.Text.Trim(), CardNumField.Text.Trim(),
                    Int32.Parse(CardTypeField.SelectedValue),
                    Int32.Parse(ExpMonthField.SelectedValue),
                    Int32.Parse(ExpYearField.SelectedValue),
                    AuthCodeField.Text.Trim());

				if (result == CheckoutResult.Succeeded)
				{
					try
					{
						WebClient.SendOrderConfirmationEmail(Int32.Parse(OrderNumber.Value),
							CartOrgs.Value);
					}
					catch (Exception) { }
					CheckoutViews.SetActiveView(SuccessView);
				}
				else
				{
					BillingErrorMessage.Visible = true;
				}
            }

            
        }

        protected void RedirectToReferer(object sender, EventArgs e)
        {
            Response.Redirect(Utility.GetReferer(ViewState));
        }
    }
}