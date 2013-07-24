<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CartViewer.ascx.cs"
    Inherits="tickets.usercontrols.CartViewer"
%><p id="EmptyCartMessage" visible="false" runat="server">Your cart is empty.</p>
<asp:Table ID="CartTable" runat="server">
    <asp:TableHeaderRow runat="server">
        <asp:TableHeaderCell ColumnSpan="4" runat="server">
            Order Details
        </asp:TableHeaderCell>
    </asp:TableHeaderRow>
    <asp:TableHeaderRow runat="server">
        <asp:TableHeaderCell runat="server">Date/Time</asp:TableHeaderCell>
        <asp:TableHeaderCell runat="server">Event/Seating</asp:TableHeaderCell>
        <asp:TableHeaderCell runat="server">Quantity/Unit Price</asp:TableHeaderCell>
        <asp:TableHeaderCell runat="server">Price</asp:TableHeaderCell>
    </asp:TableHeaderRow>
    <asp:TableRow runat="server">
        <asp:TableCell ColumnSpan="3" runat="server">
            Subtotal
        </asp:TableCell>
        <asp:TableCell ID="SubTotalCell" runat="server" />
    </asp:TableRow>
    <asp:TableRow CssClass="TotalRow" runat="server">
        <asp:TableCell ColumnSpan="3" runat="server">
            Grand total
        </asp:TableCell>
        <asp:TableCell ID="TotalCell" runat="server" />
    </asp:TableRow>
</asp:Table>
<p class="Prompt">
	Please review your order carefully, all sales are final – no refunds or exchanges.
</p>
<ul class="LinkMenu">
    <li id="CheckoutLinkLi" runat="server">
        <asp:HyperLink ID="CheckoutLink" Text="Buy tickets" ToolTip="Checkout" runat="server"
        CssClass="CommandLink" />
    </li>
    <li>
        <a href="/" title="Keep shopping" class="CommandLink">Continue shopping</a>
        <%--<asp:HyperLink ID="KeepShoppingLink" Text="Continue shopping" ToolTip="Keep shopping"
            runat="server" CssClass="CommandLink" /> --%>
    </li>
</ul>