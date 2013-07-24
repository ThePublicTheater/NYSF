<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Master.master" %>
<%@ Register TagPrefix="nysf" TagName="SessionHandler" Src="~/usercontrols/SessionHandler.ascx" %>

<%@ Register Src="~/usercontrols/AccountActivator.ascx" TagPrefix="uc1" TagName="AccountActivator" %>
<%@ Register Src="~/usercontrols/AccountActivator.ascx" TagPrefix="nysf" TagName="AccountActivator" %>



<asp:Content ContentPlaceHolderID="SessionHandlerPlaceHolder" runat="server"
>    <nysf:SessionHandler AllowTemporary="true" RequireSecureConnection="true"
runat="server" /></asp:Content>

<asp:Content ContentPlaceHolderID="MainContentPlaceHolder" runat="server">  
      <h2>Activate Your Account</h2>
    <div class="Content">
        <form runat="server">
            <nysf:AccountActivator runat="server" ID="AccountActivator" />
        </form>
    </div>
</asp:Content>