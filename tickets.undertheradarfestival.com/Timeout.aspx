<%@ Page Language="C#" MasterPageFile="~/Master.master" AutoEventWireup="true"
		CodeFile="Timeout.aspx.cs" Inherits="Timeout" %>

<asp:Content ContentPlaceHolderID="BodyIdPlaceHolder" Runat="Server">Timeout</asp:Content>

<asp:Content ContentPlaceHolderID="MainContentPlaceHolder" runat="server">
	<h2>Session Timed Out</h2>
	<p>Your browser session timed out. Please choose a menu option to continue.</p>
</asp:Content>