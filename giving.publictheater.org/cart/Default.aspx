<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Master.Master" %>
<%@ Register TagPrefix="nysf" TagName="SessionHandler" Src="~/UserControls/SessionHandler.ascx" %>
<%@ Register TagPrefix="nysf" TagName="CartForm" Src="~/UserControls/CartForm.ascx" %>

<asp:Content ContentPlaceHolderID="TitlePlaceHolder" runat="server">
	Login - Support the Public
</asp:Content>

<asp:Content ContentPlaceHolderID="BodyIdPlaceHolder" runat="server">Cart</asp:Content>

<asp:Content ContentPlaceHolderID="MainContentPlaceHolder" runat="server">
	<h2>Your cart</h2>
	<div class="Content">
		<form runat="server">
			<nysf:CartForm runat="server" />
		</form>
	</div>
</asp:Content>

<asp:Content ContentPlaceHolderId="EndOfBodyPlaceHolder" runat="server">
<iframe src="https://network.mogointeractive.com/0/18852/universal.html?page_name=rt_cart&cart=1&mpuid=" HEIGHT=1 WIDTH=1 FRAMEBORDER=0></iframe>
</asp:Content>