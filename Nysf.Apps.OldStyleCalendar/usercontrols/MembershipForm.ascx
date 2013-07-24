<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MembershipForm.ascx.cs"
	Inherits="Nysf.Apps.OldStyleTickets.usercontrols.MembershipForm"
%><script>
	  function ValidateCheckBox(sender, args)
	  {
		  if (document.getElementById(
			  "<asp:Literal ID="CheckBoxIdBlurb" runat="server" />").checked)
		  {
			  args.IsValid = true;
		  }
		  else
		  {
			  args.IsValid = false;
		  }
	  }
</script>
<asp:MultiView ID="Views" ActiveViewIndex="0" runat="server">
	<asp:View ID="SelectionView" runat="server">
		<h3>Renewing Members</h3>
		<p class="Prompt">Please call your Member Hotline to renew your membership now.</p>
		<h3>New Members</h3>
		<p class="Prompt">
			<strong>Step 1</strong> - Select the number of memberships you'd like to order.
		</p>
		<asp:ValidationSummary CssClass="Warning WarningGroup" DisplayMode="List" runat="server" />
		<fieldset class="clearfix">
			<dl>
				<dt>
					<asp:Label AssociatedControlID="QuantityInput" runat="server">Number of
						memberships that I would like to order:</asp:Label>
				</dt>
				<dd>
					<asp:DropDownList ID="QuantityInput" runat="server">
						<asp:ListItem Value="" Text="--Quantity--" Selected="True" />
						<asp:ListItem Value="1" Text="1" />
						<asp:ListItem Value="2" Text="2" />
						<asp:ListItem Value="3" Text="3" />
						<asp:ListItem Value="4" Text="4" />
						<asp:ListItem Value="5" Text="5" />
						<asp:ListItem Value="6" Text="6" />
						<asp:ListItem Value="7" Text="7" />
						<asp:ListItem Value="8" Text="8" />
						<asp:ListItem Value="9" Text="9" />
						<asp:ListItem Value="10" Text="10" />
					</asp:DropDownList>
					<asp:RequiredFieldValidator ControlToValidate="QuantityInput" Display="Dynamic"
						CssClass="Warning" Text="*" ErrorMessage="Please select the number of
						memberships that you would like to purchase." runat="server" />
					<div class="FieldSubtext">
						$55 each (fully tax deductible)
					</div>
				</dd>
				<dt>
					<asp:Label AssociatedControlID="AdditionalDonationInput" runat="server">I would
						like to include an additional tax deductible contribution of:</asp:Label>
				</dt>
				<dd>
					$<asp:TextBox ID="AdditionalDonationInput" CssClass="FieldScaleX4"
						runat="server" />
					<asp:RegularExpressionValidator ControlToValidate="AdditionalDonationInput"
						Display="Dynamic" CssClass="Warning" Text="*" runat="server"
						ValidationExpression="^\$?(\d{1,3}(\,\d{3})*|(\d+))(\.\d{2})?$"
						ErrorMessage="Please enter a valid dollar amount for your additional
						contribution." />
				</dd>
				<dt>
					<asp:Label AssociatedControlID="AutoRenewalInput" runat="server">Check this box if you would like us to automatically renew your Membership when you are about to expire.</asp:Label>
				</dt>
				<dd>
					<asp:CheckBox ID="AutoRenewalInput" runat="server" />
				</dd>
			</dl>
		</fieldset>
		<div class="InputRow">
			<asp:Button Text="Continue" OnClick="ContinueToVerification" runat="server" />
		</div>
	</asp:View>
	<asp:View ID="VerificationView" runat="server">
		<p class="Prompt">
			<strong>Step 2</strong> - Verify the following.
		</p>
		<asp:ValidationSummary CssClass="Warning WarningGroup" DisplayMode="List" runat="server" />
		<dl class="Review clearfix">
			<dt>
				Number of memberships:
			</dt>
			<dd>
				<asp:Literal ID="NumOfMembershipsBlurb" runat="server" /> ($55 each)
			</dd>
			<dt>Additional contribution:</dt>
			<dd>
				<asp:Literal ID="ContributionBlurb" runat="server" />
			</dd>
			<dt>
				Subtotal:
			</dt>
			<dd>
				<asp:Literal ID="SubtotalBlurb" runat="server" />
			</dd>
			<dt>
				Handling charges:
			</dt>
			<dd>
				<asp:Literal ID="HandlingChargesBlurb" runat="server" />
			</dd>
			<dt>
				<strong>Grand total:</strong>
			</dt>
			<dd>
				<strong><asp:Literal ID="TotalBlurb" runat="server" /></strong>
			</dd>
		</dl>
		<fieldset class="clearfix">
			<legend>Terms and Conditions</legend>
			<small>All productions, announced cast and directors are subject to change and tickets
			are subject to availability. Memberships are subject to a $5 handling fee and are
			non-refundable. Members may purchase one ticket per show for every Membership they
			hold. The exclusive Member discount is valid for all regular Public Theater Membership
			shows and excludes rentals and other special events, such as benefits. Ticket exchanges
			may be made up to 48 hours prior to scheduled performance date and are subject to
			availability. Members receive $25 off one Summer
			Supporter seat per Membership while Membership is valid. Memberships are valid for one
			year from the date of purchase.</small>
			<div class="InputRow">
				<asp:CustomValidator Text="*"
					OnServerValidate="VerifyAgreed" CssClass="Warning" Display="Dynamic"
					ErrorMessage="Please check the box to indicate that you agree to the terms and
					conditions." runat="server" ClientValidationFunction="ValidateCheckBox" />
				<asp:CheckBox ID="AgreeInput" runat="server" />
				<asp:Label AssociatedControlID="AgreeInput" runat="server">I have read and agree to
				the terms and conditions.</asp:Label>
			</div>
		</fieldset>
		<div class="InputRow">
			<asp:Button Text="Continue" OnClick="ContinueToCheckout" runat="server" />
			<asp:Button Text="Back" OnClick="BackToSelection" CausesValidation="false"
				runat="server" />
		</div>
	</asp:View>
	<asp:View ID="CheckoutView" runat="server">
		<p class="Prompt">
			<strong>Step 3</strong> - Enter your payment and contact information.
		</p>
		<p id="BillingErrorMessage" class="Warning" visible="false" runat="server">
			Sorry.  Your credit card could not be processed.  Please double-check your information
			below and try again.
		</p>
		<asp:ValidationSummary CssClass="Warning" DisplayMode="BulletList" runat="server" />
		<p class="Prompt">
			Your grand total is
			<strong><asp:Literal ID="GrandTotalBlurb" runat="server" /></strong>.
		</p>
		<fieldset>
			<legend class="SelfEvident">Billing and Delivery Info</legend>
			<dl>
				<dt>
					Name:
				</dt>
				<dd>
					<asp:Literal ID="NameBlurb" runat="server" />
				</dd>
				<dt>
					Email address:
				</dt>
				<dd>
					<asp:Literal ID="EmailBlurb" runat="server" />
				</dd>
				<dt>
					<asp:Label AssociatedControlID="CountryField"
						runat="server">Country:</asp:Label>
				</dt>
				<dd>
					<asp:DropDownList ID="CountryField" AutoPostBack="true" CausesValidation="false"
							OnSelectedIndexChanged="CountryChanged" runat="server">
						<asp:ListItem Selected="True" Value="" Text="" />
					</asp:DropDownList><%-- TODO: add non-JS alternative --%>
					<asp:RequiredFieldValidator
						ControlToValidate="CountryField" Display="Dynamic" Text="*" runat="server"
						CssClass="Warning" ErrorMessage="Please select a country." />
						<%-- TODO: make sure works --%>
				</dd>
				<dt>
					<asp:Label AssociatedControlID="AddressField" runat="server">Street
						address:</asp:Label>
				</dt>
				<dd>
					<asp:TextBox ID="AddressField" MaxLength="55" runat="server"
						CssClass="FieldScaleX20" />
					<asp:RequiredFieldValidator ID="RequiredFieldValidator8"
						ControlToValidate="AddressField" Display="Dynamic" Text="*" runat="server"
						CssClass="Warning" ErrorMessage="Please enter a street address." />
					<div class="FormFieldExample">Example: 123 Papp St Apt 456</div>
				</dd>
				<dt>
					<asp:Label AssociatedControlID="SubAddressField"
						runat="server">Sub-address:</asp:Label>
				</dt>
				<dd>
					<asp:TextBox ID="SubAddressField" MaxLength="55" runat="server"
						CssClass="FieldScaleX20" />
					<div class="FormFieldExample">Company name, department name, c/o</div>
				</dd>
				<dt>
					<asp:Label AssociatedControlID="CityField" runat="server">City:</asp:Label>
				</dt>
				<dd>
					<asp:TextBox ID="CityField" MaxLength="30" runat="server"
						CssClass="FieldScaleX8" />
					<asp:RequiredFieldValidator ID="RequiredFieldValidator9"
						ControlToValidate="CityField" Display="Dynamic" runat="server" Text="*"
						CssClass="Warning" ErrorMessage="Please enter a city." />
				</dd>
			</dl>
			<dl id="StateFieldGroup" runat="server" visible="false">
				<dt>
					<asp:Label AssociatedControlID="StateField" runat="server">State /
						Province:</asp:Label>
				</dt>
				<dd>
					<asp:DropDownList ID="StateField" runat="server">
						<asp:ListItem Selected="True" Value="" Text="" />
					</asp:DropDownList>
					<asp:RequiredFieldValidator ID="RequiredFieldValidator10"
						ControlToValidate="StateField" Display="Dynamic" runat="server" Text="*"
						CssClass="Warning" ErrorMessage="Please select a state." />
						<%-- TODO: Make sure this doesn't trigger with foreign country --%>
				</dd>
			</dl>
			<dl>
				<dt>
					<asp:Label AssociatedControlID="ZipField" runat="server">Postal
						code:</asp:Label>
				</dt>
				<dd>
					<asp:TextBox ID="ZipField" MaxLength="10" runat="server"
						CssClass="FieldScaleX4" />
					<asp:RequiredFieldValidator ID="RequiredFieldValidator11"
						ControlToValidate="ZipField" Display="Dynamic" Text="8" CssClass="Warning"
						runat="server" ErrorMessage="Please enter a postal code." />
				</dd>
				<dt>
					<asp:Label AssociatedControlID="AddressTypeField" runat="server">Address
						type:</asp:Label>
				</dt>
				<dd>
					<asp:DropDownList ID="AddressTypeField" runat="server">
						<asp:ListItem Selected="True" Value="" Text="" />
						<asp:ListItem Value="3" Text="Home" />
						<asp:ListItem Value="2" Text="Business" />
						<asp:ListItem Value="13" Text="Other" />
					</asp:DropDownList>
					<asp:RequiredFieldValidator ID="RequiredFieldValidator12"
						ControlToValidate="AddressTypeField" Display="Dynamic" Text="*"
						CssClass="Warning" runat="server"
						ErrorMessage="Please select an address type." />
				</dd>
			</dl>
			<dl>
				<dt>
					<asp:Label AssociatedControlID="PhoneField" runat="server">Phone
						number:</asp:Label>
				</dt>
				<dd>
					<asp:TextBox ID="PhoneField" MaxLength="32" runat="server" />
				</dd>
			</dl>
		</fieldset>
		<fieldset>
			<legend class="SelfEvident">Credit Card</legend>
			<dl>
				<dt>
					<asp:Label AssociatedControlID="CardTypeField" runat="server">
						Credit card type:
					</asp:Label>
				</dt>
				<dd>
					<asp:DropDownList ID="CardTypeField" runat="server">
						<asp:ListItem Text="" Value="" />
					</asp:DropDownList>
					<asp:RequiredFieldValidator ID="RequiredFieldValidator2"
						ControlToValidate="CardTypeField" Display="Dynamic" Text="*" runat="server"
						CssClass="Warning" ErrorMessage="Please select your card type." />
					<%-- TODO: Make sure initial selection (value 0) triggers invalidation --%>
				</dd>
				<dt>
					<asp:Label AssociatedControlID="CardNumField" runat="server">
						Credit card number:
					</asp:Label>
				</dt>
				<dd>
					<asp:TextBox ID="CardNumField" MaxLength="16" autocomplete="off" runat="server" />
					<asp:RequiredFieldValidator ID="RequiredFieldValidator3"
						ControlToValidate="CardNumField" Display="Dynamic" Text="*" runat="server"
						CssClass="Warning" ErrorMessage="Please enter your credit card number." />
					<asp:RegularExpressionValidator ID="RegularExpressionValidator1"
						ControlToValidate="CardNumField" Display="Dynamic" Text="*" runat="server"
						CssClass="Warning"
						ErrorMessage="Please enter only digits for your credit card number."
						ValidationExpression="^[0-9]+$" />
					<div class="FormFieldExample">(only numbers, no spaces or dashes please)</div>
				</dd>
				<dt>
					<asp:Label AssociatedControlID="NameField" runat="server">
						Name on credit card:
					</asp:Label>
				</dt>
				<dd>
					<asp:TextBox ID="NameField" MaxLength="97" runat="server" />
					<asp:RequiredFieldValidator ID="RequiredFieldValidator4"
						ControlToValidate="NameField" Display="Dynamic" Text="*" runat="server"
						CssClass="Warning" ErrorMessage="Please enter the cardholder name." />
				</dd>
				<dt>
					<asp:Label AssociatedControlID="ExpMonthField" runat="server">
						Expiration:
					</asp:Label>
				</dt>
				<dd>
					<asp:DropDownList ID="ExpMonthField" runat="server">
						<asp:ListItem Text="" Value="" Selected="True" />
						<asp:ListItem Text="Jan" Value="1" />
						<asp:ListItem Text="Feb" Value="2" />
						<asp:ListItem Text="Mar" Value="3" />
						<asp:ListItem Text="Apr" Value="4" />
						<asp:ListItem Text="May" Value="5" />
						<asp:ListItem Text="Jun" Value="6" />
						<asp:ListItem Text="Jul" Value="7" />
						<asp:ListItem Text="Aug" Value="8" />
						<asp:ListItem Text="Sep" Value="9" />
						<asp:ListItem Text="Oct" Value="10" />
						<asp:ListItem Text="Nov" Value="11" />
						<asp:ListItem Text="Dec" Value="12" />
					</asp:DropDownList><%-- TODO: make sure date is after today --%>
					<asp:DropDownList ID="ExpYearField" runat="server">
						<asp:ListItem Text="" Value="" Selected="True" />
					</asp:DropDownList><%-- TODO: add years in pageload --%>
					<asp:RequiredFieldValidator ID="RequiredFieldValidator5"
						ControlToValidate="ExpMonthField" Display="Dynamic" Text="*"
						CssClass="Warning" runat="server"
						ErrorMessage="Please select an expiration month." />
					<asp:RequiredFieldValidator ID="RequiredFieldValidator6"
						ControlToValidate="ExpYearField" Display="Dynamic" Text="*"
						CssClass="Warning" runat="server"
						ErrorMessage="Please select an expiration year." />
					<asp:CustomValidator ID="CustomValidator1" ControlToValidate="ExpYearField"
						runat="server" Display="Dynamic" Text="*" CssClass="Warning"
						ErrorMessage="Please enter an expiration date in the future."
						onservervalidate="VerifyExpDateInFuture" />
				</dd>
				<dt>
					<asp:Label AssociatedControlID="AuthCodeField" runat="server">
						CVV / CID number:
					</asp:Label>
				</dt>
				<dd>
					<asp:TextBox ID="AuthCodeField" MaxLength="4" autocomplete="off" runat="server" />
					<asp:RequiredFieldValidator
						ControlToValidate="AuthCodeField" Display="Dynamic" Text="*" runat="server"
						CssClass="Warning" ErrorMessage="Please enter the CVV / CID number." />
					<asp:RegularExpressionValidator ID="RegularExpressionValidator2"
						ControlToValidate="AuthCodeField" Display="Dynamic" Text="*" runat="server"
						CssClass="Warning" ErrorMessage="Please enter a 3-4 digit CVV / CID number."
						ValidationExpression="^[0-9]{3,4}$" />
				</dd><%-- TODO: make sure this is a 3-4 digit number, validator works --%>
			</dl>
			<div class="SecuritySeal">
				<h4 class="visuallyhidden">Security Seal</h4>
				<table width="135" border="0" cellpadding="1" cellspacing="1"
					style="display:inline">
					<tr>
						<td width="135" align="center">
							<script src="https://sealserver.trustkeeper.net/compliance/seal_js.php?code=x4ij3BrNflKuByuB1cTROdOFOTLL5E&style=normal&size=105x54&language=en">
							</script>
							<noscript>
								<a href="https://sealserver.trustkeeper.net/compliance/cert.php?code=x4ij3BrNflKuByuB1cTROdOFOTLL5E&style=normal&size=105x54&language=en" target="hATW"><img src="https://sealserver.trustkeeper.net/compliance/seal.php?code=x4ij3BrNflKuByuB1cTROdOFOTLL5E&style=normal&size=105x54&language=en" border="0" alt="Trusted Commerce"/>
								</a>
							</noscript>
						</td>
					</tr>
				</table>
			</div>
		</fieldset>
		<div class="InputRow">
			<asp:Button OnClick="ProcessPurchase" runat="server" Text="Confirm purchase" />
			<asp:Button OnClick="BackToSelection" runat="server" CausesValidation="false" Text="Back" />
		</div>
	</asp:View>
</asp:MultiView>