<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Master.master" %>
<%@ Register TagPrefix="nysf" TagName="SessionHandler" Src="~/usercontrols/SessionHandler.ascx" %>
<%@ Register TagPrefix="nysf" TagName="PasswordUpdater"
    Src="~/usercontrols/PasswordUpdater.ascx" %>

<asp:Content ContentPlaceHolderID="SessionHandlerPlaceHolder" runat="server"
>    <nysf:SessionHandler AllowAnonymous="false" RequireSecureConnection="true"
runat="server" /></asp:Content>

<asp:Content ContentPlaceHolderID="MainContentPlaceHolder" runat="server"
>    <h2>Change your password</h2>
    <div class="Content">
        <form runat="server">
            <nysf:PasswordUpdater runat="server" />
        </form>
    </div>
</asp:Content>