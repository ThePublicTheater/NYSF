<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="LoginFormNew.ascx.cs" Inherits="tickets.usercontrols.LoginForm" %>
<div id="fb-root"></div>
<script>
    window.fbAsyncInit = function () {
        FB.init({
            appId: '179157098916251', // App ID
            status: true, // check login status
            cookie: true, // enable cookies to allow the server to access the session
            xfbml: true  // parse XFBML
        });

        FB.Event.subscribe('auth.authResponseChange', function (response) {
            if (response.status === 'connected') {
                // the user is logged in and has authenticated your
                // app, and response.authResponse supplies
                // the user's ID, a valid access token, a signed
                // request, and the time the access token 
                // and signed request each expire
                var uid = response.authResponse.userID;
                var accessToken = response.authResponse.accessToken;

                // Do a post to the server to finish the logon
                // This is a form post since we don't want to use AJAX
                var form = document.createElement("form");
                form.setAttribute("method", 'post');
                form.setAttribute("action", '<% =ConfirmURL %>');
                form.setAttribute("id", 'fbForm');
                var field = document.createElement("input");
                field.setAttribute("type", "hidden");
                field.setAttribute("name", 'accessToken');
                field.setAttribute("value", accessToken);
                form.appendChild(field);

                document.body.appendChild(form);


            } else if (response.status === 'not_authorized') {
                // the user is logged in to Facebook, 
                // but has not authenticated your app
            } else {
                // the user isn't logged in to Facebook.
            }
        });
    };
    function afterLogin() {
        document.getElementById("fbForm").submit();
    }

    // Load the SDK Asynchronously
    (function (d) {
        var js, id = 'facebook-jssdk', ref = d.getElementsByTagName('script')[0];
        if (d.getElementById(id)) { return; }
        js = d.createElement('script'); js.id = id; js.async = true;
        js.src = "//connect.facebook.net/en_US/all.js";
        ref.parentNode.insertBefore(js, ref);
    }(document));


</script>
<div id="LoginFormMessages">
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

<h3>Login with your Public account</h3>
<table class="loginTable">
    <tr>
        <td rowspan="2">
            <img src="../media/unmanaged/images/logos/publicLogo_48x102.gif" />
        </td>
        <td><asp:Label ID="EmailAddressLabel" runat="server" Text="User Name/Email Address:"></asp:Label>
            <br />
            <asp:TextBox ID="EmailTextBox" runat="server"></asp:TextBox>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ControlToValidate="EmailTextBox"
                Display="None" ErrorMessage="Please enter an email address."
                runat="server" ValidationGroup="LoginInputs" />
            <asp:RegularExpressionValidator ControlToValidate="EmailTextBox"
                Display="None" ID="EmailAddressValidator"
                ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"
                ErrorMessage="Please enter a valid email address."
                runat="server" ValidationGroup="LoginInputs" />
        </td>
    </tr>
    <tr>
        <td><asp:Label ID="PasswordLabel" runat="server" Text="Password:"></asp:Label>
            <br />
            <asp:TextBox ID="PasswordTextBox" runat="server" TextMode="Password"></asp:TextBox>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" ControlToValidate="PasswordTextBox"
                Display="None" ErrorMessage="Please enter a password."
                runat="server" ValidationGroup="LoginInputs" />
        </td>
    </tr>
    <tr>
        <td colspan="2" style="text-align: center">
            <div class="InputRow">
                <asp:Button ID="SubmitButton" runat="server"
                    OnClick="SubmitButton_Click" Text="Sign In"
                    ValidationGroup="LoginInputs" />
            </div>
        </td>
    </tr>
</table>
<hr />
<h3>Log in with social media.</h3>
<script type="text/javascript">

</script>
<table>
    <tr>
        <td>
            <fb:login-button onlogin="afterLogin();"></fb:login-button>

        </td>
    </tr>
    <tr>
        <td></td>
    </tr>

</table>
