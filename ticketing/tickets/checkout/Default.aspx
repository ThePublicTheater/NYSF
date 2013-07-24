<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="tickets.checkout.Default" %>

<%@ Register TagPrefix="nysf" TagName="SessionHandler" Src="~/usercontrols/SessionHandler.ascx" %>
<%@ Register TagPrefix="nysf" TagName="CheckoutWizard"
    Src="~/usercontrols/CheckoutWizard.ascx" %>
<%@ Register Src="~/usercontrols/CheckoutFormNew.ascx" TagPrefix="nysf" TagName="CheckoutFormNew" %>
<asp:Content ID="head" ContentPlaceHolderID="EndOfHeadPlaceHolder" runat="server">

    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <link rel="/shortcut icon" href="/favicon.ico">
    <script src="../js/libs/jQuery/jquery-1.9.1.js"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            $('.dp').show();
        }); 
    </script>
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="SessionHandlerPlaceHolder" runat="server">
    <nysf:SessionHandler ID="SessionHandler1" AllowAnonymous="true" RequireSecureConnection="true"
        runat="server" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContentPlaceHolder" runat="server">
    <h2>Checkout</h2>
    <div class="Content">
        <noscript><strong>Attention! This page is not correctly displayed, please enable Javascript in your browser or <a href="/cart">click here</a> to continue your purchase.</strong></noscript>
        <form id="Form1" runat="server">
            <%--<nysf:CheckoutWizard ID="CheckoutWizard1" runat="server" />--%>
            <nysf:CheckoutFormNew runat="server" ID="CheckoutFormNew" />
        </form>
        <br />
        <br />
        <br />
        <div id="display" class="dp" style="display:none; font-size:small;">If this page is displayed incorrectly, <a href="/cart">click here</a> to go to the classic page.</div>
    </div>
</asp:Content>
<asp:Content ID="endOfBody" runat="server" ContentPlaceHolderID="EndOfBodyPlaceholder">
</asp:Content>
