<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="tickets.activate.Default" %>
<%@ Register TagPrefix="nysf" TagName="SessionHandler" Src="~/usercontrols/SessionHandler.ascx" %>
<%@ Register TagPrefix="nysf" TagName="AccountActivator"
    Src="~/usercontrols/AccountActivator.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="SessionHandlerPlaceHolder" runat="server"
>    <nysf:SessionHandler ID="SessionHandler1" AllowTemporary="true" RequireSecureConnection="true"
runat="server" /></asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContentPlaceHolder" runat="server"
>    <h2>Activate Your Account</h2>
    <div class="Content">
        <form id="Form1" runat="server">
            <nysf:AccountActivator runat="server" />
        </form>
    </div>
</asp:Content>