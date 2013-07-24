<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Master.Master" %>
<%@ Register TagPrefix="nysf" TagName="SessionHandler" Src="~/UserControls/SessionHandler.ascx" %>
<%@ Register TagPrefix="nysf" TagName="RecoverForm" Src="~/UserControls/RecoverForm.ascx" %>

<asp:Content ContentPlaceHolderID="SessionHandlerPlaceHolder" runat="server">
	<nysf:SessionHandler AllowAuthenticated="false" SetLastPage="false" runat="server" />
</asp:Content>

<asp:Content ContentPlaceHolderID="TitlePlaceHolder" runat="server">
	Account Lookup - Support the Public
</asp:Content>

<asp:Content ContentPlaceHolderID="BodyIdPlaceHolder" runat="server">Recover</asp:Content>

<asp:Content ContentPlaceHolderID="MainContentPlaceHolder" runat="server">
	<h2>Find my account</h2>
	<div class="Content">
		<form runat="server">
			<nysf:RecoverForm EmailTemplateId="107" runat="server" />
		</form>
	</div>
</asp:Content>
