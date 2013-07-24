<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AccountWidget.ascx.cs"
    Inherits="Nysf.UserControls.Demos.AccountWidget"
%><asp:MultiView ID="AccountWidgetViews" runat="server">
    <asp:View runat="server">
        <ul>
            <li>
                <asp:HyperLink ID="LoginLink" runat="server" Text="Sign in" />
            </li>
            <li>
                <asp:HyperLink ID="RegisterLink" runat="server" Text="Register" />
            </li>
            <li id="CartLinkLi" runat="server">
                <asp:HyperLink ID="CartLink" runat="server" Text="View cart" />
            </li>
        </ul>
    </asp:View>
    <asp:View runat="server">
        <p id="Greeting" runat="server">
            <asp:Literal ID="PreNameGreeting" runat="server">Welcome,
                </asp:Literal><asp:Literal ID="NamesLiteral" runat="server" /><asp:Literal
                ID="PostNameGreeting" runat="server">!</asp:Literal>
        </p>
        <ul>
            <li id="CartLinkLi2" runat="server">
                <asp:HyperLink ID="CartLink2" runat="server" Text="View cart" />
            </li>
            <li id="PromoLinkLi" runat="server">
                <asp:HyperLink ID="PromoLink" runat="server" Text="Enter promo code" />
            </li>
            <li>
                <asp:HyperLink ID="ManageLink" runat="server" Text="Manage account" />
            </li>
            <li>
                <asp:HyperLink ID="LogoutLink" runat="server" Text="Sign out" />
            </li>
        </ul>
    </asp:View>
</asp:MultiView>