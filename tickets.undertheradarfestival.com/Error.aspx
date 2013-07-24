<%@ Page Language="C#" MasterPageFile="~/Master.master" AutoEventWireup="true"
		CodeFile="Error.aspx.cs" Inherits="Error" %>

<asp:Content ContentPlaceHolderID="SessionHandlerPlaceHolder" runat="server" />

<asp:Content ContentPlaceHolderID="BodyIdPlaceHolder" runat="server">Error</asp:Content>

<asp:Content ContentPlaceHolderID="MainContentPlaceHolder" runat="server">
	<h2>Oops!</h2>
	<p>
		We're sorry, but an error has occurred on the site. The problem has been logged. Please
		choose a menu option to continue.
	</p>
	<%-- TODO: include support phone number --%>
</asp:Content>