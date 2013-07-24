<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Master.master" %>
<%@ Register TagPrefix="nysf" TagName="SessionHandler" Src="~/usercontrols/SessionHandler.ascx" %>
<%@ Register TagPrefix="nysf" TagName="AccountFinder" Src="~/usercontrols/AccountFinder.ascx" %>

<asp:Content ContentPlaceHolderID="SessionHandlerPlaceHolder" runat="server"
>    <nysf:SessionHandler AllowAuthenticated="false" RequireSecureConnection="true"
runat="server" /></asp:Content>

<asp:Content ContentPlaceHolderID="MainContentPlaceHolder" runat="server"
>    <h2 class="SelfEvident">Account Recovery</h2>
    <div class="Content">
        <form runat="server">
            <nysf:AccountFinder EmailTemplateNumber="103" runat="server" />
        </form>
    </div>
</asp:Content>