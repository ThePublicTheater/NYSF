<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PerfCalendar.ascx.cs"
    Inherits="Nysf.Apps.OldStyleTickets.PerfCalendar"
%><h3>
    <span class="SelfEvident">Month of </span><asp:Literal ID="MonthYearBlurb"
        runat="server">June 2011</asp:Literal>
</h3>
<div id="CalendarControl">
    <h4 class="SelfEvident">Control</h4>
    <ul>
        <li><asp:HyperLink ID="PrevMonthLink" ToolTip="View previous month"
            runat="server">previous month</asp:HyperLink></li>
        <li><asp:HyperLink ID="NextMonthLink" ToolTip="View next month"
            runat="server">next month</asp:HyperLink></li>
    </ul>
</div>
<aside>
    <h4 class="SelfEvident">Legend</h4>
    <ul>
        <li class="JP"><span class="SelfEvident">JP = </span>Joe's Pub</li>
        <li class="PT"><span class="SelfEvident">PT = </span>The Public Theater</li>
        <li class="SITP"><span class="SelfEvident">SITP = </span>Shakespeare in the Park</li>
    </ul>
</aside>
<div>
    <h4 class="SelfEvident">Calendar</h4>
    <asp:Table ID="Calendar" runat="server">
        <asp:TableHeaderRow runat="server">
            <asp:TableHeaderCell runat="server">Sunday</asp:TableHeaderCell>
            <asp:TableHeaderCell runat="server">Monday</asp:TableHeaderCell>
            <asp:TableHeaderCell runat="server">Tuesday</asp:TableHeaderCell>
            <asp:TableHeaderCell runat="server">Wednesday</asp:TableHeaderCell>
            <asp:TableHeaderCell runat="server">Thursday</asp:TableHeaderCell>
            <asp:TableHeaderCell runat="server">Friday</asp:TableHeaderCell>
            <asp:TableHeaderCell runat="server">Saturday</asp:TableHeaderCell>
        </asp:TableHeaderRow>
    </asp:Table>
</div>