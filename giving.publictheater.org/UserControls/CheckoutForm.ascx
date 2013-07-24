<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CheckoutForm.ascx.cs" Inherits="Nysf.Web.UserControls.UserControls.CheckoutForm" %>
<p>
	<asp:Literal ID="Blurb_PreTotal" runat="server">Your grand total is </asp:Literal><strong><asp:Literal ID="Display_GrandTotal" runat="server" /></strong><asp:Literal ID="Blurb_PostGrandTotal" runat="server">.</asp:Literal>
</p>
<asp:ValidationSummary ID="ValidationSummary1" CssClass="Warning" DisplayMode="BulletList" runat="server" />
<fieldset>
	<legend>Your account</legend>
	<div class="Field" id="Field_Name">
		<div class="Description">
			<asp:Literal ID="Label_Name" runat="server">Name:</asp:Literal>
		</div>
		<div class="Entry">
			<asp:Literal ID="Display_Name" runat="server" />
		</div>
	</div>
	<div class="Field" id="Field_Email">
		<div class="Description">
			<asp:Literal ID="Label_Email" runat="server">Email address:</asp:Literal>
		</div>
		<div class="Entry">
			<asp:Literal ID="Display_Email" runat="server" />
		</div>
	</div>
</fieldset>
<fieldset>
	<legend>Shipping</legend>
	<div class="Field" id="Field_Country">
		<div class="Description">
			<asp:Label ID="Label_Country" AssociatedControlID="Input_Country" runat="server">Country:</asp:Label>
		</div>
		<div class="Entry">
			<asp:DropDownList ID="Input_Country" AutoPostBack="true" CausesValidation="false" OnSelectedIndexChanged="DoPrepStateDropDown" runat="server">
				<asp:ListItem Text="" Value="" Selected="True" />
			</asp:DropDownList>
			<asp:RequiredFieldValidator ID="Blurb_SelectCountry" ControlToValidate="Input_Country" Text="*" Display="Dynamic" CssClass="Warning" ErrorMessage="Please select a country." runat="server" />
		</div>
	</div>
	<div class="Field" id="Field_StreetAddress">
		<div class="Description">
			<asp:Label ID="Label_StreetAddress" AssociatedControlID="Input_StreetAddress" runat="server">Street address:</asp:Label>
		</div>
		<div class="Entry">
			<asp:TextBox ID="Input_StreetAddress" CssClass="FieldScaleX20" MaxLength="55" runat="server" />
			<asp:RequiredFieldValidator ID="Blurb_EnterStreetAddress" ControlToValidate="Input_StreetAddress" Text="*" Display="Dynamic" CssClass="Warning" ErrorMessage="Please enter your street address." runat="server" />
			<div class="Subtext">
				Example: 123 Main St Apt 456
			</div>
		</div>
	</div>
	<div class="Field" id="Field_SubStreet">
		<div class="Description">
			<asp:Label ID="Label_SubStreet" AssociatedControlID="Input_SubStreet" runat="server">Sub-address:</asp:Label>
		</div>
		<div class="Entry">
			<asp:TextBox ID="Input_SubStreet" CssClass="FieldScaleX20" MaxLength="55" runat="server" />
			<div class="Subtext">
				Company name, department name, c/o
			</div>
		</div>
	</div>
	<div class="Field" id="Field_City">
		<div class="Description">
			<asp:Label ID="Label_City" AssociatedControlID="Input_City" runat="server">City:</asp:Label>
		</div>
		<div class="Entry">
			<asp:TextBox ID="Input_City" CssClass="FieldScaleX8" MaxLength="30" runat="server" />
			<asp:RequiredFieldValidator ID="Blurb_EnterCity" ControlToValidate="Input_City" Text="*" Display="Dynamic" CssClass="Warning" ErrorMessage="Please enter your city." runat="server" />
		</div>
	</div>
	<asp:Panel class="Field" id="Field_State" runat="server">
		<div class="Description">
			<asp:Label ID="Label_State" AssociatedControlID="Input_State" runat="server">State:</asp:Label>
		</div>
		<div class="Entry">
			<asp:DropDownList ID="Input_State" runat="server">
				<asp:ListItem Text="" Value="" runat="server" />
			</asp:DropDownList>
			<asp:RequiredFieldValidator ID="Blurb_ChooseState" ControlToValidate="Input_State" Text="*" Display="Dynamic" CssClass="Warning" ErrorMessage="Please choose your state." runat="server" />
		</div>
	</asp:Panel>
	<div class="Field" id="Field_PostalCode">
		<div class="Description">
			<asp:Label ID="Label_PostalCode" AssociatedControlID="Input_PostalCode" runat="server">Postal code:</asp:Label>
		</div>
		<div class="Entry">
			<asp:TextBox ID="Input_PostalCode" CssClass="FieldScaleX5" MaxLength="10" runat="server" />
			<asp:RequiredFieldValidator ID="Blurb_EnterPostalCode" ControlToValidate="Input_PostalCode" Text="*" Display="Dynamic" CssClass="Warning" ErrorMessage="Please enter your postal code." runat="server" />
		</div>
	</div>
	<%-- TODO: implement:
	<div class="Field" id="Field_AddressType">
		<div class="Description">
			<asp:Label ID="Label_AddressType" AssociatedControlID="Input_AddressType" runat="server">Address type:</asp:Label>
		</div>
		<div class="Entry">
            <asp:DropDownList ID="Input_AddressType" runat="server">
                <asp:ListItem Value="3" Text="Home" Selected="True" />
                <asp:ListItem Value="2" Text="Business" />
                <asp:ListItem Value="13" Text="Other" />
            </asp:DropDownList>
		</div>
	</div> --%>
	<div class="Field" id="Field_Phone">
		<div class="Description">
			<asp:Label ID="Label_Phone" AssociatedControlID="Input_Phone" runat="server">Phone number:</asp:Label>
		</div>
		<div class="Entry">
			<asp:TextBox ID="Input_Phone" CssClass="FieldScaleX8" MaxLength="32" runat="server" />
			<asp:RequiredFieldValidator ID="Blurb_EnterPhone" ControlToValidate="Input_Phone" Text="*" Display="Dynamic" CssClass="Warning" ErrorMessage="Please enter your phone number." runat="server" />
		</div>
	</div>
	<asp:Panel CssClass="Field" id="Field_ShippingMethod" runat="server">
		<div class="Description">
			<asp:Label ID="Label_ShippingMethod" AssociatedControlID="Input_ShippingMethod" runat="server">Shipping method:</asp:Label>
		</div>
		<div class="Entry">
			<asp:DropDownList ID="Input_ShippingMethod" runat="server">
				<asp:ListItem Value="-1" Text="Hold at Box Office" Selected="True" />
			</asp:DropDownList>
		</div>
	</asp:Panel>
</fieldset>
<fieldset>
	<legend>Billing</legend>
	<div class="Field" id="Field_CardType">
		<div class="Description">
			<asp:Label ID="Label_CardType" AssociatedControlID="Input_CardType" runat="server">Credit card type:</asp:Label>
		</div>
		<div class="Entry">
			<asp:DropDownList ID="Input_CardType" runat="server">
				<asp:ListItem Text="" Value="" runat="server" />
			</asp:DropDownList>
			<asp:RequiredFieldValidator ID="Blurb_ChooseCardType" ControlToValidate="Input_CardType" Text="*" Display="Dynamic" CssClass="Warning" ErrorMessage="Please select your credit card type." runat="server" />
		</div>
	</div>
	<div class="Field" id="Field_CardNum">
		<div class="Description">
			<asp:Label ID="Label_CardNum" AssociatedControlID="Input_CardNum" runat="server">Credit card number:</asp:Label>
		</div>
		<div class="Entry">
			<asp:TextBox ID="Input_CardNum" CssClass="FieldScaleX10" MaxLength="16" runat="server" />
			<asp:RequiredFieldValidator ID="Blurb_EnterCardNum" ControlToValidate="Input_CardNum" Text="*" Display="Dynamic" CssClass="Warning" ErrorMessage="Please enter your credit card number." runat="server" />
			<asp:RegularExpressionValidator ID="Blurb_FixCardNum" ControlToValidate="Input_CardNum" Text="*" Display="Dynamic" CssClass="Warning" ErrorMessage="Please enter only digits for your credit card number." ValidationExpression="^[0-9]+$" runat="server" />
			<div class="Subtext">
				(only numbers, no spaces or dashes please)
			</div>
		</div>
	</div>
	<div class="Field" id="Field_CardHolder">
		<div class="Description">
			<asp:Label ID="Label_CardHolder" AssociatedControlID="Input_CardHolder" runat="server">Cardholder name:</asp:Label>
		</div>
		<div class="Entry">
			<asp:TextBox ID="Input_CardHolder" CssClass="FieldScaleX15" MaxLength="97" runat="server" />
			<asp:RequiredFieldValidator ID="Blurb_EnterCardHolder" ControlToValidate="Input_CardHolder" Text="*" Display="Dynamic" CssClass="Warning" ErrorMessage="Please enter the cardholder's name." runat="server" />
		</div>
	</div>
	<div class="Field" id="Field_Exp">
		<div class="Description">
			<asp:Label ID="Label_Exp" AssociatedControlID="Input_ExpMonth" runat="server">Expiration:</asp:Label>
		</div>
		<div class="Entry">
			<asp:DropDownList ID="Input_ExpMonth" runat="server">
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
			<asp:DropDownList ID="Input_ExpYear" runat="server">
				<asp:ListItem Text="" Value="" Selected="True" />
			</asp:DropDownList>
			<asp:RequiredFieldValidator ID="Blurb_ChooseExpMonth" ControlToValidate="Input_ExpMonth" Text="*" Display="Dynamic" CssClass="Warning" ErrorMessage="Please choose your credit card's expiration month." runat="server" />
			<asp:RequiredFieldValidator ID="Blurb_ChooseExpYear" ControlToValidate="Input_ExpYear" Text="*" Display="Dynamic" CssClass="Warning" ErrorMessage="Please choose your credit card's expiration year." runat="server" />
		</div>
	</div>
	<div class="Field" id="Field_SecurityCode">
		<div class="Description">
			<asp:Label ID="Label_SecurityCode" AssociatedControlID="Input_SecurityCode" runat="server">Security code:</asp:Label>
		</div>
		<div class="Entry">
			<asp:TextBox ID="Input_SecurityCode" CssClass="FieldScaleX4" MaxLength="4" runat="server" />
		</div>
	</div>
</fieldset>
<p>
	<asp:Literal ID="Blurb_FinalDisclaimer" visible="false" runat="server">All sales are final – no refunds or exchanges.</asp:Literal>
</p>
<div class="SubmitSet">
	<asp:Button ID="Label_SubmitButton" Text="Confirm purchase" OnClick="DoMakePurchase" runat="server" />
</div>