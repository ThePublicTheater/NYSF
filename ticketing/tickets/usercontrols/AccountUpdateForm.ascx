<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AccountUpdateForm.ascx.cs"
    Inherits="Nysf.UserControls.AccountUpdateForm" %>
<%@ Register TagPrefix="recaptcha" Namespace="Recaptcha" Assembly="Recaptcha"
%><asp:MultiView ID="AccountUpdateViews" ActiveViewIndex="0" runat="server">
    <asp:View runat="server">
        <div class="InputRow">
            <asp:Button Text="Save account information" OnClick="Save_Click" runat="server" />
            <asp:Button Text="Go back" OnClick="RedirectToReferer" CausesValidation="false"
                runat="server" />
        </div>
        <p id="ConfirmationBlurb" class="SuccessMessage" visible="false" runat="server">
            Your account was updated successfully.
        </p>
        <p id="EmailExistsWarning" class="Warning" visible="false" runat="server">
            That email address is already associated with an account.
        </p>
        <asp:ValidationSummary CssClass="Warning" DisplayMode="BulletList" runat="server" />
        <asp:CustomValidator Display="None" runat="server"
            ErrorMessage="Please enter both a first and last name." 
            onservervalidate="ValidateNames" />
        <asp:RequiredFieldValidator ControlToValidate="EmailField" Display="None" runat="server"
            ErrorMessage="Please enter an email address." />
        <asp:RegularExpressionValidator ControlToValidate="EmailField" Display="None" runat="server"
            ErrorMessage="Please enter a valid email address."
            ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" />
        <asp:RegularExpressionValidator ControlToValidate="PhoneField" Display="None" runat="server"
            ErrorMessage="Please enter a valid phone number."
            ValidationExpression="^\s*\d*[\d\.\-\(\)\s]*\s*$" />
        <asp:RegularExpressionValidator ControlToValidate="Phone2Field" Display="None"
            ErrorMessage="Please enter a valid alternate phone number." runat="server"
            ValidationExpression="^\s*\d*[\d\.\-\(\)\s]*\s*$" />
        <asp:RegularExpressionValidator ControlToValidate="FaxField" Display="None" runat="server"
            ErrorMessage="Please enter a valid fax number."
            ValidationExpression="^\s*\d*[\d\.\-\(\)\s]*\s*$" />
        <fieldset>
            <legend>Name</legend>
            <dl>
                <dt>
                    <asp:Label AssociatedControlID="PrefixField" runat="server">Prefix:</asp:Label>
                </dt>
                <dd>
                    <asp:DropDownList ID="PrefixField" runat="server">
                        <asp:ListItem Selected="True" Value="0" Text="" />
                        <asp:ListItem Value="Dr." Text="Dr." />
                        <asp:ListItem Value="Miss." Text="Miss" />
                        <asp:ListItem Value="Mr." Text="Mr." />
                        <asp:ListItem Value="Mrs." Text="Mrs." />
                        <asp:ListItem Value="Ms." Text="Ms." />
                    </asp:DropDownList>
                </dd>
                <dt>
                    <asp:Label AssociatedControlID="FirstNameField" CssClass="RequiredFieldLabel"
                        runat="server">First name:</asp:Label>
                </dt>
                <dd>
                    <asp:TextBox ID="FirstNameField" MaxLength="20" runat="server" />
                </dd>
                <dt>
                    <asp:Label AssociatedControlID="MiddleNameField" runat="server">Middle
                        name:</asp:Label>
                </dt>
                <dd>
                    <asp:TextBox ID="MiddleNameField" MaxLength="20" runat="server" />
                </dd>
                <dt>
                    <asp:Label AssociatedControlID="LastNameField" CssClass="RequiredFieldLabel"
                        runat="server">Last name:</asp:Label>
                </dt>
                <dd>
                    <asp:TextBox ID="LastNameField" MaxLength="55" runat="server" />
                </dd>
                <dt>
                    <asp:Label AssociatedControlID="GenderField" runat="server">Gender:</asp:Label>
                </dt>
                <dd>
                    <asp:DropDownList ID="GenderField" runat="server">
                        <asp:ListItem Selected="True" Value="0" Text="" />
                        <asp:ListItem Value="F" Text="female" />
                        <asp:ListItem Value="M" Text="male" />
                        <asp:ListItem Value="I" Text="other" />
                    </asp:DropDownList>
                </dd>
            </dl>
            <div class="InputRow">
                <asp:LinkButton ID="Name2Button" CausesValidation="false" Text="Add a second name"
                    runat="server" onclick="Name2Button_Click" />
            </div>
        </fieldset>
        <fieldset id="Name2Fieldset" runat="server" visible="false">
            <legend>Second Name</legend>
            <dl>
                <dt>
                    <asp:Label AssociatedControlID="Prefix2Field" runat="server">Prefix:</asp:Label>
                </dt>
                <dd>
                    <asp:DropDownList ID="Prefix2Field" runat="server">
                        <asp:ListItem Selected="True" Value="0" Text="" />
                        <asp:ListItem Value="Dr." Text="Dr." />
                        <asp:ListItem Value="Miss." Text="Miss" />
                        <asp:ListItem Value="Mr." Text="Mr." />
                        <asp:ListItem Value="Mrs." Text="Mrs." />
                        <asp:ListItem Value="Ms." Text="Ms." />
                    </asp:DropDownList>
                </dd>
                <dt>
                    <asp:Label AssociatedControlID="FirstName2Field" runat="server">First name:</asp:Label>
                </dt>
                <dd>
                    <asp:TextBox ID="FirstName2Field" MaxLength="20" runat="server" />
                </dd>
                <dt>
                    <asp:Label AssociatedControlID="MiddleName2Field" runat="server">Middle
                        name:</asp:Label>
                </dt>
                <dd>
                    <asp:TextBox ID="MiddleName2Field" MaxLength="20" runat="server" />
                </dd>
                <dt>
                    <asp:Label AssociatedControlID="LastName2Field" runat="server">Last
                        name:</asp:Label>
                </dt>
                <dd>
                    <asp:TextBox ID="LastName2Field" MaxLength="55" runat="server" />
                </dd>
                <dt>
                    <asp:Label AssociatedControlID="Gender2Field" runat="server">Gender:</asp:Label>
                </dt>
                <dd>
                    <asp:DropDownList ID="Gender2Field" runat="server">
                        <asp:ListItem Selected="True" Value="0" Text="" />
                        <asp:ListItem Value="F" Text="female" />
                        <asp:ListItem Value="M" Text="male" />
                        <asp:ListItem Value="I" Text="other" />
                    </asp:DropDownList>
                </dd>
            </dl>
        </fieldset>
        <fieldset>
            <legend>Contact Information</legend>
            <dl>
                <dt>
                    <asp:Label AssociatedControlID="EmailField" CssClass="RequiredFieldLabel"
                        runat="server">E-mail address:</asp:Label>
                </dt>
                <dd>
                    <asp:TextBox ID="EmailField" MaxLength="80" runat="server" />
                </dd>
                <dt>
                    <asp:Label AssociatedControlID="PhoneField" runat="server">Phone
                        number:</asp:Label>
                </dt>
                <dd>
                    <asp:TextBox ID="PhoneField" MaxLength="32" runat="server" />
                </dd>
                <dt>
                    <asp:Label AssociatedControlID="Phone2Field" runat="server">Alternate phone
                        number:</asp:Label>
                </dt>
                <dd>
                    <asp:TextBox ID="Phone2Field" MaxLength="32" runat="server" />
                </dd>
                <dt>
                    <asp:Label AssociatedControlID="FaxField" runat="server">Fax number:</asp:Label>
                </dt>
                <dd>
                    <asp:TextBox ID="FaxField" MaxLength="32" runat="server" />
                </dd>
                <dt>
                    <asp:Label AssociatedControlID="CountryField"
                        runat="server">Country:</asp:Label>
                </dt>
                <dd>
                    <asp:DropDownList ID="CountryField" AutoPostBack="true" CausesValidation="false"
                         OnSelectedIndexChanged="CountryChanged" runat="server">
                        <asp:ListItem Selected="True" Value="0" Text="" />
                    </asp:DropDownList><%-- TODO: add non-JS alternative --%>
                </dd>
                <dt>
                    <asp:Label AssociatedControlID="AddressField" runat="server">Street
                        address:</asp:Label>
                </dt>
                <dd>
                    <asp:TextBox ID="AddressField" MaxLength="55" runat="server" />
                    <div class="FormFieldExample">Example: 123 Papp St Apt 456</div>
                </dd>
                <dt>
                    <asp:Label AssociatedControlID="SubAddressField"
                        runat="server">Sub-address:</asp:Label>
                </dt>
                <dd>
                    <asp:TextBox ID="SubAddressField" MaxLength="55" runat="server" />
                    <div class="FormFieldExample">Company name, department name, c/o</div>
                </dd>
                <dt>
                    <asp:Label AssociatedControlID="CityField" runat="server">City:</asp:Label>
                </dt>
                <dd>
                    <asp:TextBox ID="CityField" MaxLength="30" runat="server" />
                </dd>
            </dl>
            <dl id="StateFieldGroup" runat="server" visible="false">
                <dt>
                    <asp:Label AssociatedControlID="StateField" runat="server">State /
                        Province:</asp:Label>
                </dt>
                <dd>
                    <asp:DropDownList ID="StateField" runat="server">
                        <asp:ListItem Selected="True" Value="0" Text="" />
                    </asp:DropDownList>
                </dd>
			</dl>
			<dl>
                <dt>
                    <asp:Label AssociatedControlID="ZipField" runat="server">Postal
                        code:</asp:Label>
                </dt>
                <dd>
                    <asp:TextBox ID="ZipField" MaxLength="10" runat="server" />
                </dd>
                <dt>
                    <asp:Label AssociatedControlID="AddressTypeField" runat="server">Address
                        type:</asp:Label>
                </dt>
                <dd>
                    <asp:DropDownList ID="AddressTypeField" runat="server">
                        <asp:ListItem Selected="True" Value="0" Text="" />
                        <asp:ListItem Value="3" Text="Home" />
                        <asp:ListItem Value="2" Text="Business" />
                        <asp:ListItem Value="13" Text="Other" />
                    </asp:DropDownList>
                </dd>
            </dl>
        </fieldset>
        <div class="InputRow">
            <asp:Button Text="Save account information" runat="server" 
                onclick="Save_Click" />
            <asp:Button Text="Go back" OnClick="RedirectToReferer" CausesValidation="false"
                runat="server" />
        </div>
        <asp:HiddenField ID="CanReceiveHtmlEmailField" runat="server" />
        <asp:HiddenField ID="BusinessTitleField" runat="server" />
        <asp:HiddenField ID="SuffixField" runat="server" />
        <asp:HiddenField ID="Suffix2Field" runat="server" />
        <asp:HiddenField ID="OldEmailField" runat="server" />
        <asp:HiddenField ID="WantEmailField" runat="server" />
        <asp:HiddenField ID="WantMailField" runat="server" />
        <asp:HiddenField ID="WantPhoneField" runat="server" />
    </asp:View>
    <asp:View runat="server">
        <p>
            <asp:Literal ID="EmailChangeNotification" runat="server">You have entered a new email
            address:</asp:Literal>
            <strong><asp:Literal ID="EnteredEmailBlurb" runat="server" /></strong>.
            <asp:Literal ID="UsernameChangeWarning" runat="server"> From now on, this email address
            will be your username when you sign in. You will also need to verify the new address.
            Would you still like to update your account?</asp:Literal>
        </p>
        <asp:ValidationSummary ID="ValidationSummary1" CssClass="Warning" DisplayMode="SingleParagraph"
            runat="server" />
        <recaptcha:RecaptchaControl ID="recaptcha" runat="server"
            PublicKey="6Lem2cESAAAAAMsT3bIQXl9WyeQ0mkTXn2tPs2Pt"
            PrivateKey="6Lem2cESAAAAAC7mj31cAVGQRI6r8lhq6cX5ZEK7" />
        <p class="Prompt">
            Please enter your password.
        </p>
        <div class="InputRow">
            <asp:TextBox ID="PasswordField" MaxLength="32" TextMode="Password" runat="server" />
            <asp:RequiredFieldValidator ControlToValidate="PasswordField" Display="None"
                runat="server" ErrorMessage="Please enter your password." />
            <asp:CustomValidator ControlToValidate="PasswordField" Display="None" runat="server"
                OnServerValidate="CheckPassword"
                ErrorMessage="The password that you entered is incorrect." />
        </div>
        <div class="InputRow">
            <asp:Button ID="ConfirmButton" Text="Update my account" runat="server"
                onclick="ConfirmSubmit" />
            <asp:Button ID="BackButton" Text="Go back" runat="server"
                onclick="GoBack" CausesValidation="false" />
        </div>
    </asp:View>
    <asp:View runat="server">
        <p><asp:Literal ID="EmailActivationBlurb" runat="server">Please check your email for a
        confirmation message.</asp:Literal></p>
    </asp:View>
</asp:MultiView>
