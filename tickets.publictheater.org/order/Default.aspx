<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/OldStyleTickets.master" %>
<%@ Register TagPrefix="nysf" TagName="SessionHandler" Src="~/usercontrols/SessionHandler.ascx" %>
<%@ Register TagPrefix="nysf" TagName="OrderWizard" Src="~/usercontrols/OrderWizard.ascx" %>

<asp:Content ContentPlaceHolderID="SessionHandlerPlaceHolder" runat="server">
    <nysf:SessionHandler AllowAnonymous="true" runat="server" />
</asp:Content>

<asp:Content ContentPlaceHolderID="EndOfHeadPlaceHolder" runat="server"
><link rel="stylesheet" href="/js/libs/jQuery/custom-theme/jquery-ui-1.8.14.custom.css"
        type="text/css" />
    <script>
        document.writeln(
            '<link rel="stylesheet" href="/css/NYSFStandardJsOnly.css" type="text/css" />');
        document.writeln(
            '<link rel="stylesheet" href="/css/SyosMap.css" type="text/css" />');
    </script></asp:Content>

<asp:Content ContentPlaceHolderID="MainContentPlaceHolder" runat="server"
>    <h2 class="SelfEvident">Reservations</h2>
    <div id="OrderWizard" class="Content">
        <form runat="server">
            <nysf:OrderWizard runat="server" />
        </form>
    </div>
</asp:Content>

<asp:Content ContentPlaceHolderID="EndOfBodyPlaceholder" runat="server"
><script src="//ajax.googleapis.com/ajax/libs/jquery/1.5.1/jquery.js"></script>
    <script src="/js/libs/jQuery/jquery-ui-1.8.14.custom.min.js"></script>
    <script src="/js/libs/Nysf/Apps/OldStyleTickets/OrderWizardSyos.js"></script>
</asp:Content>