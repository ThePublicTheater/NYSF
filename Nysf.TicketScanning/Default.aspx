<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs"
	Inherits="Nysf.TicketScanning.Default"
%><!doctype html>
<html>
	<head>
		<title>Ticket Scanning << Public Theater Webapps</title>
		<link rel="stylesheet" href="css/reset.css" type="text/css" />
		<link rel="stylesheet" href="css/ticket_scanning.css" type="text/css" />
	</head>
	<body class="ID_Default">
		<div id="Wrapper">
			<form runat="server">
				<header>
					<h1>
						<a href="Default.aspx" title="Homepage">Ticket Scan</a>
					</h1>
				</header>
				<aside id="LogoutSection" runat="server">
					<asp:Button Text="Log out" OnClick="DoLogOut" CausesValidation="false"
						runat="server" />
				</aside>
				<div id="MainContent" class="Content">
					<asp:MultiView ID="ControlViews" runat="server">
						<asp:View ID="LoginView" runat="server">
							<div id="LoginControls" class="ControlSection">
								<p id="LoginFailMessage" class="Warning" runat="server">
									Sorry.  That username and password do not match.
								</p>
								<p id="ExpiredMessage" class="Warning" runat="server">
									Your session expired.
								</p>
								<asp:ValidationSummary CssClass="Warning" DisplayMode="BulletList"
									runat="server" />
								<div class="Field">
									<asp:Label AssociatedControlID="FacilityField"
										runat="server">Facility:</asp:Label>
									<asp:DropDownList ID="FacilityField" runat="server">
										<asp:ListItem Text="" Value="" Selected="True" />
									</asp:DropDownList>
									<asp:RequiredFieldValidator ControlToValidate="FacilityField"
										Display="None" ErrorMessage="Please choose a facility."
										runat="server" />
								</div>
								<div class="Field">
									<asp:Label AssociatedControlID="UsernameField"
										runat="server">Username:</asp:Label>
									<asp:TextBox ID="UsernameField" MaxLength="8" runat="server" />
									<asp:RequiredFieldValidator ControlToValidate="UsernameField"
										Display="None" ErrorMessage="Please enter a username."
										runat="server" />
								</div>
								<div class="Field">
									<asp:Label AssociatedControlID="PasswordField"
										runat="server">Password:</asp:Label>
									<asp:TextBox ID="PasswordField" MaxLength="20"
										TextMode="Password" runat="server" />
									<asp:RequiredFieldValidator ControlToValidate="PasswordField"
										Display="None" ErrorMessage="Please enter a password."
										runat="server" />
								</div>
								<div class="Field">
									<asp:Label AssociatedControlID="CheckDatesField"
										runat="server">Verify dates/times:</asp:Label>
									<asp:CheckBox ID="CheckDatesField" Checked="true"
										runat="server" />
								</div>
								<div class="ButtonSet">
									<asp:Button ID="LoginButton" Text="Log in" OnClick="DoLogIn"
										runat="server" />
								</div>
							</div>
						</asp:View>
						<asp:View ID="ScanView" runat="server">
							<p id="ScannerStatusMsg" class="Prompt">Ready to scan.</p>
							<div id="ScanControls" class="ControlSection">
								<div class="Field">
									<asp:Label AssociatedControlID="TicketNumField"
										runat="server">Ticket number:</asp:Label>
									<asp:TextBox ID="TicketNumField" runat="server" />
									<asp:RequiredFieldValidator ControlToValidate="TicketNumField"
										Display="None" runat="server" />
								</div>
								<div class="ButtonSet">
									<asp:Button ID="CheckTicketButton" Text="Check ticket"
										OnClick="DoCheckTicket" runat="server" />
								</div>
							</div>
						</asp:View>
					</asp:MultiView>
					<asp:MultiView ID="ResultViews" runat="server">
						<%-- TODO: make views for every scan result --%>
						<asp:View ID="ErrorView" runat="server">
							<p class="Warning">
								<img src="media/unmanaged/images/icons/scan_invalid_ticket.png"
									alt="Invalid ticket" />
								<asp:Literal ID="TicketErrorMessage" runat="server" />
							</p>
						</asp:View>
						<asp:View ID="ExceptionView" runat="server">
							<p class="Warning">
								An error has occurred. Please try your scan again.
							</p>
						</asp:View>
						<asp:View ID="DetailsView" runat="server">
							<div id="TicketInfo">
								<div id="PartyInfo" class="InfoSection">
									<h3>Party Information</h3>
									<dl>
										<dt>Name:</dt>
										<dd><asp:Literal ID="PartyNameBlurb" runat="server" /></dd>
										<dt>Party:</dt>
										<dd><asp:Literal ID="PartyCountBlurb" runat="server" /></dd>
										<dt>Member:</dt>
										<dd><asp:Literal ID="MemberStatusBlurb" runat="server" />
											</dd>
										<dt>Partner:</dt>
										<dd><asp:Literal ID="PartnerStatusBlurb" runat="server" />
											</dd>
										<dt>Ordered:</dt>
										<dd><asp:Literal ID="OrderDateBlurb" runat="server" /></dd>
									</dl>
								</div>
								<div id="PerfInfo" class="InfoSection">
									<h3>Show Information</h3>
									<dl>
										<dt>Seat:</dt>
										<dd>row <asp:Literal ID="RowBlurb" runat="server" />,
											seat <asp:Literal ID="SeatBlurb" runat="server" /> </dd>
										<dt>Title:</dt>
										<dd><asp:Literal ID="PerfTitleBlurb" runat="server" /></dd>
										<dt>Start:</dt>
										<dd><asp:Literal ID="PerfTimeBlurb" runat="server" /></dd>
										<dt>Venue:</dt>
										<dd><asp:Literal ID="PerfVenueBlurb" runat="server" /></dd>
									</dl>
								</div>
							</div>
						</asp:View>
					</asp:MultiView>
				</div>
			</form>
		</div>
		<script src="js/jquery-1.6.4.min.js"></script>
		<script src="js/ticket_scanning.js"></script>
	</body>
</html>