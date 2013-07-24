<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="RecoverForm.ascx.cs" Inherits="Nysf.Web.UserControls.RecoverForm" %>
<%@ Register TagPrefix="recaptcha" Namespace="Recaptcha" Assembly="Recaptcha" %>
<asp:MultiView ID="Views" ActiveViewIndex="0" runat="server">
	<asp:View runat="server">
		<asp:Literal ID="Blurb_Prompt" runat="server">
			<p>
				Unsure of your username or password?
			</p>
			<p>
				If you've bought tickets online before, you may be able to look it up.
			</p>
			<p>
				To give it a try, please enter your email address below.
			</p>
		</asp:Literal>
		<asp:ValidationSummary CssClass="Warning" DisplayMode="BulletList" runat="server" />
		<fieldset>
			<div class="Field" id="Field_Email">
				<asp:TextBox ID="Input_Email" MaxLength="80" CssClass="FieldScaleX15" runat="server" />
				<asp:RequiredFieldValidator ID="Blurb_EnterEmail" ControlToValidate="Input_Email" Text="*" Display="Dynamic" CssClass="Warning" ErrorMessage="Please enter your email address." runat="server" />
				<asp:RegularExpressionValidator ID="Blurb_FixEmail" ControlToValidate="Input_Email" Text="*" Display="Dynamic" CssClass="Warning" ErrorMessage="The email address appears to be formatted improperly." ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" runat="server" />
			</div>
			<div class="Field" id="Field_Captcha">
				<recaptcha:RecaptchaControl ID="recaptcha" PublicKey="6Lem2cESAAAAAMsT3bIQXl9WyeQ0mkTXn2tPs2Pt" PrivateKey="6Lem2cESAAAAAC7mj31cAVGQRI6r8lhq6cX5ZEK7" runat="server" />
			</div>
		</fieldset>
		<div class="SubmitSet">
			<asp:Button ID="Label_SubmitButton" Text="Look up account" OnClick="Do_Lookup" runat="server" />
		</div>
	</asp:View>
	<asp:View runat="server">
		<h3>Success!</h3>
		<p>
            <asp:Literal ID="Blurb_SuccessMessage" runat="server">
                Your account has been located, and a confirmation message has been sent to your
				email address.<br /><br />
				Please follow the link in that email to access your account.
            </asp:Literal>
        </p>
	</asp:View>
</asp:MultiView>