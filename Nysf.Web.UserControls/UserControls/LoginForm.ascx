<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="LoginForm.ascx.cs" Inherits="Nysf.Web.UserControls.LoginForm" %>
<asp:ValidationSummary ID="ValidationSummary1" CssClass="Warning" DisplayMode="BulletList" runat="server" />
<fieldset>
	<legend class="SelfEvident">
		<asp:Literal ID="Blurb_Legend" runat="server">Your credentials</asp:Literal>
	</legend>
	<div class="Field" id="Field_Email">
		<div class="Description">
			<asp:Label ID="Label_Email" AssociatedControlID="Input_Email" runat="server">
				Username:
			</asp:Label>
		</div>
		<div class="Entry">
			<asp:TextBox ID="Input_Email" CssClass="FieldScaleX15" MaxLength="80" runat="server" />
			<asp:RequiredFieldValidator ID="Blurb_EnterEmail" ControlToValidate="Input_Email" Text="*" Display="Dynamic" CssClass="Warning" ErrorMessage="Please enter your email address." runat="server" />
			<asp:RegularExpressionValidator ID="Blurb_FixEmail" ControlToValidate="Input_Email" Text="*" Display="Dynamic" CssClass="Warning" ErrorMessage="The email address appears to be formatted improperly." ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" runat="server" Enabled="false" /><%-- TODO: Enable when all logins standardized to email addresses --%>
		</div>
	</div>
	<div class="Field" id="Field_Password">
		<div class="Description">
			<asp:Label ID="Label_Password" AssociatedControlID="Input_Password" runat="server">
				Password:
			</asp:Label>
		</div>
		<div class="Entry">
			<asp:TextBox ID="Input_Password" CssClass="FieldScaleX15" MaxLength="32" TextMode="Password" runat="server" />
			<asp:RequiredFieldValidator ID="Blurb_EnterPassword" ControlToValidate="Input_Password" Text="*" Display="Dynamic" CssClass="Warning" ErrorMessage="Please enter your password." runat="server" />
		</div>
	</div>
</fieldset>
<div class="SubmitSet">
	<asp:Button ID="Label_SubmitButton" Text="Sign in" OnClick="Do_SignIn" runat="server" />
</div>