<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Master.Master" %>
<%@ Register TagPrefix="nysf" TagName="OrderForm" Src="~/UserControls/PartnersOrderForm.ascx" %>

<asp:Content ContentPlaceHolderID="TitlePlaceHolder" runat="server">
	Partners - Support the Public
</asp:Content>

<asp:Content ContentPlaceHolderID="BodyIdPlaceHolder" runat="server">PartnersOrder</asp:Content>

<asp:Content ContentPlaceHolderID="BodyOpenTagEndPlaceHolder" runat="server">class="GivingForm"</asp:Content>

<asp:Content ContentPlaceHolderID="MainContentPlaceHolder" runat="server">
	<h2>The Public Theater Partners Program</h2>
	<div class="Content">
		<p>
			The Public Theater is deeply grateful for the ongoing partnership of a dedicated group of individuals, whose contributions help ensure The Public’s financial stability and advance our mission of artistic excellence and inclusion.
		</p>
		<p>
			Since over 75% of The Public’s annual budget comes from the support of our generous friends, your gift helps ensure the development and expansion of our educational outreach programming, free and low-cost ticketing initiatives, and Shakespeare in the Park. As a token of our gratitude we offer a variety of benefits to our Partners.
		</p>
		<form runat="server">
			<nysf:OrderForm runat="server" />
		</form>
	</div>
	<footer>
		<p>
			If you have trouble making your gift online or have a question, please contact the Partners Desk at 212-539-8734 or partners@publictheater.org.
		</p>
	</footer>
</asp:Content>
