using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;
using System.Web.Configuration;

namespace Nysf.TicketScanning
{
	public partial class Default : System.Web.UI.Page
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			LoginClient.MaintainSession();
			if (!IsPostBack)
			{
				PrepFormElements();
				PrepInitialViews();
			}
			ClearMessages();
		}

		protected void Page_PreRender(object sender, EventArgs e)
		{
			PrepForInput();
		}

		protected void PrepFormElements()
		{
			FacilityCollection facilities = DataClient.GetFacilities();
			foreach (Facility facility in facilities)
			{
				ListItem newItem = new ListItem(facility.Name, facility.Id.ToString());
				FacilityField.Items.Add(newItem);
			}
		}

		protected void PrepInitialViews()
		{
			if (LoginClient.GetUser() == null)
			{
				ControlViews.SetActiveView(LoginView);
				LogoutSection.Visible = false;
			}
			else
			{
				ControlViews.SetActiveView(ScanView);
				LogoutSection.Visible = true;
				int chosenFacility = LoginClient.GetFacilityId();
				bool checkDatesMode = LoginClient.GetCheckDatesMode();
				FacilityField.SelectedItem.Selected = false;
				FacilityField.Items.FindByValue(chosenFacility.ToString()).Selected = true;
				CheckDatesField.Checked = checkDatesMode;
			}
			ResultViews.Visible = false;
		}

		protected void PrepForInput()
		{
			if (ControlViews.GetActiveView() == LoginView)
			{
				Form.DefaultButton = LoginButton.UniqueID;
				FacilityField.Focus();
			}
			else
			{
				Form.DefaultButton = CheckTicketButton.UniqueID;
			}
		}

		protected void ClearMessages()
		{
			LoginFailMessage.Visible = false;
			ExpiredMessage.Visible = false;
		}

		protected void DoLogIn(object sender, EventArgs e)
		{
			if (!IsValid)
				return;
			if (LoginClient.Login(UsernameField.Text.Trim(), PasswordField.Text.Trim(),
				Int32.Parse(FacilityField.SelectedValue), CheckDatesField.Checked))
			{
				ControlViews.SetActiveView(ScanView);
				LogoutSection.Visible = true;
			}
			else
			{
				LoginFailMessage.Visible = true;
			}
		}

		protected void DoLogOut(object sender, EventArgs e)
		{
			LoginClient.Logout();
			ControlViews.SetActiveView(LoginView);
			LogoutSection.Visible = false;
			ResultViews.Visible = false;
		}

		protected void DoCheckTicket(object sender, EventArgs e)
		{
			TicketResults results = null;
			try
			{
				results = DataClient.CheckTicket(
					ticketNum: Int32.Parse(TicketNumField.Text),
					username: LoginClient.GetUser(),
					facilityId: LoginClient.GetFacilityId(),
					checkDates: LoginClient.GetCheckDatesMode());
			}
			catch (Exception ex)
			{
				Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
			}
			if (results == null)
			{
				ResultViews.SetActiveView(ExceptionView);
			}
			else if (results.Message != null)
			{
				ResultViews.SetActiveView(ErrorView);
				TicketErrorMessage.Text = results.Message;
			}
			else
			{
				RowBlurb.Text = results.Row.ToString();
				SeatBlurb.Text = results.Seat.ToString();
				PartyNameBlurb.Text = results.PartyName;
				PartyCountBlurb.Text = results.PartyCount.ToString();
				MemberStatusBlurb.Text =
					results.PartyHasMembership ? "<strong>YES</strong>" : "no";
				PartnerStatusBlurb.Text =
					results.PartyHasPartnership ? "<strong>YES</strong>" : "no";
				OrderDateBlurb.Text = results.OrderDate.ToString();
				PerfTitleBlurb.Text = results.PerfTitle;
				PerfTimeBlurb.Text = results.PerfDate.ToString();
				PerfVenueBlurb.Text = FacilityField.SelectedItem.Text;
				ResultViews.SetActiveView(DetailsView);
			}
			ResultViews.Visible = true;
			TicketNumField.Text = "";
		}
	}
}