<%@ Page Title="" Language="C#" MasterPageFile="~/Master.master" AutoEventWireup="true"
		CodeFile="Default.aspx.cs" Inherits="order_Default" %>
<%@ Register TagPrefix="nysf" TagName="SessionHandler" Src="~/usercontrols/SessionHandler.ascx" %>
<%@ Register TagPrefix="nysf" TagName="PackageOrderForm"
		Src="~/usercontrols/PackageOrderForm.ascx" %>

<asp:Content ContentPlaceHolderID="SessionHandlerPlaceHolder" runat="server">
	<nysf:SessionHandler AllowAnonymous="false" runat="server" />
</asp:Content>
<asp:Content ContentPlaceHolderID="BodyIdPlaceHolder" runat="Server">OrderForm</asp:Content>

<asp:Content ContentPlaceHolderID="MainContentPlaceHolder" Runat="server">
	<h2>Order Your Pak</h2>
	<form runat="server">
		<nysf:PackageOrderForm PackageId="169" ZoneId="614" PriceTypeId="119"
				EmailTemplateNumber="104" runat="server" />
	</form>
</asp:Content>
