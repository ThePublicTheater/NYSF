<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PasswordUpdater.ascx.cs"
    Inherits="Nysf.UserControls.PasswordUpdater"
%><asp:MultiView ID="PasswordWizard" ActiveViewIndex="0" runat="server">
    <asp:View runat="server">
        <asp:ValidationSummary DisplayMode="List" CssClass="Warning"
            runat="server" />
        <dl>
            <dt>
                <asp:Label ID="OldPasswordLabel"
                    AssociatedControlId="OldPasswordTextBox" runat="server">
                    Current password:
                </asp:Label>
            </dt>
            <dd>
                <asp:TextBox ID="OldPasswordTextBox" MaxLength="32"
                    TextMode="Password" runat="server" />
                <asp:RequiredFieldValidator ControlToValidate="OldPasswordTextBox" Display="None"
                    ErrorMessage="Please enter your current password." runat="server"
                    ID="OldPasswordRequiredValidator" />
            </dd>
            <dt>
                <asp:Label ID="NewPasswordLabel"
                    AssociatedControlID="NewPasswordTextBox" runat="server">
                    New password:
                </asp:Label>
            </dt>
            <dd>
                <asp:TextBox ID="NewPasswordTextBox" MaxLength="32"
                    TextMode="Password" runat="server" />
                <asp:RequiredFieldValidator ControlToValidate="NewPasswordTextBox"
                    Display="None" runat="server" ID="NewPasswordRequiredValidator"
                    ErrorMessage="Please enter a new password." />
            </dd>
            <dt>
                <asp:Label ID="ConfirmPasswordLabel"
                    AssociatedControlID="ConfirmPasswordTextBox" runat="server">
                    Re-enter new password:
                </asp:Label>
            </dt>
            <dd>
                <asp:TextBox ID="ConfirmPasswordTextBox" MaxLength="32"
                    TextMode="Password" runat="server" />
                <asp:RequiredFieldValidator
                    ControlToValidate="ConfirmPasswordTextBox"
                    Display="None" runat="server"
                    ID="PasswordConfirmedValidator"
                    ErrorMessage="Please re-enter your new password." />
                <asp:CompareValidator ID="PasswordsMatchValidator"
                    ControlToValidate="ConfirmPasswordTextBox"
                    ControlToCompare="NewPasswordTextBox" Operator="Equal"
                    Type="String" Display="None" runat="server"
                    ErrorMessage="The new passwords do not match." />
            </dd>
        </dl>
        <div class="InputRow">
            <asp:Button ID="SubmitButton" Text="Submit" runat="server" 
                onclick="SubmitButton_Click" />
        </div>
    </asp:View>
    <asp:View runat="server">
        <p class="SuccessMessage">
            <asp:Literal ID="SuccessBlurb" runat="server">
                Your password has been updated.
            </asp:Literal>
        </p>
        <div class="InputRow">
            <asp:Button ID="ContinueButton" Text="Continue" runat="server" 
                onclick="ReturnToReferer" />
        </div>
    </asp:View>
    <asp:View runat="server">
        <p class="Warning">
            <asp:Literal ID="OldPasswordBadWarning" runat="server">
                Sorry. The current password that you entered was incorrect.
            </asp:Literal>
        </p>
        <div class="InputRow">
            <asp:Button Text="Try again" OnClick="TryAgain" runat="server" />
            <asp:Button Text="Cancel" OnClick="ReturnToReferer" runat="server" />
        </div>
    </asp:View>
</asp:MultiView>