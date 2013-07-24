<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PackageOrderForm.ascx.cs"
		Inherits="Nysf.Apps.Utr.PackageOrderForm" %>

<asp:ValidationSummary CssClass="Warning" DisplayMode="List" runat="server" />

<asp:MultiView ID="Views" runat="server">
	<asp:View ID="QuantityView" runat="server">
		<h3>Step 1 (of 3)</h3>
		<fieldset>
			<p class="Prompt">How many UTR Paks would you like?</p>
			<asp:DropDownList ID="QuantityField" runat="server">
				<asp:ListItem Text="1 - I'm a fan." Value="1" Selected="True" />
				<asp:ListItem Text="2 - I'm lovin' it." Value="2" />
				<asp:ListItem Text="3 - I'm going all out." Value="3" />
				<asp:ListItem Text="4 - I'm in it to win it." Value="4" />
			</asp:DropDownList>
		</fieldset>
		<div class="InputRow">
			<asp:Button Text="Next" OnClick="NextToConfirmation" runat="server" />
		</div>
	</asp:View>
	<asp:View ID="ConfirmationView" runat="server">
		<script>
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
		<h3>Step 2 (of 3)</h3>
		<p class="Prompt">Please confirm your selection.</p>
		<section>
			<h4 class="SelfEvident">Your selection</h4>
			<p>
				You have chosen <asp:Literal ID="QuantityBlurb" runat="server" /> paks at $75 each.
			</p>
			<dl>
				<dt>Subtotal:</dt>
				<dd><asp:Literal ID="SubtotalBlurb" runat="server" /></dd>
				<dt>Fees:</dt>
				<dd><asp:Literal ID="FeesBlurb" runat="server" /></dd>
				<dt><strong>Grand total:</strong></dt>
				<dd><strong><asp:Literal ID="GrandTotalBlurb" runat="server" /></strong></dd>
			</dl>
		</section>
		<section>
			<h4 class="SelfEvident">Terms and Conditions</h4>
			<p>
				All sales are final, no refunds or exchanges, each Pak is good for up to two tickets per UTR production*. For events at The Public Theater, tickets will be mailed at least 2 weeks in advance, otherwise they will be held at the Box Office for pick-up. For events at partner venues, tickets for each show must be booked 48 hours prior to performance time and will be held at their Box Offices for pick up on the day of the show. 
			</p>
			<p>
				There are a limited number of tickets held for Pak holders for each show at partner venues. 
			</p>
			<p>
				All Pak redemptions are subject to availability.
			</p>
			<p>
				*Except for one-night only events <i>Camille O'Sullivan: Feel</i> and <i>The Plot is The Revolution</i>, which are not part of the Pak.
			</p>
 		</section>
		<fieldset class="InputRow">
			<asp:CheckBox ID="AgreeField" Text="I agree to these terms and conditions."
					runat="server" />
			<asp:CustomValidator Text="*"
                    OnServerValidate="VerifyAgreed" CssClass="Warning" Display="Dynamic"
                    ErrorMessage="Please check the box to indicate that you agree to the terms and
                    conditions." runat="server" ClientValidationFunction="ValidateCheckBox" />
		</fieldset>
		<div class="InputRow">
			<asp:Button Text="Back" OnClick="BackToQuantity" CausesValidation="false"
					runat="server" />
			<asp:Button ID="NextToBillingButton" Text="Next" OnClick="NextToBilling"
					runat="server" />
		</div>
	</asp:View>
	<asp:View ID="BillingView" runat="server">
		<p id="BillingErrorMessage" class="Warning" visible="false" runat="server">
			Sorry.  Your credit card could not be processed.  Please double-check your information
			below and try again.
		</p>
        <p class="Prompt">
            Your grand total is
            <strong><asp:Literal ID="GrandTotalReminder" runat="server" /></strong>.
        </p>
		<h3>Step 3 (of 3)</h3>
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
                </dd>
                <dt>
                    <asp:Label AssociatedControlID="AddressField" runat="server">Street
                        address:</asp:Label>
                </dt>
                <dd>
                    <asp:TextBox ID="AddressField" MaxLength="55" runat="server"
                        CssClass="FieldScaleX20" />
                    <asp:RequiredFieldValidator
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
                    <asp:RequiredFieldValidator
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
                    <asp:RequiredFieldValidator
                        ControlToValidate="StateField" Display="Dynamic" runat="server" Text="*"
                        CssClass="Warning" ErrorMessage="Please select a state." />
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
                    <asp:RequiredFieldValidator
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
                    <asp:RequiredFieldValidator
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
                    <asp:Label ID="Label9" AssociatedControlID="CardTypeField" runat="server">
                        Credit card type:
                    </asp:Label>
                </dt>
                <dd>
                    <asp:DropDownList ID="CardTypeField" runat="server">
                        <asp:ListItem Text="" Value="" />
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator
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
                    <asp:TextBox ID="CardNumField" MaxLength="16" runat="server" />
                    <asp:RequiredFieldValidator
                        ControlToValidate="CardNumField" Display="Dynamic" Text="*" runat="server"
                        CssClass="Warning" ErrorMessage="Please enter your credit card number." />
                    <asp:RegularExpressionValidator
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
                    <asp:RequiredFieldValidator
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
                    </asp:DropDownList>
                    <asp:DropDownList ID="ExpYearField" runat="server">
                        <asp:ListItem Text="" Value="" Selected="True" />
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator
                        ControlToValidate="ExpMonthField" Display="Dynamic" Text="*"
                        CssClass="Warning" runat="server"
                        ErrorMessage="Please select an expiration month." />
                    <asp:RequiredFieldValidator
                        ControlToValidate="ExpYearField" Display="Dynamic" Text="*"
                        CssClass="Warning" runat="server"
                        ErrorMessage="Please select an expiration year." />
                    <asp:CustomValidator ControlToValidate="ExpYearField"
                        runat="server" Display="Dynamic" Text="*" CssClass="Warning"
                        ErrorMessage="Please enter an expiration date in the future."
                        onservervalidate="VerifyExpDateInFuture" />
                </dd>
                <dt>
                    <asp:Label ID="Label13" AssociatedControlID="AuthCodeField" runat="server">
                        CVV / CID number:
                    </asp:Label>
                </dt>
                <dd>
                    <asp:TextBox ID="AuthCodeField" MaxLength="4" runat="server" />
                    <asp:RequiredFieldValidator
                        ControlToValidate="AuthCodeField" Display="Dynamic" Text="*" runat="server"
                        CssClass="Warning" ErrorMessage="Please enter the CVV / CID number." />
                    <asp:RegularExpressionValidator
                        ControlToValidate="AuthCodeField" Display="Dynamic" Text="*" runat="server"
                        CssClass="Warning" ErrorMessage="Please enter a 3-4 digit CVV / CID number."
                        ValidationExpression="^[0-9]{3,4}$" />
                </dd>
            </dl>
            <div class="SecuritySeal">
                <h4 class="SelfEvident">Security Seal</h4>
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
			<asp:Button OnClick="BackToConfirmation" runat="server" CausesValidation="false"
					Text="Back" />
            <asp:Button ID="PurchaseButton" OnClick="ProcessPurchase" runat="server"
					Text="Confirm purchase" />
        </div>
	</asp:View>
	<asp:View ID="ThankYouView" runat="server">
		<h3>THANK YOU FOR ORDERING THE UTR PAK!</h3>
		<p>
			Please check your email for confirmation. You can now call our Member Hotline at 212-539-8650 or visit the Public Theater Box Office to book tickets to individual shows.
		</p>
		<p>
			For best availability, book your tickets during the priority booking period November 10 through December 6.  Alternatively, tickets may be booked for events at The Public anytime before curtain and for events at partner venues up to 48 hours prior to curtain time, subject to availability.
		</p>
	</asp:View>
</asp:MultiView>