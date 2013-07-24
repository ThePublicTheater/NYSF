<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="tickets.Default" %>

<%@ Register Src="~/usercontrols/SessionHandler.ascx" TagPrefix="uc1" TagName="SessionHandler" %>
<%@ Register Src="~/usercontrols/PerfCalendar.ascx" TagPrefix="uc1" TagName="PerfCalendar" %>


<asp:Content ID="Content1" ContentPlaceHolderID="SessionHandlerPlaceHolder" runat="server">
    <uc1:SessionHandler runat="server" ID="SessionHandler" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="EndOfHeadPlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContentPlaceHolder" runat="server">
    <h2 class="SelfEvident">Performances Calendar</h2>
    <div class="Content">
    <uc1:PerfCalendar runat="server" ID="PerfCalendar" />
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="EndOfBodyPlaceholder" runat="server">
</asp:Content>
 