<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="tickets.account.Default" %>

<%@ Register Src="~/usercontrols/ManageAccountMenu.ascx" TagPrefix="uc1" TagName="ManageAccountMenu" %>
<%@ Register Src="~/usercontrols/SessionHandler.ascx" TagPrefix="uc1" TagName="SessionHandler" %>


<asp:Content ID="Content1" ContentPlaceHolderID="SessionHandlerPlaceHolder" runat="server">
    <uc1:SessionHandler runat="server" ID="SessionHandler" AllowAnonymous="false" RequireSecureConnection="true" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="EndOfHeadPlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContentPlaceHolder" runat="server">
    <h2>Manage your account</h2>
    <div class="Content">
        <uc1:ManageAccountMenu runat="server" ID="ManageAccountMenu" />
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="EndOfBodyPlaceholder" runat="server">
</asp:Content>
