<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AccountActivator.ascx.cs"
    Inherits="Nysf.UserControls.AccountActivator"
%><asp:MultiView ID="PasswordCreationWizard" ActiveViewIndex="0" runat="server">
    <asp:View runat="server">
        <asp:ValidationSummary DisplayMode="List" CssClass="Warning"
            runat="server" />
        <p class="Prompt">Please create a (new) password for your account.</p>
        <dl>
            <dt>
                <asp:Label ID="PasswordLabel"
                    AssociatedControlID="PasswordTextBox" runat="server">
                    Password:
                </asp:Label>
            </dt>
            <dd>
                <asp:TextBox ID="PasswordTextBox" MaxLength="32"
                    TextMode="Password" runat="server" />
                <asp:RequiredFieldValidator ControlToValidate="PasswordTextBox"
                    Display="None" runat="server" ID="PasswordRequiredValidator"
                    ErrorMessage="Please enter a password." />
            </dd>
            <dt>
                <asp:Label ID="ConfirmPasswordLabel"
                    AssociatedControlID="ConfirmPasswordTextBox" runat="server">
                    Re-enter password:
                </asp:Label>
            </dt>
            <dd>
                <asp:TextBox ID="ConfirmPasswordTextBox" MaxLength="32"
                    TextMode="Password" runat="server" />
                <asp:RequiredFieldValidator
                    ControlToValidate="ConfirmPasswordTextBox"
                    Display="None" runat="server"
                    ID="PasswordConfirmedValidator"
                    ErrorMessage="Please re-enter your password." />
                <asp:CompareValidator ID="PasswordsMatchValidator"
                    ControlToValidate="ConfirmPasswordTextBox"
                    ControlToCompare="PasswordTextBox" Operator="Equal"
                    Type="String" Display="None" runat="server"
                    ErrorMessage="The passwords do not match." />
            </dd>
        </dl>
        <div class="InputRow">
            <asp:Button ID="SubmitButton" Text="Submit" runat="server" 
                onclick="SubmitButton_Click" />
        </div>
		<asp:HiddenField ID="RedirectUrl" runat="server" Value="" />
    </asp:View>
    <asp:View runat="server">
        <p class="SuccessMessage">
            <asp:Literal ID="CongratulationsMessage" runat="server">
                Congratulations! Your account has been activated.
            </asp:Literal>
        </p>
        <div class="InputRow">
            <asp:Button ID="ContinueButton" Text="Continue" runat="server" 
                onclick="ContinueButton_Click" />
        </div>
    </asp:View>
    <asp:View runat="server">
        <p>
            <asp:Literal ID="ErrorMessage" runat="server">
                Sorry for the inconvenience, but a valid email / password pair
                was not supplied. If you are having trouble accessing your
                account, please contact the Box Office at 212-967-7555.
            </asp:Literal>
        </p>
    </asp:View>
</asp:MultiView>