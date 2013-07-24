<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="RegistrationWizard.ascx.cs"
    Inherits="Nysf.UserControls.RegistrationWizard" %>
<%@ Register Src="AccountFinder.ascx"
    TagPrefix="pt" TagName="AccountFinder" %>
<%@ Register TagPrefix="recaptcha" Namespace="Recaptcha" Assembly="Recaptcha"
%><asp:ValidationSummary DisplayMode="List" CssClass="Warning" runat="server" />
<asp:MultiView ID="NewWebAccountWizard" ActiveViewIndex="0" runat="server">
    <asp:View runat="server">
        <div class="Prompt">
            <asp:Literal ID="FirstTimePrompt" runat="server">
                <p>
                    Have you ever purchased tickets from The Public Theater or Joe's Pub through the
                    Box Office or by phone?
                </p>
                <p>
                    If so, we might be able to look up your existing account.
                </p>
            </asp:Literal>
        </div>
        <asp:RadioButtonList ID="FirstTimeAnswer" RepeatLayout="UnorderedList"
            runat="server" CssClass="RadioButtonMenu">
            <asp:ListItem Value="n" Selected="True"
                Text="No, this is my first time." />
            <asp:ListItem Value="y"
                Text="Yes I have, but not from the website." />
        </asp:RadioButtonList>
        <asp:RequiredFieldValidator ID="FirstTimeAnswerValidator"
            ControlToValidate="FirstTimeAnswer" Display="None"
            ErrorMessage="Please choose an answer." runat="server" />
        <asp:Panel DefaultButton="FirstTimeNextButton" CssClass="InputRow"
            runat="server">
            <asp:Button ID="FirstTimeNextButton" runat="server" Text="Next" 
                onclick="FirstTimeNextButton_Click" />
        </asp:Panel>
    </asp:View>
    <asp:View runat="server">
        <asp:Panel ID="EmailAlreadyAssociatedErrorPanel" Visible="false"
            EnableViewState="false" runat="server">
            <p class="Warning">
                <asp:Literal ID="EmailAlreadyAssociatedError" runat="server">
                    Your email address already exists in our system.  Please visit our Account
					Lookup page or call the box office at 212-967-7555 for further assistance.
                </asp:Literal>
            </p>
            <ul class="LinkMenu">
				<li>
	                <asp:HyperLink ID="LookupLink" ToolTip="Account Lookup" runat="server"
		                Text="Go to Account Lookup" />
				</li>
            </ul>
        </asp:Panel>
		<p class="Prompt">
			Please enter the information below and click next. If you experience repeated
			difficulties, please call our Box Office at 212-967-7555 for assistance.
		</p>
        <dl class="DetailsEntryList">
            <dt>
                <asp:Label ID="NewAccountGivenNameLabel" runat="server"
                    AssociatedControlID="NewAccountGivenNameTextBox">
                    First name:
                </asp:Label>
            </dt>
            <dd>
                <asp:TextBox ID="NewAccountGivenNameTextBox" runat="server"
                    MaxLength="20"/>
                <asp:RequiredFieldValidator ID="NewAccountGivenNameValidator"
                    ControlToValidate="NewAccountGivenNameTextBox" 
                    ErrorMessage="Please enter your first name."
                    Display="None" runat="server" />
            </dd>
            <dt>
                <asp:Label ID="NewAccountSurnameLabel" runat="server"
                    AssociatedControlID="NewAccountSurnameTextBox">
                    Last name:
                </asp:Label>
            </dt>
            <dd>
                <asp:TextBox ID="NewAccountSurnameTextBox" runat="server"
                    MaxLength="55"/>
                <asp:RequiredFieldValidator ID="NewAccountSurnameValidator"
                    ControlToValidate="NewAccountSurnameTextBox" 
                    ErrorMessage="Please enter your last name."
                    Display="None" runat="server" />
            </dd>
            <dt>
                <asp:Label ID="NewAccountEmailLabel" runat="server"
                    AssociatedControlID="NewAccountEmailTextBox">
                    Email address:
                </asp:Label>
            </dt>
            <dd>
                <asp:TextBox ID="NewAccountEmailTextBox" runat="server"
                    MaxLength="80"/>
                <asp:RequiredFieldValidator
                    ID="NewAccountEmailRequiredValidator"
                    ControlToValidate="NewAccountEmailTextBox" 
                    ErrorMessage="Please enter your email address."
                    Display="None" runat="server" />
                <asp:RegularExpressionValidator
                    ID="NewAccountEmailExpressionValidator"
                    ValidationExpression=
                    "\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"
                    ControlToValidate="NewAccountEmailTextBox"
                    ErrorMessage="Please enter a valid email address."
                    Display="None" runat="server" />
            </dd>
        </dl>
        <recaptcha:RecaptchaControl ID="recaptcha" runat="server"
            PublicKey="6Lem2cESAAAAAMsT3bIQXl9WyeQ0mkTXn2tPs2Pt"
            PrivateKey="6Lem2cESAAAAAC7mj31cAVGQRI6r8lhq6cX5ZEK7" />
        <asp:Panel DefaultButton="NewAccountNextButton" CssClass="InputRow"
            runat="server">
            <asp:Button ID="NewAccountPreviousButton" runat="server"
                Text="Previous" CausesValidation="false" 
                onclick="NewAccountPreviousButton_Click" />
            <asp:Button ID="NewAccountNextButton" runat="server" Text="Next"
                onclick="NewAccountNextButton_Click" />
        </asp:Panel>
    </asp:View>
    <asp:View runat="server">
        <p class="Prompt">
            <asp:Literal ID="ConfirmDetailsPrompt" runat="server">
                You have entered the following:
            </asp:Literal>
        </p>
        <dl class="DetailsViewList">
            <dt>
                <asp:Literal ID="ConfirmDetailsFullNameLabel"
                    runat="server">
                    Your name:
                </asp:Literal>
            </dt>
            <dd>
                <%= NewAccountGivenNameTextBox.Text.Trim() + " " +
                    NewAccountSurnameTextBox.Text.Trim() %>
            </dd>
            <dt>
                <asp:Literal ID="ConfirmDetailsEmailLabel" runat="server">
                    Your email address:
                </asp:Literal>
            </dt>
            <dd>
                <%= NewAccountEmailTextBox.Text.Trim() %>
            </dd>
        </dl>
        <p class="Prompt">
            <asp:Literal ID="ConfirmDetailsFurtherInstructions"
                runat="server">
                If this is correct, click "Next".  Otherwise, please click
                "Previous" to make corrections.
            </asp:Literal>
        </p>
        <asp:Panel DefaultButton="ConfirmDetailsNextButton" CssClass="InputRow"
            runat="server">
            <asp:Button ID="ConfirmDetailsPreviousButton" runat="server"
                Text="Previous" CausesValidation="false" 
                onclick="ConfirmDetailsPreviousButton_Click" />
            <asp:Button ID="ConfirmDetailsNextButton" runat="server"
                Text="Next" onclick="ConfirmDetailsNextButton_Click" />
        </asp:Panel>
    </asp:View>
    <asp:View runat="server">
        <p>
            <asp:Literal ID="CheckEmailPrompt" runat="server">
                Please check your email for a verification message containing a link to activate
				your account. This message may take a few minutes to reach you.  Follow the
				instructions in the message to confirm your email address. We look forward to
				seeing you at the show!
            </asp:Literal>
        </p>
    </asp:View>
</asp:MultiView>