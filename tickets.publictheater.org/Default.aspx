<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/OldStyleTickets.master" %>

<%@ Register Src="~/usercontrols/PerfCalendar.ascx" TagPrefix="uc1" TagName="PerfCalendar" %>


<asp:Content ID="Content1" ContentPlaceHolderID="MainContentPlaceHolder" runat="server">
    <h2 class="SelfEvident">Performances Calendar</h2>
    <div class="Content">
        <uc1:PerfCalendar runat="server" id="PerfCalendar" />
    </div>
</asp:Content>