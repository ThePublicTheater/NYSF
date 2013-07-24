<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="update.aspx.cs" Inherits="tickets.account.update" %>
<%@ Register TagPrefix="nysf" TagName="SessionHandler" Src="~/usercontrols/SessionHandler.ascx" %>
<%@ Register TagPrefix="nysf" TagName="AccountUpdateForm"    Src="~/usercontrols/AccountUpdateForm.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="SessionHandlerPlaceHolder" runat="server"
>    <nysf:SessionHandler ID="SessionHandler1" AllowAnonymous="false" RequireSecureConnection="true"
runat="server" /></asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContentPlaceHolder" runat="server"
>    <h2>Update your account information</h2>
    <div class="Content">
        <form id="Form1" runat="server">
            <nysf:AccountUpdateForm EmailTemplateNumber="93" runat="server" />
        </form>
    </div>
</asp:Content>