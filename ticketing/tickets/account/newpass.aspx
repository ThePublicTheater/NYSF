<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="newpass.aspx.cs" Inherits="tickets.account.newpass" %>
<%@ Register TagPrefix="nysf" TagName="SessionHandler" Src="~/usercontrols/SessionHandler.ascx" %>
<%@ Register TagPrefix="nysf" TagName="PasswordUpdater" Src="~/usercontrols/PasswordUpdater.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="SessionHandlerPlaceHolder" runat="server"
>    <nysf:SessionHandler AllowAnonymous="false" RequireSecureConnection="true"
runat="server" /></asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContentPlaceHolder" runat="server">
    <h2>Change your password</h2>
    <div class="Content">
        <form id="Form1" runat="server">
            <nysf:PasswordUpdater runat="server" />
        </form>
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="EndOfBodyPlaceholder" runat="server">
</asp:Content>
