<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Master.Master" %>
<%@ Register TagPrefix="nysf" TagName="LogoutHandler" Src="~/UserControls/LogoutHandler.ascx" %>

<asp:Content ContentPlaceHolderID="SessionHandlerPlaceHolder" runat="server">
	<nysf:LogoutHandler runat="server" />
</asp:Content>

<asp:Content ContentPlaceHolderID="SessionOptionsPlaceHolder" runat="server" />

<asp:Content ContentPlaceHolderID="TitlePlaceHolder" runat="server">
	Signed Out - Support the Public
</asp:Content>

<asp:Content ContentPlaceHolderID="BodyIdPlaceHolder" runat="server">Logout</asp:Content>

<asp:Content ContentPlaceHolderID="MainContentPlaceHolder" runat="server">
	<h2>Goodbye!</h2>
	<div class="Content">
		<p>You have been signed out.</p>
		<p><a href="http://publictheater.org">Return to our homepage</a></p>
	</div>
</asp:Content>