<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AccountFinder.ascx.cs"
    Inherits="Nysf.UserControls.AccountFinder" %>
<%@ Register TagPrefix="recaptcha" Namespace="Recaptcha" Assembly="Recaptcha"
%><asp:MultiView ID="AccountLookupWizard" ActiveViewIndex="0"
    runat="server">
    <asp:View runat="server">
        <fieldset>
            <p class="Prompt">
                <asp:Literal ID="Prompt" runat="server">
                    Please enter your email address.
                </asp:Literal>
            </p>
            <p id="LookupFailureWarning" visible="false" class="Warning" runat="server">
                Sorry, but that email address cannot be found. Please enter a
                different address, or for further assistance call the Box
                Office at 212-967-7555.
            </p>
            <asp:ValidationSummary CssClass="Warning" runat="server" 
                DisplayMode="SingleParagraph" />
            <div class="InputRow">
                <asp:TextBox ID="EmailTextBox" MaxLength="80" runat="server" />
            </div>
            <asp:RequiredFieldValidator ControlToValidate="EmailTextBox"
                ErrorMessage="An email address is required to look up your account."
                Display="None" runat="server" />
            <asp:RegularExpressionValidator
                ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"
                ControlToValidate="EmailTextBox"
                ErrorMessage="Please enter a valid email address."
                Display="None" runat="server" />
            <recaptcha:RecaptchaControl ID="recaptcha" runat="server"
                PublicKey="<%$ AppSettings:nysf_UserControls_RecaptchaPublicKey %>"
                PrivateKey="<%$ AppSettings:nysf_UserControls_RecaptchaPrivateKey %>" />
        </fieldset>
        <div class="InputRow">
            <asp:Button ID="SubmitButton" Text="Submit" runat="server" 
                onclick="SubmitButton_Click" />
        </div>
    </asp:View>
    <asp:View runat="server">
        <p class="SuccessMessage">
            <asp:Literal ID="CheckEmailMessage" runat="server">
                Your account has been located, and a confirmation message has been sent to your
				email address.<br /><br />
				Please follow the link in that email to access your account.
            </asp:Literal>
        </p>
    </asp:View>
</asp:MultiView>