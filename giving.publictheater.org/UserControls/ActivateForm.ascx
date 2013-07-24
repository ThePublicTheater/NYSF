<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ActivationForm.ascx.cs" Inherits="Nysf.Web.UserControls.ActivationForm" %>
<asp:MultiView ID="Views" ActiveViewIndex="0" runat="server">
	<asp:View runat="server">
		<asp:ValidationSummary CssClass="Warning" DisplayMode="BulletList" runat="server" />
		<p>
			<asp:Literal ID="Blurb_Prompt" runat="server">Please create a new password.</asp:Literal>
		</p>
		<p>
			<asp:Literal ID="Blurb_PreUsername" runat="server">Your username is: </asp:Literal><asp:Literal ID="DisplayUsername" runat="server" />
		</p>
		<fieldset>
			<legend class="SelfEvident">New password</legend>
			<div class="Field" id="Field_Password">
				<div class="Description">
					<asp:Label ID="Label_Password" AssociatedControlID="Input_Password" runat="server">Password:</asp:Label>
				</div>
				<div class="Entry">
					<asp:TextBox ID="Input_Password" CssClass="FieldScaleX7" MaxLength="32" TextMode="Password" runat="server" />
					<asp:RequiredFieldValidator ID="Blurb_EnterPassword" ControlToValidate="Input_Password" Text="*" Display="Dynamic" CssClass="Warning" ErrorMessage="Please enter a new password." runat="server" />
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
		</fieldset>
		<div><asp:HiddenField ID="Input_Email" runat="server" /></div>
		<div class="SubmitSet">
			<asp:Button ID="Label_SubmitButton" Text="Submit" OnClick="Do_ResetPassword" runat="server" />
		</div>
	</asp:View>
	<asp:View>
		<h3>Oops!</h3>
		<p class="Warning">That account recovery link has expired.</p>
	</asp:View>
</asp:MultiView>