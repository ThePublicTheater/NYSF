<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AccountWidget.ascx.cs" Inherits="Nysf.Web.UserControls.AccountWidget" %>
<asp:Panel ID="Display_CurrentAccount" runat="server">
	<asp:Literal ID="Blurb_PreUsername" runat="server" /><span class="Username"><asp:Literal ID="Display_Username" runat="server" /></span>
</asp:Panel>
<ul>
	<li id="PromoLink" runat="server">
		<asp:HyperLink ID="Label_PromoLink" Text="Promo code" runat="server" />
	</li>
	<li id="RegisterLink" runat="server">
		<asp:HyperLink ID="Label_RegisterLink" Text="Register" runat="server" />
	</li>
	<li id="LoginLink" runat="server">
		<asp:HyperLink ID="Label_LoginLink" Text="Sign in" runat="server" />
	</li>
	<li id="AccountLink" runat="server">
		<asp:HyperLink ID="Label_AccountLink" Text="Manage account" runat="server" />
	</li>
	<li id="CartLink" runat="server">
		<asp:HyperLink ID="Label_CartLink" Text="View cart" runat="server" />
	</li>
	<li id="CheckoutLink" runat="server">
		<asp:HyperLink ID="Label_CheckoutLink" Text="Checkout" runat="server" />
	</li>
	<li id="LogoutLink" runat="server">
		<asp:HyperLink ID="Label_LogoutLink" Text="Sign out" runat="server" />
	</li>
</ul>