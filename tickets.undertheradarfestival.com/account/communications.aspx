<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Master.master" %>
<%@ Register TagPrefix="nysf" TagName="SessionHandler" Src="~/usercontrols/SessionHandler.ascx" %>
<%@ Register TagPrefix="nysf" TagName="CommPrefsForm"
    Src="~/usercontrols/CommPrefsForm.ascx" %>

<asp:Content ContentPlaceHolderID="SessionHandlerPlaceHolder" runat="server"
>    <nysf:SessionHandler AllowAnonymous="false" RequireSecureConnection="true"
runat="server" /></asp:Content>

<asp:Content ContentPlaceHolderID="MainContentPlaceHolder" runat="server"
>    <h2>Communication Preferences</h2>
    <div id="CommPrefs" class="Content">
        <form runat="server">
            <nysf:CommPrefsForm runat="server" />
        </form>
    </div>
</asp:Content>