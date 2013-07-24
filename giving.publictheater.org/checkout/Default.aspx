<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Master.Master" %>
<%@ Register TagPrefix="nysf" TagName="SessionHandler" Src="~/UserControls/SessionHandler.ascx" %>
<%@ Register TagPrefix="nysf" TagName="CheckoutForm" Src="~/UserControls/CheckoutForm.ascx" %>

<asp:Content ContentPlaceHolderID="SessionHandlerPlaceHolder" runat="server">
	<nysf:SessionHandler AllowAnonymous="false" RequireSsl="true" SetLastPage="true"
			runat="server" />
</asp:Content>

<asp:Content ContentPlaceHolderID="TitlePlaceHolder" runat="server">
	Checkout - Support the Public
</asp:Content>

<asp:Content ContentPlaceHolderID="BodyIdPlaceHolder" runat="server">Checkout</asp:Content>

<asp:Content ContentPlaceHolderID="MainContentPlaceHolder" runat="server">
	<h2>Checkout</h2>
	<div class="Content">
		<form runat="server">
			<nysf:CheckoutForm EmailTemplateId="108" runat="server" />
		</form>
	</div>
</asp:Content>
