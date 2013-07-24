using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Nysf.Tessitura;

namespace Nysf.Apps.Utr
{
	public partial class PackageOrderForm : System.Web.UI.UserControl
	{
		private bool errorMessageJustSet = false;

		public int PackageId { get; set; }
		public int ZoneId { get; set; }
		public int PriceTypeId { get; set; }
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

				Views.SetActiveView(QuantityView);
			}
		}

		protected void Page_PreRender(object sender, EventArgs e)
		{
			if (Views.GetActiveView() == ConfirmationView)
			{
				CheckBoxIdBlurb.Text = AgreeField.ClientID;
				Page.Form.DefaultButton = NextToBillingButton.UniqueID;
			}
			else if (Views.GetActiveView() == BillingView)
			{
				Page.Form.DefaultButton = PurchaseButton.UniqueID;
				if (!errorMessageJustSet)
					BillingErrorMessage.Visible = false;
			}
		}

		protected void VerifyAgreed(object source, ServerValidateEventArgs args)
		{
			args.IsValid = AgreeField.Checked;
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

		protected void NextToConfirmation(object sender, EventArgs e)
		{
			int quantity = Int32.Parse(QuantityField.SelectedValue);
			int subtotal = quantity * 75;
			int fees = quantity * 5;
			int grandTotal = subtotal + fees;
			QuantityBlurb.Text = QuantityField.SelectedValue;
			FeesBlurb.Text = fees.ToString("C");
			SubtotalBlurb.Text = subtotal.ToString("C");
			GrandTotalBlurb.Text = grandTotal.ToString("C");
			Views.SetActiveView(ConfirmationView);
		}

		protected void BackToQuantity(object sender, EventArgs e)
		{
			Views.SetActiveView(QuantityView);
		}

		protected void NextToBilling(object sender, EventArgs e)
		{
			if (Page.IsValid)
			{
				GrandTotalReminder.Text = GrandTotalBlurb.Text;

				WebClient.EmptyCart();
				WebClient.AddPackage(PackageId, ZoneId, PriceTypeId,
					Int32.Parse(QuantityField.SelectedValue));

				if (CardTypeField.Items.Count <= 1)
				{
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
				}
				Views.SetActiveView(BillingView);
			}
		}

		protected void BackToConfirmation(object sender, EventArgs e)
		{
			Views.SetActiveView(ConfirmationView);
		}

		protected void ProcessPurchase(object sender, EventArgs e)
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

				int orderNumber = WebClient.GetCart().Id;

				CheckoutResult result = WebClient.Checkout(
					orderId: orderNumber,
					amountDue: Decimal.Parse(GrandTotalReminder.Text.Substring(1)),
					shippingMethodId: -1,
					cardHolderName: NameField.Text.Trim(),
					cardNumber: CardNumField.Text.Trim(),
                    ccTypeId: Int32.Parse(CardTypeField.SelectedValue),
                    ccExpMonth: Int32.Parse(ExpMonthField.SelectedValue),
                    ccExpYear: Int32.Parse(ExpYearField.SelectedValue),
                    ccAuthCode: AuthCodeField.Text.Trim());

				if (result == CheckoutResult.Succeeded)
				{
					WebClient.RawClient.SendOrderConfirmationEmail(
						Session[WebClient.TessSessionKeySessionKey].ToString(),
						EmailTemplateNumber,
						orderNumber,
						"");
					Views.SetActiveView(ThankYouView);
				}
				else
				{
					BillingErrorMessage.Visible = true;
					errorMessageJustSet = true;
				}
			}
		}
	}
}