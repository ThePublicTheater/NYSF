<%@ Control Language="C#" AutoEventWireup="true"
    CodeBehind="LoginForm.ascx.cs"
    Inherits="Nysf.UserControls.LoginForm"
%><div id="LoginFormMessages">
    <p id="LoginFailBlurb" class="Warning">
        <asp:Literal ID="FailBlurb" EnableViewState="false" Visible="false"
            runat="server">
                Sorry, but that email address and password do not match.
                Please try again, or choose an option below.
        </asp:Literal>
    </p>
    <asp:ValidationSummary ID="ValidationSummary1" DisplayMode="BulletList" CssClass="Warning"
        runat="server" ValidationGroup="LoginInputs" />
</div>
<dl id="LoginFormInputs">
    <dt>
        <asp:Label ID="EmailAddressLabel" AssociatedControlID="EmailTextBox"
            runat="server" Text="Username:" CssClass="textBoxDescriptor" />
    </dt>
    <dd>
        <asp:TextBox ID="EmailTextBox" MaxLength="80" runat="server" />
        <asp:RequiredFieldValidator ID="RequiredField1" ControlToValidate="EmailTextBox"
            Display="None" ErrorMessage="Please enter an email address."
            runat="server" ValidationGroup="LoginInputs" />
        <asp:RegularExpressionValidator ControlToValidate="EmailTextBox"
            Display="None" ID="EmailAddressValidator"
            ValidationExpression=
            "\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"
            ErrorMessage="Please enter a valid email address."
            runat="server" ValidationGroup="LoginInputs" />
    </dd>
    <dt>
        <asp:Label ID="PasswordLabel" AssociatedControlID="PasswordTextBox"
            runat="server" Text="Password:" CssClass="textBoxDescriptor" />
    </dt>
    <dd>
        <asp:TextBox ID="PasswordTextBox" TextMode="Password"
            MaxLength="32" runat="server" />
        <asp:RequiredFieldValidator ID="RequiredField2" ControlToValidate="PasswordTextBox"
            Display="None" ErrorMessage="Please enter a password." 
            runat="server" ValidationGroup="LoginInputs" />
    </dd>
    <%-- TODO: implement
    <dt>
        <asp:Label ID="RememberLabel" AssociatedControlID="RememberCheckBox"
            runat="server" Text="Remember on this computer" />
    </dt>
    <dd>
        <asp:CheckBox ID="RememberCheckBox" Checked="false"
            runat="server" />
    </dd>--%>
</dl>
<div class="InputRow">
    <asp:Button ID="SubmitButton" runat="server"
        onclick="SubmitButton_Click" Text="Sign In"
        ValidationGroup="LoginInputs" />
</div>
