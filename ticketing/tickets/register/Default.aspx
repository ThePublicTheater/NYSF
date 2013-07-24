<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/site1.master" %>
<%@ Register TagPrefix="nysf" TagName="SessionHandler" Src="~/usercontrols/SessionHandler.ascx" %>
<%@ Register TagPrefix="nysf" TagName="RegistrationWizard"
Src="~/usercontrols/RegistrationWizard.ascx" %>

<asp:Content ContentPlaceHolderID="SessionHandlerPlaceHolder" runat="server"
>    <nysf:SessionHandler AllowAuthenticated="false" RequireSecureConnection="true"
runat="server" /></asp:Content>

<asp:Content ContentPlaceHolderID="MainContentPlaceHolder" runat="server"
>    <h2>Create a new account</h2>
    <div class="Content">
        <form runat="server">
            <nysf:RegistrationWizard EmailTemplateNumber="92" RegisteringOrg="jp" runat="server" />
        </form>
    </div>
</asp:Content>