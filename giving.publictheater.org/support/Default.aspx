<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Master.Master" %>
<%@ Register TagPrefix="nysf" TagName="OrderForm" Src="~/UserControls/SupportThePublicOrderForm.ascx" %>

<asp:Content ContentPlaceHolderID="TitlePlaceHolder" runat="server">
	Support the Public
</asp:Content>

<asp:Content ContentPlaceHolderID="BodyIdPlaceHolder" runat="server">SupportOrder</asp:Content>

<asp:Content ContentPlaceHolderID="BodyOpenTagEndPlaceHolder" runat="server">class="GivingForm"</asp:Content>

<asp:Content ContentPlaceHolderID="MainContentPlaceHolder" runat="server">
	<h2>GIVE A GIFT OF ANY AMOUNT</h2>
	<div class="Content">
		<p>
		Our supporters are a dedicated group of individuals whose contributions help The Public advance our mission of supporting the work of emerging and established artists and providing access to theater for all.
		</p>
		<p>
		Over 75% of The Public’s annual budget comes from the financial support of our generous donors. Their gifts help ensure our financial stability and the development and expansion of our outreach programming, low-cost ticketing initiatives, and free Shakespeare in the Park.
		</p>
		<p>
		<form runat="server">
			<nysf:OrderForm runat="server" />
		</form>
		If you have any questions or if you are having any trouble making your gift online, please contact our Call Center at 212.967.7555. Thank you for your support.
		</p>
	</div>
</asp:Content>
