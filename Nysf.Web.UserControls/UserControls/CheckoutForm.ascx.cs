using Ambitus;
using Nysf.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Nysf.Web.UserControls.UserControls
{
	public partial class CheckoutForm : Nysf.Web.UserControls.UserControl
	{
		public int EmailTemplateId { get; set; }

		protected void DoPrepStateDropDown(object sender, EventArgs e)
		{
			string countryVal = Input_Country.SelectedValue;
			if (String.IsNullOrWhiteSpace(countryVal))
			{
				PrepStateDropDown(null);
			}
			else
			{
				PrepStateDropDown(Int32.Parse(countryVal));
			}
		}

		protected void PrepStateDropDown(int? countryId)
		{
			int length = Input_State.Items.Count;
			for (int i = length-1; i > 0; i--)
			{
				Input_State.Items.RemoveAt(i);
			}
			if (!countryId.HasValue)
			{
				Field_State.Visible = false;
				return;
			}
			NysfSession session = BrowserUtility.GetSession();
			StateProvinceCollection states = session.GetStateProvinces(countryId.Value);
			if (states == null)
			{
				Field_State.Visible = false;
				return;
			}
			Field_State.Visible = true;
			foreach (StateProvince state in states)
			{
				Input_State.Items.Add(new ListItem(state.Name, state.Abbreviation));
			}
		}

		protected void Page_Load(object sender, EventArgs e)
		{
			NysfSession session = BrowserUtility.GetSession();
			if (!session.CartExists)
			{
				BrowserUtility.RedirectToLastPage();
			}
			if (!Page.IsPostBack)
			{
				Cart cart = session.GetCart();
				Display_GrandTotal.Text = cart.BalanceToCharge.Value.ToString("C");
				// Build the card type selector
				PaymentMethodCollection paymentMethods = session.GetPaymentMethods();
				foreach (PaymentMethod method in paymentMethods)
				{
					if (method.PaymentTypeId == 1)
					{
						Input_CardType.Items.Add(
								new ListItem(method.AccountType, method.Id.ToString()));
					}
				}

				// Build the years selector

				for (int i = 0; i < 20; i++)
				{
					Input_ExpYear.Items.Add((DateTime.Now.Year + i).ToString());
				}

				// Determine if mail delivery option should be available

				bool foundPerfSoon = false;
				// TODO: check if cart has performances within shipping window
				//ModeOfSaleRules rules = session.GetModeOfSaleRules();
				/*foreach (cart.Line in cart.SeatGroups)
				{
					if (seatGroup.Performance.StartTime < DateTime.Now.AddDays(14))
					{
						foundPerfSoon = true;
						break;
					}
				}*/
				if (session.CartExpireTime == null)
				{
					Field_ShippingMethod.Visible = false;
				}
				else if (foundPerfSoon)
				{
					Input_ShippingMethod.Enabled = false;
				}
				else
				{
					Input_ShippingMethod.Items.Add(new ListItem("Mail", "1"));
				}

				// Build the Tess-dependend UI elements

				CountryCollection countries = session.GetCountries();
				foreach (Country country in countries)
				{
					Input_Country.Items.Add(new ListItem(country.Name, country.Id.ToString()));
				}

				// Get the current account information

				AccountInfo info = session.GetAccountInfo();

				// Fill out the UI with the current account information

				PrepStateDropDown(info.CountryId);

				Display_Name.Text = (info.FirstName ?? String.Empty) + " "
						+ (info.MiddleName ?? String.Empty) + " "
						+ (info.LastName ?? String.Empty);
				Display_Email.Text = info.Email ?? String.Empty;
				Input_Country.SelectedValue = info.CountryId.ToString();
				Input_StreetAddress.Text = info.StreetAddress ?? String.Empty;
				Input_SubStreet.Text = info.SubStreetAddress ?? String.Empty;
				Input_City.Text = info.City ?? String.Empty;
				if (info.State != null)
				{
					Input_State.SelectedValue = info.State;
				}
				Input_PostalCode.Text = info.PostalCode ?? String.Empty;
				Input_Phone.Text = info.Phone ?? String.Empty;
			}
		}

		protected void DoMakePurchase(object sender, EventArgs e)
		{
			if (Page.IsValid)
			{
				if (!VerifyExpDateInFuture())
				{
					InsertValidationErrorMessage("Please choose an expiraton in the future.");
					return;
				}
				AccountInfoUpdate update = new AccountInfoUpdate();
				update.StreetAddress.Value = Input_StreetAddress.Text.Trim();
				update.SubStreetAddress.Value = Input_SubStreet.Text.Trim();
				update.City.Value = Input_City.Text.Trim();
				update.State = Input_State.SelectedValue;
				update.PostalCode.Value = Input_PostalCode.Text.Trim();
				update.Phone = Input_Phone.Text.Trim();
				NysfSession session = BrowserUtility.GetSession();
				session.UpdateAccountInfo(update);
				Cart cart = session.GetCart();
				if (session.CartExpireTime.HasValue && session.CartExpireTime < DateTime.Now)
				{
					Response.Redirect(ConfigSection.Settings.StandardPages.Expired);
				}
				CheckoutResult result = session.Checkout(
					cart.BalanceToCharge.Value, Int32.Parse(Input_ShippingMethod.SelectedValue),
					Input_CardHolder.Text, Input_CardNum.Text,
					Int32.Parse(Input_CardType.SelectedValue),
					Int32.Parse(Input_ExpMonth.SelectedValue),
					Int32.Parse(Input_ExpYear.SelectedValue),
					Input_SecurityCode.Text.Trim());
				if (result == CheckoutResult.Succeeded)
				{
					session.RecordFinalizedContributions(cart.Contributions);
					session.SendOrderConfirmationEmail(cart.OrderId.Value, EmailTemplateId);
					Response.Redirect(ConfigSection.Settings.StandardPages.PostCheckout);
				}
				else
				{
					InsertValidationErrorMessage(
							"Your credit card was not accepted. Please check your information and try again.");
				}
			}
		}

		protected bool VerifyExpDateInFuture()
		{
			int inputYear = Input_ExpYear.SelectedValue == "" ?
					0 : Int32.Parse(Input_ExpYear.SelectedValue);
			int inputMonth = Input_ExpMonth.SelectedValue == "" ?
					0 : Int32.Parse(Input_ExpMonth.SelectedValue);
			return ((inputYear == DateTime.Now.Year && inputMonth >= DateTime.Now.Month)
					|| inputYear > DateTime.Now.Year);
		}
	}
}