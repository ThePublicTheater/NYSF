<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Master.Master" %>

<asp:Content ContentPlaceHolderID="SessionHandlerPlaceHolder" runat="server" />
<asp:Content ContentPlaceHolderID="SessionOptionsPlaceHolder" runat="server" />

<asp:Content ContentPlaceHolderID="TitlePlaceHolder" runat="server">
	Timed Out - Support the Public
</asp:Content>

<asp:Content ContentPlaceHolderID="BodyIdPlaceHolder" runat="server">Timeout</asp:Content>

<asp:Content ContentPlaceHolderID="MainContentPlaceHolder" runat="server">
	<h2>Sorry!</h2>
	<div class="Content">
		<p>Your session has timed out.</p>
		<p><a href="/">Return to homepage</a></p>
	</div>
</asp:Content>