<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/OldStyleTickets.master" %>
<%@ Register TagPrefix="nysf" TagName="SessionHandler" Src="~/usercontrols/SessionHandler.ascx" %>
<%@ Register TagPrefix="nysf" TagName="ManageAccountMenu"
    Src="~/usercontrols/ManageAccountMenu.ascx" %>

<asp:Content ContentPlaceHolderID="SessionHandlerPlaceHolder" runat="server"
>    <nysf:SessionHandler AllowAnonymous="false" RequireSecureConnection="true"
runat="server" /></asp:Content>

<asp:Content ContentPlaceHolderID="MainContentPlaceHolder" runat="server"
>    <h2>Manage your account</h2>
    <div class="Content">
        <nysf:ManageAccountMenu runat="server" />
    </div>
</asp:Content>