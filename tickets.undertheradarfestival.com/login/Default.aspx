<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Master.master" %>
<%@ Register TagPrefix="nysf" TagName="SessionHandler" Src="~/usercontrols/SessionHandler.ascx" %>
<%@ Register TagPrefix="nysf" TagName="LoginForm" Src="~/usercontrols/LoginForm.ascx" %>

<asp:Content ContentPlaceHolderID="SessionHandlerPlaceHolder" runat="server"
>    <nysf:SessionHandler AllowAuthenticated="false" RequireSecureConnection="true"
runat="server" /></asp:Content>

<asp:Content ContentPlaceHolderID="MainContentPlaceHolder" runat="server"
>    <h2 class="SelfEvident">Sign In</h2>
    <div class="Content">
        <p class="Prompt">Please sign in.</p>
        <form runat="server">
            <nysf:LoginForm runat="server" />
        </form>
        <ul class="LinkMenu">
            <li>
                <a href="/register/" title="Register for a new account">Create a new account</a>
            </li>
            <li>
                <a href="/recover/" title="Look up a lost password">Lost password</a>
            </li>
        </ul>
    </div>
</asp:Content>