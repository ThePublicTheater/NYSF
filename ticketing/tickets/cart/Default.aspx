<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="tickets.cart.Default" %>
<%@ Register TagPrefix="nysf" TagName="CartViewer" Src="~/usercontrols/CartViewer.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContentPlaceHolder" runat="server"
>    <h2>Your Cart</h2>
    <div id="CartView" class="Content">
        <form id="Form1" runat="server">

            <nysf:CartViewer runat="server" />
        </form>
    </div>
</asp:Content>