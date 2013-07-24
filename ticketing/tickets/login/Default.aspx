<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="tickets.login.Default" %>

<%@ Register Src="~/usercontrols/SessionHandler.ascx" TagPrefix="uc1" TagName="SessionHandler" %>
<%@ Register Src="~/usercontrols/LoginFormNew.ascx" TagPrefix="uc1" TagName="LoginFormNew" %>
<%@ Register Src="~/usercontrols/LoginForm.ascx" TagPrefix="uc1" TagName="LoginForm" %>



<asp:Content ID="Content1" ContentPlaceHolderID="SessionHandlerPlaceHolder" runat="server">
    <uc1:SessionHandler runat="server" ID="SessionHandler" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="EndOfHeadPlaceHolder" runat="server">

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContentPlaceHolder" runat="server">
 
<div class="Content">
        <p class="Prompt">Please sign in.</p>
        <form id="Form1" runat="server">
            <uc1:LoginForm ID="loginForm" runat="server" />
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
    <asp:Content ID="Content4" ContentPlaceHolderID="EndOfBodyPlaceholder" runat="server">
</asp:Content>
