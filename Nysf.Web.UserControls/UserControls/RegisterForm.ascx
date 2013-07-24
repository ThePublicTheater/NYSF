<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="RegisterForm.ascx.cs" Inherits="Nysf.Web.UserControls.RegisterForm" %>
<%@ Register TagPrefix="recaptcha" Namespace="Recaptcha" Assembly="Recaptcha" %>
<asp:ValidationSummary CssClass="Warning" DisplayMode="BulletList" runat="server" />
<div class="SelfEvident">
	<asp:Button ID="Label_DefaultSubmit" Text="Register" OnClick="Do_Register" runat="server" />
</div>
<fieldset>
	<legend class="SelfEvident">
		<asp:Literal ID="Blurb_NewInfoLegend" runat="server">New account information</asp:Literal>
	</legend>
	<div class="Field" id="Field_FirstName">
		<div class="Description">
			<asp:Label ID="Label_FirstName" AssociatedControlID="Input_FirstName" runat="server">First name:</asp:Label>
		</div>
		<div class="Entry">
			<asp:TextBox ID="Input_FirstName" CssClass="FieldScaleX10" MaxLength="20" runat="server" />
			<asp:RequiredFieldValidator ID="Blurb_EnterFirstName" ControlToValidate="Input_FirstName" Text="*" Display="Dynamic" CssClass="Warning" ErrorMessage="Please enter your first name." runat="server" />
		</div>
	</div>
	<div class="Field" id="Field_LastName">
		<div class="Description">
			<asp:Label ID="Label_LastName" AssociatedControlID="Input_LastName" runat="server">Last name:</asp:Label>
		</div>
		<div class="Entry">
			<asp:TextBox ID="Input_LastName" CssClass="FieldScaleX10" MaxLength="55" runat="server" />
			<asp:RequiredFieldValidator ID="Blurb_EnterLastName" ControlToValidate="Input_LastName" Text="*" Display="Dynamic" CssClass="Warning" ErrorMessage="Please enter your last name." runat="server" />
		</div>
	</div>
	<div class="Field" id="Field_Email">
		<div class="Description">
			<asp:Label ID="Label_Email" AssociatedControlID="Input_Email" runat="server">Email address:</asp:Label>
		</div>
		<div class="Entry">
			<asp:TextBox ID="Input_Email" CssClass="FieldScaleX15" MaxLength="80" runat="server" />
			<asp:RequiredFieldValidator ID="Blurb_EnterEmail" ControlToValidate="Input_Email" Text="*" Display="Dynamic" CssClass="Warning" ErrorMessage="Please enter your email address." runat="server" />
			<asp:RegularExpressionValidator ID="Blurb_FixEmail" ControlToValidate="Input_Email" Text="*" Display="Dynamic" CssClass="Warning" ErrorMessage="The email address appears to be formatted improperly." ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" runat="server" />
		</div>
	</div>
	<div class="Field" id="Field_EmailConfirm">
		<div class="Description">
			<asp:Label ID="Label_EmailConfirm" AssociatedControlID="Input_EmailConfirm" runat="server">Confirm email:</asp:Label>
		</div>
		<div class="Entry">
			<asp:TextBox ID="Input_EmailConfirm" CssClass="FieldScaleX15" MaxLength="80" runat="server" />
			<asp:RequiredFieldValidator ID="Blurb_ConfirmEmail" ControlToValidate="Input_EmailConfirm" Text="*" Display="Dynamic" CssClass="Warning" ErrorMessage="Please confirm your email address." runat="server" />
			<asp:CompareValidator ID="Blurb_EnterSameEmail" ControlToValidate="Input_EmailConfirm" ControlToCompare="Input_Email" Type="String" Operator="Equal" Text="*" Display="Dynamic" CssClass="Warning" ErrorMessage="The email addresses are different." runat="server" />
		</div>
	</div>
	<div class="Field" id="Field_Password">
		<div class="Description">
			<asp:Label ID="Label_Password" AssociatedControlID="Input_Password" runat="server">Password:</asp:Label>
		</div>
		<div class="Entry">
			<asp:TextBox ID="Input_Password" CssClass="FieldScaleX7" MaxLength="32" TextMode="Password" runat="server" />
			<asp:RequiredFieldValidator ID="Blurb_EnterPassword" ControlToValidate="Input_Password" Text="*" Display="Dynamic" CssClass="Warning" ErrorMessage="Please create a password." runat="server" />
		</div>
	</div>
	<div class="Field" id="Field_PasswordConfirm">
		<div class="Description">
			<asp:Label ID="Label_PasswordConfirm" AssociatedControlID="Input_PasswordConfirm" runat="server">Confirm password:</asp:Label>
		</div>
		<div class="Entry">
			<asp:TextBox ID="Input_PasswordConfirm" CssClass="FieldScaleX7" MaxLength="32" TextMode="Password" runat="server" />
			<asp:RequiredFieldValidator ID="Blurb_ConfirmPassword" ControlToValidate="Input_PasswordConfirm" Text="*" Display="Dynamic" CssClass="Warning" ErrorMessage="Please confirm your password." runat="server" />
			<asp:CompareValidator ID="Blurb_EnterSamePassword" ControlToValidate="Input_PasswordConfirm" ControlToCompare="Input_Password" Type="String" Operator="Equal" Text="*" Display="Dynamic" CssClass="Warning" ErrorMessage="The passwords are different." runat="server" />
		</div>
	</div>
	<div class="Field" id="Field_EmailOptIn">
		<asp:CheckBox ID="Input_EmailOptIn" Checked="true" runat="server" />
		<asp:Label ID="Label_EmailOptIn" AssociatedControlID="Input_EmailOptIn" runat="server">I wish to receive email updates.</asp:Label>
	</div>
</fieldset>
<fieldset>
	<legend class="SelfEvident">
		<asp:Literal ID="Blurb_CaptchaLegend" runat="server">Captcha</asp:Literal>
	</legend>
	<div class="Field" id="Field_Captcha">
		<recaptcha:RecaptchaControl ID="recaptcha" PublicKey="6Lem2cESAAAAAMsT3bIQXl9WyeQ0mkTXn2tPs2Pt" PrivateKey="6Lem2cESAAAAAC7mj31cAVGQRI6r8lhq6cX5ZEK7" runat="server" />
	</div>
</fieldset>
<div class="SubmitSet">
	<asp:Button ID="Label_SubmitButton" Text="Register" OnClick="Do_Register" runat="server" />
</div>