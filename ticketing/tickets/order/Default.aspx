<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="tickets.order.Default" %>
<%@ Register TagPrefix="nysf" TagName="SessionHandler" Src="~/usercontrols/SessionHandler.ascx" %>
<%@ Register TagPrefix="nysf" TagName="OrderWizard" Src="~/usercontrols/OrderWizard.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="SessionHandlerPlaceHolder" runat="server">
    <nysf:SessionHandler ID="SessionHandler1" AllowAnonymous="true" runat="server" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="EndOfHeadPlaceHolder" runat="server"
><link rel="stylesheet" href="/js/libs/jQuery/custom-theme/jquery-ui-1.8.14.custom.css"
        type="text/css" />
    <script>
        document.writeln(
            '<link rel="stylesheet" href="/css/NYSFStandardJsOnly.css" type="text/css" />');
        document.writeln(
            '<link rel="stylesheet" href="/css/SyosMap.css" type="text/css" />');
    </script></asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="MainContentPlaceHolder" runat="server"
>    <h2 class="SelfEvident">Reservations</h2>
    <div id="OrderWizard" class="Content">
        <form id="Form1" runat="server">
            <nysf:OrderWizard runat="server" />
        </form>
    </div>
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="EndOfBodyPlaceholder" runat="server"
><script src="//ajax.googleapis.com/ajax/libs/jquery/1.5.1/jquery.js"></script>
    <script src="/js/libs/jQuery/jquery-ui-1.8.14.custom.min.js"></script>
    <script src="/js/libs/Nysf/Apps/OldStyleTickets/OrderWizardSyos.js"></script>
</asp:Content>