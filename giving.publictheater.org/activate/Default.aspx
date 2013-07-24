<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Master.Master" %>
<%@ Register TagPrefix="nysf" TagName="SessionHandler" Src="~/UserControls/SessionHandler.ascx" %>
<%@ Register TagPrefix="nysf" TagName="ActivateForm" Src="~/UserControls/ActivateForm.ascx" %>

<asp:Content ContentPlaceHolderID="SessionHandlerPlaceHolder" runat="server">
	<nysf:SessionHandler AllowTemporary="true" AllowAnonymous="true" RequireSsl="true"
			SetLastPage="false"	runat="server" />
</asp:Content>

<asp:Content ContentPlaceHolderID="TitlePlaceHolder" runat="server">
	Activate - Support the Public
</asp:Content>

<asp:Content ContentPlaceHolderID="BodyIdPlaceHolder" runat="server">Activate</asp:Content>

<asp:Content ContentPlaceHolderID="MainContentPlaceHolder" runat="server">
	<h2 class="SelfEvident">Reset your password</h2>
	<div class="Content">
		<form runat="server">
			<nysf:ActivateForm runat="server" />
		</form>
	</div>
</asp:Content>
