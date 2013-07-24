<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CheckoutWizard.ascx.cs"
Inherits="Nysf.Apps.OldStyleTickets.CheckoutWizard"
%><asp:MultiView ID="CheckoutViews" runat="server">
    <asp:View ID="BillingEntryView" runat="server">
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
                    <asp:TextBox ID="TextBox111" MaxLength="55" runat="server"
                        CssClass="FieldScaleX20" />
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1113"
                        ControlToValidate="TextBox111" Display="Dynamic" Text="*" runat="server"
                        CssClass="Warning" ErrorMessage="Please enter a Name." />
                    <div class="FormFieldExample">Example: First and Last Name</div>
                </dd>
                <dt>
                    Email address:
                </dt>
                <dd>
                    <asp:Literal ID="EmailBlurb" runat="server" />
                    <asp:TextBox ID="TextBox122" MaxLength="55" runat="server"
                        CssClass="FieldScaleX20" />
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1223"
                        ControlToValidate="TextBox122" Display="Dynamic" Text="*" runat="server"
                        CssClass="Warning" ErrorMessage="Please enter an email." />
                    <div class="FormFieldExample">Example: name@domain.com</div>
                </dd>
                <dt>
                    <asp:Label ID="Label6" AssociatedControlID="CountryField"
                        runat="server">Country:</asp:Label>
                </dt>
                <dd>
                    <asp:DropDownList ID="CountryField" AutoPostBack="true" CausesValidation="false"
                            OnSelectedIndexChanged="CountryChanged" runat="server">
                        <asp:ListItem Selected="True" Value="" Text="" />
                    </asp:DropDownList><%-- TODO: add non-JS alternative --%>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator7"
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
                <dt>
                    <asp:Label AssociatedControlId="ShippingMethodField" runat="server">Shipping
                        method:</asp:Label>
                </dt>
                <dd>
                    <asp:DropDownList ID="ShippingMethodField" runat="server">
                        <asp:ListItem Selected="True" Value="" Text="" />
                        <asp:ListItem Value="-1" Text="Hold at Box Office" />
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator ControlToValidate="ShippingMethodField"
                        Display="Dynamic" Text="*" CssClass="Warning" runat="server"
                        ErrorMessage="Please select a shipping method." />
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
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1"
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
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2"
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
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3"
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
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator4"
                        ControlToValidate="ExpMonthField" Display="Dynamic" Text="*"
                        CssClass="Warning" runat="server"
                        ErrorMessage="Please select an expiration month." />
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator5"
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
                    <asp:TextBox ID="AuthCodeField" MaxLength="4" runat="server" />
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator6"
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
		<p class="Prompt">
			All sales are final – no refunds or exchanges.
		</p>
        <div class="InputRow">
            <asp:Button OnClick="ProcessReservation" runat="server" Text="Confirm purchase" />
            <asp:Button OnClick="RedirectToReferer" runat="server" Text="Continue shopping"
                CausesValidation="false" />
        </div>
        <asp:HiddenField ID="OrderNumber" runat="server" />
        <asp:HiddenField ID="AmountDue" runat="server" />
        <asp:HiddenField ID="CartOrgs" runat="server" />
    </asp:View>
    <asp:View ID="EmptyCartView" runat="server">
        <p class="Warning">Sorry, but your cart is empty and may have expired. Please try again.</p>
        <div class="InputRow">
            <a href="/" title="Home" class="CommandLink">Home</a>
        </div>
    </asp:View>
    <asp:View ID="SuccessView" runat="server">
        <p class="SuccessMessage">
            Thank you! Your order has been processed. You should receive an email confirmation
            shortly.
        </p>
        <ul class="LinkMenu">
            <li><a href="/" title="Home" class="CommandLink">Home</a></li>
        </ul>
    </asp:View>
</asp:MultiView>