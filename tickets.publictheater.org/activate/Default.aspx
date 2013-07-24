<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/OldStyleTickets.master" %>
<%@ Register TagPrefix="nysf" TagName="SessionHandler" Src="~/usercontrols/SessionHandler.ascx" %>
<%@ Register TagPrefix="nysf" TagName="AccountActivator"
    Src="~/usercontrols/AccountActivator.ascx" %>

<asp:Content ContentPlaceHolderID="SessionHandlerPlaceHolder" runat="server"
>    <nysf:SessionHandler AllowTemporary="true" RequireSecureConnection="true"
runat="server" /></asp:Content>

<asp:Content ContentPlaceHolderID="MainContentPlaceHolder" runat="server"
>    <h2>Activate Your Account</h2>
    <div class="Content">
        <form runat="server">
            <nysf:AccountActivator runat="server" />
        </form>
    </div>
</asp:Content>