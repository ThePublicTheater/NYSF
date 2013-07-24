<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Master.Master" %>
<%@ Register TagPrefix="nysf" TagName="SessionHandler" Src="~/UserControls/SessionHandler.ascx" %>
<%@ Register TagPrefix="nysf" TagName="SessionTransferHandler" Src="~/UserControls/SessionTransferHandler.ascx" %>

<asp:Content ContentPlaceHolderID="SessionHandlerPlaceHolder" runat="server">
	<nysf:SessionTransferHandler runat="server" />
</asp:Content>

<asp:Content ContentPlaceHolderID="TitlePlaceHolder" runat="server">
	Thank you! - Support the Public
</asp:Content>

<asp:Content ContentPlaceHolderID="BodyIdPlaceHolder" runat="server">PostCheckout</asp:Content>

<asp:Content ContentPlaceHolderID="MainContentPlaceHolder" runat="server">
	<h2>Thank you!</h2>
	<div class="Content">
		<p>
  			Thank you for your generous gift to The Public Theater! You will receive an acknowledgement letter for tax purposes and any additional information within the next week.
		</p>
		<p>
			<strong>Partners Program</strong>: if you have any questions or if you would like to reserve tickets to a production please contact 212.539.8734.
		</p>
		<p>
			<strong>Going Public</strong>: if you have any questions about the renovation please call 212.539.8632.
		</p>
		<p>
			We hope to see you soon at the theater soon!
		</p>
	</div>
</asp:Content>

<asp:Content ContentPlaceHolderID="EndOfBodyPlaceHolder" runat="server">
	<iframe src="https://network.mogointeractive.com/0/18852/universal.html?page_name=conversion&orders=1&revenue=1&mpuid=" HEIGHT=1 WIDTH=1 FRAMEBORDER=0></iframe>
</asp:Content>